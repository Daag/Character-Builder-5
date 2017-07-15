﻿using Character_Builder_Forms;
using OGL;
using OGL.Features;
using System;
using System.IO;

namespace Character_Builder_Builder
{
    public static class SaveExtensions
    {
        public static void Save(this AbilityScores a, String file)
        {
            using (TextWriter writer = new StreamWriter(file)) AbilityScores.Serializer.Serialize(writer, a);
        }
        public static void SaveAbilityScores()
        {
            AbilityScores.Current.Save(AbilityScores.Current.Filename);
        }
        public static bool Save(this Background background, Boolean overwrite)
        {
            background.Name = background.Name.Replace(ConfigManager.SourceSeperator, '-');
            FileInfo file = SourceManager.GetFileName(background.Name, background.Source, ConfigManager.Directory_Backgrounds);
            if (file.Exists && (background.Filename == null || !background.Filename.Equals(file.FullName)) && !overwrite) return false;
            using (TextWriter writer = new StreamWriter(file.FullName)) Background.Serializer.Serialize(writer, background);
            background.Filename = file.FullName;
            return true;
        }
        public static bool Save(this ClassDefinition clz, Boolean overwrite)
        {
            clz.Name = clz.Name.Replace(ConfigManager.SourceSeperator, '-');
            FileInfo file = SourceManager.GetFileName(clz.Name, clz.Source, ConfigManager.Directory_Classes);
            if (file.Exists && (clz.filename == null || !clz.filename.Equals(file.FullName)) && !overwrite) return false;
            using (TextWriter writer = new StreamWriter(file.FullName)) ClassDefinition.Serializer.Serialize(writer, clz);
            clz.filename = file.FullName;
            return true;
        }
        public static bool Save(this Condition cond, Boolean overwrite)
        {
            cond.Name = cond.Name.Replace(ConfigManager.SourceSeperator, '-');
            FileInfo file = SourceManager.GetFileName(cond.Name, cond.Source, ConfigManager.Directory_Conditions);
            if (file.Exists && (cond.filename == null || !cond.filename.Equals(file.FullName)) && !overwrite) return false;
            using (TextWriter writer = new StreamWriter(file.FullName)) Condition.Serializer.Serialize(writer, cond);
            cond.filename = file.FullName;
            return true;
        }
        public static bool Save(this FeatureContainer f, Boolean overwrite)
        {
            FileInfo file = SourceManager.GetFileName(f.Name, f.Source, f.category);
            if (file.Exists && (f.filename == null || !f.filename.Equals(file.FullName)) && !overwrite) return false;
            using (TextWriter writer = new StreamWriter(file.FullName)) FeatureContainer.Serializer.Serialize(writer, f);
            f.filename = file.FullName;
            return true;
        }
        public static void Save(this FeatureContainer f, String path)
        {
            using (TextWriter writer = new StreamWriter(path))
            {
                FeatureContainer.Serializer.Serialize(writer, f);
            }

        }
        public static void Save(this Feature f, String path)
        {
            new FeatureContainer(f).Save(path);
        }
        public static bool Save(this Item i, Boolean overwrite)
        {

            Item o = null;
            i.Name = i.Name.Replace(ConfigManager.SourceSeperator, '-');
            if (Item.items.ContainsKey(i.Name + " " + ConfigManager.SourceSeperator + " " + i.Source)) o = Item.items[i.Name + " " + ConfigManager.SourceSeperator + " " + i.Source];
            if (o != null && !o.autogenerated && o.Category.Path != i.Category.Path)
            {
                throw new Exception("Item needs a unique name");
            }
            FileInfo file = SourceManager.GetFileName(i.Name, i.Source, Path.Combine(ConfigManager.Directory_Items, Path.Combine(i.Category.MakePath().ToArray())));
            if (file.Exists && (i.filename == null || !i.filename.Equals(file.FullName)) && !overwrite) return false;
            using (TextWriter writer = new StreamWriter(file.FullName)) Item.Serializer.Serialize(writer, i);
            i.filename = file.FullName;
            return true;
        }
        public static bool Save(this Language l, Boolean overwrite)
        {
            l.Name = l.Name.Replace(ConfigManager.SourceSeperator, '-');
            FileInfo file = SourceManager.GetFileName(l.Name, l.Source, ConfigManager.Directory_Languages);
            if (file.Exists && (l.filename == null || !l.filename.Equals(file.FullName)) && !overwrite) return false;
            using (TextWriter writer = new StreamWriter(file.FullName)) Language.Serializer.Serialize(writer, l);
            l.filename = file.FullName;
            return true;
        }
        public static void Save(this Level l, String file)
        {
            using (TextWriter writer = new StreamWriter(file)) Level.Serializer.Serialize(writer, l);
        }
        public static bool Save(this MagicProperty mp, Boolean overwrite)
        {
            mp.Name = mp.Name.Replace(ConfigManager.SourceSeperator, '-');
            MagicProperty o = null;
            if (MagicProperty.properties.ContainsKey(mp.Name + " " + ConfigManager.SourceSeperator + " " + mp.Source)) o = MagicProperty.properties[mp.Name + " " + ConfigManager.SourceSeperator + " " + mp.Source];
            if (o != null && o.Category != mp.Category)
            {
                throw new Exception("Magic Property needs a unique name");
            }
            FileInfo file = SourceManager.GetFileName(mp.Name, mp.Source, ImportExtensions.MagicPropertyCleanname(mp.Category));
            if (file.Exists && (mp.Filename == null || !mp.Filename.Equals(file.FullName)) && !overwrite) return false;
            using (TextWriter writer = new StreamWriter(file.FullName)) MagicProperty.Serializer.Serialize(writer, mp);
            mp.Filename = file.FullName;
            return true;
        }
        public static bool Save(this Race r, Boolean overwrite)
        {
            r.Name = r.Name.Replace(ConfigManager.SourceSeperator, '-');
            FileInfo file = SourceManager.GetFileName(r.Name, r.Source, ConfigManager.Directory_Races);
            if (file.Exists && (r.Filename == null || !r.Filename.Equals(file.FullName)) && !overwrite) return false;
            using (TextWriter writer = new StreamWriter(file.FullName)) Race.Serializer.Serialize(writer, r);
            r.Filename = file.FullName;
            return true;
        }
        public static bool Save(this Skill s, Boolean overwrite)
        {
            s.Name = s.Name.Replace(ConfigManager.SourceSeperator, '-');
            FileInfo file = SourceManager.GetFileName(s.Name, s.Source, ConfigManager.Directory_Skills);
            if (file.Exists && (s.Filename == null || !s.Filename.Equals(file.FullName)) && !overwrite) return false;
            using (TextWriter writer = new StreamWriter(file.FullName)) Skill.Serializer.Serialize(writer, s);
            s.Filename = file.FullName;
            return true;
        }
        public static bool Save(this Spell s, Boolean overwrite)
        {
            s.Name = s.Name.Replace(ConfigManager.SourceSeperator, '-');
            FileInfo file = SourceManager.GetFileName(s.Name, s.Source, ConfigManager.Directory_Spells);
            if (file.Exists && (s.Filename == null || !s.Filename.Equals(file.FullName)) && !overwrite) return false;
            using (TextWriter writer = new StreamWriter(file.FullName)) Spell.Serializer.Serialize(writer, s);
            s.Filename = file.FullName;
            return true;
        }
        public static bool Save(this SubClass s, Boolean overwrite)
        {
            s.Name = s.Name.Replace(ConfigManager.SourceSeperator, '-');
            FileInfo file = SourceManager.GetFileName(s.Name, s.Source, ConfigManager.Directory_SubClasses);
            if (file.Exists && (s.Filename == null || !s.Filename.Equals(file.FullName)) && !overwrite) return false;
            using (TextWriter writer = new StreamWriter(file.FullName)) SubClass.Serializer.Serialize(writer, s);
            s.Filename = file.FullName;
            return true;
        }
        public static bool Save(this SubRace r, Boolean overwrite)
        {
            r.Name = r.Name.Replace(ConfigManager.SourceSeperator, '-');
            FileInfo file = SourceManager.GetFileName(r.Name, r.Source, ConfigManager.Directory_SubRaces);
            if (file.Exists && (r.Filename == null || !r.Filename.Equals(file.FullName)) && !overwrite) return false;
            using (TextWriter writer = new StreamWriter(file.FullName)) SubRace.Serializer.Serialize(writer, r);
            r.Filename = file.FullName;
            return true;
        }
    }
}
