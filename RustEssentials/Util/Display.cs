using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

namespace RustEssentials.Util
{
    public static class Display
    {
        public static void displayDistance(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                List<string> messageList = new List<string>();
                int curIndex = 0;
                foreach (string s in args)
                {
                    if (curIndex > 0)
                        messageList.Add(s);
                    curIndex++;
                }

                string targetName = string.Join(" ", messageList.ToArray());

                PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));

                if (possibleTargets.Count() == 0)
                    Broadcast.broadcastTo(senderClient.netPlayer, "No players equal or contain \"" + targetName + "\".");
                else if (possibleTargets.Count() > 1)
                    Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal or contain \"" + targetName + "\".");
                else
                {
                    PlayerClient targetClient = possibleTargets[0];
                    Character targetChar;
                    Character.FindByUser(targetClient.userID, out targetChar);
                    Character senderChar;
                    Character.FindByUser(senderClient.userID, out senderChar);

                    Vector2 targetPos = new Vector2(targetChar.transform.position.x, targetChar.transform.position.z);
                    Vector2 senderPos = new Vector2(senderChar.transform.position.x, senderChar.transform.position.z);

                    Broadcast.broadcastTo(senderClient.netPlayer, "Distance: " + Vector2.Distance(targetPos, senderPos) + "m.");
                }
            }
        }

        public static void displayLeaderboard(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "Grabbing leaderboard... This may take a while.");
                string leaderboard = args[1];
                switch (leaderboard)
                {
                    case "kills":
                        Thread t = new Thread(() =>
                        {
                            try
                            {
                                KeyValuePair<ulong, int>[] aboveZero = Array.FindAll(Vars.sortDictionaryByValue(Vars.playerKills).ToArray(), (KeyValuePair<ulong, int> kv) => kv.Value > 0);

                                Dictionary<ulong, string> namesAndUIDs = new Dictionary<ulong, string>();

                                int curIndex = 0;
                                foreach (var data in aboveZero)
                                {
                                    if (!namesAndUIDs.ContainsKey(data.Key))
                                    {
                                        curIndex++;

                                        if (curIndex > 15)
                                            break;
                                        namesAndUIDs.Add(data.Key, "");

                                        PlayerClient playerClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID == data.Key);
                                        string playerName;
                                        if (playerClient != null) // If player is online
                                            namesAndUIDs[data.Key] = playerClient.netUser.displayName; // Use the unhidden name of that online user (in case he/she is vanished)
                                        else // If not online
                                        {
                                            if (Vars.grabNameByUID(data.Key, out playerName)) // If we are able to grab their name from the steam db
                                                namesAndUIDs[data.Key] = playerName; // Set it to the result of the steam db
                                            else // If the process for grabbing their name through steam failed
                                                namesAndUIDs[data.Key] = "???";
                                        }
                                    }
                                }

                                if (aboveZero.Count() > 0)
                                {
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Top 15 killers of all players:");
                                    curIndex = 0;
                                    foreach (var data in aboveZero)
                                    {
                                        curIndex++;

                                        if (curIndex > 15)
                                            break;

                                        string playerName = namesAndUIDs[data.Key];
                                        ulong UID = data.Key;
                                        string kills = data.Value.ToString();
                                        string fullString = curIndex + ". " + playerName + " (" + UID + "): " + kills;
                                        Broadcast.broadcastTo(senderClient.netPlayer, fullString);
                                    }
                                    for (curIndex = curIndex + 1; curIndex <= 15; curIndex++)
                                    {
                                        Broadcast.broadcastTo(senderClient.netPlayer, curIndex + ".");
                                    }
                                    string senderName = senderClient.userName;
                                    ulong senderUID = senderClient.userID;
                                    int rankNum = Array.FindIndex(aboveZero.ToArray(), (KeyValuePair<ulong, int> kv) => kv.Key == senderUID) + 1;
                                    int senderKills = 0;
                                    if (rankNum > 0)
                                        senderKills = aboveZero[rankNum - 1].Value;

                                    Broadcast.broadcastTo(senderClient.netPlayer, "> " + rankNum + ". " + senderName + " (" + senderUID + "): " + senderKills + " <");
                                }
                                else
                                    Broadcast.broadcastTo(senderClient.netPlayer, "There are no kills on record to display the kills leaderboard!");
                            }
                            catch (Exception ex)
                            {
                                Vars.conLog.Error("LBK: " + ex.ToString());
                            }
                        });
                        t.IsBackground = true;
                        t.Start();
                        break;
                    case "deaths":
                        Thread thr = new Thread(() =>
                        {
                            try
                            {
                                KeyValuePair<ulong, int>[] aboveZero = Array.FindAll(Vars.sortDictionaryByValue(Vars.playerDeaths).ToArray(), (KeyValuePair<ulong, int> kv) => kv.Value > 0);

                                Dictionary<ulong, string> namesAndUIDs = new Dictionary<ulong, string>();

                                int curIndex = 0;
                                foreach (var data in aboveZero)
                                {
                                    if (!namesAndUIDs.ContainsKey(data.Key))
                                    {
                                        curIndex++;

                                        if (curIndex > 15)
                                            break;
                                        namesAndUIDs.Add(data.Key, "");

                                        PlayerClient playerClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID == data.Key);
                                        string playerName;
                                        if (playerClient != null) // If player is online
                                            namesAndUIDs[data.Key] = playerClient.netUser.displayName; // Use the unhidden name of that online user (in case he/she is vanished)
                                        else // If not online
                                        {
                                            if (Vars.grabNameByUID(data.Key, out playerName)) // If we are able to grab their name from the steam db
                                                namesAndUIDs[data.Key] = playerName; // Set it to the result of the steam db
                                            else // If the process for grabbing their name through steam failed
                                                namesAndUIDs[data.Key] = "???";
                                        }
                                    }
                                }

                                if (aboveZero.Count() > 0)
                                {
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Top 15 deaths of all players:");
                                    curIndex = 0;
                                    foreach (var data in aboveZero)
                                    {
                                        curIndex++;

                                        if (curIndex > 15)
                                            break;

                                        string playerName = namesAndUIDs[data.Key];
                                        ulong UID = data.Key;
                                        string deaths = data.Value.ToString();
                                        string fullString = curIndex + ". " + playerName + " (" + UID + "): " + deaths;
                                        Broadcast.broadcastTo(senderClient.netPlayer, fullString);
                                    }
                                    for (curIndex = curIndex + 1; curIndex <= 15; curIndex++)
                                    {
                                        Broadcast.broadcastTo(senderClient.netPlayer, curIndex + ".");
                                    }
                                    string senderName = senderClient.userName;
                                    ulong senderUID = senderClient.userID;
                                    int rankNum = Array.FindIndex(aboveZero.ToArray(), (KeyValuePair<ulong, int> kv) => kv.Key == senderUID) + 1;
                                    int senderDeaths = 0;
                                    if (rankNum > 0)
                                        senderDeaths = aboveZero[rankNum - 1].Value;

                                    Broadcast.broadcastTo(senderClient.netPlayer, "> " + rankNum + ". " + senderName + " (" + senderUID + "): " + senderDeaths + " <");
                                }
                                else
                                    Broadcast.broadcastTo(senderClient.netPlayer, "There are no deaths on record to display the deaths leaderboard!");
                            }
                            catch (Exception ex)
                            {
                                Vars.conLog.Error("LBD: " + ex.ToString());
                            }
                        });
                        thr.IsBackground = true;
                        thr.Start();
                        break;
                    case "kdr":
                        Thread thre = new Thread(() =>
                        {
                            try
                            {
                                KeyValuePair<ulong, int>[] aboveZeroKills = Array.FindAll(Vars.sortDictionaryByValue(Vars.playerKills).ToArray(), (KeyValuePair<ulong, int> kv) => kv.Value > 0);
                                KeyValuePair<ulong, int>[] aboveZeroDeaths = Array.FindAll(Vars.sortDictionaryByValue(Vars.playerDeaths).ToArray(), (KeyValuePair<ulong, int> kv) => kv.Value > 0);
                                Dictionary<ulong, decimal> aboveZeroKDRs = new Dictionary<ulong, decimal>();

                                Dictionary<ulong, string> namesAndUIDs = new Dictionary<ulong, string>();

                                int curIndex = 0;
                                foreach (var data in aboveZeroKills)
                                {
                                    if (!namesAndUIDs.ContainsKey(data.Key))
                                    {
                                        curIndex++;

                                        if (curIndex > 15)
                                            break;
                                        namesAndUIDs.Add(data.Key, "");

                                        PlayerClient playerClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID == data.Key);
                                        string playerName;
                                        if (playerClient != null) // If player is online
                                            namesAndUIDs[data.Key] = playerClient.netUser.displayName; // Use the unhidden name of that online user (in case he/she is vanished)
                                        else // If not online
                                        {
                                            if (Vars.grabNameByUID(data.Key, out playerName)) // If we are able to grab their name from the steam db
                                                namesAndUIDs[data.Key] = playerName; // Set it to the result of the steam db
                                            else // If the process for grabbing their name through steam failed
                                                namesAndUIDs[data.Key] = "???";
                                        }
                                    }
                                }

                                if (aboveZeroKills.Count() > 0)
                                {
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Top 15 KDRs of all players:");
                                    curIndex = 0;
                                    foreach (var data in aboveZeroKills)
                                    {
                                        curIndex++;

                                        if (curIndex > 15)
                                            break;

                                        ulong UID = data.Key;
                                        int kills = data.Value;
                                        if (Vars.playerDeaths.ContainsKey(UID))
                                        {
                                            int deaths = Vars.playerDeaths[UID];
                                            if (deaths > 0)
                                            {
                                                int indexOf = Array.FindIndex(aboveZeroDeaths.ToArray(), (KeyValuePair<ulong, int> kv) => kv.Key == UID);
                                                deaths = aboveZeroDeaths[indexOf].Value;
                                                decimal KDR = Math.Round((decimal)kills / deaths, 2);
                                                aboveZeroKDRs.Add(UID, KDR);
                                            }
                                            else
                                            {
                                                decimal KDR = Math.Round((decimal)kills, 2);
                                                aboveZeroKDRs.Add(UID, KDR);
                                            }
                                        }
                                        else
                                        {
                                            decimal KDR = Math.Round((decimal)kills, 2);
                                            aboveZeroKDRs.Add(UID, KDR);
                                        }
                                    }

                                    aboveZeroKDRs = Vars.sortDictionaryByValue(aboveZeroKDRs);

                                    curIndex = 0;
                                    foreach (var data in aboveZeroKDRs)
                                    {
                                        curIndex++;

                                        string playerName = namesAndUIDs[data.Key];
                                        ulong UID = data.Key;
                                        decimal KDR = data.Value;
                                        string fullString = curIndex + ". " + playerName + " (" + UID + "): " + KDR;
                                        Broadcast.broadcastTo(senderClient.netPlayer, fullString);
                                    }

                                    for (curIndex = curIndex + 1; curIndex <= 15; curIndex++)
                                    {
                                        Broadcast.broadcastTo(senderClient.netPlayer, curIndex + ".");
                                    }

                                    string senderName = senderClient.netUser.displayName;
                                    ulong senderUID = senderClient.userID;

                                    int senderKills = Array.Find(Vars.playerKills.ToArray(), (KeyValuePair<ulong, int> kv) => kv.Key == senderUID).Value;
                                    int senderDeaths = Array.Find(Vars.playerDeaths.ToArray(), (KeyValuePair<ulong, int> kv) => kv.Key == senderUID).Value;
                                    decimal senderKDR = 0.00M;

                                    if (senderKills > 0 && senderDeaths > 0)
                                        senderKDR = Math.Round((decimal)senderKills / senderDeaths, 2);

                                    Dictionary<ulong, decimal> sortedKDRs = new Dictionary<ulong, decimal>();
                                    foreach (var data in aboveZeroKills)
                                    {
                                        ulong UID = data.Key;
                                        int killData = data.Value;
                                        try
                                        {
                                            if (Vars.playerDeaths.ContainsKey(UID))
                                            {
                                                int deaths = Vars.playerDeaths[UID];
                                                if (deaths > 0)
                                                {
                                                    int indexOf = Array.FindIndex(aboveZeroDeaths.ToArray(), (KeyValuePair<ulong, int> kv) => kv.Key == UID);
                                                    deaths = aboveZeroDeaths[indexOf].Value;
                                                    decimal KDR = Math.Round((decimal)killData / deaths, 2);
                                                    sortedKDRs.Add(UID, KDR);
                                                }
                                                else
                                                {
                                                    decimal KDR = (decimal)killData;
                                                    sortedKDRs.Add(UID, KDR);
                                                }
                                            }
                                            else
                                            {
                                                decimal KDR = (decimal)killData;
                                                sortedKDRs.Add(UID, KDR);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            sortedKDRs.Add(UID, 0.00M);
                                        }
                                    }

                                    sortedKDRs = Vars.sortDictionaryByValue(sortedKDRs);
                                    int kdrsRank = Array.FindIndex(sortedKDRs.ToArray(), (KeyValuePair<ulong, decimal> kv) => kv.Key == senderUID) + 1;
                                    Broadcast.broadcastTo(senderClient.netPlayer, "> " + kdrsRank + ". " + senderName + " (" + senderUID + "): " + senderKDR + " <");
                                }
                                else
                                    Broadcast.broadcastTo(senderClient.netPlayer, "There are no kills on record to display the kdr leaderboard!");
                            }
                            catch (Exception ex)
                            {
                                Vars.conLog.Error("LBKDR: " + ex.ToString());
                            }
                        });
                        thre.IsBackground = true;
                        thre.Start();
                        break;
                    case "okills":
                        Thread threa = new Thread(() =>
                        {
                            try
                            {
                                KeyValuePair<ulong, int>[] aboveZero = Array.FindAll(Vars.sortDictionaryByValue(Vars.playerKills).ToArray(), (KeyValuePair<ulong, int> kv) => kv.Value > 0);
                                List<ulong> removeQueue = new List<ulong>();

                                Dictionary<ulong, string> namesAndUIDs = new Dictionary<ulong, string>();

                                int curIndex = 0;
                                foreach (var data in aboveZero)
                                {
                                    if (!namesAndUIDs.ContainsKey(data.Key))
                                    {
                                        namesAndUIDs.Add(data.Key, "");

                                        PlayerClient playerClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID == data.Key);
                                        if (playerClient != null) // If player is online
                                        {
                                            namesAndUIDs[data.Key] = playerClient.netUser.displayName; // Use the unhidden name of that online user (in case he/she is vanished)
                                        }
                                        else
                                            removeQueue.Add(data.Key);
                                    }
                                }

                                Dictionary<ulong, int> aboveZeroDictionary = new Dictionary<ulong, int>();
                                foreach (var data in aboveZero)
                                {
                                    if (!removeQueue.Contains(data.Key))
                                        aboveZeroDictionary.Add(data.Key, data.Value);
                                    else
                                        namesAndUIDs.Remove(data.Key);
                                }
                                aboveZeroDictionary = Vars.sortDictionaryByValue(aboveZeroDictionary);

                                if (aboveZeroDictionary.Count() > 0)
                                {
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Top 15 killers of all online players:");
                                    curIndex = 0;
                                    foreach (var data in aboveZeroDictionary)
                                    {
                                        curIndex++;

                                        if (curIndex > 15)
                                            break;

                                        string playerName = namesAndUIDs[data.Key];
                                        ulong UID = data.Key;
                                        string kills = data.Value.ToString();
                                        string fullString = curIndex + ". " + playerName + " (" + UID + "): " + kills;
                                        Broadcast.broadcastTo(senderClient.netPlayer, fullString);
                                    }
                                    for (curIndex = curIndex + 1; curIndex <= 15; curIndex++)
                                    {
                                        Broadcast.broadcastTo(senderClient.netPlayer, curIndex + ".");
                                    }
                                    string senderName = senderClient.userName;
                                    ulong senderUID = senderClient.userID;
                                    int rankNum = Array.FindIndex(aboveZeroDictionary.ToArray(), (KeyValuePair<ulong, int> kv) => kv.Key == senderUID) + 1;
                                    string senderKills = aboveZeroDictionary[senderUID].ToString();
                                    Broadcast.broadcastTo(senderClient.netPlayer, "> " + rankNum + ". " + senderName + " (" + senderUID + "): " + senderKills + " <");
                                }
                                else
                                    Broadcast.broadcastTo(senderClient.netPlayer, "There are no kills on record to display the okills leaderboard!");
                            }
                            catch (Exception ex)
                            {
                                Vars.conLog.Error("OLBK: " + ex.ToString());
                            }
                        });
                        threa.IsBackground = true;
                        threa.Start();
                        break;
                    case "odeaths":
                        Thread thread = new Thread(() =>
                        {
                            try
                            {
                                KeyValuePair<ulong, int>[] aboveZero = Array.FindAll(Vars.sortDictionaryByValue(Vars.playerDeaths).ToArray(), (KeyValuePair<ulong, int> kv) => kv.Value > 0);
                                List<ulong> removeQueue = new List<ulong>();

                                Dictionary<ulong, string> namesAndUIDs = new Dictionary<ulong, string>();

                                int curIndex = 0;
                                foreach (var data in aboveZero)
                                {
                                    if (!namesAndUIDs.ContainsKey(data.Key))
                                    {
                                        namesAndUIDs.Add(data.Key, "");

                                        PlayerClient playerClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID == data.Key);
                                        string playerName;
                                        if (playerClient != null) // If player is online
                                        {
                                            namesAndUIDs[data.Key] = playerClient.netUser.displayName; // Use the unhidden name of that online user (in case he/she is vanished)
                                            curIndex++;
                                        }
                                        else
                                            removeQueue.Add(data.Key);
                                    }
                                }

                                Dictionary<ulong, int> aboveZeroDictionary = new Dictionary<ulong, int>();
                                foreach (var data in aboveZero)
                                {
                                    if (!removeQueue.Contains(data.Key))
                                        aboveZeroDictionary.Add(data.Key, data.Value);
                                    else
                                        namesAndUIDs.Remove(data.Key);
                                }
                                aboveZeroDictionary = Vars.sortDictionaryByValue(aboveZeroDictionary);

                                if (aboveZeroDictionary.Count() > 0)
                                {
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Top 15 deaths of all online players:");
                                    curIndex = 0;
                                    foreach (var data in aboveZeroDictionary)
                                    {
                                        curIndex++;

                                        if (curIndex > 15)
                                            break;

                                        string playerName = namesAndUIDs[data.Key];
                                        ulong UID = data.Key;
                                        string deaths = data.Value.ToString();
                                        string fullString = curIndex + ". " + playerName + " (" + UID + "): " + deaths;
                                        Broadcast.broadcastTo(senderClient.netPlayer, fullString);
                                    }
                                    for (curIndex = curIndex + 1; curIndex <= 15; curIndex++)
                                    {
                                        Broadcast.broadcastTo(senderClient.netPlayer, curIndex + ".");
                                    }
                                    string senderName = senderClient.userName;
                                    ulong senderUID = senderClient.userID;
                                    int rankNum = Array.FindIndex(aboveZeroDictionary.ToArray(), (KeyValuePair<ulong, int> kv) => kv.Key == senderUID) + 1;
                                    string senderDeaths = aboveZeroDictionary[senderUID].ToString();
                                    Broadcast.broadcastTo(senderClient.netPlayer, "> " + rankNum + ". " + senderName + " (" + senderUID + "): " + senderDeaths + " <");
                                }
                                else
                                    Broadcast.broadcastTo(senderClient.netPlayer, "There are no deaths on record to display the odeaths leaderboard!");
                            }
                            catch (Exception ex)
                            {
                                Vars.conLog.Error("OLBD: " + ex.ToString());
                            }
                        });
                        thread.IsBackground = true;
                        thread.Start();
                        break;
                    case "okdr":
                        Thread thread2 = new Thread(() =>
                        {
                            try
                            {
                                KeyValuePair<ulong, int>[] aboveZeroKills = Array.FindAll(Vars.sortDictionaryByValue(Vars.playerKills).ToArray(), (KeyValuePair<ulong, int> kv) => kv.Value > 0);
                                KeyValuePair<ulong, int>[] aboveZeroDeaths = Array.FindAll(Vars.sortDictionaryByValue(Vars.playerDeaths).ToArray(), (KeyValuePair<ulong, int> kv) => kv.Value > 0);
                                Dictionary<ulong, decimal> aboveZeroKDRs = new Dictionary<ulong, decimal>();
                                List<ulong> removeQueue = new List<ulong>();

                                Dictionary<ulong, string> namesAndUIDs = new Dictionary<ulong, string>();

                                foreach (var data in aboveZeroKills)
                                {
                                    if (!namesAndUIDs.ContainsKey(data.Key))
                                    {
                                        namesAndUIDs.Add(data.Key, "");

                                        PlayerClient playerClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID == data.Key);
                                        string playerName;
                                        if (playerClient != null) // If player is online
                                        {
                                            namesAndUIDs[data.Key] = playerClient.netUser.displayName; // Use the unhidden name of that online user (in case he/she is vanished)
                                        }
                                        else
                                            removeQueue.Add(data.Key);
                                    }
                                }

                                Dictionary<ulong, int> aboveZeroKillsDictionary = new Dictionary<ulong, int>();
                                foreach (var data in aboveZeroKills)
                                {
                                    if (!removeQueue.Contains(data.Key))
                                        aboveZeroKillsDictionary.Add(data.Key, data.Value);
                                    else
                                        namesAndUIDs.Remove(data.Key);
                                }
                                aboveZeroKillsDictionary = Vars.sortDictionaryByValue(aboveZeroKillsDictionary);

                                if (aboveZeroKillsDictionary.Count() > 0)
                                {
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Top 15 KDRs of all online players:");
                                    int curIndex = 0;
                                    foreach (var data in aboveZeroKillsDictionary)
                                    {
                                        curIndex++;

                                        if (curIndex > 15)
                                            break;

                                        ulong UID = data.Key;
                                        int kills = data.Value;
                                        if (Vars.playerDeaths.ContainsKey(UID))
                                        {
                                            int deaths = Vars.playerDeaths[UID];
                                            if (deaths > 0)
                                            {
                                                int indexOf = Array.FindIndex(aboveZeroDeaths.ToArray(), (KeyValuePair<ulong, int> kv) => kv.Key == UID);
                                                deaths = aboveZeroDeaths[indexOf].Value;
                                                decimal KDR = Math.Round((decimal)kills / deaths, 2);
                                                aboveZeroKDRs.Add(UID, KDR);
                                            }
                                            else
                                            {
                                                decimal KDR = Math.Round((decimal)kills, 2);
                                                aboveZeroKDRs.Add(UID, KDR);
                                            }
                                        }
                                        else
                                        {
                                            decimal KDR = Math.Round((decimal)kills, 2);
                                            aboveZeroKDRs.Add(UID, KDR);
                                        }
                                    }

                                    aboveZeroKDRs = Vars.sortDictionaryByValue(aboveZeroKDRs);

                                    curIndex = 0;
                                    foreach (var data in aboveZeroKDRs)
                                    {
                                        curIndex++;

                                        string playerName = namesAndUIDs[data.Key];
                                        ulong UID = data.Key;
                                        decimal KDR = data.Value;
                                        string fullString = curIndex + ". " + playerName + " (" + UID + "): " + KDR;
                                        Broadcast.broadcastTo(senderClient.netPlayer, fullString);
                                    }

                                    for (curIndex = curIndex + 1; curIndex <= 15; curIndex++)
                                    {
                                        Broadcast.broadcastTo(senderClient.netPlayer, curIndex + ".");
                                    }

                                    string senderName = senderClient.userName;
                                    ulong senderUID = senderClient.userID;

                                    int senderKills = Array.Find(Vars.playerKills.ToArray(), (KeyValuePair<ulong, int> kv) => kv.Key == senderUID).Value;
                                    int senderDeaths = Array.Find(Vars.playerDeaths.ToArray(), (KeyValuePair<ulong, int> kv) => kv.Key == senderUID).Value;
                                    decimal senderKDR = 0.00M;

                                    if (senderKills > 0 && senderDeaths > 0)
                                        senderKDR = Math.Round((decimal)senderKills / senderDeaths, 2);

                                    Dictionary<ulong, decimal> sortedKDRs = new Dictionary<ulong, decimal>();
                                    foreach (var data in aboveZeroKillsDictionary)
                                    {
                                        ulong UID = data.Key;
                                        int killData = data.Value;
                                        try
                                        {
                                            if (Vars.playerDeaths.ContainsKey(UID))
                                            {
                                                int deaths = Vars.playerDeaths[UID];
                                                if (deaths > 0)
                                                {
                                                    int indexOf = Array.FindIndex(aboveZeroDeaths.ToArray(), (KeyValuePair<ulong, int> kv) => kv.Key == UID);
                                                    deaths = aboveZeroDeaths[indexOf].Value;
                                                    decimal KDR = Math.Round((decimal)killData / deaths, 2);
                                                    sortedKDRs.Add(UID, KDR);
                                                }
                                                else
                                                {
                                                    decimal KDR = (decimal)killData;
                                                    sortedKDRs.Add(UID, KDR);
                                                }
                                            }
                                            else
                                            {
                                                decimal KDR = (decimal)killData;
                                                sortedKDRs.Add(UID, KDR);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            sortedKDRs.Add(UID, 0.00M);
                                        }
                                    }

                                    sortedKDRs = Vars.sortDictionaryByValue(sortedKDRs);
                                    int kdrsRank = Array.FindIndex(sortedKDRs.ToArray(), (KeyValuePair<ulong, decimal> kv) => kv.Key == senderUID) + 1;

                                    Broadcast.broadcastTo(senderClient.netPlayer, "> " + kdrsRank + ". " + senderName + " (" + senderUID + "): " + senderKDR + " <");
                                }
                                else
                                    Broadcast.broadcastTo(senderClient.netPlayer, "There are no kills on record to display the okdr leaderboard!");
                            }
                            catch (Exception ex)
                            {
                                Vars.conLog.Error("OLBKDR: " + ex.ToString());
                            }
                        });
                        thread2.IsBackground = true;
                        thread2.Start();
                        break;
                    case "econ":
                        Broadcast.broadcastTo(senderClient.netPlayer, "Economy will not be implemented until v1.8.0 or later.");
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "You must specify a leaderboard:  kills, deaths, kdr, okills, odeaths, or okdr.");
                        break;
                }
            }
            else
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "You must specify a leaderboard: kills, deaths, kdr, okills, odeaths, or okdr.");
            }
        }

        public static void displayShopifyURL(PlayerClient senderClient)
        {
            if (Vars.enableShopify)
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "You can donate for kits here:");
                Broadcast.broadcastTo(senderClient.netPlayer, Vars.shopifyLink);
            }
            else
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "Shopify donations are not available!");
            }
        }

        public static void displayDirection(PlayerClient senderClient)
        {
            Character senderChar;
            if (Vars.getPlayerChar(senderClient, out senderChar))
            {
                float senderYaw = senderChar.eyesAngles.yaw + 180;
                if (Vars.sunBasedCompass)
                {
                    if (senderYaw <= 307.5f && senderYaw > 262.5f)
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "You are facing East.");
                    }
                    else if (senderYaw <= 262.5f && senderYaw > 217.5f)
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "You are facing North East.");
                    }
                    else if (senderYaw <= 217.5f && senderYaw > 172.5f)
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "You are facing North.");
                    }
                    else if (senderYaw <= 172.5f && senderYaw > 127.5f)
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "You are facing North West.");
                    }
                    else if (senderYaw <= 127.5f && senderYaw > 82.5f)
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "You are facing West.");
                    }
                    else if (senderYaw <= 82.5f && senderYaw > 37.5f)
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "You are facing South West.");
                    }
                    else if ((senderYaw <= 37.5f && senderYaw >= 0) || (senderYaw <= 360 && senderYaw > 352.5f))
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "You are facing South.");
                    }
                    else if (senderYaw <= 352.5f && senderYaw > 307.5f)
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "You are facing South East.");
                    }
                }
                else
                {
                    if ((senderYaw <= 22.5 && senderYaw >= 0) || (senderYaw <= 360 && senderYaw > 337.5f))
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "You are facing West.");
                    }
                    else if (senderYaw <= 67.5 && senderYaw > 22.5f)
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "You are facing North West.");
                    }
                    else if (senderYaw <= 112.5f && senderYaw > 67.5f)
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "You are facing North.");
                    }
                    else if (senderYaw <= 157.5f && senderYaw > 112.5f)
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "You are facing North East.");
                    }
                    else if (senderYaw <= 202.5f && senderYaw > 157.5f)
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "You are facing East.");
                    }
                    else if (senderYaw <= 247.5f && senderYaw > 202.5f)
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "You are facing South East.");
                    }
                    else if (senderYaw <= 292.5f && senderYaw > 247.5f)
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "You are facing South.");
                    }
                    else if (senderYaw <= 337.5f && senderYaw > 292.5f)
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "You are facing South West.");
                    }
                }
            }
        }

        public static void displayClock(PlayerClient senderClient)
        {
            double timeDecimal = Math.Round(Time.getTime(), 2);
            TimeSpan ts = TimeSpan.FromHours(timeDecimal);
            string tsMinutes = (ts.Minutes < 10 && ts.Minutes > 0 ? "0" + ts.Minutes : (ts.Minutes == 0 ? "00" : ts.Minutes.ToString()));
            Broadcast.broadcastTo(senderClient.netPlayer, "The time is currently " + timeDecimal + " [" + ts.Hours + ":" + tsMinutes + "].");
        }

        public static void displayVoteURL(PlayerClient senderClient)
        {
            if (Vars.enableTRSVoting && Vars.enableRSVoting)
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "To vote on [color #FFA154]toprustservers[color " + Vars.defaultColor + "], visit this website and click the \"Vote for this server!\" button:");
                Broadcast.broadcastTo(senderClient.netPlayer, Vars.TRSvoteLink);
                Broadcast.broadcastTo(senderClient.netPlayer, "To vote on [color #FFA154]rust-servers[color " + Vars.defaultColor + "], visit this website, click the \"Vote\" button, and sign in through steam:");
                Broadcast.broadcastTo(senderClient.netPlayer, Vars.RSvoteLink);
                Broadcast.broadcastTo(senderClient.netPlayer, "After voting, type /voted in game to redeem your reward.");
            }
            else if (Vars.enableTRSVoting)
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "To vote, visit this website and click the \"Vote for this server!\" button:");
                Broadcast.broadcastTo(senderClient.netPlayer, Vars.TRSvoteLink);
                Broadcast.broadcastTo(senderClient.netPlayer, "After voting, type /voted in game to redeem your reward.");
            }
            else if (Vars.enableRSVoting)
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "To vote, visit this website, click the \"Vote\" button, and sign in through steam:");
                Broadcast.broadcastTo(senderClient.netPlayer, Vars.RSvoteLink);
                Broadcast.broadcastTo(senderClient.netPlayer, "After voting, type /voted in game to redeem your reward.");
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "Voting is not enabled on this server.");
        }

        public static void displayDropTime(PlayerClient senderClient)
        {
            if (Vars.dropMode == 1)
            {
                double nextAirdrop = Vars.lastAirdropTime + Vars.dropInterval;
                double timeLeft = nextAirdrop - Vars.currentTime;
                TimeSpan timeSpan = TimeSpan.FromMilliseconds(timeLeft);

                string timeString = "";

                timeString += timeSpan.Hours + " hours, ";
                timeString += timeSpan.Minutes + " minutes, and ";
                timeString += timeSpan.Seconds + " seconds";

                Broadcast.broadcastTo(senderClient.netPlayer, "The next automated airdrop will arrive in " + timeString + ". There must be atleast " + Vars.minimumPlayers + " player(s).");
            }
            else
            {
                TimeSpan dT = TimeSpan.FromHours(Vars.dropTime);
                string dTMinutes = (dT.Minutes < 10 && dT.Minutes > 0 ? "0" + dT.Minutes : (dT.Minutes == 0 ? "00" : dT.Minutes.ToString()));
                TimeSpan cT = TimeSpan.FromHours(Math.Round(Time.getTime(), 2));
                string cTMinutes = (cT.Minutes < 10 && cT.Minutes > 0 ? "0" + cT.Minutes : (cT.Minutes == 0 ? "00" : cT.Minutes.ToString()));
                Broadcast.broadcastTo(senderClient.netPlayer, "Airdrops will send at " + Vars.dropTime + " [" + dT.Hours + ":" + dTMinutes + "]. There must be atleast " + Vars.minimumPlayers + " player(s). It is currently " + Math.Round(Time.getTime(), 2) + " [" + cT.Hours + ":" + cTMinutes + "].");
            }
        }

        public static void displayTools(PlayerClient senderClient, string[] args)
        {
            string targetName = senderClient.userName;
            PlayerClient targetClient = senderClient;
            if (args.Count() > 1)
            {
                string playerName = "";
                List<string> playerNameList = new List<string>();
                int curIndex = 0;
                foreach (string s in args)
                {
                    if (curIndex > 0)
                    {
                        playerNameList.Add(s);
                    }
                    curIndex++;
                }

                playerName = string.Join(" ", playerNameList.ToArray());
                bool isQuotted = false;
                if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                {
                    isQuotted = true;
                    playerName.Substring(1, playerName.Length - 2);
                }

                try
                {
                    if (isQuotted)
                    {
                        PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(playerName));
                        if (possibleTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal \"" + playerName + "\".");
                        else if (possibleTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + playerName + "\".");
                        else
                        {
                            targetClient = possibleTargets[0];
                            targetName = targetClient.userName;
                            Broadcast.broadcastTo(senderClient.netPlayer, targetName + "'s tools:");
                        }
                    }
                    else
                    {
                        PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                        if (possibleTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                        else if (possibleTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                        else
                        {
                            targetClient = possibleTargets[0];
                            targetName = targetClient.userName;
                            Broadcast.broadcastTo(senderClient.netPlayer, targetName + "'s tools:");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Vars.conLog.Error("DTOOLS: " + ex.ToString());
                    Broadcast.broadcastTo(senderClient.netPlayer, "Something went wrong!");
                    return;
                }
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "Your tools:");

            Broadcast.broadcastTo(senderClient.netPlayer, "Vanished: " + Vars.vanishedList.Contains(targetClient.userID).ToString());
            Broadcast.broadcastTo(senderClient.netPlayer, "Hidden from AI: " + Vars.hiddenList.Contains(targetClient.userID).ToString());
            Broadcast.broadcastTo(senderClient.netPlayer, "Remove: " + Vars.destroyerList.Contains(targetClient.userID).ToString());
            Broadcast.broadcastTo(senderClient.netPlayer, "Remove all: " + Vars.destroyerAllList.Contains(targetClient.userID).ToString());
            Broadcast.broadcastTo(senderClient.netPlayer, "Remover: " + Vars.removerList.Contains(targetClient.userID).ToString());
            Broadcast.broadcastTo(senderClient.netPlayer, "Owner: " + Vars.ownershipList.Contains(targetClient.userID).ToString());
            Broadcast.broadcastTo(senderClient.netPlayer, "Wand: " + Vars.wandList.ContainsKey(targetClient.userID).ToString());
            Broadcast.broadcastTo(senderClient.netPlayer, "Portal: " + Vars.portalList.Contains(targetClient.userID).ToString());
            Broadcast.broadcastTo(senderClient.netPlayer, "Bypass: " + Vars.bypassList.Contains(targetClient.userID).ToString());
            Broadcast.broadcastTo(senderClient.netPlayer, "Ghost: " + Vars.ghostList.ContainsKey(targetClient.userID).ToString());
            Broadcast.broadcastTo(senderClient.netPlayer, "Godmode: " + Vars.godList.Contains(targetClient.userID).ToString());
            Broadcast.broadcastTo(senderClient.netPlayer, "Super Craft: " + Vars.craftList.Contains(targetClient.userID).ToString());
            Broadcast.broadcastTo(senderClient.netPlayer, "Unlimited Ammo: " + Vars.unlAmmoList.Contains(targetClient.userID).ToString());
            Broadcast.broadcastTo(senderClient.netPlayer, "Infinite Ammo: " + Vars.infAmmoList.Contains(targetClient.userID).ToString());
        }

        public static void displayEntities(PlayerClient senderClient)
        {
            Broadcast.broadcastTo(senderClient.netPlayer, "All spawnable entities:");
            Broadcast.broadcastTo(senderClient.netPlayer, "bear, wolf, deer, rabbit, pig, chicken,");
            Broadcast.broadcastTo(senderClient.netPlayer, "mutantbear, mutantwolf, ammobox, medbox,");
            Broadcast.broadcastTo(senderClient.netPlayer, "weaponbox, box, crate, wood, ore1, ore2,");
            Broadcast.broadcastTo(senderClient.netPlayer, "ore3");
        }

        public static void displayPing(PlayerClient senderClient)
        {
            Broadcast.broadcastTo(senderClient.netPlayer, "Ping:");
            Broadcast.broadcastTo(senderClient.netPlayer, "- Recent: " + senderClient.netPlayer.lastPing + " ms");
            Broadcast.broadcastTo(senderClient.netPlayer, "- Average: " + senderClient.netPlayer.averagePing + " ms");
        }

        public static void displayPlayerInfo(PlayerClient senderClient, string[] args, bool isAlternative)
        {
            if (senderClient != null)
            {
                if (args.Count() > 1)
                {
                    string playerName = "";
                    List<string> playerNameList = new List<string>();
                    int curIndex = 0;
                    foreach (string s in args)
                    {
                        if (curIndex > 0)
                        {
                            playerNameList.Add(s);
                        }
                        curIndex++;
                    }

                    playerName = string.Join(" ", playerNameList.ToArray());
                    bool isQuotted = false;
                    if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                    {
                        isQuotted = true;
                        playerName.Substring(1, playerName.Length - 2);
                    }

                    try
                    {
                        if (isQuotted)
                        {
                            PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(playerName));
                            if (possibleTargets.Count() == 0)
                                Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal \"" + playerName + "\".");
                            else if (possibleTargets.Count() > 1)
                                Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + playerName + "\".");
                            else
                            {
                                PlayerClient targetClient = possibleTargets[0];

                                if (!Vars.playerDeaths.ContainsKey(targetClient.userID))
                                    Vars.playerDeaths.Add(targetClient.userID, 0);
                                if (!Vars.playerKills.ContainsKey(targetClient.userID))
                                    Vars.playerKills.Add(targetClient.userID, 0);

                                KeyValuePair<ulong, int>[] sortedKills = Array.FindAll(Vars.sortDictionaryByValue(Vars.playerKills).ToArray(), (KeyValuePair<ulong, int> kv) => kv.Value > 0);
                                KeyValuePair<ulong, int>[] sortedDeaths = Array.FindAll(Vars.sortDictionaryByValue(Vars.playerDeaths).ToArray(), (KeyValuePair<ulong, int> kv) => kv.Value > 0);
                                int kills = Array.Find(Vars.playerKills.ToArray(), (KeyValuePair<ulong, int> kv) => kv.Key == targetClient.userID).Value;
                                int killsRank = Array.FindIndex(sortedKills.ToArray(), (KeyValuePair<ulong, int> kv) => kv.Key == targetClient.userID) + 1;
                                int deaths = Array.Find(Vars.playerDeaths.ToArray(), (KeyValuePair<ulong, int> kv) => kv.Key == targetClient.userID).Value;
                                int deathsRank = Array.FindIndex(sortedDeaths.ToArray(), (KeyValuePair<ulong, int> kv) => kv.Key == targetClient.userID) + 1;
                                decimal kdr = 0.00M;
                                if (kills > 0 && deaths > 0)
                                    kdr = Math.Round((decimal)kills / deaths, 2);
                                Dictionary<ulong, decimal> sortedKDRs = new Dictionary<ulong, decimal>();
                                foreach (var data in sortedKills)
                                {
                                    ulong UID = data.Key;
                                    int killData = data.Value;
                                    try
                                    {
                                        if (Vars.playerDeaths.ContainsKey(UID))
                                        {
                                            int deathData = Vars.playerDeaths[UID];
                                            if (deaths > 0)
                                            {
                                                decimal KDR = (decimal)killData;
                                                if (kills > 0)
                                                    KDR = Math.Round((decimal)(killData / deathData), 2);
                                                sortedKDRs.Add(UID, KDR);
                                            }
                                            else
                                            {
                                                decimal KDR = (decimal)killData;
                                                sortedKDRs.Add(UID, KDR);
                                            }
                                        }
                                        else
                                        {
                                            decimal KDR = (decimal)killData;
                                            sortedKDRs.Add(UID, KDR);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        sortedKDRs.Add(UID, 0.00M);
                                    }
                                }

                                sortedKDRs = Vars.sortDictionaryByValue(sortedKDRs);
                                int kdrsRank = Array.FindIndex(sortedKDRs.ToArray(), (KeyValuePair<ulong, decimal> kv) => kv.Key == targetClient.userID) + 1;
                                Faction faction = Vars.factions.GetByMember(targetClient.userID);

                                Broadcast.broadcastTo(senderClient.netPlayer, "Player " + targetClient.userName + "'s information:");
                                if (isAlternative)
                                    Broadcast.broadcastTo(senderClient.netPlayer, "IP: " + targetClient.netPlayer.ipAddress);
                                Broadcast.broadcastTo(senderClient.netPlayer, "Ping:");
                                Broadcast.broadcastTo(senderClient.netPlayer, "- Recent: " + targetClient.netPlayer.lastPing + " ms");
                                Broadcast.broadcastTo(senderClient.netPlayer, "- Average: " + targetClient.netPlayer.averagePing + " ms");
                                Broadcast.broadcastTo(senderClient.netPlayer, "UID: " + targetClient.userID);
                                if (isAlternative || (Vars.enableRank && !isAlternative))
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Rank: " + Vars.findRank(targetClient.userID));
                                Character targetChar;
                                if (Vars.getPlayerChar(targetClient, out targetChar))
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Alive: " + targetChar.alive.ToString());
                                else
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Alive: False");
                                Broadcast.broadcastTo(senderClient.netPlayer, "Kills: " + kills);
                                Broadcast.broadcastTo(senderClient.netPlayer, "Kills Rank: " + killsRank);
                                Broadcast.broadcastTo(senderClient.netPlayer, "Deaths: " + deaths);
                                Broadcast.broadcastTo(senderClient.netPlayer, "Deaths Rank: " + deathsRank);
                                Broadcast.broadcastTo(senderClient.netPlayer, "KDR: " + kdr);
                                Broadcast.broadcastTo(senderClient.netPlayer, "KDR Rank: " + kdrsRank);
                                Broadcast.broadcastTo(senderClient.netPlayer, "Faction: " + (faction != null ? faction.name : "This player is not in a faction"));
                            }
                        }
                        else
                        {
                            PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                            if (possibleTargets.Count() == 0)
                                Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                            else if (possibleTargets.Count() > 1)
                                Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                            else
                            {
                                PlayerClient targetClient = possibleTargets[0];

                                if (!Vars.playerDeaths.ContainsKey(targetClient.userID))
                                    Vars.playerDeaths.Add(targetClient.userID, 0);
                                if (!Vars.playerKills.ContainsKey(targetClient.userID))
                                    Vars.playerKills.Add(targetClient.userID, 0);

                                KeyValuePair<ulong, int>[] sortedKills = Array.FindAll(Vars.sortDictionaryByValue(Vars.playerKills).ToArray(), (KeyValuePair<ulong, int> kv) => kv.Value > 0);
                                KeyValuePair<ulong, int>[] sortedDeaths = Array.FindAll(Vars.sortDictionaryByValue(Vars.playerDeaths).ToArray(), (KeyValuePair<ulong, int> kv) => kv.Value > 0);
                                int kills = Array.Find(Vars.playerKills.ToArray(), (KeyValuePair<ulong, int> kv) => kv.Key == targetClient.userID).Value;
                                int killsRank = -1;
                                if (Vars.playerKills.ContainsKey(targetClient.userID))
                                    killsRank = Array.FindIndex(sortedKills, (KeyValuePair<ulong, int> kv) => kv.Key == targetClient.userID) + 1;
                                int deaths = Array.Find(Vars.playerDeaths.ToArray(), (KeyValuePair<ulong, int> kv) => kv.Key == targetClient.userID).Value;
                                int deathsRank = Array.FindIndex(sortedDeaths, (KeyValuePair<ulong, int> kv) => kv.Key == targetClient.userID) + 1;
                                decimal kdr = 0.00M;
                                if (kills > 0 && deaths > 0)
                                    kdr = Math.Round((decimal)kills / deaths, 2);
                                Dictionary<ulong, decimal> sortedKDRs = new Dictionary<ulong, decimal>();
                                foreach (var data in sortedKills)
                                {
                                    ulong UID = data.Key;
                                    int killData = data.Value;
                                    try
                                    {
                                        int indexOf = Array.FindIndex(sortedDeaths.ToArray(), (KeyValuePair<ulong, int> kv) => kv.Key == UID);
                                        int deathData = sortedDeaths[indexOf].Value;
                                        decimal KDR = 0.00M;
                                        if (kills > 0 && deaths > 0)
                                            KDR = Math.Round((decimal)(killData / deathData), 2);
                                        sortedKDRs.Add(UID, KDR);
                                    }
                                    catch (Exception ex)
                                    {
                                        sortedKDRs.Add(UID, 0.00M);
                                    }
                                }

                                sortedKDRs = Vars.sortDictionaryByValue(sortedKDRs);
                                int kdrsRank = Array.FindIndex(sortedKDRs.ToArray(), (KeyValuePair<ulong, decimal> kv) => kv.Key == targetClient.userID) + 1;
                                Faction faction = Vars.factions.GetByMember(targetClient.userID);

                                Broadcast.broadcastTo(senderClient.netPlayer, "Player " + targetClient.userName + "'s information:");
                                if (isAlternative)
                                    Broadcast.broadcastTo(senderClient.netPlayer, "IP: " + targetClient.netPlayer.ipAddress);
                                Broadcast.broadcastTo(senderClient.netPlayer, "Ping:");
                                Broadcast.broadcastTo(senderClient.netPlayer, "- Recent: " + targetClient.netPlayer.lastPing + " ms");
                                Broadcast.broadcastTo(senderClient.netPlayer, "- Average: " + targetClient.netPlayer.averagePing + " ms");
                                Broadcast.broadcastTo(senderClient.netPlayer, "UID: " + targetClient.userID);
                                if (isAlternative || (Vars.enableRank && !isAlternative))
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Rank: " + Vars.findRank(targetClient.userID));
                                Character targetChar;
                                if (Vars.getPlayerChar(targetClient, out targetChar))
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Alive: " + targetChar.alive);
                                else
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Alive: False");
                                Broadcast.broadcastTo(senderClient.netPlayer, "Kills: " + kills);
                                Broadcast.broadcastTo(senderClient.netPlayer, "Kills Rank: " + killsRank);
                                Broadcast.broadcastTo(senderClient.netPlayer, "Deaths: " + deaths);
                                Broadcast.broadcastTo(senderClient.netPlayer, "Deaths Rank: " + deathsRank);
                                Broadcast.broadcastTo(senderClient.netPlayer, "KDR: " + kdr);
                                Broadcast.broadcastTo(senderClient.netPlayer, "KDR Rank: " + kdrsRank);
                                Broadcast.broadcastTo(senderClient.netPlayer, "Faction: " + (faction != null ? faction.name : "This player is not in a faction"));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("DPI: " + ex.ToString());
                    }
                }
            }
        }

        public static string getIP()
        {

            if (!string.IsNullOrEmpty(Vars.serverIP))
                return Vars.serverIP;

            try
            {
                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.Proxy = null;
                        return wc.DownloadString("http://pwnoz0r.com/software/PwnZPanel/Config/ip.php").Replace(" ", "");
                    }
                }
                catch (Exception ex)
                {
                    Vars.conLog.Error("GETIP #1: " + ex.ToString());
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("GETIP #2: " + ex.ToString());
            }
            return "NULL";
        }

        public static void displayServerInfo(PlayerClient senderClient)
        {
            if (senderClient != null)
            {
                try
                {
                    Thread t = new Thread(() =>
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "Server name: " + server.hostname);
                        Broadcast.broadcastTo(senderClient.netPlayer, "Server IP: " + getIP() + ":" + server.port);
                    });
                    t.IsBackground = true;
                    t.Start();
                }
                catch (Exception ex)
                {
                    Vars.conLog.Error("DSI: " + ex.ToString());
                }
            }
        }

        public static void displayHistory(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                try
                {
                    int lineAmount = 0;
                    string mode = "g";
                    if (args.Count() > 2)
                    {
                        lineAmount = Convert.ToInt16(args[2]);
                        mode = args[1];
                    }
                    else
                        lineAmount = Convert.ToInt16(args[1]);

                    if (lineAmount > 50)
                        lineAmount = 50;

                    switch (mode)
                    {
                        case "g":
                            int curIndex = 0;
                            int goalIndex = Vars.historyGlobal.Count - lineAmount;
                            foreach (string s in Vars.historyGlobal)
                            {
                                if (curIndex >= goalIndex)
                                {
                                    string playerName = s.Split(new string[] { "$:|:$" }, StringSplitOptions.None)[0].ToString();
                                    string message = s.Split(new string[] { "$:|:$" }, StringSplitOptions.None)[1].ToString();

                                    Broadcast.broadcastCustomTo(senderClient.netPlayer, playerName, message);
                                }
                                curIndex++;
                            }
                            break;
                        case "f":
                            Faction faction = Vars.factions.GetByMember(senderClient.userID);
                            if (faction != null)
                            {
                                try
                                {
                                    if (Vars.historyFaction.ContainsKey(faction.name))
                                    {
                                        curIndex = 0;
                                        goalIndex = Vars.historyFaction[faction.name].Count - lineAmount;
                                        foreach (string s in Vars.historyFaction[faction.name])
                                        {
                                            if (curIndex >= goalIndex)
                                            {
                                                string playerName = s.Split(new string[] { "$:|:$" }, StringSplitOptions.None)[0].ToString();
                                                string message = s.Split(new string[] { "$:|:$" }, StringSplitOptions.None)[1].ToString();

                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, playerName, message);
                                            }
                                            curIndex++;
                                        }
                                    }
                                    else
                                    {
                                        Broadcast.broadcastTo(senderClient.netPlayer, "You have no faction chat history!");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Vars.conLog.Error("HIST: " + ex.ToString());
                                }
                            }
                            break;
                        default:
                            Broadcast.broadcastTo(senderClient.netPlayer, "No such chat channel \"" + mode + "\"!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Broadcast.broadcastTo(senderClient.netPlayer, "Line amount must be an integer!");
                }
            }
            else
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "You must specify a number, 1 - 50, of lines!");
            }
        }

        public static void displayFrozen(PlayerClient senderClient, string[] args)
        {
            if (senderClient != null)
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "All frozen players:");
                List<string> names = new List<string>();
                List<string> names2 = new List<string>();
                foreach (var player in Vars.frozenPlayers)
                {
                    ulong UID = player.Key;
                    uLink.NetworkPlayer np = Vars.getNetPlayer(UID);
                    PlayerClient playerClient = Vars.getPlayerClient(np);
                    if (playerClient != null)
                    {
                        if (playerClient.userName.Length > 0)
                            names.Add(playerClient.userName);
                    }
                }

                List<string> otherNames = new List<string>();
                while (names.Count > 0)
                {
                    names2.Clear();
                    otherNames.Clear();
                    foreach (string s in names)
                    {
                        names2.Add(s);
                        otherNames.Add(s);

                        if ((string.Join(", ", names2.ToArray())).Length > 70)
                        {
                            names2.Remove(s);
                            otherNames.Remove(s);
                            break;
                        }
                    }
                    foreach (string s in otherNames)
                    {
                        names.Remove(s);
                    }
                    Broadcast.broadcastTo(senderClient.netPlayer, string.Join(", ", names2.ToArray()));
                }
            }
        }

        public static void displayNearby(PlayerClient senderClient, string[] args)
        {
            if (senderClient != null)
            {
                Character senderChar = Vars.getPlayerChar(senderClient);
                if (senderChar != null)
                {
                    Vector3 senderPos = senderChar.transform.position;
                    float radius;
                    if (args.Count() > 1 && Single.TryParse(args[1], out radius))
                    {
                        List<Character> nearbyCharacters = new List<Character>();
                        List<PlayerClient> playerClients = new List<PlayerClient>();
                        foreach (PlayerClient pc in Vars.AllPlayerClients) { playerClients.Add(pc); }

                        foreach (PlayerClient pc in playerClients)
                        {
                            if (pc != null)
                            {
                                if (pc != senderClient)
                                {
                                    Character playerChar = Vars.getPlayerChar(pc);
                                    if (playerChar != null && playerChar.alive)
                                    {
                                        Vector3 playerPos = playerChar.transform.position;
                                        if (Vector3.Distance(senderPos, playerPos) <= radius)
                                        {
                                            if (!nearbyCharacters.Contains(playerChar))
                                                nearbyCharacters.Add(playerChar);
                                        }
                                    }
                                }
                            }
                        }
                        if (nearbyCharacters.Count() > 0)
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "Players in a " + radius + " meter radius:");
                            List<string> names = new List<string>();
                            List<string> names2 = new List<string>();
                            foreach (Character c in nearbyCharacters)
                            {
                                if (c.netUser.displayName.Length > 0)
                                    names.Add(c.netUser.displayName);
                            }

                            List<string> otherNames = new List<string>();
                            while (names.Count > 0)
                            {
                                names2.Clear();
                                otherNames.Clear();
                                foreach (string s in names)
                                {
                                    names2.Add(s);
                                    otherNames.Add(s);

                                    if ((string.Join(", ", names2.ToArray())).Length > 70)
                                    {
                                        names2.Remove(s);
                                        otherNames.Remove(s);
                                        break;
                                    }
                                }
                                foreach (string s in otherNames)
                                {
                                    names.Remove(s);
                                }
                                Broadcast.broadcastTo(senderClient.netPlayer, string.Join(", ", names2.ToArray()));
                            }
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "There are no players within a " + radius + " meter radius of you.");
                    }
                    else
                        Broadcast.broadcastTo(senderClient.netPlayer, "Radius must be a number!");
                }
            }
        }

        public static void displayRules(PlayerClient senderClient)
        {
            if (Vars.motdList.ContainsKey("Rules"))
            {
                foreach (string s in Vars.motdList["Rules"])
                {
                    Broadcast.broadcastTo(senderClient.netPlayer, s);
                }
            }
        }

        public static void displayWarps(PlayerClient senderClient)
        {
            string rank = Vars.findRank(senderClient.userID);

            Broadcast.broadcastTo(senderClient.netPlayer, "Available warps:");
            Vars.listWarps(rank, senderClient);
        }

        public static void displayKits(PlayerClient senderClient)
        {
            string rank = Vars.findRank(senderClient.userID);

            Broadcast.broadcastTo(senderClient.netPlayer, "Available kits:");
            Vars.listKits(rank, senderClient);
        }

        public static void displayPlayers(PlayerClient senderClient)
        {
            Broadcast.broadcastTo(senderClient.netPlayer, "All online players:");
            List<string> names = new List<string>();
            List<string> names2 = new List<string>();
            foreach (PlayerClient pc in Vars.AllPlayerClients.ToArray())
            {
                if (pc.userName.Length > 0)
                    names.Add(pc.userName);
            }

            List<string> otherNames = new List<string>();
            while (names.Count > 0)
            {
                names2.Clear();
                otherNames.Clear();
                foreach (string s in names)
                {
                    names2.Add(s);
                    otherNames.Add(s);

                    if ((string.Join(", ", names2.ToArray())).Length > 70)
                    {
                        names2.Remove(s);
                        otherNames.Remove(s);
                        break;
                    }
                }
                foreach (string s in otherNames)
                {
                    names.Remove(s);
                }
                Broadcast.broadcastTo(senderClient.netPlayer, string.Join(", ", names2.ToArray()));
            }
        }
    }
}
