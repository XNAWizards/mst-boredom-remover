using System;
using System.Collections.Generic;
using System.Linq;

namespace mst_boredom_remover.engine
{
    class EngineMap
    {
        public readonly Engine engine;
        public readonly int width;
        public readonly int height;

        public Tile[,] tiles;

        public enum Directions
        {
            North,
            South,
            East,
            West
        }

        public static readonly Position[] directionDeltas =
        {
            new Position(1, 0),
            new Position(0, 1),
            new Position(-1, 0),
            new Position(0, -1)
        };

        public static readonly Dictionary<char, string> charToBiomeString = new Dictionary<char, string>
        {
            {'~', "Ocean"},
            {'+', "Plain"},
            {'M', "Mountain"},
            {'F', "Forest" },
            {'%', "Dreadlands" },
            {'D', "Desert" },
            {'T', "Tundra" },
            {'G', "Gold" },
            {'L', "Iron" },
            {'*', "ManaCrystals" },
            {'@', "Coast Land on North"},
            {'/', "Coast Land on East"},
            {'&', "Coast Land on South"},
            {'#', "Coast Land on West"},
            {'r', "Coast Land on North and East"},
            {'q', "Coast Land on East and South"},
            {'w', "Coast Land on South and West"},
            {'e', "Coast Land on West and North"},
            {'t', "Coast Ocean on North"},
            {'y', "Coast Ocean on East"},
            {'u', "Coast Ocean on South"},
            {'i', "Coast Ocean on West"},
            {'^', "River Straight Vertical"},
            {',', "River Straight Horizontal"},
            {'<', "River East and South"},
            {'>', "River West and South"},
            {']', "River West and North"},
            {'[', "River East and North"},
            {'|', "River Land on North"},
            {'p', "River Land on East"},
            {'_', "River Land on South"},
            {'?', "River Land on West"},
            {'X', "River Four"},
            {'-', "River Four"}, // Default river?
        };

        public EngineMap(Engine engine, int width, int height)
        {
            this.engine = engine;
            this.width = width;
            this.height = height;
        }

        public void Initialize()
        {
            // Initialize tiles
            tiles = new Tile[width, height];
            for (var y = 0; y < height; ++y)
            {
                for (var x = 0; x < width; ++x)
                {
                    engine.AddTile(null, new Position(x, y));
                }
            }
            // Initialize neighbors
            for (var y = 0; y < height; ++y)
            {
                for (var x = 0; x < width; ++x)
                {
                    var targetPosition = new Position(x, y);
                    Tile targetTile = GetTileAt(x, y);
                    targetTile.neighbors = new List<Tile>(from delta in directionDeltas
                                                          where Inside(targetPosition + delta)
                                                          select tiles[x + delta.x, y + delta.y]);
                }
            }
        }

        public Tile GetTileAt(Position position)
        {
            return Inside(position) ? tiles[position.x, position.y] : null;
        }

        public Tile GetTileNearestTo(Position position)
        {
            var nearestPosition = new Position(Math.Max(0, Math.Min(position.x, width - 1)),
                    Math.Max(0, Math.Min(position.y, height - 1)));
            return GetTileAt(nearestPosition);
        }

        public Tile GetTileAt(int x, int y)
        {
            return Inside(x, y) ? tiles[x, y] : null;
        }

        public void SetTileAt(Position position, Tile tile)
        {
            if (Inside(position))
            {
                tiles[position.x, position.y] = tile;
            }
        }

        public void UpdateTilesFromCharmap(char [,] charmap)
        {
            engine.ai1.goldTiles = new List<Position>();
            engine.ai1.ironTiles = new List<Position>();
            engine.ai1.manaTiles = new List<Position>();
            engine.ai2.goldTiles = new List<Position>();
            engine.ai2.ironTiles = new List<Position>();
            engine.ai2.manaTiles = new List<Position>();
            for (var y = 0; y < height; ++y)
            {
                for (var x = 0; x < width; ++x)
                {
                    var targetPosition = new Position(x, y);

                    string biomeName = "Ocean";
                    if (charToBiomeString.ContainsKey(charmap[x, y]))
                    {
                        biomeName = charToBiomeString[charmap[x, y]];
                    }
                    var tileType = engine.GetTileTypeByName(biomeName);
                    
                    GetTileAt(targetPosition).tileType = tileType;
                    
                    if ( tileType.resourceType == TileType.ResourceType.Gold )
                    {
                        engine.ai1.goldTiles.Add(targetPosition);
                        engine.ai2.goldTiles.Add(targetPosition);
                    } 
                    else if (tileType.resourceType == TileType.ResourceType.Iron)
                    {
                        engine.ai1.ironTiles.Add(targetPosition);
                        engine.ai2.ironTiles.Add(targetPosition);
                    } 
                    else if (tileType.resourceType == TileType.ResourceType.ManaCrystals)
                    {
                        engine.ai1.manaTiles.Add(targetPosition);
                        engine.ai2.manaTiles.Add(targetPosition);
                    }
                }
            }
        }

        public bool Inside(Position position)
        {
            return (position.x >= 0 && position.y >= 0 && position.x < width && position.y < height);
        }

        public bool Inside(int x, int y)
        {
            return (x >= 0 && y >= 0 && x < width && y < height);
        }
    }
}
