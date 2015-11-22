using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace mst_boredom_remover.engine
{
    class TileType
    {
        public readonly string name;
        public readonly double movementCost;

        public enum Biome
        {
            Ocean,
            Plain,
            Mountain,
            Forest,
            Dreadlands,
            Desert,
            Tundra,
            Shore,
            River,
            Gold,
            Iron,
            ManaCrystals
        };
        public readonly Biome biome;
        public readonly List<UnitType.MovementType> allowedMovementTypes;

        public enum ResourceType
        {
            None,
            Gold,
            Iron,
            ManaCrystals
        };
        public readonly ResourceType resourceType;
        public readonly Texture2D texture;
        public readonly float rotation;

        public TileType(string name="", Texture2D texture = null, double movementCost=1.0, Biome biome=Biome.Plain,
            List<UnitType.MovementType> allowedMovementTypes=null, ResourceType resourceType=ResourceType.None,
            float rotation=0.0f)
        {
            this.name = name;
            this.texture = texture;
            this.movementCost = movementCost;
            this.biome = biome;
            this.allowedMovementTypes = allowedMovementTypes ?? new List<UnitType.MovementType>();
            this.resourceType = resourceType;
            this.rotation = rotation; // In radians clockwise
        }
    }
}
