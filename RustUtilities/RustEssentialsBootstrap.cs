/**
 * @file: RustEssentialsBootstrap.cs
 * @author: Team Cerionn (https://github.com/Team-Cerionn)

 * @description: Bootstrap class for Rust Essentials
 */
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
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Specialized;

namespace RustEssentials
{
    public class RustEssentialsBootstrap : Facepunch.MonoBehaviour
    {
        public static Util.Load _load = new Util.Load();

        public static void LoadEssentials()
        {
            Vars.conLog.Info("Loading RustEssentials...");
            try
            {
                new GameObject(typeof(RustEssentialsBootstrap).FullName).AddComponent(typeof(RustEssentialsBootstrap));
                Vars.conLog.Info("RustEssentials loaded! Processing...");
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
            createFiles();

            Bundling.OnceLoaded += new Bundling.OnLoadedEventHandler(AssetsReady);
            
            Rust.Steam.Server.SetModded();
        }

        public void AssetsReady()
        {
            Vars.conLog.Info("====");
            Vars.conLog.Info("Cerionn's Rust Essentials v" + Vars.currentVersion + " loaded!");
            Vars.conLog.Info("----");

            getVersion();
            getAssembly();
            Vars.conLog.startLogging();
            Vars.conLog.startLoggingChat();
            _load.loadConfig();
            Vars.conLog.deleteLogs();
            Vars.conLog.deleteChatLogs();
            _load.loadRanks();
            _load.loadCommands();
            _load.loadBans();
            _load.loadPrefixes();
            Whitelist.Start();
            _load.loadKits();
            _load.loadMOTD();
            _load.loadWarps();
            Vars.loadItems();
            _load.loadController();
            Vars.cycleMOTD();
            Vars.onceMOTD();
            Vars.readDoorData(); 
            Vars.readFactionData();
            Vars.readCooldownData();
            Vars.readZoneData();
            Vars.readRequestData();
            Vars.readRequestAllData();
            Vars.readAlliesData();
            Vars.loopKitSaving();
            Vars.zoneTimer();
            Vars.loopRequestSaving();
            Vars.loopNudity();
            Vars.loopItems();
            Vars.originalLootTables = DatablockDictionary._lootSpawnLists;
            _load.loadTables();

            Vars.conLog.Info("====");
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
                try
                {
                    Vars.remoteVersion = wc.DownloadString("http://cdn.pwnoz0r.com/rust/mods/RustEssentials/ver.txt");

                    if (Vars.currentVersion != Vars.remoteVersion)
                    {
                        Vars.conLog.Warning("There is an update available for Rust Essentials! Check the forum post for details.");
                    }
                    else
                    {
                        Vars.conLog.Info("Rust Essentials up-to-date!");
                    }
                }
                catch (Exception ex)
                {
                    Vars.conLog.Warning("Unable to parse latest version from server. Status unknown.");
                    Vars.remoteVersion = "?";
                }

            }
        }

        private void createFiles()
        {
            try
            {
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
                Vars.conLog.Error(ex.ToString());
            }
        }
    }
}
