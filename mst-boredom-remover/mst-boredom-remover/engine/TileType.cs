using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace mst_boredom_remover
{
    class TileType
    {
        public string name;
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
            None,
            Gold,
            Iron,
            ManaCrystals
        };
        public ResourceType resource_type;
        Texture2D texture;

        TileType(string name="", Texture2D texture = null, double movement_cost=1.0, Biome biome=Biome.Grassland,
            List<UnitType.MovementType> allowed_movement_types=null, ResourceType resource_type=ResourceType.None)
        {
            this.name = name;
            this.texture = texture;
            this.movement_cost = movement_cost;
            this.biome = biome;
            this.allowed_movement_types = allowed_movement_types ?? new List<UnitType.MovementType>();
            this.resource_type = resource_type;
        }
    }
}
