using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RustEssentials.Util
{
    public static class Factions
    {
        public static void handleFactions(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string arg = args[1];
                KeyValuePair<string, Dictionary<string, string>>[] possibleFactions = Array.FindAll(Vars.factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(senderClient.userID.ToString()));

                string rankToUse = Vars.findRank(senderClient.userID.ToString());
                if (!Vars.enabledCommands.ContainsKey(rankToUse))
                    rankToUse = Vars.defaultRank;

                switch (arg)
                {
                    case "create":
                        if (args.Count() > 2)
                        {
                            List<string> messageList = new List<string>();
                            int curIndex = 0;
                            foreach (string s in args)
                            {
                                if (curIndex > 1)
                                    messageList.Add(s);
                                curIndex++;
                            }

                            string factionName = string.Join(" ", messageList.ToArray());

                            if (!Vars.factions.ContainsKey(factionName) && !Vars.factionsByNames.ContainsKey(factionName) && !Vars.alliances.ContainsKey(factionName))
                            {
                                if (possibleFactions.Count() == 0)
                                {
                                    if (factionName.Length < 16)
                                    {
                                        if (!factionName.Contains("=") && !factionName.Contains(";") && !factionName.Contains(":"))
                                        {
                                            Vars.factions.Add(factionName, new Dictionary<string, string>());
                                            Vars.factionsByNames.Add(factionName, new Dictionary<string, string>());
                                            Vars.alliances.Add(factionName, new List<string>());
                                            Vars.factions[factionName].Add(senderClient.userID.ToString(), "owner");
                                            Vars.factionsByNames[factionName].Add(senderClient.userID.ToString(), senderClient.userName);
                                            Broadcast.broadcastTo(senderClient.netPlayer, "Faction [" + factionName + "] created.");
                                            Data.addFactionData(factionName, senderClient.userName, senderClient.userID.ToString(), "owner");
                                        }
                                        else
                                            Broadcast.broadcastTo(senderClient.netPlayer, "Faction names cannot contain =, :, or ;!");
                                    }
                                    else
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Faction names must be less than 16 characters!");
                                }
                                else
                                    Broadcast.broadcastTo(senderClient.netPlayer, "You are already in the faction [" + factionName + "].");
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "Faction [" + factionName + "] already exists.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "Improper syntax! Syntax: /f create *name*");
                        break;
                    case "disband":
                        if (possibleFactions.Count() > 0)
                        {
                            string rank = possibleFactions[0].Value[senderClient.userID.ToString()];

                            if (Vars.completeDoorAccess.Contains(senderClient.userID.ToString()))
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "Faction disbanded.");

                            if (rank == "owner" || Vars.completeDoorAccess.Contains(senderClient.userID.ToString()))
                            {
                                PlayerClient[] targetClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                foreach (PlayerClient pc in targetClients)
                                {
                                    Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, "Your faction was disbanded.");
                                }
                                Vars.factions.Remove(possibleFactions[0].Key);
                                Vars.factionsByNames.Remove(possibleFactions[0].Key);
                                Vars.alliances.Remove(possibleFactions[0].Key);
                                Data.remFactionData(possibleFactions[0].Key, "disband", "");
                                Data.remAlliesData(possibleFactions[0].Key, "disband");
                            }
                            else
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You do not have permission to disband your current faction.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        break;
                    case "kick":
                        if (possibleFactions.Count() > 0)
                        {
                            string rank = possibleFactions[0].Value[senderClient.userID.ToString()];

                            if (rank == "owner" || rank == "admin")
                            {
                                List<string> messageList = new List<string>();
                                int curIndex = 0;
                                foreach (string s in args)
                                {
                                    if (curIndex > 1)
                                        messageList.Add(s);
                                    curIndex++;
                                }

                                string targetName = string.Join(" ", messageList.ToArray());

                                if (targetName.StartsWith("\"") && targetName.EndsWith("\""))
                                {
                                    targetName = targetName.Substring(1, targetName.Length - 2);

                                    PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(targetName));

                                    if (possibleTargets.Count() == 0)
                                    {
                                        List<string> possibleUIDs = new List<string>();
                                        foreach (KeyValuePair<string, string> kv in Vars.factionsByNames[possibleFactions[0].Key])
                                        {
                                            if (kv.Value.Equals(targetName) && targetName != senderClient.userID.ToString())
                                                possibleUIDs.Add(kv.Key);
                                        }

                                        if (possibleUIDs.Count() == 0)
                                        {
                                            Broadcast.broadcastTo(senderClient.netPlayer, "No member name or UID equals \"" + targetName + "\".");
                                        }
                                        else if (possibleUIDs.Count() > 1)
                                        {
                                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many member names or UID equal \"" + targetName + "\".");
                                        }
                                        else
                                        {
                                            PlayerClient[] targetClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                            foreach (PlayerClient pc in targetClients)
                                            {
                                                Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, Vars.factionsByNames[possibleFactions[0].Key][possibleUIDs[0]] + " was kicked from the faction.");
                                            }
                                            Data.remFactionData(possibleFactions[0].Key, Vars.factionsByNames[possibleFactions[0].Key][possibleUIDs[0]], possibleFactions[0].Value[possibleUIDs[0]]);
                                            Vars.factions[possibleFactions[0].Key].Remove(possibleUIDs[0]);
                                            Vars.factionsByNames[possibleFactions[0].Key].Remove(possibleUIDs[0]);
                                        }
                                    }
                                    else if (possibleTargets.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many member names equal \"" + targetName + "\".");
                                    else
                                    {
                                        PlayerClient targetClient = possibleTargets[0];

                                        if (targetClient != senderClient)
                                        {
                                            if (possibleFactions[0].Value.ContainsKey(targetClient.userID.ToString()))
                                            {
                                                PlayerClient[] targetClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                                foreach (PlayerClient pc in targetClients)
                                                {
                                                    Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, targetClient.userName + " was kicked from the faction.");
                                                }
                                                Data.remFactionData(possibleFactions[0].Key, targetClient.userName, possibleFactions[0].Value[targetClient.userID.ToString()]);
                                                Vars.factions[possibleFactions[0].Key].Remove(targetClient.userID.ToString());
                                                Vars.factionsByNames[possibleFactions[0].Key].Remove(targetClient.userID.ToString());
                                                if (Vars.latestFactionRequests.ContainsKey(targetClient.userID))
                                                    Vars.latestFactionRequests.Remove(targetClient.userID);
                                            }
                                            else
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, targetClient.userName + " is not in your faction.");
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You cannot kick yourself from the faction.");
                                    }
                                }
                                else
                                {
                                    PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));

                                    if (possibleTargets.Count() == 0)
                                    {
                                        List<string> possibleUIDs = new List<string>();
                                        foreach (KeyValuePair<string, string> kv in Vars.factionsByNames[possibleFactions[0].Key])
                                        {
                                            if (kv.Value.Contains(targetName) && targetName != senderClient.userID.ToString())
                                                possibleUIDs.Add(kv.Key);
                                        }

                                        if (possibleUIDs.Count() == 0)
                                        {
                                            Broadcast.broadcastTo(senderClient.netPlayer, "No member name or UID contain \"" + targetName + "\".");
                                        }
                                        else if (possibleUIDs.Count() > 1)
                                        {
                                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many member names or UID contain \"" + targetName + "\".");
                                        }
                                        else
                                        {
                                            PlayerClient[] targetClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                            foreach (PlayerClient pc in targetClients)
                                            {
                                                Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, Vars.factionsByNames[possibleFactions[0].Key][possibleUIDs[0]] + " was kicked from the faction.");
                                            }
                                            Data.remFactionData(possibleFactions[0].Key, Vars.factionsByNames[possibleFactions[0].Key][possibleUIDs[0]], possibleFactions[0].Value[possibleUIDs[0]]);
                                            Vars.factions[possibleFactions[0].Key].Remove(possibleUIDs[0]);
                                            Vars.factionsByNames[possibleFactions[0].Key].Remove(possibleUIDs[0]);
                                        }
                                    }
                                    else if (possibleTargets.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many member names contain \"" + targetName + "\".");
                                    else
                                    {
                                        PlayerClient targetClient = possibleTargets[0];

                                        if (targetClient != senderClient)
                                        {
                                            if (possibleFactions[0].Value.ContainsKey(targetClient.userID.ToString()))
                                            {
                                                PlayerClient[] targetClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                                foreach (PlayerClient pc in targetClients)
                                                {
                                                    Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, targetClient.userName + " was kicked from the faction.");
                                                }
                                                Data.remFactionData(possibleFactions[0].Key, targetClient.userName, possibleFactions[0].Value[targetClient.userID.ToString()]);
                                                Vars.factions[possibleFactions[0].Key].Remove(targetClient.userID.ToString());
                                                Vars.factionsByNames[possibleFactions[0].Key].Remove(targetClient.userID.ToString());
                                                if (Vars.latestFactionRequests.ContainsKey(targetClient.userID))
                                                    Vars.latestFactionRequests.Remove(targetClient.userID);
                                            }
                                            else
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, targetClient.userName + " is not in your faction.");
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You cannot kick yourself from the faction.");
                                    }
                                }
                            }
                            else
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You do not have permission to kick members of your faction.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        break;
                    case "invite":
                        if (possibleFactions.Count() > 0)
                        {
                            string rank = possibleFactions[0].Value[senderClient.userID.ToString()];

                            if (rank == "owner" || rank == "admin")
                            {
                                List<string> messageList = new List<string>();
                                int curIndex = 0;
                                foreach (string s in args)
                                {
                                    if (curIndex > 1)
                                        messageList.Add(s);
                                    curIndex++;
                                }

                                string targetName = string.Join(" ", messageList.ToArray());

                                if (targetName.StartsWith("\"") && targetName.EndsWith("\""))
                                {
                                    targetName = targetName.Substring(1, targetName.Length - 2);

                                    PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(targetName));

                                    if (possibleTargets.Count() == 0)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "No players equal \"" + targetName + "\".");
                                    else if (possibleTargets.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + targetName + "\".");
                                    else
                                    {
                                        PlayerClient targetClient = possibleTargets[0];

                                        if (possibleFactions[0].Value.Count < Vars.maxMembers)
                                        {
                                            if (!possibleFactions[0].Value.ContainsKey(targetClient.userID.ToString()))
                                            {
                                                KeyValuePair<string, Dictionary<string, string>>[] targetFactions = Array.FindAll(Vars.factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(targetClient.userID.ToString()));
                                                if (targetFactions.Count() == 0)
                                                {
                                                    if (!Vars.factionInvites.ContainsKey(targetClient.userID.ToString()))
                                                    {
                                                        Vars.factionInvites.Add(targetClient.userID.ToString(), new List<string>() { { possibleFactions[0].Key } });

                                                        Broadcast.broadcastTo(senderClient.netPlayer, "You invited \"" + targetClient.userName + "\" to the faction.");
                                                        Broadcast.broadcastTo(targetClient.netPlayer, "You were invited to the faction [" + possibleFactions[0].Key + "]. Type /f join to join.");
                                                        if (!Vars.latestFactionRequests.ContainsKey(targetClient.userID))
                                                            Vars.latestFactionRequests.Add(targetClient.userID, senderClient.userID);
                                                        else
                                                            Vars.latestFactionRequests[targetClient.userID] = senderClient.userID;
                                                    }
                                                    else
                                                    {
                                                        if (!Vars.factionInvites[targetClient.userID.ToString()].Contains(possibleFactions[0].Key))
                                                        {
                                                            Vars.factionInvites[targetClient.userID.ToString()].Add(possibleFactions[0].Key);

                                                            Broadcast.broadcastTo(senderClient.netPlayer, "You invited \"" + targetClient.userName + "\" to the faction.");
                                                            Broadcast.broadcastTo(targetClient.netPlayer, "You were invited to the faction [" + possibleFactions[0].Key + "]. Type /f join to join.");
                                                            if (!Vars.latestFactionRequests.ContainsKey(targetClient.userID))
                                                                Vars.latestFactionRequests.Add(targetClient.userID, senderClient.userID);
                                                            else
                                                                Vars.latestFactionRequests[targetClient.userID] = senderClient.userID;
                                                        }
                                                        else
                                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, targetClient.userName + " has already been invited.");
                                                    }
                                                }
                                                else
                                                    Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, targetClient.userName + " is already in the faction [" + targetFactions[0].Key + "].");
                                            }
                                            else
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, targetClient.userName + " is already in the faction.");
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You have reached your member capacity.");
                                    }
                                }
                                else
                                {
                                    PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));

                                    if (possibleTargets.Count() == 0)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "No players equal or contain \"" + targetName + "\".");
                                    else if (possibleTargets.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + targetName + "\".");
                                    else
                                    {
                                        PlayerClient targetClient = possibleTargets[0];

                                        if (possibleFactions[0].Value.Count < Vars.maxMembers)
                                        {
                                            if (!possibleFactions[0].Value.ContainsKey(targetClient.userID.ToString()))
                                            {
                                                KeyValuePair<string, Dictionary<string, string>>[] targetFactions = Array.FindAll(Vars.factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(targetClient.userID.ToString()));
                                                if (targetFactions.Count() == 0)
                                                {
                                                    if (!Vars.factionInvites.ContainsKey(targetClient.userID.ToString()))
                                                    {
                                                        Vars.factionInvites.Add(targetClient.userID.ToString(), new List<string>() { { possibleFactions[0].Key } });

                                                        Broadcast.broadcastTo(senderClient.netPlayer, "You invited " + targetClient.userName + " to the faction.");
                                                        Broadcast.broadcastTo(targetClient.netPlayer, "You were invited to the faction [" + possibleFactions[0].Key + "]. Type /f join to join.");
                                                        if (!Vars.latestFactionRequests.ContainsKey(targetClient.userID))
                                                            Vars.latestFactionRequests.Add(targetClient.userID, senderClient.userID);
                                                        else
                                                            Vars.latestFactionRequests[targetClient.userID] = senderClient.userID;
                                                    }
                                                    else
                                                    {
                                                        if (!Vars.factionInvites[targetClient.userID.ToString()].Contains(possibleFactions[0].Key))
                                                        {
                                                            Vars.factionInvites[targetClient.userID.ToString()].Add(possibleFactions[0].Key);

                                                            Broadcast.broadcastTo(senderClient.netPlayer, "You invited " + targetClient.userName + " to the faction.");
                                                            Broadcast.broadcastTo(targetClient.netPlayer, "You were invited to the faction [" + possibleFactions[0].Key + "]. Type /f join to join.");
                                                            if (!Vars.latestFactionRequests.ContainsKey(targetClient.userID))
                                                                Vars.latestFactionRequests.Add(targetClient.userID, senderClient.userID);
                                                            else
                                                                Vars.latestFactionRequests[targetClient.userID] = senderClient.userID;
                                                        }
                                                        else
                                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, targetClient.userName + " has already been invited.");
                                                    }
                                                }
                                                else
                                                    Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, targetClient.userName + " is already in the faction [" + targetFactions[0].Key + "].");
                                            }
                                            else
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, targetClient.userName + " is already in the faction.");
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You have reached your member capacity.");
                                    }
                                }
                            }
                            else
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You do not have permission to invite members to your faction.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        break;
                    case "join":
                        if (possibleFactions.Count() == 0)
                        {
                            if (Vars.factionInvites.ContainsKey(senderClient.userID.ToString()))
                            {
                                if (args.Count() > 2)
                                {
                                    List<string> messageList = new List<string>();
                                    int curIndex = 0;
                                    foreach (string s in args)
                                    {
                                        if (curIndex > 1)
                                            messageList.Add(s);
                                        curIndex++;
                                    }

                                    string targetFaction = string.Join(" ", messageList.ToArray());
                                    List<string> inviterFactions = new List<string>();
                                    foreach (string s in Vars.factionInvites[senderClient.userID.ToString()])
                                    {
                                        if (s.Contains(targetFaction))
                                        {
                                            inviterFactions.Add(s);
                                        }
                                    }

                                    if (inviterFactions.Count() == 0)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "No invitations from factions that equal or contain \"" + targetFaction + "\".");
                                    else if (inviterFactions.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many invitations from factions that names contain \"" + targetFaction + "\".");
                                    else
                                    {
                                        Vars.factionInvites[senderClient.userID.ToString()].Remove(inviterFactions[0]);
                                        Vars.factions[inviterFactions[0]].Add(senderClient.userID.ToString(), "normal");
                                        Vars.factionsByNames[inviterFactions[0]].Add(senderClient.userID.ToString(), senderClient.userName);
                                        PlayerClient[] targetClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => Vars.factions[inviterFactions[0]].ContainsKey(pc.userID.ToString()));
                                        foreach (PlayerClient pc in targetClients)
                                        {
                                            Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + inviterFactions[0], senderClient.userName + " has joined the faction.");
                                        }
                                        Data.addFactionData(inviterFactions[0], senderClient.userName, senderClient.userID.ToString(), "normal");
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        if (Vars.latestFactionRequests.ContainsKey(senderClient.userID))
                                        {
                                            KeyValuePair<string, Dictionary<string, string>> inviterFaction = Array.Find(Vars.factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(Vars.latestFactionRequests[senderClient.userID].ToString()));
                                            KeyValuePair<string, Dictionary<string, string>> inviterFactionByName = Array.Find(Vars.factionsByNames.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(Vars.latestFactionRequests[senderClient.userID].ToString()));
                                            Vars.factionInvites[senderClient.userID.ToString()].Remove(inviterFaction.Key);
                                            inviterFaction.Value.Add(senderClient.userID.ToString(), "normal");
                                            inviterFactionByName.Value.Add(senderClient.userID.ToString(), senderClient.userName);
                                            PlayerClient[] targetClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => inviterFaction.Value.ContainsKey(pc.userID.ToString()));
                                            foreach (PlayerClient pc in targetClients)
                                            {
                                                Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + inviterFaction.Key, senderClient.userName + " has joined the faction.");
                                            }
                                            Data.addFactionData(inviterFaction.Key, senderClient.userName, senderClient.userID.ToString(), "normal");
                                        }
                                        else
                                            Broadcast.broadcastTo(senderClient.netPlayer, "No invitations from any factions.");
                                    }
                                    catch (Exception ex)
                                    {
                                        Vars.conLog.Error("FJOIN: " + ex.ToString());
                                    }
                                }
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "You do not have any faction invites.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You must leave your current faction first.");
                        break;
                    case "leave":
                        try
                        {
                            if (possibleFactions.Count() > 0)
                            {
                                string rank = possibleFactions[0].Value[senderClient.userID.ToString()];

                                if (rank != "owner")
                                {
                                    PlayerClient[] targetClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                    foreach (PlayerClient pc in targetClients)
                                    {
                                        Broadcast.broadcastCustomTo(pc.netPlayer, possibleFactions[0].Key, senderClient.userName + " has left the faction.");
                                    }
                                    try
                                    {
                                        Data.remFactionData(possibleFactions[0].Key, senderClient.userName, possibleFactions[0].Value[senderClient.userID.ToString()]);
                                        Vars.factions[possibleFactions[0].Key].Remove(senderClient.userID.ToString());
                                        Vars.factionsByNames[possibleFactions[0].Key].Remove(senderClient.userID.ToString());
                                        if (Vars.latestFactionRequests.ContainsKey(senderClient.userID))
                                            Vars.latestFactionRequests.Remove(senderClient.userID);
                                    }
                                    catch (Exception ex)
                                    {
                                        Vars.conLog.Error("FLEAVE: " + ex.ToString());
                                    }
                                }
                                else
                                    Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You cannot leave your faction without disbanding or passing ownership.");
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        }
                        catch (Exception ex)
                        {
                            Vars.conLog.Error("FLEAVE #2: " + ex.ToString());
                        }
                        break;
                    case "admin":
                        if (possibleFactions.Count() > 0)
                        {
                            string rank = possibleFactions[0].Value[senderClient.userID.ToString()];

                            if (rank == "owner")
                            {
                                List<string> messageList = new List<string>();
                                int curIndex = 0;
                                foreach (string s in args)
                                {
                                    if (curIndex > 1)
                                        messageList.Add(s);
                                    curIndex++;
                                }

                                string targetName = string.Join(" ", messageList.ToArray());

                                if (targetName.StartsWith("\"") && targetName.EndsWith("\""))
                                {
                                    targetName = targetName.Substring(1, targetName.Length - 2);

                                    PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(targetName));

                                    if (possibleTargets.Count() == 0)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "No players equal \"" + targetName + "\".");
                                    else if (possibleTargets.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + targetName + "\".");
                                    else
                                    {
                                        PlayerClient targetClient = possibleTargets[0];

                                        if (targetClient != senderClient)
                                        {
                                            if (possibleFactions[0].Value[targetClient.userID.ToString()] != "admin")
                                            {
                                                Data.remFactionData(possibleFactions[0].Key, targetClient.userName, possibleFactions[0].Value[targetClient.userID.ToString()]);
                                                possibleFactions[0].Value.Remove(targetClient.userID.ToString());
                                                possibleFactions[0].Value.Add(targetClient.userID.ToString(), "admin");
                                                Data.addFactionData(possibleFactions[0].Key, targetClient.userName, targetClient.userID.ToString(), "admin");
                                                PlayerClient[] targetClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                                foreach (PlayerClient pc in targetClients)
                                                {
                                                    Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, "Player " + targetClient.userName + " is now a faction admin.");
                                                }
                                            }
                                            else
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You cannot admin a player who is already an admin.");
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You cannot admin yourself.");
                                    }
                                }
                                else
                                {
                                    PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));

                                    if (possibleTargets.Count() == 0)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "No players equal or contain \"" + targetName + "\".");
                                    else if (possibleTargets.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + targetName + "\".");
                                    else
                                    {
                                        PlayerClient targetClient = possibleTargets[0];

                                        if (targetClient != senderClient)
                                        {
                                            if (possibleFactions[0].Value[targetClient.userID.ToString()] != "admin")
                                            {
                                                Data.remFactionData(possibleFactions[0].Key, targetClient.userName, possibleFactions[0].Value[targetClient.userID.ToString()]);
                                                possibleFactions[0].Value.Remove(targetClient.userID.ToString());
                                                possibleFactions[0].Value.Add(targetClient.userID.ToString(), "admin");
                                                Data.addFactionData(possibleFactions[0].Key, targetClient.userName, targetClient.userID.ToString(), "admin");
                                                PlayerClient[] targetClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                                foreach (PlayerClient pc in targetClients)
                                                {
                                                    Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, "Player \"" + targetClient.userName + "\" is now a faction admin.");
                                                }
                                            }
                                            else
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You cannot admin a player who is already an admin.");
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You cannot admin yourself.");
                                    }
                                }
                            }
                            else
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You do not have permission to assign admin to players.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        break;
                    case "deadmin":
                        if (possibleFactions.Count() > 0)
                        {
                            string rank = possibleFactions[0].Value[senderClient.userID.ToString()];

                            if (rank == "owner")
                            {
                                List<string> messageList = new List<string>();
                                int curIndex = 0;
                                foreach (string s in args)
                                {
                                    if (curIndex > 1)
                                        messageList.Add(s);
                                    curIndex++;
                                }

                                string targetName = string.Join(" ", messageList.ToArray());

                                if (targetName.StartsWith("\"") && targetName.EndsWith("\""))
                                {
                                    targetName = targetName.Substring(1, targetName.Length - 2);

                                    PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(targetName));

                                    if (possibleTargets.Count() == 0)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "No players equal \"" + targetName + "\".");
                                    else if (possibleTargets.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + targetName + "\".");
                                    else
                                    {
                                        PlayerClient targetClient = possibleTargets[0];

                                        if (targetClient != senderClient)
                                        {
                                            if (possibleFactions[0].Value[targetClient.userID.ToString()] == "admin")
                                            {
                                                Data.remFactionData(possibleFactions[0].Key, targetClient.userName, possibleFactions[0].Value[targetClient.userID.ToString()]);
                                                possibleFactions[0].Value.Remove(targetClient.userID.ToString());
                                                possibleFactions[0].Value.Add(targetClient.userID.ToString(), "normal");
                                                Data.addFactionData(possibleFactions[0].Key, targetClient.userName, targetClient.userID.ToString(), "normal");
                                                PlayerClient[] targetClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                                foreach (PlayerClient pc in targetClients)
                                                {
                                                    Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, "Player " + targetClient.userName + " is no longer a faction admin.");
                                                }
                                            }
                                            else
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You cannot deadmin a player who is not an admin.");
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You cannot deadmin yourself.");
                                    }
                                }
                                else
                                {
                                    PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));

                                    if (possibleTargets.Count() == 0)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "No players equal or contain \"" + targetName + "\".");
                                    else if (possibleTargets.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + targetName + "\".");
                                    else
                                    {
                                        PlayerClient targetClient = possibleTargets[0];

                                        if (targetClient != senderClient)
                                        {
                                            if (possibleFactions[0].Value[targetClient.userID.ToString()] == "admin")
                                            {
                                                Data.remFactionData(possibleFactions[0].Key, targetClient.userName, possibleFactions[0].Value[targetClient.userID.ToString()]);
                                                possibleFactions[0].Value.Remove(targetClient.userID.ToString());
                                                possibleFactions[0].Value.Add(targetClient.userID.ToString(), "normal");
                                                Data.addFactionData(possibleFactions[0].Key, targetClient.userName, targetClient.userID.ToString(), "normal");
                                                PlayerClient[] targetClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                                foreach (PlayerClient pc in targetClients)
                                                {
                                                    Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, "Player " + targetClient.userName + " is no longer a faction admin.");
                                                }
                                            }
                                            else
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You cannot deadmin a player who is not an admin.");
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You cannot deadmin yourself.");
                                    }
                                }
                            }
                            else
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You do not have permission to revoke admin from players.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        break;
                    case "ownership":
                        if (possibleFactions.Count() > 0)
                        {
                            string rank = possibleFactions[0].Value[senderClient.userID.ToString()];

                            if (rank == "owner")
                            {
                                List<string> messageList = new List<string>();
                                int curIndex = 0;
                                foreach (string s in args)
                                {
                                    if (curIndex > 1)
                                        messageList.Add(s);
                                    curIndex++;
                                }

                                string targetName = string.Join(" ", messageList.ToArray());

                                if (targetName.StartsWith("\"") && targetName.EndsWith("\""))
                                {
                                    targetName = targetName.Substring(1, targetName.Length - 2);

                                    PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(targetName));

                                    if (possibleTargets.Count() == 0)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "No players equal \"" + targetName + "\".");
                                    else if (possibleTargets.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + targetName + "\".");
                                    else
                                    {
                                        PlayerClient targetClient = possibleTargets[0];

                                        if (targetClient != senderClient)
                                        {
                                            Data.remFactionData(possibleFactions[0].Key, senderClient.userName, possibleFactions[0].Value[senderClient.userID.ToString()]);
                                            possibleFactions[0].Value.Remove(senderClient.userID.ToString());

                                            Data.remFactionData(possibleFactions[0].Key, targetClient.userName, possibleFactions[0].Value[targetClient.userID.ToString()]);
                                            possibleFactions[0].Value.Remove(targetClient.userID.ToString());
                                            possibleFactions[0].Value.Add(targetClient.userID.ToString(), "owner");
                                            Data.addFactionData(possibleFactions[0].Key, targetClient.userName, targetClient.userID.ToString(), "owner");

                                            possibleFactions[0].Value.Add(senderClient.userID.ToString(), "normal");
                                            Data.addFactionData(possibleFactions[0].Key, senderClient.userName, targetClient.userID.ToString(), "normal");

                                            PlayerClient[] targetClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                            foreach (PlayerClient pc in targetClients)
                                            {
                                                Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, targetClient.userName + " now owns the faction.");
                                            }
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You are already the owner of this faction.");
                                    }
                                }
                                else
                                {
                                    PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));

                                    if (possibleTargets.Count() == 0)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "No players equal or contain \"" + targetName + "\".");
                                    else if (possibleTargets.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + targetName + "\".");
                                    else
                                    {
                                        PlayerClient targetClient = possibleTargets[0];

                                        if (targetClient != senderClient)
                                        {
                                            Data.remFactionData(possibleFactions[0].Key, senderClient.userName, possibleFactions[0].Value[senderClient.userID.ToString()]);
                                            possibleFactions[0].Value.Remove(senderClient.userID.ToString());

                                            Data.remFactionData(possibleFactions[0].Key, targetClient.userName, possibleFactions[0].Value[targetClient.userID.ToString()]);
                                            possibleFactions[0].Value.Remove(targetClient.userID.ToString());
                                            possibleFactions[0].Value.Add(targetClient.userID.ToString(), "owner");
                                            Data.addFactionData(possibleFactions[0].Key, targetClient.userName, targetClient.userID.ToString(), "owner");

                                            possibleFactions[0].Value.Add(senderClient.userID.ToString(), "normal");
                                            Data.addFactionData(possibleFactions[0].Key, senderClient.userName, targetClient.userID.ToString(), "normal");

                                            PlayerClient[] targetClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                            foreach (PlayerClient pc in targetClients)
                                            {
                                                Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, targetClient.userName + " now owns the faction.");
                                            }
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You are already the owner of this faction.");
                                    }
                                }
                            }
                            else
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You do not have permission to pass ownership.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        break;
                    case "list":
                        try
                        {
                            Dictionary<string, List<string>> factionNames = new Dictionary<string, List<string>>();
                            List<string> factionNames2 = new List<string>();
                            int currentPage = 1;

                            foreach (string factionName in Vars.factions.Keys)
                            {
                                if (!factionNames.ContainsKey(currentPage.ToString()))
                                {
                                    factionNames.Add(currentPage.ToString(), new List<string>() { { factionName } });
                                }
                                else
                                {
                                    if (factionNames[currentPage.ToString()].Count <= 20)
                                        factionNames[currentPage.ToString()].Add(factionName);
                                    else
                                    {
                                        currentPage++;
                                        if (!factionNames.ContainsKey(currentPage.ToString()))
                                        {
                                            factionNames.Add(currentPage.ToString(), new List<string>() { { factionName } });
                                        }
                                        else
                                        {
                                            factionNames[currentPage.ToString()].Add(factionName);
                                        }
                                    }
                                }
                            }

                            int pageNumber = 1;
                            bool continueOn = true;
                            if (args.Count() > 2)
                            {
                                if (!int.TryParse(args[2], out pageNumber) || !factionNames.ContainsKey(pageNumber.ToString()))
                                {
                                    Broadcast.broadcastTo(senderClient.netPlayer, "No such page \"" + args[2] + "\".");
                                    continueOn = false;
                                }
                            }

                            if (continueOn)
                            {
                                List<string> otherFactionNames = new List<string>();
                                if (factionNames.Count > 0)
                                {
                                    Broadcast.broadcastTo(senderClient.netPlayer, "All factions [" + pageNumber + "/" + currentPage + "]:");
                                    while (factionNames[pageNumber.ToString()].Count > 0)
                                    {
                                        factionNames2.Clear();
                                        otherFactionNames.Clear();
                                        foreach (string s in factionNames[pageNumber.ToString()])
                                        {
                                            factionNames2.Add(s);
                                            otherFactionNames.Add(s);

                                            if ((string.Join(", ", factionNames2.ToArray())).Length > 70)
                                            {
                                                factionNames2.Remove(s);
                                                otherFactionNames.Remove(s);
                                                break;
                                            }
                                        }
                                        foreach (string s in otherFactionNames)
                                        {
                                            factionNames[pageNumber.ToString()].Remove(s);
                                        }

                                        Broadcast.broadcastTo(senderClient.netPlayer, string.Join(", ", factionNames2.ToArray()));
                                    }
                                }
                                else
                                    Broadcast.broadcastTo(senderClient.netPlayer, "There are no factions!");
                            }
                        }
                        catch (Exception ex)
                        {
                            Vars.conLog.Error("FLIST: " + ex.ToString());
                        }
                        break;
                    case "ally":
                        if (possibleFactions.Count() > 0)
                        {
                            string rank = possibleFactions[0].Value[senderClient.userID.ToString()];

                            if (rank == "owner" || rank == "admin")
                            {
                                if (args.Count() > 2)
                                {
                                    List<string> messageList = new List<string>();
                                    int curIndex = 0;
                                    foreach (string s in args)
                                    {
                                        if (curIndex > 1)
                                            messageList.Add(s);
                                        curIndex++;
                                    }

                                    string factionName = string.Join(" ", messageList.ToArray());

                                    KeyValuePair<string, Dictionary<string, string>>[] factionResults = Array.FindAll(Vars.factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Key.Contains(factionName));

                                    if (factionResults.Count() == 0)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "No factions equal or contain \"" + factionName + "\".");
                                    else if (factionResults.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many faction names contain \"" + factionName + "\".");
                                    else
                                    {
                                        if (possibleFactions[0].Key != factionResults[0].Key)
                                        {
                                            if (!Vars.alliances.ContainsKey(possibleFactions[0].Key))
                                                Vars.alliances.Add(possibleFactions[0].Key, new List<string>());
                                            if (!Vars.alliances.ContainsKey(factionResults[0].Key))
                                                Vars.alliances.Add(factionResults[0].Key, new List<string>());

                                            if (!Vars.alliances[possibleFactions[0].Key].Contains(factionResults[0].Key))
                                            {
                                                PlayerClient[] targetClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                                foreach (PlayerClient pc in targetClients)
                                                {
                                                    Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, "Your faction is now allied with the faction [" + factionResults[0].Key + "].");
                                                }
                                                Vars.alliances[factionResults[0].Key].Add(possibleFactions[0].Key);
                                                Vars.alliances[possibleFactions[0].Key].Add(factionResults[0].Key);
                                                Data.addAlliesData(factionResults[0].Key, possibleFactions[0].Key);
                                            }
                                            else
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You are already allied with [" + factionResults[0].Key + "].");
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You cannot ally your own faction.");
                                    }
                                }
                                else
                                    Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You must specify a faction name in order to form an alliance.");
                            }
                            else
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You do not have permission to form alliances.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        break;
                    case "unally":
                        if (possibleFactions.Count() > 0)
                        {
                            string rank = possibleFactions[0].Value[senderClient.userID.ToString()];

                            if (rank == "owner" || rank == "admin")
                            {
                                if (args.Count() > 2)
                                {
                                    List<string> messageList = new List<string>();
                                    int curIndex = 0;
                                    foreach (string s in args)
                                    {
                                        if (curIndex > 1)
                                            messageList.Add(s);
                                        curIndex++;
                                    }

                                    string factionName = string.Join(" ", messageList.ToArray());

                                    KeyValuePair<string, Dictionary<string, string>>[] factionResults = Array.FindAll(Vars.factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Key.Contains(factionName));

                                    if (factionResults.Count() == 0)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "No factions equal or contain \"" + factionName + "\".");
                                    else if (factionResults.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many faction names contain \"" + factionName + "\".");
                                    else
                                    {
                                        if (!Vars.alliances.ContainsKey(possibleFactions[0].Key))
                                            Vars.alliances.Add(possibleFactions[0].Key, new List<string>());
                                        if (!Vars.alliances.ContainsKey(factionResults[0].Key))
                                            Vars.alliances.Add(factionResults[0].Key, new List<string>());
                                        if (!Vars.alliances[possibleFactions[0].Key].Contains(factionResults[0].Key))
                                        {
                                            PlayerClient[] targetClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                            foreach (PlayerClient pc in targetClients)
                                            {
                                                Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, "You are no longer allied with the faction [" + factionResults[0].Key + "].");
                                            }
                                            Vars.alliances[factionResults[0].Key].Remove(possibleFactions[0].Key);
                                            Vars.alliances[possibleFactions[0].Key].Remove(factionResults[0].Key);
                                            Data.remAlliesData(factionResults[0].Key, possibleFactions[0].Key);
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You are not allied with [" + factionResults[0].Key + "].");
                                    }
                                }
                                else
                                    Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You must specify a faction name in order to remove an alliance.");
                            }
                            else
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You do not have permission to remove alliances.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        break;
                    case "info":
                        if (args.Count() > 2)
                        {
                            List<string> messageList = new List<string>();
                            int curIndex = 0;
                            foreach (string s in args)
                            {
                                if (curIndex > 1)
                                    messageList.Add(s);
                                curIndex++;
                            }

                            string factionName = string.Join(" ", messageList.ToArray());
                            KeyValuePair<string, Dictionary<string, string>>[] factionResults = Array.FindAll(Vars.factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Key.Contains(factionName));

                            if (factionResults.Count() == 0)
                            {
                                PlayerClient[] possibleClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(factionName));
                                if (possibleClients.Count() == 0)
                                    Broadcast.broadcastTo(senderClient.netPlayer, "No factions or players contain \"" + factionName + "\".");
                                else if (possibleClients.Count() > 1)
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + factionName + "\".");
                                else
                                {
                                    try
                                    {
                                        KeyValuePair<string, Dictionary<string, string>>[] playerFactions = Array.FindAll(Vars.factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(possibleClients[0].userID.ToString()));
                                        int onlineMembers = 0;
                                        if (playerFactions.Count() == 1)
                                        {
                                            KeyValuePair<string, Dictionary<string, string>> playerFaction = playerFactions[0];
                                            foreach (string s in playerFaction.Value.Keys)
                                            {
                                                PlayerClient[] possibleClients2 = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID.ToString() == s);
                                                if (possibleClients2.Count() > 0)
                                                    onlineMembers++;
                                            }
                                            string ownerName = Vars.factionsByNames[playerFaction.Key][Array.Find(playerFaction.Value.ToArray(), (KeyValuePair<string, string> kv) => kv.Value == "owner").Key];

                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.Key, "=== [" + playerFaction.Key + "]'s information ===");
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.Key, "Total members: " + playerFaction.Value.Count);
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.Key, "Online members: " + onlineMembers);
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.Key, "Offline members: " + (playerFaction.Value.Count - onlineMembers));
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.Key, "Owner: " + ownerName);
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.Key, "Members:");
                                            List<string> names = new List<string>();
                                            List<string> names2 = new List<string>();
                                            foreach (string name in Vars.factionsByNames[playerFaction.Key].Values)
                                            {
                                                if (name != ownerName)
                                                    names.Add(name);
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
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.Key, string.Join(", ", names2.ToArray()));
                                            }
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.Key, "Allies:");
                                            List<string> allies = new List<string>();
                                            List<string> allies2 = new List<string>();
                                            if (Vars.alliances.ContainsKey(playerFaction.Key))
                                            {
                                                foreach (string name in Vars.alliances[playerFaction.Key])
                                                {
                                                    allies.Add(name);
                                                }

                                                List<string> otherAllies = new List<string>();
                                                while (allies.Count > 0)
                                                {
                                                    allies2.Clear();
                                                    otherAllies.Clear();
                                                    foreach (string s in allies)
                                                    {
                                                        allies2.Add(s);
                                                        otherAllies.Add(s);

                                                        if ((string.Join(", ", allies2.ToArray())).Length > 70)
                                                        {
                                                            allies2.Remove(s);
                                                            otherAllies.Remove(s);
                                                            break;
                                                        }
                                                    }
                                                    foreach (string s in otherAllies)
                                                    {
                                                        allies.Remove(s);
                                                    }
                                                    Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.Key, string.Join(", ", allies2.ToArray()));
                                                }
                                            }
                                        }
                                        else if (playerFactions.Count() == 0)
                                        {
                                            Broadcast.broadcastTo(senderClient.netPlayer, possibleClients[0].userName + " is not in a faction.");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Vars.conLog.Error("FINFO: " + ex.ToString());
                                    }
                                }
                            }
                            else if (factionResults.Count() > 1)
                                Broadcast.broadcastTo(senderClient.netPlayer, "Too many faction names contain \"" + factionName + "\".");
                            else
                            {
                                int onlineMembers = 0;
                                foreach (string s in factionResults[0].Value.Keys)
                                {
                                    PlayerClient[] possibleClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID.ToString() == s);
                                    if (possibleClients.Count() > 0)
                                        onlineMembers++;
                                }
                                string ownerName = Vars.factionsByNames[factionResults[0].Key][Array.Find(factionResults[0].Value.ToArray(), (KeyValuePair<string, string> kv) => kv.Value == "owner").Key];

                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].Key, "=== [" + factionResults[0].Key + "]'s information ===");
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].Key, "Total members: " + factionResults[0].Value.Count);
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].Key, "Online members: " + onlineMembers);
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].Key, "Offline members: " + (factionResults[0].Value.Count - onlineMembers));
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].Key, "Owner: " + ownerName);
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].Key, "Members:");
                                List<string> names = new List<string>();
                                List<string> names2 = new List<string>();
                                foreach (string name in Vars.factionsByNames[factionResults[0].Key].Values)
                                {
                                    if (name != ownerName)
                                        names.Add(name);
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
                                    Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].Key, string.Join(", ", names2.ToArray()));
                                }
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].Key, "Allies:");
                                List<string> allies = new List<string>();
                                List<string> allies2 = new List<string>();
                                if (Vars.alliances.ContainsKey(factionResults[0].Key))
                                {
                                    foreach (string name in Vars.alliances[factionResults[0].Key])
                                    {
                                        allies.Add(name);
                                    }

                                    List<string> otherAllies = new List<string>();
                                    while (allies.Count > 0)
                                    {
                                        allies2.Clear();
                                        otherAllies.Clear();
                                        foreach (string s in allies)
                                        {
                                            allies2.Add(s);
                                            otherAllies.Add(s);

                                            if ((string.Join(", ", allies2.ToArray())).Length > 70)
                                            {
                                                allies2.Remove(s);
                                                otherAllies.Remove(s);
                                                break;
                                            }
                                        }
                                        foreach (string s in otherAllies)
                                        {
                                            allies.Remove(s);
                                        }
                                        Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].Key, string.Join(", ", allies2.ToArray()));
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (possibleFactions.Count() > 0)
                            {
                                int onlineMembers = 0;
                                foreach (string s in possibleFactions[0].Value.Keys)
                                {
                                    PlayerClient[] possibleClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID.ToString() == s);
                                    if (possibleClients.Count() > 0)
                                        onlineMembers++;
                                }
                                string ownerName = Vars.factionsByNames[possibleFactions[0].Key][Array.Find(possibleFactions[0].Value.ToArray(), (KeyValuePair<string, string> kv) => kv.Value == "owner").Key];

                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "=== Your faction's information ===");
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "Total members: " + possibleFactions[0].Value.Count);
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "Online members: " + onlineMembers);
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "Offline members: " + (possibleFactions[0].Value.Count - onlineMembers));
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "Owner: " + ownerName);
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "Members:");
                                List<string> names = new List<string>();
                                List<string> names2 = new List<string>();
                                foreach (string name in Vars.factionsByNames[possibleFactions[0].Key].Values)
                                {
                                    if (name != ownerName)
                                        names.Add(name);
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
                                    Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, string.Join(", ", names2.ToArray()));
                                }
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "Allies:");
                                List<string> allies = new List<string>();
                                List<string> allies2 = new List<string>();
                                if (Vars.alliances.ContainsKey(possibleFactions[0].Key))
                                {
                                    foreach (string name in Vars.alliances[possibleFactions[0].Key])
                                    {
                                        allies.Add(name);
                                    }

                                    List<string> otherAllies = new List<string>();
                                    while (allies.Count > 0)
                                    {
                                        allies2.Clear();
                                        otherAllies.Clear();
                                        foreach (string s in allies)
                                        {
                                            allies2.Add(s);
                                            otherAllies.Add(s);

                                            if ((string.Join(", ", allies2.ToArray())).Length > 70)
                                            {
                                                allies2.Remove(s);
                                                otherAllies.Remove(s);
                                                break;
                                            }
                                        }
                                        foreach (string s in otherAllies)
                                        {
                                            allies.Remove(s);
                                        }
                                        Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, string.Join(", ", allies2.ToArray()));
                                    }
                                }
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        }
                        break;
                    case "players":
                        if (possibleFactions.Count() > 0)
                        {
                            List<string> UIDs = new List<string>();
                            List<string> onlineNames = new List<string>();
                            List<string> offlineNames = new List<string>();
                            foreach (string s in possibleFactions[0].Value.Keys)
                            {
                                UIDs.Add(s);
                            }

                            List<string> otherNames = new List<string>();
                            bool saidOnline = false;
                            bool saidOffline = false;
                            while (UIDs.Count > 0)
                            {
                                onlineNames.Clear();
                                offlineNames.Clear();
                                otherNames.Clear();
                                bool hasOnline = false;
                                foreach (string s in UIDs)
                                {
                                    PlayerClient[] possibleClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID.ToString() == s);

                                    if (possibleClients.Count() > 0)
                                    {
                                        if (possibleClients[0].userName.Length > 0)
                                            hasOnline = true;
                                    }
                                }
                                foreach (string s in UIDs)
                                {
                                    if (hasOnline)
                                    {
                                        PlayerClient[] possibleClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID.ToString() == s);
                                        string playerName = Vars.factionsByNames[possibleFactions[0].Key][s];

                                        if (possibleClients.Count() > 0)
                                        {
                                            if (!saidOnline)
                                            {
                                                saidOnline = true;
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "All online faction members:");
                                            }

                                            onlineNames.Add(playerName);
                                            otherNames.Add(s);

                                            if ((string.Join(", ", onlineNames.ToArray())).Length > 70)
                                            {
                                                onlineNames.Remove(playerName);
                                                otherNames.Remove(s);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string playerName = Vars.factionsByNames[possibleFactions[0].Key][s];

                                        if (!saidOffline)
                                        {
                                            saidOffline = true;
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "All offline faction members:");
                                        }

                                        offlineNames.Add(playerName);
                                        otherNames.Add(s);

                                        if ((string.Join(", ", offlineNames.ToArray())).Length > 70)
                                        {
                                            offlineNames.Remove(playerName);
                                            otherNames.Remove(s);
                                            break;
                                        }
                                    }
                                }
                                foreach (string s in otherNames)
                                {
                                    UIDs.Remove(s);
                                }
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, string.Join(", ", (hasOnline ? onlineNames.ToArray() : offlineNames.ToArray())));
                            }
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        break;
                    case "online":
                        if (possibleFactions.Count() > 0)
                        {
                            int onlineMembers = 0;
                            foreach (string s in possibleFactions[0].Value.Keys)
                            {
                                PlayerClient[] possibleClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID.ToString() == s);
                                if (possibleClients.Count() > 0)
                                {
                                    if (possibleClients[0].userName.Length > 0)
                                        onlineMembers++;
                                }
                            }

                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, onlineMembers + "/" + possibleFactions[0].Value.Count + " faction members currently connected. Faction is at " + possibleFactions[0].Value.Count + "/" + Vars.maxMembers + " member capacity.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        break;
                    case "build":
                        if (Vars.enabledCommands[rankToUse].Contains("/f build"))
                        {
                            if (args.Count() > 2)
                            {
                                string mode = args[2];
                                string UID = senderClient.userID.ToString();

                                switch (mode)
                                {
                                    case "on":
                                        if (!Vars.buildList.Contains(UID))
                                        {
                                            Broadcast.broadcastTo(senderClient.netPlayer, "You can now build in safe zones and war zones.");
                                            Vars.buildList.Add(UID);
                                        }
                                        else
                                            Broadcast.broadcastTo(senderClient.netPlayer, "You are already in build mode.");
                                        break;
                                    case "off":
                                        if (Vars.buildList.Contains(UID))
                                        {
                                            Broadcast.broadcastTo(senderClient.netPlayer, "You can no longer build in safe zones and war zones.");
                                            Vars.buildList.Remove(UID);
                                        }
                                        else
                                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in build mode.");
                                        break;
                                }
                            }
                        }
                        break;
                    case "buildable":
                        if (Vars.enabledCommands[rankToUse].Contains("/f buildable"))
                        {
                            bool inSafeZone = false;
                            bool inWarZone = false;

                            if (Vars.inSafeZone.ContainsKey(senderClient))
                                inSafeZone = true;

                            if (Vars.inWarZone.ContainsKey(senderClient))
                                inWarZone = true;

                            if (inSafeZone || inWarZone)
                            {
                                if (args.Count() > 2)
                                {
                                    string mode = args[2];
                                    string UID = senderClient.userID.ToString();

                                    switch (mode)
                                    {
                                        case "on":
                                            if (inSafeZone)
                                            {
                                                if (!Vars.inSafeZone[senderClient].buildable)
                                                {
                                                    Vars.inSafeZone[senderClient].buildable = true;
                                                    Broadcast.broadcastTo(senderClient.netPlayer, "This safezone is now buildable.");
                                                    Data.updateZoneData(Vars.inSafeZone[senderClient]);
                                                }
                                                else
                                                    Broadcast.broadcastTo(senderClient.netPlayer, "This safezone is already set to buildable.");
                                            }
                                            if (inWarZone)
                                            {
                                                if (!Vars.inWarZone[senderClient].buildable)
                                                {
                                                    Vars.inWarZone[senderClient].buildable = true;
                                                    Broadcast.broadcastTo(senderClient.netPlayer, "This warzone is now buildable.");
                                                    Data.updateZoneData(Vars.inSafeZone[senderClient]);
                                                }
                                                else
                                                    Broadcast.broadcastTo(senderClient.netPlayer, "This warzone is already set to buildable.");
                                            }
                                            break;
                                        case "off":
                                            if (inSafeZone)
                                            {
                                                if (Vars.inSafeZone[senderClient].buildable)
                                                {
                                                    Vars.inSafeZone[senderClient].buildable = false;
                                                    Broadcast.broadcastTo(senderClient.netPlayer, "This safezone is no longer buildable.");
                                                    Data.updateZoneData(Vars.inSafeZone[senderClient]);
                                                }
                                                else
                                                    Broadcast.broadcastTo(senderClient.netPlayer, "This safezone is not set to buildable.");
                                            }
                                            if (inWarZone)
                                            {
                                                if (Vars.inWarZone[senderClient].buildable)
                                                {
                                                    Vars.inWarZone[senderClient].buildable = false;
                                                    Broadcast.broadcastTo(senderClient.netPlayer, "This warzone is no longer buildable.");
                                                    Data.updateZoneData(Vars.inSafeZone[senderClient]);
                                                }
                                                else
                                                    Broadcast.broadcastTo(senderClient.netPlayer, "This warzone is not set to buildable.");
                                            }
                                            break;
                                    }
                                }
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "You must be in a safezone or warzone to change it to buildable.");
                        }
                        break;
                    case "help":
                        Broadcast.broadcastTo(senderClient.netPlayer, "=================== Factions ===================");
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f create *name*: Creates a faction.");
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f disband: Disbands current faction if you're the owner.");
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f kick *name*: Kicks the player from your faction. Partials accepted.");
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f invite *name*: Invites the player to your faction. Partials accepted.");
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f join: Joins the faction last invited to.");
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f join *name*: Joins the faction by name if invited.");
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f leave: Leaves current faction.");
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f admin *name*: Gives the specified user access to invite players.");
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f deadmin *name*: Revokes access to invite players from a player.");
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f ownership *name*: Passes faction ownership from owner to admin.");
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f list: List all factions.");
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f info: Displays current faction information.");
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f info *name*: Displays that faction's information.");
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f ally *name*: Forms an alliance with another faction.");
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f unally *name*: Removes an alliance with another faction.");
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f players: Lists players in current faction.");
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f online: Displays count of currently online faction members.");
                        if (Vars.enabledCommands[rankToUse].Contains("/f safezone"))
                            Broadcast.broadcastTo(senderClient.netPlayer, "/f safezone {1/2/3/4/set/clear/clearall}: Manages safezones.");
                        if (Vars.enabledCommands[rankToUse].Contains("/f warzone"))
                            Broadcast.broadcastTo(senderClient.netPlayer, "/f warzone {1/2/3/4/set/clear/clearall}: Manages warzones.");
                        if (Vars.enabledCommands[rankToUse].Contains("/f build"))
                            Broadcast.broadcastTo(senderClient.netPlayer, "/f build {on/off}: Allows self-building within zones.");
                        if (Vars.enabledCommands[rankToUse].Contains("/f buildable"))
                            Broadcast.broadcastTo(senderClient.netPlayer, "/f buildable {on/off}: Allows building within the current zone for all players.");
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "Unknown faction action \"" + arg + "\".");
                        break;
                }
            }
        }

        public static void manageZones(PlayerClient senderClient, string[] args, bool safeZone)
        {
            if (args.Count() > 2)
            {
                string mode = args[2];
                Character senderChar;
                Character.FindByUser(senderClient.userID, out senderChar);
                Vector3 posV3 = senderChar.transform.position;
                Vector2 currentPos = new Vector2(posV3.x, posV3.z);

                switch (mode)
                {
                    case "1":
                        if (!Vars.firstPoints.ContainsKey(senderClient))
                        {
                            Vars.firstPoints.Add(senderClient, currentPos);
                            Broadcast.broadcastTo(senderClient.netPlayer, "First zone point set to " + currentPos.ToString());
                        }
                        else
                        {
                            Vars.firstPoints[senderClient] = currentPos;
                            Broadcast.broadcastTo(senderClient.netPlayer, "First zone point reset to " + currentPos.ToString());
                        }
                        break;
                    case "2":
                        if (!Vars.secondPoints.ContainsKey(senderClient))
                        {
                            Vars.secondPoints.Add(senderClient, currentPos);
                            Broadcast.broadcastTo(senderClient.netPlayer, "Second zone point set to " + currentPos.ToString());
                        }
                        else
                        {
                            Vars.secondPoints[senderClient] = currentPos;
                            Broadcast.broadcastTo(senderClient.netPlayer, "Second zone point reset to " + currentPos.ToString());
                        }
                        break;
                    case "3":
                        if (!Vars.thirdPoints.ContainsKey(senderClient))
                        {
                            Vars.thirdPoints.Add(senderClient, currentPos);
                            Broadcast.broadcastTo(senderClient.netPlayer, "Third zone point set to " + currentPos.ToString());
                        }
                        else
                        {
                            Vars.thirdPoints[senderClient] = currentPos;
                            Broadcast.broadcastTo(senderClient.netPlayer, "Third zone point reset to " + currentPos.ToString());
                        }
                        break;
                    case "4":
                        if (!Vars.fourthPoints.ContainsKey(senderClient))
                        {
                            Vars.fourthPoints.Add(senderClient, currentPos);
                            Broadcast.broadcastTo(senderClient.netPlayer, "Fourth zone point set to " + currentPos.ToString());
                        }
                        else
                        {
                            Vars.fourthPoints[senderClient] = currentPos;
                            Broadcast.broadcastTo(senderClient.netPlayer, "Fourth zone point reset to " + currentPos.ToString());
                        }
                        break;
                    case "set":
                        if (!Vars.secondPoints.ContainsKey(senderClient) && !Vars.firstPoints.ContainsKey(senderClient))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You must set points before creating a zone.");
                            return;
                        }
                        if (!Vars.firstPoints.ContainsKey(senderClient))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You must set the first point before creating a zone.");
                            return;
                        }
                        if (!Vars.secondPoints.ContainsKey(senderClient))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You must set the second point before creating a zone.");
                            return;
                        }
                        if (!Vars.thirdPoints.ContainsKey(senderClient))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You must set the third point before creating a zone.");
                            return;
                        }
                        if (!Vars.fourthPoints.ContainsKey(senderClient))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You must set the fourth point before creating a zone.");
                            return;
                        }

                        Vector2 firstPoint = Vars.firstPoints[senderClient];
                        Vector2 secondPoint = Vars.secondPoints[senderClient];
                        Vector2 thirdPoint = Vars.thirdPoints[senderClient];
                        Vector2 fourthPoint = Vars.fourthPoints[senderClient];

                        if (safeZone)
                        {
                            int instances = Vars.safeZones.Count;
                            Zone zone = new Zone("safezone_" + instances, firstPoint, secondPoint, thirdPoint, fourthPoint);
                            Vars.safeZones.Add(zone);
                            Broadcast.broadcastTo(senderClient.netPlayer, "Safezone created! Size: " + Math.Round(Vector2.Distance(firstPoint, secondPoint), 2) + " square meters.");
                            Data.addZoneData(zone);
                        }
                        else
                        {
                            int instances = Vars.warZones.Count;
                            Zone zone = new Zone("warzone_" + instances, firstPoint, secondPoint, thirdPoint, fourthPoint);
                            Vars.warZones.Add(zone);
                            Broadcast.broadcastTo(senderClient.netPlayer, "Warzone created! Size: " + Math.Round(Vector2.Distance(firstPoint, secondPoint), 2) + " square meters.");
                            Data.addZoneData(zone);
                        }
                        break;
                    case "clear":
                        if (safeZone)
                        {
                            if (Vars.inSafeZone.ContainsKey(senderClient))
                            {
                                KeyValuePair<PlayerClient, Zone>[] clients = Array.FindAll(Vars.inSafeZone.ToArray(), (KeyValuePair<PlayerClient, Zone> kv) => kv.Value.zoneName == Vars.inSafeZone[senderClient].zoneName);
                                if (Vars.safeZones.Contains(Vars.inSafeZone[senderClient]))
                                {
                                    Zone zone = Vars.inSafeZone[senderClient];
                                    string zoneName = zone.zoneName;
                                    if (Vars.zoneStructures.ContainsKey(zone))
                                    {
                                        foreach (var component in Vars.zoneStructures[zone])
                                        {
                                            Vars.structuresInZones.Remove(component);
                                        }
                                        Vars.zoneStructures.Remove(zone);
                                    }
                                    if (Vars.zoneObjects.ContainsKey(zone))
                                    {
                                        foreach (var obj in Vars.zoneObjects[zone])
                                        {
                                            Vars.objectsInZones.Remove(obj);
                                        }
                                        Vars.zoneObjects.Remove(zone);
                                    }
                                    if (Vars.zoneEnvDecays.ContainsKey(zone))
                                    {
                                        foreach (var obj in Vars.zoneEnvDecays[zone])
                                        {
                                            Vars.envDecaysInZones.Remove(obj);
                                        }
                                        Vars.zoneEnvDecays.Remove(zone);
                                    }
                                    Vars.safeZones.Remove(zoneName);
                                }
                                Data.remZoneData(Vars.inSafeZone[senderClient].zoneName);
                                foreach (KeyValuePair<PlayerClient, Zone> kv in clients)
                                {
                                    Broadcast.noticeTo(kv.Key.netPlayer, "!", "Safe zone deleted! You are no longer in a safe zone.", 3);
                                    Vars.inSafeZone.Remove(kv.Key);
                                }
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "You must be in a safe zone to delete it.");
                        }
                        else
                        {
                            if (Vars.inWarZone.ContainsKey(senderClient))
                            {
                                KeyValuePair<PlayerClient, Zone>[] clients = Array.FindAll(Vars.inWarZone.ToArray(), (KeyValuePair<PlayerClient, Zone> kv) => kv.Value.zoneName == Vars.inWarZone[senderClient].zoneName);
                                if (Vars.warZones.Contains(Vars.inWarZone[senderClient]))
                                {
                                    Zone zone = Vars.inWarZone[senderClient];
                                    string zoneName = zone.zoneName;
                                    if (Vars.zoneStructures.ContainsKey(zone))
                                    {
                                        foreach (var component in Vars.zoneStructures[zone])
                                        {
                                            Vars.structuresInZones.Remove(component);
                                        }
                                        Vars.zoneStructures.Remove(zone);
                                    }
                                    if (Vars.zoneObjects.ContainsKey(zone))
                                    {
                                        foreach (var obj in Vars.zoneObjects[zone])
                                        {
                                            Vars.objectsInZones.Remove(obj);
                                        }
                                        Vars.zoneObjects.Remove(zone);
                                    }
                                    if (Vars.zoneEnvDecays.ContainsKey(zone))
                                    {
                                        foreach (var obj in Vars.zoneEnvDecays[zone])
                                        {
                                            Vars.envDecaysInZones.Remove(obj);
                                        }
                                        Vars.zoneEnvDecays.Remove(zone);
                                    }
                                    Vars.warZones.Remove(Vars.inWarZone[senderClient]);
                                }
                                Data.remZoneData(Vars.inWarZone[senderClient].zoneName);
                                foreach (KeyValuePair<PlayerClient, Zone> kv in clients)
                                {
                                    Broadcast.noticeTo(kv.Key.netPlayer, "!", "War zone deleted! You are no longer in a war zone.", 3);
                                    Vars.inWarZone.Remove(kv.Key);
                                }
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "You must be in a war zone to delete it.");
                        }
                        break;
                    case "clearall":
                        if (safeZone)
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "All safe zones deleted.");
                            foreach (var zone in Vars.safeZones)
                            {
                                if (Vars.zoneStructures.ContainsKey(zone))
                                {
                                    foreach (var component in Vars.zoneStructures[zone])
                                    {
                                        Vars.structuresInZones.Remove(component);
                                    }
                                    Vars.zoneStructures.Remove(zone);
                                }
                                if (Vars.zoneObjects.ContainsKey(zone))
                                {
                                    foreach (var obj in Vars.zoneObjects[zone])
                                    {
                                        Vars.objectsInZones.Remove(obj);
                                    }
                                    Vars.zoneObjects.Remove(zone);
                                }
                                if (Vars.zoneEnvDecays.ContainsKey(zone))
                                {
                                    foreach (var obj in Vars.zoneEnvDecays[zone])
                                    {
                                        Vars.envDecaysInZones.Remove(obj);
                                    }
                                    Vars.zoneEnvDecays.Remove(zone);
                                }
                            }
                            Vars.safeZones.Clear();
                            foreach (PlayerClient pc in Vars.inSafeZone.Keys)
                            {
                                Broadcast.noticeTo(pc.netPlayer, "!", "Safe zone deleted! You are no longer in a safe zone.", 3);
                            }
                            Vars.inSafeZone.Clear();
                            Data.remZoneData("clearallS");
                        }
                        else
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "All war zones deleted.");
                            foreach (var zone in Vars.warZones)
                            {
                                if (Vars.zoneStructures.ContainsKey(zone))
                                {
                                    foreach (var component in Vars.zoneStructures[zone])
                                    {
                                        Vars.structuresInZones.Remove(component);
                                    }
                                    Vars.zoneStructures.Remove(zone);
                                }
                                if (Vars.zoneObjects.ContainsKey(zone))
                                {
                                    foreach (var obj in Vars.zoneObjects[zone])
                                    {
                                        Vars.objectsInZones.Remove(obj);
                                    }
                                    Vars.zoneObjects.Remove(zone);
                                }
                                if (Vars.zoneEnvDecays.ContainsKey(zone))
                                {
                                    foreach (var obj in Vars.zoneEnvDecays[zone])
                                    {
                                        Vars.envDecaysInZones.Remove(obj);
                                    }
                                    Vars.zoneEnvDecays.Remove(zone);
                                }
                            }
                            Vars.warZones.Clear();
                            foreach (PlayerClient pc in Vars.inWarZone.Keys)
                            {
                                Broadcast.noticeTo(pc.netPlayer, "!", "War zone deleted! You are no longer in a war zone.", 3);
                            }
                            Vars.inWarZone.Clear();
                            Data.remZoneData("clearallW");
                        }
                        break;
                }
            }
        }
    }
}
