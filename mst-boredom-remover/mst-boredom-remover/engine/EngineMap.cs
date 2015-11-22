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
            {'@', "Coast Land on North"}, // @
            {'/', "Coast Land on East"}, // /
            {'&', "Coast Land on South"}, // &
            {'#', "Coast Land on West"}, // #
            {'^', "River Straight Vertical"},
            {',', "River Straight Horizontal"},
            {'<', "River East and South"},
            {'>', "River West and South"},
            {']', "River West and North"},
            {'[', "River East and North"},
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
            engine.ai.goldTiles = new List<Position>();
            engine.ai.ironTiles = new List<Position>();
            engine.ai.manaTiles = new List<Position>();
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
                        engine.ai.goldTiles.Add(targetPosition);
                    } 
                    else if (tileType.resourceType == TileType.ResourceType.Iron)
                    {
                        engine.ai.ironTiles.Add(targetPosition);
                    } 
                    else if (tileType.resourceType == TileType.ResourceType.ManaCrystals)
                    {
                        engine.ai.manaTiles.Add(targetPosition);
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

        /// <summary>
        /// A generator that produces positions by doing a breadth first iteration of tiles starting at a given position.
        /// </summary>
        /// <param name="position">The starting position.</param>
        /// <param name="distance">The maximum distance from the starting position.  Or negative for no limit.</param>
        /// <param name="size">The maximum number of tiles traversed.  Or negative for no limit.</param>
        public IEnumerable<Position> BreadthFirst(Position position, int distance=-1, int size=-1) // Yay C# has generators!
        {
            List<Tile> perimeter = new List<Tile>() { tiles[position.x, position.y] };
            HashSet<Tile> seen = new HashSet<Tile>(perimeter);
            perimeter[0].pathDistance = 0;
            while (size != 0 && perimeter.Count > 0)
            {
                Tile top = perimeter[0];
                perimeter.RemoveAt(0);
                if (distance >= 0 && top.pathDistance > distance)
                {
                    yield break;
                }
                yield return top.position;

                foreach (var neighbor in top.neighbors)
                {
                    if (!seen.Contains(neighbor))
                    {
                        perimeter.Add(neighbor);
                        seen.Add(neighbor);
                        neighbor.pathDistance = top.pathDistance + 1;
                    }
                }
                size -= 1;
            }
        }
    }
}
