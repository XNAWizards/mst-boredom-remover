﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mst_boredom_remover
{
    class MapEngine
    {
        public int width;
        public int height;

        public Tile[,] tiles;

        public enum Directions
        {
            North,
            South,
            East,
            West
        }

        public MapEngine(int width=0, int height=0)
        {
            this.width = width;
            this.height = height;
            this.tiles = new Tile[width, height];
        }
    }
}

