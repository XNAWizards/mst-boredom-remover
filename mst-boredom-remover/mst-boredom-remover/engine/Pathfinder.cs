using System.Collections.Generic;
using PriorityQueues;

namespace mst_boredom_remover.engine
{
    class Pathfinder
    {
        public static int pathCounter = int.MinValue + 1; // Incremented after every check 

        /// <summary>
        /// A generator that produces positions by doing a breadth first iteration of tiles starting at a given position.
        /// </summary>
        /// <param name="engine">An engine object ot get tiles from</param>
        /// <param name="position">The starting position.</param>
        /// <param name="distance">The maximum distance from the starting position.  Or negative for no limit.</param>
        /// <param name="size">The maximum number of tiles traversed.  Or negative for no limit.</param>
        public static IEnumerable<Position> BreadthFirst(Engine engine, Position position, int distance = -1, int size = -1) // Yay C# has generators!
        {
            int localPathCounter = pathCounter++;
            List<Tile> perimeter = new List<Tile>() { engine.map.GetTileAt(position) };
            perimeter[0].pathDistance = 0;
            while (size != 0 && perimeter.Count > 0)
            {
                Tile top = perimeter[0];
                top.pathIndex = localPathCounter;
                perimeter.RemoveAt(0);
                if (distance >= 0 && top.pathDistance > distance)
                {
                    yield break;
                }
                yield return top.position;

                foreach (var neighbor in top.neighbors)
                {
                    if (neighbor.pathIndex < localPathCounter)
                    {
                        perimeter.Add(neighbor);
                        neighbor.pathIndex = localPathCounter;
                        neighbor.pathDistance = top.pathDistance + 1;
                    }
                }
                size -= 1;
            }
            // This is not the place for cleanup code
        }

        public static Position FindNextStep( Engine engine, Unit unit, Position start, Position end)
        {
            return FindNextStep(engine, unit, engine.map.GetTileAt(start), engine.map.GetTileAt(end));
        }

        public static Position FindNextStep( Engine engine, Unit unit, Tile start, Tile end )
        {
            List<Position> temp = FindPath(engine, unit, start, end);
            if (temp != null && temp.Count > 1)
                return temp[1];
            else
                return null;
        }

        public static List<Position> FindPath ( Engine engine, Unit unit, Tile start, Tile end )
        {
            List<Position> path = null;
            
            if (!unit.CanMove(end.position))
            {
                foreach (Position pos in BreadthFirst(engine, end.position, -1, -1))
                {
                    if (unit.CanMove(pos))
                    {
                        end = engine.map.tiles[pos.x, pos.y];
                        break;
                    }
                }
            }
            
            bool success = false;
            start.pathParent = null;
            start.pathDistance = 0;
            start.pathHeuristic = start.pathDistance + FValue(start, end);
            PriorityQueue<Tile> openSet = new PriorityQueue<Tile>();
            openSet.Enqueue(start);
            int count = 0;

            //generate path
            while ( openSet.Count() > 0 )
            {
                count += 1;
                Tile currentBest = openSet.Dequeue();
                currentBest.pathIndex = pathCounter;
                // if we are at the goal end
                if (currentBest.position.Equals(end.position))
                {
                    success = true;
                    break;
                }
                // Give up if we backtrack too far
                /*if (currentBest.pathHeuristic >= start.pathHeuristic*2 &&
                    currentBest.position.Distance(end.position) > 200)
                {
                    break;
                }*/
                // Take current best, generate all possible nodes, push them onto queue
                foreach (var neighbor in currentBest.neighbors)
                {
                    if (!unit.CanMove(neighbor.position))
                    {
                        continue;
                    }
                    double tentativeCost = currentBest.pathDistance + neighbor.tileType.movementCost;
                    if (neighbor.pathIndex < pathCounter)
                    {
                        neighbor.pathIndex = pathCounter;
                        neighbor.pathParent = currentBest;
                        neighbor.pathDistance = tentativeCost;
                        neighbor.pathHeuristic = neighbor.pathDistance + FValue(neighbor, end);
                        openSet.Enqueue(neighbor);
                    }
                    else if (tentativeCost < neighbor.pathDistance)
                    {
                        // Update costs if the current path is better than the existing one
                        neighbor.pathParent = currentBest;
                        neighbor.pathDistance = tentativeCost;
                        neighbor.pathHeuristic = neighbor.pathDistance + FValue(neighbor, end);
                        openSet.HeapifyUp(neighbor);
                    }
                }
            }
            if ( success )
            {
                // Generate path by following parent from end to start
                path = new List<Position>();
                Tile runner = end;
                while (runner != null)
                {
                    path.Insert(0, runner.position);
                    runner = runner.pathParent;
                }
            }

            pathCounter += 1;

            return path;
        }

        private static double FValue( Tile start, Tile end )
        {
            return start.position.EuclideanDistance(end.position);
        }
    }
}
