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
        public Dictionary<int, List<Unit>> futureUpdates;

        // Map stuff
        public EngineMap map;
        
        // Game types
        public List<UnitType> unitTypes;
        public List<TileType> tileTypes;

        // Game objects
        public List<Player> players;
        public List<Unit> units;
        public Unit[,] unitGrid;

        public AI ai;

        private int idCounter;

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

        public void SetUnitAt(Position position, Unit unit)
        {
            if (map.Inside(position))
            {
                unitGrid[position.x, position.y] = unit;
            }
        }

        public Player AddPlayer(string name, int teamIndex=0)
        {
            Player newPlayer = new Player(GetNextId(), name, teamIndex);
            players.Add(newPlayer);
            return newPlayer;
        }

        public Unit AddUnit(UnitType unitType, Position position, Player owner)
        {
            if (!map.Inside(position)) return null;

            var newUnit = new Unit(GetNextId(), this, unitType, position, owner);
            units.Add(newUnit);
            SetUnitAt(newUnit.position, newUnit);
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
        
        // This function should not have any Unit-specific logic
        //  Therefore this should only be called when the Unit KNOWS that it can move to a location
        public void MoveUnit(Unit unit, Position targetPosition)
        {
            var blockingUnit = GetUnitAt(targetPosition);
            if (blockingUnit != null)
            {
                SwapUnits(unit, blockingUnit);
            }
            else
            {
                // Update cache
                SetUnitAt(unit.position, null);
                SetUnitAt(targetPosition, unit);
                // Change units position
                unit.position = targetPosition;
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
                SetUnitAt(target.position, null);
                units.Remove(target);
                target.status = Unit.Status.Dead;
            }

        }

        // Private methods

        private int GetNextId()
        {
            return ++idCounter;
        }

        private void SwapUnits(Unit a, Unit b)
        {
            var targetPosition = b.position;
            // Remove a
            SetUnitAt(a.position, null);
            // Tell b to move into a's spot
            Debug.Assert(b.orders.Count == 0, "Swap: b still had orders left.");
            b.orders.Add(Order.CreateMoveOrder(a.position));
            b.Update(); // This will force the unit to wait the appropriate amount of time before moving back
            Debug.Assert(b.position.Equals(a.position), "Swap: b did not move back to original position.");
            // Add a to b's old position
            a.position = targetPosition;
            SetUnitAt(a.position, a);
            // Give an order to b to go back to his original position
            OrderMove(b, targetPosition);
        }
    }
}
