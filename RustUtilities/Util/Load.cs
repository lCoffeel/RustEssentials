/**
 * @file: Load.cs
 * @author: Team Cerionn (https://github.com/Team-Cerionn)
 * @version: 1.0.0.0
 * @description: Load class for Rust Essentials
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RustEssentials.Util
{
    public class Load
    {
        private string currentRank = "";
        private string currentPrefix = "";

        public void loadRanks()
        {
            // Replace this function with Pwn's Regex one-liner.
            try
            {
                if (File.Exists(Vars.ranksFile))
                {
                    Vars.rankPrefixes.Clear();
                    Vars.rankList.Clear();
                    using (StreamReader sr = new StreamReader(Vars.ranksFile))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (!line.StartsWith("#"))
                            {
                                if (line.LastIndexOf("#") > -1)
                                {
                                    line = line.Substring(0, line.LastIndexOf("#"));
                                }

                                line = line.Replace(" ", "");

                                if (line.StartsWith("[") && line.EndsWith("]"))
                                {
                                    if (line.Contains("."))
                                    {
                                        currentRank = line.Substring(1, line.IndexOf(".") - 1);
                                        currentPrefix = line.Substring(line.IndexOf(".") + 1, line.Length - line.IndexOf(".") - 2);
                                        Vars.rankPrefixes.Add(currentRank, "[" + currentPrefix + "]");
                                        Vars.rankList.Add(currentRank, new List<string>());
                                    }
                                    else
                                    {
                                        currentRank = line.Substring(1, line.Length - 2);
                                        Vars.rankList.Add(currentRank, new List<string>());
                                    }
                                }
                                else if (line.Equals("isDefaultRank"))
                                {
                                    Vars.defaultRank = currentRank;
                                }
                                else
                                {
                                    if (line.Length >= 17)
                                    {
                                        if (currentRank != "Member" && currentRank != Vars.defaultRank)
                                        {
                                            if (Vars.rankList.ContainsKey(currentRank))
                                                Vars.rankList[currentRank].Add(line);
                                            Vars.conLog.Info("Adding " + line + " as " + currentRank + ".");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Vars.conLog.Error(ex.ToString()); }
        }

        public void loadPrefixes()
        {
            // Replace this function with Pwn's Regex one-liners.
            try
            {
                if (File.Exists(Vars.prefixFile))
                {
                    Vars.playerPrefixes.Clear();
                    using (StreamReader sr = new StreamReader(Vars.prefixFile))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (!line.StartsWith("#"))
                            {
                                if (line.LastIndexOf("#") > -1)
                                {
                                    line = line.Substring(0, line.LastIndexOf("#"));
                                }

                                line = line.Trim();

                                if (line.Contains(":"))
                                {
                                    string UID = line.Split(':')[0];
                                    string prefix = line.Split(':')[1];

                                    if (!Vars.playerPrefixes.ContainsKey(UID))
                                        Vars.playerPrefixes.Add(UID, prefix);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Vars.conLog.Error(ex.ToString()); }
        }

        public void loadCommands()
        {
            // Replace this function with Pwn's Regex one-liners.
            try
            {
                if (File.Exists(Vars.commandsFile))
                {
                    Vars.enabledCommands.Clear();
                    Vars.totalCommands.Clear();
                    using (StreamReader sr = new StreamReader(Vars.commandsFile))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (!line.StartsWith("#"))
                            {
                                if (line.LastIndexOf("#") > -1)
                                {
                                    line = line.Substring(0, line.LastIndexOf("#"));
                                }

                                line = line.Trim();

                                if (line.StartsWith("[") && line.EndsWith("]"))
                                {
                                    currentRank = line.Substring(1, line.Length - 2);
                                    Vars.conLog.Info("Adding commands for [" + currentRank + "]...");
                                }
                                else
                                {
                                    if (line.StartsWith("/"))
                                    {
                                        if (Vars.enabledCommands.Keys.Contains(currentRank))
                                            Vars.enabledCommands[currentRank].Add(line);
                                        else
                                            Vars.enabledCommands.Add(currentRank, new List<string>(){ { line } });

                                        Vars.totalCommands.Add(line);
                                    }
                                }
                            }
                        }
                    }
                }
                if (Vars.inheritCommands)
                    inheritCommands();
            }
            catch (Exception ex) { Vars.conLog.Error(ex.ToString()); }
        }

        public void inheritCommands()
        {
            foreach (KeyValuePair<string, List<string>> kv in Vars.enabledCommands)
            {
                int indexOf = Vars.enabledCommands.Keys.ToList().IndexOf(kv.Key);
                foreach (KeyValuePair<string, List<string>> nkv in Vars.enabledCommands)
                {
                    int newIndexOf = Vars.enabledCommands.Keys.ToList().IndexOf(nkv.Key);
                    if (newIndexOf > indexOf)
                    {
                        foreach(string s in nkv.Value)
                        {
                            Vars.enabledCommands[kv.Key].Add(s);
                        }
                    }
                }
            }
            if (Vars.enabledCommands.Count > 0)
                Vars.conLog.Info("Commands inherited for each rank successfully!");
        }

        public string currentKit = "";
        public bool isSkipping = false;
        public void loadKits()
        {
            try
            {
                if (File.Exists(Vars.kitsFile))
                {
                    Vars.kits.Clear();
                    Vars.kitsForRanks.Clear();
                    foreach (KeyValuePair<string, string> kv in Vars.rankPrefixes)
                    {
                        if (kv.Key != Vars.defaultRank)
                        {
                            Vars.kitsForRanks.Add(kv.Key, new List<string>());
                        }
                    }
                    using (StreamReader sr = new StreamReader(Vars.kitsFile))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (!line.StartsWith("#"))
                            {
                                if (line.LastIndexOf("#") > -1)
                                {
                                    line = line.Substring(0, line.LastIndexOf("#"));
                                }

                                if (line.StartsWith("[") && line.EndsWith("]"))
                                {
                                    if (line.Contains("."))
                                    {
                                        currentKit = line.Substring(1, line.IndexOf(".") - 1);
                                        string prefix = line.Substring(line.IndexOf(".") + 1, line.Length - line.IndexOf(".") - 2);
                                        string rank = "";
                                        foreach(KeyValuePair<string, string> kv in Vars.rankPrefixes)
                                        {
                                            if (kv.Value == "[" + prefix + "]")
                                            {
                                                rank = kv.Key;
                                            }
                                        }
                                        if (Vars.rankPrefixes.Keys.Contains(rank))
                                        {
                                            Vars.kitsForRanks[rank].Add(currentKit.ToLower());
                                            Vars.kits.Add(currentKit.ToLower(), new Dictionary<string, int>());
                                            isSkipping = false;
                                            Vars.conLog.Info("Loading items for kit [" + currentKit + "]...");
                                        }
                                        else
                                        {
                                            Vars.conLog.Error("No such rank prefix " + prefix + ". Skipping kit [" + currentKit + "]...");
                                            isSkipping = true;
                                        }
                                    }
                                    else
                                    {
                                        currentKit = line.Substring(1, line.Length - 2);
                                        Vars.kits.Add(currentKit.ToLower(), new Dictionary<string, int>());
                                        Vars.unassignedKits.Add(currentKit.ToLower());
                                        isSkipping = false;
                                        Vars.conLog.Info("Loading items for kit [" + currentKit + "]...");
                                    }
                                }
                                else
                                {
                                    line = line.Trim();
                                    if (line.Contains(":") && !isSkipping)
                                    {
                                        string itemName = line.Split(':')[0];
                                        string amount = line.Split(':')[1];

                                        try
                                        {
                                            ItemDataBlock itemData = DatablockDictionary.GetByName(itemName);
                                            if (itemData != null)
                                            {
                                                try
                                                {
                                                    int itemAmount = Convert.ToInt16(amount);
                                                    Vars.kits[currentKit.ToLower()].Add(itemName, itemAmount);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Vars.conLog.Error("Something went wrong when loading kit [" + currentKit + "]. Skipping...");
                                                    isSkipping = true;
                                                }
                                            }
                                            else
                                            {
                                                Vars.conLog.Error("\"" + itemName + "\" [" + currentKit + "] is not a valid item name.");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Vars.conLog.Error("Something went wrong when loading kit [" + currentKit + "]. Skipping...");
                                            isSkipping = true;
                                        }
                                    }
                                    else if (line.Contains("=") && !isSkipping)
                                    {
                                        try
                                        {
                                            string cooldown = line.Split('=')[1];
                                            int multiplier = 1000;
                                            if (cooldown.EndsWith("m"))
                                                multiplier *= 60;
                                            if (cooldown.EndsWith("h"))
                                                multiplier *= 3600;
                                            cooldown = cooldown.Remove(cooldown.Length - 1);

                                            Vars.kitCooldowns.Add(currentKit.ToLower(), Convert.ToInt16(cooldown) * multiplier);
                                            Vars.conLog.Info("Time: "  + (Convert.ToInt16(cooldown) * multiplier));
                                        }
                                        catch (Exception ex)
                                        {
                                            Vars.conLog.Error("Something went wrong when loading kit [" + currentKit + "]. Skipping...");
                                            isSkipping = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (Vars.inheritKits)
                    inheritKits();
            }
            catch (Exception ex) { Vars.conLog.Error(ex.ToString()); }
        }

        public void inheritKits()
        {
            foreach (KeyValuePair<string, List<string>> kv in Vars.kitsForRanks)
            {
                foreach (KeyValuePair<string, List<string>> nkv in Vars.kitsForRanks)
                {
                    if (Vars.ofLowerRank(nkv.Key, kv.Key, true))
                    {
                        foreach (string s in nkv.Value)
                        {
                            Vars.kitsForRanks[kv.Key].Add(s);
                        }
                    }
                }
                foreach (KeyValuePair<string, Dictionary<string, int>> kv2 in Vars.kits)
                {
                    if (Vars.unassignedKits.Contains(kv2.Key))
                        Vars.kitsForRanks[kv.Key].Add(kv2.Key);
                }
            }
            if (Vars.enabledCommands.Count > 0)
                Vars.conLog.Info("Kits inherited for each rank successfully!");
        }

        private string currentMode = "";
        public void loadMOTD()
        {
            // Replace this function with Pwn's Regex one-liners.
            try
            {
                if (File.Exists(Vars.motdFile))
                {
                    Vars.motdList.Clear();
                    using (StreamReader sr = new StreamReader(Vars.motdFile))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (!line.StartsWith("#"))
                            {
                                if (line.LastIndexOf("#") > -1)
                                {
                                    line = line.Substring(0, line.LastIndexOf("#"));
                                }

                                if (line.StartsWith("[") && line.EndsWith("]"))
                                {
                                    if (line.Contains("."))
                                    {
                                        currentMode = line.Substring(1, line.IndexOf(".") - 1);
                                        string interval = line.Substring(line.IndexOf(".") + 1, line.Length - line.IndexOf(".") - 2);
                                        int multiplier = 1000;
                                        if (interval.EndsWith("m"))
                                            multiplier *= 60;
                                        if (interval.EndsWith("h"))
                                            multiplier *= 3600;
                                        try
                                        {
                                            Vars.cycleInterval = Convert.ToInt16(interval.Remove(interval.Length - 1)) * multiplier;
                                        }
                                        catch (Exception ex)
                                        {
                                            Vars.conLog.Error("Cycle Interval must be an integer! Defaulting to 15 minutes...");
                                        }
                                        Vars.motdList.Add(currentMode, new List<string>());
                                        Vars.conLog.Info("Adding MOTD [" + currentMode + "]...");
                                    }
                                    else
                                    {
                                        currentMode = line.Substring(1, line.Length - 2);
                                        Vars.motdList.Add(currentMode, new List<string>());
                                        Vars.conLog.Info("Adding MOTD [" + currentMode + "]...");
                                    }
                                }
                                else
                                {
                                    if (line.Length > 1)
                                    {
                                        Vars.motdList[currentMode].Add(line);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Vars.conLog.Error(ex.ToString()); }
        }

        public void loadConfig()
        {
            if (File.Exists(Vars.cfgFile))
            {
                Config.setVariables();

                try { Vars.enableWhitelist = Convert.ToBoolean(Config.enabledWhitelist); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("enableWhitelist could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }
                Vars.useMySQL = false;
                //try { Vars.useMySQL = Convert.ToBoolean(Config.MySQL); }
                //catch (Exception ex)
                //{
                //    Vars.conLog.Error("useMySQL could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                //}
                try { Vars.useSteamGroup = Convert.ToBoolean(Config.useSteamGroup); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("useSteamGroup could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }
                Vars.steamGroup = Config.steamGroup;
                try { Vars.autoRefresh = Convert.ToBoolean(Config.autoRefresh); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("autoRefresh could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }
                try { Vars.refreshInterval = Convert.ToInt16(Config.refreshInterval) * 1000; }
                catch (Exception ex)
                {
                    Vars.conLog.Error("refreshInterval could not be parsed as a number!");
                }
                try { Vars.whitelistToMembers = Convert.ToBoolean(Config.whitelistToMembers); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("whitelistToMembers could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }
                Vars.whitelistKickCMD = Config.whitelistKickCMD;
                Vars.whitelistKickJoin = Config.whitelistKickJoin;
                Vars.whitelistCheckGood = Config.whitelistCheckGood;
                Vars.whitelistCheckBad = Config.whitelistCheckBad;

                try { Vars.announceDrops = Convert.ToBoolean(Config.announceDrops); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("announceDrops could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }

                try { Vars.directChat = Convert.ToBoolean(Config.directChat); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("directChat could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }
                try { Vars.globalChat = Convert.ToBoolean(Config.globalChat); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("globalChat could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }
                try { Vars.removeTag = Convert.ToBoolean(Config.removeTag); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("removeTag could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }
                if (Vars.directChat)
                    Vars.removeTag = false;

                Vars.defaultChat = Config.defaultChat;
                if (!Vars.directChat && !Vars.globalChat)
                {
                    if (Vars.defaultChat == "direct" || Vars.defaultChat == "global")
                        Vars.conLog.Error("Both chat channels were disabled! Enabling channel defined as defaultChat...");
                    else
                    {
                        Vars.conLog.Error("Both chat channels were disabled and defaultChat was not a recognized channel!");
                        Vars.conLog.Error("Defaulting to direct...");
                    }
                }

                Vars.botName = Vars.replaceQuotes(Config.botName).Replace("\r\n", "");
                if (Config.joinMessage.Contains("$USER$"))
                    Vars.joinMessage = Vars.replaceQuotes(Config.joinMessage).Replace("\n", "");
                else
                    Vars.conLog.Error("Join Message does not contain $USER$! Defaulting to original...");
                try { Vars.enableJoin = Convert.ToBoolean(Config.enableJoin); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("enableJoin could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }

                if (Config.leaveMessage.Contains("$USER$"))
                    Vars.leaveMessage = Vars.replaceQuotes(Config.leaveMessage).Replace("\n", "");
                else
                    Vars.conLog.Error("Leave Message does not contain $USER$! Defaulting to original...");
                try { Vars.enableLeave = Convert.ToBoolean(Config.enableLeave); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("enableLeave could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }

                if (Config.suicideMessage.Contains("$VICTIM$"))
                    Vars.suicideMessage = Vars.replaceQuotes(Config.suicideMessage).Replace("\n", "");
                else
                    Vars.conLog.Error("Suicide Message does not contain $VICTIM$! Defaulting to original...");
                try { Vars.suicideMessages = Convert.ToBoolean(Config.enableSuicide); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("enableSuicide could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }

                if (Config.murderMessage.Contains("$VICTIM$") && Config.murderMessage.Contains("$KILLER$"))
                    Vars.murderMessage = Vars.replaceQuotes(Config.murderMessage).Replace("\n", "");
                else
                    Vars.conLog.Error("Murder Message must contain both $VICTIM$ and $KILLER$! Defaulting to original...");
                try { Vars.murderMessages = Convert.ToBoolean(Config.enableMurder); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("enableMurder could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }

                if (Config.deathMessage.Contains("$VICTIM$") && Config.deathMessage.Contains("$KILLER$"))
                    Vars.accidentMessage = Vars.replaceQuotes(Config.deathMessage).Replace("\n", "");
                else
                    Vars.conLog.Error("Death Message must contain both $VICTIM$ and $KILLER$! Defaulting to original...");
                try { Vars.accidentMessages = Convert.ToBoolean(Config.enableDeath); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("enableDeath could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }

                try { Vars.logPluginChat = Convert.ToBoolean(Config.logPluginChat); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("logPluginChat could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }
                try { Vars.chatLogCap = Convert.ToInt16(Config.chatLogCap); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("refreshInterval could not be parsed as a number!");
                }
                try { Vars.logCap = Convert.ToInt16(Config.logCap); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("refreshInterval could not be parsed as a number!");
                }
                try { Vars.unknownCommand = Convert.ToBoolean(Config.unknownCommand); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("unknownCommand could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }
                try { Vars.nextToName = Convert.ToBoolean(Config.nextToName); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("nextToName could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }
                try { Vars.removePrefix = Convert.ToBoolean(Config.removePrefix); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("removePrefix could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }

                try { Vars.teleportRequestOn = Convert.ToBoolean(Config.teleportRequest); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("teleportRequest could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }

                try { Vars.inheritCommands = Convert.ToBoolean(Config.inheritCommands); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("inheritCommands could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }
                try { Vars.inheritKits = Convert.ToBoolean(Config.inheritKits); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("inheritKits could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }

                Vars.conLog.Info("Config loaded.");
            }
            else
            {
                Vars.conLog.Error("Config was not found! Using defaults...");
            }
        }

        public void loadBans()
        {
            if (File.Exists(Vars.bansFile))
            {

                Dictionary<string, string> previousBans = new Dictionary<string, string>();
                Dictionary<string, string> previousBanReasons = new Dictionary<string, string>();
                using (StreamReader sr = new StreamReader(Vars.bansFile))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains("="))
                        {
                            string reason = line.Substring(line.IndexOf("#") + 1).Trim();
                            line = line.Substring(0, line.IndexOf("#")).Trim();
                            string playerName = line.Split('=')[0];
                            string playerUID = line.Split('=')[1];
                            previousBans.Add(playerName, playerUID);
                            previousBanReasons.Add(playerUID, reason);
                        }
                    }
                }

                Vars.currentBans = previousBans;
                Vars.currentBanReasons = previousBanReasons;
            }
        }
    }
}
