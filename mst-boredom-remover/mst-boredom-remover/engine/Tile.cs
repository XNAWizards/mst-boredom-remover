using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mst_boredom_remover
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
