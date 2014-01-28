/**
 * @file: RustEssentialsBootstrap.cs
 * @author: Team Cerionn (https://github.com/Team-Cerionn)
 * @version: 1.0.0.0
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
using LeatherLoader;
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
    [Bootstrap]
    public class RustEssentialsBootstrap : Facepunch.MonoBehaviour
    {
        public static Util.Load _load = new Util.Load();

        public void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        private string setLeatherModsDir;

        public void ReceiveLeatherConfiguration(LeatherConfig config)
        {
            setLeatherModsDir = config.ConfigDirectoryPath;
        }

        public void Start()
        {
            if (!Directory.Exists(setLeatherModsDir))
            {
                Directory.CreateDirectory(setLeatherModsDir);
            }

            createFiles();

            Bundling.OnceLoaded += new Bundling.OnLoadedEventHandler(AssetsReady);

            Rust.Steam.Server.SetModded();
        }

        public void AssetsReady()
        {
            Vars.conLog.Info("====");
            Vars.conLog.Info("Cerionn's Rust Essentials " + Vars.currentVersion + " loaded!");
            Vars.conLog.Info("----");

            getVersion();
            getAssembly();
            Vars.conLog.startLogging();
            Vars.conLog.startLoggingChat();
            _load.loadConfig();
            Vars.conLog.deleteLogs();
            Vars.conLog.deleteChatLogs();
            _load.loadRanks();
            Whitelist.Start();
            _load.loadCommands();
            _load.loadKits();
            _load.loadMOTD();
            Vars.loadItems();
            Vars.cycleMOTD();
            Vars.readDoorData();
            Vars.readFactionData();

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
