﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace mst_boredom_remover.engine
{
    class Unit
    {
        public Engine engine; // This is probably a bad idea
        // But it sure makes things a whole lot easier...

        public readonly int id;
        public readonly UnitType type;
        public double health;
        public Position position;
        public Position previousPosition;
        public int nextMove; //This is the tick that the unit gets its next move
        public bool selected;

        public enum Status
        {
            Idle,
            FightMoving,
            Moving,
            Hiding,
            Attacking,
            Producing,
            Dead
        };
        public Status status;
        public Player owner;
        public List<Order> orders;

        public enum UnitModifier
        {
            Slowed,
            Frozen,
            Disarmed,
            Poisoned,
            Burning,
            Enraged,
            Blessed
        };
        public List<UnitModifier> modifiers;

        public int animationStartTick; // This tells us on which tick an animation was started
        public int direction;

        private List<Position> currentPath;
        private Position currentTargetPosition;

        private const int moveRetryCooldown = 10;
        private const int buildRetryCooldown = 10;
        private const int buildCooldown = 10;
        private const int gatherCooldown = 10;

        public Unit(int id, Engine engine, UnitType unitType, Position position, Player owner)
        {
            this.id = id;
            this.engine = engine;
            this.type = unitType;
            this.position = position;
            this.previousPosition = position;
            this.nextMove = -1;
            this.owner = owner;

            health = type.maxHealth;
            status = Status.Idle;
            orders = new List<Order>();
            modifiers = new List<UnitModifier>();

            animationStartTick = 0;
            direction = 0;

            currentPath = null;
            currentTargetPosition = null;
        }

        public bool CanMove(Position targetPosition)
        {
            if (CanMove() && engine.map.Inside(targetPosition))
            {
                var targetUnit = engine.GetUnitAt(targetPosition);
                if (targetUnit != null)
                {
                    return targetUnit.owner == owner && targetUnit.CanMove() && targetUnit.orders.Count == 0;
                }
                return true;
            }
            return false;
        }

        public bool CanMove()
        {
            return type.actions.Contains(UnitType.Action.Move);
        }

        public bool CanAttack()
        {
            return type.actions.Contains(UnitType.Action.Attack);
        }

        public bool CanProduce()
        {
            return type.actions.Contains(UnitType.Action.Produce);
        }

        public bool CanGather()
        {
            return type.actions.Contains(UnitType.Action.Gather);
        }

        public bool CanBuild()
        {
            return type.actions.Contains(UnitType.Action.Build);
        }

        public bool CanCast()
        {
            return type.actions.Contains(UnitType.Action.Cast);
        }

        public int AttackRange()
        {
            //TODO: make this account for modifiers and decide how to go from double to int, floor always?
            return (int) type.attackRange;
        }

        public int AttackStrength()
        {
            //TODO: make this account for modifiers
            return (int)type.attackStrength;
        }

        public int Defense()
        {
            //TODO: make this account for modifiers
            return (int)type.defense;
        }

        public int GetMoveCooldown(Position startPosition, Position endPosition)
        {
            // TODO: Account for tile effects, unit modifiers, etc.
            int nominalSpeed = (int)(engine.map.GetTileAt(endPosition).tileType.movementCost * 10.0 / type.movementSpeed);
            return nominalSpeed > 0 ? nominalSpeed : 1;
        }

        public int GetAttackCooldown()
        {
            // TODO: Account for modifiers
            return (int) (10.0/type.attackSpeed);
        }

        public Vector2 GetAnimatedPosition()
        {
            if (status == Status.Moving)
            {
                // LLLLLEEEEERRRRRRP
                return Vector2.Lerp(previousPosition.ToVector2(),
                    position.ToVector2(),
                    (engine.currentTick - animationStartTick)/(float)GetMoveCooldown(previousPosition, position));
            }
            return position.ToVector2();
        }

        public void Update()
        {
            if (status == Status.Dead) // In case this unit got killed on this tick
                return; // Otherwise this unit might unset it's DEAD state and be attacked twice

            if (orders.Count == 0)
            {
                status = Status.Idle;
                return;
            }
            var currentOrder = orders.First();

            switch (currentOrder.orderType)
            {
                case Order.OrderType.Move:
                    MoveOrder(currentOrder);
                    break;
                case Order.OrderType.Attack:
                    AttackOrder(currentOrder);
                    break;
                case Order.OrderType.Produce:
                    ProduceOrder(currentOrder);
                    break;
                case Order.OrderType.Gather:
                    GatherOrder(currentOrder);
                    break;
            }
        }

        // Private

        private int CalculateDirection(Position delta)
        {
            bool bottomRight = delta.x > -delta.y;
            bool topRight = delta.x > delta.y;
            if (bottomRight && topRight) return 0;
            if (bottomRight) return 1;
            if (!topRight) return 2;
            return 3;
            // return 1 - delta.x + (delta.y == -1 ? 2 : 0);
        }

        private void NextOrder()
        {
            orders.RemoveAt(0);
        }

        private void MoveTowards(Position targetPosition)
        {
            if (currentPath == null || currentPath.Count == 0 ||
                !targetPosition.Equals(currentTargetPosition) || !CanMove(currentPath.First()) ||
                (position.Distance(targetPosition) % 11 == 0)) // Every 11 steps make sure that a new best path hasn't magically appeared
            {
                // Try to create a path if the current one is invalid
                currentPath = Pathfinder.FindPath(engine, this, engine.map.GetTileAt(position), engine.map.GetTileAt(targetPosition));
                currentTargetPosition = targetPosition;
                // Remove the first element because we are already there
                if (currentPath != null)
                    currentPath.RemoveAt(0);
            }
            if (currentPath != null && currentPath.Count > 0 && currentTargetPosition.Equals(targetPosition) && CanMove(currentPath.First()))
            {
                Position nextStep = currentPath.First();
                currentPath.RemoveAt(0);
                TryToMove(nextStep);
            }
            else
            {
                status = Status.Idle;
                // Retry after moveRetryCooldown ticks
                engine.ScheduleUpdate(moveRetryCooldown, this);
            }
        }

        private void TryToMove(Position targetPosition)
        {
            if (targetPosition != null)
            {
                status = Status.Moving;
                direction = CalculateDirection(targetPosition - position);
                engine.ScheduleUpdate(GetMoveCooldown(position, targetPosition), this);

                var blockingUnit = engine.GetUnitAt(targetPosition);
                if (blockingUnit != null)
                {
                    engine.SwapUnits(this, blockingUnit);
                }
                else
                {
                    engine.MoveUnit(this, targetPosition);
                }
            }
            else
            {
                status = Status.Idle;
                // Retry after moveRetryCooldown ticks
                engine.ScheduleUpdate(moveRetryCooldown, this);
            }
        }

        private void MoveOrder(Order order)
        {
            // Check for goal reached
            if (position.Equals(order.targetPosition))
            {
                NextOrder();
                Update();
                return;
            }
            MoveTowards(order.targetPosition);
        }

        private void AttackOrder(Order order)
        {
            // Check for goal reached
            if (order.targetUnit.status == Status.Dead)
            {
                NextOrder();
                Update();
                return;
            }

            var targetPosition = order.targetUnit.position;
            if (position.Distance(targetPosition) > type.attackRange)
            {
                // Move into range
                MoveTowards(targetPosition);
                return;
            }

            status = Status.Attacking;
            engine.Attack(this, order.targetUnit);
            direction = CalculateDirection(targetPosition - position);
            engine.ScheduleUpdate(GetAttackCooldown(), this);
        }

        private void ProduceOrder(Order order)
        {
            // If the unit is told to build a structure far away, and he is not in range yet
            if (CanMove() && order.targetPosition != null && order.targetPosition.Distance(position) > 1)
            {
                MoveTowards(order.targetPosition);
                return;
            }

            status = Status.Producing;
            // If the player does not has enough moneyz
            if (owner.gold < order.unitTypeBuild.goldCost ||
                owner.iron < order.unitTypeBuild.ironCost ||
                owner.manaCystals < order.unitTypeBuild.manaCrystalsCost)
            {
                // Try again later
                engine.ScheduleUpdate(buildRetryCooldown, this);
                return;
            }

            // Find place to put unit
            Position targetLocation = position;
            Position producePosition = null;
            if (order.targetPosition == null)
            {
                foreach (var testPosition in Pathfinder.BreadthFirst(engine, targetLocation, distance: 1))
                {
                    if (engine.GetUnitAt(testPosition) == null)
                    {
                        producePosition = testPosition;
                        break;
                    }
                }
            }
            else
            {
                producePosition = order.targetPosition;
            }
            if (producePosition == null)
            {
                // Try again in the future
                engine.ScheduleUpdate(buildRetryCooldown, this);
                return;
            }

            // Subtract resources
            owner.gold -= order.unitTypeBuild.goldCost;
            owner.iron -= order.unitTypeBuild.ironCost;
            owner.manaCystals -= order.unitTypeBuild.manaCrystalsCost;

            // Create the unit
            Unit u = engine.AddUnit(order.unitTypeBuild, producePosition, owner);
            if (order.targetPosition != null && u.CanMove())
            {
                // Tell the produced unit to go to the rally point
                engine.OrderMove(u, order.targetPosition);
            }
            else if (u.type.name.Equals("GoldMine"))
            {
                engine.OrderGather(u, u.position);
            }

            NextOrder();
            engine.ScheduleUpdate(buildCooldown, this);
        }

        private void GatherOrder(Order order)
        {
            if (!position.Equals(order.targetPosition))
            {
                // Move to resource
                MoveTowards(order.targetPosition);
                return;
            }
            status = Status.Producing;
            // TODO: / ASSUPTION We have infinate resources, is that okay?
            var tileResoure = engine.map.tiles[order.targetPosition.x, order.targetPosition.y].tileType.resourceType;
            if (tileResoure == TileType.ResourceType.Gold)
            {
                owner.gold += type.gatherRate;
            }
            else if (tileResoure == TileType.ResourceType.Iron)
            {
                owner.iron += type.gatherRate;
            }
            else if (tileResoure == TileType.ResourceType.ManaCrystals)
            {
                owner.manaCystals += type.gatherRate;
            }
            engine.ScheduleUpdate(gatherCooldown, this);
            if (orders.Count > 1)
            {
                NextOrder();
            }
        }
    }
}
