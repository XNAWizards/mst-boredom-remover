﻿using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace mst_boredom_remover.engine
{
    class TileType
    {
        public string name;
        public double movementCost;

        public enum Biome
        {
            Grassland,
            Desert
        };
        public Biome biome;
        public List<UnitType.MovementType> allowedMovementTypes;

        public enum ResourceType
        {
            None,
            Gold,
            Iron,
            ManaCrystals
        };
        public ResourceType resourceType;
        public Texture2D texture;

        TileType(string name="", Texture2D texture = null, double movementCost=1.0, Biome biome=Biome.Grassland,
            List<UnitType.MovementType> allowedMovementTypes=null, ResourceType resourceType=ResourceType.None)
        {
            this.name = name;
            this.texture = texture;
            this.movementCost = movementCost;
            this.biome = biome;
            this.allowedMovementTypes = allowedMovementTypes ?? new List<UnitType.MovementType>();
            this.resourceType = resourceType;
        }
    }
}
