using System.Collections.Generic;
using System.Linq;

namespace mst_boredom_remover.engine
{
    class Unit
    {
        public Engine engine; // This is probably a bad idea
        // But it sure makes things a whole lot easier...

        public int id;
        public UnitType type;
        public double health;
        public Position position;
        public Position previousPosition;
        public int nextMove; //This is the tick that the unit gets its next move

        public enum Status
        {
            Idle,
            FightMoving,
            Moving,
            Hiding,
            Attacking,
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

        public Unit(Engine engine, UnitType unitType, Position position, Player owner)
        {
            // TODO: Set id
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
        }

        public bool CanMove(Position targetPosition)
        {
            return (engine.map.Inside(targetPosition) && engine.unitGrid[targetPosition.x, targetPosition.y] == null);
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
        public void NextOrder()
        {
            orders.RemoveAt(0);
        }

        public void Update()
        {
            if (orders.Count == 0)
            {
                return;
            }
            var currentOrder = orders.First();

            switch (currentOrder.orderType)
            {
                case Order.OrderType.Move:
                    if (position.Equals(currentOrder.targetPosition))
                    {
                        status = Status.Idle;
                        NextOrder();
                        Update();
                        break;
                    }
                    Position nextPosition = Pathfinder.FindNextStep(engine, this, position, currentOrder.targetPosition);

                    if (nextPosition != null)
                    {
                        engine.MoveUnit(this, nextPosition);
                    }
                    status = Status.Moving;
                    // TODO: Calculate cooldown based on speed and tile and modifiers
                    engine.ScheduleUpdate(10, this);
                    break;
                case Order.OrderType.Attack:
                    if (currentOrder.targetUnit.status == Status.Dead)
                    {
                        status = Status.Idle;
                        NextOrder();
                        Update();
                        break;
                    }
                    currentOrder.targetPosition = currentOrder.targetUnit.position;
                    if (position.Distance(currentOrder.targetUnit.position) > type.attackRange)
                    {
                        // TODO: Move into range

                        nextPosition = Pathfinder.FindNextStep(engine, this, position, currentOrder.targetPosition);

                        if (nextPosition != null)
                        {
                            engine.MoveUnit(this, nextPosition);
                        }

                        // TODO: Calculate cooldown based on speed and tile and modifiers
                        engine.ScheduleUpdate(10, this);
                        break;
                    }
                    // TODO: Attack
                    engine.Attack(this, currentOrder.targetUnit);
                    // TODO: Calculate cooldown based on attack cooldown and modifiers
                    engine.ScheduleUpdate(10, this);
                    break;
                case Order.OrderType.Produce:
                    if (owner.gold < currentOrder.unitTypeBuild.goldCost ||
                        owner.iron < currentOrder.unitTypeBuild.ironCost ||
                        owner.manaCystals < currentOrder.unitTypeBuild.manaCrystalsCost)
                    {
                        // TODO: Decide what happens here
                        // Wait a certain amount of time and check again
                        // or just skip to next order
                        engine.ScheduleUpdate(5, this);
                        break;
                    }
                    // Find place to put unit
                    Position targetLocation = position;
                    Position producePosition = null;
                    foreach (var testPosition in engine.map.BreadthFirst(targetLocation, distance: 1))
                    {
                        if (engine.unitGrid[testPosition.x, testPosition.y] == null)
                        {
                            producePosition = testPosition;
                            break;
                        }
                    }
                    if (producePosition == null)
                    {
                        // Try again in the future
                        engine.ScheduleUpdate(5, this);
                        break;
                    }
                    // Subtract resources
                    owner.gold -= currentOrder.unitTypeBuild.goldCost;
                    owner.iron -= currentOrder.unitTypeBuild.ironCost;
                    owner.manaCystals -= currentOrder.unitTypeBuild.manaCrystalsCost;
                    // Create the unit
                    // TODO: Apply orders to the new unit, such as a rally point
                    engine.AddUnit(new Unit(engine, currentOrder.unitTypeBuild, producePosition, owner));
                    NextOrder();
                    break;
                case Order.OrderType.Gather:
                    if ( this.position != current_order.target_position )
                    {
                        // TODO: Move onto resource

                        next_position = Pathfinder.findNextStep(engine, this, position, current_order.target_position);

                        if (next_position != null)
                        {
                            engine.MoveUnit(this, next_position);
                        }

                        // TODO: Calculate cooldown based on speed and tile and modifiers
                        engine.ScheduleUpdate(10, this);
                        break;
                    }
                    // TODO: / ASSUPTION We have infinate resources, is that okay?
                    var tileResoure = engine.map.tiles[current_order.target_position.x, current_order.target_position.y].tile_type.resource_type;
                    if ( tileResoure == TileType.ResourceType.Gold)
                    {
                        this.owner.gold += this.type.gather_rate;
                    }
                    else if ( tileResoure == TileType.ResourceType.Iron )
                    {
                        this.owner.iron += this.type.gather_rate;
                    }
                    else if ( tileResoure == TileType.ResourceType.ManaCrystals )
                    {
                        this.owner.iron += this.type.gather_rate;
                    }
                    engine.ScheduleUpdate(10, this);
                    if ( orders.Count > 1 )
                    {
                        NextOrder();
                    }
                    break;
            }
        }
    }
}
