using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mst_boredom_remover
{
    class Player
    {
        public int id;
        public string name;
        public int team;
        public double gold;
        public double mana_cystals;
        public double iron;
        public bool is_alive;
        public int unit_count;

        public enum PlayerModifier
        {
            Efficient,
            Lazy,
            Hated,
            Loved
        };
        public List<PlayerModifier> modifiers;

        public Player(string name, int team=0)
        {
            // TODO: Set id

            this.name = name;
            this.team = team;

            this.gold = 0.0;
            this.mana_cystals = 0.0;
            this.iron = 0.0;
            this.is_alive = true;
            this.modifiers = new List<PlayerModifier>();
            this.unit_count = 0;
        }
    }
}
