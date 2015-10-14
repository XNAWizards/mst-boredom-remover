using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;

namespace mst_boredom_remover
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

        public Unit(Engine engine, UnitType unit_type, Position position, Player owner)
        {
            // TODO: Set id
            this.engine = engine;
            this.type = unit_type;
            this.position = position;
            this.previousPosition = position;
            this.nextMove = -1;
            this.owner = owner;

            health = type.max_health;
            status = Status.Idle;
            orders = new List<Order>();
            modifiers = new List<UnitModifier>();

            animationStartTick = 0;
        }

        public bool CanMove(Position position)
        {
            return (engine.map.Inside(position) && engine.unitGrid[position.x, position.y] == null);
        }

        public int AttackRange()
        {
            //TODO: make this account for modifiers and decide how to go from double to int, floor always?
            return (int) type.attack_range;
        }

        public int AttackStrength()
        {
            //TODO: make this account for modifiers
            return (int)type.attack_strength;
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

            switch (currentOrder.order_type)
            {
                case Order.OrderType.Move:
                    if (position.Equals(currentOrder.target_position))
                    {
                        status = Status.Idle;
                        NextOrder();
                        Update();
                        break;
                    }
                    Position nextPosition = Pathfinder.findNextStep(engine, this, position, currentOrder.target_position);

                    if (nextPosition != null)
                    {
                        engine.MoveUnit(this, nextPosition);
                    }
                    status = Status.Moving;
                    // TODO: Calculate cooldown based on speed and tile and modifiers
                    engine.ScheduleUpdate(10, this);
                    break;
                case Order.OrderType.Attack:
                    if (currentOrder.target_unit.status == Status.Dead)
                    {
                        status = Status.Idle;
                        NextOrder();
                        Update();
                        break;
                    }
                    currentOrder.target_position = currentOrder.target_unit.position;
                    if (position.Distance(currentOrder.target_unit.position) > type.attack_range)
                    {
                        // TODO: Move into range

                        nextPosition = Pathfinder.findNextStep(engine, this, position, currentOrder.target_position);

                        if (nextPosition != null)
                        {
                            engine.MoveUnit(this, nextPosition);
                        }

                        // TODO: Calculate cooldown based on speed and tile and modifiers
                        engine.ScheduleUpdate(10, this);
                        break;
                    }
                    // TODO: Attack
                    engine.Attack(this, currentOrder.target_unit);
                    // TODO: Calculate cooldown based on attack cooldown and modifiers
                    engine.ScheduleUpdate(10, this);
                    break;
                case Order.OrderType.Produce:
                    if (owner.gold < currentOrder.unit_type_build.gold_cost ||
                        owner.iron < currentOrder.unit_type_build.iron_cost ||
                        owner.mana_cystals < currentOrder.unit_type_build.mana_crystals_cost)
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
                    foreach (var test_position in engine.map.BreadthFirst(targetLocation, distance: 1))
                    {
                        if (engine.unitGrid[test_position.x, test_position.y] == null)
                        {
                            producePosition = test_position;
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
                    owner.gold -= currentOrder.unit_type_build.gold_cost;
                    owner.iron -= currentOrder.unit_type_build.iron_cost;
                    owner.mana_cystals -= currentOrder.unit_type_build.mana_crystals_cost;
                    // Create the unit
                    // TODO: Apply orders to the new unit, such as a rally point
                    engine.AddUnit(new Unit(engine, currentOrder.unit_type_build, producePosition, owner));
                    NextOrder();
                    break;
            }
        }
    }
}
