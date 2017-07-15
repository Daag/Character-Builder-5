﻿using OGL.Common;
using OGL.Descriptions;
using OGL.Keywords;
using OGL.Spells;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using NCalc;

namespace OGL
{
    [XmlInclude(typeof(ModifiedSpell))]
    public class Spell : IComparable<Spell>, IXML, IOGLElement<Spell>
    {
        [XmlArrayItem(Type = typeof(Keyword)),
        XmlArrayItem(Type = typeof(Save)),
        XmlArrayItem(Type = typeof(Material))]
        public List<Keyword> Keywords;
        [XmlIgnore]
        public static XmlSerializer Serializer = new XmlSerializer(typeof(Spell));
        [XmlIgnore]
        static public Dictionary<String, Spell> spells = new Dictionary<string, Spell>(StringComparer.OrdinalIgnoreCase);
        [XmlIgnore]
        static public Dictionary<String, List<Spell>> SpellLists = new Dictionary<string, List<Spell>>(StringComparer.OrdinalIgnoreCase);
        [XmlIgnore]
        public string Filename { get; set; }
        [XmlIgnore]
        static public Dictionary<String, Spell> simple = new Dictionary<string, Spell>(StringComparer.OrdinalIgnoreCase);
        public string Name { get; set; }
        public string CastingTime { get; set; }
        public string Range { get; set; }
        public string Duration { get; set; }
        public int Level {get; set;}
        public string Description { get; set; }
        [XmlArrayItem(Type = typeof(Description)),
        XmlArrayItem(Type = typeof(ListDescription)),
        XmlArrayItem(Type = typeof(TableDescription))]
        public List<Description> Descriptions { get; set; }
        public String Source { get; set; }
        public List<CantripDamage> CantripDamage;
        [XmlIgnore]
        public bool KWChanged = false;
        [XmlIgnore]
        public bool ShowSource { get; set; } = false;
        public Spell()
        {
            Keywords = new List<Keyword>();
            Descriptions = new List<Description>();
            Source = ConfigManager.DefaultSource;
            CantripDamage = new List<CantripDamage>();
        }
        public Spell(int level, string name, string castingTime, string range, string duration, string description)
        {
            Keywords = new List<Keyword>();
            Descriptions = new List<Description>();
            Level = level;
            Name = name;
            CastingTime = castingTime;
            Range = range;
            Duration = duration;
            Description = description;
            Source = ConfigManager.DefaultSource;
            CantripDamage = new List<CantripDamage>();
            if (Duration != null && Duration.ToLowerInvariant().Contains("instantaneous")) Keywords.Add(new Keyword("Instantaneous"));
            if (Duration != null && Duration.ToLowerInvariant().Contains("concentration")) Keywords.Add(new Keyword("Concentration"));
            if (Level == 0) Keywords.Add(new Keyword("Cantrip"));
            if (Description != null && Description.ToLowerInvariant().Contains("make a ranged spell attack"))
            {
                Keywords.Add(new Keyword("Ranged"));
                Keywords.Add(new Keyword("Attack"));
            }
            if (Description != null && Description.ToLowerInvariant().Contains("make a melee spell attack"))
            {
                Keywords.Add(new Keyword("Melee"));
                if (!Keywords.Exists(k=>k.Name=="attack")) Keywords.Add(new Keyword("Attack"));
            }
            if (Description != null && (Description.ToLowerInvariant().Contains("your spell save dc")))
            {
                Keywords.Add(new Keyword("Save"));
            }
            if (Description != null && (Description.ToLowerInvariant().Contains("must make a strength saving throw") || Description.ToLowerInvariant().Contains("must succeed on a strength saving throw")))
            {
                Keywords.Add(new Keyword("Strength Saving Throw"));
                if (!Keywords.Exists(k => k.Name == "save")) Keywords.Add(new Keyword("Save"));
            }
            if (Description != null && (Description.ToLowerInvariant().Contains("must make a dexterity saving throw") || Description.ToLowerInvariant().Contains("must succeed on a dexterity saving throw")))
            {
                Keywords.Add(new Keyword("Dexterity Saving Throw"));
                if (!Keywords.Exists(k => k.Name == "save")) Keywords.Add(new Keyword("Save"));
            }
            if (Description != null && (Description.ToLowerInvariant().Contains("must make a constitution saving throw") || Description.ToLowerInvariant().Contains("must succeed on a constitution saving throw")))
            {
                Keywords.Add(new Keyword("Constitution Saving Throw"));
                if (!Keywords.Exists(k => k.Name == "save")) Keywords.Add(new Keyword("Save"));
            }
            if (Description != null && (Description.ToLowerInvariant().Contains("must make an intelligence saving throw") || Description.ToLowerInvariant().Contains("must succeed on an intelligence saving throw")))
            {
                Keywords.Add(new Keyword("Intelligence Saving Throw"));
                if (!Keywords.Exists(k => k.Name == "save")) Keywords.Add(new Keyword("Save"));
            }
            if (Description != null && (Description.ToLowerInvariant().Contains("must make a wisdom saving throw") || Description.ToLowerInvariant().Contains("must succeed on a wisdom saving throw")))
            {
                Keywords.Add(new Keyword("Wisdom Saving Throw"));
                if (!Keywords.Exists(k => k.Name == "save")) Keywords.Add(new Keyword("Save"));
            }
            if (Description != null && (Description.ToLowerInvariant().Contains("must make a charisma saving throw") || Description.ToLowerInvariant().Contains("must succeed on a charisma saving throw")))
            {
                Keywords.Add(new Keyword("Charisma Saving Throw"));
                if (!Keywords.Exists(k => k.Name == "save")) Keywords.Add(new Keyword("Save"));
            }
            Register(null);
        }
        public Spell AssignKeywords(Keyword kw1, Keyword kw2 = null, Keyword kw3 = null, Keyword kw4 = null, Keyword kw5 = null, Keyword kw6 = null, Keyword kw7 = null, Keyword kw8 = null, Keyword kw9 = null, Keyword kw10 = null, Keyword kw11 = null, Keyword kw12 = null)
        {
            List<Keyword> kws = new List<Keyword>() { kw1, kw2, kw3, kw4, kw5, kw6, kw7, kw8, kw9, kw10, kw11, kw12 };
            kws.RemoveAll(k => k == null);
            kws.RemoveAll(k => Keywords.Contains(k));
            Keywords.AddRange(kws);
            Keywords.Sort();
            if (kws.Count > 0) KWChanged = true;
            return this;
        }
        public Spell AddDescription(Description d1, Description d2 = null, Description d3 = null, Description d4 = null, Description d5 = null, Description d6 = null, Description d7 = null, Description d8 = null, Description d9 = null)
        {
            List<Description> ds = new List<Description>() { d1, d2, d3, d4, d5, d6, d7, d8, d9 };
            ds.RemoveAll(d => d == null);
            Descriptions.AddRange(ds);
            return this;
        }
        public Spell AddDescription(String name, string text)
        {
            Descriptions.Add(new Description(name, text));
            return this;
        }
        public Spell AddCantripDamage(string firstdamage, int second = 0, string seconddamage = "", int third = 0, string thirddamage = "", int fourth = 0, string fourthdamage = "", int fifth = 0, string fifthdamage = "")
        {
            CantripDamage.Add(new CantripDamage(1, firstdamage));
            if (second > 0) CantripDamage.Add(new CantripDamage(second, seconddamage));
            if (third > 0) CantripDamage.Add(new CantripDamage(third, thirddamage));
            if (fourth > 0) CantripDamage.Add(new CantripDamage(fourth, fourthdamage));
            if (fifth > 0) CantripDamage.Add(new CantripDamage(fifth, fifthdamage));
            return this;
        }
        public void Register(String file)
        {
            Filename = file;
            if (this is ModifiedSpell) return;
            foreach (Keyword kw in Keywords) kw.check();
            string full = Name + " " + ConfigManager.SourceSeperator + " " + Source;
            if (spells.ContainsKey(full)) throw new Exception("Duplicate Spell: " + full);
            spells.Add(full, this);
            if (simple.ContainsKey(Name))
            {
                simple[Name].ShowSource = true;
                ShowSource = true;
            }
            else simple.Add(Name, this);
        }
        public String ToXML()
        {
            using (StringWriter mem = new StringWriter())
            {
                Serializer.Serialize(mem, this);
                return mem.ToString();
            }
        }

        public MemoryStream ToXMLStream()
        {
            MemoryStream mem = new MemoryStream();
            Serializer.Serialize(mem, this);
            return mem;
        }

        public static Spell Get(String name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperatorString))
            {
                if (spells.ContainsKey(name)) return spells[name];
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && spells.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return spells[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (simple.ContainsKey(name)) return simple[name];
            ConfigManager.LogError("Spell not found: " + name);
            Spell missing = new Spell()
            {
                Name = name,
                Source = "Autogenerated Entry",
                ShowSource = true
            };
            missing.Register(null);
            return missing;
        }
        public override string ToString()
        {
            if (ShowSource || ConfigManager.AlwaysShowSource) return Name + " " + ConfigManager.SourceSeperator + " " + Source;
            return Name;
        }
        public int CompareTo(Spell other)
        {
            return Name.CompareTo(other.Name);
        }
        public override bool Equals(object other)
        {
            if (other is Spell) return String.Equals(Name, ((Spell)other).Name, StringComparison.OrdinalIgnoreCase);
            return false;
        }
        public override int GetHashCode()
        {
            if (Name != null) return Name.GetHashCode();
            return 0;
        }
        public bool Test()
        {
            if (Name.ToLowerInvariant().Contains(Item.Search)) return true;
            if (Description != null && Description.ToLowerInvariant().Contains(Item.Search)) return true;
            if (Keywords != null && Keywords.Exists(k => k.Name == Item.Search)) return true;
            if (Descriptions != null && Descriptions.Exists(d => d.Test())) return true;
            return false;
        }
        public static IOrderedEnumerable<Spell> Subsection()
        {
            if (Item.Search == "") return from s in spells.Values orderby s select s;
            else return from s in spells.Values where s.Test() orderby s select s;
        }

        public string GetCantripDamage(int level)
        {
            string damage="";
            foreach (CantripDamage s in CantripDamage) if (s.Level <= level) damage = s.Damage;
            return damage;
        }
        public string GetDamageType()
        {
            List<string> damagetypes=new List<string>();
            foreach (Keyword kw in GetKeywords())
            {
                //TODO external file
                if (kw.Name == "cold") damagetypes.Add(kw.Name);
                else if (kw.Name == "force") damagetypes.Add(kw.Name);
                else if (kw.Name == "necrotic") damagetypes.Add(kw.Name);
                else if (kw.Name == "fire") damagetypes.Add(kw.Name);
                else if (kw.Name == "lightning") damagetypes.Add(kw.Name);
                else if (kw.Name == "thunder") damagetypes.Add(kw.Name);
                else if (kw.Name == "acid") damagetypes.Add(kw.Name);
                else if (kw.Name == "psychic") damagetypes.Add(kw.Name);
                else if (kw.Name == "poison") damagetypes.Add(kw.Name);
                else if (kw.Name == "radiant") damagetypes.Add(kw.Name);
            }
            return String.Join(", ",damagetypes);
        }
        public virtual List<Keyword> GetKeywords()
        {
            return Keywords;
        }
        public Spell Clone()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                Serializer.Serialize(mem, this);
                mem.Seek(0, SeekOrigin.Begin);
                Spell r = (Spell)Serializer.Deserialize(mem);
                r.Filename = Filename;
                return r;
            }
        }
        public static List<Spell> Filter(string expression)
        {
            if (expression == null || expression == "") expression = "true";
            try
            {
                Expression ex = new Expression(ConfigManager.FixQuotes(expression));
                Spell current = null;
                ex.EvaluateParameter += delegate (string name, ParameterArgs args)
                {
                    name = name.ToLowerInvariant();
                    if (name == "classlevel") args.Result = int.MaxValue;
                    else if (name == "classspelllevel") args.Result = int.MaxValue;
                    else if (name == "maxspellslot") args.Result = int.MaxValue;
                    else if (name == "name") args.Result = current.Name.ToLowerInvariant();
                    else if (name == "namelower") args.Result = current.Name.ToLowerInvariant();
                    else if (name == "level") args.Result = current.Level;
                    else if (current.Keywords.Count > 0 && current.Keywords.Exists(k => MatchesKW(k.Name, name))) args.Result = true;
                    else args.Result = false;
                };
                ex.EvaluateFunction += FunctionExtensions;
                List<Spell> res = new List<Spell>();
                foreach (Spell f in Spell.spells.Values)
                {
                    current = f;
                    object o = ex.Evaluate();
                    if (o is Boolean && (Boolean)o) res.Add(current);

                }
                res.Sort();
                return res;
            }
            catch (Exception e)
            {
                throw new Exception("Error while evaluating expression " + expression, e);
            }
        }
        private static void FunctionExtensions(string name, FunctionArgs args)
        {
            if (name.Equals("ClassLevel", StringComparison.OrdinalIgnoreCase))
            {
                args.Result = int.MaxValue;
            }
            if (name.Equals("SubClass", StringComparison.OrdinalIgnoreCase))
            {
                args.Result = "";
            }
        }
        private static bool MatchesKW(string kw, string kw2)
        {
            return kw.Replace('-', '_').Equals(kw2.Replace('-', '_'), StringComparison.OrdinalIgnoreCase);
        }
    }
}
