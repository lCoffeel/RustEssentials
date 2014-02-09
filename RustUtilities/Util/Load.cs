/**
 * @file: Load.cs
 * @author: Team Cerionn (https://github.com/Team-Cerionn)

 * @description: Load class for Rust Essentials
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

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
                                if (line.IndexOf("#") > -1)
                                {
                                    line = line.Substring(0, line.IndexOf("#"));
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
                                if (line.IndexOf("#") > -1)
                                {
                                    line = line.Substring(0, line.IndexOf("#"));
                                }

                                line = line.Trim();

                                if (line.Contains(":"))
                                {
                                    string UID = line.Split(':')[0];
                                    string prefix = line.Split(':')[1];

                                    if (!Vars.playerPrefixes.ContainsKey(UID))
                                        Vars.playerPrefixes.Add(UID, prefix);
                                }
                                else
                                {
                                    string UID = line.Trim();

                                    if (!Vars.emptyPrefixes.Contains(UID))
                                        Vars.emptyPrefixes.Add(UID);
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
                                if (line.IndexOf("#") > -1)
                                {
                                    line = line.Substring(0, line.IndexOf("#"));
                                }

                                line = line.Trim();

                                if (line.StartsWith("[") && line.EndsWith("]"))
                                {
                                    currentRank = line.Substring(1, line.Length - 2);
                                    if (!Vars.enabledCommands.Keys.Contains(currentRank))
                                    {
                                        Vars.enabledCommands.Add(currentRank, new List<string>());
                                        Vars.conLog.Info("Adding commands for [" + currentRank + "]...");
                                    }
                                    else
                                    {
                                        Vars.conLog.Error("Rank [" + currentRank + "] already exists!");
                                    }
                                }
                                else
                                {
                                    if (line.StartsWith("/"))
                                    {
                                        if (Vars.enabledCommands.Keys.Contains(currentRank))
                                            Vars.enabledCommands[currentRank].Add(line);

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
                    Vars.kitCooldowns.Clear();
                    Vars.kits.Clear();
                    Vars.kitsForRanks.Clear();
                    Vars.kitsForUIDs.Clear();
                    Vars.unassignedKits.Clear();
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
                                if (line.IndexOf("#") > -1)
                                {
                                    line = line.Substring(0, line.IndexOf("#"));
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
                                            long UID;
                                            if (prefix.Length == 17 && long.TryParse(prefix, out UID))
                                            {
                                                if (!Vars.kitsForUIDs.ContainsKey(UID.ToString()))
                                                    Vars.kitsForUIDs.Add(UID.ToString(), new List<string>() { { currentKit.ToLower() } });
                                                else
                                                    Vars.kitsForUIDs[UID.ToString()].Add(currentKit.ToLower());

                                                if (!Vars.kits.ContainsKey(currentKit.ToLower()))
                                                    Vars.kits.Add(currentKit.ToLower(), new Dictionary<string, int>());

                                                isSkipping = false;
                                                Vars.conLog.Info("Loading items for kit [" + currentKit + "] for user [" + UID.ToString() + "]...");
                                            }
                                            else
                                            {
                                                Vars.conLog.Error("No such rank prefix " + prefix + ". Skipping kit [" + currentKit + "]...");
                                                isSkipping = true;
                                            }
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
            Vars.conLog.Info("Kits inherited for each rank successfully!");
        }

        public string currentWarp = "";
        public bool isSkippingWarp = false;
        public void loadWarps()
        {
            try
            {
                if (File.Exists(Vars.warpsFile))
                {
                    Vars.warpCooldowns.Clear();
                    Vars.warps.Clear();
                    Vars.warpsForRanks.Clear();
                    Vars.warpsForUIDs.Clear();
                    Vars.unassignedWarps.Clear();
                    foreach (KeyValuePair<string, string> kv in Vars.rankPrefixes)
                    {
                        if (kv.Key != Vars.defaultRank)
                        {
                            Vars.warpsForRanks.Add(kv.Key, new List<string>());
                        }
                    }
                    using (StreamReader sr = new StreamReader(Vars.warpsFile))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (!line.StartsWith("#"))
                            {
                                if (line.IndexOf("#") > -1)
                                {
                                    line = line.Substring(0, line.IndexOf("#"));
                                }

                                if (line.StartsWith("[") && line.EndsWith("]"))
                                {
                                    if (line.Contains("."))
                                    {
                                        currentKit = line.Substring(1, line.IndexOf(".") - 1);
                                        string prefix = line.Substring(line.IndexOf(".") + 1, line.Length - line.IndexOf(".") - 2);
                                        string rank = "";
                                        foreach (KeyValuePair<string, string> kv in Vars.rankPrefixes)
                                        {
                                            if (kv.Value == "[" + prefix + "]")
                                            {
                                                rank = kv.Key;
                                            }
                                        }
                                        if (Vars.rankPrefixes.Keys.Contains(rank))
                                        {
                                            Vars.warpsForRanks[rank].Add(currentWarp.ToLower());
                                            Vars.warps.Add(currentWarp.ToLower(), new Vector3());
                                            isSkippingWarp = false;
                                            Vars.conLog.Info("Loading location for warp [" + currentWarp + "]...");
                                        }
                                        else
                                        {
                                            long UID;
                                            if (prefix.Length == 17 && long.TryParse(prefix, out UID))
                                            {
                                                if (!Vars.warpsForUIDs.ContainsKey(UID.ToString()))
                                                    Vars.warpsForUIDs.Add(UID.ToString(), new List<string>() { { currentWarp.ToLower() } });
                                                else
                                                    Vars.warpsForUIDs[UID.ToString()].Add(currentWarp.ToLower());

                                                if (!Vars.warps.ContainsKey(currentWarp.ToLower()))
                                                    Vars.warps.Add(currentWarp.ToLower(), new Vector3());
                                                isSkippingWarp = false;
                                                Vars.conLog.Info("Loading location for warp [" + currentWarp + "] for user [" + UID.ToString() + "]...");
                                            }
                                            else
                                            {
                                                Vars.conLog.Error("No such rank prefix " + prefix + ". Skipping warp [" + currentWarp + "]...");
                                                isSkippingWarp = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        currentWarp = line.Substring(1, line.Length - 2);
                                        Vars.warps.Add(currentWarp.ToLower(), new Vector3());
                                        Vars.unassignedWarps.Add(currentWarp.ToLower());
                                        isSkippingWarp = false;
                                        Vars.conLog.Info("Loading location for warp [" + currentWarp + "]...");
                                    }
                                }
                                else
                                {
                                    line = line.Trim();
                                    if (line.Contains("(") && line.Contains(",") && line.Contains(")") && !isSkipping)
                                    {
                                        line = line.Replace("(", "").Replace(")", "").Replace(" ", "");
                                        string xposStr = line.Split(',')[0];
                                        string yposStr = line.Split(',')[1];
                                        string zposStr = line.Split(',')[2];
                                        float xpos;
                                        float ypos;
                                        float zpos;

                                        if (float.TryParse(xposStr, out xpos) && float.TryParse(yposStr, out ypos) && float.TryParse(zposStr, out zpos))
                                        {
                                            Vector3 warpLocation = new Vector3(xpos, ypos, zpos);
                                            Vars.warps[currentWarp.ToLower()] = warpLocation;
                                        }
                                        else
                                        {
                                            Vars.conLog.Error("Something went wrong when loading warp [" + currentWarp + "]. Skipping...");
                                            isSkippingWarp = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (Vars.inheritWarps)
                    inheritWarps();
            }
            catch (Exception ex) { Vars.conLog.Error(ex.ToString()); }
        }

        public void inheritWarps()
        {
            foreach (KeyValuePair<string, List<string>> kv in Vars.warpsForRanks)
            {
                foreach (KeyValuePair<string, List<string>> nkv in Vars.warpsForRanks)
                {
                    if (Vars.ofLowerRank(nkv.Key, kv.Key, true))
                    {
                        foreach (string s in nkv.Value)
                        {
                            Vars.kitsForRanks[kv.Key].Add(s);
                        }
                    }
                }
                foreach (KeyValuePair<string, Vector3> kv2 in Vars.warps)
                {
                    if (Vars.unassignedWarps.Contains(kv2.Key))
                        Vars.warpsForRanks[kv.Key].Add(kv2.Key);
                }
            }
            Vars.conLog.Info("Warps inherited for each rank successfully!");
        }

        private string currentMode = "";
        private int currentInstance = 0;
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
                        int lineNumber = 0;
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            lineNumber++;
                            if (!line.StartsWith("#"))
                            {
                                if (line.IndexOf("#") > -1)
                                {
                                    line = line.Substring(0, line.IndexOf("#"));
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
                                        if (currentMode == "Cycle")
                                        {
                                            int instances = Vars.cycleMOTDList.Count() + 1;
                                            currentInstance = instances;
                                            Vars.conLog.Info("Adding MOTD [" + currentMode + "]...");
                                            try
                                            {
                                                Vars.cycleMOTDList.Add(currentMode + instances, new Dictionary<string, List<string>>() { { (Convert.ToInt16(interval.Remove(interval.Length - 1)) * multiplier).ToString(), new List<string>() } });

                                            }
                                            catch (Exception ex)
                                            {
                                                Vars.conLog.Error("Cycle Interval must be an integer! Defaulting to 15 minutes...");
                                                Vars.cycleMOTDList.Add(currentMode + instances, new Dictionary<string, List<string>>() { { "900000", new List<string>() } });
                                            }
                                        }
                                        else if (currentMode == "Once")
                                        {
                                            try
                                            {
                                                int instances = Vars.onceMOTDList.Count() + 1;
                                                currentInstance = instances;
                                                Vars.onceMOTDList.Add(currentMode + instances, new Dictionary<string, List<string>>() { { (Convert.ToInt16(interval.Remove(interval.Length - 1)) * multiplier).ToString(), new List<string>() } });

                                                Vars.conLog.Info("Adding MOTD [" + currentMode + "]...");
                                            }
                                            catch (Exception ex)
                                            {
                                                Vars.conLog.Error("Once Interval must be an integer on line " + lineNumber + "! Skipping...");
                                            }
                                        }
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
                                        if (Vars.cycleMOTDList.ContainsKey(currentMode + currentInstance))
                                        {
                                            Vars.cycleMOTDList[currentMode + currentInstance].ElementAt(0).Value.Add(line);
                                        }
                                        else if (Vars.onceMOTDList.ContainsKey(currentMode + currentInstance))
                                        {
                                            Vars.onceMOTDList[currentMode + currentInstance].ElementAt(0).Value.Add(line);
                                        }
                                        else
                                        {
                                            Vars.motdList[currentMode].Add(line);
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
                Vars.steamGroup = Config.steamGroup.Replace("\r\n", "").Replace("\n", "");
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

                try { Vars.fallDamage = Convert.ToBoolean(Config.fallDamage); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("fallDamage could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }
                try { voice.distance = (float)Convert.ToInt16(Config.voiceDistance); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("voiceDistance could not be parsed as a number!");
                }
                try { Vars.enableRepair = Convert.ToBoolean(Config.enableRepair); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("enableRepair could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }
                try { Vars.forceNudity = Convert.ToBoolean(Config.forceNudity); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("forceNudity could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
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
                Vars.allowedChars = Config.allowedChars.Replace("\n", "").Split(',').ToList();
                try { Vars.restrictChars = Convert.ToBoolean(Config.restrictChars); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("restrictChars could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }
                try { Vars.minimumNameCount = Convert.ToInt16(Config.minimumNameCount); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("minimumNameCount could not be parsed as a number!");
                }
                try { Vars.maximumNameCount = Convert.ToInt16(Config.maximumNameCount); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("maximumNameCount could not be parsed as a number!");
                }
                try { Vars.kickDuplicate = Convert.ToBoolean(Config.kickDuplicate); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("kickDuplicate could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }
                try { Vars.lowerAuthority = Convert.ToBoolean(Config.lowerAuthority); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("lowerAuthority could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }
                Vars.illegalWords = Config.illegalWords.Replace("\n", "").Split(',').ToList();
                try { Vars.censorship = Convert.ToBoolean(Config.censorship); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("censorship could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }

                Vars.botName = Vars.replaceQuotes(Config.botName);
                Vars.joinMessage = Vars.replaceQuotes(Config.joinMessage).Replace("\n", "");
                try { Vars.enableJoin = Convert.ToBoolean(Config.enableJoin); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("enableJoin could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }

                Vars.leaveMessage = Vars.replaceQuotes(Config.leaveMessage).Replace("\n", "");
                try { Vars.enableLeave = Convert.ToBoolean(Config.enableLeave); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("enableLeave could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }

                Vars.suicideMessage = Vars.replaceQuotes(Config.suicideMessage).Replace("\n", "");
                try { Vars.suicideMessages = Convert.ToBoolean(Config.enableSuicide); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("enableSuicide could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }

                Vars.murderMessage = Vars.replaceQuotes(Config.murderMessage).Replace("\n", "");
                if (Config.murderMessageUnknown.Contains("$VICTIM$") && Config.murderMessageUnknown.Contains("$KILLER$"))
                    Vars.murderMessageUnknown = Vars.replaceQuotes(Config.murderMessageUnknown).Replace("\n", "");
                else
                    Vars.conLog.Error("Murder Message Unknown must contain both $VICTIM$ and $KILLER$! Defaulting to original...");
                try { Vars.murderMessages = Convert.ToBoolean(Config.enableMurder); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("enableMurder could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }

                Vars.accidentMessage = Vars.replaceQuotes(Config.deathMessage).Replace("\n", "");
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
                try { Vars.requestDelay = Convert.ToInt16(Config.requestDelay); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("requestDelay could not be parsed as a number!");
                }
                try { Vars.warpDelay = Convert.ToInt16(Config.warpDelay); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("warpDelay could not be parsed as a number!");
                }
                try { Vars.requestCooldownType = Convert.ToInt16(Config.requestCooldownType); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("requestCooldownType could not be parsed as a number!");
                }
                try
                {
                    int number = Convert.ToInt16(Config.requestCooldown.Substring(0, Config.requestCooldown.Length - 1));
                    int multiplier = 1000;
                    if (Config.requestCooldown.EndsWith("m"))
                        multiplier *= 60;
                    Vars.requestCooldown = number * multiplier; 
                }
                catch (Exception ex)
                {
                    Vars.conLog.Error("requestCooldown could not be parsed!");
                }
                try { Vars.denyRequestWarzone = Convert.ToBoolean(Config.denyRequestWarzone); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("denyRequestWarzone could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
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
                try { Vars.inheritWarps = Convert.ToBoolean(Config.inheritWarps); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("inheritWarps could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }

                try { Vars.friendlyFire = Convert.ToBoolean(Config.friendlyFire); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("friendlyFire could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }
                try { Vars.alliedFire = Convert.ToBoolean(Config.alliedFire); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("alliedFire could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                }
                try { Vars.neutralDamage = Convert.ToInt16(Config.neutralDamage); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("neutralDamage could not be parsed as a number!");
                }
                try { Vars.warDamage = Convert.ToInt16(Config.warDamage); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("warDamage could not be parsed as a number!");
                }
                try { Vars.warFriendlyDamage = Convert.ToInt16(Config.warFriendlyDamage); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("warFriendlyDamage could not be parsed as a number!");
                }
                try { Vars.warAllyDamage = Convert.ToInt16(Config.warAllyDamage); }
                catch (Exception ex)
                {
                    Vars.conLog.Error("warAllyDamage could not be parsed as a number!");
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
