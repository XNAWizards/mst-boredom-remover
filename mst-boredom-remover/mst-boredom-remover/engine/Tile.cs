using System.Collections.Generic;

namespace mst_boredom_remover.engine
{
    class Tile
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

        public int pathDistance; // For use with pathing algorithms

        public Tile(int id, Position position, TileType tileType, List<TileModifier> tileModifiers=null, List<Tile> neighbors=null)
        {
            this.id = id;
            this.position = position;
            this.tileType = tileType;
            this.tileModifiers = tileModifiers ?? new List<TileModifier>();
            this.neighbors = neighbors ?? new List<Tile>();
            pathDistance = 0;
        }
    }
}