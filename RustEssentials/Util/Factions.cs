using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RustEssentials.Util
{
    public class Factions
    {
        public static void handleFactions(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string arg = args[1];
                Faction faction = Vars.factions.GetByMember(senderClient.userID);

                string rankToUse = Vars.findRank(senderClient.userID);
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

                            if (Vars.factions.GetByName(factionName) == null)
                            {
                                if (faction == null)
                                {
                                    if (factionName.Length < 16)
                                    {
                                        if (!factionName.Contains("=") && !factionName.Contains(";") && !factionName.Contains(":"))
                                        {
                                            Vars.factions.Add(new Faction(factionName, senderClient.userName, senderClient.userID));
                                            Broadcast.broadcastTo(senderClient.netPlayer, "Faction [" + factionName + "] created.");
                                            Data.saveFactions();
                                            //Data.addFactionData(factionName, senderClient.userName, senderClient.userID.ToString(), "owner");
                                        }
                                        else
                                            Broadcast.broadcastTo(senderClient.netPlayer, "Faction names cannot contain =, :, or ;!");
                                    }
                                    else
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Faction names must be less than 16 characters!");
                                }
                                else
                                    Broadcast.broadcastTo(senderClient.netPlayer, "You are already in the faction [" + faction.name + "].");
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "Faction [" + factionName + "] already exists.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "Improper syntax! Syntax: /f create *name*");
                        break;
                    case "disband":
                        if (faction != null)
                        {
                            FactionMember member = faction.GetMember(senderClient.userID);
                            string rank = member.rank;

                            if (Vars.completeDoorAccess.Contains(senderClient.userID))
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "Faction disbanded.");

                            if (rank == "owner" || Vars.completeDoorAccess.Contains(senderClient.userID))
                            {
                                foreach (var m in faction.members)
                                {
                                    PlayerClient pc;
                                    if (Vars.getPlayerClient(m.userID, out pc))
                                        Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + faction.name, "Your faction was disbanded.");
                                }
                                Vars.factions.Remove(faction.name);
                                Data.saveFactions();
                                //Data.remFactionData(faction.name, "disband", "");
                            }
                            else
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You do not have permission to disband your current faction.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        break;
                    case "kick":
                        if (faction != null)
                        {
                            FactionMember member = faction.GetMember(senderClient.userID);
                            string rank = member.rank;

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
                                        List<ulong> possibleUIDs = new List<ulong>();
                                        foreach (var m in faction.members)
                                        {
                                            if (m.name == targetName && m.userID != senderClient.userID)
                                                possibleUIDs.Add(m.userID);
                                        }

                                        if (possibleUIDs.Count() == 0)
                                        {
                                            Broadcast.broadcastTo(senderClient.netPlayer, "No player name or UID equals \"" + targetName + "\".");
                                        }
                                        else if (possibleUIDs.Count() > 1)
                                        {
                                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names or UIDs equal \"" + targetName + "\".");
                                        }
                                        else
                                        {
                                            FactionMember target = faction.GetMember(possibleUIDs[0]);
                                            if (target != null)
                                            {
                                                foreach (var m in faction.members)
                                                {
                                                    PlayerClient pc;
                                                    if (Vars.getPlayerClient(m.userID, out pc))
                                                    {
                                                        if (m.userID == target.userID)
                                                            Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + faction.name, "You were kicked from the faction.");
                                                        else
                                                            Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + faction.name, m.name + " was kicked from the faction.");
                                                    }
                                                }
                                                faction.RemoveMember(target.userID);
                                                Data.saveFactions();
                                                //Data.remFactionData(faction.name, target.name, target.rank);
                                            }
                                            else
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, possibleUIDs[0] + " is not in your faction.");
                                        }
                                    }
                                    else if (possibleTargets.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + targetName + "\".");
                                    else
                                    {
                                        PlayerClient targetClient = possibleTargets[0];

                                        if (targetClient != senderClient)
                                        {
                                            FactionMember target = faction.GetMember(targetClient.userID);
                                            if (target != null)
                                            {
                                                foreach (var m in faction.members)
                                                {
                                                    PlayerClient pc;
                                                    if (Vars.getPlayerClient(m.userID, out pc))
                                                    {
                                                        if (m.userID == target.userID)
                                                            Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + faction.name, "You were kicked from the faction.");
                                                        else
                                                            Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + faction.name, m.name + " was kicked from the faction.");
                                                    }
                                                }
                                                faction.RemoveMember(target.userID);
                                                Data.saveFactions();
                                                //Data.remFactionData(faction.name, target.name, target.rank);
                                            }
                                            else
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, targetClient.userName + " is not in your faction.");
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You cannot kick yourself from the faction.");
                                    }
                                }
                                else
                                {
                                    PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));

                                    if (possibleTargets.Count() == 0)
                                    {
                                        List<ulong> possibleUIDs = new List<ulong>();
                                        foreach (var m in faction.members)
                                        {
                                            if (m.name == targetName && m.userID != senderClient.userID)
                                                possibleUIDs.Add(m.userID);
                                        }

                                        if (possibleUIDs.Count() == 0)
                                        {
                                            Broadcast.broadcastTo(senderClient.netPlayer, "No player name or UID contain \"" + targetName + "\".");
                                        }
                                        else if (possibleUIDs.Count() > 1)
                                        {
                                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names or UID contain \"" + targetName + "\".");
                                        }
                                        else
                                        {
                                            FactionMember target = faction.GetMember(possibleUIDs[0]);
                                            if (target != null)
                                            {
                                                foreach (var m in faction.members)
                                                {
                                                    PlayerClient pc;
                                                    if (Vars.getPlayerClient(m.userID, out pc))
                                                    {
                                                        if (m.userID == target.userID)
                                                            Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + faction.name, "You were kicked from the faction.");
                                                        else
                                                            Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + faction.name, m.name + " was kicked from the faction.");
                                                    }
                                                }
                                                faction.RemoveMember(target.userID);
                                                Data.saveFactions();
                                                //Data.remFactionData(faction.name, target.name, target.rank);
                                            }
                                            else
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, possibleUIDs[0] + " is not in your faction.");
                                        }
                                    }
                                    else if (possibleTargets.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + targetName + "\".");
                                    else
                                    {
                                        PlayerClient targetClient = possibleTargets[0];

                                        if (targetClient != senderClient)
                                        {
                                            FactionMember target = faction.GetMember(targetClient.userID);
                                            if (target != null)
                                            {
                                                foreach (var m in faction.members)
                                                {
                                                    PlayerClient pc;
                                                    if (Vars.getPlayerClient(m.userID, out pc))
                                                    {
                                                        if (m.userID == target.userID)
                                                            Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + faction.name, "You were kicked from the faction.");
                                                        else
                                                            Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + faction.name, m.name + " was kicked from the faction.");
                                                    }
                                                }
                                                faction.RemoveMember(target.userID);
                                                Data.saveFactions();
                                                //Data.remFactionData(faction.name, target.name, target.rank);
                                            }
                                            else
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, targetClient.userName + " is not in your faction.");
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You cannot kick yourself from the faction.");
                                    }
                                }
                            }
                            else
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You do not have permission to kick members of your faction.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        break;
                    case "invite":
                        if (faction != null)
                        {
                            FactionMember member = faction.GetMember(senderClient.userID);
                            string rank = member.rank;

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

                                        if (faction.members.Count < Vars.memberLimit || Vars.memberLimit == 0)
                                        {
                                            if (faction.GetMember(targetClient.userID) == null)
                                            {
                                                Faction targetFaction = Vars.factions.GetByMember(targetClient.userID);
                                                if (targetFaction == null)
                                                {
                                                    if (!Vars.factionInvites.ContainsKey(targetClient.userID))
                                                    {
                                                        Vars.factionInvites.Add(targetClient.userID, new List<string>() { { faction.name } });

                                                        Broadcast.broadcastTo(senderClient.netPlayer, "You invited \"" + targetClient.userName + "\" to the faction.");
                                                        Broadcast.broadcastTo(targetClient.netPlayer, "You were invited to the faction [" + faction.name + "]. Type /f join to join.");
                                                        if (!Vars.latestFactionRequests.ContainsKey(targetClient.userID))
                                                            Vars.latestFactionRequests.Add(targetClient.userID, senderClient.userID);
                                                        else
                                                            Vars.latestFactionRequests[targetClient.userID] = senderClient.userID;
                                                    }
                                                    else
                                                    {
                                                        if (!Vars.factionInvites[targetClient.userID].Contains(faction.name))
                                                        {
                                                            Vars.factionInvites[targetClient.userID].Add(faction.name);

                                                            Broadcast.broadcastTo(senderClient.netPlayer, "You invited \"" + targetClient.userName + "\" to the faction.");
                                                            Broadcast.broadcastTo(targetClient.netPlayer, "You were invited to the faction [" + faction.name + "]. Type /f join to join.");
                                                            if (!Vars.latestFactionRequests.ContainsKey(targetClient.userID))
                                                                Vars.latestFactionRequests.Add(targetClient.userID, senderClient.userID);
                                                            else
                                                                Vars.latestFactionRequests[targetClient.userID] = senderClient.userID;
                                                        }
                                                        else
                                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, targetClient.userName + " has already been invited.");
                                                    }
                                                }
                                                else
                                                    Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, targetClient.userName + " is already in the faction [" + targetFaction.name + "].");
                                            }
                                            else
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, targetClient.userName + " is already in the faction.");
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You have reached your member capacity.");
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

                                        if (faction.members.Count < Vars.memberLimit || Vars.memberLimit == 0)
                                        {
                                            if (faction.GetMember(targetClient.userID) == null)
                                            {
                                                Faction targetFaction = Vars.factions.GetByMember(targetClient.userID);
                                                if (targetFaction == null)
                                                {
                                                    if (!Vars.factionInvites.ContainsKey(targetClient.userID))
                                                    {
                                                        Vars.factionInvites.Add(targetClient.userID, new List<string>() { { faction.name } });

                                                        Broadcast.broadcastTo(senderClient.netPlayer, "You invited " + targetClient.userName + " to the faction.");
                                                        Broadcast.broadcastTo(targetClient.netPlayer, "You were invited to the faction [" + faction.name + "]. Type /f join to join.");
                                                        if (!Vars.latestFactionRequests.ContainsKey(targetClient.userID))
                                                            Vars.latestFactionRequests.Add(targetClient.userID, senderClient.userID);
                                                        else
                                                            Vars.latestFactionRequests[targetClient.userID] = senderClient.userID;
                                                    }
                                                    else
                                                    {
                                                        if (!Vars.factionInvites[targetClient.userID].Contains(faction.name))
                                                        {
                                                            Vars.factionInvites[targetClient.userID].Add(faction.name);

                                                            Broadcast.broadcastTo(senderClient.netPlayer, "You invited " + targetClient.userName + " to the faction.");
                                                            Broadcast.broadcastTo(targetClient.netPlayer, "You were invited to the faction [" + faction.name + "]. Type /f join to join.");
                                                            if (!Vars.latestFactionRequests.ContainsKey(targetClient.userID))
                                                                Vars.latestFactionRequests.Add(targetClient.userID, senderClient.userID);
                                                            else
                                                                Vars.latestFactionRequests[targetClient.userID] = senderClient.userID;
                                                        }
                                                        else
                                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, targetClient.userName + " has already been invited.");
                                                    }
                                                }
                                                else
                                                    Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, targetClient.userName + " is already in the faction [" + targetFaction.name + "].");
                                            }
                                            else
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, targetClient.userName + " is already in the faction.");
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You have reached your member capacity.");
                                    }
                                }
                            }
                            else
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You do not have permission to invite members to your faction.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        break;
                    case "join":
                        if (faction == null)
                        {
                            if (Vars.factionInvites.ContainsKey(senderClient.userID))
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
                                    foreach (string s in Vars.factionInvites[senderClient.userID])
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
                                        Vars.factionInvites[senderClient.userID].Remove(inviterFactions[0]);
                                        Faction inviterFaction = Vars.factions.GetByName(inviterFactions[0]);
                                        inviterFaction.AddMember(senderClient.userName, "normal", senderClient.userID);
                                        foreach (var m in inviterFaction.members)
                                        {
                                            PlayerClient pc;
                                            if (Vars.getPlayerClient(m.userID, out pc))
                                            {
                                                Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + inviterFaction.name, senderClient.userName + " has joined the faction.");
                                            }
                                        }
                                        Data.saveFactions();
                                        //Data.addFactionData(inviterFactions[0], senderClient.userName, senderClient.userID.ToString(), "normal");
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        if (Vars.latestFactionRequests.ContainsKey(senderClient.userID))
                                        {
                                            Faction inviterFaction = Vars.factions.GetByMember(Vars.latestFactionRequests[senderClient.userID]);
                                            Vars.factionInvites[senderClient.userID].Remove(inviterFaction.name);
                                            inviterFaction.AddMember(senderClient.userName, "normal", senderClient.userID);
                                            Data.saveFactions();
                                            //Data.addFactionData(inviterFaction.name, senderClient.userName, senderClient.userID.ToString(), "normal");
                                            try
                                            {
                                                foreach (var m in inviterFaction.members)
                                                {
                                                    PlayerClient pc;
                                                    if (Vars.getPlayerClient(m.userID, out pc))
                                                    {
                                                        Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + inviterFaction.name, senderClient.userName + " has joined the faction.");
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Vars.conLog.Error("FJOIN2: " + ex.ToString());
                                            }
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
                            if (faction != null)
                            {
                                FactionMember member = faction.GetMember(senderClient.userID);
                                string rank = member.rank;

                                if (rank != "owner")
                                {
                                    foreach (var m in faction.members)
                                    {
                                        PlayerClient pc;
                                        if (Vars.getPlayerClient(m.userID, out pc))
                                        {
                                            Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + faction.name, senderClient.userName + " has left the faction.");
                                        }
                                    }
                                    try
                                    {
                                        Data.saveFactions();
                                        //Data.remFactionData(faction.name, senderClient.userName, rank);
                                        faction.RemoveMember(senderClient.userID);
                                        if (Vars.latestFactionRequests.ContainsKey(senderClient.userID))
                                            Vars.latestFactionRequests.Remove(senderClient.userID);
                                    }
                                    catch (Exception ex)
                                    {
                                        Vars.conLog.Error("FLEAVE: " + ex.ToString());
                                    }
                                }
                                else
                                    Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You cannot leave your faction without disbanding or passing ownership.");
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
                        if (faction != null)
                        {
                            FactionMember member = faction.GetMember(senderClient.userID);
                            string rank = member.rank;

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
                                            FactionMember targetMember = faction.GetMember(targetClient.userID);
                                            string targetRank = member.rank;
                                            if (targetRank != "admin")
                                            {
                                                Data.saveFactions();
                                                //Data.remFactionData(faction.name, targetClient.userName, targetRank);
                                                faction.SetRank(targetClient.userID, "admin");
                                                Data.saveFactions();
                                                //Data.addFactionData(faction.name, targetClient.userName, targetClient.userID.ToString(), "admin");
                                                foreach (var m in faction.members)
                                                {
                                                    PlayerClient pc;
                                                    if (Vars.getPlayerClient(m.userID, out pc))
                                                    {
                                                        Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + faction.name, targetClient.userName + " is now a faction admin.");
                                                    }
                                                }
                                            }
                                            else
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You cannot admin a player who is already an admin.");
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You cannot admin yourself.");
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
                                            FactionMember targetMember = faction.GetMember(targetClient.userID);
                                            string targetRank = member.rank;
                                            if (targetRank != "admin")
                                            {
                                                Data.saveFactions();
                                                //Data.remFactionData(faction.name, targetClient.userName, targetRank);
                                                faction.SetRank(targetClient.userID, "admin");
                                                Data.saveFactions();
                                                //Data.addFactionData(faction.name, targetClient.userName, targetClient.userID.ToString(), "admin");
                                                foreach (var m in faction.members)
                                                {
                                                    PlayerClient pc;
                                                    if (Vars.getPlayerClient(m.userID, out pc))
                                                    {
                                                        Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + faction.name, targetClient.userName + " is now a faction admin.");
                                                    }
                                                }
                                            }
                                            else
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You cannot admin a player who is already an admin.");
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You cannot admin yourself.");
                                    }
                                }
                            }
                            else
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You do not have permission to assign admin to players.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        break;
                    case "deadmin":
                        if (faction != null)
                        {
                            FactionMember member = faction.GetMember(senderClient.userID);
                            string rank = member.rank;

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
                                            FactionMember targetMember = faction.GetMember(targetClient.userID);
                                            string targetRank = member.rank;
                                            if (targetRank == "admin")
                                            {
                                                Data.saveFactions();
                                                //Data.remFactionData(faction.name, targetClient.userName, targetRank);
                                                faction.SetRank(targetClient.userID, "normal");
                                                Data.saveFactions();
                                                //Data.addFactionData(faction.name, targetClient.userName, targetClient.userID.ToString(), "normal");
                                                foreach (var m in faction.members)
                                                {
                                                    PlayerClient pc;
                                                    if (Vars.getPlayerClient(m.userID, out pc))
                                                    {
                                                        Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + faction.name, targetClient.userName + " is no longer a faction admin.");
                                                    }
                                                }
                                            }
                                            else
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You cannot deadmin a player who is not an admin.");
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You cannot deadmin yourself.");
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
                                            FactionMember targetMember = faction.GetMember(targetClient.userID);
                                            string targetRank = member.rank;
                                            if (targetRank == "admin")
                                            {
                                                Data.saveFactions();
                                                //Data.remFactionData(faction.name, targetClient.userName, targetRank);
                                                faction.SetRank(targetClient.userID, "normal");
                                                Data.saveFactions();
                                                //Data.addFactionData(faction.name, targetClient.userName, targetClient.userID.ToString(), "normal");
                                                foreach (var m in faction.members)
                                                {
                                                    PlayerClient pc;
                                                    if (Vars.getPlayerClient(m.userID, out pc))
                                                    {
                                                        Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + faction.name, targetClient.userName + " is no longer a faction admin.");
                                                    }
                                                }
                                            }
                                            else
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You cannot deadmin a player who is not an admin.");
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You cannot deadmin yourself.");
                                    }
                                }
                            }
                            else
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You do not have permission to revoke admin from players.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        break;
                    case "ownership":
                        if (faction != null)
                        {
                            FactionMember member = faction.GetMember(senderClient.userID);
                            string rank = member.rank;

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
                                            string targetRank = member.rank;

                                            //Data.remFactionData(faction.name, senderClient.userName, rank);

                                            Data.saveFactions();
                                            //Data.remFactionData(faction.name, targetClient.userName, targetRank);
                                            faction.SetRank(targetClient.userID, "owner");
                                            //Data.addFactionData(faction.name, targetClient.userName, targetClient.userID.ToString(), "owner");

                                            faction.SetRank(senderClient.userID, "normal");
                                            Data.saveFactions();
                                            //Data.addFactionData(faction.name, senderClient.userName, targetClient.userID.ToString(), "normal");

                                            foreach (var m in faction.members)
                                            {
                                                PlayerClient pc;
                                                if (Vars.getPlayerClient(m.userID, out pc))
                                                {
                                                    Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + faction.name, targetClient.userName + " now owns the faction.");
                                                }
                                            }
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You are already the owner of this faction.");
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
                                            FactionMember targetMember = faction.GetMember(targetClient.userID);
                                            string targetRank = member.rank;

                                            //Data.remFactionData(faction.name, senderClient.userName, rank);

                                            Data.saveFactions();
                                            //Data.remFactionData(faction.name, targetClient.userName, targetRank);
                                            faction.SetRank(targetClient.userID, "owner");
                                            //Data.addFactionData(faction.name, targetClient.userName, targetClient.userID.ToString(), "owner");

                                            faction.SetRank(senderClient.userID, "normal");
                                            Data.saveFactions();
                                            //Data.addFactionData(faction.name, senderClient.userName, targetClient.userID.ToString(), "normal");

                                            foreach (var m in faction.members)
                                            {
                                                PlayerClient pc;
                                                if (Vars.getPlayerClient(m.userID, out pc))
                                                {
                                                    Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + faction.name, targetClient.userName + " now owns the faction.");
                                                }
                                            }
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You are already the owner of this faction.");
                                    }
                                }
                            }
                            else
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You do not have permission to pass ownership.");
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

                            foreach (var f in Vars.factions)
                            {
                                if (!factionNames.ContainsKey(currentPage.ToString()))
                                {
                                    factionNames.Add(currentPage.ToString(), new List<string>() { { f.name } });
                                }
                                else
                                {
                                    if (factionNames[currentPage.ToString()].Count <= 20)
                                        factionNames[currentPage.ToString()].Add(f.name);
                                    else
                                    {
                                        currentPage++;
                                        if (!factionNames.ContainsKey(currentPage.ToString()))
                                        {
                                            factionNames.Add(currentPage.ToString(), new List<string>() { { f.name } });
                                        }
                                        else
                                        {
                                            factionNames[currentPage.ToString()].Add(f.name);
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
                        if (faction != null)
                        {
                            FactionMember member = faction.GetMember(senderClient.userID);
                            string rank = member.rank;

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

                                    FactionList factionResults = Vars.factions.GetListByName(factionName);

                                    if (factionResults.Count() == 0)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "No factions equal or contain \"" + factionName + "\".");
                                    else if (factionResults.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many faction names contain \"" + factionName + "\".");
                                    else
                                    {
                                        if (faction.name != factionResults[0].name)
                                        {
                                            if (faction.allies.Contains(factionResults[0].name))
                                            {
                                                foreach (var m in faction.members)
                                                {
                                                    PlayerClient pc;
                                                    if (Vars.getPlayerClient(m.userID, out pc))
                                                    {
                                                        Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + faction.name, "Your faction is now allied with the faction [" + factionResults[0].name + "].");
                                                    }
                                                }
                                                faction.AddAlly(factionResults[0].name);
                                                Data.saveFactions();
                                            }
                                            else
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You are already allied with [" + factionResults[0].name + "].");
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You cannot ally your own faction.");
                                    }
                                }
                                else
                                    Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You must specify a faction name in order to form an alliance.");
                            }
                            else
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You do not have permission to form alliances.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        break;
                    case "unally":
                        if (faction != null)
                        {
                            FactionMember member = faction.GetMember(senderClient.userID);
                            string rank = member.rank;

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

                                    FactionList factionResults = Vars.factions.GetListByName(factionName);

                                    if (factionResults.Count() == 0)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "No factions equal or contain \"" + factionName + "\".");
                                    else if (factionResults.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many faction names contain \"" + factionName + "\".");
                                    else
                                    {
                                        if (faction.allies.Contains(factionResults[0].name))
                                        {
                                            foreach (var m in faction.members)
                                            {
                                                PlayerClient pc;
                                                if (Vars.getPlayerClient(m.userID, out pc))
                                                {
                                                    Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + faction.name, "Your faction is no longer allied with the faction [" + factionResults[0].name + "].");
                                                }
                                            }
                                            faction.RemoveAlly(factionResults[0].name);
                                            Data.saveFactions();
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You are not allied with [" + factionResults[0].name + "].");
                                    }
                                }
                                else
                                    Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You must specify a faction name in order to remove an alliance.");
                            }
                            else
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You do not have permission to remove alliances.");
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
                            FactionList factionResults = Vars.factions.GetListByName(factionName);

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
                                        Faction playerFaction = Vars.factions.GetByMember(possibleClients[0].userID);
                                        int onlineMembers = 0;
                                        if (playerFaction != null)
                                        {
                                            foreach (var m in playerFaction.members)
                                            {
                                                PlayerClient[] possibleClients2 = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID == m.userID);
                                                if (possibleClients2.Count() > 0)
                                                    onlineMembers++;
                                            }
                                            string ownerName = playerFaction.owner;

                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.name, "=== [" + playerFaction.name + "]'s information ===");
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.name, "Total members: " + playerFaction.members.Count);
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.name, "Online members: " + onlineMembers);
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.name, "Offline members: " + (playerFaction.members.Count - onlineMembers));
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.name, "Owner: " + ownerName);
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.name, "Members:");
                                            List<string> names = new List<string>();
                                            List<string> names2 = new List<string>();
                                            foreach (var m in playerFaction.members)
                                            {
                                                if (m.name != ownerName)
                                                    names.Add(m.name);
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
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.name, string.Join(", ", names2.ToArray()));
                                            }
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.name, "Allies:");
                                            List<string> allies = new List<string>();
                                            List<string> allies2 = new List<string>();
                                            foreach (string name in playerFaction.allies)
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
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.name, string.Join(", ", allies2.ToArray()));
                                            }
                                        }
                                        else 
                                            Broadcast.broadcastTo(senderClient.netPlayer, possibleClients[0].userName + " is not in a faction.");
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
                                foreach (var m in factionResults[0].members)
                                {
                                    PlayerClient playerClient;
                                    if (Vars.getPlayerClient(m.userID, out playerClient)) 
                                        onlineMembers++;
                                }
                                string ownerName = factionResults[0].owner;

                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].name, "=== [" + factionResults[0].name + "]'s information ===");
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].name, "Total members: " + factionResults[0].members.Count);
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].name, "Online members: " + onlineMembers);
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].name, "Offline members: " + (factionResults[0].members.Count - onlineMembers));
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].name, "Owner: " + ownerName);
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].name, "Members:");
                                List<string> names = new List<string>();
                                List<string> names2 = new List<string>();
                                foreach (var m in factionResults[0].members)
                                {
                                    if (m.name != ownerName)
                                        names.Add(m.name);
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
                                    Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].name, string.Join(", ", names2.ToArray()));
                                }
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].name, "Allies:");
                                List<string> allies = new List<string>();
                                List<string> allies2 = new List<string>();
                                foreach (string name in factionResults[0].allies)
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
                                    Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].name, string.Join(", ", allies2.ToArray()));
                                }
                            }
                        }
                        else
                        {
                            if (faction != null)
                            {
                                int onlineMembers = 0;
                                foreach (var m in faction.members)
                                {
                                    PlayerClient playerClient;
                                    if (Vars.getPlayerClient(m.userID, out playerClient))
                                        onlineMembers++;
                                }
                                string ownerName = faction.owner;

                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "=== Your faction's information ===");
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "Total members: " + faction.members.Count);
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "Online members: " + onlineMembers);
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "Offline members: " + (faction.members.Count - onlineMembers));
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "Owner: " + ownerName);
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "Members:");
                                List<string> names = new List<string>();
                                List<string> names2 = new List<string>();
                                foreach (var m in faction.members)
                                {
                                    if (m.name != ownerName)
                                        names.Add(m.name);
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
                                    Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, string.Join(", ", names2.ToArray()));
                                }
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "Allies:");
                                List<string> allies = new List<string>();
                                List<string> allies2 = new List<string>();
                                foreach (string name in faction.allies)
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
                                    Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, string.Join(", ", allies2.ToArray()));
                                }
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        }
                        break;
                    case "players":
                        if (faction != null)
                        {
                            List<ulong> UIDs = new List<ulong>();
                            List<string> onlineNames = new List<string>();
                            List<string> offlineNames = new List<string>();
                            foreach (var m in faction.members)
                            {
                                UIDs.Add(m.userID);
                            }

                            List<ulong> otherUIDs = new List<ulong>();
                            bool saidOnline = false;
                            bool saidOffline = false;
                            while (UIDs.Count > 0)
                            {
                                onlineNames.Clear();
                                offlineNames.Clear();
                                otherUIDs.Clear();
                                bool hasOnline = false;
                                foreach (ulong s in UIDs)
                                {
                                    PlayerClient[] possibleClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID == s);

                                    if (possibleClients.Count() > 0)
                                    {
                                        if (possibleClients[0].userName.Length > 0)
                                            hasOnline = true;
                                    }
                                }
                                foreach (ulong s in UIDs)
                                {
                                    if (hasOnline)
                                    {
                                        PlayerClient[] possibleClients = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID == s);
                                        string playerName = faction.members.Get(s).name;

                                        if (possibleClients.Count() > 0)
                                        {
                                            if (!saidOnline)
                                            {
                                                saidOnline = true;
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "All online faction members:");
                                            }

                                            onlineNames.Add(playerName);
                                            otherUIDs.Add(s);

                                            if ((string.Join(", ", onlineNames.ToArray())).Length > 70)
                                            {
                                                onlineNames.Remove(playerName);
                                                otherUIDs.Remove(s);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string playerName = faction.members.Get(s).name;

                                        if (!saidOffline)
                                        {
                                            saidOffline = true;
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "All offline faction members:");
                                        }

                                        offlineNames.Add(playerName);
                                        otherUIDs.Add(s);

                                        if ((string.Join(", ", offlineNames.ToArray())).Length > 70)
                                        {
                                            offlineNames.Remove(playerName);
                                            otherUIDs.Remove(s);
                                            break;
                                        }
                                    }
                                }
                                foreach (ulong s in otherUIDs)
                                {
                                    UIDs.Remove(s);
                                }
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, string.Join(", ", (hasOnline ? onlineNames.ToArray() : offlineNames.ToArray())));
                            }
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        break;
                    case "online":
                        if (faction != null)
                        {
                            int onlineMembers = 0;
                            foreach (var m in faction.members)
                            {
                                PlayerClient playerClient;
                                if (Vars.getPlayerClient(m.userID, out playerClient))
                                    onlineMembers++;
                            }

                            if (Vars.memberLimit > 0)
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, onlineMembers + "/" + faction.members.Count + " faction members currently connected. Faction is at " + faction.members.Count + "/" + Vars.memberLimit + " member capacity.");
                            else
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, onlineMembers + "/" + faction.members.Count + " faction members currently connected. There is no member capacity.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        break;
                    case "sethome":
                        if (Vars.enabledCommands[rankToUse].Contains("/f sethome"))
                        {
                            if (faction != null)
                            {
                                Character playerChar;
                                if (Vars.getPlayerChar(senderClient, out playerChar))
                                {
                                    faction.home = new FactionHome(playerChar.eyesOrigin);
                                    Data.saveFactions();
                                    Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "Faction home set! " + playerChar.eyesOrigin);
                                }
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You don't have permission to do that.");
                        break;
                    case "home":
                        if (Vars.enabledCommands[rankToUse].Contains("/f home"))
                        {
                            if (faction != null)
                            {
                                if (Vars.blockedFHomes.ContainsKey(senderClient.userID))
                                {
                                    double timeLeft = Math.Round((Vars.blockedFHomes[senderClient.userID].timeLeft / 1000));
                                    if (timeLeft > 0)
                                    {
                                        TimeSpan timeSpan = TimeSpan.FromMilliseconds(Vars.blockedFHomes[senderClient.userID].timeLeft);

                                        string timeString = timeSpan.Minutes + " minutes, and " + timeSpan.Seconds + " seconds";

                                        Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You cannot teleport to your faction home for " + timeString);
                                        return;
                                    }
                                }
                                if (!Vars.enableInHouse && Checks.inHouse(senderClient))
                                {
                                    Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You cannot teleport to your faction home while in a house.");
                                }
                                else if (Vars.isTeleporting.Contains(senderClient))
                                {
                                    Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "You are already mid-teleport!");
                                }
                                else
                                {
                                    if (Vars.factionHomeDelay > 0)
                                        Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "Teleporting to the faction home in " + Vars.factionHomeDelay + " seconds...");
                                    else
                                        Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + faction.name, "Teleporting to the faction home!");
                                    TimerPlus tp = TimerPlus.Create(Vars.factionHomeCooldown, false, Vars.unblockFactionHomeTP, senderClient.userID);
                                    Vars.blockedFHomes.Add(senderClient.userID, tp);
                                    Vars.REB.StartCoroutine(Vars.homeTeleporting(senderClient, faction.home.origin, true));
                                    Vars.isTeleporting.Add(senderClient);
                                }
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You don't have permission to do that.");
                        break;
                    case "sethomea":
                        if (Vars.enabledCommands[rankToUse].Contains("/f sethomea"))
                        {
                            if (args.Count() > 2)
                            {
                                Faction targetFaction = Vars.factions.GetByName(args[2]);
                                Character playerChar;
                                if (Vars.getPlayerChar(senderClient, out playerChar) && targetFaction != null)
                                {
                                    targetFaction.home = new FactionHome(playerChar.eyesOrigin);
                                    Data.saveFactions();
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Faction [" + targetFaction.name + "]'s home set! " + playerChar.eyesOrigin);
                                }
                                else
                                    Broadcast.broadcastTo(senderClient.netPlayer, "No such faction named \"" + args[2] + "\".");
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "You need to specify a faction.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You don't have permission to do that.");
                        break;
                    case "homea":
                        if (Vars.enabledCommands[rankToUse].Contains("/f homea"))
                        {
                            if (args.Count() > 2)
                            {
                                Faction targetFaction = Vars.factions.GetByName(args[2]);
                                if (targetFaction != null)
                                {
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting to the [" + args[2] + "] faction home!");
                                    Vars.simulateTeleport(senderClient, targetFaction.home.origin);
                                }
                                else
                                    Broadcast.broadcastTo(senderClient.netPlayer, "No such faction named \"" + args[2] + "\".");
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "You need to specify a faction.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You don't have permission to do that.");
                        break;
                    case "build":
                        if (Vars.enabledCommands[rankToUse].Contains("/f build"))
                        {
                            if (args.Count() > 2)
                            {
                                string mode = args[2];
                                ulong UID = senderClient.userID;

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
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You don't have permission to do that.");
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
                                    ulong UID = senderClient.userID;

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
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You don't have permission to do that.");
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
                        if (Vars.enabledCommands[rankToUse].Contains("/f sethome"))
                            Broadcast.broadcastTo(senderClient.netPlayer, "/f sethome: Sets your faction's home at your current position.");
                        if (Vars.enabledCommands[rankToUse].Contains("/f home"))
                            Broadcast.broadcastTo(senderClient.netPlayer, "/f home: Teleports you to your faction's home.");
                        if (Vars.enabledCommands[rankToUse].Contains("/f sethomea"))
                            Broadcast.broadcastTo(senderClient.netPlayer, "/f sethome [faction]: Sets a faction's home at your current position.");
                        if (Vars.enabledCommands[rankToUse].Contains("/f homea"))
                            Broadcast.broadcastTo(senderClient.netPlayer, "/f home [faction]: Teleports you to a faction's home.");
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
                            foreach (var zone in Vars.inSafeZone)
                            {
                                PlayerClient pc = zone.Key;
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
                            foreach (var zone in Vars.inWarZone)
                            {
                                PlayerClient pc = zone.Key;
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
