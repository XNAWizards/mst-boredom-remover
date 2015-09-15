using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mst_boredom_remover
{
    class UnitType
    {
        public double max_health;

        public enum AttackType
        {
            Melee,
            Arrow,
            Fireball
        };
        public AttackType attack_type;
        public double attack_strength;
        public double attack_range;

        public enum MovementType
        {
            Walker,
            Swimmer,
            Flier,
            Digger
        };
        public MovementType movement_type;
        public double movement_speed;

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

        public UnitType(double max_health=100.0, AttackType attack_type=AttackType.Melee,
            double attack_strength=1.0, double attack_range=1.0, MovementType movement_type=MovementType.Walker,
            double movement_speed=1.0, List<Action> actions=null, List<Spell> spells=null)
        {
            this.max_health = max_health;
            this.attack_type = attack_type;
            this.attack_strength = attack_strength;
            this.attack_range = attack_range;
            this.movement_type = movement_type;
            this.movement_speed = movement_speed;
            this.actions = actions ?? new List<Action>();
            this.spells = spells ?? new List<Spell>();
        }
    }
}
