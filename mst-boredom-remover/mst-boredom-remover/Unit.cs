﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;

namespace mst_boredom_remover
{
    class Unit
    {
        public int id;
        public UnitType type;
        public double health;
        public Position position;

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

        public Order DefaultOrder = new Order
        {
            order_type = Order.OrderType.Idle,
            target_position = null,
            target_unit = null
        };

        public Unit(UnitType unit_type, Position position, Player owner)
        {
            // TODO: Set id
            this.type = unit_type;
            this.position = position;
            this.owner = owner;

            health = type.max_health;
            status = Status.Idle;
            orders = new List<Order>();
            modifiers = new List<UnitModifier>();
        }

        public bool CanMove(Position p)
        {
            return true;
        }

        public void NextOrder()
        {
            orders.RemoveAt(0);
        }

        public void Update(Game game)
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
                        Update(game);
                        break;
                    }
                    Position next_position = Pathfinder.findNextStep(this, position, current_order.target_position);

                    game.MoveUnit(this, next_position);
                    status = Status.Moving;
                    // TODO: Calculate cooldown based on speed and tile and modifiers
                    game.ScheduleUpdate(1, this);
                    break;
                case Order.OrderType.Attack:
                    if (current_order.target_unit.status == Status.Dead)
                    {
                        status = Status.Idle;
                        NextOrder();
                        Update(game);
                        break;
                    }
                    if (position.Distance(current_order.target_unit.position) > type.attack_range)
                    {
                        // TODO: Move into range
                    }
                    // TODO: Attack
                    // TODO: Calculate cooldown based on attack cooldown and modifiers
                    game.ScheduleUpdate(1, this);
                    break;
            }
        }
    }
}