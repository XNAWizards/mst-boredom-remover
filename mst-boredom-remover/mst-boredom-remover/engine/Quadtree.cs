using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace mst_boredom_remover.engine
{
    class Quadtree
    {
        public readonly int width, height;

        private readonly int chunkWidth, chunkHeight;
        private readonly int xChunks, yChunks;
        private readonly Chunk[,] chunks;

        public Quadtree(int width, int height, int chunkWidth=8, int chunkHeight=8)
        {
            this.width = width;
            this.height = height;
            this.chunkWidth = chunkWidth;
            this.chunkHeight = chunkHeight;

            xChunks = width/chunkWidth;
            yChunks = height/chunkHeight;

            chunks = new Chunk[xChunks, yChunks];

            // Initialize chunks

            for (var y = 0; y < yChunks; ++y)
            {
                for (var x = 0; x < xChunks; ++x)
                {
                    chunks[x, y] = new Chunk(new Position(x * chunkWidth, y * chunkHeight), chunkWidth, chunkHeight);
                }
            }

            // Compute neighbors

            int[] xOff = {1, 0, -1, 0};
            int[] yOff = {0, 1, 0, -1};

            for (var y = 0; y < yChunks; ++y)
            {
                for (var x = 0; x < xChunks; ++x)
                {
                    for (var i = 0; i < 4; ++i)
                    {
                        var neighborX = x + xOff[i];
                        var neighborY = y + yOff[i];

                        if (neighborX >= 0 && neighborY >= 0 && neighborX < xChunks && neighborY < yChunks)
                        {
                            chunks[x, y].AddNeighbor(chunks[neighborX, neighborY]);
                        }
                    }
                }
            }
        }
        
        public void AddUnit(Unit unit)
        {
            GetChunk(unit.position)?.AddUnit(unit);
        }

        public void UpdateUnit(Unit unit, Position previousPosition)
        {
            var previousChunk = GetChunk(previousPosition);
            var newChunk = GetChunk(unit.position);
            if (previousChunk != newChunk)
            {
                previousChunk?.CheckAndRemoveUnit(unit);
                newChunk?.AddUnit(unit);
            }
        }

        public Unit NearestUnit(Position startPosition, int maxDistance)
        {
            if (!Inside(startPosition))
            {
                var nearestPosition = new Position(Math.Max(0, Math.Min(startPosition.x, width - 1)),
                    Math.Max(0, Math.Min(startPosition.y, height - 1)));
                maxDistance -= nearestPosition.Distance(startPosition);
                startPosition = nearestPosition;
            }

            Chunk chunk = GetChunk(startPosition);

            // Breadth first search
            HashSet<Chunk> haveBeenInFrontier = new HashSet<Chunk> {chunk};
            LinkedList<Chunk> frontier = new LinkedList<Chunk>(haveBeenInFrontier);
            while (frontier.Count > 0)
            {
                var top = frontier.First();
                frontier.RemoveFirst();
                // Try to find nearest in this chunk
                Unit closestUnit = null;
                int nearestDistance = Int32.MaxValue;
                foreach (var unit in top.units)
                {
                    int distance = startPosition.Distance(unit.position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        closestUnit = unit;
                    }
                }
                if (closestUnit != null && nearestDistance <= maxDistance)
                {
                    return closestUnit;
                }
                // if a unit has not been found yet, try the neighbors
                foreach (var neighbor in top.neighbors)
                {
                    if (!haveBeenInFrontier.Contains(neighbor) && neighbor.GetDistanceFrom(startPosition) <= maxDistance)
                    {
                        frontier.AddLast(neighbor);
                        haveBeenInFrontier.Add(neighbor);
                    }
                }
            }
            return null; // Didn't find a unit within maxDistance
        }

        // Private methods

        private bool Inside(Position position)
        {
            return position.x >= 0 && position.y >= 0 && position.x < width && position.y < height;
        }

        private Chunk GetChunk(Position position)
        {
            if (Inside(position))
            {
                return chunks[position.x/chunkWidth, position.y/chunkHeight];
            }
            return null;
        }

        // Private Classes

        private class Chunk
        {
            public readonly HashSet<Unit> units;
            public readonly List<Chunk> neighbors;

            private readonly Position topLeft;
            private readonly Position bottomRight;

            public Chunk(Position position, int width, int height)
            {
                topLeft = position;
                bottomRight = topLeft + new Position(width - 1, height - 1);

                units = new HashSet<Unit>();
                neighbors = new List<Chunk>();
            }

            public bool Contains(Unit unit)
            {
                return units.Contains(unit);
            }

            public void AddUnit(Unit unit)
            {
                Debug.Assert(Inside(unit.position));
                units.Add(unit);
            }

            public bool CheckAndRemoveUnit(Unit unit)
            {
                return !Inside(unit.position) && units.Remove(unit);
            }

            public void AddNeighbor(Chunk chunk)
            {
                neighbors.Add(chunk);
            }

            public int GetDistanceFrom(Position position)
            {
                // Calculate point nearest to startPosition inside this chunk
                Position nearestPosition = new Position(Math.Max(topLeft.x, Math.Min(bottomRight.x, position.x)),
                    Math.Max(topLeft.y, Math.Min(bottomRight.y, position.y)));
                // Return distance
                return nearestPosition.Distance(position);
            }

            private bool Inside(Position position)
            {
                return position.x >= topLeft.x && position.y >= topLeft.y &&
                       position.x <= bottomRight.x && position.y <= bottomRight.y;
            }
        }
    }
}
