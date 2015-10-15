using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;

namespace mst_boredom_remover
{
    class Pathfinder
    {
        private class Node : IComparable<Node>
        {
            public Tile tile;
            public Node parent;    
            public int value;
            public int distance;

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

        public static Position findNextStep( Engine engine, Unit unit, Position start, Position end)
        {
            return findNextStep(engine, unit, engine.map.tiles[start.x, start.y], engine.map.tiles[end.x, end.y]);
        }

        public static Position findNextStep( Engine engine, Unit unit, Tile start, Tile end )
        {
            List<Position> temp = null;
            temp = findPath(engine, unit, start, end);
            if (temp != null && temp.Count > 1)
                return temp[1];
            else
                return null;
        }

        public static List<Position> findPath ( Engine engine, Unit unit, Tile start, Tile end )
        {
            List<Position> path = null;
            int numPossibleMoves = Enum.GetNames(typeof(EngineMap.Directions)).Length;
            
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

            bool stop = false;
            bool success = false;
            Node tempNode = new Node(start, null, 0, fValue(start,end));
            Node currentBest = null;
            Position tempPosition = new Position(0,0);
            PriorityQueues.PriorityQueue<Node> openSet = new PriorityQueues.PriorityQueue<Node>();
            HashSet<Position> visitedTiles = new HashSet<Position>();
            int phase = 0;
            openSet.Enqueue(tempNode);
            visitedTiles.Add(tempNode.tile.position);
            
            //generate path
            while ( !stop )
            {
                if ( openSet.Count() == 0 )
                {
                    success = false;
                    stop = true;
                    break;
                }
                currentBest = openSet.Dequeue();
                //if we are at the goal end
                if ( currentBest.tile.Equals(end) )
                {
                    stop = true;
                    success = true;
                    break;
                }
                //take current best, generate all possible nodes, push them onto queue
                Tile neighbor = null;
                for ( int i = phase; i < currentBest.tile.neighbors.Count+phase; i++)
                {
                    neighbor = currentBest.tile.neighbors[i % currentBest.tile.neighbors.Count];
                    tempNode = new Node(neighbor, currentBest, currentBest.distance + 1, currentBest.distance + 1 + fValue(neighbor, end));

                    if (unit.CanMove(neighbor.position) && !visitedTiles.Contains(neighbor.position))
                    {
                        visitedTiles.Add(neighbor.position);
                        openSet.Enqueue(tempNode);
                    }
                }
                phase = (phase + 1) % currentBest.tile.neighbors.Count;
                
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

        private static int fValue( Tile start, Tile end )
        {
            return start.position.Distance(end.position);
        }
    }
}
