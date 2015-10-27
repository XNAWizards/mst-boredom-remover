using System.Collections.Generic;
using Microsoft.Xna.Framework;

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

        public Engine(int mapWidth, int mapHeight)
        {
            currentTick = 0;
            futureUpdates = new Dictionary<int, List<Unit>>();
            map = new EngineMap(mapWidth, mapHeight);
            unitTypes = new List<UnitType>();
            players = new List<Player>() {new Player("Frodo"), new Player("Sauron")};
            units = new List<Unit>();
            unitGrid = new Unit[map.width, map.height];
        }

        public Unit AddUnit(Unit unit)
        {
            units.Add(unit);
            if (unit.position.x >= 0 && unit.position.x < map.width && unit.position.y >= 0 &&
                unit.position.y < map.height)
            {
                unitGrid[unit.position.x, unit.position.y] = unit;
            }
            return unit;
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

        public void MoveUnit(Unit unit, Position targetPosition)
        {
            if (unitGrid[targetPosition.x, targetPosition.y] != null)
            {
                Unit targetUnit = unitGrid[targetPosition.x, targetPosition.y];
                if (targetUnit.orders.Count == 0)
                {
                    SwapUnits(unit, unitGrid[targetPosition.x, targetPosition.y]);
                }
            }
            else
            {
                unitGrid[unit.position.x, unit.position.y] = null;
                unitGrid[targetPosition.x, targetPosition.y] = unit;
                unit.previousPosition = unit.position;
                unit.position = targetPosition;
            }
        }

        public void SwapUnits(Unit a, Unit b)
        {
            unitGrid[a.position.x, a.position.y] = b;
            unitGrid[b.position.x, b.position.y] = a;
            a.previousPosition = a.position;
            b.previousPosition = b.position;
            a.animationStartTick = currentTick;
            b.animationStartTick = currentTick;
            a.status = Unit.Status.Moving;
            b.status = Unit.Status.Moving;
            var temp = a.position;
            a.position = b.position;
            b.position = temp;
            OrderMove(b, a.position);
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

        private double Max(double a, double b) { if (a > b) return a; return b; }

        public void Attack( Unit attacker, Unit target )
        {
            //check to make sure you are in range
            if ( attacker.AttackRange() < attacker.position.Distance(target.position))
            {
                return;
            }
            target.health -= Max(1, attacker.AttackStrength() - target.Defense());
            if (target.health<=0) //target dead
            {
                RemoveUpdate(target);
                unitGrid[target.position.x,target.position.y] = null;
                units.Remove(target);
                target.status = Unit.Status.Dead;
            }
        }
    }
}
