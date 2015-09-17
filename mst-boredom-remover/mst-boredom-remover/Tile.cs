using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mst_boredom_remover
{
    class Tile
    {
        public int id;
        public TileType tile_type;

        public enum TileModifier
        {
            Blazing,
            Freezing,
            Windy
        };
        public List<TileModifier> tile_modifiers;
    }
}
