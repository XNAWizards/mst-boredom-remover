using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mst_boredom_remover
{
    class Engine
    {
        // In C# int's are always 32-bits, if we assume 60 ticks a second we should be able to handle
        //  a game that lasts a full year long without overflowing. =D
        public int current_tick;
        public Dictionary<int, List<Unit>> future_updates;

        // Map stuff
        public EngineMap map;
        
        // Game types
        public List<UnitType> unit_types;

        // Game objects
        public List<Player> players;
        public List<Unit> units;
        public Unit[,] unit_grid;

        public Engine(int map_width, int map_height)
        {
            current_tick = 0;
            future_updates = new Dictionary<int, List<Unit>>();
            map = new EngineMap(map_width, map_height);
            unit_types = new List<UnitType>();
            players = new List<Player>() {new Player("Frodo")};
            units = new List<Unit>();
            unit_grid = new Unit[map.width, map.height];
        }

        public void AddUnit(Unit unit)
        {
            units.Add(unit);
            if (unit.position.x >= 0 && unit.position.x < map.width && unit.position.y >= 0 &&
                unit.position.y < map.height)
            {
                unit_grid[unit.position.x, unit.position.y] = unit;
            }
        }

        public void Tick()
        {
            if (future_updates.ContainsKey(current_tick))
            {
                // Apply all updates for this tick
                foreach (var unit in future_updates[current_tick])
                {
                    unit.Update(this);
                }

                // We are done with all the updates for this tick
                future_updates.Remove(current_tick);
            }

            current_tick += 1;
        }

        public void ScheduleUpdate(int ticks_from_now, Unit unit)
        {
            if (!future_updates.ContainsKey(current_tick + ticks_from_now))
            {
                future_updates[current_tick + ticks_from_now] = new List<Unit>();
            }

            future_updates[current_tick + ticks_from_now].Add(unit);
        }

        public void MoveUnit(Unit unit, Position target_position)
        {
            unit_grid[unit.position.x, unit.position.y] = null;
            unit_grid[target_position.x, target_position.y] = unit;
            unit.position = target_position;
        }
    }
}
