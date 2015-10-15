using System.Collections.Generic;

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
            players = new List<Player>() {new Player("Frodo")};
            units = new List<Unit>();
            unitGrid = new Unit[map.width, map.height];
        }

        public void AddUnit(Unit unit)
        {
            units.Add(unit);
            if (unit.position.x >= 0 && unit.position.x < map.width && unit.position.y >= 0 &&
                unit.position.y < map.height)
            {
                unitGrid[unit.position.x, unit.position.y] = unit;
            }
        }

        public void Tick()
        {
            if (futureUpdates.ContainsKey(currentTick))
            {
                // Apply all updates for this tick
                foreach (var unit in futureUpdates[currentTick])
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

            futureUpdates[currentTick + ticksFromNow].Add(unit);
            unit.nextMove = currentTick + ticksFromNow;
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
            unitGrid[unit.position.x, unit.position.y] = null;
            unitGrid[targetPosition.x, targetPosition.y] = unit;
            unit.position = targetPosition;
        }

        public void OrderMove(Unit unit, Position targetPosition)
        {
            unit.orders.Add(Order.CreateMoveOrder(targetPosition));
            ScheduleUpdate(1, unit);
        }

        public void OrderProduce(Unit factory, UnitType unitType)
        {
            factory.orders.Add(Order.CreateProduceOrder(unitType));
            ScheduleUpdate(1, factory);
        }

        public void OrderAttack(Unit attacker, Unit target)
        {
            attacker.orders.Add(Order.CreateAttackOrder(target));
            ScheduleUpdate(1, attacker);
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
