using System.Collections.Generic;

namespace mst_boredom_remover.engine
{
    class Tile
    {
        public int id;
        public Position position;
        public TileType tileType;

        public enum TileModifier
        {
            Blazing,
            Freezing,
            Windy
        };

        public List<TileModifier> tileModifiers;

        public List<Tile> neighbors;

        public int temp; // TROLOLOL
    }
}