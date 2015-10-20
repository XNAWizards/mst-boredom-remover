using System;
using System.Collections.Generic;

namespace mst_boredom_remover.engine
{
    class Pathfinder
    {
        private class Node : IComparable<Node>
        {
            public readonly Tile tile;
            public readonly Node parent;    
            public readonly int value;
            public readonly int distance;

            public Node(Tile tile, Node par, int d, int v)
            {
                this.tile = tile;
                this.parent = par;
                this.distance = d;
                this.value = v;
            }
            public override string ToString()
            {
                return this.tile.ToString() + ',' + this.distance.ToString() + ',' + this.value.ToString();
            }
            public int CompareTo(Node n)
            {
                int val = value.CompareTo(n.value);
                if (val.Equals(0))
                {
                    val = distance.CompareTo(n.distance);
                    return -val;
                }
                return value.CompareTo(n.value);
            }

        }

        public static Position FindNextStep( Engine engine, Unit unit, Position start, Position end)
        {
            return FindNextStep(engine, unit, engine.map.tiles[start.x, start.y], engine.map.tiles[end.x, end.y]);
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
                foreach (Position pos in engine.map.BreadthFirst(end.position, -1, -1))
                {
                    if (unit.CanMove(pos))
                    {
                        end = engine.map.tiles[pos.x, pos.y];
                        break;
                    }
                }
            }
            
            bool success = false;
            Node tempNode = new Node(start, null, 0, FValue(start,end));
            Node currentBest = null;
            PriorityQueues.PriorityQueue<Node> openSet = new PriorityQueues.PriorityQueue<Node>();
            HashSet<Position> visitedTiles = new HashSet<Position>();
            openSet.Enqueue(tempNode);
            visitedTiles.Add(tempNode.tile.position);
            
            //generate path
            while ( true )
            {
                if ( openSet.Count() == 0 )
                {
                    success = false;
                    break;
                }
                currentBest = openSet.Dequeue();
                //if we are at the goal end
                if ( currentBest.tile.Equals(end) )
                {
                    success = true;
                    break;
                }
                //take current best, generate all possible nodes, push them onto queue
                foreach (var neighbor in currentBest.tile.neighbors)
                {
                    tempNode = new Node(neighbor, currentBest, currentBest.distance + 1, currentBest.distance + 1 + FValue(neighbor, end));

                    if (unit.CanMove(neighbor.position) && !visitedTiles.Contains(neighbor.position))
                    {
                        visitedTiles.Add(neighbor.position);
                        openSet.Enqueue(tempNode);
                    }
                }
            }
            if ( success )
            {
                //generate path
                path = new List<Position>();
                tempNode = currentBest;
                while(tempNode != null )
                {
                    path.Insert(0, tempNode.tile.position);
                    tempNode = tempNode.parent;
                }
            }
            
            return path;
        }

        private static int FValue( Tile start, Tile end )
        {
            return start.position.EuclideanDistanceSquared(end.position);
        }
    }
}
