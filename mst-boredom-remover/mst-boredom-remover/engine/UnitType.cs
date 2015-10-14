using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace mst_boredom_remover
{
    class UnitType
    {
        public string name;
        public double maxHealth;

        public enum AttackType
        {
            Melee,
            Arrow,
            Fireball
        };
        public AttackType attackType;
        public double attackStrength;
        public double attackRange;
        public double defense;

        public enum MovementType
        {
            Walker,
            Swimmer,
            Flier,
            Digger
        };
        public MovementType movementType;
        public double movementSpeed;

        public enum Action
        {
            Move,
            Attack,
            Produce,
            Cast
        };
        public List<Action> actions;

        public enum Spell
        {
            Fireball,
            IvyWhip
        }
        public List<Spell> spells;

        public double goldCost;
        public double ironCost;
        public double manaCrystalsCost;

        // Animation stuff
        public Texture2D[] idleTextures;
        public Texture2D[] attackTextures;
        public Texture2D[] moveTextures;

        public UnitType(string name="", double maxHealth=100.0, AttackType attackType=AttackType.Melee,
            double attackStrength=1.0, double attackRange=1.0, double defense=0.0, MovementType movementType=MovementType.Walker,
            double movementSpeed=1.0, List<Action> actions=null, List<Spell> spells=null,
            double goldCost=0.0, double ironCost=0.0, double manaCrystalsCost=0.0,
            Texture2D[] idleTextures=null, Texture2D[] attackTextures=null, Texture2D[] moveTextures=null)
        {
            this.name = name;
            this.maxHealth = maxHealth;
            this.attackType = attackType;
            this.attackStrength = attackStrength;
            this.attackRange = attackRange;
            this.defense = defense;
            this.movementType = movementType;
            this.movementSpeed = movementSpeed;
            this.actions = actions ?? new List<Action>();
            this.spells = spells ?? new List<Spell>();
            this.goldCost = goldCost;
            this.ironCost = ironCost;
            this.manaCrystalsCost = manaCrystalsCost;
            this.idleTextures = idleTextures ?? new Texture2D[] { };
            this.attackTextures = attackTextures ?? new Texture2D[] { };
            this.moveTextures = moveTextures ?? new Texture2D[] { };
        }
    }
}
