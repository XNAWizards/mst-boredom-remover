using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mst_boredom_remover
{
    class Order
    {
        public enum OrderType
        {
            Move,
            Attack,
            Follow,
            AttackMove,
            Patrol,
            Gaurd,
            Idle,
            Build,
            Produce
        };
        public OrderType order_type;
        public Position target_position; // Can be Null
        public Unit target_unit; // Can be Null
        public UnitType unit_type_build; // Nullable

        public static Order CreateMoveOrder(Position position)
        {
            return new Order()
            {
                order_type = OrderType.Move,
                target_position = position,
                target_unit = null
            };
        }
    }
}
