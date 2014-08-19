using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Facepunch;
using UnityEngine;
using uLink;
using Rust;
using RustEssentials.Util;
using System.Net;
using System.Timers;
using System.IO.MemoryMappedFiles;
using System.Collections.Specialized;
using RustEssentials.Asm;
using System.Runtime.InteropServices;

namespace RustEssentials
{
    public class RustEssentialsBootstrap : Facepunch.MonoBehaviour
    {
        public static Util.Load _load = new Util.Load();

        public static void LoadEssentials()
        {
            try
            {
                Vars.conLog.Info("Loading RustEssentials...");
                new GameObject(typeof(RustEssentialsBootstrap).FullName).AddComponent(typeof(RustEssentialsBootstrap));
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("RustEssentials could not be loaded! Error: ");
                Vars.conLog.Error(ex.ToString());
            }
        }

        public void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        public void Start()
        {
            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);
            EmbeddedAssembly.Load("RustEssentials.Asm.System.Data.dll", "System.Data.dll");
            EmbeddedAssembly.Load("RustEssentials.Asm.Newtonsoft.Json.dll", "Newtonsoft.Json.dll");
            AppDomain.CurrentDomain.AssemblyResolve += resolveAssembly;

            Bundling.OnceLoaded += new Bundling.OnLoadedEventHandler(AssetsReady);

            Rust.Steam.Server.SetModded();
        }

        private static Assembly resolveAssembly(object sender, ResolveEventArgs args)
        {
            return EmbeddedAssembly.Get(args.Name);
        }

        public void Update()
        {
        }

        public void AssetsReady()
        {
            Vars.REB = this;

            Vars.conLog.Info("====");
            Vars.conLog.Info("MistaD's RustEssentials v" + Vars.currentVersion + " loaded!");
            Vars.conLog.Info("----");

            getVersion();
            getAssembly();
            _load.loadPathConfig();
            createFiles();
            Vars.conLog.startLogging();
            Vars.conLog.startLoggingChat();
            Vars.conLog.startLoggingStorage();
            Vars.conLog.startLoggingSleeperDeaths();
            _load.loadConfig();
            Vars.conLog.deleteLogs();
            Vars.conLog.deleteChatLogs();
            Vars.conLog.deleteStorageLogs();
            Vars.conLog.deleteSleeperDeathLogs();
            _load.loadRanks();
            _load.loadCommands();
            _load.loadBans();
            _load.loadPrefixes();
            Whitelist.Start();
            _load.loadKits();
            _load.loadMOTD();
            _load.loadWarps();
            _load.loadItems();
            _load.loadController();
            Data.readDoorData();
            Data.readRemoverData();
            Data.readFactionData();
            Data.readCooldownData();
            Data.readWarpCooldownData();
            Data.readZoneData();
            Data.readRequestData();
            Data.readRequestAllData();
            Data.readAlliesData();
            Data.readKillsData();
            Data.readDeathsData();
            Vars.originalLootTables = DatablockDictionary._lootSpawnLists;
            _load.loadTables();
            _load.loadDefaultLoadout();
            _load.loadDecay();
            Vars.threadMainUpdate();
            Vars.threadSecUpdate();
            Vars.threadSpeedHackUpdate();

            //Vars.conLog.Info("====");
            try
            {
                string apiDLL = Path.Combine(Vars.dllDir, "RustEssentialsAPI.dll");
                if (File.Exists(apiDLL))
                {
                    Vars.conLog.Info("Loading RustEssentialsAPI...");

                    Vars.API = Assembly.LoadFrom(apiDLL);
                    Type RustEssentialsAPI = Vars.API.GetType("RustEssentialsAPI.RustEssentialsAPI");
                    MethodInfo LoadAPI = RustEssentialsAPI.GetMethod("LoadAPI", BindingFlags.NonPublic | BindingFlags.Static);
                    LoadAPI.Invoke(null, null);
                    Vars.runningAPI = true;
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("RustEssentialsAPI could not be loaded! Error: ");
                Vars.conLog.Error(ex.ToString());
            }
            Vars.isReady = true;
        }

        private void getAssembly()
        {
            //Vars.conLog.Error("You are using an out of date version of Assembly-CSharp.dll!");
            //Vars.conLog.Error("Complete functionality not guaranteed.");
        }

        private void getVersion()
        {
            using (WebClient wc = new WebClient())
            {
                wc.Proxy = null;
                if (Vars.currentBuild == "")
                {
                    try
                    {
                        Vars.remoteVersion = wc.DownloadString("http://cdn.pwnoz0r.com/rust/mods/RustEssentials/ver.txt");

                            if (Vars.currentVersion != Vars.remoteVersion)
                            {
                                Vars.conLog.Warning("There is an update available for RustEssentials! Check the forum post for details.");
                            }
                            else
                            {
                                Vars.conLog.Info("RustEssentials up-to-date!");
                            }
                    }
                    catch (Exception ex)
                    {
                        Vars.conLog.Warning("Unable to parse latest version from server. Status unknown.");
                        Vars.remoteVersion = "?";
                    }
                }
                else
                    Vars.conLog.Info("You are currently running a dev build of RustEssentials. No version check needed.");
            }
        }

        private void createFiles()
        {
            try
            {
                if (!Vars.useDefaultPaths)
                {
                    Vars.cfgFile = Path.Combine(Vars.essentialsDir, "config.ini");
                    Vars.whiteListFile = Path.Combine(Vars.essentialsDir, "whitelist.txt");
                    Vars.ranksFile = Path.Combine(Vars.essentialsDir, "ranks.ini");
                    Vars.commandsFile = Path.Combine(Vars.essentialsDir, "commands.ini");
                    Vars.allCommandsFile = Path.Combine(Vars.essentialsDir, "allCommands.txt");
                    Vars.itemsFile = Path.Combine(Vars.essentialsDir, "itemIDs.txt");
                    Vars.kitsFile = Path.Combine(Vars.essentialsDir, "kits.ini");
                    Vars.defaultLoadoutFile = Path.Combine(Vars.essentialsDir, "default_loadout.ini");
                    Vars.motdFile = Path.Combine(Vars.essentialsDir, "motd.ini");
                    Vars.bansFile = Path.Combine(Vars.essentialsDir, "bans.ini");
                    Vars.prefixFile = Path.Combine(Vars.essentialsDir, "prefix.ini");
                    Vars.warpsFile = Path.Combine(Vars.essentialsDir, "warps.ini");
                    Vars.doorsFile = Path.Combine(Vars.essentialsDir, "door_data.dat");
                    Vars.removerDataFile = Path.Combine(Vars.essentialsDir, "remover_data.dat");
                    Vars.factionsFile = Path.Combine(Vars.essentialsDir, "factions.dat");
                    Vars.alliesFile = Path.Combine(Vars.essentialsDir, "allies.dat");
                    Vars.cooldownsFile = Path.Combine(Vars.essentialsDir, "kit_cooldowns.dat");
                    Vars.warpCooldownsFile = Path.Combine(Vars.essentialsDir, "warp_cooldowns.dat");
                    Vars.requestCooldownsFile = Path.Combine(Vars.essentialsDir, "tpaPer_cooldowns.dat");
                    Vars.requestCooldownsAllFile = Path.Combine(Vars.essentialsDir, "tpaAll_cooldowns.dat");
                    Vars.itemControllerFile = Path.Combine(Vars.essentialsDir, "controller.ini");
                    Vars.deathsFile = Path.Combine(Vars.essentialsDir, "deaths.dat");
                    Vars.decayFile = Path.Combine(Vars.essentialsDir, "decay.ini");
                    Vars.donorKitsFile = Path.Combine(Vars.essentialsDir, "donorkits.ini");

                    Vars.allDirs = new List<string>()
                    {
                        { Vars.saveDir },
                        { Vars.essentialsDir },
                        { Vars.logsDir },
                        { Vars.tablesDir },
                        { Vars.bigBrotherDir },
                        { Vars.storageLogsDir },
                        { Vars.sleeperDeathLogsDir }
                    };

                    Vars.allFiles = new List<string>()
                    {
                        { Vars.cfgFile },
                        { Vars.whiteListFile },
                        { Vars.ranksFile },
                        { Vars.commandsFile },
                        { Vars.itemsFile },
                        { Vars.allCommandsFile },
                        { Vars.kitsFile },
                        { Vars.defaultLoadoutFile },
                        { Vars.motdFile },
                        { Vars.doorsFile },
                        { Vars.removerDataFile },
                        { Vars.factionsFile },
                        { Vars.alliesFile },
                        { Vars.cooldownsFile },
                        { Vars.warpCooldownsFile },
                        { Vars.requestCooldownsFile },
                        { Vars.requestCooldownsAllFile },
                        { Vars.bansFile },
                        { Vars.prefixFile },
                        { Vars.warpsFile },
                        { Vars.zonesFile },
                        { Vars.itemControllerFile },
                        { Vars.killsFile },
                        { Vars.deathsFile },
                        { Vars.decayFile },
                        //{ Vars.donorKitsFile },
                        { Vars.pathsFile }
                    };

                    Vars.textForFiles = new Dictionary<string, StringBuilder>()
                    {
                        { Vars.cfgFile, FileTexts.cfgText() },
                        { Vars.ranksFile, FileTexts.ranksText() },
                        { Vars.commandsFile, FileTexts.commandsText() },
                        { Vars.allCommandsFile, FileTexts.allCommandsText() },
                        { Vars.kitsFile, FileTexts.kitsText() },
                        { Vars.defaultLoadoutFile, FileTexts.defaultLoadoutText() },
                        { Vars.motdFile, FileTexts.motdText() },
                        { Vars.prefixFile, FileTexts.prefixText() },
                        { Vars.warpsFile, FileTexts.warpsText() },
                        { Vars.itemControllerFile, FileTexts.itemControllerText() },
                        { Vars.decayFile, FileTexts.decayText() },
                        //{ Vars.donorKitsFile, FileTexts.donorKitsText() },
                        { Vars.pathsFile, FileTexts.pathsText() }
                    };
                }

                foreach (string s in Vars.allDirs)
                {
                    if (!Directory.Exists(s))
                    {
                        Directory.CreateDirectory(s);
                    }
                }
                
                foreach (string s in Vars.allFiles)
                {
                    if (!File.Exists(s))
                    {
                        File.Create(s).Close();
                        if (Vars.textForFiles.Keys.Contains(s))
                        {
                            File.WriteAllText(s, Vars.textForFiles[s].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("CF: " + ex.ToString());
            }
        }

        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

        public delegate bool HandlerRoutine(CtrlTypes CtrlType);
        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        /// <summary>
        /// Handle certain types of control events.
        /// </summary>
        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            switch (ctrlType)
            {
                case CtrlTypes.CTRL_C_EVENT: // CTRL+C
                    Vars.shuttingDown("Ctrl+C Executed.");
                    break;

                case CtrlTypes.CTRL_BREAK_EVENT: // Exited main loop
                    Vars.shuttingDown("Main Loop Exited.");
                    break;

                case CtrlTypes.CTRL_CLOSE_EVENT: // Clicked the X
                    Vars.shuttingDown("Clicked X.");
                    break;

                case CtrlTypes.CTRL_LOGOFF_EVENT: // Logging out of system
                    Vars.shuttingDown("System Log Off.");
                    break;

                case CtrlTypes.CTRL_SHUTDOWN_EVENT: // Shutting down system
                    Vars.shuttingDown("System Shutdown.");
                    break;

                default: // Unhandled
                    Vars.shuttingDown("Unhandled Shutdown.");
                    break;
            }
            return true;
        }
    }
}
