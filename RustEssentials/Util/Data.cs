﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace RustEssentials.Util
{
    public static class Data
    {
        #region Per Person TPA Cooldowns

        #region Read Per Person TPA Cooldowns
        public static void readRequestData()
        {
            try
            {
                List<string> cooldownFileData = File.ReadAllLines(Vars.requestCooldownsFile).ToList();
                foreach (string s in cooldownFileData)
                {
                    string UID = s.Split('=')[0];
                    string requestsString = s.Split('=')[1];

                    foreach (string s2 in requestsString.Split(';'))
                    {
                        string otherUID = s2.Split(':')[0];
                        string cooldown = s2.Split(':')[1];

                        TimerPlus t = new TimerPlus();
                        t.AutoReset = false;
                        t.Interval = Convert.ToInt64(cooldown);
                        t.timerCallback = new TimerCallback((senderObject) => Vars.unblockRequests(otherUID, UID));
                        t.Start();
                        if (!Vars.blockedRequestsPer.ContainsKey(UID))
                            Vars.blockedRequestsPer.Add(UID, new Dictionary<string, TimerPlus>());

                        Vars.blockedRequestsPer[UID].Add(otherUID, t);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("RRD: " + ex.ToString());
            }
        }
        #endregion

        #region Save Per Person TPA Cooldowns
        public static void saveRequestsPer()
        {
            try
            {
                Dictionary<string, List<string>> blockedPeoplePer = new Dictionary<string, List<string>>();

                foreach (KeyValuePair<string, Dictionary<string, TimerPlus>> kv in Vars.blockedRequestsPer)
                {
                    string UID = kv.Key;
                    if (!blockedPeoplePer.ContainsKey(UID))
                        blockedPeoplePer.Add(UID, new List<string>());
                    foreach (KeyValuePair<string, TimerPlus> kv2 in kv.Value)
                    {
                        string otherUID = kv2.Key;
                        blockedPeoplePer[UID].Add(otherUID);
                        updateRequestData(UID, otherUID, Vars.requestCooldown, kv2.Value);
                    }
                }
                remOldRequests(blockedPeoplePer);
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("SRP: " + ex.ToString());
            }
        }
        #endregion

        #region Update Per Person TPA Cooldowns
        public static void updateRequestData(string UID, string otherUID, double cooldown, TimerPlus t)
        {
            try
            {
                List<string> cooldownFileData = File.ReadAllLines(Vars.requestCooldownsFile).ToList();
                List<string> UIDs = new List<string>();
                foreach (string str in cooldownFileData)
                {
                    UIDs.Add(str.Split('=')[0]);
                }
                if (UIDs.Contains(UID))
                {
                    string fullString = "";
                    string currentRequests = Array.Find(cooldownFileData.ToArray(), (string s) => s.StartsWith(UID)).Split('=')[1];
                    if (currentRequests.Contains(otherUID))
                    {
                        List<string> allRequests = currentRequests.Split(';').ToList();
                        int index = Array.FindIndex(allRequests.ToArray(), (string s) => s.StartsWith(otherUID));

                        allRequests[index] = otherUID + ":" + t.TimeLeft;

                        fullString = UID + "=" + string.Join(";", allRequests.ToArray());
                    }
                    else
                    {
                        fullString = UID + "=" + currentRequests;

                        fullString += ";" + otherUID + ":" + cooldown;
                    }

                    int indexOfUID = Array.FindIndex(cooldownFileData.ToArray(), (string s) => s.StartsWith(UID));
                    cooldownFileData[indexOfUID] = fullString;
                }
                else
                {
                    cooldownFileData.Add(UID + "=" + otherUID + ":" + cooldown);
                }
                using (StreamWriter sw = new StreamWriter(Vars.requestCooldownsFile, false))
                {
                    foreach (string s in cooldownFileData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("URD: " + ex.ToString());
            }
        }
        #endregion

        #region Remove Per Person TPA Cooldowns
        public static void remOldRequests(Dictionary<string, List<string>> oldRequests)
        {
            try
            {
                List<string> cooldownFileData = File.ReadAllLines(Vars.requestCooldownsFile).ToList();
                List<string> removeQueue = new List<string>();
                int curIndex = 0;
                Dictionary<string, int> modifyQueue = new Dictionary<string, int>();
                foreach (string str in cooldownFileData)
                {
                    string UID = str.Split('=')[0];
                    if (!oldRequests.ContainsKey(UID)) // If all my cooldowns are completed but the file still has me cooling down
                    {
                        removeQueue.Add(str);
                    }
                    else // If I still have some cooldowns running
                    {
                        string currentRequests = Array.Find(cooldownFileData.ToArray(), (string s) => s.StartsWith(UID)).Split('=')[1];
                        foreach (string s in currentRequests.Split(';'))
                        {
                            string otherUID = s.Split(':')[0];
                            string cooldown = s.Split(':')[1];

                            if (!oldRequests[UID].Contains(otherUID)) // If a kit that is said to be cooling down in the file is no longer actually cooling down
                            {
                                string combinedStr = otherUID + ":" + cooldown;

                                if (currentRequests.Split(';').Count() > 1 && !currentRequests.EndsWith(combinedStr))
                                    currentRequests.Replace(combinedStr + ";", "");

                                if (currentRequests.Split(';').Count() > 1 && currentRequests.EndsWith(combinedStr))
                                    currentRequests.Replace(";" + combinedStr, "");

                                if (currentRequests.Split(';').Count() == 1)
                                    currentRequests.Replace(combinedStr, "");

                                string fullString = UID + "=" + currentRequests;

                                if (!modifyQueue.ContainsKey(fullString))
                                    modifyQueue.Add(fullString, curIndex);
                            }
                        }
                        curIndex++;
                    }
                }
                try
                {
                    foreach (string s in removeQueue)
                    {
                        cooldownFileData.Remove(s);
                    }
                }
                catch (Exception ex) { Vars.conLog.Error("REMOR: " + ex.ToString()); }
                foreach (KeyValuePair<string, int> kv in modifyQueue)
                {
                    cooldownFileData[kv.Value] = kv.Key;
                }
                using (StreamWriter sw = new StreamWriter(Vars.requestCooldownsFile, false))
                {
                    foreach (string s in cooldownFileData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("ROR: " + ex.ToString());
            }
        }
        #endregion

        #endregion

        #region Per All TPA Cooldowns

        #region Read Per All TPA Cooldowns
        public static void readRequestAllData()
        {
            try
            {
                List<string> cooldownFileData = File.ReadAllLines(Vars.requestCooldownsAllFile).ToList();
                foreach (string s in cooldownFileData)
                {
                    string UID = s.Split('=')[0];
                    string cooldown = s.Split('=')[1];

                    TimerPlus t = new TimerPlus();
                    t.AutoReset = false;
                    t.Interval = Convert.ToInt64(cooldown);
                    t.timerCallback = new TimerCallback((senderObject) => Vars.unblockRequests("", UID));
                    t.Start();
                    if (!Vars.blockedRequestsAll.ContainsKey(UID))
                        Vars.blockedRequestsAll.Add(UID, t);
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("RRAD: " + ex.ToString());
            }
        }
        #endregion

        #region Save Per All TPA Cooldowns
        public static void saveRequestsAll()
        {
            try
            {
                List<string> blockedPeopleAll = new List<string>();

                foreach (KeyValuePair<string, TimerPlus> kv in Vars.blockedRequestsAll)
                {
                    string UID = kv.Key;
                    if (!blockedPeopleAll.Contains(UID))
                        blockedPeopleAll.Add(UID);

                    updateRequestAllData(UID, Vars.requestCooldown, kv.Value);
                }
                remOldRequestsAll(blockedPeopleAll);
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("SRA: " + ex.ToString());
            }
        }
        #endregion

        #region Update Per All TPA Cooldowns
        public static void updateRequestAllData(string UID, double cooldown, TimerPlus t)
        {
            try
            {
                List<string> cooldownFileData = File.ReadAllLines(Vars.requestCooldownsAllFile).ToList();
                List<string> UIDs = new List<string>();
                foreach (string str in cooldownFileData)
                {
                    UIDs.Add(str.Split('=')[0]);
                }
                if (UIDs.Contains(UID))
                {
                    int indexOfUID = Array.FindIndex(cooldownFileData.ToArray(), (string s) => s.StartsWith(UID));
                    cooldownFileData[indexOfUID] = UID + "=" + cooldown;
                }
                else
                {
                    cooldownFileData.Add(UID + "=" + cooldown);
                }
                using (StreamWriter sw = new StreamWriter(Vars.requestCooldownsAllFile, false))
                {
                    foreach (string s in cooldownFileData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("URAD: " + ex.ToString());
            }
        }
        #endregion

        #region Remove Per All TPA Cooldowns
        public static void remOldRequestsAll(List<string> oldRequests)
        {
            try
            {
                List<string> cooldownFileData = File.ReadAllLines(Vars.requestCooldownsAllFile).ToList();
                List<int> removeQueue1 = new List<int>();
                Dictionary<string, int> removeQueue2 = new Dictionary<string, int>();
                foreach (string str in cooldownFileData)
                {
                    string UID = str.Split('=')[0];
                    if (!oldRequests.Contains(UID))
                    {
                        int indexOfUID = Array.FindIndex(cooldownFileData.ToArray(), (string s) => s.StartsWith(UID));
                        removeQueue1.Add(indexOfUID);
                    }
                }
                try
                {
                    foreach (int i in removeQueue1)
                    {
                        cooldownFileData.RemoveAt(i);
                    }
                }
                catch (Exception ex) { Vars.conLog.Error("REMORA: " + ex.ToString()); }
                using (StreamWriter sw = new StreamWriter(Vars.requestCooldownsAllFile, false))
                {
                    foreach (string s in cooldownFileData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("RORA: " + ex.ToString());
            }
        }
        #endregion

        #endregion

        #region Deaths

        #region Read Deaths
        public static void readDeathsData()
        {
            try
            {
                List<string> deathsFileData = File.ReadAllLines(Vars.deathsFile).ToList();
                foreach (string s in deathsFileData)
                {
                    string UID = s.Split('=')[0];
                    string deathsString = s.Split('=')[1];
                    int deaths;
                    if (int.TryParse(deathsString, out deaths))
                    {
                        if (!Vars.playerDeaths.ContainsKey(UID))
                            Vars.playerDeaths.Add(UID, deaths);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("RDD: " + ex.ToString());
            }
        }
        #endregion

        #region Update Deaths
        public static void updateDeathsData(string UID, int deaths)
        {
            try
            {
                List<string> deathsFileData = File.ReadAllLines(Vars.deathsFile).ToList();
                List<string> UIDs = new List<string>();
                foreach (string str in deathsFileData)
                {
                    UIDs.Add(str.Split('=')[0]);
                }
                if (UIDs.Contains(UID))
                {
                    int indexOfUID = Array.FindIndex(deathsFileData.ToArray(), (string s) => s.StartsWith(UID));
                    deathsFileData[indexOfUID] = UID + "=" + deaths;
                }
                else
                {
                    deathsFileData.Add(UID + "=" + deaths);
                }
                using (StreamWriter sw = new StreamWriter(Vars.deathsFile, false))
                {
                    foreach (string s in deathsFileData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("UDD: " + ex.ToString());
            }
        }
        #endregion

        #endregion

        #region Kills

        #region Read Kills
        public static void readKillsData()
        {
            try
            {
                List<string> killsFileData = File.ReadAllLines(Vars.killsFile).ToList();
                foreach (string s in killsFileData)
                {
                    string UID = s.Split('=')[0];
                    string killsString = s.Split('=')[1];
                    int kills;
                    if (int.TryParse(killsString, out kills))
                    {
                        if (!Vars.playerKills.ContainsKey(UID))
                            Vars.playerKills.Add(UID, kills);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("RKD: " + ex.ToString());
            }
        }
        #endregion

        #region Update Kills
        public static void updateKillsData(string UID, int kills)
        {
            try
            {
                List<string> killsFileData = File.ReadAllLines(Vars.killsFile).ToList();
                List<string> UIDs = new List<string>();
                foreach (string str in killsFileData)
                {
                    UIDs.Add(str.Split('=')[0]);
                }
                if (UIDs.Contains(UID))
                {
                    int indexOfUID = Array.FindIndex(killsFileData.ToArray(), (string s) => s.StartsWith(UID));
                    killsFileData[indexOfUID] = UID + "=" + kills;
                }
                else
                {
                    killsFileData.Add(UID + "=" + kills);
                }
                using (StreamWriter sw = new StreamWriter(Vars.killsFile, false))
                {
                    foreach (string s in killsFileData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("UKD: " + ex.ToString());
            }
        }
        #endregion

        #endregion

        #region Kit Cooldowns

        #region Save Kit Cooldowns
        public static void saveCooldowns()
        {
            try
            {
                Dictionary<string, List<string>> kits = new Dictionary<string, List<string>>();
                foreach (KeyValuePair<string, Dictionary<TimerPlus, string>> kv in Vars.activeKitCooldowns)
                {
                    string UID = kv.Key;
                    if (!kits.ContainsKey(UID))
                        kits.Add(UID, new List<string>());
                    foreach (KeyValuePair<TimerPlus, string> kv2 in kv.Value)
                    {
                        string kitName = kv2.Value;
                        kits[UID].Add(kitName);
                        if (Vars.kitCooldowns.ContainsKey(kitName))
                        {
                            string cooldown = Vars.kitCooldowns[kitName].ToString();
                            updateCooldownData(UID, kitName, cooldown, kv2.Key);
                        }
                    }
                }
                remOldCooldowns(kits);
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("SCD: " + ex.ToString());
            }
        }
        #endregion

        #region Read Kit Cooldowns
        public static void readCooldownData()
        {
            try
            {
                List<string> cooldownFileData = File.ReadAllLines(Vars.cooldownsFile).ToList();
                foreach (string s in cooldownFileData)
                {
                    string UID = s.Split('=')[0];
                    string kitsString = s.Split('=')[1];

                    foreach (string s2 in kitsString.Split(';'))
                    {
                        string kitName = s2.Split(':')[0].ToLower();
                        string cooldown = s2.Split(':')[1];
                        if (!cooldown.Contains("-"))
                        {
                            TimerPlus t = new TimerPlus();
                            t.AutoReset = false;
                            t.Interval = Convert.ToInt64(cooldown);
                            t.timerCallback = new TimerCallback((senderObj) => Vars.restoreKit(kitName, UID));
                            t.Start();

                            if (!Vars.activeKitCooldowns.ContainsKey(UID))
                            {
                                Vars.activeKitCooldowns.Add(UID, new Dictionary<TimerPlus, string>() { { t, kitName } });
                            }
                            else
                            {
                                if (!Vars.activeKitCooldowns[UID].ContainsValue(kitName))
                                    Vars.activeKitCooldowns[UID].Add(t, kitName);
                            }
                        }
                        else if (cooldown == "-1")
                        {
                            TimerPlus t = new TimerPlus();
                            t.isNull = true;
                            if (!Vars.activeKitCooldowns.ContainsKey(UID))
                            {
                                Vars.activeKitCooldowns.Add(UID, new Dictionary<TimerPlus, string>() { { t, kitName } });
                            }
                            else
                            {
                                if (!Vars.activeKitCooldowns[UID].ContainsValue(kitName))
                                    Vars.activeKitCooldowns[UID].Add(t, kitName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("RCDD: " + ex.ToString());
            }
        }
        #endregion

        #region Remove Kit Cooldowns
        public static void remOldCooldowns(Dictionary<string, List<string>> oldKits)
        {
            try
            {
                List<string> cooldownFileData = File.ReadAllLines(Vars.cooldownsFile).ToList();
                List<string> removeQueue1 = new List<string>();
                Dictionary<string, string> removeQueue2 = new Dictionary<string, string>();
                foreach (string str in cooldownFileData)
                {
                    string UID = str.Split('=')[0];
                    if (!oldKits.ContainsKey(UID)) // If all my cooldowns are completed but the file still has me cooling down
                    {
                        if (!removeQueue1.Contains(UID))
                            removeQueue1.Add(UID);
                    }
                    else // If I still have some cooldowns running
                    {
                        string currentKits = Array.Find(cooldownFileData.ToArray(), (string s) => s.StartsWith(UID)).Split('=')[1];
                        foreach (string s in currentKits.Split(';'))
                        {
                            string kitName = s.Split(':')[0];
                            string cooldown = s.Split(':')[1];

                            if (!oldKits[UID].Contains(kitName)) // If a kit that is said to be cooling down in the file is no longer actually cooling down
                            {
                                string combinedStr = kitName + ":" + cooldown;

                                if (currentKits.Split(';').Count() > 1 && !currentKits.EndsWith(combinedStr))
                                    currentKits.Replace(combinedStr + ";", "");

                                if (currentKits.Split(';').Count() > 1 && currentKits.EndsWith(combinedStr))
                                    currentKits.Replace(";" + combinedStr, "");

                                if (currentKits.Split(';').Count() == 1)
                                {
                                    combinedStr = "";
                                    if (!removeQueue1.Contains(UID))
                                        removeQueue1.Add(UID);
                                }

                                if (combinedStr.Length > 0)
                                {
                                    string fullString = UID + "=" + currentKits;

                                    if (!removeQueue2.ContainsKey(UID))
                                        removeQueue2.Add(UID, fullString);
                                }
                            }
                        }
                    }
                }
                try
                {
                    foreach (string s in removeQueue1)
                    {
                        int indexOfUID = Array.FindIndex(cooldownFileData.ToArray(), (string str) => str.StartsWith(s));
                        cooldownFileData.RemoveAt(indexOfUID);
                    }
                }
                catch (Exception ex) { Vars.conLog.Error("REMOC: " + ex.ToString()); }
                try
                {
                    foreach (KeyValuePair<string, string> kv in removeQueue2)
                    {
                        int indexOfUID = Array.FindIndex(cooldownFileData.ToArray(), (string st) => st.StartsWith(kv.Key));
                        cooldownFileData[indexOfUID] = kv.Value;
                    }
                }
                catch (Exception ex) { Vars.conLog.Error("REMOC #2: " + ex.ToString()); }
                using (StreamWriter sw = new StreamWriter(Vars.cooldownsFile, false))
                {
                    foreach (string s in cooldownFileData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("ROCD: " + ex.ToString());
            }
        }
        #endregion

        #region Update Kit Cooldowns
        public static void updateCooldownData(string UID, string kitName, string cooldown, TimerPlus t)
        {
            try
            {
                List<string> cooldownFileData = File.ReadAllLines(Vars.cooldownsFile).ToList();
                List<string> UIDs = new List<string>();
                foreach (string str in cooldownFileData)
                {
                    UIDs.Add(str.Split('=')[0]);
                }
                if (UIDs.Contains(UID)) // If I have any kits currenty cooling down
                {
                    string fullString = "";
                    string currentKits = Array.Find(cooldownFileData.ToArray(), (string s) => s.StartsWith(UID)).Split('=')[1];
                    if (currentKits.Contains(kitName)) // If the kit I am updating is currently cooling down, update the cooldown
                    {
                        List<string> allKits = currentKits.Split(';').ToList();
                        int index = Array.FindIndex(allKits.ToArray(), (string s) => s.StartsWith(kitName));

                        if (!t.isNull && t.TimeLeft > 0)
                            allKits[index] = kitName + ":" + t.TimeLeft;
                        else if (t.isNull)
                            allKits[index] = kitName + ":-1";
                        else
                            allKits.RemoveAt(index);

                        if (allKits.Count > 0)
                            fullString = UID + "=" + string.Join(";", allKits.ToArray());
                    }
                    else
                    {
                        fullString = UID + "=" + currentKits;

                        fullString += ";" + kitName + ":" + cooldown;
                    }

                    int indexOfUID = Array.FindIndex(cooldownFileData.ToArray(), (string s) => s.StartsWith(UID));
                    if (fullString.Length > 0)
                        cooldownFileData[indexOfUID] = fullString;
                    else
                        cooldownFileData.RemoveAt(indexOfUID);
                }
                else
                {
                    cooldownFileData.Add(UID + "=" + kitName + ":" + cooldown);
                }
                using (StreamWriter sw = new StreamWriter(Vars.cooldownsFile, false))
                {
                    foreach (string s in cooldownFileData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("UCDD: " + ex.ToString());
            }
        }
        #endregion

        #endregion

        #region Allies

        #region Read Allies
        public static void readAlliesData()
        {
            try
            {
                List<string> alliesFileData = File.ReadAllLines(Vars.alliesFile).ToList();
                foreach (string s in alliesFileData)
                {
                    if (s.Contains("="))
                    {
                        string factionName = s.Split('=')[0];
                        string alliesString = s.Split('=')[1];

                        if (!Vars.alliances.ContainsKey(factionName))
                            Vars.alliances.Add(factionName, new List<string>());
                        else
                            Vars.conLog.Error("Faction [" + factionName + "] and their alliances are already loaded!");

                        foreach (string s2 in alliesString.Split(';'))
                        {
                            string alliedFactionName = s2;

                            if (!Vars.alliances[factionName].Contains(alliedFactionName))
                                Vars.alliances[factionName].Add(alliedFactionName);
                            else
                                Vars.conLog.Error("Faction [" + factionName + "] already allied with faction [" + alliedFactionName + "]!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("RAD #1: " + ex.ToString());
            }
        }
        #endregion

        #region Add Allies
        public static void addAlliesData(string factionName, string alliedFactionName)
        {
            try
            {
                List<string> alliesFileData = File.ReadAllLines(Vars.alliesFile).ToList();
                List<string> factionNames = new List<string>();
                foreach (string str in alliesFileData)
                {
                    factionNames.Add(str.Split('=')[0]);
                }
                if (factionNames.Contains(factionName))
                {
                    string currentAlliances = Array.Find(alliesFileData.ToArray(), (string s) => s.StartsWith(factionName)).Split('=')[1];
                    string fullString = factionName + "=" + currentAlliances;

                    fullString += ";" + alliedFactionName;

                    int index = Array.FindIndex(alliesFileData.ToArray(), (string s) => s.StartsWith(factionName));
                    alliesFileData[index] = fullString;
                }
                else
                {
                    alliesFileData.Add(factionName + "=" + alliedFactionName);
                }
                if (factionNames.Contains(alliedFactionName))
                {
                    string currentAlliances = Array.Find(alliesFileData.ToArray(), (string s) => s.StartsWith(alliedFactionName)).Split('=')[1];
                    string fullString = alliedFactionName + "=" + currentAlliances;

                    fullString += ";" + factionName;

                    int index = Array.FindIndex(alliesFileData.ToArray(), (string s) => s.StartsWith(alliedFactionName));
                    alliesFileData[index] = fullString;
                }
                else
                {
                    alliesFileData.Add(alliedFactionName + "=" + factionName);
                }
                using (StreamWriter sw = new StreamWriter(Vars.alliesFile, false))
                {
                    foreach (string s in alliesFileData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("AAD: " + ex.ToString());
            }
        }
        #endregion

        #region Remove Allies
        public static void remAlliesData(string factionName, string alliedFactionName)
        {
            try
            {
                List<string> alliesFileData = File.ReadAllLines(Vars.alliesFile).ToList();
                if (alliedFactionName == "disband")
                {
                    string fullLine = Array.Find(alliesFileData.ToArray(), (string s) => s.StartsWith(factionName));
                    alliesFileData.Remove(fullLine);
                    foreach (string line in alliesFileData)
                    {
                        if (line.Contains(factionName))
                        {
                            string currentAlliesString = line.Split('=')[1];
                            List<string> newAllies = new List<string>();
                            foreach (string s in currentAlliesString.Split(';'))
                            {
                                if (s != factionName)
                                    newAllies.Add(s);
                            }
                            string fullString = line.Split('=')[0] + "=" + string.Join(";", newAllies.ToArray());
                            alliesFileData[alliesFileData.IndexOf(line)] = fullString;
                        }
                    }
                }
                else
                {
                    foreach (string line in alliesFileData)
                    {
                        if (line.StartsWith(factionName))
                        {
                            string currentAlliesString = Array.Find(alliesFileData.ToArray(), (string s) => s.StartsWith(factionName)).Split('=')[1];
                            List<string> newAllies = new List<string>();
                            foreach (string s in currentAlliesString.Split(';'))
                            {
                                if (s != alliedFactionName)
                                    newAllies.Add(s);
                            }
                            string fullString = factionName + "=" + string.Join(";", newAllies.ToArray());
                            alliesFileData[alliesFileData.IndexOf(line)] = fullString;
                        }
                        if (line.StartsWith(alliedFactionName))
                        {
                            string currentAlliesString = Array.Find(alliesFileData.ToArray(), (string s) => s.StartsWith(alliedFactionName)).Split('=')[1];
                            List<string> newAllies = new List<string>();
                            foreach (string s in currentAlliesString.Split(';'))
                            {
                                if (s != factionName)
                                    newAllies.Add(s);
                            }
                            string fullString = alliedFactionName + "=" + string.Join(";", newAllies.ToArray());
                            alliesFileData[alliesFileData.IndexOf(line)] = fullString;
                        }
                    }
                }
                using (StreamWriter sw = new StreamWriter(Vars.alliesFile, false))
                {
                    foreach (string s in alliesFileData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("RAD #2: " + ex.ToString());
            }
        }
        #endregion

        #endregion

        #region Factions

        #region Read Factions
        public static void readFactionData()
        {
            try
            {
                List<string> factionFileData = File.ReadAllLines(Vars.factionsFile).ToList();
                foreach (string s in factionFileData)
                {
                    if (s.Contains("="))
                    {
                        string factionName = s.Split('=')[0];
                        string membersString = s.Split('=')[1];

                        if (!Vars.factions.ContainsKey(factionName))
                            Vars.factions.Add(factionName, new Dictionary<string, string>());
                        else
                            Vars.conLog.Error("#1: Faction [" + factionName + "] already loaded!");
                        if (!Vars.factionsByNames.ContainsKey(factionName))
                            Vars.factionsByNames.Add(factionName, new Dictionary<string, string>());
                        else
                            Vars.conLog.Error("#2: Faction [" + factionName + "] already loaded!");

                        foreach (string s2 in membersString.Split(';'))
                        {
                            string nameAndUID = s2.Split(':')[0];
                            string rank = s2.Split(':')[1];
                            string name = nameAndUID.Substring(1, nameAndUID.IndexOf(')') - 1);
                            string UID = nameAndUID.Substring(nameAndUID.LastIndexOf(')') + 1);

                            if (!Vars.factions[factionName].ContainsKey(UID))
                                Vars.factions[factionName].Add(UID, rank);
                            else
                                Vars.conLog.Error("A: Faction [" + factionName + "] already includes UID \"" + UID + "\"!");
                            if (!Vars.factionsByNames[factionName].ContainsKey(UID))
                                Vars.factionsByNames[factionName].Add(UID, name);
                            else
                                Vars.conLog.Error("B: Faction [" + factionName + "] already includes UID \"" + UID + "\"!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("RFD #1: " + ex.ToString());
            }
        }
        #endregion

        #region Add Faction
        public static void addFactionData(string factionName, string userName, string userID, string rank)
        {
            try
            {
                List<string> factionFileData = File.ReadAllLines(Vars.factionsFile).ToList();
                List<string> factionNames = new List<string>();
                foreach (string str in factionFileData)
                {
                    factionNames.Add(str.Split('=')[0]);
                }
                if (factionNames.Contains(factionName))
                {
                    string currentMembers = Array.Find(factionFileData.ToArray(), (string s) => s.StartsWith(factionName)).Split('=')[1];
                    string fullString = factionName + "=" + currentMembers;

                    fullString += ";(" + userName + ")" + userID + ":" + rank;

                    int index = Array.FindIndex(factionFileData.ToArray(), (string s) => s.StartsWith(factionName));
                    factionFileData[index] = fullString;
                }
                else
                {
                    factionFileData.Add(factionName + "=(" + userName + ")" + userID + ":" + rank);
                }
                using (StreamWriter sw = new StreamWriter(Vars.factionsFile, false))
                {
                    foreach (string s in factionFileData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("AFD: " + ex.ToString());
            }
        }
        #endregion

        #region Remove Faction
        public static void remFactionData(string factionName, string userName, string rank)
        {
            try
            {
                List<string> factionFileData = File.ReadAllLines(Vars.factionsFile).ToList();
                if (userName == "disband")
                {
                    string fullLine = Array.Find(factionFileData.ToArray(), (string s) => s.StartsWith(factionName));
                    factionFileData.Remove(fullLine);
                }
                else
                {
                    foreach (string line in factionFileData)
                    {
                        if (line.StartsWith(factionName))
                        {
                            string currentMembersString = Array.Find(factionFileData.ToArray(), (string s) => s.StartsWith(factionName)).Split('=')[1];
                            Dictionary<string, string> currentMembers = new Dictionary<string, string>();
                            foreach (string s in currentMembersString.Split(';'))
                            {
                                string name = s.Substring(1, s.LastIndexOf(')') - 1);
                                string UID = s.Substring(s.LastIndexOf(')') + 1, 17);
                                currentMembers.Add(name, UID);
                            }
                            string fullString = factionName + "=" + currentMembersString;

                            if (currentMembersString.StartsWith("(" + userName + ")"))
                            {
                                fullString = fullString.Replace("(" + userName + ")" + currentMembers[userName] + ":" + rank + ";", "");
                                factionFileData[factionFileData.IndexOf(line)] = fullString;
                            }
                            else
                            {
                                fullString = fullString.Replace(";(" + userName + ")" + currentMembers[userName] + ":" + rank, "");
                                factionFileData[factionFileData.IndexOf(line)] = fullString;
                            }
                            break;
                        }
                    }
                }
                using (StreamWriter sw = new StreamWriter(Vars.factionsFile, false))
                {
                    foreach (string s in factionFileData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("RFD #2: " + ex.ToString());
            }
        }
        #endregion

        #endregion

        #region Bouncing Betties

        #region Read Bouncing Betties
        public static void readBettyData()
        {
            try
            {
                List<string> bettyFileData = File.ReadAllLines(Vars.bettyFile).ToList();
                foreach (string s in bettyFileData)
                {
                    if (s.Contains("="))
                    {
                        string bettyOwnerInfo = s.Split('=')[0];
                        if (bettyOwnerInfo.Contains("-"))
                        {
                            string ownerID = bettyOwnerInfo.Split('-')[0];
                            string ownerName = bettyOwnerInfo.Split('-')[1];

                            string bettyPositions = s.Split('=')[1];

                            if (bettyPositions.Contains(";"))
                            {
                                foreach (string s2 in bettyPositions.Split(';'))
                                {
                                    string[] args = s2.Replace(" ", "").Replace("(", "").Replace(")", "").Split(',');

                                    try
                                    {
                                        float x = float.Parse(args[0]);
                                        float y = float.Parse(args[1]);
                                        float z = float.Parse(args[2]);

                                        Vector3 bettyPos = new Vector3(x, y, z);

                                        ulong ownerUID;
                                        if (ulong.TryParse(ownerID, out ownerUID))
                                        {
                                            Vars.REB.StartCoroutine(Explosions.dropMine(bettyPos, ownerName, ownerUID));
                                        }
                                        else
                                            Vars.conLog.Error("Could not parse " + ownerID);
                                    }
                                    catch (Exception ex)
                                    {
                                        Vars.conLog.Error("RBD #2: Invalid betty position. Skipping...");
                                    }
                                }
                            }
                            else
                            {
                                string[] args = bettyPositions.Replace(" ", "").Replace("(", "").Replace(")", "").Split(',');

                                try
                                {
                                    float x = float.Parse(args[0]);
                                    float y = float.Parse(args[1]);
                                    float z = float.Parse(args[2]);

                                    Vector3 bettyPos = new Vector3(x, y, z);

                                    ulong ownerUID;
                                    if (ulong.TryParse(ownerID, out ownerUID))
                                        Vars.REB.StartCoroutine(Explosions.dropMine(bettyPos, ownerName, ownerUID));
                                    else
                                        Vars.conLog.Error("Could not parse " + ownerID);
                                }
                                catch (Exception ex)
                                {
                                    Vars.conLog.Error("RBD #2-2: Invalid betty position. Skipping...");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("RBD #1: " + ex.ToString());
            }
        }
        #endregion

        #region Add Bouncing Betty
        public static void addBettyData(string ownerName, string ownerUID, Vector3 bettyPos)
        {
            try
            {
                List<string> bettyFileData = File.ReadAllLines(Vars.bettyFile).ToList();
                List<string> bettyOwners = new List<string>();
                foreach (string str in bettyFileData)
                {
                    string ownerInfo = str.Split('=')[0];

                    if (ownerInfo.Contains("-"))
                        bettyOwners.Add(ownerInfo.Split('-')[0]);
                }
                if (bettyOwners.Contains(ownerUID))
                {
                    string currentBetties = Array.Find(bettyFileData.ToArray(), (string s) => s.StartsWith(ownerUID)).Split('=')[1];
                    string fullString = ownerUID + "-" + ownerName + "=" + currentBetties;

                    fullString += ";" + bettyPos;

                    int index = Array.FindIndex(bettyFileData.ToArray(), (string s) => s.StartsWith(ownerUID));
                    bettyFileData[index] = fullString;
                }
                else
                {
                    bettyFileData.Add(ownerUID + "-" + ownerName + "=" + bettyPos);
                }

                using (StreamWriter sw = new StreamWriter(Vars.bettyFile, false))
                {
                    foreach (string s in bettyFileData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("AFD: " + ex.ToString());
            }
        }
        #endregion

        #region Remove Bouncing Betty
        public static void remBettyData(string ownerUID, Vector3 bettyPos)
        {
            try
            {
                List<string> bettyFileData = File.ReadAllLines(Vars.bettyFile).ToList();
                foreach (string line in bettyFileData)
                {
                    if (line.StartsWith(ownerUID))
                    {
                        string ownerInfo = Array.Find(bettyFileData.ToArray(), (string s) => s.StartsWith(ownerUID)).Split('=')[0];
                        string currentBetties = Array.Find(bettyFileData.ToArray(), (string s) => s.StartsWith(ownerUID)).Split('=')[1];

                        string fullString = ownerInfo + "=" + currentBetties;

                        if (currentBetties.StartsWith(bettyPos.ToString()))
                        {
                            if (currentBetties.Contains(";"))
                            {
                                fullString = fullString.Replace(bettyPos + ";", "");
                                bettyFileData[bettyFileData.IndexOf(line)] = fullString;
                            }
                            else
                                bettyFileData.RemoveAt(bettyFileData.IndexOf(line));
                        }
                        else
                        {
                            fullString = fullString.Replace((currentBetties.Contains(";") ? ";" : "") + bettyPos, "");
                            bettyFileData[bettyFileData.IndexOf(line)] = fullString;
                        }

                        break;
                    }
                }

                using (StreamWriter sw = new StreamWriter(Vars.bettyFile, false))
                {
                    foreach (string s in bettyFileData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("RBD #3: " + ex.ToString());
            }
        }
        #endregion

        #region Update Bouncing Betties
        public static void updateBettyData(PlayerClient ownerClient)
        {
            try
            {
                string ownerName = ownerClient.userName;
                string ownerUID = ownerClient.userID.ToString();

                if (Vars.vanishedList.Contains(ownerUID))
                    return;

                List<string> bettyFileData = File.ReadAllLines(Vars.bettyFile).ToList();
                List<string> bettyOwners = new List<string>();
                foreach (string str in bettyFileData)
                {
                    string ownerInfo = str.Split('=')[0];
                    
                    if (ownerInfo.Contains("-"))
                        bettyOwners.Add(ownerInfo.Split('-')[0]);
                }

                if (bettyOwners.Contains(ownerUID))
                {
                    string currentBetties = Array.Find(bettyFileData.ToArray(), (string s) => s.StartsWith(ownerUID)).Split('=')[1];
                    string fullString = ownerUID + "-" + ownerName + "=" + currentBetties;

                    int index = Array.FindIndex(bettyFileData.ToArray(), (string s) => s.StartsWith(ownerUID));
                    bettyFileData[index] = fullString;
                }

                using (StreamWriter sw = new StreamWriter(Vars.bettyFile, false))
                {
                    foreach (string s in bettyFileData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("UBD: " + ex.ToString());
            }
        }
        #endregion

        #endregion

        #region Remover Sharing

        #region Read Remover Sharing
        public static void readRemoverData()
        {
            try
            {
                List<string> removerLines = File.ReadAllLines(Vars.removerDataFile).ToList();
                foreach (string s in removerLines)
                {
                    string owner = s.Split('=')[0];
                    string partnerString = s.Split('=')[1];
                    Vars.removerSharingData.Add(owner, partnerString);
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("RRD #1: " + ex.ToString());
            }
        }
        #endregion

        #region Add Remover Sharing
        public static void addRemoverData(string ownerID, string partnerID)
        {
            try
            {
                List<string> removerLines = File.ReadAllLines(Vars.removerDataFile).ToList();
                List<string> owners = new List<string>();
                foreach (string str in removerLines)
                {
                    owners.Add(str.Split('=')[0]);
                }

                if (owners.Contains(ownerID))
                {
                    string previousPartners = Array.Find(removerLines.ToArray(), (string s) => s.StartsWith(ownerID)).Split('=')[1];
                    string fullString = ownerID + "=" + previousPartners;

                    fullString += ":" + partnerID;

                    int index = Array.FindIndex(removerLines.ToArray(), (string s) => s.StartsWith(ownerID));
                    removerLines[index] = fullString;
                }
                else
                {
                    removerLines.Add(ownerID + "=" + partnerID);
                }

                using (StreamWriter sw = new StreamWriter(Vars.removerDataFile, false))
                {
                    foreach (string s in removerLines)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("ARD: " + ex.ToString());
            }
        }
        #endregion

        #region Remove Remover Sharing
        public static void remRemoverData(string ownerID, string partnerID)
        {
            try
            {
                List<string> removerLines = File.ReadAllLines(Vars.removerDataFile).ToList();
                if (partnerID == "all")
                {
                    string fullLine = Array.Find(removerLines.ToArray(), (string s) => s.StartsWith(ownerID));
                    removerLines.Remove(fullLine);
                }
                else
                {
                    foreach (string line in removerLines)
                    {
                        if (line.StartsWith(ownerID))
                        {
                            string previousPartners = Array.Find(removerLines.ToArray(), (string s) => s.StartsWith(ownerID)).Split('=')[1];
                            string fullString = ownerID + "=" + previousPartners;

                            if (previousPartners.StartsWith(partnerID))
                            {
                                if (previousPartners.Contains(":"))
                                {
                                    fullString = fullString.Replace(partnerID + ":", "");
                                    removerLines[removerLines.IndexOf(line)] = fullString;
                                }
                                else
                                    removerLines.Remove(fullString);
                            }
                            else
                            {
                                fullString = fullString.Replace(":" + partnerID, "");
                                removerLines[removerLines.IndexOf(line)] = fullString;
                            }
                            break;
                        }
                    }
                }
                using (StreamWriter sw = new StreamWriter(Vars.removerDataFile, false))
                {
                    foreach (string s in removerLines)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("RRD #2: " + ex.ToString());
            }
        }
        #endregion

        #endregion

        #region Door Sharing

        #region Read Door Sharing
        public static void readDoorData()
        {
            try
            {
                List<string> doorDataFile = File.ReadAllLines(Vars.doorsFile).ToList();
                foreach (string s in doorDataFile)
                {
                    string owner = s.Split('=')[0];
                    string partnerString = s.Split('=')[1];
                    Vars.sharingData.Add(owner, partnerString);
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("RDD #1: " + ex.ToString());
            }
        }
        #endregion

        #region Add Door Sharing
        public static void addDoorData(string ownerID, string partnerID)
        {
            try
            {
                List<string> doorDataFile = File.ReadAllLines(Vars.doorsFile).ToList();
                List<string> owners = new List<string>();
                foreach (string str in doorDataFile)
                {
                    owners.Add(str.Split('=')[0]);
                }

                if (owners.Contains(ownerID))
                {
                    string previousPartners = Array.Find(doorDataFile.ToArray(), (string s) => s.StartsWith(ownerID)).Split('=')[1];
                    string fullString = ownerID + "=" + previousPartners;

                    fullString += ":" + partnerID;

                    int index = Array.FindIndex(doorDataFile.ToArray(), (string s) => s.StartsWith(ownerID));
                    doorDataFile[index] = fullString;
                }
                else
                {
                    doorDataFile.Add(ownerID + "=" + partnerID);
                }

                using (StreamWriter sw = new StreamWriter(Vars.doorsFile, false))
                {
                    foreach (string s in doorDataFile)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("ADD: " + ex.ToString());
            }
        }
        #endregion

        #region Remove Door Sharing
        public static void remDoorData(string ownerID, string partnerID)
        {
            try
            {
                List<string> doorDataFile = File.ReadAllLines(Vars.doorsFile).ToList();
                if (partnerID == "all")
                {
                    string fullLine = Array.Find(doorDataFile.ToArray(), (string s) => s.StartsWith(ownerID));
                    doorDataFile.Remove(fullLine);
                }
                else
                {
                    foreach (string line in doorDataFile)
                    {
                        if (line.StartsWith(ownerID))
                        {
                            string previousPartners = Array.Find(doorDataFile.ToArray(), (string s) => s.StartsWith(ownerID)).Split('=')[1];
                            string fullString = ownerID + "=" + previousPartners;

                            if (previousPartners.StartsWith(partnerID))
                            {
                                if (previousPartners.Contains(":"))
                                {
                                    fullString = fullString.Replace(partnerID + ":", "");
                                    doorDataFile[doorDataFile.IndexOf(line)] = fullString;
                                }
                                else
                                    doorDataFile.Remove(fullString);
                            }
                            else
                            {
                                fullString = fullString.Replace(":" + partnerID, "");
                                doorDataFile[doorDataFile.IndexOf(line)] = fullString;
                            }
                            break;
                        }
                    }
                }
                using (StreamWriter sw = new StreamWriter(Vars.doorsFile, false))
                {
                    foreach (string s in doorDataFile)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("RDD #2: " + ex.ToString());
            }
        }
        #endregion

        #endregion

        #region Zones

        #region Read Zones
        public static void readZoneData()
        {
            List<string> zoneFileData = File.ReadAllLines(Vars.zonesFile).ToList();
            foreach (string s in zoneFileData)
            {
                if (s.Contains("="))
                {
                    string zoneName = s.Split('=')[0];
                    string posString = s.Split('=')[1];
                    if (posString.Contains(";"))
                    {
                        string[] split = posString.Split(';');
                        string firstPoint = split[0].Replace("(", "").Replace(")", ""); // (x,y) - EX:(500.2,802.7)
                        string secPoint = split[1].Replace("(", "").Replace(")", "");
                        string thirdPoint = split[2].Replace("(", "").Replace(")", "");
                        string fourthPoint = split[3].Replace("(", "").Replace(")", "");
                        string buildable = split[4];
                        string sleepersCanDie = split[5];
                        Vector2 point1 = new Vector2(Convert.ToSingle(firstPoint.Split(',')[0]), Convert.ToSingle(firstPoint.Split(',')[1]));
                        Vector2 point2 = new Vector2(Convert.ToSingle(secPoint.Split(',')[0]), Convert.ToSingle(secPoint.Split(',')[1]));
                        Vector2 point3 = new Vector2(Convert.ToSingle(thirdPoint.Split(',')[0]), Convert.ToSingle(thirdPoint.Split(',')[1]));
                        Vector2 point4 = new Vector2(Convert.ToSingle(fourthPoint.Split(',')[0]), Convert.ToSingle(fourthPoint.Split(',')[1]));
                        bool buildableBool;
                        bool sleepersCanDieBool;
                        if (!bool.TryParse(buildable, out buildableBool)) buildableBool = false;
                        if (!bool.TryParse(sleepersCanDie, out sleepersCanDieBool)) sleepersCanDieBool = false;

                        if (zoneName.StartsWith("safezone"))
                            Vars.safeZones.Add(new Zone(zoneName, point1, point2, point3, point4, buildableBool, sleepersCanDieBool));

                        if (zoneName.StartsWith("warzone"))
                            Vars.warZones.Add(new Zone(zoneName, point1, point2, point3, point4, buildableBool, sleepersCanDieBool));

                        Vars.conLog.Info("Adding zone [" + zoneName + "]...");
                    }
                }
            }
        }
        #endregion

        #region Add Zone
        public static void addZoneData(Zone zone)
        {
            List<string> zoneFileData = File.ReadAllLines(Vars.zonesFile).ToList();
            List<string> safezones = new List<string>();
            List<string> warzones = new List<string>();
            foreach (string s in zoneFileData)
            {
                bool isSafeZone = s.StartsWith("safezone");

                string[] split = s.Split('=');
                string pointsAndBools = split[1];

                if (isSafeZone)
                {
                    safezones.Add("safezone_" + safezones.Count + "=" + pointsAndBools);
                }
                else
                {
                    warzones.Add("warzone_" + warzones.Count + "=" + pointsAndBools);
                }
            }
            zoneFileData.Clear();
            foreach (string s in safezones)
            {
                zoneFileData.Add(s);
            }
            if (zone.zoneName.StartsWith("safezone"))
            {
                zoneFileData.Add("safezone_" + safezones.Count + "=(" + zone.firstPoint.x + "," + zone.firstPoint.y + ");(" + zone.secondPoint.x + "," + zone.secondPoint.y + ");(" + zone.thirdPoint.x + "," + zone.thirdPoint.y + ");(" + zone.fourthPoint.x + "," + zone.fourthPoint.y + ");" + zone.buildable.ToString().ToLower() + ";" + zone.sleepersCanDie.ToString().ToLower());
            }
            foreach (string s in warzones)
            {
                zoneFileData.Add(s);
            }
            if (zone.zoneName.StartsWith("warzone"))
            {
                zoneFileData.Add("warzone_" + warzones.Count + "=(" + zone.firstPoint.x + "," + zone.firstPoint.y + ");(" + zone.secondPoint.x + "," + zone.secondPoint.y + ");(" + zone.thirdPoint.x + "," + zone.thirdPoint.y + ");(" + zone.fourthPoint.x + "," + zone.fourthPoint.y + ");" + zone.buildable.ToString().ToLower() + ";" + zone.sleepersCanDie.ToString().ToLower());
            }
            using (StreamWriter sw = new StreamWriter(Vars.zonesFile, false))
            {
                foreach (string s in zoneFileData)
                {
                    sw.WriteLine(s);
                }
            }
        }
        #endregion

        #region Update Zone
        public static void updateZoneData(Zone zone)
        {
            List<string> zoneFileData = File.ReadAllLines(Vars.zonesFile).ToList();
            List<string> safezones = new List<string>();
            List<string> warzones = new List<string>();
            foreach (string s in zoneFileData)
            {
                bool isSafeZone = s.StartsWith("safezone");

                string[] split = s.Split('=');
                string pointsAndBools = split[1];
                Vars.conLog.Info(s);
                Vars.conLog.Info("1");
                if (pointsAndBools.Contains(';'))
                {
                    Vars.conLog.Info("2");
                    split = pointsAndBools.Split(';');
                    string firstPoint = split[0].Replace("(", "").Replace(")", "");
                    string secPoint = split[1].Replace("(", "").Replace(")", "");
                    string thirdPoint = split[2].Replace("(", "").Replace(")", "");
                    string fourthPoint = split[3].Replace("(", "").Replace(")", "");
                    Vector2 point1 = new Vector2(Convert.ToSingle(firstPoint.Split(',')[0]), Convert.ToSingle(firstPoint.Split(',')[1]));
                    Vector2 point2 = new Vector2(Convert.ToSingle(secPoint.Split(',')[0]), Convert.ToSingle(secPoint.Split(',')[1]));
                    Vector2 point3 = new Vector2(Convert.ToSingle(thirdPoint.Split(',')[0]), Convert.ToSingle(thirdPoint.Split(',')[1]));
                    Vector2 point4 = new Vector2(Convert.ToSingle(fourthPoint.Split(',')[0]), Convert.ToSingle(fourthPoint.Split(',')[1]));
                    Vars.conLog.Info("3");

                    if (Vector2.Distance(zone.firstPoint, point1) < 0.2f && Vector2.Distance(zone.secondPoint, point2) < 0.2f && Vector2.Distance(zone.thirdPoint, point3) < 0.2f && Vector2.Distance(zone.fourthPoint, point4) < 0.2f)
                    {
                        Vars.conLog.Info("4");
                        if (isSafeZone && zone.zoneName.StartsWith("safezone"))
                        {
                            Vars.conLog.Info("5");
                            safezones.Add("safezone_" + safezones.Count + "=(" + zone.firstPoint.x + "," + zone.firstPoint.y + ");(" + zone.secondPoint.x + "," + zone.secondPoint.y + ");(" + zone.thirdPoint.x + "," + zone.thirdPoint.y + ");(" + zone.fourthPoint.x + "," + zone.fourthPoint.y + ");" + zone.buildable.ToString().ToLower() + ";" + zone.sleepersCanDie.ToString().ToLower());
                        }
                        else if (!isSafeZone && zone.zoneName.StartsWith("warzone"))
                        {
                            warzones.Add("warzone_" + safezones.Count + "=(" + zone.firstPoint.x + "," + zone.firstPoint.y + ");(" + zone.secondPoint.x + "," + zone.secondPoint.y + ");(" + zone.thirdPoint.x + "," + zone.thirdPoint.y + ");(" + zone.fourthPoint.x + "," + zone.fourthPoint.y + ");" + zone.buildable.ToString().ToLower() + ";" + zone.sleepersCanDie.ToString().ToLower());
                        }
                    }
                }
            }
            zoneFileData.Clear();
            foreach (string s in safezones)
            {
                zoneFileData.Add(s);
            }
            foreach (string s in warzones)
            {
                zoneFileData.Add(s);
            }
            using (StreamWriter sw = new StreamWriter(Vars.zonesFile, false))
            {
                foreach (string s in zoneFileData)
                {
                    sw.WriteLine(s);
                }
            }
        }
        #endregion

        #region Remove Zone
        public static void remZoneData(string zoneName)
        {
            List<string> zoneFileData = File.ReadAllLines(Vars.zonesFile).ToList();
            List<string> safezones = new List<string>();
            List<string> warzones = new List<string>();
            bool clearAllSafe = zoneName == "clearallS";
            bool clearAllWar = zoneName == "clearallW";
            foreach (string s in zoneFileData)
            {
                bool isSafeZone = s.StartsWith("safezone");

                if (!s.StartsWith(zoneName))
                {
                    if (s.Contains("="))
                    {
                        string[] split = s.Split('=');
                        string pointsAndBools = split[1];

                        if (isSafeZone)
                        {
                            if (!clearAllSafe)
                                safezones.Add("safezone_" + safezones.Count + "=" + pointsAndBools);
                        }
                        else
                        {
                            if (!clearAllWar)
                                warzones.Add("warzone_" + warzones.Count + "=" + pointsAndBools);
                        }
                    }
                }
            }
            zoneFileData.Clear();
            foreach (string s in safezones)
            {
                zoneFileData.Add(s);
            }
            foreach (string s in warzones)
            {
                zoneFileData.Add(s);
            }
            using (StreamWriter sw = new StreamWriter(Vars.zonesFile, false))
            {
                foreach (string s in zoneFileData)
                {
                    sw.WriteLine(s);
                }
            }
        }
        #endregion

        #endregion

        #region Warps

        #region Add Warp
        public static void addWarp(string warpName, string owner, Vector3 position)
        {
            List<string> warpFileData = File.ReadAllLines(Vars.warpsFile).ToList();
            bool isDefault = owner == Vars.defaultRank;
            warpFileData.Add("[" + warpName + (isDefault ? "]" : "." + owner + "]"));
            warpFileData.Add("(" + position.x + ", " + position.y + ", " + position.z + ")");
            using (StreamWriter sw = new StreamWriter(Vars.warpsFile, false))
            {
                foreach (string s in warpFileData)
                {
                    sw.WriteLine(s);
                }
            }
        }
        #endregion

        #region Edit Warp
        public static void editWarp(string warpName, string newOwner, bool wasUnassigned)
        {
            List<string> warpFileData = File.ReadAllLines(Vars.warpsFile).ToList();
            int curIndex = 0;
            bool found = false;

            foreach (string s in warpFileData)
            {
                if (s.ToLower().StartsWith("[" + warpName + (wasUnassigned ? "]" : ".")) && s.ToLower().EndsWith("]"))
                {
                    found = true;
                    break;
                }

                curIndex++;
            }
            if (found)
            {
                if (newOwner.ToLower() == Vars.defaultRank.ToLower())
                    warpFileData[curIndex] = "[" + warpName + "]";
                else
                    warpFileData[curIndex] = "[" + warpName + "." + newOwner + "]";
            }
            using (StreamWriter sw = new StreamWriter(Vars.warpsFile, false))
            {
                foreach (string s in warpFileData)
                {
                    sw.WriteLine(s);
                }
            }
        }
        #endregion

        #region Remove Warp
        public static void remWarp(string warpName)
        {
            List<string> warpFileData = File.ReadAllLines(Vars.warpsFile).ToList();
            int startLine = 0;
            int endLine = 0;
            int curIndex = 0;
            List<int> emptyLine = new List<int>();
            bool lookingForPos = false;
            bool foundWarp = false;
            bool foundPos = false;
            foreach (string s in warpFileData)
            {
                if (s.ToLower().StartsWith("[" + warpName + ".") && s.EndsWith("]") && !lookingForPos)
                {
                    endLine = startLine;
                    lookingForPos = true;
                    foundWarp = true;
                }

                if (s.ToLower().StartsWith("(") && s.EndsWith(")") && s.Contains(",") && lookingForPos)
                {
                    foundPos = true;
                    break;
                }

                curIndex++;

                if (!lookingForPos)
                    startLine++;
                else
                    endLine++;
            }
            if (foundWarp && foundPos)
            {
                if (warpFileData.Count - 1 > endLine)
                {
                    if (warpFileData[endLine + 1].Replace(" ", "").Length == 0)
                        emptyLine.Add(endLine + 1);
                }
                warpFileData.RemoveAt(startLine);
                warpFileData.RemoveAt(endLine - 1);
            }
            foreach (int i in emptyLine)
            {
                warpFileData.RemoveAt(i);
            }
            using (StreamWriter sw = new StreamWriter(Vars.warpsFile, false))
            {
                foreach (string s in warpFileData)
                {
                    sw.WriteLine(s);
                }
            }
        }
        #endregion

        #endregion

        #region Warp Cooldowns

        #region Save Warp Cooldowns
        public static void saveWarpCooldowns()
        {
            try
            {
                Dictionary<string, List<string>> warps = new Dictionary<string, List<string>>();
                foreach (KeyValuePair<string, Dictionary<TimerPlus, string>> kv in Vars.activeWarpCooldowns)
                {
                    string UID = kv.Key;
                    if (!warps.ContainsKey(UID))
                        warps.Add(UID, new List<string>());
                    foreach (KeyValuePair<TimerPlus, string> kv2 in kv.Value)
                    {
                        string warpName = kv2.Value;
                        warps[UID].Add(warpName);
                        if (Vars.warpCooldowns.ContainsKey(warpName))
                        {
                            string cooldown = Vars.kitCooldowns[warpName].ToString();
                            updateWarpCooldownData(UID, warpName, cooldown, kv2.Key);
                        }
                    }
                }
                remOldWarpCooldowns(warps);
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("SWCD: " + ex.ToString());
            }
        }
        #endregion

        #region Read Warp Cooldowns
        public static void readWarpCooldownData()
        {
            try
            {
                List<string> warpCooldownsData = File.ReadAllLines(Vars.warpCooldownsFile).ToList();
                foreach (string s in warpCooldownsData)
                {
                    string UID = s.Split('=')[0];
                    string warpsString = s.Split('=')[1];

                    foreach (string s2 in warpsString.Split(';'))
                    {
                        string warpName = s2.Split(':')[0].ToLower();
                        string cooldown = s2.Split(':')[1];
                        if (!cooldown.Contains("-"))
                        {
                            TimerPlus t = new TimerPlus();
                            t.AutoReset = false;
                            t.Interval = Convert.ToInt64(cooldown);
                            t.timerCallback = new TimerCallback((senderObj) => Vars.restoreWarp(warpName, UID));
                            t.Start();

                            if (!Vars.activeWarpCooldowns.ContainsKey(UID))
                            {
                                Vars.activeWarpCooldowns.Add(UID, new Dictionary<TimerPlus, string>() { { t, warpName } });
                            }
                            else
                            {
                                if (!Vars.activeWarpCooldowns[UID].ContainsValue(warpName))
                                    Vars.activeWarpCooldowns[UID].Add(t, warpName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("RWCDD: " + ex.ToString());
            }
        }
        #endregion

        #region Remove Warp Cooldowns
        public static void remOldWarpCooldowns(Dictionary<string, List<string>> oldWarps)
        {
            try
            {
                List<string> warpCooldownsData = File.ReadAllLines(Vars.warpCooldownsFile).ToList();
                List<string> removeQueue1 = new List<string>();
                Dictionary<string, string> removeQueue2 = new Dictionary<string, string>();
                foreach (string str in warpCooldownsData)
                {
                    string UID = str.Split('=')[0];
                    if (!oldWarps.ContainsKey(UID)) // If all my cooldowns are completed but the file still has me cooling down
                    {
                        if (!removeQueue1.Contains(UID))
                            removeQueue1.Add(UID);
                    }
                    else // If I still have some cooldowns running
                    {
                        string currentWarps = Array.Find(warpCooldownsData.ToArray(), (string s) => s.StartsWith(UID)).Split('=')[1];
                        foreach (string s in currentWarps.Split(';'))
                        {
                            string warpName = s.Split(':')[0];
                            string cooldown = s.Split(':')[1];

                            if (!oldWarps[UID].Contains(warpName)) // If a kit that is said to be cooling down in the file is no longer actually cooling down
                            {
                                string combinedStr = warpName + ":" + cooldown;

                                if (currentWarps.Split(';').Count() > 1 && !currentWarps.EndsWith(combinedStr))
                                    currentWarps.Replace(combinedStr + ";", "");

                                if (currentWarps.Split(';').Count() > 1 && currentWarps.EndsWith(combinedStr))
                                    currentWarps.Replace(";" + combinedStr, "");

                                if (currentWarps.Split(';').Count() == 1)
                                {
                                    combinedStr = "";
                                    if (!removeQueue1.Contains(UID))
                                        removeQueue1.Add(UID);
                                }

                                if (combinedStr.Length > 0)
                                {
                                    string fullString = UID + "=" + currentWarps;

                                    if (!removeQueue2.ContainsKey(UID))
                                        removeQueue2.Add(UID, fullString);
                                }
                            }
                        }
                    }
                }
                try
                {
                    foreach (string s in removeQueue1)
                    {
                        int indexOfUID = Array.FindIndex(warpCooldownsData.ToArray(), (string str) => str.StartsWith(s));
                        warpCooldownsData.RemoveAt(indexOfUID);
                    }
                }
                catch (Exception ex) { Vars.conLog.Error("ROWCD #2: " + ex.ToString()); }
                try
                {
                    foreach (KeyValuePair<string, string> kv in removeQueue2)
                    {
                        int indexOfUID = Array.FindIndex(warpCooldownsData.ToArray(), (string st) => st.StartsWith(kv.Key));
                        warpCooldownsData[indexOfUID] = kv.Value;
                    }
                }
                catch (Exception ex) { Vars.conLog.Error("ROWCD #3: " + ex.ToString()); }
                using (StreamWriter sw = new StreamWriter(Vars.warpCooldownsFile, false))
                {
                    foreach (string s in warpCooldownsData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("ROWCD: " + ex.ToString());
            }
        }
        #endregion

        #region Update Warp Cooldowns
        public static void updateWarpCooldownData(string UID, string warpName, string cooldown, TimerPlus t)
        {
            try
            {
                List<string> warpCooldownsData = File.ReadAllLines(Vars.warpCooldownsFile).ToList();
                List<string> UIDs = new List<string>();
                foreach (string str in warpCooldownsData)
                {
                    UIDs.Add(str.Split('=')[0]);
                }
                if (UIDs.Contains(UID)) // If I have any kits currenty cooling down
                {
                    string fullString = "";
                    string currentWarps = Array.Find(warpCooldownsData.ToArray(), (string s) => s.StartsWith(UID)).Split('=')[1];
                    if (currentWarps.Contains(warpName)) // If the kit I am updating is currently cooling down, update the cooldown
                    {
                        List<string> allWarps = currentWarps.Split(';').ToList();
                        int index = Array.FindIndex(allWarps.ToArray(), (string s) => s.StartsWith(warpName));

                        if (t.TimeLeft > 0)
                            allWarps[index] = warpName + ":" + t.TimeLeft;
                        else
                            allWarps.RemoveAt(index);

                        if (allWarps.Count > 0)
                            fullString = UID + "=" + string.Join(";", allWarps.ToArray());
                    }
                    else
                    {
                        fullString = UID + "=" + currentWarps;

                        fullString += ";" + warpName + ":" + cooldown;
                    }

                    int indexOfUID = Array.FindIndex(warpCooldownsData.ToArray(), (string s) => s.StartsWith(UID));
                    if (fullString.Length > 0)
                        warpCooldownsData[indexOfUID] = fullString;
                    else
                        warpCooldownsData.RemoveAt(indexOfUID);
                }
                else
                {
                    warpCooldownsData.Add(UID + "=" + warpName + ":" + cooldown);
                }
                using (StreamWriter sw = new StreamWriter(Vars.warpCooldownsFile, false))
                {
                    foreach (string s in warpCooldownsData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("UWCDD: " + ex.ToString());
            }
        }
        #endregion

        #endregion
    }
}