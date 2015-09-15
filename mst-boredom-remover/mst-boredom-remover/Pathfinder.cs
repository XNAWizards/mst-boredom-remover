using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mst_boredom_remover
{
    class Pathfinder
    {
        private class Node : IComparable<Node>
        {
            public Position position;
            public Node parent;    
            public int value;
            public int distance;

            public Node( Position p, Node par, int d, int v)
            {
                this.position = p;
                this.parent = par;
                this.distance = d;
                this.value = v;
            }
            public override string ToString()
            {
                return this.position.ToString() + ',' + this.distance.ToString() + ',' + this.value.ToString();
            }
            public int CompareTo(Node n)
            {
                return value.CompareTo(n.value);
            }

        }
        public static Position findNextStep( Unit unit, Position start, Position end )
        {
            List<Position> temp = null;
            temp = findPath(unit, start, end);
            if (temp != null && temp.Count > 1)
                return temp[1];
            else
                return null;
        }

        public static List<Position> findPath ( Unit unit, Position start, Position end )
        {
            List<Position> path = null;
            int possibleMoves = Enum.GetNames(typeof(Map.Directions)).Length;
            bool stop = false;
            bool success = false;
            Node tempNode = new Node(start, null, 0, fValue(start,end));
            Node currentBest = null;
            Position tempPosition = new Position(0,0);
            PriorityQueues.PriorityQueue<Node> openSet = new PriorityQueues.PriorityQueue<Node>();

            openSet.Enqueue(tempNode);

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
                if ( currentBest.position.Equals(end) )
                {
                    stop = true;
                    success = true;
                    break;
                }
                //take current best, generate all possible nodes, push them onto queue
                foreach (Map.Directions move in Enum.GetValues(typeof(Map.Directions)) )
                {
                    switch ( move )
                    {
                        case Map.Directions.North:
                            tempPosition = new Position(currentBest.position.x + 1,currentBest.position.y);
                            tempNode = new Node( tempPosition , currentBest, currentBest.distance+1, currentBest.distance+1+fValue(tempPosition,end));
                            break;
                        case Map.Directions.South:
                            tempPosition = new Position(currentBest.position.x - 1,currentBest.position.y);
                            tempNode = new Node( tempPosition , currentBest, currentBest.distance+1, currentBest.distance+1+fValue(tempPosition,end));
                            break;
                        case Map.Directions.East:
                            tempPosition = new Position(currentBest.position.x,currentBest.position.y + 1);
                            tempNode = new Node( tempPosition , currentBest, currentBest.distance+1, currentBest.distance+1+fValue(tempPosition,end));
                            break;
                        case Map.Directions.West:
                            tempPosition = new Position(currentBest.position.x,currentBest.position.y - 1);
                            tempNode = new Node( tempPosition , currentBest, currentBest.distance+1, currentBest.distance+1+fValue(tempPosition,end));
                            break;
                    }
                    if ( unit.CanMove( tempPosition ) )
                    {
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
                    path.Insert(0, tempNode.position);
                    tempNode = tempNode.parent;
                }
            }
            
            return path;
        }

        private static int fValue( Position start, Position end )
        {
            return start.Distance(end);
        }
    }
}
