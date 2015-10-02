using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace mst_boredom_remover
{
    
    class Position
    {
        public int x;
        public int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int Distance(Position other)
        {
            return Math.Abs(other.y - y) + Math.Abs(other.x - x);
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Position p = obj as Position;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (x == p.x) && (y == p.y);
        }

        public bool Equals(Position obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }
            
            return x == obj.x && y == obj.y;
        }

        public Vector2 ToVector2()
        {
            return new Vector2(x, y);
        }
    }
}
