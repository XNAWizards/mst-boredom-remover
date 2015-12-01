using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace mst_boredom_remover.engine
{
    class AI
    {
        readonly Engine engine;

        public Player me;
        public List<Position> goldTiles;
        public List<Position> ironTiles;
        public List<Position> manaTiles;

        public AI( Engine e, Player player, List<Position> g, List<Position> i, List<Position> m )
        {
            engine = e;

            me = player;


            goldTiles = g;          
            ironTiles = i;
            manaTiles = m;
        }

        //The engine can call this to tell the AI to issue upto numMoves orders. This allows the AI to scale how much it is doing
        public void makeMoves( int numMoves )
        {
            int peasentCount = 0,knightCount =0,archerCount =0, mineCount =0, townCount =0;
            foreach (Unit u in engine.units)
            {
                if (u.type.Equals(engine.unitTypes[0]))
                {
                    peasentCount++;
                }
                if (u.type.Equals(engine.unitTypes[0]))
                {
                    peasentCount++;
                }
                if (u.type.Equals(engine.unitTypes[0]))
                {
                    peasentCount++;
                }
                if (u.type.Equals(engine.unitTypes[0]))
                {
                    peasentCount++;
                }
                if (u.type.Equals(engine.unitTypes[0]))
                {
                    peasentCount++;
                }
            }
            //generic loop to determine what the AI does
            for ( int i = 0; i < numMoves; i++ )
            {
                //Currently we want to be able to gather
                Unit currentUnit = null;
                foreach (Unit u in engine.units)
                {
                    if (u.owner == me)
                    {
                        if ( me.gold >= engine.unitTypes[3].goldCost && u.CanBuild() ) //Can build a town, build it
                        {
                            if ( u.orders.Count == 0 )
                            {
                                knightCount++;
                                //engine.OrderProduce(u, engine.unitTypes[3]);
                            }
                        }
                        if ( me.gold >= engine.unitTypes[4].goldCost )
                        {
                            if (u.orders.Count == 0 || u.orders[0].orderType.Equals(Order.OrderType.Gather) ) //if the unit is not doing anything
                            {
                                currentUnit = u;
                                if ( u.orders.Count != 0 && u.orders[0].orderType.Equals(Order.OrderType.Gather) && goldTiles.Contains(u.position) )
                                {
                                    u.orders.Clear();
                                    engine.OrderMove(u, u.position + new Position(0, 1));
                                    engine.OrderProduce(u, engine.unitTypes[4], u.position);
                                    break;
                                }
                                //find closest open goldtile and issue order for that
                                Position closestGold = goldTiles[0];
                                Position unitPosition = currentUnit.position;
                                int distance = unitPosition.Distance(closestGold);
                                foreach (Position p in goldTiles)
                                {
                                    if (engine.GetUnitAt(p) == null ) //no one is on the tile
                                    {
                                        if (unitPosition.Distance(p) < distance)
                                        {
                                            distance = unitPosition.Distance(p);
                                            closestGold = p;
                                        }
                                    }
                                }
                                engine.OrderProduce(currentUnit, engine.unitTypes[4], closestGold);
                                break;
                            }
                        }
                        else if (u.CanGather())
                        {
                            if (u.orders.Count == 0 || u.orders[0].orderType.Equals(Order.OrderType.Gather) ) //if the unit is not doing anything
                            {
                                currentUnit = u;
                                if (currentUnit == null)
                                {
                                    return; //I have no units
                                }
                                if ( goldTiles.Contains(currentUnit.position) )
                                {
                                    continue;
                                }
                                //find closest open goldtile and issue order for that
                                Position closestGold = goldTiles[0];
                                Position unitPosition = currentUnit.position;
                                int distance = unitPosition.Distance(closestGold);
                                foreach (Position p in goldTiles)
                                {
                                    if (engine.GetUnitAt(p) == null) //no one is on the tile
                                    {
                                        if (unitPosition.Distance(p) < distance)
                                        {
                                            distance = unitPosition.Distance(p);
                                            closestGold = p;
                                        }
                                    }
                                }
                                if ( distance != 0 )
                                {
                                    engine.OrderGather(currentUnit, closestGold);
                                    break; 
                                }
                            }
                        }
                    }
                }

            }
        }

    };
}