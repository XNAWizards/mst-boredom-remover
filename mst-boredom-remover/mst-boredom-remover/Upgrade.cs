using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mst_boredom_remover.engine
{
    class Upgrade
    {
        string name;
        public UnitType u;
        public double attackStrength;
        public double attackRange;
        public double attackSpeed;
        public double defense;
        public double HP; // plus multiplier

        public Upgrade(string name, int amount, UnitType u, int playerIndex)
        {
            this.u = u;
            switch (name)
            {
                case "AttackUp":
                    attackStrength = amount;
                    break;
                case "DefenseUp":
                    defense = amount;
                    break;
                case "HPUp":
                    HP = amount;
                    break;
                case "RangeUp":
                    attackRange = amount;
                    break;
                case "AttackSpeedUp":
                    attackSpeed = amount;
                    break;
                default:

                    break;
            }
        }
    }
}
