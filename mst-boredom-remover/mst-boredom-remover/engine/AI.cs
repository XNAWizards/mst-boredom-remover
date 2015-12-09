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
            int peasentCount = 0,knightCount =0,archerCount =0, mineCount =0, townCount =0, attackingCount=0;
            foreach (Unit u in engine.units)
            {
                if (u.owner != me)
                    continue;
                if (u.type.Equals(engine.unitTypes[2]))
                {
                    peasentCount++;
                }
                if (u.type.Equals(engine.unitTypes[0]))
                {
                    knightCount++;
                    if (u.orders.Count > 0 && u.orders[0].orderType.Equals(Order.OrderType.Attack))
                    {
                        attackingCount++;
                    }
                }
                if (u.type.Equals(engine.unitTypes[1]))
                {
                    archerCount++;
                    if (u.orders.Count>0 && u.orders[0].orderType.Equals(Order.OrderType.Attack))
                    {
                        attackingCount++;
                    }
                }
                if (u.type.Equals(engine.unitTypes[4]))
                {
                    mineCount++;
                }
                if (u.type.Equals(engine.unitTypes[3]))
                {
                    townCount++;
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
                        if ( u.CanAttack() && !u.CanGather() && !u.CanProduce())
                        {
                            if ( u.orders.Count != 0 )
                            {
                                continue;
                            }
                            Position p = null;
                            int distance = 100000000;
                            Unit closestTarget = null;
                            foreach ( Unit target in engine.units)
                            {
                                if ( target.owner != me )
                                {
                                    if ( target.position.Distance(u.position) < distance )
                                    {
                                        distance = target.position.Distance(u.position);
                                        closestTarget = target;
                                        p = target.position;
                                    }
                                }
                            }
                            if ( p != null )
                            {
                                engine.OrderAttack(u, closestTarget);
                                break;
                            }


                        }
                        if (u.CanProduce() && !u.CanGather() && archerCount+knightCount<=attackingCount)
                        {
                            int unit_to_produce = -1;
                            if (peasentCount == 0)
                            {
                                unit_to_produce = 2;
                            }
                            else if (knightCount < archerCount)
                            {
                                unit_to_produce = 0;
                            }
                            else
                            {
                                unit_to_produce = 1;
                            }
                            if (unit_to_produce >= 0)
                            {
                                engine.OrderProduce(u, engine.unitTypes[unit_to_produce]);
                                break;
                            }
                        }
                        if ( townCount < 5 && me.gold >= engine.unitTypes[3].goldCost && u.CanBuild() ) //Can build a town, build it
                        {
                            if ( u.orders.Count == 0 )
                            {
                                engine.OrderProduce(u, engine.unitTypes[3]);
                            }
                        }
                        if ( u.CanMove() && u.CanBuild() && me.gold >= engine.unitTypes[4].goldCost && mineCount < 5 ) //make mines
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
                        else if (u.CanMove() && u.CanGather() && me.gold < 1000)
                        {
                            if (u.orders.Count == 0/* || u.orders[0].orderType.Equals(Order.OrderType.Move) */) //if the unit is not doing anything
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
                                        bool alreadyTargeted = false;
                                        foreach (Unit blocker in engine.units)
                                        {
                                            if (blocker.orders.Count > 0 &&
                                                blocker.orders[0].orderType.Equals(Order.OrderType.Gather) &&
                                                blocker.orders[0].targetPosition.Equals(p))
                                            {
                                                alreadyTargeted = true;
                                                break;
                                            }
                                        }
                                        if (alreadyTargeted)
                                            continue;
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