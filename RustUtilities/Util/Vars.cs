/**
 * @file: Vars.cs
 * @author: Team Cerionn (https://github.com/Team-Cerionn)
 * @version: 1.0.0.0
 * @description: Vars class for Rust Essentials
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using UnityEngine;

namespace RustEssentials.Util
{
    public class Vars
    {
        public static string rootDir = Directory.GetCurrentDirectory();
        public static string dataDir = Path.Combine(rootDir, "rust_server_Data");
        public static string dllDir = Path.Combine(dataDir, "Managed");
        public static string modsDir = Path.Combine(dataDir, "mods");
        public static string saveDir = Path.Combine(rootDir, "save\\RustEssentials");
        public static string logsDir = Path.Combine(saveDir, "Logs");
        public static string cfgFile = Path.Combine(saveDir, "config.ini");
        public static string whiteListFile = Path.Combine(saveDir, "whitelist.txt");
        public static string ranksFile = Path.Combine(saveDir, "ranks.ini");
        public static string commandsFile = Path.Combine(saveDir, "commands.ini");
        public static string allCommandsFile = Path.Combine(saveDir, "allCommands.txt");
        public static string itemsFile = Path.Combine(saveDir, "itemIDs.txt");
        public static string kitsFile = Path.Combine(saveDir, "kits.ini");
        public static string motdFile = Path.Combine(saveDir, "motd.ini");
        public static string bansFile = Path.Combine(saveDir, "bans.txt");
        public static string prefixFile = Path.Combine(saveDir, "prefix.txt");
        public static string doorsFile = Path.Combine(saveDir, "door_data.dat");
        public static string factionsFile = Path.Combine(saveDir, "factions.dat");
        public static string defaultRank = "Default";
        public static string currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, 5);
        public static string rustCurrentVer = "4.3.1.93830";
        public static string remoteVersion = "?.?.?";
        public static string currentLog;
        public static string currentChatLog;

        public static Logging conLog = new Logging();

        // SAVED VARIABLES START
        public static bool enableWhitelist = false;
        public static bool useMySQL = false;
        public static bool useSteamGroup = true;
        public static bool autoRefresh = true;
        public static bool whitelistToMembers = false;
        public static bool announceDrops = true;
        public static bool directChat = true;
        public static bool globalChat = true;
        public static bool logPluginChat = true;
        public static bool unknownCommand = true;
        public static bool enableJoin = true;
        public static bool enableLeave = true;
        public static bool teleportRequestOn = true;
        public static bool removeTag = true;
        public static bool nextToName = true;
        public static bool removePrefix = true;
        public static bool inheritCommands = true;
        public static bool inheritKits = true;
        public static bool suicideMessages = false;
        public static bool murderMessages = true;
        public static bool accidentMessages = true;
        public static bool fallDamage = true;

        public static string whitelistKickCMD = "Whitelist was enabled and you are not whitelisted.";
        public static string whitelistKickJoin = "You are not whitelisted!";
        public static string whitelistCheckGood = "You are whitelisted!";
        public static string whitelistCheckBad = "You are not whitelisted!";
        public static string defaultChat = "direct";
        public static string botName = "Essentials";
        public static string joinMessage = "Player $USER$ has joined.";
        public static string leaveMessage = "Player $USER$ has left.";
        public static string steamGroup = "";
        public static string suicideMessage = "$VICTIM$ killed himself.";
        public static string murderMessage = "$KILLER$ [$WEAPON$ ($PART$)] $VICTIM$";
        public static string accidentMessage = "$VICTIM$ got mauled by a $KILLER$.";

        public static int cycleInterval = 900000;
        public static int refreshInterval = 15000;
        public static int directDistance = 150;
        public static int chatLogCap = 15;
        public static int logCap = 15;
        // SAVED VARIABLES END

        public static bool noErrors = true;
        public static bool timeFrozen = false;
        public static bool firstPlayerJoined = false;

        public static List<string> allFiles = new List<string>()
        {
            { cfgFile },
            { whiteListFile },
            { ranksFile },
            { commandsFile },
            { itemsFile },
            { allCommandsFile },
            { kitsFile },
            { motdFile },
            { doorsFile },
            { factionsFile },
            { bansFile },
            { prefixFile }
        };
        public static List<string> allDirs = new List<string>()
        {
            { rootDir },
            { saveDir },
            { logsDir }
        };
        public static List<string> whitelist = new List<string>();
        public static List<string> totalCommands = new List<string>();
        public static List<string> inGlobal = new List<string>();
        public static List<string> inDirect = new List<string>();
        public static List<string> inGlobalV = new List<string>();
        public static List<string> inDirectV = new List<string>();
        public static List<string> inFactionV = new List<string>();
        public static List<string> mutedUsers = new List<string>();
        public static List<string> historyGlobal = new List<string>();
        public static List<string> historyDirect = new List<string>();
        public static List<string> kickQueue = new List<string>();
        public static List<string> groupMembers = new List<string>();
        public static List<string> completeDoorAccess = new List<string>();
        public static List<string> godList = new List<string>();
        public static List<string> destroyerList = new List<string>();
        public static List<GameObject> beingDestroyed = new List<GameObject>();
        public static List<PlayerClient> killList = new List<PlayerClient>();
        public static List<PlayerClient> isTeleporting = new List<PlayerClient>();
        public static List<PlayerClient> isAccepting = new List<PlayerClient>();
        public static List<PlayerClient> wasHit = new List<PlayerClient>();

        public static Dictionary<string, string> sharingData = new Dictionary<string, string>();
        public static Dictionary<string, string> playerPrefixes = new Dictionary<string, string>();
        public static Dictionary<string, string> rankPrefixes = new Dictionary<string, string>();
        public static Dictionary<string, string> currentBans = new Dictionary<string, string>();
        public static Dictionary<string, string> currentBanReasons = new Dictionary<string, string>();
        public static Dictionary<string, List<string>> rankList = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> motdList = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> enabledCommands = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> kitsForRanks = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> factionInvites = new Dictionary<string, List<string>>();
        public static Dictionary<string, TimerPlus> muteTimes = new Dictionary<string, TimerPlus>();
        public static Dictionary<PlayerClient, PlayerClient> latestPM = new Dictionary<PlayerClient, PlayerClient>();
        public static Dictionary<PlayerClient, PlayerClient> latestRequests = new Dictionary<PlayerClient, PlayerClient>();
        public static Dictionary<PlayerClient, PlayerClient> latestFactionRequests = new Dictionary<PlayerClient, PlayerClient>();
        public static Dictionary<PlayerClient, Dictionary<PlayerClient, TimerPlus>> teleportRequests = new Dictionary<PlayerClient, Dictionary<PlayerClient, TimerPlus>>();
        public static Dictionary<string, Dictionary<string, string>> factions = new Dictionary<string, Dictionary<string, string>>();
        public static List<string> unassignedKits = new List<string>();
        public static Dictionary<int, string> itemIDs = new Dictionary<int, string>();
        public static Dictionary<string, Dictionary<string, int>> kits = new Dictionary<string, Dictionary<string, int>>();
        public static Dictionary<string, int> kitCooldowns = new Dictionary<string, int>();
        public static Dictionary<string, Dictionary<TimerPlus, string>> playerCooldowns = new Dictionary<string, Dictionary<TimerPlus, string>>();
        public static Dictionary<string, StringBuilder> textForFiles = new Dictionary<string, StringBuilder>()
        {
            { cfgFile, cfgText() },
            { ranksFile, ranksText() },
            { commandsFile, commandsText() },
            { allCommandsFile, allCommandsText() },
            { kitsFile, kitsText() },
            { motdFile, motdText() },
            { prefixFile, prefixText() }
        };

        public static string filterNames(string playerName, string uid)
        {
            foreach (KeyValuePair<string, string> kv in rankPrefixes)
            {
                playerName = playerName.Replace("[" + kv.Key + "]", "");
                playerName = playerName.Replace(kv.Value, "");
            }

            foreach (KeyValuePair<string, string> kv in playerPrefixes)
            {
                playerName = playerName.Replace("[" + kv.Value + "]", "");
            }

            playerName = playerName.Replace("<G> ", "");
            playerName = playerName.Replace("* <G> ", "");
            playerName = playerName.Replace("<D> ", "");
            playerName = playerName.Replace("* <D> ", "");

            if (!playerPrefixes.ContainsKey(uid))
            {
                foreach (KeyValuePair<string, List<string>> kv in rankList)
                {
                    if (kv.Value.Contains(uid))
                    {
                        if (rankPrefixes.Keys.Contains(kv.Key))
                        {
                            playerName = (removePrefix ? "" : rankPrefixes[kv.Key] + " ") + playerName;
                            break;
                        }
                    }
                }
            }
            else
            {
                playerName = "[" + playerPrefixes[uid] + "] " + playerName;
            }

            return playerName;
        }

        public static void SetDeathReason(PlayerClient playerClient, ref DamageEvent damage)
        {
            if ((playerClient != null) && NetCheck.PlayerValid(playerClient.netPlayer))
            {
                IDMain idMain = damage.attacker.idMain;
                string message = "";
                if (idMain != null)
                {
                    idMain = idMain.idMain;
                }
                if (idMain is Character)
                {
                    Character character = idMain as Character;
                    Controller playerControlledController = character.playerControlledController;
                    if (playerControlledController != null)
                    {
                        if (playerControlledController.playerClient == playerClient)
                        {
                            if (killList.Contains(playerClient))
                            {
                                killList.Remove(playerClient);
                                DeathScreen.SetReason(playerClient.netPlayer, "You fell victim to /kill");
                                Broadcast.broadcastAll(playerClient.userName + " fell victim to /kill.");
                            }
                            else
                            {
                                DeathScreen.SetReason(playerClient.netPlayer, "You killed yourself. You silly sod.");
                                if (Vars.suicideMessages)
                                {
                                    message = Vars.suicideMessage.Replace("$VICTIM$", playerClient.userName);

                                    Broadcast.broadcastAll(message);
                                }
                            }
                            return;
                        }
                        Character killerChar;
                        Character victimChar;
                        Character.FindByUser(playerControlledController.playerClient.userID, out killerChar);
                        Character.FindByUser(playerClient.userID, out victimChar);
                        Vector3 killerPos = killerChar.transform.position;
                        Vector3 victimPos = victimChar.transform.position;
                        double distance = Math.Round(Vector3.Distance(killerPos, victimPos));

                        WeaponImpact extraData = damage.extraData as WeaponImpact;
                        if (extraData != null)
                        {
                            if (Vars.murderMessages)
                            {
                                message = Vars.murderMessage.Replace("$VICTIM$", playerClient.userName).Replace("$KILLER$", playerControlledController.playerClient.userName).Replace("$WEAPON$", extraData.dataBlock.name);
                                if (Vars.murderMessage.Contains("$PART$"))
                                    message = message.Replace("$PART$", BodyParts.GetNiceName(damage.bodyPart));
                                if (Vars.murderMessage.Contains("$DISTANCE$"))
                                    message = message.Replace("$DISTANCE$", Convert.ToString(distance) + "m");

                                Broadcast.broadcastAll(message);
                            }
                            DeathScreen.SetReason(playerClient.netPlayer, playerControlledController.playerClient.userName + " killed you using a " + extraData.dataBlock.name + " with a hit to your " + BodyParts.GetNiceName(damage.bodyPart));
                            return;
                        }
                        if (Vars.murderMessages)
                        {
                            message = Vars.murderMessage.Replace("$VICTIM$", playerClient.userName).Replace("$KILLER$", playerControlledController.playerClient.userName).Replace("[$WEAPON$]", "killed");
                            if (Vars.murderMessage.Contains("$PART$"))
                                message = message.Replace("$PART$", BodyParts.GetNiceName(damage.bodyPart));
                            if (Vars.murderMessage.Contains("$DISTANCE$"))
                                message = message.Replace("$DISTANCE$", Convert.ToString(distance) + "m");

                            Broadcast.broadcastAll(message);
                        }
                        DeathScreen.SetReason(playerClient.netPlayer, playerControlledController.playerClient.userName + " killed you with a hit to your " + BodyParts.GetNiceName(damage.bodyPart));
                        return;
                    }
                }

                string killer = idMain.ToString();
                if (killer.Contains("(Clone)"))
                    killer = killer.Substring(0, killer.IndexOf("(Clone)"));

                switch (killer)
                {
                    case "ZombieNPC_SLOW":
                        killer = "Dark Zombie";
                        break;
                    case "ZombieNPC_FAST":
                        killer = "Red Zombie";
                        break;
                    case "ZombieNPC":
                        killer = "Zombie";
                        break;
                }

                if (Vars.accidentMessages)
                {
                    message = Vars.accidentMessage.Replace("$VICTIM$", playerClient.userName).Replace("$KILLER$", killer);

                    Broadcast.broadcastAll(message);
                }

                DeathScreen.SetReason(playerClient.netPlayer, "You were killed by a " + killer);
            }
        }

        public static void FallImpact(float fallspeed, float min_vel, float max_vel, float healthFraction, Character idMain, float maxHealth, FallDamage fallDamage)
        {
            try
            {
                if (fallDamage)
                {
                    float num = (fallspeed - min_vel) / (max_vel - min_vel);
                    bool flag = num > 0.25f;
                    bool flag2 = ((num > 0.35f) || (UnityEngine.Random.Range(0, 3) == 0)) || (healthFraction < 0.5f);
                    if (!godList.Contains(idMain.playerClient.userID.ToString()))
                    {
                        if (flag)
                        {
                            idMain.controllable.GetComponent<HumanBodyTakeDamage>().AddBleedingLevel(3f + ((num - 0.25f) * 10f));
                        }
                        if (flag2)
                        {
                            fallDamage.AddLegInjury(1f);
                        }
                        TakeDamage.HurtSelf(idMain, (float)(10f + (num * maxHealth)));
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error(ex.ToString());
            }
        }

        public static void handleFactions(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string arg = args[1];
                KeyValuePair<string, Dictionary<string, string>>[] possibleFactions = Array.FindAll(factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(senderClient.userID.ToString()));

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

                            if (!factions.ContainsKey(factionName))
                            {
                                if (possibleFactions.Count() == 0)
                                {
                                    if (factionName.Length < 15)
                                    {
                                        factions.Add(factionName, new Dictionary<string, string>());
                                        factions[factionName].Add(senderClient.userID.ToString(), "owner");
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Faction \"" + factionName + "\" created.");
                                        addFactionData(factionName, senderClient.userName, senderClient.userID.ToString(), "owner");
                                    }
                                    else
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Faction names must be less than 15 characters!");
                                }
                                else
                                    Broadcast.broadcastTo(senderClient.netPlayer, "You are already in the faction \"" + factionName + "\".");
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "Faction \"" + factionName + "\" already exists.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "Improper syntax! Syntax: /f create *name*");
                        break;
                    case "disband":
                        if (possibleFactions.Count() > 0)
                        {
                            string rank = possibleFactions[0].Value[senderClient.userID.ToString()];

                            if (rank == "owner" || completeDoorAccess.Contains(senderClient.userID.ToString()))
                            {
                                PlayerClient[] targetClients = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                foreach (PlayerClient pc in targetClients)
                                {
                                    Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, "Your faction was disbanded.");
                                }
                                factions.Remove(possibleFactions[0].Key);
                                remFactionData(possibleFactions[0].Key, "disband", "");
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
                                PlayerClient[] possibleTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));

                                if (possibleTargets.Count() == 0)
                                {
                                    Broadcast.broadcastTo(senderClient.netPlayer, "No member equal or contain \"" + targetName + "\".");
                                }
                                else if (possibleTargets.Count() > 1)
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Too many member names contain \"" + targetName + "\".");
                                else
                                {
                                    PlayerClient targetClient = possibleTargets[0];

                                    if (possibleFactions[0].Value.ContainsKey(targetClient.userID.ToString()))
                                    {
                                        PlayerClient[] targetClients = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                        foreach (PlayerClient pc in targetClients)
                                        {
                                            Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, targetClient.userName + " was kicked from the faction.");
                                        }
                                        remFactionData(possibleFactions[0].Key, targetClient.userName, possibleFactions[0].Value[targetClient.userID.ToString()]);
                                        possibleFactions[0].Value.Remove(targetClient.userID.ToString());
                                    }
                                    else
                                        Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, targetClient.userName + " is not in your faction.");
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
                                PlayerClient[] possibleTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));

                                if (possibleTargets.Count() == 0)
                                    Broadcast.broadcastTo(senderClient.netPlayer, "No players equal or contain \"" + targetName + "\".");
                                else if (possibleTargets.Count() > 1)
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + targetName + "\".");
                                else
                                {
                                    PlayerClient targetClient = possibleTargets[0];

                                    if (possibleFactions[0].Value.Count < 15)
                                    {
                                        if (!factionInvites.ContainsKey(targetClient.userID.ToString()))
                                        {
                                            factionInvites.Add(targetClient.userID.ToString(), new List<string>() { { possibleFactions[0].Key } });

                                            Broadcast.broadcastTo(senderClient.netPlayer, "You invited \"" + targetClient.userName + "\" to the faction .");
                                            Broadcast.broadcastTo(targetClient.netPlayer, "You were invited to the faction \"" + possibleFactions[0].Key + "\".");
                                            if (!latestFactionRequests.ContainsKey(targetClient))
                                                latestFactionRequests.Add(targetClient, senderClient);
                                            else
                                                latestFactionRequests[targetClient] = senderClient;
                                        }
                                        else
                                        {
                                            if (!factionInvites[targetClient.userID.ToString()].Contains(possibleFactions[0].Key))
                                            {
                                                factionInvites[targetClient.userID.ToString()].Add(possibleFactions[0].Key);

                                                Broadcast.broadcastTo(senderClient.netPlayer, "You invited \"" + targetClient.userName + "\" to the faction .");
                                                Broadcast.broadcastTo(targetClient.netPlayer, "You were invited to the faction \"" + possibleFactions[0].Key + "\".");
                                                if (!latestFactionRequests.ContainsKey(targetClient))
                                                    latestFactionRequests.Add(targetClient, senderClient);
                                                else
                                                    latestFactionRequests[targetClient] = senderClient;
                                            }
                                            else
                                                Broadcast.broadcastCustomTo(targetClient.netPlayer, "[F] " + possibleFactions[0].Key, targetClient.userName + " has already been invited.");
                                        }
                                    }
                                    else
                                        Broadcast.broadcastCustomTo(targetClient.netPlayer, "[F] " + possibleFactions[0].Key, "You have reached your member capacity.");
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
                            if (factionInvites.ContainsKey(senderClient.userID.ToString()))
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
                                    foreach(string s in factionInvites[senderClient.userID.ToString()])
                                    {
                                        if (s.Contains(targetFaction))
                                        {
                                            inviterFactions.Add(s);
                                        }
                                    }
                                    
                                    if (inviterFactions.Count() == 0)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "No factions equal or contain \"" + targetFaction + "\".");
                                    else if (inviterFactions.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many faction names contain \"" + targetFaction + "\".");
                                    else
                                    {
                                        factions[inviterFactions[0]].Add(senderClient.userID.ToString(), "normal");
                                        PlayerClient[] targetClients = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => factions[inviterFactions[0]].ContainsKey(pc.userID.ToString()));
                                        foreach (PlayerClient pc in targetClients)
                                        {
                                            Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + inviterFactions[0], senderClient.userName + " has joined the faction.");
                                        }
                                        addFactionData(inviterFactions[0], senderClient.userName, senderClient.userID.ToString(), "normal");
                                    }
                                }
                                else
                                {
                                    KeyValuePair<string, Dictionary<string, string>> inviterFaction = Array.Find(factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(latestFactionRequests[senderClient].userID.ToString()));
                                    inviterFaction.Value.Add(senderClient.userID.ToString(), "normal");
                                    PlayerClient[] targetClients = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => inviterFaction.Value.ContainsKey(pc.userID.ToString()));
                                    foreach (PlayerClient pc in targetClients)
                                    {
                                        Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + inviterFaction.Key, senderClient.userName + " has joined the faction.");
                                    }
                                    addFactionData(inviterFaction.Key, senderClient.userName, senderClient.userID.ToString(), "normal");
                                }
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "You do not have any faction invites.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You must leave your current faction first.");
                        break;
                    case "leave":
                        if (possibleFactions.Count() > 0)
                        {
                            string rank = possibleFactions[0].Value[senderClient.userID.ToString()];

                            if (rank != "owner")
                            {
                                PlayerClient[] targetClients = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                foreach (PlayerClient pc in targetClients)
                                {
                                    Broadcast.broadcastCustomTo(pc.netPlayer, possibleFactions[0].Key, senderClient.userName + " has left the faction.");
                                }
                                try
                                {
                                    remFactionData(possibleFactions[0].Key, senderClient.userName, possibleFactions[0].Value[senderClient.userID.ToString()]);
                                    possibleFactions[0].Value.Remove(senderClient.userID.ToString());
                                }
                                catch (Exception ex)
                                {
                                    Vars.conLog.Error(ex.ToString());
                                }
                            }
                            else
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You cannot leave your faction without disbanding or passing ownership.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        break;
                    case "admin":
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
                                PlayerClient[] possibleTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));

                                if (possibleTargets.Count() == 0)
                                    Broadcast.broadcastTo(senderClient.netPlayer, "No players equal or contain \"" + targetName + "\".");
                                else if (possibleTargets.Count() > 1)
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + targetName + "\".");
                                else
                                {
                                    PlayerClient targetClient = possibleTargets[0];

                                    remFactionData(possibleFactions[0].Key, targetClient.userName, possibleFactions[0].Value[targetClient.userID.ToString()]);
                                    possibleFactions[0].Value.Remove(targetClient.userID.ToString());
                                    possibleFactions[0].Value.Add(targetClient.userID.ToString(), "admin");
                                    addFactionData(possibleFactions[0].Key, targetClient.userName, targetClient.userID.ToString(), "admin");
                                    PlayerClient[] targetClients = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                    foreach (PlayerClient pc in targetClients)
                                    {
                                        Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, "Player \"" + targetClient.userName + "\" is now a faction admin.");
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
                                PlayerClient[] possibleTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));

                                if (possibleTargets.Count() == 0)
                                    Broadcast.broadcastTo(senderClient.netPlayer, "No players equal or contain \"" + targetName + "\".");
                                else if (possibleTargets.Count() > 1)
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + targetName + "\".");
                                else
                                {
                                    PlayerClient targetClient = possibleTargets[0];

                                    remFactionData(possibleFactions[0].Key, targetClient.userName, possibleFactions[0].Value[targetClient.userID.ToString()]);
                                    possibleFactions[0].Value.Remove(targetClient.userID.ToString());
                                    possibleFactions[0].Value.Add(targetClient.userID.ToString(), "normal");
                                    addFactionData(possibleFactions[0].Key, targetClient.userName, targetClient.userID.ToString(), "normal");
                                    PlayerClient[] targetClients = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                    foreach (PlayerClient pc in targetClients)
                                    {
                                        Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, "Player \"" + targetClient.userName + "\" is no longer a faction admin.");
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
                                PlayerClient[] possibleTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));

                                if (possibleTargets.Count() == 0)
                                    Broadcast.broadcastTo(senderClient.netPlayer, "No players equal or contain \"" + targetName + "\".");
                                else if (possibleTargets.Count() > 1)
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + targetName + "\".");
                                else
                                {
                                    PlayerClient targetClient = possibleTargets[0];

                                    remFactionData(possibleFactions[0].Key, senderClient.userName, possibleFactions[0].Value[senderClient.userID.ToString()]);
                                    possibleFactions[0].Value.Remove(senderClient.userID.ToString());
                                    possibleFactions[0].Value.Add(senderClient.userID.ToString(), "admin");
                                    addFactionData(possibleFactions[0].Key, senderClient.userName, senderClient.userID.ToString(), "admin");
                                    remFactionData(possibleFactions[0].Key, targetClient.userName, possibleFactions[0].Value[targetClient.userID.ToString()]);
                                    possibleFactions[0].Value.Remove(targetClient.userID.ToString());
                                    possibleFactions[0].Value.Add(targetClient.userID.ToString(), "owner");
                                    addFactionData(possibleFactions[0].Key, targetClient.userName, targetClient.userID.ToString(), "owner");
                                    PlayerClient[] targetClients = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                    foreach (PlayerClient pc in targetClients)
                                    {
                                        Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, senderClient.userName + " now owns the faction.");
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
                        break;
                    case "info":
                        break;
                    case "help":
                        break;
                    default:
                        break;
                }
            }
        }

        public static void clientSpeak(VoiceCom voiceCom, int setupData, byte[] data)
        {
            try
            {
                PlayerClient client;
                if (((voice.distance > 0f) && PlayerClient.Find(voiceCom.networkViewOwner, out client)) && client.hasLastKnownPosition)
                {
                    float num = inGlobalV.Contains(client.userID.ToString()) ? (1000000000f * 1000000000f) : (voice.distance * voice.distance);
                    Vector3 lastKnownPosition = client.lastKnownPosition;
                    int num3 = 0;
                    try
                    {
                        foreach (PlayerClient client2 in PlayerClient.All)
                        {
                            if (((client2 != null) && client2.hasLastKnownPosition) && (client2 != client))
                            {
                                Vector3 vector;
                                vector.x = client2.lastKnownPosition.x - lastKnownPosition.x;
                                vector.y = client2.lastKnownPosition.y - lastKnownPosition.y;
                                vector.z = client2.lastKnownPosition.z - lastKnownPosition.z;
                                float num2 = ((vector.x * vector.x) + (vector.y * vector.y)) + (vector.z * vector.z);
                                if (num2 <= num)
                                {
                                    num3++;
                                    voiceCom.playerList.Add(client2.netPlayer);
                                }
                            }
                        }
                        if (num3 > 0)
                        {
                            object[] args = new object[] { voice.distance, setupData, data };
                            voiceCom.networkView.RPC("VoiceCom:voiceplay", voiceCom.playerList, args);
                        }
                    }
                    finally
                    {
                        if (num3 > 0)
                        {
                            voiceCom.playerList.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                conLog.Error(ex.ToString());
            }
        }

        public static void removerTool(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();

                switch (mode)
                {
                    case "on":
                        if (!destroyerList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "Remover tool activated. Hit AI or structures to delete them.");
                            destroyerList.Add(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You already have the remover tool activated.");
                        break;
                    case "off":
                        if (destroyerList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "Remover tool deactivated.");
                            destroyerList.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You do not have the remover tool activated.");
                        break;
                }
            }
        }

        public static bool isPlayer(IDMain idMain)
        {
            if (idMain is Character)
            {
                Character character = idMain as Character;
                Controller player = character.playerControlledController;
                if (player != null)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public static void StructureUCH(IDMain idMain, StructureComponent structureComponent)
        {
            float health = idMain.GetComponent<TakeDamage>().health;
            if (health != structureComponent.oldHealth)
            {
                NetEntityID entID = NetEntityID.Get((MonoBehaviour)structureComponent);
                NetCull.RemoveRPCsByName(entID, "ClientHealthUpdate");
                if (!beingDestroyed.Contains(idMain.gameObject))
                    NetCull.RPC<float>(entID, "ClientHealthUpdate", uLink.RPCMode.OthersBuffered, health);
                else
                    beingDestroyed.Remove(idMain.gameObject);
            }
        }

        public static void DeployableUCH(IDMain idMain, DeployableObject deployableObject)
        {
            TakeDamage component = idMain.GetComponent<TakeDamage>();
            if (component != null)
            {
                float health = component.health;
                NetEntityID entID = NetEntityID.Get((MonoBehaviour) deployableObject);
                NetCull.RemoveRPCsByName(entID, "ClientHealthUpdate");
                if (!beingDestroyed.Contains(idMain.gameObject))
                    NetCull.RPC<float>(entID, "ClientHealthUpdate", uLink.RPCMode.OthersBuffered, health);
                else
                    beingDestroyed.Remove(idMain.gameObject);
            }

        }

        public static void OnHurt(TakeDamage takeDamage, ref DamageEvent damage)
        {
            if (damage.extraData != null)
            {
                if (damage.victim.idMain is Character)
                {
                    if (isPlayer(damage.victim.idMain))
                    {
                        OnHumanHurt(ref damage);
                    }
                    else
                    {
                        OnAIHurt(ref damage);
                    }
                }
                else
                {
                    if (damage.victim.idMain is StructureComponent || damage.victim.idMain is DeployableObject)
                    {
                        if (isPlayer(damage.attacker.idMain))
                        {
                            if (destroyerList.Contains(damage.attacker.userID.ToString()))
                            {
                                beingDestroyed.Add(damage.victim.idMain.gameObject);
                                NetCull.Destroy(damage.victim.idMain.gameObject);
                            }
                        }
                    }
                }
            }
        }
        public static void OnZombieHurt(Controller controller, ZombieController zombieController, DamageEvent damage)
        {
            if (!zombieController.HasTarget())
            {
                Character newTarg = damage.attacker.character;
                if (newTarg != null)
                {
                    if (newTarg != zombieController.attackTarget)
                    {
                        zombieController.SetTarget(newTarg);
                        zombieController.lastAttackedMePosition = newTarg.origin;
                    }
                    zombieController.targetAttackedMe = true;
                    if (zombieController.wasRoaming)
                    {
                        zombieController.RoamFrameEnd();
                        NavMeshAgent agent = controller.agent;
                        if (agent != null)
                        {
                            if (!beingDestroyed.Contains(damage.victim.idMain.gameObject))
                                agent.ResetPath();
                            else
                                beingDestroyed.Remove(damage.victim.idMain.gameObject);
                        }
                        zombieController.wasRoaming = false;
                        zombieController.currentlyRoaming = false;
                    }
                }
            }
        }

        public static void OnAIHurt(ref DamageEvent damage)
        {
            if (isPlayer(damage.attacker.idMain))
            {
                PlayerClient attackerClient = damage.attacker.client;

                if (destroyerList.Contains(attackerClient.userID.ToString()))
                {
                    beingDestroyed.Add(damage.victim.idMain.gameObject);

                    if (damage.victim.idMain.gameObject.name.Contains("Zombie"))
                        NetCull.Destroy(damage.victim.idMain.gameObject);
                    else
                    {
                        damage.amount = 1000f;
                        damage.status = LifeStatus.WasKilled;
                    }
                }
            }
        }

        public static void OnAIKilled(BasicWildLifeAI BWAI, DamageEvent damage)
        {
            BWAI.ExitCurrentState();
            BWAI.EnterState_Dead();
            Facepunch.NetworkView networkView = BWAI.networkView;
            NetCull.RemoveRPCsByName(networkView, "GetNetworkUpdate");
            object[] args = new object[] { BWAI._myTransform.position, damage.attacker.networkViewID };
            networkView.RPC("ClientDeath", uLink.RPCMode.Others, args);
            if (beingDestroyed.Contains(damage.victim.idMain.gameObject))
                NetCull.Destroy(damage.victim.idMain.gameObject);
            else
                BWAI.Invoke("DelayedDestroy", 90f);
            WildlifeManager.RemoveWildlifeInstance(BWAI);

        }

        public static void OnHumanHurt(ref DamageEvent damage)
        { 
            PlayerClient victim = damage.victim.client;
            PlayerClient attacker = null;
            
            string victimFaction = "Neutral";
            string attackerFaction = "";

            KeyValuePair<string, Dictionary<string, string>>[] possibleFactions = Array.FindAll(factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(victim.userID.ToString()));
            if (possibleFactions.Count() > 0)
            {
                victimFaction = possibleFactions[0].Key;
            }

            if (isPlayer(damage.attacker.idMain))
            {
                attacker = damage.attacker.client;

                possibleFactions = Array.FindAll(factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(attacker.userID.ToString()));
                if (possibleFactions.Count() > 0)
                    attackerFaction = possibleFactions[0].Key;
            }

            if (victimFaction == attackerFaction && damage.attacker.IsDifferentPlayer(victim))
            {
                if (damage.damageTypes == DamageTypeFlags.damage_bullet)
                {
                    Broadcast.sideNoticeTo(attacker.netPlayer, "You shot " + victim.userName);
                    Broadcast.sideNoticeTo(victim.netPlayer, attacker.userName + " shot you");
                }
                else
                {
                    Broadcast.sideNoticeTo(attacker.netPlayer, "You hit " + victim.userName);
                    Broadcast.sideNoticeTo(victim.netPlayer, attacker.userName + " hit you");
                }
                damage.amount = 0f;
                damage.status = LifeStatus.IsAlive;
            }
            else
            {
                if (isTeleporting.Contains(victim))
                {
                    wasHit.Add(victim);
                }
                if (isAccepting.Contains(victim))
                {
                    wasHit.Add(victim);
                }
            }
        }

        public static bool belongsTo(ulong userID, ulong ownerID)
        {
            if (completeDoorAccess.Contains(userID.ToString()))
                return true;

            if (!Vars.sharingData.ContainsKey(ownerID.ToString()))
            {
                if (userID == ownerID)
                    return true;
            }
            else
            {
                string shareData = Vars.sharingData[ownerID.ToString()];
                if (shareData.Contains(":"))
                {
                    if (shareData.Split(':').Contains(userID.ToString()) || ownerID == userID)
                        return true;
                }
                else
                {
                    if (shareData.Contains(userID.ToString()) || ownerID == userID)
                        return true;
                }
            }
            return false;
        }

        public static void OnUserConnected(NetUser user)
        {
            try
            {
                string steamUID = user.userID.ToString();
                if (!Vars.firstPlayerJoined)
                {
                    Vars.firstPlayerJoined = true;
                    Vars.loadEnvironment();
                    truth.punish = false;
                    truth.threshold = 999999999;
                }
                if (Vars.currentBans.ContainsValue(steamUID))
                {
                    Vars.kickPlayer(user, Vars.currentBanReasons[steamUID], true);
                }
                else
                {
                    if (!Vars.teleportRequests.ContainsKey(user.playerClient))
                    {
                        Vars.teleportRequests.Add(user.playerClient, new Dictionary<PlayerClient, TimerPlus>());
                        Vars.latestRequests.Add(user.playerClient, null);
                    }

                    if (Vars.useSteamGroup && Vars.groupMembers.Contains(steamUID))
                    {
                        Vars.conLog.Info("Player " + user.displayName + " (" + steamUID + ") has connected through steam group \"" + Vars.steamGroup + "\".");
                        if (Vars.motdList.Keys.Contains("Join"))
                        {
                            foreach (string s in Vars.motdList["Join"])
                            {
                                Broadcast.broadcastTo(user.networkPlayer, s);
                            }
                        }
                        switch (Vars.defaultChat)
                        {
                            case "global":
                                if (!Vars.inGlobal.Contains(user.userID.ToString()))
                                    Vars.inGlobal.Add(user.userID.ToString());
                                break;
                            case "direct":
                                if (!Vars.inDirect.Contains(user.userID.ToString()))
                                    Vars.inDirect.Add(user.userID.ToString());
                                break;
                        }
                        if (!Vars.inDirectV.Contains(user.userID.ToString()))
                            Vars.inDirectV.Add(user.userID.ToString());
                        string joinMessage = "";
                        if (Vars.enableJoin)
                        {
                            joinMessage = Vars.joinMessage.Replace("$USER$", Vars.filterFullNames(user.displayName, steamUID));
                            Broadcast.broadcastJoinLeave(joinMessage);
                        }
                        Vars.conLog.Chat("<BROADCAST ALL> " + Vars.botName + ": " + joinMessage + " (" + steamUID + ")");
                    }
                    else
                    {
                        if (!Vars.whitelist.Contains(steamUID) && Vars.enableWhitelist)
                        {
                            Vars.whitelistKick(user, Vars.whitelistKickJoin);
                        }
                        else
                        {
                            if (Vars.motdList.Keys.Contains("Join"))
                            {
                                foreach (string s in Vars.motdList["Join"])
                                {
                                    Broadcast.broadcastTo(user.networkPlayer, s);
                                }
                            }
                            switch (Vars.defaultChat)
                            {
                                case "global":
                                    if (!Vars.inGlobal.Contains(user.userID.ToString()))
                                        Vars.inGlobal.Add(user.userID.ToString());
                                    break;
                                case "direct":
                                    if (!Vars.inDirect.Contains(user.userID.ToString()))
                                        Vars.inDirect.Add(user.userID.ToString());
                                    break;
                            }
                            if (!Vars.inDirectV.Contains(user.userID.ToString()))
                                Vars.inDirectV.Add(user.userID.ToString());
                            string joinMessage = "";
                            if (Vars.enableJoin)
                            {
                                joinMessage = Vars.joinMessage.Replace("$USER$", Vars.filterFullNames(user.displayName, steamUID));
                                Broadcast.broadcastJoinLeave(joinMessage);
                            }
                            Vars.conLog.Chat("<BROADCAST ALL> " + Vars.botName + ": " + joinMessage + " (" + steamUID + ")");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Vars.conLog.Error(ex.ToString());
            }
        }

        public static void OnUserDisconnected(NetUser user)
        {
            try
            {
                PlayerClient playerClient = Array.Find(Vars.teleportRequests.ToArray(), (KeyValuePair<PlayerClient, Dictionary<PlayerClient, TimerPlus>> kv) => kv.Key.netPlayer == user.networkPlayer).Key;

                if (latestPM.ContainsKey(playerClient))
                    latestPM.Remove(playerClient);

                string leaveMessage = "";
                if (Vars.enableLeave && !Vars.kickQueue.Contains(playerClient.userName))
                {
                    leaveMessage = Vars.leaveMessage.Replace("$USER$", Vars.filterTags(playerClient.userName));
                    Broadcast.broadcastJoinLeave(leaveMessage);
                    Vars.conLog.Chat("<BROADCAST ALL> " + Vars.botName + ": " + leaveMessage);
                }
                else
                    Vars.kickQueue.Remove(playerClient.userName);

                Vars.teleportRequests.Remove(playerClient);
            }
            catch (Exception ex)
            {
                //Vars.conLog.Error(ex.ToString());
            }
        }

        public static void save(PlayerClient senderClient)
        {
            try
            {
                ConsoleSystem.Run("save.all");
                Broadcast.broadcastTo(senderClient.netPlayer, "All data has been saved.");
            } catch (Exception ex)
            {
                Broadcast.broadcastTo(senderClient.netPlayer, ex.ToString());
            }
        }

        public static void stopServer()
        {
            try
            {
                ConsoleSystem.Run("save.all");
                Broadcast.broadcastAll("Server shutting down...");
                Rust.Steam.Server.Shutdown();
                Application.Quit();
            } catch (Exception ex)
            {
                Vars.conLog.Error(ex.ToString());
            }
        }

        public static void listKits(string rank, PlayerClient senderClient)
        {
            List<string> kits = new List<string>();
            List<string> otherKits = new List<string>();
            if (rank == Vars.defaultRank)
            {
                foreach (string s in Vars.unassignedKits)
                {
                    otherKits.Add(s);
                }
            }
            else
            {
                foreach (string s in Vars.kitsForRanks[rank])
                {
                    otherKits.Add(s);
                }
            }

            List<string> otherKits2 = new List<string>();
            while (otherKits.Count > 0)
            {
                int curIndex = 0;
                kits.Clear();
                otherKits2.Clear();
                foreach (string s in otherKits)
                {
                    curIndex++;
                    if (curIndex < 9)
                    {
                        kits.Add(s);
                        otherKits2.Add(s);
                    }
                    else
                        break;
                }
                foreach (string s in otherKits2)
                {
                    otherKits.Remove(s);
                }
                Broadcast.broadcastTo(senderClient.netPlayer, string.Join(", ", kits.ToArray()), true);
            }
        }

        public static void listCommands(string rank, PlayerClient senderClient)
        {
            List<string> commands1 = new List<string>();
            List<string> otherCommands = new List<string>();
            foreach (string s in Vars.enabledCommands[rank])
            {
                otherCommands.Add(s);
            }

            List<string> otherCommands1 = new List<string>();
            while (otherCommands.Count > 0)
            {
                int curIndex = 0;
                commands1.Clear();
                otherCommands1.Clear();
                foreach (string s in otherCommands)
                {
                    curIndex++;
                    if (curIndex < 9)
                    {
                        commands1.Add(s);
                        otherCommands1.Add(s);
                    }
                    else
                        break;
                }
                foreach (string s in otherCommands1)
                {
                    otherCommands.Remove(s);
                }
                Broadcast.broadcastTo(senderClient.netPlayer, string.Join(" ", commands1.ToArray()), true);
            }
        }

        public static void showHistory(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                try
                {
                    int lineAmount = Convert.ToInt16(args[1]);
                    if (lineAmount > 50)
                        lineAmount = 50;

                    int curIndex = 0;
                    int goalIndex = historyGlobal.Count - lineAmount;
                    foreach (string s in historyGlobal)
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

        public static void say(string[] args)
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

                Broadcast.broadcastAll(string.Join(" ", messageList.ToArray()));
            }
        }

        public static void saypop(string[] args)
        {
            if (args.Count() > 1)
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

                    Broadcast.noticeAll(args[1], string.Join(" ", messageList.ToArray()));
                }
                else
                {
                    List<string> messageList = new List<string>();
                    int curIndex = 0;
                    foreach (string s in args)
                    {
                        if (curIndex > 0)
                            messageList.Add(s);
                        curIndex++;
                    }

                    Broadcast.noticeAll("!", string.Join(" ", messageList.ToArray()));
                }
            }
        }

        public static void kickAll(PlayerClient senderClient)
        {
            foreach (PlayerClient targetClient in PlayerClient.All)
            {
                if (targetClient != senderClient)
                    kickPlayer(targetClient.netUser, "All users were kicked.", false);
            }
        }

        public static void teleportDeny(PlayerClient senderClient, string[] args)
        {
            try
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
                    if (playerName.ToLower() == "all")
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "All teleport requests denied.");
                        foreach (KeyValuePair<PlayerClient, TimerPlus> kv in teleportRequests[senderClient])
                        {
                            Broadcast.broadcastTo(kv.Key.netPlayer, senderClient.userName + " denied your teleport request.");
                            teleportRequests[senderClient].Remove(kv.Key);
                        }
                    }
                    else
                    {
                        PlayerClient[] possibleTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                        if (possibleTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                        else if (possibleTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                        else
                        {
                            PlayerClient targetClient = possibleTargets[0];

                            if (teleportRequests[senderClient].ContainsKey(targetClient))
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, "Denied " + targetClient.userName + "'s teleport request.");
                                Broadcast.broadcastTo(targetClient.netPlayer, senderClient.userName + " denied your teleport request.");
                                teleportRequests[senderClient].Remove(targetClient);
                            }
                            else
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, "You do not have a teleport request from " + targetClient.userName + ".");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Broadcast.broadcastTo(senderClient.netPlayer, ex.ToString());
            }
        }

        public static void teleportAccept(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
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

                string playerName = string.Join(" ", playerNameList.ToArray());

                PlayerClient[] possibleTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                if (possibleTargets.Count() == 0)
                    Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                else if (possibleTargets.Count() > 1)
                    Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                else
                {
                    PlayerClient targetClient = possibleTargets[0];

                    if (teleportRequests[senderClient].ContainsKey(targetClient))
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting " + targetClient.userName + " in 10 seconds...");
                        Broadcast.broadcastTo(targetClient.netPlayer, "Teleporting to " + senderClient.userName + " in 10 seconds. Do not move...");
                        Thread t = new Thread(() => teleporting(targetClient, senderClient));
                        t.Start();
                        isTeleporting.Add(targetClient);
                        isAccepting.Add(senderClient);
                    }
                    else
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "You do not have a teleport request from " + targetClient.userName + ".");
                    }
                }
            }
            else
            {
                if (latestRequests[senderClient] != null)
                {
                    PlayerClient targetClient = latestRequests[senderClient];
                    Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting " + targetClient.userName + " in 10 seconds...");
                    Broadcast.broadcastTo(targetClient.netPlayer, "Teleporting to " + senderClient.userName + " in 10 seconds. Do not move...");
                    Thread t = new Thread(() => teleporting(targetClient, senderClient));
                    t.Start();
                    isTeleporting.Add(targetClient);
                }
                else
                    Broadcast.broadcastTo(senderClient.netPlayer, "You have no teleport requests to accept!");
            }
        }

        public static void teleporting(PlayerClient targetClient, PlayerClient senderClient)
        {
            Character targetChar;
            Character.FindByUser(targetClient.userID, out targetChar);
            Vector3 oldPos = targetChar.transform.position;
            bool b = true;
            int timeElapsed = 0;
            RustServerManagement serverManagement = RustServerManagement.Get();
            while (b)
            {
                if (wasHit.Contains(targetClient))
                {
                    teleportRequests[senderClient].Remove(targetClient);
                    latestRequests[senderClient] = null;
                    isTeleporting.Remove(targetClient);
                    isAccepting.Remove(senderClient);
                    wasHit.Remove(targetClient);
                    Broadcast.broadcastTo(senderClient.netPlayer, "Teleport request from " + targetClient.userName + " was interrupted due to damage.");
                    Broadcast.broadcastTo(targetClient.netPlayer, "Teleport request to " + senderClient.userName + " was interrupted due to damage.");
                    b = false;
                    break;
                }
                if (wasHit.Contains(senderClient))
                {
                    teleportRequests[senderClient].Remove(targetClient);
                    latestRequests[senderClient] = null;
                    isTeleporting.Remove(targetClient);
                    isAccepting.Remove(senderClient);
                    wasHit.Remove(senderClient);
                    Broadcast.broadcastTo(senderClient.netPlayer, "Teleport request from " + targetClient.userName + " was interrupted due to damage.");
                    Broadcast.broadcastTo(targetClient.netPlayer, "Teleport request to " + senderClient.userName + " was interrupted due to damage.");
                    b = false;
                    break;
                }
                if (timeElapsed >= 10)
                {
                    serverManagement.TeleportPlayerToPlayer(targetClient.netPlayer, senderClient.netPlayer);
                    teleportRequests[senderClient].Remove(targetClient);
                    latestRequests[senderClient] = null;
                    isTeleporting.Remove(targetClient);
                    isAccepting.Remove(senderClient);
                    b = false;
                    break;
                }
                Vector3 newPos = targetChar.transform.position;
                if (Vector3.Distance(oldPos, newPos) > 3)
                {
                    Broadcast.broadcastTo(senderClient.netPlayer, targetClient.userName + " moved. Teleportation canceled.");
                    Broadcast.broadcastTo(targetClient.netPlayer, "Teleportation canceled.");
                    teleportRequests[senderClient].Remove(targetClient);
                    latestRequests[senderClient] = null;
                    isTeleporting.Remove(targetClient);
                    isAccepting.Remove(senderClient);
                    b = false;
                    break;
                }
                Thread.Sleep(1000);
                timeElapsed++;
            }
        }

        public static void teleportRequest(PlayerClient senderClient, string[] args)
        {
            if (teleportRequestOn)
            {
                if (args.Count() > 1)
                {
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

                    string playerName = string.Join(" ", playerNameList.ToArray());

                    PlayerClient[] possibleTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                    if (possibleTargets.Count() == 0)
                        Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                    else if (possibleTargets.Count() > 1)
                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                    else
                    {
                        PlayerClient targetClient = possibleTargets[0];
                        if (!teleportRequests[targetClient].ContainsKey(senderClient))
                        {
                            TimerPlus t = new TimerPlus();
                            t.AutoReset = false;
                            t.Interval = 30000;
                            t.Elapsed += (sender, e) => requestTimeout(targetClient, senderClient);
                            t.Start();
                            teleportRequests[targetClient].Add(senderClient, t);
                            latestRequests[targetClient] = senderClient;
                            Broadcast.broadcastTo(senderClient.netPlayer, "Teleport request sent to " + targetClient.userName + ".");
                            Broadcast.broadcastTo(targetClient.netPlayer, senderClient.userName + " requested to teleport to you. Type /tpaccept <name> or /tpdeny <name>.");
                        }
                        else
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You already sent a teleport request to " + targetClient.userName + ".");
                        }
                    }
                }
            }
            else
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "Teleport requesting is disabled.");
            }
        }

        public static void requestTimeout(PlayerClient targetClient, PlayerClient senderClient)
        {
            if (teleportRequests[targetClient].ContainsKey(senderClient))
            {
                teleportRequests[targetClient].Remove(senderClient);
                Broadcast.broadcastTo(senderClient.netPlayer, "Request to teleport to " + targetClient.userName + " has timed out.");
            }
        }

        public static void teleportPos(PlayerClient senderClient, string[] args)
        {
            if (args.Count() == 4)
            {
                float xpos = -1f;
                float ypos = -1f;
                float zpos = -1f;
                try
                {
                    xpos = float.Parse(args[1]);
                    ypos = float.Parse(args[2]);
                    zpos = float.Parse(args[3]);
                }
                catch (Exception ex)
                {
                    Broadcast.broadcastTo(senderClient.netPlayer, "X, Y, and Z must be numerical!");
                }

                if (xpos != -1 && ypos != -1 && zpos != -1)
                {
                    RustServerManagement serverManagement = RustServerManagement.Get();
                    serverManagement.TeleportPlayerToWorld(senderClient.netPlayer, new Vector3(xpos, ypos, zpos));
                }
            }
            else
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "Improper syntax! Syntax: /tppos x y z");
            }
        }

        public static void teleport(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                RustServerManagement serverManagement = RustServerManagement.Get();
                if (args.Count() == 2)
                {
                    List<string> playerNameList = new List<string>();
                    int curIndex = 0;
                    foreach (string s in args)
                    {
                        if (curIndex > 0)
                            playerNameList.Add(s);
                        curIndex++;
                    }
                    string playerName = string.Join(" ", playerNameList.ToArray());
                    PlayerClient[] possibleTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                    if (possibleTargets.Count() == 0)
                        Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                    else if (possibleTargets.Count() > 1)
                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                    else
                    {
                        PlayerClient targetClient = possibleTargets[0];

                        serverManagement.TeleportPlayerToPlayer(senderClient.netPlayer, targetClient.netPlayer);
                        Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting to " + targetClient.userName + "...");
                    }
                }
                if (args.Count() > 2)
                {
                    List<string> playerNameList = new List<string>();
                    int lastIndex = -1;
                    if (args[1].Contains("\""))
                    {
                        bool hadQuote = false;
                        foreach (string s in args)
                        {
                            lastIndex++;
                            if (s.StartsWith("\"")) hadQuote = true;
                            if (hadQuote)
                            {
                                playerNameList.Add(s.Replace("\"", ""));
                            }
                            if (s.EndsWith("\""))
                            {
                                hadQuote = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        playerNameList.Add(args[1]);
                        lastIndex = 1;
                    }
                    string playerName = string.Join(" ", playerNameList.ToArray());
                    PlayerClient[] possibleTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                    if (possibleTargets.Count() == 0)
                        Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                    else if (possibleTargets.Count() > 1)
                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                    else
                    {
                        PlayerClient targetClient = possibleTargets[0];
                        playerNameList.Clear();
                        if (args[lastIndex + 1].Contains("\""))
                        {
                            bool hadQuote = false;
                            int curIndex = 0;
                            foreach (string s in args)
                            {
                                if (curIndex > lastIndex)
                                {
                                    if (s.StartsWith("\"")) hadQuote = true;
                                    if (hadQuote)
                                    {
                                        playerNameList.Add(s.Replace("\"", ""));
                                    }
                                    if (s.EndsWith("\""))
                                    {
                                        hadQuote = false;
                                        break;
                                    }
                                }
                                curIndex++;
                            }
                        }
                        else
                        {
                            playerNameList.Add(args[lastIndex + 1]);
                        }
                        playerName = string.Join(" ", playerNameList.ToArray());
                        PlayerClient[] otherTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                        if (otherTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                        else if (otherTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                        else
                        {
                            PlayerClient targetClient2 = otherTargets[0];

                            serverManagement.TeleportPlayerToPlayer(targetClient.netPlayer, targetClient2.netPlayer);
                            Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting \"" + targetClient.userName + "\" to \"" + targetClient2.userName + "\"...");
                        }
                    }
                }
            }
        }

        public static void mute(PlayerClient senderClient, string[] args, bool mode)
        {
            if (args.Count() > 1)
            {
                int time;
                if (mode && int.TryParse(args.Last().Remove(args.Last().Length - 1), out time) && (args.Last().EndsWith("s") || args.Last().EndsWith("m") || args.Last().EndsWith("h")))
                {
                    string timeString = args.Last().Remove(args.Last().Length - 1);
                    string timeMode = args.Last().Substring(args.Last().Length - 1, 1);
                    time *= 1000;
                    if (timeMode == "s")
                        timeMode = " second(s).";
                    if (timeMode == "m")
                    {
                        timeMode = " minute(s).";
                        time *= 60;
                    }
                    if (timeMode == "h")
                    {
                        timeMode = " hour(s).";
                        time *= 3600;
                    }

                    List<string> playerNameList = new List<string>();
                    int curIndex = 0;
                    foreach (string s in args)
                    {
                        if (curIndex > 0 && curIndex < args.Count() - 1)
                            playerNameList.Add(s);
                        curIndex++;
                    }
                    string playerName = string.Join(" ", playerNameList.ToArray());
                    PlayerClient[] possibleTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                    if (possibleTargets.Count() == 0)
                        Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                    else if (possibleTargets.Count() > 1)
                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                    else
                    {
                        PlayerClient targetClient = possibleTargets[0];
                        if (!mutedUsers.Contains(targetClient.userID.ToString()))
                        {
                            Broadcast.broadcastAll("Player \"" + playerName + "\" has been muted on global chat for " + timeString + timeMode);
                            mutedUsers.Add(targetClient.userID.ToString());
                            TimerPlus tp = new TimerPlus();
                            tp.AutoReset = false;
                            tp.Interval = time;
                            tp.Elapsed += (sender, e) => unmuteElapsed(sender, e, targetClient.userID.ToString());
                            tp.Start();
                            muteTimes.Add(targetClient.userID.ToString(), tp);
                        }
                        else
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, targetClient.userName + " is already muted for " + timeString + timeMode);
                        }
                    }
                }
                else
                {
                    List<string> playerNameList = new List<string>();
                    int curIndex = 0;
                    foreach (string s in args)
                    {
                        if (curIndex > 0)
                            playerNameList.Add(s);
                        curIndex++;
                    }
                    string playerName = string.Join(" ", playerNameList.ToArray());
                    PlayerClient[] possibleTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                    if (possibleTargets.Count() == 0)
                        Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                    else if (possibleTargets.Count() > 1)
                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                    else
                    {
                        PlayerClient targetClient = possibleTargets[0];
                        if (mode)
                        {
                            if (!mutedUsers.Contains(targetClient.userID.ToString()))
                            {
                                Broadcast.broadcastAll("Player \"" + playerName + "\" has been muted on global chat.");
                                mutedUsers.Add(targetClient.userID.ToString());
                            }
                            else
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, targetClient.userName + " is already muted.");
                            }
                        }
                        else
                        {
                            if (mutedUsers.Contains(targetClient.userID.ToString()))
                            {
                                Broadcast.broadcastAll("Player \"" + playerName + "\" has been unmuted on global chat.");
                                mutedUsers.Remove(targetClient.userID.ToString());
                            }
                            else
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, targetClient.userName + " is not muted.");
                            }
                        }
                    }
                }
            }
        }

        static void unmuteElapsed(object sender, ElapsedEventArgs e, string UID)
        {
            mutedUsers.Remove(UID);
            muteTimes.Remove(UID);
        }

        public static void whitelistCheck(PlayerClient senderClient)
        {
            string UID = senderClient.userID.ToString();
            if (useSteamGroup)
            {
                if (groupMembers.Contains(UID))
                    Broadcast.broadcastTo(senderClient.netPlayer, whitelistCheckGood);
                else
                    Broadcast.broadcastTo(senderClient.netPlayer, whitelistCheckBad);
            }
            else
            {
                if (whitelist.Contains(UID))
                    Broadcast.broadcastTo(senderClient.netPlayer, whitelistCheckGood);
                else
                    Broadcast.broadcastTo(senderClient.netPlayer, whitelistCheckBad);
            }
        }

        public static void whitelistPlayer(uLink.NetworkPlayer sender, string[] args)
        {
            if (args.Count() > 1)
            {
                string action = args[1];
                string UID = "";

                if (args.Count() > 2)
                {
                    UID = args[2];
                }

                PlayerClient[] possibleTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userID.ToString() == UID);
                string targetName = "Unknown Player";
                if (possibleTargets.Count() > 0)
                    targetName = possibleTargets[0].userName;

                switch (action)
                {
                    case "add":
                        whitelist.Add(UID);
                        Broadcast.broadcastTo(sender, "UID " + UID + " added to the whitelist.");

                        if (useMySQL)
                        {
                            //try
                            //{
                            //    Vars.mysqlConnection.Open();
                            //    Vars.whitelist.Clear();

                            //    MySqlCommand add = new MySqlCommand("INSERT INTO whitelist (name, uid) VALUES('" + targetName + "', '" + UID + "')", Vars.mysqlConnection);
                            //    add.ExecuteNonQuery();

                            //    Vars.mysqlConnection.Close();
                            //}
                            //catch (MySqlException ex)
                            //{
                            //    switch (ex.Number)
                            //    {
                            //        case 0:
                            //            Vars.conLog.Error("Unable to connect to MySQL server!");
                            //            break;
                            //        case 1045:
                            //            Vars.conLog.Error("Invalid credentials when connecting to the MySQL server!");
                            //            break;
                            //    }
                            //}
                        }
                        else
                        {
                            if (File.Exists(whiteListFile))
                            {
                                using (StreamWriter sw = new StreamWriter(whiteListFile))
                                {
                                    foreach (string s in whitelist)
                                    {
                                        sw.WriteLine(s + " # " + targetName);
                                    }
                                }
                            }
                        }
                        break;
                    case "rem":
                        whitelist.Remove(UID);
                        Broadcast.broadcastTo(sender, "UID " + UID + " removed from the whitelist.");
                        if (useMySQL)
                        {
                            //try
                            //{
                            //    Vars.mysqlConnection.Open();
                            //    Vars.whitelist.Clear();

                            //    MySqlCommand add = new MySqlCommand("DELETE FROM whitelist WHERE uid='" + UID + "'", Vars.mysqlConnection);
                            //    add.ExecuteNonQuery();

                            //    Vars.mysqlConnection.Close();
                            //}
                            //catch (MySqlException ex)
                            //{
                            //    switch (ex.Number)
                            //    {
                            //        case 0:
                            //            Vars.conLog.Error("Unable to connect to MySQL server!");
                            //            break;
                            //        case 1045:
                            //            Vars.conLog.Error("Invalid credentials when connecting to the MySQL server!");
                            //            break;
                            //    }
                            //}
                        }
                        else
                        {
                            if (File.Exists(whiteListFile))
                            {
                                using (StreamWriter sw = new StreamWriter(whiteListFile))
                                {
                                    foreach (string s in whitelist)
                                    {
                                        sw.WriteLine(s + " # " + targetName);
                                    }
                                }
                            }
                        }
                        break;
                    case "on":
                        enableWhitelist = true;
                        Broadcast.broadcastTo(sender, "Whitelist enabled! Nonwhitelisted users can be kicked with /whitelist kick.");
                        break;
                    case "off":
                        enableWhitelist = false;
                        Broadcast.broadcastTo(sender, "Whitelist disabled!");
                        break;
                    case "kick":
                        if (enableWhitelist)
                        {
                            if (useSteamGroup)
                            {
                                PlayerClient[] targetUsers = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => !groupMembers.Contains(pc.userID.ToString()));
                                foreach (PlayerClient targetClient in targetUsers)
                                {
                                    whitelistKick(targetClient.netUser, whitelistKickCMD);
                                }
                            }
                            else
                            {
                                PlayerClient[] targetUsers = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => !whitelist.Contains(pc.userID.ToString()));
                                foreach (PlayerClient targetClient in targetUsers)
                                {
                                    whitelistKick(targetClient.netUser, whitelistKickCMD);
                                }
                            }
                        }
                        else
                            Broadcast.broadcastTo(sender, "Whitelist must be enabled to kick nonwhitelisted players.");
                        break;
                    default:
                        Broadcast.broadcastTo(sender, "Unknown Action!");
                        break;
                }
            }
        }

        public static void voiceChannels(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string action = args[1];
                string UID = senderClient.userID.ToString();
                bool hasPermission = false;
                switch (action)
                {
                    case "g":
                        if (hasPermission)
                        {
                            if (!inGlobalV.Contains(UID))
                            {
                                inGlobalV.Add(UID);
                                if (inDirectV.Contains(UID))
                                    inDirectV.Remove(UID);
                                Broadcast.broadcastTo(senderClient.netPlayer, "You are now talking in global voice chat.");
                            }
                            else
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, "You are already talking in global voice chat.");
                            }
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You do not have access to global voice chat.");
                        break;
                    case "global":
                        if (hasPermission)
                        {
                            if (!inGlobalV.Contains(UID))
                            {
                                inGlobalV.Add(UID);
                                if (inDirectV.Contains(UID))
                                    inDirectV.Remove(UID);
                                Broadcast.broadcastTo(senderClient.netPlayer, "You are now talking in global voice chat.");
                            }
                            else
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, "You are already talking in global voice chat.");
                            }
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You do not have access to global voice chat.");
                        break;
                    case "d":
                        if (!inDirectV.Contains(UID))
                        {
                            inDirectV.Add(UID);
                            if (inGlobalV.Contains(UID))
                                inGlobalV.Remove(UID);
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are now talking in direct voice chat.");
                        }
                        else
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are already talking in direct voice chat.");
                        }
                        break;
                    case "direct":
                        if (!inDirectV.Contains(UID))
                        {
                            inDirectV.Add(UID);
                            if (inGlobalV.Contains(UID))
                                inGlobalV.Remove(UID);
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are now talking in direct voice chat.");
                        }
                        else
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are already talking in direct voice chat.");
                        }
                        break;
                }
            }
        }

        public static void channels(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string action = args[1];
                string UID = senderClient.userID.ToString();
                switch (action)
                {
                    case "g":
                        if (!inGlobal.Contains(UID))
                        {
                            inGlobal.Add(UID);
                            if (inDirect.Contains(UID))
                                inDirect.Remove(UID);
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are now talking in global chat.");
                        }
                        else
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are already talking in global chat.");
                        }
                        break;
                    case "global":
                        if (!inGlobal.Contains(UID))
                        {
                            inGlobal.Add(UID);
                            if (inDirect.Contains(UID))
                                inDirect.Remove(UID);
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are now talking in global chat.");
                        }
                        else
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are already talking in global chat.");
                        }
                        break;
                    case "d":
                        if (!inDirect.Contains(UID))
                        {
                            inDirect.Add(UID);
                            if (inGlobal.Contains(UID))
                                inGlobal.Remove(UID);
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are now talking in direct chat.");
                        }
                        else
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are already talking in direct chat.");
                        }
                        break;
                    case "direct":
                        if (!inDirect.Contains(UID))
                        {
                            inDirect.Add(UID);
                            if (inGlobal.Contains(UID))
                                inGlobal.Remove(UID);
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are now talking in direct chat.");
                        }
                        else
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are already talking in direct chat.");
                        }
                        break;
                }
            }
        }

        public static string filterTags(string playerName)
        {
            foreach (KeyValuePair<string, string> kv in rankPrefixes)
            {
                playerName = playerName.Replace("[" + kv.Key + "]", "");
                playerName = playerName.Replace(kv.Value, "");
            }

            return playerName;
        }

        public static string filterFullNames(string playerName, string uid)
        {
            foreach (KeyValuePair<string, string> kv in rankPrefixes)
            {
                playerName = playerName.Replace("[" + kv.Key + "]", "");
                playerName = playerName.Replace(kv.Value, "");
            }

            foreach (KeyValuePair<string, List<string>> kv in rankList)
            {
                if (kv.Value.Contains(uid))
                {
                    playerName = "[" + kv.Key + "]" + " " + playerName;
                    break;
                }
            }

            return playerName;
        }

        public static void fakeJoin(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string fakeName = "";
                if (args[1].Contains("\""))
                {
                    bool hadQuote = false;
                    int lastIndex = -1;
                    List<string> playerNameList = new List<string>();
                    foreach (string s in args)
                    {
                        lastIndex++;
                        if (s.StartsWith("\"")) hadQuote = true;
                        if (hadQuote)
                        {
                            playerNameList.Add(s);
                        }
                        if (s.EndsWith("\""))
                        {
                            hadQuote = false;
                            break;
                        }
                    }

                    fakeName = string.Join(" ", playerNameList.ToArray()).Replace("\"", "");
                }
                else
                {
                    fakeName = args[1];
                }

                string joinMessage = Vars.joinMessage.Replace("$USER$", fakeName);
                Broadcast.broadcastAll(joinMessage);
            }
            else
            {
                string joinMessage = Vars.joinMessage.Replace("$USER$", senderClient.userName);
                Broadcast.broadcastAll(joinMessage);
            }
        }

        public static void sendToSurrounding(PlayerClient senderClient, string message)
        {
            Character senderChar;
            Character.FindByUser(senderClient.userID, out senderChar);

            Vector3 senderPos = senderChar.transform.position;
            foreach (PlayerClient targetClient in PlayerClient.All)
            {
                Character targetChar;
                Character.FindByUser(targetClient.userID, out targetChar);
                Vector3 targetPos = targetChar.transform.position;

                float distance = Vector3.Distance(senderPos, targetPos);
                if (distance < directDistance)
                {
                    Broadcast.broadcastCustomTo(targetClient.netPlayer, senderClient.userName, message);
                }
            }
        }

        public static void startTimer()
        {
            TimerPlus timer = new TimerPlus();
            timer.AutoReset = true;
            timer.Interval = refreshInterval;
            timer.Elapsed += ((sender, e) => grabGroupMembers());
            timer.Start();
        }

        public static void grabGroupMembers()
        {
            try
            {
                if (useSteamGroup)
                {
                    groupMembers.Clear();
                    string groupURL = "http://steamcommunity.com/groups/" + steamGroup + "/memberslistxml/?xml=1\\";
                    using (XmlTextReader reader = new XmlTextReader(groupURL))
                    {
                        string currentElement = "";
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "steamID64")
                            {
                                currentElement = "steamID64";
                            }
                            if (reader.NodeType == XmlNodeType.Text && currentElement == "steamID64")
                            {
                                currentElement = "";
                                groupMembers.Add(reader.Value);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Info(ex.ToString());
            }
        }

        public static StringBuilder cfgText()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("[Whitelist]");
            sb.AppendLine("# Enable whitelist upon server startup");
            sb.AppendLine("enableWhitelist=false");
            sb.AppendLine("# Use the MySQL settings defined below for whitelisting - BROKEN");
            sb.AppendLine("useMySQL=false");
            sb.AppendLine("# Use the Steam Group specified as the whitelist");
            sb.AppendLine("useSteamGroup=false");
            sb.AppendLine("steamGroupName=");
            sb.AppendLine("# Auto refresh the whitelist every time the interval elapses");
            sb.AppendLine("autoRefresh=true");
            sb.AppendLine("# Auto refresh interval in seconds");
            sb.AppendLine("refreshInterval=10");
            sb.AppendLine("# Instead of using whitelist as itself, users in whitelist will become [Members]");
            sb.AppendLine("useAsMembers=false");
            sb.AppendLine("# The message that is sent to unwhitelisted users upon /whitelist kick");
            sb.AppendLine("whitelistKickCMD=Whitelist was enabled and you were not whitelisted.");
            sb.AppendLine("# The message that is sent to unwhitelisted users upon join");
            sb.AppendLine("whitelistKickJoin=You are not whitelisted!");
            sb.AppendLine("# The message that is sent to sender upon /whitelist check IF whitelisted");
            sb.AppendLine("whitelistCheckGood=You are whitelisted!");
            sb.AppendLine("# The message that is sent to sender upon /whitelist check IF NOT whitelisted");
            sb.AppendLine("whitelistCheckBad=You are not whitelisted!");
            sb.AppendLine("");
            sb.AppendLine("[Airdrop]");
            sb.AppendLine("# Announce the coming of an airdrop");
            sb.AppendLine("announceDrops=true");
            sb.AppendLine("");
            sb.AppendLine("[Environment]");
            sb.AppendLine("# Set the time of day upon server start");
            sb.AppendLine("startTime=12");
            sb.AppendLine("# Set the time scale upon server start (Default 0.01)");
            sb.AppendLine("timeScale=0.01");
            sb.AppendLine("# Freeze time upon server start");
            sb.AppendLine("freezeTime=false");
            sb.AppendLine("# Set fall damage server-wide");
            sb.AppendLine("fallDamage=true");
            sb.AppendLine("# Set distance at which you can hear players in direct voice chat (Default 100)");
            sb.AppendLine("voiceDistance=100");
            sb.AppendLine("");
            sb.AppendLine("[Channels]");
            sb.AppendLine("# Enables or disables direct chat. ATLEAST ONE MUST BE ENABLED!");
            sb.AppendLine("directChat=true");
            sb.AppendLine("# Enables or disables global chat. ATLEAST ONE MUST BE ENABLED!");
            sb.AppendLine("globalChat=true");
            sb.AppendLine("# Toggles the display of the <g> tag if global is the only channel enabled");
            sb.AppendLine("removeTag=false");
            sb.AppendLine("# Sets the default chat players will talk in upon join. (global or direct)");
            sb.AppendLine("defaultChat=direct");
            sb.AppendLine("# Sets the distance the radius of possible text communication when in direct chat");
            sb.AppendLine("directDistance=150");
            sb.AppendLine("");
            sb.AppendLine("[Messages]");
            sb.AppendLine("# Name that the plugin bot will use to PM and chat with (Default Essentials)");
            sb.AppendLine("botName=Essentials");
            sb.AppendLine("# Join message that is displayed to all when a user joins (Default \"Player $USER$ has joined.\")");
            sb.AppendLine("joinMessage=Player $USER$ has joined.");
            sb.AppendLine("# Enables or disables the display of join messages");
            sb.AppendLine("enableJoin=true");
            sb.AppendLine("# Leave message that is displayed to all when a user leaves (Default \"Player $USER$ has left.\")");
            sb.AppendLine("leaveMessage=Player $USER$ has left.");
            sb.AppendLine("# Enables or disables the display of leave messages");
            sb.AppendLine("enableLeave=true");
            sb.AppendLine("# Suicide message that is displayed to all when a user commits suicide (Tags: $VICTIM$)");
            sb.AppendLine("suicideMessage=$VICTIM$ killed himself.");
            sb.AppendLine("# Enables or disables the display of suicide messages");
            sb.AppendLine("enableSuicide=true");
            sb.AppendLine("# Murder message that is displayed to all when a user is murdered by another user (Tags: $VICTIM$, $KILLER$, $WEAPON$, $PART$, $DISTANCE$)");
            sb.AppendLine("murderMessage=$KILLER$ [$WEAPON$ ($PART$)] $VICTIM$");
            sb.AppendLine("# Enables or disables the display of murder messages");
            sb.AppendLine("enableMurder=true");
            sb.AppendLine("# Murder message that is displayed to all when a user is killed by a mob (Tags: $VICTIM$, $KILLER$)");
            sb.AppendLine("deathMessage=$VICTIM$ was killed by a $KILLER$.");
            sb.AppendLine("# Enables or disables the display of death messages");
            sb.AppendLine("enableDeath=true");
            sb.AppendLine("# Enables/Disables the display of \"Unknown Command\" when an unrecognized command is typed");
            sb.AppendLine("unknownCommand=true");
            sb.AppendLine("# Enables/Disables the display of [PM to] and [PM from] in front of the user's name. If false, it will display before the message");
            sb.AppendLine("nextToName=true");
            sb.AppendLine("# Toggles the display of a user's rank prefix");
            sb.AppendLine("removePrefix=false");
            sb.AppendLine("");
            sb.AppendLine("[Logs]");
            sb.AppendLine("# When the plugin says something in chat, be it PM or broadcast to all, it will show in the logs");
            sb.AppendLine("logPluginChat=true");
            sb.AppendLine("# Number of chat logs the file will accept. If the number of chat logs exceeds this number, the extraneous file(s) will be deleted.");
            sb.AppendLine("chatLogCap=15");
            sb.AppendLine("# Number of logs the file will accept. If the number of logs exceeds this number, the extraneous file(s) will be deleted.");
            sb.AppendLine("logCap=15");
            sb.AppendLine("");
            sb.AppendLine("[Movement]");
            sb.AppendLine("# Enables or Disables teleport requesting.");
            sb.AppendLine("teleportRequest=true");
            sb.AppendLine("");
            sb.AppendLine("[MySQL]");
            sb.AppendLine("# IP for the MySQL whitelist database.");
            sb.AppendLine("host=localhost");
            sb.AppendLine("# Port for the MySQL whitelist database.");
            sb.AppendLine("port=3306");
            sb.AppendLine("# Database name for the MySQL whitelist database.");
            sb.AppendLine("database=RustEssentials");
            sb.AppendLine("# Username for the MySQL whitelist database.");
            sb.AppendLine("user=root");
            sb.AppendLine("# Password for the MySQL whitelist database.");
            sb.AppendLine("pass=");
            sb.AppendLine("");
            sb.AppendLine("[Inheritance]");
            sb.AppendLine("# If true, users will inherit their assigned commands plus the ones useable by those of lower ranks.");
            sb.AppendLine("inheritCommands=true");
            sb.AppendLine("# If true, users will inherit their assigned kits plus the ones useable by those of lower ranks.");
            sb.AppendLine("inheritKits=true");

            return sb;
        }

        public static StringBuilder prefixText()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("# This is the prefix file. Prefixes for specific users should go here.");
            sb.AppendLine("# Please note that the # symbol resembles a comment and should not be used when configuring.");
            sb.AppendLine("# ");
            sb.AppendLine("# Prefixes added to the file will overwrite the ones assigned by rank.");
            sb.AppendLine("# If, for example, a user is assigned the moderator rank, the prefix he is assigned here will be used instead of the moderator prefix.");
            sb.AppendLine("# Example:");
            sb.AppendLine("#   <USER ID>:<PREFIX>");
            sb.AppendLine("#   76569811000000000:!!");
            sb.AppendLine("");

            return sb;
        }

        public static StringBuilder kitsText()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("# This is the kits file. Kits that can be spawned with /kit should go here.");
            sb.AppendLine("# Please note that the # symbol resembles a comment and should not be used when configuring.");
            sb.AppendLine("# ");
            sb.AppendLine("# If inheritKits is false, kits will only be assigned to their respective rank/prefix.");
            sb.AppendLine("# ");
            sb.AppendLine("# Each kit in the list can be set with [<Name>] and spawned with /kit <Name>.");
            sb.AppendLine("# Kits CANNOT have a space in their name.");
            sb.AppendLine("# Kits are not case sensitive.");
            sb.AppendLine("# Each item specified per kit should be set with name in front and amount after.");
            sb.AppendLine("# Example:");
            sb.AppendLine("#   [Starter]");
            sb.AppendLine("#   Stone Hatchet:1");
            sb.AppendLine("#");
            sb.AppendLine("# Kits can also be set to work for users with direct or inherited permissions by appending \".<Rank Prefix>\" after the kit name.");
            sb.AppendLine("# Example:");
            sb.AppendLine("#   [Starter] <--- This kit is usable by everyone IF inheritKits is true. If not, only defaults.");
            sb.AppendLine("#   Stone Hatchet:1");
            sb.AppendLine("#   [SomeKit.O] <--- This kit is only usable by those with the Owner rank.");
            sb.AppendLine("#   Stone Hatchet:1");
            sb.AppendLine("#   [ModKit.M] <--- This kit can be used by Moderators and those more powerful than that (AKA Admins and Owners) IF inheritKits is true.");
            sb.AppendLine("#   Stone Hatchet:1");
            sb.AppendLine("#");
            sb.AppendLine("# Kits can also be designed to have cooldowns by adding \"cooldown=<#(s/m/h)>\" after the kit name.");
            sb.AppendLine("# Example:");
            sb.AppendLine("#   [Starter] <--- This kit can be used without any cool down");
            sb.AppendLine("#   Stone Hatchet:1");
            sb.AppendLine("#   [SomeKit.O] <--- This kit has a 30 second cool down.");
            sb.AppendLine("#   cooldown=30s");
            sb.AppendLine("#   Stone Hatchet:1");
            sb.AppendLine("#   [ModKit.M] <--- This kit has a 30 minute cool down.");
            sb.AppendLine("#   cooldown=30m");
            sb.AppendLine("#   Stone Hatchet:1");
            sb.AppendLine("");
            sb.AppendLine("[Starter]");
            sb.AppendLine("cooldown=30s");
            sb.AppendLine("Stone Hatchet:1");
            sb.AppendLine("Cloth Vest:1");
            sb.AppendLine("Cloth Pants:1");
            sb.AppendLine("");
            sb.AppendLine("[Invis.O]");
            sb.AppendLine("Invisible Boots:1");
            sb.AppendLine("Invisible Pants:1");
            sb.AppendLine("Invisible Vest:1");
            sb.AppendLine("Invisible Helmet:1");

            return sb;
        }

        public static StringBuilder ranksText()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("# This is the ranks file. Rank assignments along with custom ranks should go here.");
            sb.AppendLine("# Please note that the # symbol resembles a comment and should not be used when configuring.");
            sb.AppendLine("#");
            sb.AppendLine("# The list is read in order of authority. The means the top most rank has the highest power, authority, and priority and vice versa for the bottom most rank.");
            sb.AppendLine("# EX: [Owner] has an access level/authority of 5 (highest) and [Default] of 1 (highest).");
            sb.AppendLine("#");
            sb.AppendLine("# Users are assigned ranks per their Steam User ID (SUID/UID). Using # <Name> is simply a way of easily identifying for future reading and is not required.");
            sb.AppendLine("# Users can grab their UID in game by typing \"/uid\".");
            sb.AppendLine("# Example format:");
            sb.AppendLine("# [Owner.O]");
            sb.AppendLine("# 76560000000000000 # Some Guy");
            sb.AppendLine("#");
            sb.AppendLine("# Appending .<string> will assign a rank prefix to that rank. Prefix recommended to be only 1 or 2 characters.");
            sb.AppendLine("# [Owner.O] or [Member.:D]");
            sb.AppendLine("# will make format user names in chat into:");
            sb.AppendLine("# [O] OwnerGuy: Hello");
            sb.AppendLine("# [:D] MemberGuy: What up");
            sb.AppendLine("");
            sb.AppendLine("[Owner.O]");
            sb.AppendLine("");
            sb.AppendLine("[Administrator.A]");
            sb.AppendLine("");
            sb.AppendLine("[Moderator.M]");
            sb.AppendLine("");
            sb.AppendLine("# Users under the [Member] rank will be ignored - always.");
            sb.AppendLine("# To enable the [Member] rank, set \"whitelist\" to false and \"useAsMembers\" to true in the config.ini.");
            sb.AppendLine("# User IDs found in the whitelist.txt will then be read as [Member]'s.");
            sb.AppendLine("# Note: You cannot change the rank name [Member]. If [Member] does not exist, \"useAsMembers\" will not work.");
            sb.AppendLine("[Member.m] # DO NOT ASSIGN PLAYERS TO THIS RANK, READ ABOVE");
            sb.AppendLine("");
            sb.AppendLine("# This is the rank with the lowest authority.");
            sb.AppendLine("# isDefaultRank sets the preceding rank, in this case [Default], as the default rank for new users.");
            sb.AppendLine("# The rank set as isDefaultRank will not accept UID specification.");
            sb.AppendLine("[Default] # DO NOT ASSIGN PLAYERS TO THIS RANK");
            sb.AppendLine("isDefaultRank");
            sb.AppendLine("");

            return sb;
        }

        public static StringBuilder motdText()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("# This is the MOTD file. Messages displayed upon join or cycle should go here.");
            sb.AppendLine("# Please note that the # symbol resembles a comment and should not be used when configuring.");
            sb.AppendLine("# ");
            sb.AppendLine("# [Join] is the MOTD that is broadcasted directly to the user that joins upon connecting.");
            sb.AppendLine("# [Cycle.#(s/m/h)] is the MOTD that is broadcasted to all users every time the # elapses.");
            sb.AppendLine("#");
            sb.AppendLine("# The #(s/m/h) in [Cycle.#(s/m/h)] resembles the interval in (s)econds, (m)inutes, or (h)ours.");
            sb.AppendLine("# Example:");
            sb.AppendLine("#   [Cycle.2h]");
            sb.AppendLine("#   This is the MOTD.");
            sb.AppendLine("# This MOTD will be broadcasted every 2 hours to all users.");
            sb.AppendLine("#");
            sb.AppendLine("# Remeber that you can add and remove as many lines as you want for the two MOTDs.");
            sb.AppendLine("");
            sb.AppendLine("[Join]");
            sb.AppendLine("JoinMessage1 # DELETE THESE LINES TO REMOVE JOIN MOTD");
            sb.AppendLine("JoinMessage2 # DELETE THESE LINES TO REMOVE JOIN MOTD");
            sb.AppendLine("JoinMessage3 # DELETE THESE LINES TO REMOVE JOIN MOTD");
            sb.AppendLine("");
            sb.AppendLine("[Cycle.15m]");
            sb.AppendLine("CycleMessage1 # DELETE THESE LINES TO REMOVE CYCLE MOTD");
            sb.AppendLine("CycleMessage2 # DELETE THESE LINES TO REMOVE CYCLE MOTD");
            sb.AppendLine("CycleMessage3 # DELETE THESE LINES TO REMOVE CYCLE MOTD");
            sb.AppendLine("");
            sb.AppendLine("[Rules]");
            sb.AppendLine("RuleMessage1 # DELETE THESE LINES TO REMOVE CYCLE MOTD");
            sb.AppendLine("RuleMessage2 # DELETE THESE LINES TO REMOVE CYCLE MOTD");
            sb.AppendLine("RuleMessage3 # DELETE THESE LINES TO REMOVE CYCLE MOTD");

            return sb;
        }

        public static StringBuilder commandsText()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("# This is the commands file. Permissions for commands should go here.");
            sb.AppendLine("# Please note that the # symbol resembles a comment and should not be used when configuring.");
            sb.AppendLine("#");
            sb.AppendLine("# The list is read in order of authority unless inheritCommands is false. This means that the ranks with the most power should be at the top and least at the bottom.");
            sb.AppendLine("# Ex: [Owner] at the top and [Default] at the bottom.");
            sb.AppendLine("#");
            sb.AppendLine("# Ranks will inherit the authority/permissions of the ranks below them.");
            sb.AppendLine("# Ex: [Owner] has permission to use the commands of [Administrator] even though it is only defined for [Administrator].");
            sb.AppendLine("#");
            sb.AppendLine("# Example format:");
            sb.AppendLine("#   [Rank name]");
            sb.AppendLine("#   /kick");
            sb.AppendLine("#   /ban");
            sb.AppendLine("#   /online");
            sb.AppendLine("#");
            sb.AppendLine("#   [Rank name 2]");
            sb.AppendLine("#   /help");
            sb.AppendLine("");
            sb.AppendLine("[Owner]");
            sb.AppendLine("/i");
            sb.AppendLine("/give");
            sb.AppendLine("/reload");
            sb.AppendLine("/timescale");
            sb.AppendLine("/kill");
            sb.AppendLine("/stop");
            sb.AppendLine("/access");
            sb.AppendLine("/remove");
            sb.AppendLine("");
            sb.AppendLine("[Administrator]");
            sb.AppendLine("/airdrop");
            sb.AppendLine("/ban");
            sb.AppendLine("/unban");
            sb.AppendLine("/kickall");
            sb.AppendLine("/pos");
            sb.AppendLine("/say");
            sb.AppendLine("/saypop");
            sb.AppendLine("/time");
            sb.AppendLine("/tp");
            sb.AppendLine("/god");
            sb.AppendLine("/ungod");
            sb.AppendLine("/whitelist");
            sb.AppendLine("/tppos");
            sb.AppendLine("/heal");
            sb.AppendLine("");
            sb.AppendLine("[Moderator]");
            sb.AppendLine("/kick");
            sb.AppendLine("/join");
            sb.AppendLine("/leave");
            sb.AppendLine("/mute");
            sb.AppendLine("/unmute");
            sb.AppendLine("/save");
            sb.AppendLine("");
            sb.AppendLine("[Member]");
            sb.AppendLine("");
            sb.AppendLine("[Default]");
            sb.AppendLine("/uid");
            sb.AppendLine("/kit");
            sb.AppendLine("/kits");
            sb.AppendLine("/help");
            sb.AppendLine("/pm");
            sb.AppendLine("/f");
            sb.AppendLine("/online");
            sb.AppendLine("/players");
            sb.AppendLine("/chan");
            sb.AppendLine("/history");
            sb.AppendLine("/tpa");
            sb.AppendLine("/tpaccept");
            sb.AppendLine("/tpdeny");
            sb.AppendLine("/share");
            sb.AppendLine("/unshare");
            sb.AppendLine("/rules");
            sb.AppendLine("/version");
            sb.AppendLine("/whitelist check");

            return sb;
        }

        public static void readFactionData()
        {
            List<string> factionFileData = File.ReadAllLines(factionsFile).ToList();
            foreach (string s in factionFileData)
            {
                string factionName = s.Split('=')[0];
                string membersString = s.Split('=')[1];

                factions.Add(factionName, new Dictionary<string, string>());

                foreach (string s2 in membersString.Split(';'))
                {
                    string nameAndUID = s2.Split(':')[0];
                    string rank = s2.Split(':')[1];
                    string UID = nameAndUID.Substring(nameAndUID.LastIndexOf(')') + 1);

                    factions[factionName].Add(UID, rank);
                }
            }
        }

        public static void addFactionData(string factionName, string userName, string userID, string rank)
        {
            List<string> factionFileData = File.ReadAllLines(factionsFile).ToList();
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
            using (StreamWriter sw = new StreamWriter(factionsFile, false))
            {
                foreach (string s in factionFileData)
                {
                    sw.WriteLine(s);
                }
            }
        }

        public static void remFactionData(string factionName, string userName, string rank)
        {
            List<string> factionFileData = File.ReadAllLines(factionsFile).ToList();
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
                        Dictionary<string, string> currentMembers = new Dictionary<string,string>();
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
            using (StreamWriter sw = new StreamWriter(factionsFile, false))
            {
                foreach (string s in factionFileData)
                {
                    sw.WriteLine(s);
                }
            }
        }

        public static void readDoorData()
        {
            List<string> doorDataFile = File.ReadAllLines(Vars.doorsFile).ToList();
            foreach (string s in doorDataFile)
            {
                string owner = s.Split('=')[0];
                string partnerString = s.Split('=')[1];
                Vars.sharingData.Add(owner, partnerString);
            }
        }

        public static void addDoorData(string ownerID, string partnerID)
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

        public static void remDoorData(string ownerID, string partnerID)
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

        public static StringBuilder allCommandsText()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("# Braces ({}) symbolize input that should be typed literally.");
            sb.AppendLine("# Example (/time {day}):");
            sb.AppendLine("# /time day");
            sb.AppendLine("#");
            sb.AppendLine("# Asterisks (**) symbolize input that should NOT be surrounded by quotes and can be MORE than one word.");
            sb.AppendLine("# Example (/pm <player name> [message] should be typed as):");
            sb.AppendLine("# /pm \"Some Dude\" Hey, Some Dude!");
            sb.AppendLine("#");
            sb.AppendLine("# Brackets ([]) symbolize input that should NOT be surrounded by quotes and can only be one word.");
            sb.AppendLine("# Example (/kit [kit name] should be typed as):");
            sb.AppendLine("# /kit Starter");
            sb.AppendLine("#");
            sb.AppendLine("# Arrows (<>) symbolize input that DOES require quotes if it is more than one word. No quotes needed if input is only one word.");
            sb.AppendLine("# Example (/kick <name> [reason] should be typed as):");
            sb.AppendLine("# /kick \"Some Guy\" Being some guy.");
            sb.AppendLine("# or");
            sb.AppendLine("# /kick SomeGuy Kick.");
            sb.AppendLine("#");
            sb.AppendLine("# Player names and item names are case sensitive. However, player names can be accepted as partials.");
            sb.AppendLine("");
            sb.AppendLine("/access {on} (Gives the sender access to all doors)");
            sb.AppendLine("/access {off} (Revokes access to all doors from the sender)");
            sb.AppendLine("/airdrop (Spawns an airdrop with a random drop location)");
            sb.AppendLine("/airdrop <player name> (Spawns an airdrop with a drop location at the specified player)");
            sb.AppendLine("/ban <player name> (Bans player with reason: \"Banned by a(n) <Your Rank>\")");
            sb.AppendLine("/ban <player name> [reason] (Bans player with reason)");
            sb.AppendLine("/chan {g} (Joins the global chat)");
            sb.AppendLine("/chan {global} (Joins the global chat)");
            sb.AppendLine("/chan {d} (Joins the direct chat)");
            sb.AppendLine("/chan {direct} (Joins the direct chat)");
            sb.AppendLine("/f {admin} *player name* (Gives faction admin to the specified faction member)");
            sb.AppendLine("/f {create} *name* (Creates and joins a faction with specified name)");
            sb.AppendLine("/f {deadmin} *player name* (Revokes faction admin from the specified faction member)");
            sb.AppendLine("/f {disband} (Disbands the current faction if user is the owner of said faction)");
            sb.AppendLine("/f {join} (Joins the faction from the last invitation received)");
            sb.AppendLine("/f {join} *name* (Joins the specified faction if invited)");
            sb.AppendLine("/f {leave} (Leaves current faction)");
            sb.AppendLine("/f {kick} *name* (Kicks user with said name from faction)");
            sb.AppendLine("/f {ownership} *player name* (Transfers ownership of faction to specified faction member)");
            sb.AppendLine("/give <player name> <item name> (Gives the item to that player)");
            sb.AppendLine("/give <player name> <item name> [amount] (Gives the amount of the item to that player)");
            sb.AppendLine("/give <player name> [item id] (Gives 1 of the item with the corresponding id to that player)");
            sb.AppendLine("/give <player name> [item id] [amount] (Gives the amount of the item with the corresponding id to that player)");
            sb.AppendLine("/god *player name* (Gives the specified player god mode)");
            sb.AppendLine("/heal *player name* (Heals the designated player)");
            sb.AppendLine("/help (Returns available commands for your current rank)");
            sb.AppendLine("/help [command without /] (Returns the documentation and syntax for the specified command)");
            sb.AppendLine("/history {1-50} (Returns the the last # lines of the chat history)");
            sb.AppendLine("/i <item name> (Gives the item to you)");
            sb.AppendLine("/i <item name> [amount] (Gives the amount of the item to you)");
            sb.AppendLine("/i [item id] (Gives 1 of the item with the corresponding id to you)");
            sb.AppendLine("/i [item id] [amount] (Gives the amount of the item with the corresponding id to you)");
            sb.AppendLine("/join (Emulates the joining of yourself)");
            sb.AppendLine("/join <player name> (Emulates the joining of a fake player)");
            sb.AppendLine("/kick <player name> (Kick player with reason: \"Kicked by a(n) <Your Rank>\")");
            sb.AppendLine("/kick <player name> [reason] (Kick player with reason)");
            sb.AppendLine("/kickall (Kicks all users, except for the command executor, out of the server)");
            sb.AppendLine("/kill *player name* (Kills the specified player)");
            sb.AppendLine("/kit [kit name] (Gives the user the specified kit if the user has the correct authority level)");
            sb.AppendLine("/kits (Lists the kits available to you)");
            sb.AppendLine("/leave (Emulates the joining of yourself)");
            sb.AppendLine("/leave *player name* (Emulates the leaving of a fake player)");
            sb.AppendLine("/mute *player name* (Mutes the player on global chat)");
            sb.AppendLine("/mute *player name* <time[s/m/h]>(Mutes the player on global chat for a period of time (time example: 15s or 30m))");
            sb.AppendLine("/online (Returns the amount of players currently connected)");
            sb.AppendLine("/players (Lists the names of all connected players)");
            sb.AppendLine("/pm <player name> *message* (Sends a private message to that player)");
            sb.AppendLine("/pos (Returns the player's position)");
            sb.AppendLine("/reload {config/whitelist/ranks/commands/kits/motd/bans/all} (Reloads the specified file)");
            sb.AppendLine("/remove {on} (Gives access to delete entities (structures and AI entities) upon hit)");
            sb.AppendLine("/remove {of} (Revokes access to delete entities (structures and AI entities) upon hit)");
            sb.AppendLine("/rules (Lists the server rules)");
            sb.AppendLine("/save (Saves all world data)");
            sb.AppendLine("/say *message* (Says a message through the plugin)");
            sb.AppendLine("/saypop *message* (Says a (!) dropdown message to all clients)");
            sb.AppendLine("/saypop [icon] *message* (Says a dropdown message to all clients with designated icon)");
            sb.AppendLine("/share *player name* (Shares ownership of your doors with the designated user)");
            sb.AppendLine("/stop (Saves, deactivates, and effectively stops the server)");
            sb.AppendLine("/time (Returns current time of day)");
            sb.AppendLine("/time {0-24} (Sets time to a number between 0 and 24)");
            sb.AppendLine("/time {day} (Sets time to day)");
            sb.AppendLine("/time {freeze} (Freezes time)");
            sb.AppendLine("/time {night} (Sets time to night)");
            sb.AppendLine("/time {unfreeze} (Unfreezes time)");
            sb.AppendLine("/timescale (Returns the speed at which time passes)");
            sb.AppendLine("/timescale [#] (Sets the speed at which time passes. Recommended between 0 and 1. WARNING: THIS AFFECTS AIRDROPS)");
            sb.AppendLine("/tp *player name* (Teleports the operator/sender to the designated user)");
            sb.AppendLine("/tp <player name 1> <player name 2> (Teleports player 1 to player 2)");
            sb.AppendLine("/tpa *player name* (Sends a teleport request to that user)");
            sb.AppendLine("/tpaccept (Accepts the last teleport request recieved)");
            sb.AppendLine("/tpaccept *player name* (Accepts the teleport request from that user)");
            sb.AppendLine("/tpdeny *player name* (Denies the teleport request from that user)");
            sb.AppendLine("/tpdeny {all} (Denies all current teleport requests)");
            sb.AppendLine("/tppos [x] [y] [z] (Teleports your character to the designated vector)");
            sb.AppendLine("/uid (Returns your steam UID)");
            sb.AppendLine("/uid *player name* (Returns that user's steam UID)");
            sb.AppendLine("/unban *player name* (Unbans the specified player)");
            sb.AppendLine("/ungod *player name* (Revokes god mode from the specified player)");
            sb.AppendLine("/unmute *player name* (Unmutes the player on global chat)");
            sb.AppendLine("/unshare {all} (Revokes ownership of your doors from everyone)");
            sb.AppendLine("/unshare *player name*(Revokes ownership of your doors from the designated user)");
            sb.AppendLine("/version (Returns the current running version of Rust Essentials)");
            sb.AppendLine("/whitelist {add} [UID] (Adds the specified Steam UID to the whitelist)");
            sb.AppendLine("/whitelist {check} (Checks if you're currently on the whitelist)");
            sb.AppendLine("/whitelist {kick} (Kicks all players that are not whitelisted. This only work if whitelist is enabled)");
            sb.AppendLine("/whitelist {off} (Turns whitelist off)");
            sb.AppendLine("/whitelist {on} (Turns whitelist on)");
            sb.AppendLine("/whitelist {rem} [UID] (Removes the specified Steam UID to the whitelist)");

            return sb;
        }

        public static void reloadFile(uLink.NetworkPlayer sender, string[] args)
        {
            if (args.Count() > 1)
            {
                string file = args[1];
                switch (file)
                {
                    case "config":
                        RustEssentialsBootstrap._load.loadConfig();
                        Broadcast.broadcastTo(sender, "Config reloaded.");
                        break;
                    case "whitelist":
                        Whitelist.readWhitelist();
                        Broadcast.broadcastTo(sender, "Whitelist reloaded.");
                        break;
                    case "ranks":
                        RustEssentialsBootstrap._load.loadRanks();
                        Broadcast.broadcastTo(sender, "Ranks reloaded.");
                        break;
                    case "commands":
                        RustEssentialsBootstrap._load.loadCommands();
                        Broadcast.broadcastTo(sender, "Command permissions reloaded.");
                        break;
                    case "kits":
                        RustEssentialsBootstrap._load.loadKits();
                        Broadcast.broadcastTo(sender, "Kits reloaded.");
                        break;
                    case "motd":
                        RustEssentialsBootstrap._load.loadMOTD();
                        Broadcast.broadcastTo(sender, "MOTD reloaded.");
                        break;
                    case "bans":
                        RustEssentialsBootstrap._load.loadBans();
                        Broadcast.broadcastTo(sender, "Bans reloaded.");
                        break;
                    case "prefix":
                        RustEssentialsBootstrap._load.loadPrefixes();
                        Broadcast.broadcastTo(sender, "Prefixes reloaded.");
                        break;
                    case "all":
                        RustEssentialsBootstrap._load.loadConfig();
                        Whitelist.readWhitelist();
                        RustEssentialsBootstrap._load.loadRanks();
                        RustEssentialsBootstrap._load.loadCommands();
                        RustEssentialsBootstrap._load.loadKits();
                        RustEssentialsBootstrap._load.loadMOTD();
                        RustEssentialsBootstrap._load.loadBans();
                        RustEssentialsBootstrap._load.loadPrefixes();
                        Broadcast.broadcastTo(sender, "All files reloaded.");
                        break;
                    default:
                        Broadcast.broadcastTo(sender, "No such file " + file + ".");
                        break;
                }
            }
        }

        public static string replaceQuotes(string s)
        {
            return s.Replace("\"", "\\\"");
        }

        public static void fakeLeave(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string fakeName = "";
                if (args[1].Contains("\""))
                {
                    bool hadQuote = false;
                    int lastIndex = -1;
                    List<string> playerNameList = new List<string>();
                    foreach (string s in args)
                    {
                        lastIndex++;
                        if (s.StartsWith("\"")) hadQuote = true;
                        if (hadQuote)
                        {
                            playerNameList.Add(s);
                        }
                        if (s.EndsWith("\""))
                        {
                            hadQuote = false;
                            break;
                        }
                    }

                    fakeName = string.Join(" ", playerNameList.ToArray()).Replace("\"", "").Trim();
                }
                else
                {
                    fakeName = args[1];
                }

                string leaveMessage = Vars.leaveMessage.Replace("$USER$", fakeName);
                Broadcast.broadcastAll(leaveMessage);
            }
            else
            {
                string leaveMessage = Vars.leaveMessage.Replace("$USER$", senderClient.userName);
                Broadcast.broadcastAll(leaveMessage);
            }
        }

        public static void setScale(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                try
                {
                    if (!timeFrozen)
                    {
                        double time = Convert.ToDouble(args[1]);
                        Time.setScale(time);
                        Broadcast.broadcastTo(senderClient.netPlayer, "Time scale set to " + time.ToString() + ".");
                    }
                    else
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "Time is currently frozen! Unfreeze time to change the time scale.");
                    }
                }
                catch (Exception ex)
                {
                    Broadcast.broadcastTo(senderClient.netPlayer, "Time scale must be a number!");
                }
            }
            else
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "The time scale is currently " + Math.Round(Time.getScale(), 2) + ".");
            }
        }

        public static void setTime(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                try
                {
                        double time = Convert.ToDouble(args[1]);
                        Time.setTime(time);
                        Broadcast.broadcastTo(senderClient.netPlayer, "Time set to " + time.ToString() + ".");
                }
                catch (Exception ex)
                {
                    if (args[1] == "freeze")
                    {
                        Time.freezeTime(true);
                        Broadcast.broadcastTo(senderClient.netPlayer, "Time has been frozen.");
                    }
                    else if (args[1] == "unfreeze")
                    {
                        Time.freezeTime(false);
                        Broadcast.broadcastTo(senderClient.netPlayer, "Time has been unfrozen.");
                    }
                    else if (args[1] == "day")
                    {
                        Time.setDay();
                        Broadcast.broadcastTo(senderClient.netPlayer, "Time set to day.");
                    }
                    else if (args[1] == "night")
                    {
                        Time.setNight();
                        Broadcast.broadcastTo(senderClient.netPlayer, "Time set to night.");
                    }
                }
            }
            else
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "The time is currently " + Math.Round(Time.getTime(), 2) + ".");
            }
        }

        public static void cycleMOTD()
        {
            TimerPlus t = new TimerPlus();
            t.AutoReset = true;
            t.Interval = cycleInterval;
            t.Elapsed += cycleMOTDElapsed;
            t.Start();
        }

        private static int timesCycled = 0;
        private static void cycleMOTDElapsed(object sender, ElapsedEventArgs e)
        {
            timesCycled++;
            if (timesCycled > 1)
            {
                if (motdList.Keys.Contains("Cycle"))
                {
                    foreach (string s in motdList["Cycle"])
                    {
                        Broadcast.broadcastAll(s);
                    }
                }
            }
        }

        public static string findRank(string UID)
        {
            string rank = defaultRank;
            foreach (KeyValuePair<string, List<string>> kv in rankList)
            {
                if (kv.Value.Contains(UID))
                {
                    rank = kv.Key;
                    break;
                }
            }
            return rank;
        }

        public static int findRankPriority(string rank)
        {
            int rankPriority = (rankList.Count - rankList.Keys.ToList().FindIndex((string s) => s == rank)) + 1;
            return rankPriority;
        }

        public static void godMode(uLink.NetworkPlayer sender, string senderName, string[] args, bool b)
        {
            if (args.Count() > 1)
            {
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

                string playerName = string.Join(" ", playerNameList.ToArray());

                PlayerClient[] possibleTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                if (possibleTargets.Count() == 0)
                    Broadcast.broadcastTo(sender, "No player names equal or contain \"" + playerName + "\".");
                else if (possibleTargets.Count() > 1)
                    Broadcast.broadcastTo(sender, "Too many player names contain \"" + playerName + "\".");
                else
                {
                    PlayerClient targetClient = possibleTargets[0];

                    TakeDamage component = targetClient.controllable.GetComponent<TakeDamage>();

                    if (b)
                    {
                        if (!godList.Contains(targetClient.userID.ToString()))
                            godList.Add(targetClient.userID.ToString());
                    }
                    else
                    {
                        if (godList.Contains(targetClient.userID.ToString()))
                            godList.Remove(targetClient.userID.ToString());
                    }

                    component.SetGodMode(b);
                    if (targetClient.netPlayer != sender)
                    {
                        Broadcast.noticeTo(sender, "♫", (b ? "God mode granted to " + targetClient.userName + "." : "Revoked " + targetClient.userName + "'s god mode."));
                        Broadcast.noticeTo(targetClient.netPlayer, "♫", (b ? "God mode granted by " + senderName + "." : "God mode revoked by " + senderName + "."));
                    }
                    else
                    {
                        Broadcast.noticeTo(sender, "♫", (b ? "God mode activated." : "God mode deactivated."));
                    }
                }
            }
        }

        public static void healPlayer(uLink.NetworkPlayer sender, string senderName, string[] args)
        {
            if (args.Count() > 1)
            {
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

                string playerName = string.Join(" ", playerNameList.ToArray());

                PlayerClient[] possibleTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                if (possibleTargets.Count() == 0)
                    Broadcast.broadcastTo(sender, "No player names equal or contain \"" + playerName + "\".");
                else if (possibleTargets.Count() > 1)
                    Broadcast.broadcastTo(sender, "Too many player names contain \"" + playerName + "\".");
                else
                {
                    PlayerClient targetClient = possibleTargets[0];

                    TakeDamage component = targetClient.controllable.GetComponent<TakeDamage>();
                    Character targetChar;
                    Character.FindByUser(targetClient.userID, out targetChar);

                    component.Heal(targetChar.idMain, 100f);

                    if (targetClient.netPlayer != sender)
                    {
                        Broadcast.noticeTo(targetClient.netPlayer, "♫", ("You were healed by " + senderName + "."));
                    }
                    else
                    {
                        Broadcast.noticeTo(sender, "♫", "You were healed.");
                    }
                }
            }
        }

        public static void loadItems()
        {
            using (StreamWriter sw = new StreamWriter(itemsFile))
            {

                int curIndex = 0;
                foreach (var item in DatablockDictionary.All)
                {
                    curIndex++;
                    itemIDs.Add(curIndex, item.name);

                    sw.WriteLine(curIndex + ": " + item.name);
                }

                Vars.conLog.Info(curIndex + " items found and loaded!");
            }
        }

        public static void setFall(uLink.NetworkPlayer sender, string[] args)
        {
            if (args.Count() == 2)
            {
                string mode = args[1];
                switch (mode)
                {
                    case "on":
                        Broadcast.broadcastAll("Fall damage has been disabled for everyone.");
                        fallDamage = true;
                        break;
                    case "off":
                        Broadcast.broadcastAll("Fall damage has been enabled for everyone.");
                        fallDamage = false;
                        break;
                }
            }
        }

        public static void killTarget(uLink.NetworkPlayer sender, string[] args)
        {
            if (args.Count() == 2)
            {
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

                string playerName = string.Join(" ", playerNameList.ToArray());

                PlayerClient senderClient = Array.Find(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.netPlayer == sender);
                PlayerClient[] possibleTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                if (possibleTargets.Count() == 0)
                    Broadcast.broadcastTo(sender, "No player names equal or contain \"" + playerName + "\".");
                else if (possibleTargets.Count() > 1)
                    Broadcast.broadcastTo(sender, "Too many player names contain \"" + playerName + "\".");
                else
                {
                    try
                    {
                        PlayerClient targetClient = possibleTargets[0];
                        Character targetChar;
                        Character.FindByUser(targetClient.userID, out targetChar);

                        if (ofLowerRank(targetClient.userID.ToString(), senderClient.userID.ToString(), false))
                        {
                            IDBase idBase = (IDBase)targetChar;
                            Broadcast.noticeTo(targetClient.netPlayer, "№", "You fell victim to /kill.");
                            killList.Add(targetClient);
                            int result = (int)TakeDamage.Kill(idBase, idBase);
                        }
                        else
                        {
                            Broadcast.noticeTo(sender, "№", "You are not allowed to /kill those of higher authority.");
                            Broadcast.noticeTo(targetClient.netPlayer, "№", senderClient.userName + " tried to /kill you.");
                        }
                    }
                    catch (Exception ex) { Vars.conLog.Error(ex.ToString()); }
                }
            }
        }

        public static void saveBans()
        {
            if (File.Exists(bansFile))
            {
                using (StreamWriter sw = new StreamWriter(bansFile))
                {
                    foreach (KeyValuePair<string, string> kv in currentBans)
                    {
                        string reason = currentBanReasons[kv.Value];
                        sw.WriteLine(kv.Key + "=" + kv.Value + " # " + reason);
                    }
                }
            }
        }

        public static void grabUID(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
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

                string playerName = string.Join(" ", playerNameList.ToArray());

                PlayerClient[] possibleTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                if (possibleTargets.Count() == 0)
                    Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                else if (possibleTargets.Count() > 1)
                    Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                else
                {
                    PlayerClient targetClient = possibleTargets[0];
                    Broadcast.broadcastTo(senderClient.netPlayer, targetClient.userName + "'s UID is " + targetClient.userID.ToString() + ".");
                }
            }
            else
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "Your UID is " + senderClient.userID.ToString() + ".");
            }
        }

        public static void giveAccess(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();
                switch (mode)
                {
                    case "on":
                        Broadcast.broadcastTo(senderClient.netPlayer, "You now have access to all doors.");
                        if (completeDoorAccess.Contains(UID))
                            Broadcast.broadcastTo(senderClient.netPlayer, "You already have access to all doors.");
                        else
                            completeDoorAccess.Add(UID);
                        break;
                    case "off":
                        Broadcast.broadcastTo(senderClient.netPlayer, "You no longer have access to all doors.");
                        if (completeDoorAccess.Contains(UID))
                            completeDoorAccess.Remove(UID);
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You do not currently have access to all doors.");
                        break;
                }
            }
        }

        public static void unbanPlayer(PlayerClient playerClient, string[] args)
        {
            if (args.Count() > 1)
            {
                bool hadQuote = false;
                string targetName = "";
                int lastIndex = 0;
                if (args[1].Contains("\""))
                {
                    foreach (string s in args)
                    {
                        lastIndex++;
                        if (s.StartsWith("\"")) hadQuote = true;
                        if (hadQuote)
                        {
                            targetName += s + " ";
                        }
                        if (s.EndsWith("\""))
                        {
                            hadQuote = false;
                            break;
                        }
                    }

                    targetName = targetName.Replace("\"", "").Trim();
                }
                else
                {
                    targetName = args[1];
                    lastIndex = 1;
                }

                RustEssentialsBootstrap._load.loadBans();
                if (currentBans.Keys.Contains(targetName))
                {
                    string UID = currentBans[targetName];
                    currentBans.Remove(targetName);
                    currentBanReasons.Remove(UID);
                    saveBans();

                    Broadcast.noticeTo(playerClient.netPlayer, "☻", "Player " + targetName + " has been unbanned.");

                    File.WriteAllText(Path.Combine(Vars.rootDir, "cfg\\bans.cfg"), String.Empty);
                    using (StreamWriter sw = new StreamWriter(Path.Combine(Vars.rootDir, "cfg\\bans.cfg")))
                    {
                        foreach (KeyValuePair<string, string> kv in currentBans)
                        {
                            sw.WriteLine("banid " + kv.Value);
                        }
                    }
                }
                else if (currentBans.Values.Contains(targetName))
                {
                    try
                    {
                        string UID = Convert.ToInt64(targetName).ToString();
                        string playerName = "";
                        foreach (KeyValuePair<string, string> kv in currentBans)
                        {
                            if (kv.Value == UID)
                                playerName = kv.Key;
                        }
                        if (currentBans.ContainsKey(playerName))
                        {
                            currentBans.Remove(playerName);
                            currentBanReasons.Remove(UID);
                            saveBans();

                            Broadcast.noticeTo(playerClient.netPlayer, "☻", "Player " + playerName + " (" + UID + ") has been unbanned.");

                            File.WriteAllText(Path.Combine(Vars.rootDir, "cfg\\bans.cfg"), String.Empty);
                            using (StreamWriter sw = new StreamWriter(Path.Combine(Vars.rootDir, "cfg\\bans.cfg")))
                            {
                                foreach (KeyValuePair<string, string> kv in currentBans)
                                {
                                    sw.WriteLine("banid " + kv.Value);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Broadcast.noticeTo(playerClient.netPlayer, "№", "Player " + targetName + " is not banned!");
                    }
                }
                else
                {
                    Broadcast.noticeTo(playerClient.netPlayer, "№", "Player " + targetName + " is not banned!");
                }
            }
        }

        public static void banPlayer(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                bool hadQuote = false;
                string targetName = "";
                int lastIndex = 0;
                if (args[1].Contains("\""))
                {
                    foreach (string s in args)
                    {
                        lastIndex++;
                        if (s.StartsWith("\"")) hadQuote = true;
                        if (hadQuote)
                        {
                            targetName += s + " ";
                        }
                        if (s.EndsWith("\""))
                        {
                            hadQuote = false;
                            break;
                        }
                    }

                    targetName = targetName.Replace("\"", "").Trim();
                }
                else
                {
                    targetName = args[1];
                    lastIndex = 1;
                }

                try
                {
                    string UID = Convert.ToInt64(targetName).ToString();
                    PlayerClient[] possibleTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userID.ToString() == UID);
                    PlayerClient possibleTarget = null;
                    if (possibleTargets.Count() > 0)
                    {
                        possibleTarget = possibleTargets[0];

                        try
                        {
                            NetUser target = possibleTarget.netUser;
                            if (target != null)
                            {
                                string reason = "";
                                List<string> reasonList = new List<string>();
                                if (args.Count() - 1 > lastIndex)
                                {
                                    int curIndex = 0;
                                    foreach (string s in args)
                                    {
                                        if (curIndex > lastIndex)
                                        {
                                            reasonList.Add(s);
                                        }
                                        curIndex++;
                                    }

                                    reason = string.Join(" ", reasonList.ToArray());
                                }
                                else
                                {
                                    reason = "Banned by a(n) " + findRank(senderClient.userID.ToString()) + ".";
                                }

                                if (ofLowerRank(target.userID.ToString(), senderClient.userID.ToString(), false))
                                {
                                    RustEssentialsBootstrap._load.loadBans();
                                    if (!currentBans.Keys.Contains(target.displayName))
                                    {
                                        Broadcast.broadcastTo(target.networkPlayer, "You were banned! Reason: " + reason);
                                        target.Kick(NetError.Facepunch_Kick_Ban, false);
                                        Broadcast.broadcastAll("Player " + target.displayName + " was banned. Reason: " + reason);
                                        currentBans.Add(target.displayName, target.userID.ToString());
                                        currentBanReasons.Add(UID, reason);
                                        saveBans();
                                    }
                                    else
                                    {
                                        Broadcast.noticeTo(senderClient.netPlayer, "!", "Player " + target.displayName + " is already banned!");
                                    }
                                }
                                else
                                {
                                    Broadcast.noticeTo(senderClient.netPlayer, "№", "You are not allowed to /ban those of higher authority.");
                                    Broadcast.noticeTo(target.networkPlayer, "№", senderClient.userName + " tried to /ban you.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                        string reason = "";
                        List<string> reasonList = new List<string>();
                        if (args.Count() - 1 > lastIndex)
                        {
                            int curIndex = 0;
                            foreach (string s in args)
                            {
                                if (curIndex > lastIndex)
                                {
                                    reasonList.Add(s);
                                }
                                curIndex++;
                            }

                            reason = string.Join(" ", reasonList.ToArray());
                        }
                        else
                        {
                            reason = "Banned by a(n) " + findRank(senderClient.userID.ToString()) + ".";
                        }

                        if (ofLowerRank(UID.ToString(), senderClient.userID.ToString(), false))
                        {
                            RustEssentialsBootstrap._load.loadBans();
                            if (!currentBans.Values.Contains(UID))
                            {
                                Broadcast.broadcastAll("UID " + UID + " was banned. Reason: " + reason);
                                ulong UIDLong = Convert.ToUInt64(UID);
                                currentBans.Add("Unknown Player", UID);
                                currentBanReasons.Add(UID, reason);
                                saveBans();
                            }
                            else
                            {
                                Broadcast.noticeTo(senderClient.netPlayer, "!", "UID " + UID + " is already banned!");
                            }
                        }
                        else
                        {
                            Broadcast.noticeTo(senderClient.netPlayer, "№", "You are not allowed to /ban those of higher authority.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    PlayerClient[] possibleTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));
                    if (possibleTargets.Count() == 0)
                        Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + targetName + "\".");
                    else if (possibleTargets.Count() > 1)
                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + targetName + "\".");
                    else
                    {
                        NetUser target = possibleTargets[0].netUser;
                        try
                        {
                            if (target != null)
                            {
                                string reason = "";
                                List<string> reasonList = new List<string>();
                                if (args.Count() - 1 > lastIndex)
                                {
                                    int curIndex = 0;
                                    foreach (string s in args)
                                    {
                                        if (curIndex > lastIndex)
                                        {
                                            reasonList.Add(s);
                                        }
                                        curIndex++;
                                    }

                                    reason = string.Join(" ", reasonList.ToArray());
                                }
                                else
                                {
                                    reason = "Banned by a(n) " + findRank(senderClient.userID.ToString()) + ".";
                                }

                                if (ofLowerRank(target.userID.ToString(), senderClient.userID.ToString(), false))
                                {
                                    RustEssentialsBootstrap._load.loadBans();
                                    if (!currentBans.Keys.Contains(target.displayName))
                                    {
                                        Broadcast.broadcastTo(target.networkPlayer, "You were banned! Reason: " + reason);
                                        target.Kick(NetError.Facepunch_Kick_Ban, false);
                                        Broadcast.broadcastAll("Player " + target.displayName + " was banned. Reason: " + reason);
                                        currentBans.Add(target.displayName, target.userID.ToString());
                                        currentBanReasons.Add(target.userID.ToString(), reason);
                                        saveBans();
                                    }
                                    else
                                    {
                                        Broadcast.noticeTo(senderClient.netPlayer, "!", "Player " + target.displayName + " is already banned!");
                                    }
                                }
                                else
                                {
                                    Broadcast.noticeTo(senderClient.netPlayer, "№", "You are not allowed to /ban those of higher authority.");
                                    Broadcast.noticeTo(target.networkPlayer, "№", senderClient.userName + " tried to /ban you.");
                                }
                            }
                        }
                        catch (Exception ex2)
                        {

                        }
                    }
                }
            }
        }

        public static void kickPlayer(PlayerClient senderClient, string[] args, bool isBan)
        {
            if (args.Count() > 1)
            {
                bool hadQuote = false;
                string targetName = "";
                int lastIndex = 0;
                if (args[1].Contains("\""))
                {
                    foreach (string s in args)
                    {
                        lastIndex++;
                        if (s.StartsWith("\"")) hadQuote = true;
                        if (hadQuote)
                        {
                            targetName += s + " ";
                        }
                        if (s.EndsWith("\""))
                        {
                            hadQuote = false;
                            break;
                        }
                    }

                    targetName = targetName.Replace("\"", "").Trim();
                }
                else
                {
                    targetName = args[1];
                    lastIndex = 1;
                }

                PlayerClient[] possibleTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));
                if (possibleTargets.Count() == 0)
                    Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + targetName + "\".");
                else if (possibleTargets.Count() > 1)
                    Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + targetName + "\".");
                else
                {
                    NetUser target = possibleTargets[0].netUser;
                    try
                    {
                        if (target != null)
                        {
                            string reason = "";
                            List<string> reasonList = new List<string>();
                            if (args.Count() - 1 > lastIndex)
                            {
                                int curIndex = 0;
                                foreach (string s in args)
                                {
                                    if (curIndex > lastIndex)
                                    {
                                        reasonList.Add(s);
                                    }
                                    curIndex++;
                                }

                                reason = string.Join(" ", reasonList.ToArray());
                            }
                            else
                            {
                                reason = "Kicked by a(n) " + findRank(senderClient.userID.ToString()) + ".";
                            }

                            if (ofLowerRank(target.userID.ToString(), senderClient.userID.ToString(), false))
                            {
                                kickQueue.Add(target.displayName);
                                Broadcast.broadcastTo(target.networkPlayer, (isBan ? "You were banned! Reason: " : "You were kicked! Reason: ") + reason);
                                target.Kick(NetError.Facepunch_Kick_Ban, false);
                                if (!isBan)
                                    Broadcast.broadcastAll("Player " + target.displayName + " was kicked. Reason: " + reason);
                            }
                            else
                            {
                                Broadcast.noticeTo(senderClient.netPlayer, "№", "You are not allowed to /kick those of higher authority.");
                                Broadcast.noticeTo(target.networkPlayer, "№", senderClient.userName + " tried to /kick you.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        public static void whitelistKick(NetUser target, string reason)
        {
            if (target != null)
            {
                kickQueue.Add(target.displayName);
                Broadcast.broadcastTo(target.networkPlayer, "You were kicked! Reason: " + reason);
                Vars.conLog.Error("Nonwhitelisted player " + target.displayName + " (" + target.userID + ") attempted to join.");
                target.Kick(NetError.Facepunch_Kick_Ban, false);
            }
        }

        public static void kickPlayer(NetUser target, string reason, bool isBan)
        {
            if (target != null)
            {
                kickQueue.Add(target.displayName);
                Broadcast.broadcastTo(target.networkPlayer, (isBan ? "You were banned! Reason: " : "You were kicked! Reason: ") + reason);
                if (isBan)
                    Vars.conLog.Error("Banned player " + target.displayName + " (" + target.userID + ") attempted to join.");
                target.Kick(NetError.Facepunch_Kick_Ban, false);
                if (!isBan)
                {
                    Broadcast.broadcastAll("Player " + target.displayName + " was kicked. Reason: " + reason);
                    Vars.conLog.Error("Player " + target.displayName + " (" + target.userID + ") was kicked for: " + reason);
                }
            }
        }

        public static bool ofLowerRank(string rankOrUID1, string rankOrUID2, bool useRank)
        {
            int target1Level;
            int target2Level;

            if (!useRank)
            {
                target1Level = findRankPriority(findRank(rankOrUID1));
                target2Level = findRankPriority(findRank(rankOrUID2));
            }
            else
            {
                target1Level = findRankPriority(rankOrUID1);
                target2Level = findRankPriority(rankOrUID2);
            }

            if (target1Level < target2Level)
                return true;
            return false;
        }

        public static void getPlayerPos(PlayerClient senderClient)
        {
            Character senderChar;
            Character.FindByUser(senderClient.userID, out senderChar);

            string combineOutput = senderChar.transform.position.ToString();

            Broadcast.broadcastTo(senderClient.netPlayer, combineOutput);
        }

        public static void airdrop(uLink.NetworkPlayer sender, string[] args)
        {
            if (args.Count() > 1)
            {
                PlayerClient[] possibleTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userName.Contains(args[1]));
                if (possibleTargets.Count() == 0)
                    Broadcast.broadcastTo(sender, "No player names equal or contain \"" + args[1] + "\".");
                else if (possibleTargets.Count() > 1)
                    Broadcast.broadcastTo(sender, "Too many player names contain \"" + args[1] + "\".");
                else
                {
                    PlayerClient targetClient = possibleTargets[0];

                    Character targetChar;
                    Character.FindByUser(targetClient.userID, out targetChar);

                    if (Vars.announceDrops)
                        Broadcast.broadcastAll("Incoming airdrop!");

                    SupplyDropZone.CallAirDropAt(targetChar.rigidbody.position + new Vector3(UnityEngine.Random.Range(-20f, 20f), 75f, UnityEngine.Random.Range(-20f, 20f)));
                }
            }
            else
            {
                if (Vars.announceDrops)
                    Broadcast.broadcastAll("Incoming airdrop!");
                SupplyDropZone.CallAirDrop();
            }
        }

        public static void loadEnvironment()
        {
            try
            {
                Time.setTime(Convert.ToDouble(Config.startTime));
                Time.setScale(Convert.ToDouble(Config.timeScale));
                Time.freezeTime(Convert.ToBoolean(Config.freezeTime));
                
                conLog.Info("Overriding time...");
                conLog.Info("Overriding time scale...");

                if (Convert.ToBoolean(Config.freezeTime))
                    conLog.Info("Time frozen.");
            }
            catch (Exception ex)
            {
                conLog.Error("Something went wrong when overriding the enviroment! Skipping...");
            }
        }

        public static void restoreKit(object sender, ElapsedEventArgs e, PlayerClient playerClient, string kitName)
        {
            playerCooldowns.Remove(playerClient.userID.ToString());
        }

        public static void showPlayers(PlayerClient senderClient)
        {
            Broadcast.broadcastTo(senderClient.netPlayer, "All online players:", true);
            List<string> names = new List<string>();
            List<string> names2 = new List<string>();
            foreach (PlayerClient pc in PlayerClient.All.ToArray())
            {
                names.Add(pc.userName);
            }

            List<string> otherNames = new List<string>();
            while (names.Count > 0)
            {
                int curIndex = 0;
                names2.Clear();
                otherNames.Clear();
                foreach (string s in names)
                {
                    curIndex++;
                    if (curIndex < 9)
                    {
                        names2.Add(s);
                        otherNames.Add(s);
                    }
                    else
                        break;
                }
                foreach (string s in otherNames)
                {
                    names.Remove(s);
                }
                Broadcast.broadcastTo(senderClient.netPlayer, string.Join(", ", names2.ToArray()), true);
            }
        }

        public static void showRules(PlayerClient senderClient)
        {
            if (motdList.ContainsKey("Rules"))
            {
                foreach (string s in motdList["Rules"])
                {
                    Broadcast.broadcastTo(senderClient.netPlayer, s);
                }
            }
        }

        public static void showKits(PlayerClient senderClient)
        {
            string rank = Vars.findRank(senderClient.userID.ToString());

            Broadcast.broadcastTo(senderClient.netPlayer, "Available kits:", true);
            Vars.listKits(rank, senderClient);
        }

        public static void giveKit(PlayerClient senderClient, string[] args, string message)
        {
            try
            {
                if (args.Count() > 1)
                {
                    string kitName = args[1];
                    string kitNameToLower = kitName.ToLower();
                    Dictionary<string, int> kitItems = new Dictionary<string,int>();
                    if (kits.Keys.Contains(kitNameToLower)) // If kit exists
                    {
                        if (kitsForRanks.Keys.Contains(findRank(senderClient.userID.ToString()))) // If kit exist for my rank
                        {
                            if (kitsForRanks[findRank(senderClient.userID.ToString())].Contains(kitNameToLower)) // If the kit I requested is permitted for my rank
                            {
                                bool b = true;
                                if (playerCooldowns.Keys.Contains(senderClient.userID.ToString()))
                                {
                                    if (playerCooldowns[senderClient.userID.ToString()].Values.Contains(kitNameToLower))
                                        b = false;
                                }

                                if (b) // If I am not on cool down for this kit
                                {
                                    kitItems = kits[kitNameToLower];
                                    Broadcast.noticeTo(senderClient.netPlayer, "☻", "You were given the " + kitName + " kit.");

                                    if (kitCooldowns.Keys.Contains(kitNameToLower)) // If a cooldown is set for this kit, set my cool down
                                    {
                                        TimerPlus t = new TimerPlus();
                                        t.AutoReset = false;
                                        t.Interval = kitCooldowns[kitNameToLower];
                                        t.Elapsed += (sender, e) => restoreKit(sender, e, senderClient, kitNameToLower);
                                        t.Start();

                                        if (!playerCooldowns.Keys.Contains(senderClient.userID.ToString()))
                                            playerCooldowns.Add(senderClient.userID.ToString(), new Dictionary<TimerPlus, string>() { { t, kitNameToLower } });
                                        else
                                            playerCooldowns[senderClient.userID.ToString()].Add(t, kitNameToLower);
                                    }
                                }
                                else // If I am on cool down
                                {
                                    foreach (KeyValuePair<TimerPlus, string> kv in playerCooldowns[senderClient.userID.ToString()])
                                    {
                                        if (kv.Value == kitNameToLower)
                                        {
                                            // Return how long I have to wait
                                            double timeLeft = Math.Round((kv.Key.TimeLeft / 1000));
                                            Broadcast.noticeTo(senderClient.netPlayer, "✯", "You must wait " + (timeLeft > 999999999 ? "forever" : timeLeft.ToString()) + " seconds before using this.");
                                        }
                                    }
                                }
                            }
                            else // If I am not allowed to use this kit
                            {
                                Broadcast.noticeTo(senderClient.netPlayer, ":(", "You do not have permission to do this.");
                            }
                        }
                        else // If I do not have any kits assigned to my rank
                        {
                            if (unassignedKits.Contains(kitNameToLower)) // If the kit is actually unassigned to a rank
                            {
                                bool b = true;
                                if (playerCooldowns.Keys.Contains(senderClient.userID.ToString()))
                                {
                                    if (playerCooldowns[senderClient.userID.ToString()].Values.Contains(kitNameToLower))
                                        b = false;
                                }

                                if (b) // If I am not on cool down for this kit
                                {
                                    kitItems = kits[kitNameToLower];
                                    Broadcast.noticeTo(senderClient.netPlayer, "☻", "You were given the " + kitName + " kit.");

                                    if (kitCooldowns.Keys.Contains(kitNameToLower)) // If a cooldown is set for this kit, set my cool down
                                    {
                                        TimerPlus t = new TimerPlus();
                                        t.AutoReset = false;
                                        t.Interval = kitCooldowns[kitNameToLower];
                                        t.Elapsed += (sender, e) => restoreKit(sender, e, senderClient, kitNameToLower);
                                        t.Start();

                                        if (!playerCooldowns.Keys.Contains(senderClient.userID.ToString()))
                                            playerCooldowns.Add(senderClient.userID.ToString(), new Dictionary<TimerPlus, string>() { { t, kitNameToLower } });
                                        else
                                            playerCooldowns[senderClient.userID.ToString()].Add(t, kitNameToLower);
                                    }
                                }
                                else // If I am on cool down
                                {
                                    foreach (KeyValuePair<TimerPlus, string> kv in playerCooldowns[senderClient.userID.ToString()])
                                    {
                                        if (kv.Value == kitNameToLower)
                                        {
                                            // Return how long I have to wait
                                            double timeLeft = Math.Round((kv.Key.TimeLeft / 1000));
                                            Broadcast.noticeTo(senderClient.netPlayer, "✯", "You must wait " + (timeLeft > 999999999 ? "forever" : timeLeft.ToString()) + " seconds before using this.");
                                        }
                                    }
                                }
                            }
                            else // If the kit is truly assigned to a rank, but not mine
                            {
                                Broadcast.noticeTo(senderClient.netPlayer, ":(", "You do not have permission to do this.");
                            }
                        }
                    }
                    List<string> parameters;
                    foreach (KeyValuePair<string, int> kv in kitItems)
                    {
                        parameters = new List<string>()
                        {
                            { "/i" },
                            { "\"" + kv.Key + "\"" },
                            { kv.Value.ToString() }
                        };
                        createItem(senderClient, senderClient, parameters.ToArray(), string.Join(" ", parameters.ToArray()), false);
                    }
                }
            } catch (Exception ex)
            {
                Broadcast.broadcastTo(senderClient.netPlayer, ex.ToString());
            }
        }

        public static void createItem(PlayerClient senderClient, string[] args, string message)
        {
            try
            {
                if (args.Count() > 1)
                {
                    if (args[1].Contains("\""))
                    {
                        bool hadQuote = false;
                        string targetName = "";
                        int lastIndex = 0;
                        List<string> splitName = new List<string>();
                        foreach (string s in args)
                        {
                            if (s.StartsWith("\"")) hadQuote = true;
                            if (hadQuote)
                            {
                                targetName += s + " ";
                                splitName.Add(s);
                            }
                            lastIndex++;
                            if (s.EndsWith("\""))
                            {
                                hadQuote = false;
                                break;
                            }
                        }

                        targetName = targetName.Replace("\"", "").Trim();

                        PlayerClient[] possibleTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));
                        if (possibleTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + targetName + "\".");
                        else if (possibleTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + targetName + "\".");
                        else
                        {
                            PlayerClient targetClient = possibleTargets[0];
                            List<string> newArgs = new List<string>();
                            foreach (string s in args)
                            {
                                if (!splitName.Contains(s))
                                {
                                    newArgs.Add(s);
                                }
                            }
                            createItem(senderClient, targetClient, args, message, true);
                        }
                    }
                    else
                    {
                        PlayerClient[] possibleTargets = Array.FindAll(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.userName.Contains(args[1]));
                        if (possibleTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + args[1] + "\".");
                        else if (possibleTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + args[1] + "\".");
                        else
                        {
                            PlayerClient targetClient = possibleTargets[0];
                            List<string> newArgs = new List<string>();
                            foreach (string s in args)
                            {
                                if (s != args[1])
                                {
                                    newArgs.Add(s);
                                }
                            }
                            createItem(senderClient, targetClient, args, message, true);
                        }
                    }
                }
            } catch (Exception ex)
            {
                Broadcast.broadcastTo(senderClient.netPlayer, ex.ToString());
            }
        }

        public static void createItem(PlayerClient senderClient, PlayerClient targetClient, string[] args, string message, bool b)
        {
            try
            {
                if (args.Count() > 1)
                {
                    if (message.Contains("\""))
                    {
                        bool hadQuote = false;
                        string itemName = "";
                        int lastIndex = -1;
                        List<string> playerNameList = new List<string>();
                        foreach (string s in args)
                        {
                            lastIndex++;
                            if (s.StartsWith("\"")) hadQuote = true;
                            if (hadQuote)
                                playerNameList.Add(s);
                            if (s.EndsWith("\""))
                            {
                                hadQuote = false;
                                break;
                            }
                        }
                        itemName = string.Join(" ", playerNameList.ToArray()).Replace("\"", "").Trim();
                        if (!Vars.itemIDs.Values.Contains(itemName))
                            Broadcast.broadcastTo(senderClient.netPlayer, "No such item name \"" + itemName + "\".");
                        else
                        {
                            int amount = 1;
                            if (args.Count() - 1 > lastIndex)
                            {
                                try
                                {
                                    amount = Convert.ToInt16(args[lastIndex + 1]);
                                }
                                catch (Exception ex) { Broadcast.broadcastTo(senderClient.netPlayer, "Amount must be an integer!"); }
                            }

                            if (amount > 0)
                            {
                                ConsoleSystem.Run("inv.giveplayer \"" + targetClient.userName + "\" \"" + itemName + "\" " + amount);
                                if (b)
                                {
                                    if (senderClient != targetClient)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "You gave " + targetClient.userName + " " + amount + " " + itemName);
                                    Broadcast.noticeTo(targetClient.netPlayer, "☻", "You were given " + amount + " " + itemName + " by " + senderClient.userName);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (args.Count() > 1)
                        {
                            int itemID = 0;
                            string itemName = "";
                            try
                            {
                                itemID = Convert.ToInt16(args[1]);
                                itemName = Vars.itemIDs[itemID];
                            }
                            catch (Exception ex)
                            {
                                itemName = args[1];
                                if (!Vars.itemIDs.Values.Contains(itemName))
                                    Broadcast.broadcastTo(senderClient.netPlayer, "No such item name \"" + itemName + "\".");
                            }

                            int amount = 1;
                            if (args.Count() > 2)
                            {
                                try
                                {
                                    amount = Convert.ToInt16(args[2]);
                                }
                                catch (Exception ex) { Broadcast.broadcastTo(senderClient.netPlayer, "Amount must be an integer!"); }
                            }

                            if (Vars.itemIDs.Values.Contains(itemName) && amount > 0)
                            {
                                ConsoleSystem.Run("inv.giveplayer \"" + targetClient.userName + "\" \"" + itemName + "\" " + amount);
                                if (b)
                                {
                                    if (senderClient != targetClient)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "You gave " + targetClient.userName + " " + amount + " " + itemName);
                                    Broadcast.noticeTo(targetClient.netPlayer, "☻", "You were given " + amount + " " + itemName + " by " + senderClient.userName);
                                }
                            }
                        }
                    }
                }
            } catch (Exception ex)
            {
                Broadcast.broadcastTo(senderClient.netPlayer, ex.ToString());
            }
        }
    }
}
