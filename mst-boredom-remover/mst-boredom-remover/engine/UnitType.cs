using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace mst_boredom_remover.engine
{
    class UnitType
    {
        public readonly string name;
        public readonly double maxHealth;

        public enum AttackType
        {
            Melee,
            Arrow,
            Fireball
        };
        public readonly AttackType attackType;
        public readonly double attackStrength;
        public readonly double attackRange;
        public readonly double attackSpeed;
        public readonly double defense;

        public enum MovementType
        {
            Walker,
            Swimmer,
            Flier,
            Digger,
            None
        };
        public readonly MovementType movementType;
        public readonly double movementSpeed;

        public enum Action
        {
            Move,
            Attack,
            Produce,
            Gather,
            Build,
            Cast
        };
        public readonly List<Action> actions;

        public enum Spell
        {
            Fireball,
            IvyWhip
        }
        public readonly List<Spell> spells;

        public readonly double gatherRate;
        public readonly double goldCost;
        public readonly double ironCost;
        public readonly double manaCrystalsCost;

        // Animation stuff
        public readonly Texture2D[] idleTextures;
        public readonly Texture2D[] attackTextures;
        public readonly Texture2D[] moveTextures;

        public UnitType(string name="", double maxHealth=100.0, AttackType attackType=AttackType.Melee,
            double attackStrength=1.0, double attackRange=1.0, double attackSpeed = 1.0, double defense=0.0,
            MovementType movementType=MovementType.Walker,
            double movementSpeed=10.0, List<Action> actions=null, List<Spell> spells=null, double gatherRate = 10.0,
            double goldCost=10.0, double ironCost=0.0, double manaCrystalsCost=0.0,
            Texture2D[] idleTextures=null, Texture2D[] attackTextures=null, Texture2D[] moveTextures=null)
        {
            this.name = name;
            this.maxHealth = maxHealth;
            this.attackType = attackType;
            this.attackStrength = attackStrength;
            this.attackRange = attackRange;
            this.attackSpeed = attackSpeed;
            this.defense = defense;
            this.movementType = movementType;
            this.movementSpeed = movementSpeed;
            this.actions = actions ?? new List<Action>();
            this.spells = spells ?? new List<Spell>();
            this.gatherRate = gatherRate;
            this.goldCost = goldCost;
            this.ironCost = ironCost;
            this.manaCrystalsCost = manaCrystalsCost;
            this.idleTextures = idleTextures ?? new Texture2D[] { };
            this.attackTextures = attackTextures ?? new Texture2D[] { };
            this.moveTextures = moveTextures ?? new Texture2D[] { };
        }
    }
}
