using System.Collections.Generic;

namespace mst_boredom_remover.engine
{
    class Player
    {
        public readonly int id;
        public readonly string name;
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

        public Player(int id, string name, int team=0)
        {
            this.id = id;

            this.name = name;
            this.team = team;

            gold = 0.0;
            manaCystals = 0.0;
            iron = 0.0;
            isAlive = true;
            modifiers = new List<PlayerModifier>();
            unitCount = 0;
        }
    }
}
