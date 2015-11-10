using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace mst_boredom_remover.engine
{
    class AI
    {
        readonly Engine engine;

        public List<Unit> units;
        public List<Position> goldTiles;
        public List<Position> ironTiles;
        public List<Position> manaTiles;

        AI( ref Engine e, List<Unit> u = null, List<Position> g = null, List<Position> i = null, List<Position> m = null )
        {
            engine = e;

            if ( u == null )
            {
                units = new List<Unit>();
            }
            else
            {
                units = u;
            }
            if ( g == null )
            {
                goldTiles = new List<Position>();
            }
            else
            {
                goldTiles = g;
            }            
            if ( i == null )
            {
                ironTiles = new List<Position>();
            }
            else
            {
                ironTiles = i;
            }            
            if ( m == null )
            {
                manaTiles = new List<Position>();
            }
            else
            {
                manaTiles = m;
            }
        }

        //The engine can call this to tell the AI to issue upto numMoves orders. This allows the AI to scale how much it is doing
        void makeMoves( int numMoves )
        {
            //generic loop to determine what the AI does
            for ( int i = 0; i < numMoves; i++ )
            {
                //Currently we want to be able to gather
                Unit currentUnit = null;
                foreach (Unit u in units)
                {
                    if ( u.CanGather() ) 
                    {
                        if (u.orders.Count == 0) //if the unit is not doing anything
                        {
                            currentUnit = u;
                            break;
                        }
                    }
                }
                //find closest open goldtile and issue order for that
                Position closestGold = goldTiles[0];
                Position unitPosition = currentUnit.position;
                int distance = unitPosition.Distance(closestGold);
                foreach (Position p in goldTiles)
                {
                    if (engine.unitGrid[p.x, p.y] == null) //no one is on the tile
                    {
                        if (unitPosition.Distance(p) < distance)
                        {
                            distance = unitPosition.Distance(p);
                            closestGold = p;
                        }
                    }
                }
                engine.OrderGather(currentUnit, closestGold);
            }
        }

    };
}