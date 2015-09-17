using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mst_boredom_remover
{
    class TileType
    {
        public double movement_cost;

        public enum Biome
        {
            Grassland,
            Desert
        };
        public Biome biome;
        public List<UnitType.MovementType> allowed_movement_types;

        public enum ResourceType
        {
            Gold,
            Iron,
            ManaCrystals
        };
        public ResourceType resource_type;
    }
}
