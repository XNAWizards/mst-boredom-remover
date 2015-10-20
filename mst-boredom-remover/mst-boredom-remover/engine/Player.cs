using System.Collections.Generic;

namespace mst_boredom_remover.engine
{
    class Player
    {
        public int id;
        public string name;
        public int team;
        public double gold;
        public double manaCystals;
        public double iron;
        public bool isAlive;
        public int unitCount;

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
            this.manaCystals = 0.0;
            this.iron = 0.0;
            this.isAlive = true;
            this.modifiers = new List<PlayerModifier>();
            this.unitCount = 0;
        }
    }
}
