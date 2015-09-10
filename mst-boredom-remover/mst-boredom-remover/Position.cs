using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mst_boredom_remover
{
    
    class Position
    {
        public int x;
        public int y;

        public double Distance(Position other)
        {
            return Math.Abs(other.y - y) + Math.Abs(other.x - x);
        }
    }
}
