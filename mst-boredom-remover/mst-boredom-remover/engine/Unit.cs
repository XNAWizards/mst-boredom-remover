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
        public Position previous_position;
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

        public int animation_start_tick; // This tells us on which tick an animation was started

        public Unit(Engine engine, UnitType unit_type, Position position, Player owner)
        {
            // TODO: Set id
            this.engine = engine;
            this.type = unit_type;
            this.position = position;
            this.previous_position = position;
            this.nextMove = -1;
            this.owner = owner;

            health = type.max_health;
            status = Status.Idle;
            orders = new List<Order>();
            modifiers = new List<UnitModifier>();

            animation_start_tick = 0;
        }

        public bool CanMove(Position position)
        {
            return (engine.map.Inside(position) && engine.unit_grid[position.x, position.y] == null);
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
            var current_order = orders.First();

            switch (current_order.order_type)
            {
                case Order.OrderType.Move:
                    if (position.Equals(current_order.target_position))
                    {
                        status = Status.Idle;
                        NextOrder();
                        Update();
                        break;
                    }
                    Position next_position = Pathfinder.findNextStep(engine, this, position, current_order.target_position);

                    if (next_position != null)
                    {
                        engine.MoveUnit(this, next_position);
                    }
                    status = Status.Moving;
                    // TODO: Calculate cooldown based on speed and tile and modifiers
                    engine.ScheduleUpdate(10, this);
                    break;
                case Order.OrderType.Attack:
                    if (current_order.target_unit.status == Status.Dead)
                    {
                        status = Status.Idle;
                        NextOrder();
                        Update();
                        break;
                    }
                    current_order.target_position = current_order.target_unit.position;
                    if (position.Distance(current_order.target_unit.position) > type.attack_range)
                    {
                        // TODO: Move into range

                        next_position = Pathfinder.findNextStep(engine, this, position, current_order.target_position);

                        if (next_position != null)
                        {
                            engine.MoveUnit(this, next_position);
                        }

                        // TODO: Calculate cooldown based on speed and tile and modifiers
                        engine.ScheduleUpdate(10, this);
                        break;
                    }
                    // TODO: Attack
                    engine.Attack(this, current_order.target_unit);
                    // TODO: Calculate cooldown based on attack cooldown and modifiers
                    engine.ScheduleUpdate(10, this);
                    break;
                case Order.OrderType.Produce:
                    if (owner.gold < current_order.unit_type_build.gold_cost ||
                        owner.iron < current_order.unit_type_build.iron_cost ||
                        owner.mana_cystals < current_order.unit_type_build.mana_crystals_cost)
                    {
                        // TODO: Decide what happens here
                        // Wait a certain amount of time and check again
                        // or just skip to next order
                        engine.ScheduleUpdate(5, this);
                        break;
                    }
                    // Find place to put unit
                    Position target_location = position;
                    Position produce_position = null;
                    foreach (var test_position in engine.map.BreadthFirst(target_location, distance: 1))
                    {
                        if (engine.unit_grid[test_position.x, test_position.y] == null)
                        {
                            produce_position = test_position;
                            break;
                        }
                    }
                    if (produce_position == null)
                    {
                        // Try again in the future
                        engine.ScheduleUpdate(5, this);
                        break;
                    }
                    // Subtract resources
                    owner.gold -= current_order.unit_type_build.gold_cost;
                    owner.iron -= current_order.unit_type_build.iron_cost;
                    owner.mana_cystals -= current_order.unit_type_build.mana_crystals_cost;
                    // Create the unit
                    // TODO: Apply orders to the new unit, such as a rally point
                    engine.AddUnit(new Unit(engine, current_order.unit_type_build, produce_position, owner));
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
