using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mst_boredom_remover
{
    class EngineMap
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

        public static Position[] direction_deltas = new Position[]
        {
            new Position(1, 0),
            new Position(0, 1),
            new Position(-1, 0),
            new Position(0, -1)
        };

        public EngineMap(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.tiles = new Tile[width, height];
            Position target_position = new Position(0, 0);
            for (target_position.y = 0; target_position.y < height; ++target_position.y)
            {
                for (target_position.x = 0; target_position.x < width; ++target_position.x)
                {
                    Tile target_tile = new Tile();
                    tiles[target_position.x, target_position.y] = target_tile;
                    target_tile.position = new Position(target_position.x, target_position.y);
                }
            }
            for (target_position.y = 0; target_position.y < height; ++target_position.y)
            {
                for (target_position.x = 0; target_position.x < width; ++target_position.x)
                {
                    Tile target_tile = tiles[target_position.x, target_position.y];
                    target_tile.neighbors = new List<Tile>(from delta in direction_deltas
                                             where Inside(target_position + delta)
                                             select tiles[target_position.x + delta.x, target_position.y + delta.y]);
                }
            }
        }

        public bool Inside(Position position)
        {
            return (position.x >= 0 && position.y >= 0 && position.x < width && position.y < height);
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
            perimeter[0].temp = 0;
            while (size != 0 && perimeter.Count > 0)
            {
                Tile top = perimeter[0];
                perimeter.RemoveAt(0);
                if (distance >= 0 && top.temp > distance)
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
                        neighbor.temp = top.temp + 1;
                    }
                }
                size -= 1;
            }
        }
    }
}
