using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

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

        // Offline graphics stuff
        public readonly string textureFilename;
        public readonly int[][] idleIndices; // [RDLU][index]
        public readonly int[][] walkIndices; // [RDLU][index]
        public readonly int[][] attackIndices; // [RDLU][index]
        // Online graphics stuff
        public readonly Texture2D texture;

        // Animation stuff
        public readonly Texture2D[] idleTextures;
        public readonly Texture2D[] attackTextures;
        public readonly Texture2D[] moveTextures;

        public UnitType(string name="", double maxHealth=100.0, AttackType attackType=AttackType.Melee,
            double attackStrength=1.0, double attackRange=1.0, double attackSpeed = 1.0, double defense=0.0,
            MovementType movementType=MovementType.Walker,
            double movementSpeed=1.0, List<Action> actions=null, List<Spell> spells=null, double gatherRate = 10.0,
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

        public UnitType(string name = "", double maxHealth = 100.0, AttackType attackType = AttackType.Melee,
            double attackStrength = 1.0, double attackRange = 1.0, double attackSpeed = 1.0, double defense = 0.0,
            MovementType movementType = MovementType.Walker,
            double movementSpeed = 1.0, List<Action> actions = null, List<Spell> spells = null, double gatherRate = 10.0,
            double goldCost = 10.0, double ironCost = 0.0, double manaCrystalsCost = 0.0,
            string textureFilename="", int[][] idleIndices=null, int[][] walkIndices=null, int[][]attackIndices=null)
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
            this.textureFilename = textureFilename;
            this.idleIndices = idleIndices ?? new [] {new [] { 0 }, new [] { 0 }, new [] { 0 }, new [] { 0 } };
            this.walkIndices = walkIndices ?? new [] { new[] { 0 }, new[] { 0 }, new[] { 0 }, new[] { 0 } };
            this.attackIndices = attackIndices ?? new [] { new[] { 0 }, new[] { 0 }, new[] { 0 }, new[] { 0 } };
        }

        public static UnitType LoadFromFile(string filename)
        {
            using (XmlTextReader reader = new XmlTextReader(filename))
            {
                reader.WhitespaceHandling = WhitespaceHandling.None;

                string name = reader.GetAttribute("name", "") ?? "UNKOWN";
                double maxHealth = XmlConvert.ToDouble(reader.GetAttribute("maxHealth", "") ?? "100");
                AttackType attackType = (AttackType) Enum.Parse(typeof (AttackType), reader.GetAttribute("attackType", "") ?? "Melee", true); // OPTIMIZE: Use dict
                double attackStrength = XmlConvert.ToDouble(reader.GetAttribute("attackStrength", "") ?? "1");
                double attackRange = XmlConvert.ToDouble(reader.GetAttribute("attackRange", "") ?? "1");
                double attackSpeed = XmlConvert.ToDouble(reader.GetAttribute("attackSpeed", "") ?? "1");
                double defense = XmlConvert.ToDouble(reader.GetAttribute("defense", "") ?? "0");
                MovementType movementType = (MovementType)Enum.Parse(typeof(MovementType), reader.GetAttribute("movementType", "") ?? "Walker", true);
                double movementSpeed = XmlConvert.ToDouble(reader.GetAttribute("movementSpeed", "") ?? "1");
                double gatherRate = XmlConvert.ToDouble(reader.GetAttribute("gatherRate", "") ?? "10");
                double goldCost = XmlConvert.ToDouble(reader.GetAttribute("goldCost", "") ?? "10");
                double ironCost = XmlConvert.ToDouble(reader.GetAttribute("ironCost", "") ?? "0");
                double manaCrystalCost = XmlConvert.ToDouble(reader.GetAttribute("manaCrystalCost", "") ?? "0");
                string textureFilename = reader.GetAttribute("textureFilename", "") ?? "";

                reader.ReadStartElement("UnitType"); // Moves to the next thing after the UnitType start tag

                List<Action> actions = new List<Action>();
                while (reader.NodeType == XmlNodeType.Element && reader.Name == "Action")
                {
                    reader.ReadStartElement("Action");
                    actions.Add((Action)Enum.Parse(typeof(Action), reader.ReadString(), true));
                    reader.ReadEndElement(); // Action
                }
                List<Spell> spells = new List<Spell>();
                while (reader.NodeType == XmlNodeType.Element && reader.Name == "Spell")
                {
                    reader.ReadStartElement("Spell");
                    spells.Add((Spell)Enum.Parse(typeof(Spell), reader.ReadString(), true));
                    reader.ReadEndElement(); // Spell
                }

                string[] idleNames = new[] {"IdleRight", "IdleDown", "IdleLeft", "IdleUp"};
                int[][] idleIndices = new int[4][];
                while (reader.NodeType == XmlNodeType.Element && idleNames.Contains(reader.Name))
                {
                    reader.ReadStartElement(reader.Name);
                    int index = Array.IndexOf(idleNames, reader.Name);
                    List<int> frames = new List<int>(); // Use list because length is unknown
                    while (reader.NodeType == XmlNodeType.Element && reader.Name == "Frame")
                    {
                        reader.ReadStartElement("Frame");
                        frames.Add(XmlConvert.ToInt32(reader.ReadString()));
                        reader.ReadEndElement(); // Frame
                    }
                    idleIndices[index] = frames.ToArray();
                    reader.ReadEndElement(); // IdleXXXX
                }

                string[] walkNames = new[] { "WalkRight", "WalkDown", "WalkLeft", "WalkUp" };
                int[][] walkIndices = new int[4][];
                while (reader.NodeType == XmlNodeType.Element && walkNames.Contains(reader.Name))
                {
                    reader.ReadStartElement(reader.Name);
                    int index = Array.IndexOf(walkNames, reader.Name);
                    List<int> frames = new List<int>(); // Use list because length is unknown
                    while (reader.NodeType == XmlNodeType.Element && reader.Name == "Frame")
                    {
                        reader.ReadStartElement("Frame");
                        frames.Add(XmlConvert.ToInt32(reader.ReadString()));
                        reader.ReadEndElement(); // Frame
                    }
                    walkIndices[index] = frames.ToArray();
                    reader.ReadEndElement(); // WalkXXXX
                }

                string[] attackNames = new[] { "AttackRight", "AttackDown", "AttackLeft", "AttackUp" };
                int[][] attackIndices = new int[4][];
                while (reader.NodeType == XmlNodeType.Element && attackNames.Contains(reader.Name))
                {
                    reader.ReadStartElement(reader.Name);
                    int index = Array.IndexOf(attackNames, reader.Name);
                    List<int> frames = new List<int>(); // Use list because length is unknown
                    while (reader.NodeType == XmlNodeType.Element && reader.Name == "Frame")
                    {
                        reader.ReadStartElement("Frame");
                        frames.Add(XmlConvert.ToInt32(reader.ReadString()));
                        reader.ReadEndElement(); // Frame
                    }
                    attackIndices[index] = frames.ToArray();
                    reader.ReadEndElement(); // AttackXXXX
                }
                reader.ReadEndElement(); // UnitType

                return new UnitType(name, maxHealth, attackType, attackStrength, attackRange, attackSpeed, defense,
                    movementType, movementSpeed, actions, spells, gatherRate, goldCost, ironCost, manaCrystalCost,
                    textureFilename, idleIndices, walkIndices, attackIndices);
            }
        }

        public void SaveToFile(string filename)
        {
            using (FileStream fs = File.Open(filename, FileMode.Create, FileAccess.Write))
            {
                using (XmlWriter writer = XmlWriter.Create(fs)) // This forces the XmlWriter to flush to the file
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("UnitType");

                    writer.WriteAttributeString("name", name);
                    writer.WriteAttributeString("maxHealth", XmlConvert.ToString(maxHealth));
                    writer.WriteAttributeString("attackType", attackType.ToString());
                    writer.WriteAttributeString("attackStrength", XmlConvert.ToString(attackStrength));
                    writer.WriteAttributeString("attackRange", XmlConvert.ToString(attackRange));
                    writer.WriteAttributeString("attackSpeed", XmlConvert.ToString(attackSpeed));
                    writer.WriteAttributeString("defense", XmlConvert.ToString(defense));
                    writer.WriteAttributeString("movementType", movementType.ToString());
                    writer.WriteAttributeString("movementSpeed", XmlConvert.ToString(movementSpeed));

                    writer.WriteAttributeString("gatherRate", XmlConvert.ToString(gatherRate));
                    writer.WriteAttributeString("goldCost", XmlConvert.ToString(goldCost));
                    writer.WriteAttributeString("ironCost", XmlConvert.ToString(ironCost));
                    writer.WriteAttributeString("manaCrystalsCost", XmlConvert.ToString(manaCrystalsCost));
                    writer.WriteAttributeString("textureFilename", textureFilename);

                    foreach (var action in actions) writer.WriteElementString("Action", action.ToString());
                    foreach (var spell in spells) writer.WriteElementString("Spell", spell.ToString());
                    int directionIndex = 0;
                    foreach (var direction in new []{"Right", "Down", "Left", "Up"})
                    {
                        writer.WriteStartElement("Idle" + direction);
                        foreach (var idleIndex in idleIndices[directionIndex++]) writer.WriteElementString("Frame", XmlConvert.ToString(idleIndex));
                        writer.WriteEndElement();
                    }
                    directionIndex = 0;
                    foreach (var direction in new[] { "Right", "Down", "Left", "Up" })
                    {
                        writer.WriteStartElement("Walk" + direction);
                        foreach (var idleIndex in walkIndices[directionIndex++]) writer.WriteElementString("Frame", XmlConvert.ToString(idleIndex));
                        writer.WriteEndElement();
                    }
                    directionIndex = 0;
                    foreach (var direction in new[] { "Right", "Down", "Left", "Up" })
                    {
                        writer.WriteStartElement("Attack" + direction);
                        foreach (var idleIndex in attackIndices[directionIndex++]) writer.WriteElementString("Frame", XmlConvert.ToString(idleIndex));
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement(); // UnitType
                    writer.WriteEndDocument();
                }
            }
        }
    }
}
