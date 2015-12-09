using System;
using System.Collections.Generic;

namespace mst_boredom_remover.engine
{
    class Tile : IComparable<Tile>
    {
        public readonly int id;
        public readonly Position position;
        public TileType tileType;

        public enum TileModifier
        {
            Blazing,
            Freezing,
            Windy
        };

        public List<TileModifier> tileModifiers;

        public List<Tile> neighbors;

        public double pathDistance; // For use with pathing algorithms
        public double pathHeuristic;
        public int pathIndex;
        public Tile pathParent;

        public Tile(int id, Position position, TileType tileType, List<TileModifier> tileModifiers=null, List<Tile> neighbors=null)
        {
            this.id = id;
            this.position = position;
            this.tileType = tileType;
            this.tileModifiers = tileModifiers ?? new List<TileModifier>();
            this.neighbors = neighbors ?? new List<Tile>();
            pathDistance = 0;
            pathHeuristic = 0.0d;
            pathIndex = int.MinValue;
            pathParent = null;
        }

        public int CompareTo(Tile rhs)
        {
            int val = pathHeuristic.CompareTo(rhs.pathHeuristic);
            if (val.Equals(0))
            {
                val = pathDistance.CompareTo(rhs.pathDistance);
                return -val;
            }
            return val;
        }
    }
}