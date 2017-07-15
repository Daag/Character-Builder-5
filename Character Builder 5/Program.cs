﻿using Character_Builder;
using Character_Builder_Forms;
using Microsoft.Win32;
using OGL;
using OGL.Keywords;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Character_Builder_5
{
    public static class Program
    {
        public static void Exit()
        {
            Application.Exit();
        }
        public static ErrorLog Errorlog = null;
        public static Form1 MainWindow = null;
        public static bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        public static void Register()
        {
            if (IsRunAsAdmin())
            {
                string ext = ".cb5";
                RegistryKey key = Registry.ClassesRoot.CreateSubKey(ext);
                key.SetValue("", "Character Builder 5");
                key.Close();

                key = Registry.ClassesRoot.CreateSubKey(ext + "\\Shell\\Open\\command");
                //key = key.CreateSubKey("command");

                key.SetValue("", "\"" + Application.ExecutablePath + "\" \"%L\"");
                key.Close();

                key = Registry.ClassesRoot.CreateSubKey(ext + "\\DefaultIcon");
                key.SetValue("", Application.ExecutablePath);
                key.Close();
            }
        }

        public static void ReloadData()
        {
            Config.LoadConfig(Application.StartupPath);
            SourceManager.Init(Application.StartupPath, true);
            LoadData();
            Player.Current.ChoiceCounter.Clear();
            Player.Current.ChoiceTotal.Clear();
            MainWindow.BuildSources();
        }

        public static void LoadData() {
            PluginManager.manager = new PluginManager(Path.Combine(Application.StartupPath, ConfigManager.Directory_Plugins));
            ImportExtensions.LoadLevel(ImportExtensions.Fullpath(Application.StartupPath, ConfigManager.Loaded.Levels));
            ImportExtensions.ImportSkills();
            ImportExtensions.ImportLanguages();
            ImportExtensions.ImportSpells();
            ImportExtensions.ImportItems();
            ImportExtensions.ImportBackgrounds();
            ImportExtensions.ImportRaces();
            ImportExtensions.ImportSubRaces();
            ImportExtensions.ImportStandaloneFeatures();
            ImportExtensions.ImportConditions();
            ImportExtensions.ImportMagic();
            ImportExtensions.ImportClasses(true);
            ImportExtensions.ImportSubClasses(true);
        }

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        /// 
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ConfigManager.LogEvents += (sender, text, e) => Console.WriteLine((text != null ? text + ": " : "") + e?.StackTrace);
            string[] args = Environment.GetCommandLineArgs();
            Errorlog = new ErrorLog();
            Config.LoadConfig(Application.StartupPath);
            HTMLExtensions.LoadTransform += (t, o) => { if (o is Possession) t.Load(HTMLExtensions.Transform_Possession.FullName); };
            ConfigManager.LicenseProvider = new LicenseProvider();
            if (SourceManager.Init(Application.StartupPath, true))
            {
                if (args.Count() > 1)
                {
                    string file = args[1];
                    if (File.Exists(file) && ".pdf".Equals(Path.GetExtension(file), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PDF.markFields(file);
                        Application.Exit();
                        return;
                    }
                }
                LoadData();
            } else
            {
                Exit();
                return;
            }
            Player.SourcesChangedEvent += Player_SourcesChangedEvent;
            MainWindow = new Form1();
            if (args.Count() > 1)
            {
                string file = args[1];
                if (File.Exists(file))
                {
                    MainWindow.lastfile = file;
                    using (FileStream fs = new FileStream(file, FileMode.Open))
                    {
                        try
                        {
                            Player.Current = Player.Load(fs);
                            MainWindow.UpdateLayout();
                        }
                        catch (Exception e)
                        {
                            System.Windows.Forms.MessageBox.Show(e.Message + "\n" + e.InnerException + "\n" + e.StackTrace, "Error while loading "+file);
                            Program.Exit();
                        }
                    }
                }
            }
            if (args.Count() > 2 && args[2] == "register") Register();
            Application.Run(MainWindow);
        }

        private static void Player_SourcesChangedEvent(object sender, EventArgs e)
        {
            ReloadData();
        }

        public static void Resetglobals()
        {
            MainWindow.Resetglobals();
        }
    }
}
