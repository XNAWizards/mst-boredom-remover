﻿using System;
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
    }
}
