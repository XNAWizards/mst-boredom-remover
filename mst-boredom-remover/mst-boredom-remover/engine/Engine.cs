using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace mst_boredom_remover.engine
{
    class Engine
    {
        // In C# int's are always 32-bits, if we assume 60 ticks a second we should be able to handle
        //  a game that lasts a full year long without overflowing. =D
        public int currentTick;
        public readonly Dictionary<int, List<Unit>> futureUpdates;

        // Map stuff
        public readonly EngineMap map;
        
        // Game types
        public readonly List<UnitType> unitTypes;
        public readonly List<TileType> tileTypes;

        // Game objects
        public readonly List<Player> players;
        public readonly List<Unit> units;

        public AI ai;

        private int idCounter;
        private readonly Unit[,] unitGrid;
        private readonly Dictionary<Player, Quadtree> unitQuadtrees;

        public Engine(int mapWidth, int mapHeight)
        {
            currentTick = 0;
            futureUpdates = new Dictionary<int, List<Unit>>();
            map = new EngineMap(this, mapWidth, mapHeight);
            unitTypes = new List<UnitType>();
            tileTypes = new List<TileType>();
            players = new List<Player>();
            units = new List<Unit>();
            unitGrid = new Unit[map.width, map.height];
            unitQuadtrees = new Dictionary<Player, Quadtree>();

            idCounter = 0;

            map.Initialize();
            List<Position> goldTiles = new List<Position>();
            List<Position> ironTiles = new List<Position>();
            List<Position> manaTiles = new List<Position>();
            ai = new AI(this, null, goldTiles, ironTiles, manaTiles);
        }

        public Unit GetUnitAt(Position position)
        {
            return map.Inside(position) ? unitGrid[position.x, position.y] : null;
        }

        public Unit GetUnitAt(int x, int y)
        {
            return map.Inside(x, y) ? unitGrid[x, y] : null;
        }

        public Player AddPlayer(string name, int teamIndex=0)
        {
            Player newPlayer = new Player(GetNextId(), name, teamIndex);
            players.Add(newPlayer);
            unitQuadtrees[newPlayer] = new Quadtree(map.width, map.height);
            return newPlayer;
        }

        public Unit AddUnit(UnitType unitType, Position position, Player owner)
        {
            if (!map.Inside(position)) return null;

            var newUnit = new Unit(GetNextId(), this, unitType, position, owner);
            units.Add(newUnit);
            unitQuadtrees[owner].AddUnit(newUnit);
            CacheSetUnitAt(newUnit);
            return newUnit;
        }

        public Tile AddTile(TileType tileType, Position position)
        {
            if (!map.Inside(position)) return null;
            
            var newTile = new Tile(GetNextId(), position, tileType);
            map.SetTileAt(position, newTile);
            return newTile;
        }

        public TileType GetTileTypeByName(string name)
        {
            return tileTypes.FirstOrDefault(tileType => tileType.name == name);
        }

        public void Tick()
        {
            if (futureUpdates.ContainsKey(currentTick))
            {
                var x = futureUpdates[currentTick];
                // Apply all updates for this tick
                foreach (var unit in x)
                {
                    unit.Update();
                }

                // We are done with all the updates for this tick
                futureUpdates.Remove(currentTick);

                foreach (Unit unit in units.Where(unit => unit.orders.Count == 0))
                {
                    Unit nearestEnemy = FindNearestEnemy(unit, 20);
                    if (nearestEnemy != null)
                    {
                        OrderAttack(unit, nearestEnemy);
                    }
                }
            }
            if ( currentTick%100 == 0 )
            {
                ai.makeMoves(1);
            }
            currentTick += 1;
        }

        public void ScheduleUpdate(int ticksFromNow, Unit unit)
        {
            if (unit.nextMove <= currentTick)
            {
                if (!futureUpdates.ContainsKey(currentTick + ticksFromNow))
                {
                    futureUpdates[currentTick + ticksFromNow] = new List<Unit>();
                }

                futureUpdates[currentTick + ticksFromNow].Add(unit);
                unit.nextMove = currentTick + ticksFromNow;
            }
        }

        public void RemoveUpdate(Unit unit)
        {
            if (!futureUpdates.ContainsKey(unit.nextMove))
            {
                return;
            }
            if( futureUpdates[unit.nextMove].Contains(unit))
            {
                futureUpdates[unit.nextMove].Remove(unit);
            }
        }

        public void OrderMove(Unit unit, Position targetPosition)
        {
            unit.orders.Add(Order.CreateMoveOrder(targetPosition));
            ScheduleUpdate(1, unit);
        }

        public void OrderProduce(Unit factory, UnitType unitType, Position targetPosition = null)
        {
            factory.orders.Add(Order.CreateProduceOrder(unitType, targetPosition));
            ScheduleUpdate(1, factory);
        }

        public void OrderAttack(Unit attacker, Unit target)
        {
            attacker.orders.Add(Order.CreateAttackOrder(target));
            ScheduleUpdate(1, attacker);
        }

        public void OrderGather(Unit gatherer, Position target)
        {
            gatherer.orders.Add(Order.CreateGatherOrder(target));
            ScheduleUpdate(1, gatherer);
        }

        public void Attack( Unit attacker, Unit target )
        {
            //check to make sure you are in range
            if ( attacker.AttackRange() < attacker.position.Distance(target.position))
            {
                return;
            }
            target.health -= Math.Max(1, attacker.AttackStrength() - target.Defense());
            if (target.health<=0) //target dead
            {
                RemoveUpdate(target);
                CacheRemoveUnitAt(target.position);
                units.Remove(target);
                unitQuadtrees[target.owner].RemoveUnit(target);
                target.status = Unit.Status.Dead;
            }
        }

        public Unit FindNearestEnemy(Unit unit, int maxDistance)
        {
            Unit nearestEnemy = null;
            int nearestDistance = int.MaxValue;
            foreach (var player in players)
            {
                if (player.team != unit.owner.team)
                {
                    Unit nearUnit = unitQuadtrees[player].NearestUnitTo(unit, maxDistance);
                    if (nearUnit != null)
                    {
                        int distance = nearUnit.position.Distance(unit.position);
                        if (nearestEnemy == null || distance < nearestDistance)
                        {
                            nearestEnemy = nearUnit;
                            nearestDistance = distance;
                        }
                    }
                }
            }
            return nearestEnemy;
        }

        // Kinda private methods
        
        public void MoveUnit(Unit unit, Position targetPosition)
        {
            Debug.Assert(targetPosition.Distance(unit.position) == 1);
            Debug.Assert(unitGrid[targetPosition.x, targetPosition.y] == null);
            
            CacheRemoveUnitAt(unit.position);
            unit.previousPosition = unit.position;
            unit.position = targetPosition;
            unit.animationStartTick = currentTick;
            CacheSetUnitAt(unit);
        }

        public void SwapUnits(Unit a, Unit b)
        {
            Debug.Assert(a.position.Distance(b.position) == 1);

            var targetPosition = b.position;
            // Remove a
            CacheRemoveUnitAt(a.position);
            // Tell b to move into a's spot
            Debug.Assert(b.orders.Count == 0, "Swap: b still had orders left.");
            b.orders.Add(Order.CreateMoveOrder(a.position));
            b.Update(); // This will force the unit to wait the appropriate amount of time before moving back
            Debug.Assert(b.position.Equals(a.position), "Swap: b did not move back to original position.");
            // Add a to b's old position
            a.previousPosition = a.position;
            a.position = targetPosition;
            a.animationStartTick = currentTick;
            CacheSetUnitAt(a);
            // Give an order to b to go back to his original position
            OrderMove(b, targetPosition);
        }

        // Private methods

        private int GetNextId()
        {
            return ++idCounter;
        }
        
        private void CacheSetUnitAt(Unit unit)
        {
            Debug.Assert(unit != null);
            if (map.Inside(unit.position))
            {
                unitGrid[unit.position.x, unit.position.y] = unit;
                unitQuadtrees[unit.owner].UpdateUnit(unit);
            }
        }

        private void CacheRemoveUnitAt(Position position)
        {
            if (map.Inside(position))
            {
                unitGrid[position.x, position.y] = null;
            }
        }
    }
}
