/**
 * @file: Vars.cs
 * @author: Team Cerionn (https://github.com/Team-Cerionn)

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
using Facepunch;
using UnityEngine;
//using MySql.Data;
//using MySql.Data.MySqlClient;

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
        public static string tablesDir = Path.Combine(saveDir, "Tables");
        public static string cfgFile = Path.Combine(saveDir, "config.ini");
        public static string whiteListFile = Path.Combine(saveDir, "whitelist.txt");
        public static string ranksFile = Path.Combine(saveDir, "ranks.ini");
        public static string commandsFile = Path.Combine(saveDir, "commands.ini");
        public static string allCommandsFile = Path.Combine(saveDir, "allCommands.txt");
        public static string itemsFile = Path.Combine(saveDir, "itemIDs.txt");
        public static string kitsFile = Path.Combine(saveDir, "kits.ini");
        public static string motdFile = Path.Combine(saveDir, "motd.ini");
        public static string bansFile = Path.Combine(saveDir, "bans.txt");
        public static string prefixFile = Path.Combine(saveDir, "prefix.ini");
        public static string warpsFile = Path.Combine(saveDir, "warps.ini");
        public static string doorsFile = Path.Combine(saveDir, "door_data.dat");
        public static string factionsFile = Path.Combine(saveDir, "factions.dat");
        public static string alliesFile = Path.Combine(saveDir, "allies.dat");
        public static string cooldownsFile = Path.Combine(saveDir, "kit_cooldowns.dat");
        public static string requestCooldownsFile = Path.Combine(saveDir, "tpaPer_cooldowns.dat");
        public static string requestCooldownsAllFile = Path.Combine(saveDir, "tpaAll_cooldowns.dat");
        public static string itemControllerFile = Path.Combine(saveDir, "controller.ini");
        public static string zonesFile = Path.Combine(saveDir, "zones.dat");
        public static string defaultRank = "Default";
        public static string currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, 5);
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
        public static bool directChat = false;
        public static bool globalChat = true;
        public static bool logPluginChat = true;
        public static bool unknownCommand = true;
        public static bool enableJoin = true;
        public static bool enableLeave = true;
        public static bool teleportRequestOn = true;
        public static bool removeTag = false;
        public static bool nextToName = true;
        public static bool removePrefix = true;
        public static bool inheritCommands = true;
        public static bool inheritKits = true;
        public static bool inheritWarps = true;
        public static bool suicideMessages = false;
        public static bool murderMessages = true;
        public static bool accidentMessages = true;
        public static bool fallDamage = true;
        public static bool restrictChars = true;
        public static bool kickDuplicate = false;
        public static bool lowerAuthority = false;
        public static bool censorship = false;
        public static bool friendlyFire = false;
        public static bool alliedFire = false;
        public static bool enableRepair = true;
        public static bool forceNudity = false;
        public static bool denyRequestWarzone = true;
        public static bool doorStops = true;
        public static bool researchAtBench = true;
        public static bool infiniteResearch = false;
        public static bool researchPaper = false;
        public static bool craftAtBench = true;

        public static string whitelistKickCMD = "Whitelist was enabled and you are not whitelisted.";
        public static string whitelistKickJoin = "You are not whitelisted!";
        public static string whitelistCheckGood = "You are whitelisted!";
        public static string whitelistCheckBad = "You are not whitelisted!";
        public static string defaultChat = "global";
        public static string botName = "Essentials";
        public static string joinMessage = "Player $USER$ has joined.";
        public static string leaveMessage = "Player $USER$ has left.";
        public static string steamGroup = "";
        public static string suicideMessage = "$VICTIM$ killed himself.";
        public static string murderMessage = "$KILLER$ [$WEAPON$ ($PART$)] $VICTIM$";
        public static string murderMessageUnknown = "$KILLER$ killed $VICTIM$.";
        public static string accidentMessage = "$VICTIM$ got mauled by a $KILLER$.";

        public static List<string> allowedChars = new List<string>()
        {
            { "a" },{ "b" },{ "c" },{ "d" },{ "e" },{ "f" },{ "g" },{ "h" },{ "i" },{ "j" },
            { "k" },{ "l" },{ "m" },{ "n" },{ "o" },{ "p" },{ "q" },{ "r" },{ "s" },{ "t" },
            { "u" },{ "v" },{ "w" },{ "x" },{ "y" },{ "z" },{ "1" },{ "2" },{ "3" },{ "4" },
            { "5" },{ "6" },{ "7" },{ "8" },{ "9" },{ "0" },{ "`" },{ "-" },{ "=" },{ "'" },
            { "." },{ "[" },{ "]" },{ "(" },{ ")" },{ "{" },{ "}" },{ "~" },{ "_" }
        };
        public static List<string> illegalWords = new List<string>()
        {
            { "fuck" },{ "shit" },{ "cunt" },{ "bitch" },{ "pussy" },{ "slut" },{ "whore" },
            { "ass" }
        };

        public static int refreshInterval = 15000;
        public static int directDistance = 150;
        public static int chatLogCap = 15;
        public static int logCap = 15;
        public static int maxMembers = 15;
        public static int minimumNameCount = 2;
        public static int maximumNameCount = 15;
        public static int requestDelay = 10;
        public static int warpDelay = 10;
        public static int requestCooldownType = 0;
        public static int requestCooldown = 900000;

        public static float neutralDamage = 1f;
        public static float warDamage = 1f;
        public static float warFriendlyDamage = 0f;
        public static float warAllyDamage = 0.70f;
        // SAVED VARIABLES END

        //public static MySqlConnection mysqlConnection;

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
            { alliesFile },
            { cooldownsFile },
            { requestCooldownsFile },
            { requestCooldownsAllFile },
            { bansFile },
            { prefixFile },
            { warpsFile },
            { zonesFile },
            { itemControllerFile }
        };
        public static List<string> allDirs = new List<string>()
        {
            { rootDir },
            { saveDir },
            { logsDir },
            { tablesDir }
        };
        public static List<string> whitelist = new List<string>();
        public static List<string> totalCommands = new List<string>();
        public static List<string> inGlobal = new List<string>();
        public static List<string> inDirect = new List<string>();
        public static List<string> inFaction = new List<string>();
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
        public static List<string> destroyerAllList = new List<string>();
        public static List<string> ownershipList = new List<string>();
        public static List<string> hiddenList = new List<string>();
        public static List<string> unassignedWarps = new List<string>();
        public static List<string> unassignedKits = new List<string>();
        public static List<string> emptyPrefixes = new List<string>();
        public static List<string> vanishedList = new List<string>();
        public static List<string> buildList = new List<string>();
        public static List<string> craftList = new List<string>();
        public static List<string> restrictItems = new List<string>();
        public static List<string> restrictCrafting = new List<string>();
        public static List<string> restrictResearch = new List<string>();
        public static List<string> permitResearch = new List<string>();
        public static List<string> restrictBlueprints = new List<string>();
        public static List<string> lastWinners = new List<string>();
        public static List<HostileWildlifeAI> ignoringAIList = new List<HostileWildlifeAI>();
        public static List<PlayerClient> AllPlayerClients = new List<PlayerClient>();
        public static Dictionary<string, Zone> safeZones = new Dictionary<string, Zone>();
        public static Dictionary<string, Zone> warZones = new Dictionary<string, Zone>();
        public static Dictionary<PlayerClient, Vector2> firstPoints = new Dictionary<PlayerClient, Vector2>();
        public static Dictionary<PlayerClient, Vector2> secondPoints = new Dictionary<PlayerClient, Vector2>();
        public static Dictionary<PlayerClient, Vector2> thirdPoints = new Dictionary<PlayerClient, Vector2>();
        public static Dictionary<PlayerClient, Vector2> forthPoints = new Dictionary<PlayerClient, Vector2>();
        public static Dictionary<PlayerClient, string> inWarZone = new Dictionary<PlayerClient, string>();
        public static Dictionary<PlayerClient, string> inSafeZone = new Dictionary<PlayerClient, string>();
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
        public static Dictionary<string, TimerPlus> blockedRequestsAll = new Dictionary<string, TimerPlus>();
        public static Dictionary<string, List<string>> previousArmor = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> rankList = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> motdList = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> enabledCommands = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> kitsForRanks = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> kitsForUIDs = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> warpsForRanks = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> warpsForUIDs = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> factionInvites = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> alliances = new Dictionary<string, List<string>>();
        public static Dictionary<string, LootSpawnList> originalLootTables = new Dictionary<string, LootSpawnList>();
        public static OrderedDictionary historyFaction = new OrderedDictionary();
        public static Dictionary<string, TimerPlus> muteTimes = new Dictionary<string, TimerPlus>();
        public static Dictionary<PlayerClient, PlayerClient> latestPM = new Dictionary<PlayerClient, PlayerClient>();
        public static Dictionary<PlayerClient, PlayerClient> latestRequests = new Dictionary<PlayerClient, PlayerClient>();
        public static Dictionary<PlayerClient, PlayerClient> latestFactionRequests = new Dictionary<PlayerClient, PlayerClient>();
        public static Dictionary<PlayerClient, Dictionary<PlayerClient, TimerPlus>> teleportRequests = new Dictionary<PlayerClient, Dictionary<PlayerClient, TimerPlus>>();
        public static Dictionary<string, Dictionary<string, TimerPlus>> blockedRequestsPer = new Dictionary<string, Dictionary<string, TimerPlus>>();
        public static Dictionary<string, Dictionary<string, string>> factions = new Dictionary<string, Dictionary<string, string>>();
        public static Dictionary<string, Dictionary<string, string>> factionsByNames = new Dictionary<string, Dictionary<string, string>>();
        public static Dictionary<int, string> itemIDs = new Dictionary<int, string>();
        public static Dictionary<string, Dictionary<string, int>> kits = new Dictionary<string, Dictionary<string, int>>();
        public static Dictionary<string, Vector3> warps = new Dictionary<string, Vector3>();
        public static Dictionary<string, Dictionary<string, List<string>>> cycleMOTDList = new Dictionary<string, Dictionary<string, List<string>>>();
        public static Dictionary<string, Dictionary<string, List<string>>> onceMOTDList = new Dictionary<string, Dictionary<string, List<string>>>();
        public static Dictionary<string, int> kitCooldowns = new Dictionary<string, int>();
        public static Dictionary<string, int> warpCooldowns = new Dictionary<string, int>();
        public static Dictionary<string, Dictionary<TimerPlus, string>> playerCooldowns = new Dictionary<string, Dictionary<TimerPlus, string>>();
        public static Dictionary<string, StringBuilder> textForFiles = new Dictionary<string, StringBuilder>()
        {
            { cfgFile, cfgText() },
            { ranksFile, ranksText() },
            { commandsFile, commandsText() },
            { allCommandsFile, allCommandsText() },
            { kitsFile, kitsText() },
            { motdFile, motdText() },
            { prefixFile, prefixText() },
            { warpsFile, warpsText() },
            { itemControllerFile, itemControllerText() }
        };

        public static string filterNames(string playerName, string uid)
        {
            if (!emptyPrefixes.Contains(uid))
            {
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
            }

            return playerName;
        }

        public static void SetDeathReason(PlayerClient playerClient, ref DamageEvent damage)
        {
            try
            {   
                if (playerClient != null)
                {
                    if (playerClient.netPlayer != null)
                    {
                        if (NetCheck.PlayerValid(playerClient.netPlayer))
                        {
                            IDMain idMain = damage.attacker.idMain;
                            string message = "";
                            if (idMain != null)
                            {
                                idMain = idMain.idMain;
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
                                        if (wasHit.Contains(playerClient))
                                        {
                                            wasHit.Remove(playerClient);
                                        }
                                        if (extraData != null)
                                        {
                                            if (Vars.murderMessages)
                                            {
                                                message = Vars.murderMessage.Replace("$VICTIM$", playerClient.userName).Replace("$KILLER$", playerControlledController.playerClient.userName).Replace("$WEAPON$", extraData.dataBlock.name).Replace("$PART$", BodyParts.GetNiceName(damage.bodyPart)).Replace("$DISTANCE$", Convert.ToString(distance) + "m");

                                                Broadcast.broadcastAll(message);
                                            }
                                            DeathScreen.SetReason(playerClient.netPlayer, playerControlledController.playerClient.userName + " killed you using a " + extraData.dataBlock.name + " with a hit to your " + BodyParts.GetNiceName(damage.bodyPart));
                                            return;
                                        }
                                        if (Vars.murderMessages)
                                        {
                                            message = Vars.murderMessageUnknown.Replace("$VICTIM$", playerClient.userName).Replace("$KILLER$", playerControlledController.playerClient.userName);

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
                                    case "MutantBear":
                                        killer = "Mutant Bear";
                                        break;
                                    case "MutantWolf":
                                        killer = "Mutant Wolf";
                                        break;
                                }
                                Character victimChar2;
                                Character.FindByUser(playerClient.userID, out victimChar2);

                                if (Vars.accidentMessages)
                                {
                                    message = Vars.accidentMessage.Replace("$VICTIM$", victimChar2.netUser.displayName).Replace("$KILLER$", killer);

                                    Broadcast.broadcastAll(message);
                                }

                                DeathScreen.SetReason(playerClient.netPlayer, "You were killed by a " + killer);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("SDR: " + ex.ToString());
            }
        }

        public static void FallImpact(float fallspeed, float min_vel, float max_vel, float healthFraction, Character idMain, float maxHealth, FallDamage fallDamage)
        {
            try
            {
                if (Vars.fallDamage)
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

                            if (!factions.ContainsKey(factionName) && !factionsByNames.ContainsKey(factionName) && !alliances.ContainsKey(factionName))
                            {
                                if (possibleFactions.Count() == 0)
                                {
                                    if (factionName.Length < 16)
                                    {
                                        if (!factionName.Contains("=") && !factionName.Contains(";") && !factionName.Contains(":"))
                                        {
                                            factions.Add(factionName, new Dictionary<string, string>());
                                            factionsByNames.Add(factionName, new Dictionary<string, string>());
                                            alliances.Add(factionName, new List<string>());
                                            factions[factionName].Add(senderClient.userID.ToString(), "owner");
                                            factionsByNames[factionName].Add(senderClient.userID.ToString(), senderClient.userName);
                                            Broadcast.broadcastTo(senderClient.netPlayer, "Faction [" + factionName + "] created.");
                                            addFactionData(factionName, senderClient.userName, senderClient.userID.ToString(), "owner");
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

                            if (rank == "owner" || completeDoorAccess.Contains(senderClient.userID.ToString()))
                            {
                                PlayerClient[] targetClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                foreach (PlayerClient pc in targetClients)
                                {
                                    Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, "Your faction was disbanded.");
                                }
                                factions.Remove(possibleFactions[0].Key);
                                factionsByNames.Remove(possibleFactions[0].Key);
                                alliances.Remove(possibleFactions[0].Key);
                                remFactionData(possibleFactions[0].Key, "disband", "");
                                remAlliesData(possibleFactions[0].Key, "disband");
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

                                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(targetName));

                                    if (possibleTargets.Count() == 0)
                                    {
                                        List<string> possibleUIDs = new List<string>();
                                        foreach (KeyValuePair<string, string> kv in factionsByNames[possibleFactions[0].Key])
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
                                            PlayerClient[] targetClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                            foreach (PlayerClient pc in targetClients)
                                            {
                                                Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, factionsByNames[possibleFactions[0].Key][possibleUIDs[0]] + " was kicked from the faction.");
                                            }
                                            remFactionData(possibleFactions[0].Key, factionsByNames[possibleFactions[0].Key][possibleUIDs[0]], possibleFactions[0].Value[possibleUIDs[0]]);
                                            factions[possibleFactions[0].Key].Remove(possibleUIDs[0]);
                                            factionsByNames[possibleFactions[0].Key].Remove(possibleUIDs[0]);
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
                                                PlayerClient[] targetClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                                foreach (PlayerClient pc in targetClients)
                                                {
                                                    Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, targetClient.userName + " was kicked from the faction.");
                                                }
                                                remFactionData(possibleFactions[0].Key, targetClient.userName, possibleFactions[0].Value[targetClient.userID.ToString()]);
                                                factions[possibleFactions[0].Key].Remove(targetClient.userID.ToString());
                                                factionsByNames[possibleFactions[0].Key].Remove(targetClient.userID.ToString());
                                                if (latestFactionRequests.ContainsKey(targetClient))
                                                    latestFactionRequests.Remove(targetClient);
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
                                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));

                                    if (possibleTargets.Count() == 0)
                                    {
                                        List<string> possibleUIDs = new List<string>();
                                        foreach (KeyValuePair<string, string> kv in factionsByNames[possibleFactions[0].Key])
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
                                            PlayerClient[] targetClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                            foreach (PlayerClient pc in targetClients)
                                            {
                                                Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, factionsByNames[possibleFactions[0].Key][possibleUIDs[0]] + " was kicked from the faction.");
                                            }
                                            remFactionData(possibleFactions[0].Key, factionsByNames[possibleFactions[0].Key][possibleUIDs[0]], possibleFactions[0].Value[possibleUIDs[0]]);
                                            factions[possibleFactions[0].Key].Remove(possibleUIDs[0]);
                                            factionsByNames[possibleFactions[0].Key].Remove(possibleUIDs[0]);
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
                                                PlayerClient[] targetClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                                foreach (PlayerClient pc in targetClients)
                                                {
                                                    Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, targetClient.userName + " was kicked from the faction.");
                                                }
                                                remFactionData(possibleFactions[0].Key, targetClient.userName, possibleFactions[0].Value[targetClient.userID.ToString()]);
                                                factions[possibleFactions[0].Key].Remove(targetClient.userID.ToString());
                                                factionsByNames[possibleFactions[0].Key].Remove(targetClient.userID.ToString());
                                                if (latestFactionRequests.ContainsKey(targetClient))
                                                    latestFactionRequests.Remove(targetClient);
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

                                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(targetName));

                                    if (possibleTargets.Count() == 0)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "No players equal \"" + targetName + "\".");
                                    else if (possibleTargets.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + targetName + "\".");
                                    else
                                    {
                                        PlayerClient targetClient = possibleTargets[0];

                                        if (possibleFactions[0].Value.Count < maxMembers)
                                        {
                                            if (!possibleFactions[0].Value.ContainsKey(targetClient.userID.ToString()))
                                            {
                                                if (!factionInvites.ContainsKey(targetClient.userID.ToString()))
                                                {
                                                    factionInvites.Add(targetClient.userID.ToString(), new List<string>() { { possibleFactions[0].Key } });

                                                    Broadcast.broadcastTo(senderClient.netPlayer, "You invited \"" + targetClient.userName + "\" to the faction.");
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

                                                        Broadcast.broadcastTo(senderClient.netPlayer, "You invited \"" + targetClient.userName + "\" to the faction.");
                                                        Broadcast.broadcastTo(targetClient.netPlayer, "You were invited to the faction \"" + possibleFactions[0].Key + "\".");
                                                        if (!latestFactionRequests.ContainsKey(targetClient))
                                                            latestFactionRequests.Add(targetClient, senderClient);
                                                        else
                                                            latestFactionRequests[targetClient] = senderClient;
                                                    }
                                                    else
                                                        Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, targetClient.userName + " has already been invited.");
                                                }
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
                                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));

                                    if (possibleTargets.Count() == 0)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "No players equal or contain \"" + targetName + "\".");
                                    else if (possibleTargets.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + targetName + "\".");
                                    else
                                    {
                                        PlayerClient targetClient = possibleTargets[0];

                                        if (possibleFactions[0].Value.Count < maxMembers)
                                        {
                                            KeyValuePair<string, Dictionary<string, string>>[] targetFactions = Array.FindAll(factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(targetClient.userID.ToString()));
                                            if (targetFactions.Count() == 0)
                                            {
                                                if (!factionInvites.ContainsKey(targetClient.userID.ToString()))
                                                {
                                                    factionInvites.Add(targetClient.userID.ToString(), new List<string>() { { possibleFactions[0].Key } });

                                                    Broadcast.broadcastTo(senderClient.netPlayer, "You invited " + targetClient.userName + " to the faction.");
                                                    Broadcast.broadcastTo(targetClient.netPlayer, "You were invited to the faction [" + possibleFactions[0].Key + "].");
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

                                                        Broadcast.broadcastTo(senderClient.netPlayer, "You invited " + targetClient.userName + " to the faction.");
                                                        Broadcast.broadcastTo(targetClient.netPlayer, "You were invited to the faction [" + possibleFactions[0].Key + "].");
                                                        if (!latestFactionRequests.ContainsKey(targetClient))
                                                            latestFactionRequests.Add(targetClient, senderClient);
                                                        else
                                                            latestFactionRequests[targetClient] = senderClient;
                                                    }
                                                    else
                                                        Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, targetClient.userName + " has already been invited.");
                                                }
                                            }
                                            else
                                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, targetClient.userName + " is already in the faction [" + targetFactions[0] + "].");
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
                                        Broadcast.broadcastTo(senderClient.netPlayer, "No invitations from factions that equal or contain \"" + targetFaction + "\".");
                                    else if (inviterFactions.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many invitations from factions that names contain \"" + targetFaction + "\".");
                                    else
                                    {
                                        factionInvites[senderClient.userID.ToString()].Remove(inviterFactions[0]);
                                        factions[inviterFactions[0]].Add(senderClient.userID.ToString(), "normal");
                                        factionsByNames[inviterFactions[0]].Add(senderClient.userID.ToString(), senderClient.userName);
                                        PlayerClient[] targetClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => factions[inviterFactions[0]].ContainsKey(pc.userID.ToString()));
                                        foreach (PlayerClient pc in targetClients)
                                        {
                                            Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + inviterFactions[0], senderClient.userName + " has joined the faction.");
                                        }
                                        addFactionData(inviterFactions[0], senderClient.userName, senderClient.userID.ToString(), "normal");
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        if (latestFactionRequests.ContainsKey(senderClient))
                                        {
                                            KeyValuePair<string, Dictionary<string, string>> inviterFaction = Array.Find(factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(latestFactionRequests[senderClient].userID.ToString()));
                                            KeyValuePair<string, Dictionary<string, string>> inviterFactionByName = Array.Find(factionsByNames.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(latestFactionRequests[senderClient].userID.ToString()));
                                            factionInvites[senderClient.userID.ToString()].Remove(inviterFaction.Key);
                                            inviterFaction.Value.Add(senderClient.userID.ToString(), "normal");
                                            inviterFactionByName.Value.Add(senderClient.userID.ToString(), senderClient.userName);
                                            PlayerClient[] targetClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => inviterFaction.Value.ContainsKey(pc.userID.ToString()));
                                            foreach (PlayerClient pc in targetClients)
                                            {
                                                Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + inviterFaction.Key, senderClient.userName + " has joined the faction.");
                                            }
                                            addFactionData(inviterFaction.Key, senderClient.userName, senderClient.userID.ToString(), "normal");
                                        }
                                        else
                                            Broadcast.broadcastTo(senderClient.netPlayer, "No invitations from any factions.");
                                    }
                                    catch (Exception ex)
                                    {
                                        Vars.conLog.Error(ex.ToString());
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
                                    PlayerClient[] targetClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                    foreach (PlayerClient pc in targetClients)
                                    {
                                        Broadcast.broadcastCustomTo(pc.netPlayer, possibleFactions[0].Key, senderClient.userName + " has left the faction.");
                                    }
                                    try
                                    {
                                        remFactionData(possibleFactions[0].Key, senderClient.userName, possibleFactions[0].Value[senderClient.userID.ToString()]);
                                        factions[possibleFactions[0].Key].Remove(senderClient.userID.ToString());
                                        factionsByNames[possibleFactions[0].Key].Remove(senderClient.userID.ToString());
                                        if (latestFactionRequests.ContainsKey(senderClient))
                                            latestFactionRequests.Remove(senderClient);
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
                        }
                        catch (Exception ex)
                        {
                            Vars.conLog.Error(ex.ToString());
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

                                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(targetName));

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
                                                remFactionData(possibleFactions[0].Key, targetClient.userName, possibleFactions[0].Value[targetClient.userID.ToString()]);
                                                possibleFactions[0].Value.Remove(targetClient.userID.ToString());
                                                possibleFactions[0].Value.Add(targetClient.userID.ToString(), "admin");
                                                addFactionData(possibleFactions[0].Key, targetClient.userName, targetClient.userID.ToString(), "admin");
                                                PlayerClient[] targetClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
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
                                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));

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
                                                remFactionData(possibleFactions[0].Key, targetClient.userName, possibleFactions[0].Value[targetClient.userID.ToString()]);
                                                possibleFactions[0].Value.Remove(targetClient.userID.ToString());
                                                possibleFactions[0].Value.Add(targetClient.userID.ToString(), "admin");
                                                addFactionData(possibleFactions[0].Key, targetClient.userName, targetClient.userID.ToString(), "admin");
                                                PlayerClient[] targetClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
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

                                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(targetName));

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
                                                remFactionData(possibleFactions[0].Key, targetClient.userName, possibleFactions[0].Value[targetClient.userID.ToString()]);
                                                possibleFactions[0].Value.Remove(targetClient.userID.ToString());
                                                possibleFactions[0].Value.Add(targetClient.userID.ToString(), "normal");
                                                addFactionData(possibleFactions[0].Key, targetClient.userName, targetClient.userID.ToString(), "normal");
                                                PlayerClient[] targetClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
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
                                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));

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
                                                remFactionData(possibleFactions[0].Key, targetClient.userName, possibleFactions[0].Value[targetClient.userID.ToString()]);
                                                possibleFactions[0].Value.Remove(targetClient.userID.ToString());
                                                possibleFactions[0].Value.Add(targetClient.userID.ToString(), "normal");
                                                addFactionData(possibleFactions[0].Key, targetClient.userName, targetClient.userID.ToString(), "normal");
                                                PlayerClient[] targetClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
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

                                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(targetName));

                                    if (possibleTargets.Count() == 0)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "No players equal \"" + targetName + "\".");
                                    else if (possibleTargets.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + targetName + "\".");
                                    else
                                    {
                                        PlayerClient targetClient = possibleTargets[0];

                                        if (targetClient != senderClient)
                                        {
                                            remFactionData(possibleFactions[0].Key, senderClient.userName, possibleFactions[0].Value[senderClient.userID.ToString()]);
                                            possibleFactions[0].Value.Remove(senderClient.userID.ToString());

                                            remFactionData(possibleFactions[0].Key, targetClient.userName, possibleFactions[0].Value[targetClient.userID.ToString()]);
                                            possibleFactions[0].Value.Remove(targetClient.userID.ToString());
                                            possibleFactions[0].Value.Add(targetClient.userID.ToString(), "owner");
                                            addFactionData(possibleFactions[0].Key, targetClient.userName, targetClient.userID.ToString(), "owner");

                                            possibleFactions[0].Value.Add(senderClient.userID.ToString(), "normal");
                                            addFactionData(possibleFactions[0].Key, senderClient.userName, targetClient.userID.ToString(), "normal");

                                            PlayerClient[] targetClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
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
                                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));

                                    if (possibleTargets.Count() == 0)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "No players equal or contain \"" + targetName + "\".");
                                    else if (possibleTargets.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + targetName + "\".");
                                    else
                                    {
                                        PlayerClient targetClient = possibleTargets[0];

                                        if (targetClient != senderClient)
                                        {
                                            remFactionData(possibleFactions[0].Key, senderClient.userName, possibleFactions[0].Value[senderClient.userID.ToString()]);
                                            possibleFactions[0].Value.Remove(senderClient.userID.ToString());

                                            remFactionData(possibleFactions[0].Key, targetClient.userName, possibleFactions[0].Value[targetClient.userID.ToString()]);
                                            possibleFactions[0].Value.Remove(targetClient.userID.ToString());
                                            possibleFactions[0].Value.Add(targetClient.userID.ToString(), "owner");
                                            addFactionData(possibleFactions[0].Key, targetClient.userName, targetClient.userID.ToString(), "owner");

                                            possibleFactions[0].Value.Add(senderClient.userID.ToString(), "normal");
                                            addFactionData(possibleFactions[0].Key, senderClient.userName, targetClient.userID.ToString(), "normal");

                                            PlayerClient[] targetClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                            foreach (PlayerClient pc in targetClients)
                                            {
                                                Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, senderClient.userName + " now owns the faction.");
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

                            foreach (string factionName in factions.Keys)
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
                                    Broadcast.broadcastTo(senderClient.netPlayer, "All factions [" + pageNumber + "/" + currentPage + "]:", true);
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

                                        Broadcast.broadcastTo(senderClient.netPlayer, string.Join(", ", factionNames2.ToArray()), true);
                                    }
                                }
                                else
                                    Broadcast.broadcastTo(senderClient.netPlayer, "There are no factions!", true);
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

                                    KeyValuePair<string, Dictionary<string, string>>[] factionResults = Array.FindAll(factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Key.Contains(factionName));
                                    
                                    if (factionResults.Count() == 0)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "No factions equal or contain \"" + factionName + "\".");
                                    else if (factionResults.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many faction names contain \"" + factionName + "\".");
                                    else
                                    {
                                        if (possibleFactions[0].Key != factionResults[0].Key)
                                        {
                                            if (!alliances[possibleFactions[0].Key].Contains(factionResults[0].Key))
                                            {
                                                PlayerClient[] targetClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                                foreach (PlayerClient pc in targetClients)
                                                {
                                                    Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, "You are now allied with the faction [" + factionResults[0].Key + "].");
                                                }
                                                alliances[factionResults[0].Key].Add(possibleFactions[0].Key);
                                                alliances[possibleFactions[0].Key].Add(factionResults[0].Key);
                                                addAlliesData(factionResults[0].Key, possibleFactions[0].Key);
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
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.", true);
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

                                    KeyValuePair<string, Dictionary<string, string>>[] factionResults = Array.FindAll(factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Key.Contains(factionName));

                                    if (factionResults.Count() == 0)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "No factions equal or contain \"" + factionName + "\".");
                                    else if (factionResults.Count() > 1)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many faction names contain \"" + factionName + "\".");
                                    else
                                    {
                                        if (!alliances[possibleFactions[0].Key].Contains(factionResults[0].Key))
                                        {
                                            PlayerClient[] targetClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => possibleFactions[0].Value.ContainsKey(pc.userID.ToString()));
                                            foreach (PlayerClient pc in targetClients)
                                            {
                                                Broadcast.broadcastCustomTo(pc.netPlayer, "[F] " + possibleFactions[0].Key, "You are no longer allied with the faction [" + factionResults[0].Key + "].");
                                            }
                                            alliances[factionResults[0].Key].Remove(possibleFactions[0].Key);
                                            alliances[possibleFactions[0].Key].Remove(factionResults[0].Key);
                                            remAlliesData(factionResults[0].Key, possibleFactions[0].Key);
                                        }
                                        else
                                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You are not allied with [" + factionResults[0].Key +"].");
                                    }
                                }
                                else
                                    Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You must specify a faction name in order to remove an alliance.");
                            }
                            else
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "You do not have permission to remove alliances.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.", true);
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
                            KeyValuePair<string, Dictionary<string, string>>[] factionResults = Array.FindAll(factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Key.Contains(factionName));

                            if (factionResults.Count() == 0)
                            {
                                PlayerClient[] possibleClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(factionName));
                                if (possibleClients.Count() == 0)
                                    Broadcast.broadcastTo(senderClient.netPlayer, "No factions or players contain \"" + factionName + "\".");
                                else if (possibleClients.Count() > 1)
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + factionName + "\".");
                                else
                                {
                                    try
                                    {
                                        KeyValuePair<string, Dictionary<string, string>> playerFaction = Array.Find(factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(possibleClients[0].userID.ToString()));
                                        int onlineMembers = 0;
                                        foreach (string s in playerFaction.Value.Keys)
                                        {
                                            PlayerClient[] possibleClients2 = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID.ToString() == s);
                                            if (possibleClients2.Count() > 0)
                                                onlineMembers++;
                                        }
                                        string ownerName = factionsByNames[playerFaction.Key][Array.Find(playerFaction.Value.ToArray(), (KeyValuePair<string, string> kv) => kv.Value == "owner").Key];

                                        Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.Key, "=== [" + playerFaction.Key + "]'s information ===");
                                        Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.Key, "Total members: " + playerFaction.Value.Count);
                                        Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.Key, "Online members: " + onlineMembers);
                                        Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.Key, "Offline members: " + (playerFaction.Value.Count - onlineMembers));
                                        Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.Key, "Owner: " + ownerName);
                                        Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + playerFaction.Key, "Members:");
                                        List<string> names = new List<string>();
                                        List<string> names2 = new List<string>();
                                        foreach (string name in factionsByNames[playerFaction.Key].Values)
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
                                        if (alliances.ContainsKey(playerFaction.Key))
                                        {
                                            foreach (string name in alliances[playerFaction.Key])
                                            {
                                                names.Add(name);
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
                                                Broadcast.broadcastTo(senderClient.netPlayer, string.Join(", ", allies2.ToArray()));
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        conLog.Error(ex.ToString());
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
                                    PlayerClient[] possibleClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID.ToString() == s);
                                    if (possibleClients.Count() > 0)
                                        onlineMembers++;
                                }
                                string ownerName = factionsByNames[factionResults[0].Key][Array.Find(factionResults[0].Value.ToArray(), (KeyValuePair<string, string> kv) => kv.Value == "owner").Key];

                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].Key, "=== [" + factionResults[0].Key + "]'s information ===");
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].Key, "Total members: " + factionResults[0].Value.Count);
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].Key, "Online members: " + onlineMembers);
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].Key, "Offline members: " + (factionResults[0].Value.Count - onlineMembers));
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].Key, "Owner: " + ownerName);
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + factionResults[0].Key, "Members:");
                                List<string> names = new List<string>();
                                List<string> names2 = new List<string>();
                                foreach (string name in factionsByNames[factionResults[0].Key].Values)
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
                                if (alliances.ContainsKey(factionResults[0].Key))
                                {
                                    foreach (string name in alliances[factionResults[0].Key])
                                    {
                                        names.Add(name);
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
                                        Broadcast.broadcastTo(senderClient.netPlayer, string.Join(", ", allies2.ToArray()));
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
                                    PlayerClient[] possibleClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID.ToString() == s);
                                    if (possibleClients.Count() > 0)
                                        onlineMembers++;
                                }
                                string ownerName = factionsByNames[possibleFactions[0].Key][Array.Find(possibleFactions[0].Value.ToArray(), (KeyValuePair<string, string> kv) => kv.Value == "owner").Key];

                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "=== Your faction's information ===");
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "Total members: " + possibleFactions[0].Value.Count);
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "Online members: " + onlineMembers);
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "Offline members: " + (possibleFactions[0].Value.Count - onlineMembers));
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "Owner: " + ownerName);
                                Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, "Members:");
                                List<string> names = new List<string>();
                                List<string> names2 = new List<string>();
                                foreach (string name in factionsByNames[possibleFactions[0].Key].Values)
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
                                if (alliances.ContainsKey(possibleFactions[0].Key))
                                {
                                    foreach (string name in alliances[possibleFactions[0].Key])
                                    {
                                        names.Add(name);
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
                                        Broadcast.broadcastTo(senderClient.netPlayer, string.Join(", ", allies2.ToArray()));
                                    }
                                }
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.", true);
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
                                    PlayerClient[] possibleClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID.ToString() == s);

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
                                        PlayerClient[] possibleClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID.ToString() == s);
                                        string playerName = factionsByNames[possibleFactions[0].Key][s];

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
                                        string playerName = factionsByNames[possibleFactions[0].Key][s];

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
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.", true);
                        break;
                    case "online":
                        if (possibleFactions.Count() > 0)
                        {
                            int onlineMembers = 0;
                            foreach (string s in possibleFactions[0].Value.Keys)
                            {
                                PlayerClient[] possibleClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID.ToString() == s);
                                if (possibleClients.Count() > 0)
                                {
                                    if (possibleClients[0].userName.Length > 0)
                                        onlineMembers++;
                                }
                            }

                            Broadcast.broadcastCustomTo(senderClient.netPlayer, "[F] " + possibleFactions[0].Key, onlineMembers + "/" + possibleFactions[0].Value.Count + " faction members currently connected. Faction is at " + possibleFactions[0].Value.Count + "/" + maxMembers + " member capacity.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.", true);
                        break;
                    case "build":
                        if (Vars.enabledCommands[Vars.findRank(senderClient.userID.ToString())].Contains("/f build"))
                        {
                            if (args.Count() > 2)
                            {
                                string mode = args[2];
                                string UID = senderClient.userID.ToString();

                                switch (mode)
                                {
                                    case "on":
                                        if (!buildList.Contains(UID))
                                        {
                                            Broadcast.broadcastTo(senderClient.netPlayer, "You can now build in safe zones and war zones.");
                                            buildList.Add(UID);
                                        }
                                        else
                                            Broadcast.broadcastTo(senderClient.netPlayer, "You are already in build mode.");
                                        break;
                                    case "off":
                                        if (buildList.Contains(UID))
                                        {
                                            Broadcast.broadcastTo(senderClient.netPlayer, "You can no longer build in safe zones and war zones.");
                                            buildList.Remove(UID);
                                        }
                                        else
                                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in build mode.");
                                        break;
                                }
                            }
                        }
                        break;
                    case "help":
                        Broadcast.broadcastTo(senderClient.netPlayer, "=================== Factions ===================", true);
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f create *name*: Creates a faction.", true);
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f disband: Disbands current faction if you're the owner.", true);
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f kick *name*: Kicks the player from your faction. Partials accepted.", true);
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f invite *name*: Invites the player to your faction. Partials accepted.", true);
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f join: Joins the faction last invited to.", true);
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f join *name*: Joins the faction by name if invited.", true);
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f leave: Leaves current faction.", true);
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f admin *name*: Leaves current faction.", true);
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f deadmin *name*: Leaves current faction.", true);
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f ownership *name*: Leaves current faction.", true);
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f list: List all factions.", true);
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f info: Displays current faction information.", true);
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f info *name*: Displays that faction's information.", true);
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f ally *name*: Forms an alliance with another faction.", true);
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f unally *name*: Removes an alliance with another faction.", true);
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f players: Lists players in current faction.", true);
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f online: Displays count of currently online faction members.", true);
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f safezone {1/2/3/4/set/clear/clearall}: Manages safezones.", true);
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f warzone {1/2/3/4/set/clear/clearall}: Manages warzones.", true);
                        Broadcast.broadcastTo(senderClient.netPlayer, "/f build {on/off}: Allows building within zones.", true);
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "Unknown faction action \"" + arg + "\".");
                        break;
                }
            }
        }

        public static void ResourceUseItem(IResourceTypeItem rs, ResourceTypeItemDataBlock RTIDB)
        {
            RaycastHit hit;
            bool flag;
            Facepunch.MeshBatch.MeshBatchInstance instance;
            if (MeshBatchPhysics.SphereCast(rs.character.eyesRay, 0.5f, out hit, 4f, out flag, out instance))
            {
                IDMain idMain;
                if (flag)
                {
                    idMain = instance.idMain;
                }
                else
                {
                    idMain = IDBase.GetMain(hit.collider.gameObject);
                }
                if (idMain != null)
                {
                    RepairReceiver local = idMain.GetLocal<RepairReceiver>();
                    TakeDamage damage = idMain.GetLocal<TakeDamage>();
                    if ((((local != null) && (damage != null)) && (local.GetRepairAmmo() == RTIDB)) && (damage.health != damage.maxHealth))
                    {
                        if (enableRepair)
                        {
                            if (damage.TimeSinceHurt() < 5f)
                            {
                                int timeToWait = 5 - (int)Math.Round(damage.TimeSinceHurt());
                                Rust.Notice.Popup(rs.character.netUser.networkPlayer, "⌛", "You must wait " + timeToWait + " seconds.", 4f);
                            }
                            else
                            {
                                float amount = damage.maxHealth / ((float)local.ResForMaxHealth);
                                if (amount > (damage.maxHealth - damage.health))
                                {
                                    amount = damage.maxHealth - damage.health;
                                }
                                damage.Heal(rs.character.idMain, amount);
                                rs.lastUseTime = UnityEngine.Time.time;
                                int count = 1;
                                if (rs.Consume(ref count))
                                {
                                    rs.inventory.RemoveItem(rs.slot);
                                }
                                string strText = string.Format("Healed {0} ({1}/{2})", (int)amount, (int)damage.health, (int)damage.maxHealth);
                                Broadcast.sideNoticeTo(rs.inventory.networkViewOwner, strText);
                            }
                        }
                        else
                            Broadcast.broadcastTo(rs.inventory.networkViewOwner, "Repairing is disabled on this server.");
                    }
                }
            }

        }

        public static void readZoneData()
        {
            List<string> zoneFileData = File.ReadAllLines(zonesFile).ToList();
            foreach (string s in zoneFileData)
            {
                string zoneName = s.Split('=')[0];
                string posString = s.Split('=')[1];
                string firstPoint = posString.Split(';')[0].Replace("(", "").Replace(")", ""); // (x,y) - EX:(500.2,802.7)
                string secPoint = posString.Split(';')[1].Replace("(", "").Replace(")", "");
                string thirdPoint = posString.Split(';')[2].Replace("(", "").Replace(")", "");
                string forthPoint = posString.Split(';')[3].Replace("(", "").Replace(")", "");
                Vector2 point1 = new Vector2(Convert.ToSingle(firstPoint.Split(',')[0]), Convert.ToSingle(firstPoint.Split(',')[1]));
                Vector2 point2 = new Vector2(Convert.ToSingle(secPoint.Split(',')[0]), Convert.ToSingle(secPoint.Split(',')[1]));
                Vector2 point3 = new Vector2(Convert.ToSingle(thirdPoint.Split(',')[0]), Convert.ToSingle(thirdPoint.Split(',')[1]));
                Vector2 point4 = new Vector2(Convert.ToSingle(forthPoint.Split(',')[0]), Convert.ToSingle(forthPoint.Split(',')[1]));

                if (zoneName.StartsWith("safezone"))
                    safeZones.Add(zoneName, new Zone(point1, point2, point3, point4));

                if (zoneName.StartsWith("warzone"))
                    warZones.Add(zoneName, new Zone(point1, point2, point3, point4));

                Vars.conLog.Info("Adding zone [" + zoneName + "]...");
            }
        }

        public static void addZoneData(string zoneName, Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4)
        {
            List<string> zoneFileData = File.ReadAllLines(zonesFile).ToList();
            List<string> safezones = new List<string>();
            List<string> warzones = new List<string>();
            foreach (string s in zoneFileData)
            {
                bool isSafeZone = s.StartsWith("safezone");

                if (!s.StartsWith(zoneName))
                {
                    string points = s.Split('=')[1];

                    if (isSafeZone)
                    {
                        safezones.Add("safezone_" + safezones.Count + "=" + points);
                    }
                    else
                    {
                        warzones.Add("warzone_" + warzones.Count + "=" + points);
                    }
                }
            }
            zoneFileData.Clear();
            foreach (string s in safezones)
            {
                zoneFileData.Add(s);
            }
            if (zoneName.StartsWith("safezone"))
            {
                zoneFileData.Add("safezone_" + safezones.Count + "=(" + point1.x + "," + point1.y + ");(" + point2.x + "," + point2.y + ");(" + point3.x + "," + point3.y + ");(" + point4.x + "," + point4.y + ")");
            }
            foreach (string s in warzones)
            {
                zoneFileData.Add(s);
            }
            if (zoneName.StartsWith("warzone"))
            {
                zoneFileData.Add("warzone_" + warzones.Count + "=(" + point1.x + "," + point1.y + ");(" + point2.x + "," + point2.y + ");(" + point3.x + "," + point3.y + ");(" + point4.x + "," + point4.y + ")");
            }
            using (StreamWriter sw = new StreamWriter(zonesFile, false))
            {
                foreach (string s in zoneFileData)
                {
                    sw.WriteLine(s);
                }
            }
        }

        public static void remZoneData(string zoneName)
        {
            List<string> zoneFileData = File.ReadAllLines(zonesFile).ToList();
            List<string> safezones = new List<string>();
            List<string> warzones = new List<string>();
            bool clearAllSafe = zoneName == "clearallS";
            bool clearAllWar = zoneName == "clearallW";
            foreach (string s in zoneFileData)
            {
                bool isSafeZone = s.StartsWith("safezone");

                if (!s.StartsWith(zoneName))
                {
                    string points = s.Split('=')[1];

                    if (isSafeZone)
                    {
                        if (!clearAllSafe)
                            safezones.Add("safezone_" + safezones.Count + "=" + points);
                    }
                    else
                    {
                        if (!clearAllWar)
                            warzones.Add("warzone_" + warzones.Count + "=" + points);
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
            using (StreamWriter sw = new StreamWriter(zonesFile, false))
            {
                foreach (string s in zoneFileData)
                {
                    sw.WriteLine(s);
                }
            }
        }

        public static double PolygonArea(Vector2[] polygon)
        {
            int i, j;
            double area = 0;

            for (i = 0; i < polygon.Length; i++)
            {
                j = (i + 1) % polygon.Length;

                area += polygon[i].x * polygon[j].y;
                area -= polygon[i].y * polygon[j].x;
            }

            area /= 2;
            return (area < 0 ? -area : area);
        }

        public static void zoneTimer()
        {
            Thread t = new Thread(zoneLoop);
            t.Start();
        }

        public static void zoneLoop()
        {
            bool loopZones = true;
            while (loopZones)
            {
                cycleZones();
                Thread.Sleep(500);
            }
        }

        public static void cycleZones()
        {
            List<PlayerClient> playerClients = AllPlayerClients;
            foreach (PlayerClient pc in playerClients)
            {
                try
                {
                    if (pc != null)
                    {
                        if (pc.userID != null)
                        {
                            Character playerChar;
                            Character.FindByUser(pc.userID, out playerChar);
                            if (playerChar != null)
                            {
                                Vector2 playerPos = new Vector2(playerChar.transform.position.x, playerChar.transform.position.z);

                                string safeZone = "";
                                bool inZoneS = false;
                                foreach (KeyValuePair<string, Zone> kv in safeZones)
                                {
                                    Zone zone = kv.Value;
                                    Vector2 point1 = zone.firstPoint;
                                    Vector2 point2 = zone.secondPoint;
                                    Vector2 point3 = zone.thirdPoint;
                                    Vector2 point4 = zone.forthPoint;
                                    float s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, playerPos) + Vector2.Distance(playerPos, point1)) / 2;
                                    float s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, playerPos) + Vector2.Distance(playerPos, point2)) / 2;
                                    float s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, playerPos) + Vector2.Distance(playerPos, point3)) / 2;
                                    float s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, playerPos) + Vector2.Distance(playerPos, point4)) / 2;
                                    double areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, playerPos)) * (s1 - Vector2.Distance(playerPos, point1)));
                                    double areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, playerPos)) * (s2 - Vector2.Distance(playerPos, point2)));
                                    double areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, playerPos)) * (s3 - Vector2.Distance(playerPos, point3)));
                                    double areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, playerPos)) * (s4 - Vector2.Distance(playerPos, point4)));
                                    double areaActual = PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                                    double areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                                    if (areaAdded <= (areaActual + 1))
                                        inZoneS = true;

                                    if (inZoneS)
                                        safeZone = kv.Key;
                                }
                                if (!inZoneS && inSafeZone.ContainsKey(pc))
                                {
                                    Broadcast.noticeTo(pc.netPlayer, "⊛", "You have left the safe zone.");
                                    inSafeZone.Remove(pc);
                                }

                                if (!inSafeZone.ContainsKey(pc) && inZoneS)
                                {
                                    Broadcast.noticeTo(pc.netPlayer, "⊛", "You have entered a safe zone. Players cannot harm you.");
                                    inSafeZone.Add(pc, safeZone);
                                }

                                string warZone = "";
                                bool inZoneW = false;
                                foreach (KeyValuePair<string, Zone> kv in warZones)
                                {
                                    Zone zone = kv.Value;
                                    Vector2 point1 = zone.firstPoint;
                                    Vector2 point2 = zone.secondPoint;
                                    Vector2 point3 = zone.thirdPoint;
                                    Vector2 point4 = zone.forthPoint;
                                    float s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, playerPos) + Vector2.Distance(playerPos, point1)) / 2;
                                    float s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, playerPos) + Vector2.Distance(playerPos, point2)) / 2;
                                    float s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, playerPos) + Vector2.Distance(playerPos, point3)) / 2;
                                    float s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, playerPos) + Vector2.Distance(playerPos, point4)) / 2;
                                    double areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, playerPos)) * (s1 - Vector2.Distance(playerPos, point1)));
                                    double areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, playerPos)) * (s2 - Vector2.Distance(playerPos, point2)));
                                    double areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, playerPos)) * (s3 - Vector2.Distance(playerPos, point3)));
                                    double areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, playerPos)) * (s4 - Vector2.Distance(playerPos, point4)));
                                    double areaActual = PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                                    double areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                                    if (areaAdded <= (areaActual + 1))
                                        inZoneW = true;

                                    if (inZoneW)
                                        warZone = kv.Key;
                                }
                                if (!inZoneW && inWarZone.ContainsKey(pc))
                                {
                                    Broadcast.noticeTo(pc.netPlayer, "", "You have left the war zone.");
                                    inWarZone.Remove(pc);
                                }

                                if (!inWarZone.ContainsKey(pc) && inZoneW)
                                {
                                    Broadcast.noticeTo(pc.netPlayer, "", "You have entered a war zone. Damage is multiplied by " + warDamage + "x.");
                                    inWarZone.Add(pc, warZone);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    conLog.Error("CZ: " + ex.ToString());
                }
            }
        }

        public static void StructureComponentAction(uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info, StructureComponentDataBlock SCDB)
        {
            uLink.NetworkPlayer netPlayer = info.sender;
            PlayerClient playerClient = Array.Find(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == netPlayer);
            IStructureComponentItem item;
            NetCull.VerifyRPC(ref info);
            if (rep.Item<IStructureComponentItem>(out item) && (item.uses > 0))
            {
                StructureComponent structureToPlacePrefab = SCDB.structureToPlacePrefab;
                Vector3 origin = stream.ReadVector3();
                Vector3 direction = stream.ReadVector3();
                Vector3 position = stream.ReadVector3();
                Quaternion rotation = stream.ReadQuaternion();
                uLink.NetworkViewID viewID = stream.ReadNetworkViewID();
                StructureMaster component = null;
                if (nearZone(position) && !buildList.Contains(playerClient.userID.ToString()))
                {
                    Rust.Notice.Popup(info.sender, "", "You can't place that near safe zones or war zones.", 4f);
                }
                else
                {
                    if (viewID == uLink.NetworkViewID.unassigned)
                    {
                        if (SCDB.MasterFromRay(new Ray(origin, direction)))
                        {
                            return;
                        }
                        if (structureToPlacePrefab.type != StructureComponent.StructureComponentType.Foundation)
                        {
                            Debug.Log("ERROR, tried to place non foundation structure on terrain!");
                        }
                        else
                        {
                            component = NetCull.InstantiateClassic<StructureMaster>(Bundling.Load<StructureMaster>("content/structures/StructureMasterPrefab"), position, rotation, 0);
                            component.SetupCreator(item.controllable);
                        }
                    }
                    else
                    {
                        component = uLink.NetworkView.Find(viewID).gameObject.GetComponent<StructureMaster>();
                    }
                    if (component == null)
                    {
                        Debug.Log("NO master, something seriously wrong");
                    }
                    else if (SCDB._structureToPlace.CheckLocation(component, position, rotation) && SCDB.CheckBlockers(position))
                    {
                        StructureComponent comp = NetCull.InstantiateStatic(SCDB.structureToPlaceName, position, rotation).GetComponent<StructureComponent>();
                        if (comp != null)
                        {
                            component.AddStructureComponent(comp);
                            int count = 1;
                            if (item.Consume(ref count))
                            {
                                item.inventory.RemoveItem(item.slot);
                            }
                        }
                    }
                }
            }
        }

        public static void DeployableItemAction(uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info, DeployableItemDataBlock DIDB)
        {
            uLink.NetworkPlayer netPlayer = info.sender;
            PlayerClient playerClient = Array.Find(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == netPlayer);
            IDeployableItem item;
            NetCull.VerifyRPC(ref info);
            if (rep.Item<IDeployableItem>(out item) && (item.uses > 0))
            {
                Vector3 vector3;
                Quaternion quaternion;
                TransCarrier carrier;
                Vector3 origin = stream.ReadVector3();
                Vector3 direction = stream.ReadVector3();
                Ray ray = new Ray(origin, direction);
                if (!DIDB.CheckPlacement(ray, out vector3, out quaternion, out carrier))
                {
                    Rust.Notice.Popup(info.sender, "", "You can't place that here.", 4f);
                }
                else
                {
                    if (nearZone(vector3) && !buildList.Contains(playerClient.userID.ToString()))
                    {
                        Rust.Notice.Popup(info.sender, "", "You can't place that near safe zones or war zones.", 4f);
                    }
                    else
                    {
                        DeployableObject component = NetCull.InstantiateStatic(DIDB.DeployableObjectPrefabName, vector3, quaternion).GetComponent<DeployableObject>(); // Creates model in world space
                        if (component != null)
                        {
                            try
                            {
                                component.SetupCreator(item.controllable); // Sets object variables such as ownerID
                                DIDB.SetupDeployableObject(stream, rep, ref info, component, carrier);
                            }
                            finally
                            {
                                int count = 1;
                                if (item.Consume(ref count))
                                {
                                    item.inventory.RemoveItem(item.slot);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static bool inZone(Vector3 origin)
        {
            Vector2 origin2D = new Vector2(origin.x, origin.z);

            bool inZone = false;
                foreach (KeyValuePair<string, Zone> kv in safeZones)
                {
                    Zone zone = kv.Value;
                    Vector2 point1 = zone.firstPoint;
                    Vector2 point2 = zone.secondPoint;
                    Vector2 point3 = zone.thirdPoint;
                    Vector2 point4 = zone.forthPoint;
                    float s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, origin2D) + Vector2.Distance(origin2D, point1)) / 2;
                    float s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, origin2D) + Vector2.Distance(origin2D, point2)) / 2;
                    float s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, origin2D) + Vector2.Distance(origin2D, point3)) / 2;
                    float s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, origin2D) + Vector2.Distance(origin2D, point4)) / 2;
                    double areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, origin2D)) * (s1 - Vector2.Distance(origin2D, point1)));
                    double areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, origin2D)) * (s2 - Vector2.Distance(origin2D, point2)));
                    double areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, origin2D)) * (s3 - Vector2.Distance(origin2D, point3)));
                    double areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, origin2D)) * (s4 - Vector2.Distance(origin2D, point4)));
                    double areaActual = PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    double areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                        inZone = true;
                }
                foreach (KeyValuePair<string, Zone> kv in warZones)
                {
                    Zone zone = kv.Value;
                    Vector2 point1 = zone.firstPoint;
                    Vector2 point2 = zone.secondPoint;
                    Vector2 point3 = zone.thirdPoint;
                    Vector2 point4 = zone.forthPoint;
                    float s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, origin2D) + Vector2.Distance(origin2D, point1)) / 2;
                    float s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, origin2D) + Vector2.Distance(origin2D, point2)) / 2;
                    float s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, origin2D) + Vector2.Distance(origin2D, point3)) / 2;
                    float s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, origin2D) + Vector2.Distance(origin2D, point4)) / 2;
                    double areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, origin2D)) * (s1 - Vector2.Distance(origin2D, point1)));
                    double areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, origin2D)) * (s2 - Vector2.Distance(origin2D, point2)));
                    double areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, origin2D)) * (s3 - Vector2.Distance(origin2D, point3)));
                    double areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, origin2D)) * (s4 - Vector2.Distance(origin2D, point4)));
                    double areaActual = PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    double areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                        inZone = true;
                }

            return inZone;
        }

        public static bool nearZone(Vector3 origin)
        {
            Vector2 origin2D = new Vector2(origin.x, origin.z);
            Vector2 originLeft = new Vector2(origin.x - 15, origin.z);
            Vector2 originRight = new Vector2(origin.x + 15, origin.z);
            Vector2 originBottom = new Vector2(origin.x, origin.z - 15);
            Vector2 originTop = new Vector2(origin.x, origin.z + 15);

            bool inZone = false;
                foreach (KeyValuePair<string, Zone> kv in safeZones)
                {
                    Zone zone = kv.Value;
                    Vector2 point1 = zone.firstPoint;
                    Vector2 point2 = zone.secondPoint;
                    Vector2 point3 = zone.thirdPoint;
                    Vector2 point4 = zone.forthPoint;
                    float s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, origin2D) + Vector2.Distance(origin2D, point1)) / 2;
                    float s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, origin2D) + Vector2.Distance(origin2D, point2)) / 2;
                    float s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, origin2D) + Vector2.Distance(origin2D, point3)) / 2;
                    float s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, origin2D) + Vector2.Distance(origin2D, point4)) / 2;
                    double areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, origin2D)) * (s1 - Vector2.Distance(origin2D, point1)));
                    double areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, origin2D)) * (s2 - Vector2.Distance(origin2D, point2)));
                    double areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, origin2D)) * (s3 - Vector2.Distance(origin2D, point3)));
                    double areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, origin2D)) * (s4 - Vector2.Distance(origin2D, point4)));
                    double areaActual = PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    double areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        inZone = true;
                        break;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originLeft) + Vector2.Distance(originLeft, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originLeft) + Vector2.Distance(originLeft, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originLeft) + Vector2.Distance(originLeft, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originLeft) + Vector2.Distance(originLeft, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originLeft)) * (s1 - Vector2.Distance(originLeft, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originLeft)) * (s2 - Vector2.Distance(originLeft, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originLeft)) * (s3 - Vector2.Distance(originLeft, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originLeft)) * (s4 - Vector2.Distance(originLeft, point4)));
                    areaActual = PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        inZone = true;
                        break;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originRight) + Vector2.Distance(originRight, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originRight) + Vector2.Distance(originRight, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originRight) + Vector2.Distance(originRight, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originRight) + Vector2.Distance(originRight, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originRight)) * (s1 - Vector2.Distance(originRight, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originRight)) * (s2 - Vector2.Distance(originRight, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originRight)) * (s3 - Vector2.Distance(originRight, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originRight)) * (s4 - Vector2.Distance(originRight, point4)));
                    areaActual = PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        inZone = true;
                        break;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originTop) + Vector2.Distance(originTop, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originTop) + Vector2.Distance(originTop, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originTop) + Vector2.Distance(originTop, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originTop) + Vector2.Distance(originTop, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originTop)) * (s1 - Vector2.Distance(originTop, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originTop)) * (s2 - Vector2.Distance(originTop, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originTop)) * (s3 - Vector2.Distance(originTop, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originTop)) * (s4 - Vector2.Distance(originTop, point4)));
                    areaActual = PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        inZone = true;
                        break;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originBottom) + Vector2.Distance(originBottom, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originBottom) + Vector2.Distance(originBottom, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originBottom) + Vector2.Distance(originBottom, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originBottom) + Vector2.Distance(originBottom, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originBottom)) * (s1 - Vector2.Distance(originBottom, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originBottom)) * (s2 - Vector2.Distance(originBottom, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originBottom)) * (s3 - Vector2.Distance(originBottom, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originBottom)) * (s4 - Vector2.Distance(originBottom, point4)));
                    areaActual = PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        inZone = true;
                        break;
                    }
                }

                foreach (KeyValuePair<string, Zone> kv in warZones)
                {
                    Zone zone = kv.Value;
                    Vector2 point1 = zone.firstPoint;
                    Vector2 point2 = zone.secondPoint;
                    Vector2 point3 = zone.thirdPoint;
                    Vector2 point4 = zone.forthPoint;
                    float s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, origin2D) + Vector2.Distance(origin2D, point1)) / 2;
                    float s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, origin2D) + Vector2.Distance(origin2D, point2)) / 2;
                    float s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, origin2D) + Vector2.Distance(origin2D, point3)) / 2;
                    float s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, origin2D) + Vector2.Distance(origin2D, point4)) / 2;
                    double areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, origin2D)) * (s1 - Vector2.Distance(origin2D, point1)));
                    double areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, origin2D)) * (s2 - Vector2.Distance(origin2D, point2)));
                    double areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, origin2D)) * (s3 - Vector2.Distance(origin2D, point3)));
                    double areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, origin2D)) * (s4 - Vector2.Distance(origin2D, point4)));
                    double areaActual = PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    double areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        inZone = true;
                        break;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originLeft) + Vector2.Distance(originLeft, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originLeft) + Vector2.Distance(originLeft, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originLeft) + Vector2.Distance(originLeft, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originLeft) + Vector2.Distance(originLeft, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originLeft)) * (s1 - Vector2.Distance(originLeft, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originLeft)) * (s2 - Vector2.Distance(originLeft, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originLeft)) * (s3 - Vector2.Distance(originLeft, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originLeft)) * (s4 - Vector2.Distance(originLeft, point4)));
                    areaActual = PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        inZone = true;
                        break;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originRight) + Vector2.Distance(originRight, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originRight) + Vector2.Distance(originRight, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originRight) + Vector2.Distance(originRight, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originRight) + Vector2.Distance(originRight, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originRight)) * (s1 - Vector2.Distance(originRight, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originRight)) * (s2 - Vector2.Distance(originRight, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originRight)) * (s3 - Vector2.Distance(originRight, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originRight)) * (s4 - Vector2.Distance(originRight, point4)));
                    areaActual = PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        inZone = true;
                        break;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originTop) + Vector2.Distance(originTop, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originTop) + Vector2.Distance(originTop, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originTop) + Vector2.Distance(originTop, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originTop) + Vector2.Distance(originTop, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originTop)) * (s1 - Vector2.Distance(originTop, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originTop)) * (s2 - Vector2.Distance(originTop, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originTop)) * (s3 - Vector2.Distance(originTop, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originTop)) * (s4 - Vector2.Distance(originTop, point4)));
                    areaActual = PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        inZone = true;
                        break;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originBottom) + Vector2.Distance(originBottom, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originBottom) + Vector2.Distance(originBottom, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originBottom) + Vector2.Distance(originBottom, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originBottom) + Vector2.Distance(originBottom, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originBottom)) * (s1 - Vector2.Distance(originBottom, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originBottom)) * (s2 - Vector2.Distance(originBottom, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originBottom)) * (s3 - Vector2.Distance(originBottom, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originBottom)) * (s4 - Vector2.Distance(originBottom, point4)));
                    areaActual = PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        inZone = true;
                        break;
                    }
                }

            return inZone;
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
                        if (!firstPoints.ContainsKey(senderClient))
                        {
                            firstPoints.Add(senderClient, currentPos);
                            Broadcast.broadcastTo(senderClient.netPlayer, "First zone point set to " + currentPos.ToString());
                        }
                        else
                        {
                            firstPoints[senderClient] = currentPos;
                            Broadcast.broadcastTo(senderClient.netPlayer, "First zone point reset to " + currentPos.ToString());
                        }
                        break;
                    case "2":
                        if (!secondPoints.ContainsKey(senderClient))
                        {
                            secondPoints.Add(senderClient, currentPos);
                            Broadcast.broadcastTo(senderClient.netPlayer, "Second zone point set to " + currentPos.ToString());
                        }
                        else
                        {
                            secondPoints[senderClient] = currentPos;
                            Broadcast.broadcastTo(senderClient.netPlayer, "Second zone point reset to " + currentPos.ToString());
                        }
                        break;
                    case "3":
                        if (!thirdPoints.ContainsKey(senderClient))
                        {
                            thirdPoints.Add(senderClient, currentPos);
                            Broadcast.broadcastTo(senderClient.netPlayer, "Third zone point set to " + currentPos.ToString());
                        }
                        else
                        {
                            thirdPoints[senderClient] = currentPos;
                            Broadcast.broadcastTo(senderClient.netPlayer, "Third zone point reset to " + currentPos.ToString());
                        }
                        break;
                    case "4":
                        if (!forthPoints.ContainsKey(senderClient))
                        {
                            forthPoints.Add(senderClient, currentPos);
                            Broadcast.broadcastTo(senderClient.netPlayer, "Forth zone point set to " + currentPos.ToString());
                        }
                        else
                        {
                            forthPoints[senderClient] = currentPos;
                            Broadcast.broadcastTo(senderClient.netPlayer, "Forth zone point reset to " + currentPos.ToString());
                        }
                        break;
                    case "set":
                        if (!secondPoints.ContainsKey(senderClient) && !firstPoints.ContainsKey(senderClient))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You must set points before creating a zone.");
                            return;
                        }
                        if (!firstPoints.ContainsKey(senderClient))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You must set the first point before creating a zone.");
                            return;
                        }
                        if (!secondPoints.ContainsKey(senderClient))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You must set the second point before creating a zone.");
                            return;
                        }

                        Vector2 firstPoint = firstPoints[senderClient];
                        Vector2 secondPoint = secondPoints[senderClient];
                        Vector2 thirdPoint = thirdPoints[senderClient];
                        Vector2 forthPoint = forthPoints[senderClient];

                        if (safeZone)
                        {
                            safeZones.Add("safezone_" + safeZones.Count, new Zone(firstPoint, secondPoint, thirdPoint, forthPoint));
                            Broadcast.broadcastTo(senderClient.netPlayer, "Safe zone created! Size: " + Math.Round(Vector2.Distance(firstPoint, secondPoint), 2) + " square meters.");
                            addZoneData("safezone_" + safeZones.Count, firstPoint, secondPoint, thirdPoint, forthPoint);
                        }
                        else
                        {
                            warZones.Add("warzone_" + warZones.Count, new Zone(firstPoint, secondPoint, thirdPoint, forthPoint));
                            Broadcast.broadcastTo(senderClient.netPlayer, "War zone created! Size: " + Math.Round(Vector2.Distance(firstPoint, secondPoint), 2) + " square meters.");
                            addZoneData("warzone_" + safeZones.Count, firstPoint, secondPoint, thirdPoint, forthPoint);
                        }
                        break;
                    case "clear":
                        if (safeZone)
                        {
                            if (inSafeZone.ContainsKey(senderClient))
                            {
                                KeyValuePair<PlayerClient, string>[] clients = Array.FindAll(inSafeZone.ToArray(), (KeyValuePair<PlayerClient, string> kv) => kv.Value == inSafeZone[senderClient]);
                                foreach (KeyValuePair<PlayerClient, string> kv in clients)
                                {
                                    Broadcast.noticeTo(kv.Key.netPlayer, "!", "Safe zone deleted! You are no longer in a safe zone.", 3);
                                    inSafeZone.Remove(kv.Key);
                                    if (safeZones.ContainsKey(kv.Value))
                                        safeZones.Remove(kv.Value);
                                    remZoneData(kv.Value);
                                }
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "You must be in a safe zone to delete it.");
                        }
                        else
                        {
                            if (inWarZone.ContainsKey(senderClient))
                            {
                                KeyValuePair<PlayerClient, string>[] clients = Array.FindAll(inWarZone.ToArray(), (KeyValuePair<PlayerClient, string> kv) => kv.Value == inWarZone[senderClient]);
                                foreach (KeyValuePair<PlayerClient, string> kv in clients)
                                {
                                    Broadcast.noticeTo(kv.Key.netPlayer, "!", "War zone deleted! You are no longer in a war zone.", 3);
                                    inWarZone.Remove(kv.Key);
                                    if (warZones.ContainsKey(kv.Value))
                                        warZones.Remove(kv.Value);
                                    remZoneData(kv.Value);
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
                            safeZones.Clear();
                            foreach (PlayerClient pc in inSafeZone.Keys)
                            {
                                Broadcast.noticeTo(pc.netPlayer, "!", "Safe zone deleted! You are no longer in a safe zone.", 3);
                            }
                            inSafeZone.Clear();
                            remZoneData("clearallS");
                        }
                        else
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "All war zones deleted.");
                            warZones.Clear();
                            foreach (PlayerClient pc in inWarZone.Keys)
                            {
                                Broadcast.noticeTo(pc.netPlayer, "!", "War zone deleted! You are no longer in a war zone.", 3);
                            }
                            inWarZone.Clear();
                            remZoneData("clearallW");
                        }
                        break;
                }
            }
        }

        public static void clientSpeak(VoiceCom voiceCom, int setupData, byte[] data)
        {
            //try
            //{
            //    PlayerClient client;
            //    if (((voice.distance > 0f) && PlayerClient.Find(voiceCom.networkViewOwner, out client)) && client.hasLastKnownPosition)
            //    {
            //        float num = inGlobalV.Contains(client.userID.ToString()) ? (1000000000f * 1000000000f) : (voice.distance * voice.distance);
            //        Vector3 lastKnownPosition = client.lastKnownPosition;
            //        int num3 = 0;
            //        try
            //        {
            //            foreach (PlayerClient client2 in AllPlayerClients)
            //            {
            //                if (((client2 != null) && client2.hasLastKnownPosition) && (client2 != client))
            //                {
            //                    Vector3 vector;
            //                    vector.x = client2.lastKnownPosition.x - lastKnownPosition.x;
            //                    vector.y = client2.lastKnownPosition.y - lastKnownPosition.y;
            //                    vector.z = client2.lastKnownPosition.z - lastKnownPosition.z;
            //                    float num2 = ((vector.x * vector.x) + (vector.y * vector.y)) + (vector.z * vector.z);
            //                    if (num2 <= num)
            //                    {
            //                        num3++;
            //                        voiceCom.playerList.Add(client2.netPlayer);
            //                    }
            //                }
            //            }
            //            if (num3 > 0)
            //            {
            //                object[] args = new object[] { voice.distance, setupData, data };
            //                voiceCom.networkView.RPC("VoiceCom:voiceplay", voiceCom.playerList, args);
            //            }
            //        }
            //        finally
            //        {
            //            if (num3 > 0)
            //            {
            //                voiceCom.playerList.Clear();
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    conLog.Error(ex.ToString());
            //}
        }

        public static void grabClient(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                int clientNumber;

                if (int.TryParse(args[1], out clientNumber))
                {
                    PlayerClient targetClient = AllPlayerClients[clientNumber];

                    Broadcast.broadcastTo(senderClient.netPlayer, "Client #: " + clientNumber);
                    Broadcast.broadcastTo(senderClient.netPlayer, "Client String: " + targetClient.ToString());
                    Broadcast.broadcastTo(senderClient.netPlayer, "Username: " + targetClient.userName);
                    Broadcast.broadcastTo(senderClient.netPlayer, "UID: " + targetClient.userID);
                }
            }
        }

        public static void showDistance(PlayerClient senderClient, string[] args)
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

                PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));

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

        public static void showOwner(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();

                switch (mode)
                {
                    case "on":
                        if (!ownershipList.Contains(UID))
                        {
                            if (destroyerList.Contains(UID))
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, "Remover tool deactivated.");
                                destroyerList.Remove(UID);
                            }

                            Broadcast.broadcastTo(senderClient.netPlayer, "Ownership tool activated. Hit structures to show who owns them.");
                            ownershipList.Add(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You already have the ownership tool activated.");
                        break;
                    case "off":
                        if (ownershipList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "Ownership tool deactivated.");
                            ownershipList.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You do not have the ownership tool activated.");
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "Unknown argument \"" + mode + "\".");
                        break;
                }
            }
        }

        public static bool inventoryFull(PlayerClient playerClient)
        {
            Inventory inventory = playerClient.controllable.GetComponent<Inventory>();
            return inventory.noVacantSlots;
        }

        public static void clearInventory(PlayerClient playerClient)
        {
            Inventory inventory = playerClient.controllable.GetComponent<Inventory>();

            for (int i = 0; i < inventory.slotCount; i++)
            {
                inventory.RemoveItem(i);
            }
        }

        public static void removeItem(PlayerClient playerClient, IInventoryItem item)
        {
            Inventory inventory = playerClient.controllable.GetComponent<Inventory>();
            inventory.RemoveItem(item.slot);
        }

        public static void clearArmor(PlayerClient playerClient)
        {
            Inventory inventory = playerClient.controllable.GetComponent<Inventory>();

            for (int i = 36; i < 40; i++)
            {
                inventory.RemoveItem(i);
            }
        }

        public static bool addItem(PlayerClient playerClient, string itemName, int amount)
        {
            Inventory inventory = playerClient.controllable.GetComponent<Inventory>();

            if (addArmor(playerClient, itemName))
            {
                return true;
            }
            else
            {
                if (inventory.vacantSlotCount > 0)
                {
                    inventory.AddItemAmount(DatablockDictionary.GetByName(itemName), amount);
                    return true;
                }
                else
                    return false;
            }
        }

        public static bool addArmor(PlayerClient playerClient, string itemName, bool replaceCurrent = false)
        {
            Inventory inventory = playerClient.controllable.GetComponent<Inventory>();
            int slot = 0;
            
            if (itemName.Contains("Helmet"))
                slot = 36;
            if (itemName.Contains("Vest"))
                slot = 37;
            if (itemName.Contains("Pants"))
                slot = 38;
            if (itemName.Contains("Boots"))
                slot = 39;

            if (slot > 0)
            {
                if (replaceCurrent)
                    inventory.RemoveItem(slot);

                if (!inventory.IsSlotOccupied(slot))
                    inventory.AddItemAmount(DatablockDictionary.GetByName(itemName), 1, Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Armor, false, Inventory.Slot.KindFlags.Armor));
                else
                    inventory.AddItemAmount(DatablockDictionary.GetByName(itemName), 1);
                return true;
            }
            else
                return false;
        }

        public static bool grabArmor(PlayerClient playerClient, out List<IInventoryItem> items)
        {
            Inventory inventory = playerClient.controllable.GetComponent<Inventory>();
            items = new List<IInventoryItem>();
            for (int i = 36; i < 40 ; i++)
            {
                IInventoryItem item;
                if (inventory.GetItem(i, out item))
                {
                    try
                    {
                        if (item != null)
                            items.Add(item);
                    }
                    catch { }
                }
            }
            return items.Count() > 0;
        }

        public static bool grabItem(PlayerClient playerClient, string itemName, out List<IInventoryItem> items)
        {
            Inventory inventory = playerClient.controllable.GetComponent<Inventory>();
            items = new List<IInventoryItem>();
            for (int i = 0; i < inventory.slotCount - 1; i++)
            {
                IInventoryItem item;
                if (inventory.GetItem(i, out item))
                {
                    try
                    {
                        if (item.datablock.name == itemName)
                            items.Add(item);
                    }
                    catch { }
                }
            }
            return items.Count() > 0;
        }

        public static bool hasItem(PlayerClient playerClient, string itemName)
        {
            Inventory inventory = playerClient.controllable.GetComponent<Inventory>();
            for (int i = 0; i < inventory.slotCount - 1; i++)
            {
                IInventoryItem item2;
                if (inventory.GetItem(i, out item2))
                {
                    try
                    {
                        if (item2.datablock.name == itemName)
                        {
                            return true;
                        }
                    }
                    catch { }
                }
            }
            return false;
        }

        public static bool hasItem(PlayerClient playerClient, string itemName, out IInventoryItem item)
        {
            Inventory inventory = playerClient.controllable.GetComponent<Inventory>();
            for (int i = 0; i < inventory.slotCount - 1; i++)
            {
                IInventoryItem item2;
                if (inventory.GetItem(i, out item2))
                {
                    try
                    {
                        if (item2.datablock.name == itemName)
                        {
                            item = item2;
                            return true;
                        }
                    }
                    catch { }
                }
            }
            item = null;
            return false;
        }

        public static void craftTool(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();
                Inventory senderInv = senderClient.controllable.GetComponent<Inventory>();

                switch (mode)
                {
                    case "on":
                        if (!craftList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are now in super craft mode. Crafting, research, and blueprint restrictions have been bypassed.");
                            craftList.Add(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are already in super craft mode.");
                        break;
                    case "off":
                        if (craftList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are now in normal craft mode. Crafting, research, and blueprint restrictions are in place.");
                            craftList.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are already in normal craft mode.");
                        break;
                }
            }
        }

        public static void vanishTool(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();
                Inventory senderInv = senderClient.controllable.GetComponent<Inventory>();

                switch (mode)
                {
                    case "on":
                        if (!vanishedList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You have vanished. Your body is invisible. Reconnect to make your name invisible.");
                            vanishedList.Add(UID);

                            List<IInventoryItem> armorList = new List<IInventoryItem>();
                            previousArmor.Add(UID, new List<string>());
                            if (grabArmor(senderClient, out armorList))
                            {
                                foreach (IInventoryItem item in armorList)
                                {
                                    previousArmor[UID].Add(item.datablock.name);
                                }
                            }
                            addArmor(senderClient, "Invisible Helmet", true);
                            addArmor(senderClient, "Invisible Vest", true);
                            addArmor(senderClient, "Invisible Pants", true);
                            addArmor(senderClient, "Invisible Boots", true);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You have already vanished.");
                        break;
                    case "off":
                        if (vanishedList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You have appeared. Your body is visible. Reconnect to make your body visible.");
                            vanishedList.Remove(UID);
                            clearArmor(senderClient);
                            foreach (string s in previousArmor[UID])
                            {
                                addArmor(senderClient, s);
                            }
                            previousArmor.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You have not vanished.");
                        break;
                }
            }
        }

        public static PlayerClient CreatePlayerClientForUser(NetUser user)
        {
            try
            {
                ServerManagement SM = ServerManagement.Get();
                string userName = user.user.Displayname;
                if (vanishedList.Contains(user.userID.ToString()))
                    userName = "";

                object[] args = new object[] { user.user.Userid, userName };
                GameObject go = NetCull.InstantiateClassicWithArgs(user.networkPlayer, ":client", Vector3.zero, Quaternion.identity, 0, args);
                PlayerClient component = go.GetComponent<PlayerClient>();
                if (component == null)
                {
                    NetCull.Destroy(go);
                    return component;
                }
                SM._playerClientList.Add(component);
                return component;
            }
            catch (Exception ex) { Vars.conLog.Error(ex.ToString()); }
            return null;
        }

        public static void hideTool(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();

                switch (mode)
                {
                    case "on":
                        if (!hiddenList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are now hidden from AI.");
                            hiddenList.Add(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are already hidden from AI.");
                        break;
                    case "off":
                        if (hiddenList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are no longer hidden from AI.");
                            hiddenList.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not currently hidden from AI.");
                        break;
                }
            }
        }

        public static void removerAllTool(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();

                switch (mode)
                {
                    case "on":
                        if (!destroyerAllList.Contains(UID))
                        {
                            if (ownershipList.Contains(UID))
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, "Ownership tool deactivated.");
                                ownershipList.Remove(UID);
                            }

                            if (destroyerList.Contains(UID))
                                destroyerList.Remove(UID);
                            Broadcast.broadcastTo(senderClient.netPlayer, "Remover tool activated. Hit AI or structures to delete them and their connected structures.");
                            destroyerAllList.Add(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You already have the remover tool activated.");
                        break;
                    case "off":
                        if (destroyerAllList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "Remover tool deactivated.");
                            destroyerAllList.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You do not have the remover tool activated.");
                        break;
                }
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
                            if (ownershipList.Contains(UID))
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, "Ownership tool deactivated.");
                                ownershipList.Remove(UID);
                            }

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

        public static StructureMaster.DecayStatus DoDecay(StructureMaster SM)
        {
            float num = UnityEngine.Time.time - SM._lastDecayTime;
            SM._lastDecayTime = UnityEngine.Time.time;
            if (SM._decayRate <= 0f)
            {
                return StructureMaster.DecayStatus.Delaying;
            }
            SM._decayDelayRemaining -= num;
            num = -SM._decayDelayRemaining;
            if (SM._decayDelayRemaining < 0f)
            {
                SM._decayDelayRemaining = 0f;
            }
            if (num <= 0f)
            {
                return StructureMaster.DecayStatus.Delaying;
            }
            SM._pentUpDecayTime += num;
            float decayTimeMaxHealth = SM.GetDecayTimeMaxHealth();
            float num3 = SM._pentUpDecayTime / decayTimeMaxHealth;
            if (num3 < structure.minpercentdmg)
            {
                return StructureMaster.DecayStatus.PentUpDecay;
            }
            SM._pentUpDecayTime = 0f;
            foreach (StructureComponent component in SM._structureComponents)
            {
                Vector3 position = component.transform.position;
                if (!inZone(position))
                {
                    TakeDamage damage = component.GetComponent<TakeDamage>();
                    if (damage != null)
                    {
                        float damageQuantity = ((damage.maxHealth * num3) * UnityEngine.Random.Range((float)0.75f, (float)1.25f)) * SM._decayRate;
                        if (((component.type == StructureComponent.StructureComponentType.Wall) || (component.type == StructureComponent.StructureComponentType.Doorway)) || (component.type == StructureComponent.StructureComponentType.WindowWall))
                        {
                            RaycastHit hit;
                            bool flag4;
                            Facepunch.MeshBatch.MeshBatchInstance instance;
                            Ray ray = new Ray(component.transform.position + new Vector3(0f, 2.5f, 0f), component.transform.forward);
                            Ray ray2 = new Ray(component.transform.position + new Vector3(0f, 2.5f, 0f), -component.transform.forward);
                            bool flag3 = false;
                            if (Facepunch.MeshBatch.MeshBatchPhysics.Raycast(ray, out hit, 25f, out flag4, out instance))
                            {
                                RaycastHit hit2;
                                IDMain main = !flag4 ? IDBase.GetMain(hit.collider.gameObject) : instance.idMain;
                                if (((main != null) && ((main is StructureComponent) || main.CompareTag("Door"))) && Facepunch.MeshBatch.MeshBatchPhysics.Raycast(ray2, out hit2, 25f, out flag4, out instance))
                                {
                                    main = !flag4 ? IDBase.GetMain(hit2.collider.gameObject) : instance.idMain;
                                    if ((main != null) && ((main is StructureComponent) || main.CompareTag("Door")))
                                    {
                                        flag3 = true;
                                    }
                                }
                            }
                            if (flag3)
                            {
                                damageQuantity *= 0.2f;
                            }
                            TakeDamage.HurtSelf(component, damageQuantity, null);
                        }
                        else if (component.type == StructureComponent.StructureComponentType.Pillar)
                        {
                            if (!SM.ComponentCarryingWeight(component))
                            {
                                TakeDamage.HurtSelf(component, damageQuantity, null);
                            }
                        }
                        else if (component.type == StructureComponent.StructureComponentType.Ceiling)
                        {
                            if (!SM.ComponentCarryingWeight(component))
                            {
                                TakeDamage.HurtSelf(component, damageQuantity, null);
                            }
                        }
                        else if (component.type == StructureComponent.StructureComponentType.Foundation)
                        {
                            if (!SM.ComponentCarryingWeight(component))
                            {
                                TakeDamage.HurtSelf(component, damageQuantity, null);
                            }
                        }
                        else
                        {
                            TakeDamage.HurtSelf(component, damageQuantity, null);
                        }
                    }
                }
            }
            return StructureMaster.DecayStatus.Decaying;
        }

        public static EnvDecay.ThinkResult DecayThink(EnvDecay ED)
        {
            if (ED._takeDamage == null)
            {
                return EnvDecay.ThinkResult.Done;
            }
            if ((ED._deployable == null) || (ED._deployable.GetCarrier() == null))
            {
                float num = UnityEngine.Time.time - ED.lastDecayThink;
                if (num < decay.decaytickrate)
                {
                    return EnvDecay.ThinkResult.TooEarly;
                }
                if (ED.CanApplyDecayDamage())
                {
                    float damageQuantity = ((Mathf.Clamp(UnityEngine.Time.time - ED.lastDecayThink, 0f, decay.decaytickrate) / decay.deploy_maxhealth_sec) * ED._takeDamage.maxHealth) * ED.decayMultiplier;
                    
                    Vector3 position = ED.transform.position;
                    if (!inZone(position))
                    {
                        if (TakeDamage.HurtSelf(ED, damageQuantity, null) == LifeStatus.WasKilled)
                        {
                            return EnvDecay.ThinkResult.Done;
                        }
                    }
                }
            }
            return EnvDecay.ThinkResult.AgainLater;
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
                NetEntityID entID = NetEntityID.Get((UnityEngine.MonoBehaviour)structureComponent);
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
                NetEntityID entID = NetEntityID.Get((UnityEngine.MonoBehaviour)deployableObject);
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
                            else if (destroyerAllList.Contains(damage.attacker.userID.ToString()))
                            {
                                if (damage.victim.idMain is StructureComponent)
                                {
                                    HashSet<StructureComponent> structureComponents = damage.victim.idMain.gameObject.GetComponent<StructureComponent>()._master._structureComponents;
                                    List<GameObject> toDestroy = new List<GameObject>();
                                    foreach (var kv in structureComponents)
                                    {
                                        if (kv.gameObject != null)
                                        {
                                            toDestroy.Add(kv.gameObject);
                                        }
                                    }
                                    foreach (GameObject GO in toDestroy)
                                    {
                                        if (GO != null)
                                        {
                                            beingDestroyed.Add(GO);
                                            NetCull.Destroy(GO);
                                        }
                                    }
                                }
                                if (damage.victim.idMain is DeployableObject)
                                {
                                    beingDestroyed.Add(damage.victim.idMain.gameObject);
                                    NetCull.Destroy(damage.victim.idMain.gameObject);
                                }
                            }
                            else
                            {
                                if (inZone(damage.victim.idMain.transform.position))
                                {
                                    damage.amount = 0f;
                                    damage.status = LifeStatus.IsAlive;
                                }

                                if (ownershipList.Contains(damage.attacker.userID.ToString()))
                                {
                                    damage.amount = 0f;
                                    damage.status = LifeStatus.IsAlive;
                                    string ownerUID = "";
                                    if (damage.victim.idMain is StructureComponent)
                                        ownerUID = (damage.victim.idMain as StructureComponent)._master.ownerID.ToString();

                                    if (damage.victim.idMain is DeployableObject)
                                        ownerUID = (damage.victim.idMain as DeployableObject).ownerID.ToString();

                                    Thread t = new Thread((DE) =>
                                        {
                                            Broadcast.noticeTo(((DamageEvent)DE).attacker.client.netPlayer, "✲", "This is owned by " + grabNameByUID(ownerUID) + "!");
                                        });
                                    t.Start(damage);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (damage.victim.idMain is Character)
                {
                    if (isPlayer(damage.victim.idMain))
                    {
                        if (godList.Contains(damage.victim.userID.ToString()))
                        {
                            damage.amount = 0f;
                            damage.status = LifeStatus.IsAlive;
                        }
                    }
                }
            }
        }

        public static void OnAIHurt(ref DamageEvent damage)
        {
            if (damage.attacker.idMain != null)
            {
                if (isPlayer(damage.attacker.idMain))
                {
                    PlayerClient attackerClient = damage.attacker.client;

                    if (destroyerList.Contains(attackerClient.userID.ToString()))
                    {
                        beingDestroyed.Add(damage.victim.idMain.gameObject);
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
            try
            {
                PlayerClient victim = damage.victim.client;
                TakeDamage takeDamage = victim.controllable.GetComponent<TakeDamage>();
                PlayerClient attacker = null;

                if (godList.Contains(victim.userID.ToString()))
                {
                    damage.amount = 0f;
                }

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

                    if (inSafeZone.ContainsKey(attacker) && !inWarZone.ContainsKey(attacker))
                    {
                        Broadcast.noticeTo(attacker.netPlayer, "☃", "You cannot hurt players while in a safe zone!");
                        damage.amount = 0f;
                    }
                    else if (inSafeZone.ContainsKey(victim) && !inWarZone.ContainsKey(victim))
                    {
                        Broadcast.noticeTo(attacker.netPlayer, "☃", "You cannot hurt players in a safe zone!");
                        damage.amount = 0f;
                    }
                }
                else
                {
                    if (inSafeZone.ContainsKey(victim) && !inWarZone.ContainsKey(victim))
                    {
                        damage.amount = 0f;
                    }
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
                    if (!inWarZone.ContainsKey(attacker) && !inWarZone.ContainsKey(victim))
                    {
                        if (!friendlyFire)
                        {
                            damage.amount = 0f;
                        }
                    }
                    else
                    {
                        damage.amount = damage.amount * warFriendlyDamage;
                    }
                }
                else
                {
                    if (damage.attacker.IsDifferentPlayer(victim) && alliances.ContainsKey(attackerFaction))
                    {
                        if (alliances[attackerFaction].Contains(victimFaction))
                        {
                            if (damage.damageTypes == DamageTypeFlags.damage_bullet)
                            {
                                Broadcast.sideNoticeTo(attacker.netPlayer, "You shot an ally");
                                Broadcast.sideNoticeTo(victim.netPlayer, "An ally shot you");
                            }
                            else
                            {
                                Broadcast.sideNoticeTo(attacker.netPlayer, "You hit an ally");
                                Broadcast.sideNoticeTo(victim.netPlayer, "An ally hit you");
                            }
                            if (!inWarZone.ContainsKey(attacker) && !inWarZone.ContainsKey(victim))
                            {
                                if (!alliedFire)
                                    damage.amount = (damage.amount * 2) / 3;
                            }
                            else
                            {
                                damage.amount = damage.amount * warAllyDamage;
                            }
                        }
                    }
                    else
                    {
                        if (!inWarZone.ContainsKey(attacker) && !inWarZone.ContainsKey(victim))
                        {
                            damage.amount = damage.amount * neutralDamage;
                        }
                        else
                        {
                            damage.amount = damage.amount * warDamage;
                        }
                    }

                    if (!godList.Contains(victim.userID.ToString()))
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

                if (takeDamage.health > damage.amount)
                {
                    damage.status = LifeStatus.IsAlive;
                }
                else
                {
                    damage.status = LifeStatus.WasKilled;
                }
            }
            catch { }
        }

        public static bool belongsTo(Controllable controllable, ulong ownerID)
        {
            if (controllable == null)
                return false;

            PlayerClient playerClient = controllable.playerClient;

            if (playerClient == null)
                return false;

            ulong userID = playerClient.userID;

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

        public static RustProto.User.Builder SetNameOnJoin(string value, RustProto.User.Builder builder)
        {
            Google.ProtocolBuffers.ThrowHelper.ThrowIfNull(value, "value");
            builder.PrepareBuilder();
            builder.result.hasDisplayname = true;

            string userName = value;
            try
            {
                foreach (KeyValuePair<string, string> kv in rankPrefixes)
                {
                    userName = userName.Replace("[" + kv.Key + "]", "");
                    userName = userName.Replace(kv.Value, "");
                }

                foreach (KeyValuePair<string, string> kv in playerPrefixes)
                {
                    userName = userName.Replace("[" + kv.Value + "]", "");
                }

                userName = userName.Replace("<G> ", "");
                userName = userName.Replace("* <G> ", "");
                userName = userName.Replace("<D> ", "");
                userName = userName.Replace("* <D> ", "");
                userName = userName.Replace("<F> ", "");
                userName = userName.Replace("* <F> ", "");

                bool nameOccupied = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == userName).Count() > 0;
                int instanceNum = 0;
                if (nameOccupied && !kickDuplicate)
                {
                    instanceNum = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == userName).Count();

                    nameOccupied = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == userName + " (" + instanceNum + ")").Count() > 0;
                    while (nameOccupied)
                    {
                        instanceNum++;
                        nameOccupied = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == userName + " (" + instanceNum + ")").Count() > 0;
                    }
                    string newName = userName + " (" + instanceNum + ")";
                }
            }
            catch (Exception ex) { Vars.conLog.Error(ex.ToString()); }

            builder.result.displayname_ = userName;
            return builder;
        }

        public static void OnUserConnected(NetUser user)
        {
            try
            {
                if (!AllPlayerClients.Contains(user.playerClient))
                    AllPlayerClients.Add(user.playerClient);

                if (!Vars.teleportRequests.ContainsKey(user.playerClient))
                {
                    Vars.teleportRequests.Add(user.playerClient, new Dictionary<PlayerClient, TimerPlus>());
                    Vars.latestRequests.Add(user.playerClient, null);
                }

                string steamUID = user.userID.ToString();
                if (!Vars.firstPlayerJoined)
                {
                    Vars.firstPlayerJoined = true;
                    Vars.loadEnvironment();
                    truth.punish = false;
                    truth.threshold = 999999999;
                }
                if (Vars.currentBans.ContainsKey(steamUID))
                {
                    Vars.kickPlayer(user, Vars.currentBanReasons[steamUID], true);
                }
                else
                {
                    bool containsIllegalWord = false;
                    List<string> illegalWords = new List<string>();
                    foreach (string s in illegalWords)
                    {
                        if (user.displayName.ToLower().Contains(s.ToLower()))
                        {
                            containsIllegalWord = true;
                            illegalWords.Add(s);
                        }
                    }

                    bool containsIllegalChar = false;
                    List<string> illegalChars = new List<string>();
                    foreach (char c in user.displayName)
                    {
                        if (!allowedChars.Contains(c.ToString().ToLower()) && !allowedChars.Contains(c.ToString()) && c != ' ' && c != ',')
                        {
                            illegalChars.Add(c.ToString());
                            containsIllegalChar = true;
                        }
                    }
                    bool nameOccupied = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == user.displayName).Count() - 1 > 0;
                    PlayerClient connectedClient = null;
                    int instanceNum = 0;
                    if (nameOccupied)
                    {
                        connectedClient = Array.Find(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == user.displayName && pc.userID != user.userID);
                        instanceNum = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == user.displayName).Count();
                    }
                    if ((censorship && containsIllegalWord) || (restrictChars && containsIllegalChar) || (user.displayName.Length > maximumNameCount) || (user.displayName.Length < minimumNameCount) || (kickDuplicate && nameOccupied) || (user.displayName == botName))
                    {
                        if (containsIllegalWord)
                            Vars.otherKick(user, "Illegal words in name: " + string.Join(", ", illegalWords.ToArray()));
                        else
                        {
                            if (containsIllegalChar)
                                Vars.otherKick(user, "Illegal characters in name: " + string.Join(", ", illegalChars.ToArray()));
                            else
                            {
                                if (user.displayName.Length > maximumNameCount)
                                    Vars.otherKick(user, "Name must be less than " + maximumNameCount + " characters.");
                                else
                                {
                                    if (user.displayName.Length < minimumNameCount)
                                        Vars.otherKick(user, "Name must be more than " + minimumNameCount + " characters.");
                                    else
                                    {
                                        if (nameOccupied)
                                        {
                                            if (lowerAuthority)
                                            {
                                                if (ofLowerRank(user.userID.ToString(), connectedClient.userID.ToString(), false))
                                                    Vars.otherKick(user, "Player name \"" + user.displayName + "\" already in use.");

                                                if (ofLowerRank(connectedClient.userID.ToString(), user.userID.ToString(), false))
                                                    Vars.otherKick(connectedClient.netUser, "Player of higher authority with name \"" + user.displayName + "\" joined.");
                                            }
                                            else
                                                Vars.otherKick(user, "Player name \"" + user.displayName + "\" already in use.");
                                        }
                                        else
                                        {
                                            if (user.displayName == botName)
                                                Vars.otherKick(user, "You cannot impersonate the server bot.");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!Vars.blockedRequestsPer.ContainsKey(user.userID.ToString()))
                            blockedRequestsPer.Add(user.userID.ToString(), new Dictionary<string, TimerPlus>());

                        if (Vars.useSteamGroup && Vars.groupMembers.Contains(steamUID))
                        {
                            Vars.conLog.Info("Player " + user.displayName + " (" + steamUID + ") has connected through steam group \"" + Vars.steamGroup + "\".");
                            Broadcast.broadcastTo(user.networkPlayer, "The server is running Rust Essentials v" + Vars.currentVersion + ".");
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
                            if (Vars.enableJoin && user.playerClient.userName.Length > 0)
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
                                Broadcast.broadcastTo(user.networkPlayer, "The server is running Rust Essentials v" + Vars.currentVersion + ".");
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
                                if (Vars.enableJoin && user.playerClient.userName.Length > 0)
                                {
                                    joinMessage = Vars.joinMessage.Replace("$USER$", Vars.filterFullNames(user.displayName, steamUID));
                                    Broadcast.broadcastJoinLeave(joinMessage);
                                }
                                Vars.conLog.Chat("<BROADCAST ALL> " + Vars.botName + ": " + joinMessage + " (" + steamUID + ")");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error(ex.ToString());
            }
        }

        public static void OnUserInitialize(NetUser user)
        {
            try
            {
                if (user != null)
                {
                    string userName = user.displayName;

                    RustProto.Avatar objB = user.LoadAvatar();
                    if (objB != null)
                    {
                        RustServerManagement RSM = RustServerManagement.Get();
                        if (RSM != null)
                        {
                            RSM.UpdateConnectingUserAvatar(user, ref user.avatar);
                            if (!object.ReferenceEquals(user.avatar, objB))
                            {
                                user.SaveAvatar();
                            }
                            if (user.playerClient != null)
                            {
                                if (RSM.SpawnPlayer(user.playerClient, false, user.avatar) != null)
                                {
                                    user.did_join = true;
                                    conLog.Info((vanishedList.Contains(user.userID.ToString()) ? "Vanished user" : userName) + " (" + user.userID + ") has joined the game world. Avatar loaded.");
                                    if (vanishedList.Contains(user.userID.ToString()))
                                    {
                                        addArmor(user.playerClient, "Invisible Helmet", true);
                                        addArmor(user.playerClient, "Invisible Vest", true);
                                        addArmor(user.playerClient, "Invisible Pants", true);
                                        addArmor(user.playerClient, "Invisible Boots", true);
                                    }
                                }
                            }
                            else
                            {
                               conLog.Info("User " + userName + " (" + user.userID.ToString() + ") joined but the instance of PlayerClient is null! DO NOT IGNORE THIS!");
                            }
                        }
                        else
                            conLog.Info("User " + userName + " (" + user.userID.ToString() + ") joined but the RSM is null!");
                    }
                    else
                        conLog.Info("User " + userName + " (" + user.userID.ToString() + ") joined but the avatar could not be loaded!");
                }
                else
                    conLog.Info("User was null when joining! DO NOT IGNORE THIS!");
            }
            catch (Exception ex) { Vars.conLog.Error(ex.ToString()); }
        }

        public static void OnUserDisconnected(uLink.NetworkPlayer player, ConnectionAcceptor CA)
        {
            try
            {
                if (CA != null)
                {
                    if (player != null)
                    {
                        object localData = player.GetLocalData();
                        if (localData != null)
                        {
                            RustServerManagement RSM = RustServerManagement.Get();
                            if (localData is NetUser)
                            {
                                NetUser user = (NetUser)localData;
                                PlayerClient playerClient = user.playerClient;
                                List<PlayerClient> possibleClient = new List<PlayerClient>();
                                try
                                {
                                    if (playerClient == null || playerClient.netUser == null || playerClient.netPlayer == null || playerClient.userName == null)
                                        possibleClient = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netUser == user || pc.userID == user.userID || pc.netPlayer == user.networkPlayer || pc.netPlayer == player).ToList();

                                    if (possibleClient.Count() == 1)
                                        playerClient = possibleClient[0];

                                    if (possibleClient.Count == 0 || playerClient == null || playerClient.netUser == null || playerClient.netPlayer == null || playerClient.userName == null)
                                    {
                                        possibleClient.Clear();
                                        LockedList<PlayerClient> playerClients = PlayerClient.All;
                                        possibleClient = Array.FindAll(playerClients.ToArray(), (PlayerClient pc) => pc.netUser == user || pc.userID == user.userID || pc.netPlayer == user.networkPlayer || pc.netPlayer == player).ToList();
                                    }

                                    if (possibleClient.Count() == 1)
                                        playerClient = possibleClient[0];

                                    if (possibleClient.Count == 0 || playerClient == null || playerClient.netUser == null || playerClient.netPlayer == null || playerClient.userName == null)
                                    {
                                        possibleClient.Clear();
                                        List<PlayerClient> playerClients = RSM._playerClientList;
                                        possibleClient = Array.FindAll(playerClients.ToArray(), (PlayerClient pc) => pc.netUser == user || pc.userID == user.userID || pc.netPlayer == user.networkPlayer || pc.netPlayer == player).ToList();
                                    }

                                    if (possibleClient.Count() == 1)
                                        playerClient = possibleClient[0];

                                    if (possibleClient.Count == 0 || playerClient == null || playerClient.netUser == null || playerClient.netPlayer == null || playerClient.userName == null)
                                        playerClient = null;

                                    if (playerClient == null)
                                        conLog.Error("Could not find a proper playerclient after many tries!");
                                }
                                catch (Exception ex)
                                {
                                    conLog.Error("Could not find a proper playerclient: " + ex.ToString());
                                }

                                if (user != null && playerClient != null)
                                {
                                    if (latestPM.ContainsKey(playerClient))
                                        latestPM.Remove(playerClient);

                                    if (latestRequests.ContainsKey(playerClient))
                                        latestRequests.Remove(playerClient);

                                    if (latestFactionRequests.ContainsKey(playerClient))
                                        latestFactionRequests.Remove(playerClient);

                                    if (killList.Contains(playerClient))
                                        killList.Remove(playerClient);

                                    if (isTeleporting.Contains(playerClient))
                                        isTeleporting.Remove(playerClient);

                                    if (isAccepting.Contains(playerClient))
                                        isAccepting.Remove(playerClient);

                                    if (wasHit.Contains(playerClient))
                                        wasHit.Remove(playerClient);

                                    if (inSafeZone.ContainsKey(playerClient))
                                        inSafeZone.Remove(playerClient);

                                    if (inWarZone.ContainsKey(playerClient))
                                        inWarZone.Remove(playerClient);

                                    if (firstPoints.ContainsKey(playerClient))
                                        firstPoints.Remove(playerClient);

                                    if (secondPoints.ContainsKey(playerClient))
                                        secondPoints.Remove(playerClient);

                                    if (blockedRequestsPer.ContainsKey(playerClient.userID.ToString()))
                                    {
                                        if (blockedRequestsPer[playerClient.userID.ToString()].Count < 1)
                                            blockedRequestsPer.Remove(playerClient.userID.ToString());
                                    }

                                    if (teleportRequests.ContainsKey(playerClient))
                                        teleportRequests.Remove(playerClient);

                                    if (AllPlayerClients.Contains(playerClient))
                                        AllPlayerClients.Remove(playerClient);

                                    string leaveMessage = "";
                                    if (Vars.enableLeave && !Vars.kickQueue.Contains(playerClient.userID.ToString()) && playerClient.userName.Length > 0)
                                    {
                                        leaveMessage = Vars.leaveMessage.Replace("$USER$", Vars.filterFullNames(playerClient.userName, playerClient.userID.ToString()));
                                        Broadcast.broadcastJoinLeave(leaveMessage);
                                        Vars.conLog.Chat("<BROADCAST ALL> " + Vars.botName + ": " + leaveMessage);
                                    }

                                    user.connection.netUser = null;
                                    CA.m_Connections.Remove(user.connection);
                                    bool b = true;
                                    if (Vars.kickQueue.Contains(playerClient.userID.ToString()) || playerClient.userName.Length == 0)
                                    {
                                        if (Vars.kickQueue.Contains(playerClient.userID.ToString()))
                                            Vars.kickQueue.Remove(playerClient.userID.ToString());
                                        b = false;
                                    }
                                    try
                                    {
                                        if (playerClient != null)
                                        {
                                            if (user == null)
                                                user = playerClient.netUser;
                                            if (user != null)
                                            {
                                                try
                                                {
                                                    Controllable controllable = playerClient.controllable;
                                                    if (controllable != null)
                                                    {
                                                        Character forCharacter = controllable.character;
                                                        try
                                                        {
                                                            RSM.SaveAvatar(forCharacter);
                                                        }
                                                        catch (Exception exception)
                                                        {
                                                            conLog.Error("SA: " + exception);
                                                        }
                                                        if (forCharacter != null)
                                                        {
                                                            try
                                                            {
                                                                RSM.ShutdownAvatar(forCharacter);
                                                            }
                                                            catch (Exception exception)
                                                            {
                                                                conLog.Error("SDA: " + exception);
                                                            }
                                                            try
                                                            {
                                                                Character.DestroyCharacter(forCharacter);
                                                            }
                                                            catch (Exception exception)
                                                            {
                                                                conLog.Error("CDC: " + exception);
                                                            }
                                                        }
                                                    }
                                                    try
                                                    {
                                                        RSM._playerClientList.Remove(playerClient);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        conLog.Error("COULD NOT REMOEVE PLAYERCLIENT FROM LIST: " + ex.ToString());
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    conLog.Error("ECFC: " + ex.ToString());
                                                }
                                            }
                                            else
                                                conLog.Info("COULD NOT EARSE CHARACTERS FOR CLIENT!");
                                        }
                                        if (player == null)
                                            player = playerClient.netPlayer;
                                        if (player != null)
                                            NetCull.DestroyPlayerObjects(player);
                                        else
                                            conLog.Info("COULD NOT DESTROY PLAYER OBJECTS!");
                                        if (user == null)
                                            user = playerClient.netUser;
                                        if (user != null)
                                            CullGrid.ClearPlayerCulling(user);
                                        else
                                            conLog.Info("COULD NOT CLEAR PLAYER CULLING!");
                                        if (player == null)
                                            player = playerClient.netPlayer;
                                        if (player != null)
                                            NetCull.RemoveRPCs(player);
                                        else
                                            conLog.Info("COULD NOT REMOVE RPCS!");
                                    }
                                    catch (Exception exception)
                                    {
                                        conLog.Error("#1: " + exception.ToString());
                                        conLog.Error("DO NOT IGNORE THE ERROR ABOVE. THESE THINGS SHOULD NOT BE FAILING. EVER.");
                                    }
                                    if (b)
                                        conLog.Info("Player " + user.displayName + " (" + user.userID + ") disconnected. Data unloaded.");
                                    Rust.Steam.Server.OnUserLeave(user.connection.UserID);
                                    try
                                    {
                                        user.Dispose();
                                    }
                                    catch (Exception exception2)
                                    {
                                        conLog.Error("#2: " + exception2.ToString());
                                        conLog.Error("DO NOT IGNORE THE ERROR ABOVE. THESE THINGS SHOULD NOT BE FAILING. EVER.");
                                    }
                                }
                                else
                                {
                                    conLog.Error("So... A user disconnected but his/her NetUser/PlayerClient was null... Shit. Things will break due to this.");
                                    LockedList<PlayerClient> playerClients = PlayerClient.All;
                                    foreach (PlayerClient pc in playerClients)
                                    {
                                        if (!AllPlayerClients.Contains(pc))
                                        {
                                            conLog.Error("Fixing the issue with the PlayerClient...");
                                            try
                                            {
                                                if (latestPM.ContainsKey(playerClient))
                                                    latestPM.Remove(playerClient);

                                                if (latestRequests.ContainsKey(playerClient))
                                                    latestRequests.Remove(playerClient);

                                                if (latestFactionRequests.ContainsKey(playerClient))
                                                    latestFactionRequests.Remove(playerClient);

                                                if (killList.Contains(playerClient))
                                                    killList.Remove(playerClient);

                                                if (isTeleporting.Contains(playerClient))
                                                    isTeleporting.Remove(playerClient);

                                                if (isAccepting.Contains(playerClient))
                                                    isAccepting.Remove(playerClient);

                                                if (wasHit.Contains(playerClient))
                                                    wasHit.Remove(playerClient);

                                                if (inSafeZone.ContainsKey(playerClient))
                                                    inSafeZone.Remove(playerClient);

                                                if (inWarZone.ContainsKey(playerClient))
                                                    inWarZone.Remove(playerClient);

                                                if (firstPoints.ContainsKey(playerClient))
                                                    firstPoints.Remove(playerClient);

                                                if (secondPoints.ContainsKey(playerClient))
                                                    secondPoints.Remove(playerClient);

                                                if (blockedRequestsPer.ContainsKey(playerClient.userID.ToString()))
                                                {
                                                    if (blockedRequestsPer[playerClient.userID.ToString()].Count < 1)
                                                        blockedRequestsPer.Remove(playerClient.userID.ToString());
                                                }

                                                if (teleportRequests.ContainsKey(playerClient))
                                                    teleportRequests.Remove(playerClient);

                                                if (AllPlayerClients.Contains(playerClient))
                                                    AllPlayerClients.Remove(playerClient);

                                                string leaveMessage = "";
                                                if (Vars.enableLeave && !Vars.kickQueue.Contains(playerClient.userID.ToString()) && playerClient.userName.Length > 0)
                                                {
                                                    leaveMessage = Vars.leaveMessage.Replace("$USER$", Vars.filterFullNames(playerClient.userName, playerClient.userID.ToString()));
                                                    Broadcast.broadcastJoinLeave(leaveMessage);
                                                    Vars.conLog.Chat("<BROADCAST ALL> " + Vars.botName + ": " + leaveMessage);
                                                }

                                                user.connection.netUser = null;
                                                CA.m_Connections.Remove(user.connection);
                                                bool b = true;
                                                if (Vars.kickQueue.Contains(playerClient.userID.ToString()) || playerClient.userName.Length == 0)
                                                {
                                                    if (Vars.kickQueue.Contains(playerClient.userID.ToString()))
                                                        Vars.kickQueue.Remove(playerClient.userID.ToString());
                                                    b = false;
                                                }
                                                try
                                                {
                                                    if (playerClient != null)
                                                    {
                                                        RSM.EraseCharactersForClient(playerClient, true, user);
                                                    }
                                                    NetCull.DestroyPlayerObjects(player);
                                                    CullGrid.ClearPlayerCulling(user);
                                                    NetCull.RemoveRPCs(player);
                                                }
                                                catch (Exception exception)
                                                {
                                                    conLog.Error("#3: " + exception.ToString());
                                                    conLog.Error("DO NOT IGNORE THE ERROR ABOVE. THESE THINGS SHOULD NOT BE FAILING. EVER.");
                                                }
                                                if (b)
                                                    conLog.Info("Player " + user.displayName + " (" + user.userID + ") disconnected. Data unloaded.");
                                                Rust.Steam.Server.OnUserLeave(user.connection.UserID);
                                                try
                                                {
                                                    user.Dispose();
                                                }
                                                catch (Exception exception2)
                                                {
                                                    conLog.Error("#4: " + exception2.ToString());
                                                    conLog.Error("DO NOT IGNORE THE ERROR ABOVE. THESE THINGS SHOULD NOT BE FAILING. EVER.");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                conLog.Error("Something went TERRIBLY TERRIBLY wrong. Contact MistaD ASAP. Send him this: " + ex.ToString());
                                            }
                                        }
                                    }
                                }
                            }
                            else if (localData is ClientConnection)
                            {
                                ClientConnection item = (ClientConnection)localData;
                                CA.m_Connections.Remove(item);
                                ConsoleSystem.Print("User Disconnected: (unconnected " + player.ipAddress + ")", false);
                            }

                            player.SetLocalData(null);
                            Rust.Steam.Server.OnPlayerCountChanged();
                        }
                        else
                            conLog.Error("User attempted to disconnect but the localData was corrupted.");
                    }
                    else
                        conLog.Error("User attempted to disconnect but the NetworkPlayer was corrupted.");
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("OUD: " + ex.ToString());
            }
        }

        public static void saveServer()
        {
            try
            {
                ConsoleSystem.Run("save.all");
                Broadcast.broadcastAll("All data has been saved.");
            }
            catch (Exception ex)
            {
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

        public static void listWarps(string rank, PlayerClient senderClient)
        {
            List<string> warps = new List<string>();
            List<string> otherWarps = new List<string>();
            if (rank == Vars.defaultRank)
            {
                foreach (string s in Vars.unassignedWarps)
                {
                    otherWarps.Add(s);
                }
            }
            else
            {
                foreach (string s in Vars.warpsForRanks[rank])
                {
                    otherWarps.Add(s);
                }
                if (warpsForUIDs.ContainsKey(senderClient.userID.ToString()))
                {
                    foreach (string s in Vars.warpsForUIDs[senderClient.userID.ToString()])
                    {
                        otherWarps.Add(s);
                    }
                }
            }

            List<string> otherWarps2 = new List<string>();
            while (otherWarps.Count > 0)
            {
                int curIndex = 0;
                warps.Clear();
                otherWarps2.Clear();
                foreach (string s in otherWarps)
                {
                    curIndex++;
                    if (curIndex < 9)
                    {
                        warps.Add(s);
                        otherWarps2.Add(s);
                    }
                    else
                        break;
                }
                foreach (string s in otherWarps2)
                {
                    otherWarps.Remove(s);
                }
                Broadcast.broadcastTo(senderClient.netPlayer, string.Join(", ", warps.ToArray()), true);
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
                if (kitsForUIDs.ContainsKey(senderClient.userID.ToString()))
                {
                    foreach (string s in Vars.kitsForUIDs[senderClient.userID.ToString()])
                    {
                        otherKits.Add(s);
                    }
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
                int duration = 2;
                bool changedDuration = false;
                if (args[args.Count() - 1].EndsWith("s"))
                {
                    if (int.TryParse(args[args.Count() - 1].Substring(0, args[args.Count() - 1].Length - 1), out duration))
                    {
                        duration = Mathf.Clamp(duration, 1, 7);
                        changedDuration = true;
                    }
                }

                if (args[1].Length < 3 && args.Count() > 2)
                {
                    List<string> messageList = new List<string>();
                    int curIndex = 0;
                    foreach (string s in args)
                    {
                        if (curIndex > 1)
                        {
                            if (!changedDuration || curIndex != args.Count() - 1)
                                messageList.Add(s);
                        }
                        curIndex++;
                    }

                    Broadcast.noticeAll(args[1], string.Join(" ", messageList.ToArray()), duration);
                }
                else
                {
                    List<string> messageList = new List<string>();
                    int curIndex = 0;
                    foreach (string s in args)
                    {
                        if (curIndex > 0)
                        {
                            if (!changedDuration || curIndex != args.Count() - 1)
                                messageList.Add(s);
                        }
                        curIndex++;
                    }

                    Broadcast.noticeAll("!", string.Join(" ", messageList.ToArray()), duration);
                }
            }
        }

        public static void kickAllServer()
        {
            List<PlayerClient> APC = AllPlayerClients;
            foreach (PlayerClient targetClient in APC)
            {
                kickPlayer(targetClient.netUser, "All users were kicked.", false);
            }
        }

        public static void kickAll(PlayerClient senderClient)
        {
            List<PlayerClient> APC = AllPlayerClients;
            foreach (PlayerClient targetClient in APC)
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
                        if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                        {
                            playerName = playerName.Substring(1, playerName.Length - 2);

                            PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(playerName));
                            if (possibleTargets.Count() == 0)
                                Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal \"" + playerName + "\".");
                            else if (possibleTargets.Count() > 1)
                                Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + playerName + "\".");
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
                        else
                        {
                            PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
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

                if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                {
                    playerName = playerName.Substring(1, playerName.Length - 2);

                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(playerName));
                    if (possibleTargets.Count() == 0)
                        Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal \"" + playerName + "\".");
                    else if (possibleTargets.Count() > 1)
                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + playerName + "\".");
                    else
                    {
                        PlayerClient targetClient = possibleTargets[0];

                        if (teleportRequests[senderClient].ContainsKey(targetClient))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting " + targetClient.userName + " in " + requestDelay + " seconds...");
                            Broadcast.broadcastTo(targetClient.netPlayer, "Teleporting to " + senderClient.userName + " in " + requestDelay + " seconds. Do not move...");
                            Thread t = new Thread(() => teleporting(targetClient, senderClient));
                            t.Start();
                            isTeleporting.Add(targetClient);
                            isAccepting.Add(senderClient);
                            if (requestCooldownType == 1 && !blockedRequestsPer[targetClient.userID.ToString()].ContainsKey(senderClient.userID.ToString()))
                            {
                                TimerPlus t1 = new TimerPlus();
                                t1.AutoReset = false;
                                t1.Interval = requestCooldown;
                                t1.Elapsed += (sender, e) => unblockRequests(senderClient.userID.ToString(), targetClient.userID.ToString());
                                t1.Start();
                                blockedRequestsPer[targetClient.userID.ToString()].Add(senderClient.userID.ToString(), t1);
                            }
                            if (requestCooldownType == 2 && !blockedRequestsAll.ContainsKey(targetClient.userID.ToString()))
                            {
                                TimerPlus t1 = new TimerPlus();
                                t1.AutoReset = false;
                                t1.Interval = requestCooldown;
                                t1.Elapsed += (sender, e) => unblockRequests(senderClient.userID.ToString(), targetClient.userID.ToString());
                                t1.Start();
                                blockedRequestsAll.Add(targetClient.userID.ToString(), t1);
                            }
                        }
                        else
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You do not have a teleport request from " + targetClient.userName + ".");
                        }
                    }
                }
                else
                {
                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                    if (possibleTargets.Count() == 0)
                        Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                    else if (possibleTargets.Count() > 1)
                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                    else
                    {
                        PlayerClient targetClient = possibleTargets[0];

                        if (teleportRequests[senderClient].ContainsKey(targetClient))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting " + targetClient.userName + " in " + requestDelay + " seconds...");
                            Broadcast.broadcastTo(targetClient.netPlayer, "Teleporting to " + senderClient.userName + " in " + requestDelay + " seconds. Do not move...");
                            Thread t = new Thread(() => teleporting(targetClient, senderClient));
                            t.Start();
                            isTeleporting.Add(targetClient);
                            isAccepting.Add(senderClient);

                            if (requestCooldownType == 1 && !blockedRequestsPer[targetClient.userID.ToString()].ContainsKey(senderClient.userID.ToString()))
                            {
                                TimerPlus t1 = new TimerPlus();
                                t1.AutoReset = false;
                                t1.Interval = requestCooldown;
                                t1.Elapsed += (sender, e) => unblockRequests(senderClient.userID.ToString(), targetClient.userID.ToString());
                                t1.Start();
                                blockedRequestsPer[targetClient.userID.ToString()].Add(senderClient.userID.ToString(), t1);
                            }
                            if (requestCooldownType == 2 && !blockedRequestsAll.ContainsKey(targetClient.userID.ToString()))
                            {
                                TimerPlus t1 = new TimerPlus();
                                t1.AutoReset = false;
                                t1.Interval = requestCooldown;
                                t1.Elapsed += (sender, e) => unblockRequests(senderClient.userID.ToString(), targetClient.userID.ToString());
                                t1.Start();
                                blockedRequestsAll.Add(targetClient.userID.ToString(), t1);
                            }
                        }
                        else
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You do not have a teleport request from " + targetClient.userName + ".");
                        }
                    }
                }
            }
            else
            {
                if (latestRequests[senderClient] != null)
                {
                    PlayerClient targetClient = latestRequests[senderClient];
                    Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting " + targetClient.userName + " in " + requestDelay + " seconds...");
                    Broadcast.broadcastTo(targetClient.netPlayer, "Teleporting to " + senderClient.userName + " in " + requestDelay + " seconds. Do not move...");
                    Thread t = new Thread(() => teleporting(targetClient, senderClient));
                    t.Start();
                    isTeleporting.Add(targetClient);
                    if (requestCooldownType == 1 && !blockedRequestsPer[targetClient.userID.ToString()].ContainsKey(senderClient.userID.ToString()))
                    {
                        TimerPlus t1 = new TimerPlus();
                        t1.AutoReset = false;
                        t1.Interval = requestCooldown;
                        t1.Elapsed += (sender, e) => unblockRequests(senderClient.userID.ToString(), targetClient.userID.ToString());
                        t1.Start();
                        blockedRequestsPer[targetClient.userID.ToString()].Add(senderClient.userID.ToString(), t1);
                    }
                    if (requestCooldownType == 2 && !blockedRequestsAll.ContainsKey(targetClient.userID.ToString()))
                    {
                        TimerPlus t1 = new TimerPlus();
                        t1.AutoReset = false;
                        t1.Interval = requestCooldown;
                        t1.Elapsed += (sender, e) => unblockRequests(senderClient.userID.ToString(), targetClient.userID.ToString());
                        t1.Start();
                        blockedRequestsAll.Add(targetClient.userID.ToString(), t1);
                    }
                }
                else
                    Broadcast.broadcastTo(senderClient.netPlayer, "You have no teleport requests to accept!");
            }
        }

        public static void warping(PlayerClient senderClient, Vector3 destination)
        {
            bool b = true;
            int timeElapsed = 0;
            RustServerManagement serverManagement = RustServerManagement.Get();
            Character senderChar;
            Character.FindByUser(senderClient.userID, out senderChar);

            Vector3 oldPos = senderChar.transform.position;
            while (b)
            {
                if (wasHit.Contains(senderClient))
                {
                    isTeleporting.Remove(senderClient);
                    wasHit.Remove(senderClient);
                    Broadcast.broadcastTo(senderClient.netPlayer, "Warp was interrupted due to damage.");
                    b = false;
                    break;
                }
                if (timeElapsed >= warpDelay)
                {
                    Vector3 oldDest = destination;
                    oldDest.y += 1;
                    if (Vector3.Distance(oldPos, destination) > 375)
                        destination.y += 8;

                    serverManagement.TeleportPlayerToWorld(senderClient.netPlayer, destination);
                    TimerPlus tp = new TimerPlus();
                    tp.AutoReset = false;
                    tp.Interval = 500;
                    tp.Elapsed += ((sender, e) => teleportToWorldElapsed(senderClient.netPlayer, oldDest));
                    tp.Start();
                    isTeleporting.Remove(senderClient);
                    b = false;
                    break;
                }
                Vector3 newPos = senderChar.transform.position;
                if (Vector3.Distance(oldPos, newPos) > 3)
                {
                    Broadcast.broadcastTo(senderClient.netPlayer, "You moved. Warp cancelled.");
                    isTeleporting.Remove(senderClient);
                    b = false;
                    break;
                }
                Thread.Sleep(1000);
                timeElapsed++;
            }
        }

        public static void teleportToWorldElapsed(uLink.NetworkPlayer np, Vector3 v3)
        {
            RustServerManagement serverManagement = RustServerManagement.Get();
            serverManagement.TeleportPlayerToWorld(np, v3);
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
                    teleportRequests[senderClient][targetClient].Close();
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
                    teleportRequests[senderClient][targetClient].Close();
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
                if (timeElapsed >= requestDelay)
                {
                    Character senderChar;
                    Character.FindByUser(senderClient.userID, out senderChar);

                    Vector3 destinationPos = senderChar.transform.position;

                    Vector3 oldDest = destinationPos;
                    oldDest.y += 1;
                    if (Vector3.Distance(destinationPos, oldPos) > 375)
                        destinationPos.y += 8;

                    serverManagement.TeleportPlayerToWorld(targetClient.netPlayer, destinationPos);
                    TimerPlus tp = new TimerPlus();
                    tp.AutoReset = false;
                    tp.Interval = 500;
                    tp.Elapsed += ((sender, e) => teleportToWorldElapsed(targetClient.netPlayer, oldDest));
                    tp.Start();
                    teleportRequests[senderClient][targetClient].Close();
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
                    Broadcast.broadcastTo(senderClient.netPlayer, targetClient.userName + " moved. Teleportation cancelled.");
                    Broadcast.broadcastTo(targetClient.netPlayer, "Teleportation cancelled.");
                    teleportRequests[senderClient][targetClient].Close();
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

        public void TeleportPlayerToPlayer(uLink.NetworkPlayer target, uLink.NetworkPlayer destination)
        {
        }

        public static void teleportRequest(PlayerClient senderClient, string[] args)
        {
            if (teleportRequestOn)
            {
                bool b = true;
                if (denyRequestWarzone && inWarZone.ContainsKey(senderClient))
                    b = false;

                if (b)
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

                        if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                        {
                            playerName = playerName.Substring(1, playerName.Length - 2);

                            PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(playerName));
                            if (possibleTargets.Count() == 0)
                                Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal \"" + playerName + "\".");
                            else if (possibleTargets.Count() > 1)
                                Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + playerName + "\".");
                            else
                            {
                                PlayerClient targetClient = possibleTargets[0];
                                if (requestCooldownType == 1 && blockedRequestsPer[senderClient.userID.ToString()].ContainsKey(targetClient.userID.ToString()))
                                {
                                    double timeLeft = Math.Round((blockedRequestsPer[senderClient.userID.ToString()][targetClient.userID.ToString()].TimeLeft / 1000));
                                    if (timeLeft > 0)
                                    {
                                        Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport to that player for " + timeLeft + " second(s).");
                                        return;
                                    }
                                }
                                if (requestCooldownType == 2 && blockedRequestsAll.ContainsKey(senderClient.userID.ToString()))
                                {
                                    double timeLeft = Math.Round((blockedRequestsAll[senderClient.userID.ToString()].TimeLeft / 1000));
                                    if (timeLeft > 0)
                                    {
                                        Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport to anyone for " + timeLeft + " second(s).");
                                        return;
                                    }
                                }
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
                        else
                        {
                            PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                            if (possibleTargets.Count() == 0)
                                Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                            else if (possibleTargets.Count() > 1)
                                Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                            else
                            {
                                PlayerClient targetClient = possibleTargets[0];
                                if (requestCooldownType == 1 && blockedRequestsPer[senderClient.userID.ToString()].ContainsKey(targetClient.userID.ToString()))
                                {
                                    double timeLeft = Math.Round((blockedRequestsPer[senderClient.userID.ToString()][targetClient.userID.ToString()].TimeLeft / 1000));
                                    if (timeLeft > 0)
                                    {
                                        Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport to that player for " + timeLeft + " second(s).");
                                        return;
                                    }
                                }
                                if (requestCooldownType == 2 && blockedRequestsAll.ContainsKey(senderClient.userID.ToString()))
                                {
                                    double timeLeft = Math.Round((blockedRequestsAll[senderClient.userID.ToString()].TimeLeft / 1000));
                                    if (timeLeft > 0)
                                    {
                                        Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport to anyone for " + timeLeft + " second(s).");
                                        return;
                                    }
                                }
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
                }
                else
                {
                    Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport while in a war zone!");
                }
            }
            else
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "Teleport requesting is disabled on this server.");
            }
        }

        public static void unblockRequests(string toRemove, string removeFrom)
        {
            if (blockedRequestsAll.ContainsKey(removeFrom))
            {
                blockedRequestsAll.Remove(removeFrom);
            }
            if (blockedRequestsPer[removeFrom].ContainsKey(toRemove))
            {
                blockedRequestsPer[removeFrom].Remove(toRemove);
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

        public static void teleportHere(PlayerClient senderClient, string[] args)
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
                    if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                    {
                        playerName = playerName.Substring(1, playerName.Length - 2);

                        PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(playerName));
                        if (possibleTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal \"" + playerName + "\".");
                        else if (possibleTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + playerName + "\".");
                        else
                        {
                            PlayerClient targetClient = possibleTargets[0];
                            Character senderChar;
                            Character.FindByUser(senderClient.userID, out senderChar);
                            Character targetChar;
                            Character.FindByUser(targetClient.userID, out targetChar);
                            Vector3 destination = senderChar.transform.position;

                            Vector3 oldDest = destination;
                            oldDest.y += 1;
                            if (Vector3.Distance(destination, targetChar.transform.position) > 375)
                                destination.y += 8;
                            serverManagement.TeleportPlayerToWorld(targetClient.netPlayer, destination);
                            TimerPlus tp = new TimerPlus();
                            tp.AutoReset = false;
                            tp.Interval = 500;
                            tp.Elapsed += ((sender, e) => teleportToWorldElapsed(targetClient.netPlayer, oldDest));
                            tp.Start();
                            Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting " + targetClient.userName + " here...");
                        }
                    }
                    else
                    {
                        PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                        if (possibleTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                        else if (possibleTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                        else
                        {
                            PlayerClient targetClient = possibleTargets[0];
                            Character senderChar;
                            Character.FindByUser(senderClient.userID, out senderChar);
                            Character targetChar;
                            Character.FindByUser(targetClient.userID, out targetChar);
                            Vector3 destination = senderChar.transform.position;

                            Vector3 oldDest = destination;
                            oldDest.y += 1;
                            if (Vector3.Distance(destination, targetChar.transform.position) > 375)
                                destination.y += 8;
                            serverManagement.TeleportPlayerToWorld(targetClient.netPlayer, destination);
                            TimerPlus tp = new TimerPlus();
                            tp.AutoReset = false;
                            tp.Interval = 500;
                            tp.Elapsed += ((sender, e) => teleportToWorldElapsed(targetClient.netPlayer, oldDest));
                            tp.Start();
                            Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting " + targetClient.userName + " here...");
                        }
                    }
                }
            }
        }

        public static void warpPlayer(PlayerClient senderClient, string[] args)
        {
            bool b = true;
            if (denyRequestWarzone && inWarZone.ContainsKey(senderClient))
                b = false;

            if (b)
            {
                if (args.Count() > 1)
                {
                    RustServerManagement serverManagement = RustServerManagement.Get();

                    List<string> warpNameList = new List<string>();
                    int curIndex = 0;
                    foreach (string s in args)
                    {
                        if (curIndex > 0)
                            warpNameList.Add(s);
                        curIndex++;
                    }
                    string warpName = string.Join(" ", warpNameList.ToArray());
                    string warpNameToLower = warpName.ToLower();

                    if (warps.ContainsKey(warpNameToLower))
                    {
                        if (warpsForRanks.ContainsKey(findRank(senderClient.userID.ToString())))
                        {
                            if (warpsForRanks[findRank(senderClient.userID.ToString())].Contains(warpNameToLower))
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, "Warping in " + warpDelay + " seconds...");
                                Thread t = new Thread(() => warping(senderClient, warps[warpNameToLower]));
                                t.Start();
                                isTeleporting.Add(senderClient);
                            }
                            else
                            {
                                if (warpsForUIDs.ContainsKey(senderClient.userID.ToString()))
                                {
                                    if (warpsForUIDs[senderClient.userID.ToString()].Contains(warpNameToLower))
                                    {
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Warping in " + warpDelay + " seconds...");
                                        Thread t = new Thread(() => warping(senderClient, warps[warpNameToLower]));
                                        t.Start();
                                        isTeleporting.Add(senderClient);
                                    }
                                    else
                                    {
                                        Broadcast.noticeTo(senderClient.netPlayer, ":(", "You do not have permission to do this.");
                                    }
                                }
                                else
                                {
                                    Broadcast.noticeTo(senderClient.netPlayer, ":(", "You do not have permission to do this.");
                                }
                            }
                        }
                        else
                        {
                            if (unassignedWarps.Contains(warpNameToLower))
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, "Warping in " + warpDelay + " seconds...");
                                Thread t = new Thread(() => warping(senderClient, warps[warpNameToLower]));
                                t.Start();
                                isTeleporting.Add(senderClient);
                            }
                            else
                            {
                                if (warpsForUIDs.ContainsKey(senderClient.userID.ToString()))
                                {
                                    if (warpsForUIDs[senderClient.userID.ToString()].Contains(warpNameToLower))
                                    {
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Warping in " + warpDelay + " seconds...");
                                        Thread t = new Thread(() => warping(senderClient, warps[warpNameToLower]));
                                        t.Start();
                                        isTeleporting.Add(senderClient);
                                    }
                                    else
                                    {
                                        Broadcast.noticeTo(senderClient.netPlayer, ":(", "You do not have permission to do this.");
                                    }
                                }
                                else
                                {
                                    Broadcast.noticeTo(senderClient.netPlayer, ":(", "You do not have permission to do this.");
                                }
                            }
                        }
                    }
                    else
                        Broadcast.broadcastTo(senderClient.netPlayer, "No such warp named \"" + warpName + "\".");
                }
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport while in a war zone!");
        }

        public static void teleport(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                RustServerManagement serverManagement = RustServerManagement.Get();
                List<string> playerNameList = new List<string>();
                int quotedNames = 0;
                bool searchingQuotes = false;
                int curIndex = 0;
                foreach (string s in args)
                {
                    if (curIndex > 0)
                    {
                        playerNameList.Add(s);

                        if (s.StartsWith("\"") && !searchingQuotes)
                            searchingQuotes = true;
                        
                        if (playerNameList.Count > 1 && searchingQuotes && s.EndsWith("\""))
                        {
                            quotedNames++;
                        }
                    }

                    curIndex++;
                }
                string playerName = string.Join(" ", playerNameList.ToArray());

                if (args.Count() == 2 || quotedNames == 1)
                {
                    if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                    {
                        playerName = playerName.Substring(1, playerName.Length - 2);

                        PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(playerName));
                        if (possibleTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal \"" + playerName + "\".");
                        else if (possibleTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + playerName + "\".");
                        else
                        {
                            PlayerClient targetClient = possibleTargets[0];
                            Character senderChar;
                            Character.FindByUser(senderClient.userID, out senderChar);
                            Character targetChar;
                            Character.FindByUser(targetClient.userID, out targetChar);
                            Vector3 destination = targetChar.transform.position;

                            Vector3 oldDest = destination;
                            oldDest.y += 1;
                            if (Vector3.Distance(destination, senderChar.transform.position) > 375)
                                destination.y += 8;

                            serverManagement.TeleportPlayerToWorld(senderClient.netPlayer, destination);
                            TimerPlus tp = new TimerPlus();
                            tp.AutoReset = false;
                            tp.Interval = 500;
                            tp.Elapsed += ((sender, e) => teleportToWorldElapsed(senderClient.netPlayer, oldDest));
                            tp.Start();
                            Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting to " + targetClient.userName + "...");
                        }
                    }
                    else
                    {
                        PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                        if (possibleTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                        else if (possibleTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                        else
                        {
                            PlayerClient targetClient = possibleTargets[0];
                            Character senderChar;
                            Character.FindByUser(senderClient.userID, out senderChar);
                            Character targetChar;
                            Character.FindByUser(targetClient.userID, out targetChar);
                            Vector3 destination = targetChar.transform.position;

                            Vector3 oldDest = destination;
                            oldDest.y += 1;
                            if (Vector3.Distance(destination, senderChar.transform.position) > 375)
                                destination.y += 8;

                            serverManagement.TeleportPlayerToWorld(senderClient.netPlayer, destination);
                            TimerPlus tp = new TimerPlus();
                            tp.AutoReset = false;
                            tp.Interval = 500;
                            tp.Elapsed += ((sender, e) => teleportToWorldElapsed(senderClient.netPlayer, oldDest));
                            tp.Start();
                            Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting to " + targetClient.userName + "...");
                        }
                    }
                }
                else
                {
                    playerNameList.Clear();
                    curIndex = 0;

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
                    playerName = string.Join(" ", playerNameList.ToArray());
                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
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
                            curIndex = 0;
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
                        PlayerClient[] otherTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                        if (otherTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                        else if (otherTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                        else
                        {
                            PlayerClient targetClient2 = otherTargets[0];
                            Character targetChar;
                            Character.FindByUser(targetClient.userID, out targetChar);
                            Character targetChar2;
                            Character.FindByUser(targetClient2.userID, out targetChar2);
                            Vector3 destination = targetChar2.transform.position;

                            Vector3 oldDest = destination;
                            oldDest.y += 1;
                            if (Vector3.Distance(destination, targetChar.transform.position) > 375)
                                destination.y += 8;

                            serverManagement.TeleportPlayerToWorld(targetClient.netPlayer, destination);
                            TimerPlus tp = new TimerPlus();
                            tp.AutoReset = false;
                            tp.Interval = 500;
                            tp.Elapsed += ((sender, e) => teleportToWorldElapsed(targetClient.netPlayer, oldDest));
                            tp.Start();
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

                    if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                    {
                        playerName = playerName.Substring(1, playerName.Length - 2);

                        PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(playerName));
                        if (possibleTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal \"" + playerName + "\".");
                        else if (possibleTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + playerName + "\".");
                        else
                        {
                            PlayerClient targetClient = possibleTargets[0];
                            if (!mutedUsers.Contains(targetClient.userID.ToString()))
                            {
                                Broadcast.broadcastAll("Player \"" + targetClient.userName + "\" has been muted on global chat for " + timeString + timeMode);
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
                        PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                        if (possibleTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                        else if (possibleTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                        else
                        {
                            PlayerClient targetClient = possibleTargets[0];
                            if (!mutedUsers.Contains(targetClient.userID.ToString()))
                            {
                                Broadcast.broadcastAll("Player \"" + targetClient.userName + "\" has been muted on global chat for " + timeString + timeMode);
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

                    if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                    {
                        playerName = playerName.Substring(1, playerName.Length - 2);

                        PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(playerName));
                        if (possibleTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal \"" + playerName + "\".");
                        else if (possibleTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + playerName + "\".");
                        else
                        {
                            PlayerClient targetClient = possibleTargets[0];
                            if (mode)
                            {
                                if (!mutedUsers.Contains(targetClient.userID.ToString()))
                                {
                                    Broadcast.broadcastAll("Player \"" + targetClient.userName + "\" has been muted on global chat.");
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
                                    Broadcast.broadcastAll("Player \"" + targetClient.userName + "\" has been unmuted on global chat.");
                                    mutedUsers.Remove(targetClient.userID.ToString());
                                }
                                else
                                {
                                    Broadcast.broadcastTo(senderClient.netPlayer, targetClient.userName + " is not muted.");
                                }
                            }
                        }
                    }
                    else
                    {
                        PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
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
                                    Broadcast.broadcastAll("Player \"" + targetClient.userName + "\" has been muted on global chat.");
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
                                    Broadcast.broadcastAll("Player \"" + targetClient.userName + "\" has been unmuted on global chat.");
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

                PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID.ToString() == UID);
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
                                PlayerClient[] targetUsers = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => !groupMembers.Contains(pc.userID.ToString()));
                                foreach (PlayerClient targetClient in targetUsers)
                                {
                                    whitelistKick(targetClient.netUser, whitelistKickCMD);
                                }
                            }
                            else
                            {
                                PlayerClient[] targetUsers = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => !whitelist.Contains(pc.userID.ToString()));
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
                KeyValuePair<string, Dictionary<string, string>>[] possibleFactions = Array.FindAll(factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(UID));

                switch (action)
                {
                    case "g":
                        if (!inGlobal.Contains(UID))
                        {
                            inGlobal.Add(UID);
                            if (inDirect.Contains(UID))
                                inDirect.Remove(UID);
                            if (inFaction.Contains(UID))
                                inFaction.Remove(UID);
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
                            if (inFaction.Contains(UID))
                                inFaction.Remove(UID);
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
                            if (inFaction.Contains(UID))
                                inFaction.Remove(UID);
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
                            if (inFaction.Contains(UID))
                                inFaction.Remove(UID);
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are now talking in direct chat.");
                        }
                        else
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are already talking in direct chat.");
                        }
                        break;
                    case "f":
                        if (possibleFactions.Count() > 0)
                        {
                            if (!inFaction.Contains(UID))
                            {
                                inFaction.Add(UID);
                                if (inGlobal.Contains(UID))
                                    inGlobal.Remove(UID);
                                if (inDirect.Contains(UID))
                                    inDirect.Remove(UID);
                                Broadcast.broadcastTo(senderClient.netPlayer, "You are now talking in the faction chat of [" + possibleFactions[0].Key + "].");
                            }
                            else
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, "You are already talking in the faction chat of [" + possibleFactions[0].Key + "].");
                            }
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        break;
                    case "faction":
                        if (possibleFactions.Count() > 0)
                        {
                            if (!inFaction.Contains(UID))
                            {
                                inFaction.Add(UID);
                                if (inGlobal.Contains(UID))
                                    inGlobal.Remove(UID);
                                if (inDirect.Contains(UID))
                                    inDirect.Remove(UID);
                                Broadcast.broadcastTo(senderClient.netPlayer, "You are now talking in the faction chat of " + possibleFactions[0].Key + ".");
                            }
                            else
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, "You are already talking in the faction chat of " + possibleFactions[0].Key + ".");
                            }
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
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
            if (!emptyPrefixes.Contains(uid))
            {
                foreach (KeyValuePair<string, List<string>> kv in rankList)
                {
                    if (kv.Value.Contains(uid))
                    {
                        playerName = (!removePrefix ? "[" + kv.Key + "]" + " " : "") + playerName;
                        break;
                    }
                }
            }

            return playerName;
        }

        public static void fakeJoinServer(string[] args)
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

                PlayerClient[] possibleClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(fakeName));
                string rank = "";

                if (possibleClients.Count() == 1 && !removePrefix)
                    rank = "[" + findRank(possibleClients[0].userID.ToString()) + "] ";

                string joinMessage = Vars.joinMessage.Replace("$USER$", rank + fakeName);
                Broadcast.broadcastAll(joinMessage);
            }
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

                PlayerClient[] possibleClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(fakeName));
                string rank = "";

                if (possibleClients.Count() == 1 && !removePrefix)
                    rank = "[" + findRank(possibleClients[0].userID.ToString()) + "] ";

                string joinMessage = Vars.joinMessage.Replace("$USER$", rank + fakeName);
                Broadcast.broadcastAll(joinMessage);
            }
            else
            {
                string rank = "[" + findRank(senderClient.userID.ToString()) + "] ";

                string joinMessage = Vars.joinMessage.Replace("$USER$", (!removePrefix ? rank : "") + senderClient.userName);
                Broadcast.broadcastAll(joinMessage);
            }
        }

        public static void sendToFaction(PlayerClient senderClient, string message)
        {
            Character senderChar;
            Character.FindByUser(senderClient.userID, out senderChar);

            string[] factionMembers = Array.Find(factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(senderClient.userID.ToString())).Value.Keys.ToArray();
            PlayerClient[] factionClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => factionMembers.Contains(pc.userID.ToString()));
            foreach (PlayerClient targetClient in factionClients)
            {
                Broadcast.broadcastCustomTo(targetClient.netPlayer, "<F> " + senderClient.userName, message);
            }
        }

        public static void sendToSurrounding(PlayerClient senderClient, string message)
        {
            Character senderChar;
            Character.FindByUser(senderClient.userID, out senderChar);

            Vector3 senderPos = senderChar.transform.position;
            foreach (PlayerClient targetClient in AllPlayerClients)
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

        public static string grabNameByUID(string UID)
        {
            PlayerClient[] possibleClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID.ToString() == UID);

            if (possibleClients.Count() > 0) // If the player is online, grab his name through the game
            {
                return possibleClients[0].userName;
            }
            else // If he is not, grab his name through steam
            {
                try
                {
                    string profileURL = "http://steamcommunity.com/profiles/" + UID + "/?xml=1\\";
                    WebRequest request = WebRequest.Create(profileURL);
                    request.Timeout = 3000;

                    using (WebResponse response = request.GetResponse())
                    {
                        using (XmlTextReader reader = new XmlTextReader(response.GetResponseStream()))
                        {
                            string currentElement = "";
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Element && reader.Name == "steamID")
                                {
                                    currentElement = "steamID";
                                }
                                if (reader.NodeType == XmlNodeType.CDATA && currentElement == "steamID")
                                {
                                    return reader.Value;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Vars.conLog.Error(ex.ToString());
                }
                return "an unknown player";
            }
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
                Vars.conLog.Error(ex.ToString());
            }
        }

        public static StringBuilder cfgText()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("[Whitelist]");
            sb.AppendLine("# Enable whitelist upon server startup");
            sb.AppendLine("enableWhitelist=false");
            //sb.AppendLine("# Use the MySQL settings defined below for whitelisting - BROKEN");
            //sb.AppendLine("useMySQL=false");
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
            sb.AppendLine("# Set the day length in minutes upon server start (Default 45 minutes)");
            sb.AppendLine("dayLength=45");
            sb.AppendLine("# Set the night length in minutes upon server start (Default 15 minutes)");
            sb.AppendLine("nightLength=15");
            sb.AppendLine("# Freeze time upon server start");
            sb.AppendLine("freezeTime=false");
            sb.AppendLine("# Set fall damage server-wide");
            sb.AppendLine("fallDamage=true");
            sb.AppendLine("# Set distance at which you can hear players in direct voice chat (Default 100)");
            sb.AppendLine("voiceDistance=100");
            sb.AppendLine("# Enables repairing of structures");
            sb.AppendLine("enableRepair=true");
            sb.AppendLine("# Forces all players to be naked");
            sb.AppendLine("forceNudity=false");
            sb.AppendLine("# Sets the creations of door stops after the door has been destroyed");
            sb.AppendLine("doorStops=true");
            sb.AppendLine("");
            sb.AppendLine("[Chat]");
            sb.AppendLine("# Enables or disables direct chat. ATLEAST ONE MUST BE ENABLED!");
            sb.AppendLine("directChat=false");
            sb.AppendLine("# Enables or disables global chat. ATLEAST ONE MUST BE ENABLED!");
            sb.AppendLine("globalChat=true");
            sb.AppendLine("# Toggles the display of the <g> tag if global is the only channel enabled");
            sb.AppendLine("removeTag=true");
            sb.AppendLine("# Sets the default chat players will talk in upon join. (global or direct)");
            sb.AppendLine("defaultChat=direct");
            sb.AppendLine("# Sets the distance the radius of possible text communication when in direct chat");
            sb.AppendLine("directDistance=150");
            sb.AppendLine("# Sets the characters a player can have in his name.");
            sb.AppendLine("allowedChars=a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,1,2,3,4,5,6,7,8,9,0,`,-,=,',.,[,],(,),{,},~,_");
            sb.AppendLine("# If false, players can have anything they want in their names with no restrictions.");
            sb.AppendLine("restrictChars=true");
            sb.AppendLine("# Minimum amount of characters a player must have in his name.");
            sb.AppendLine("minimumNameCount=3");
            sb.AppendLine("# Maximum amount of characters a player can have in his name.");
            sb.AppendLine("maximumNameCount=15");
            sb.AppendLine("# If a player that is joining has a name that is currently in use on the server and kickDuplicate is true, kick him. If false, append \"(#1,2,3,etc..)\" to his name.");
            sb.AppendLine("kickDuplicate=false");
            sb.AppendLine("# If kickDuplicate is true, lowerAuthority is true, and a joining user has the same name as a connected user, the user of lower authority will be kicked.");
            sb.AppendLine("lowerAuthority=false");
            sb.AppendLine("# Illegal words for use with censorship.");
            sb.AppendLine("illegalWords=fuck,shit,cunt,bitch,pussy,slut,whore,ass");
            sb.AppendLine("# If a player joins with a word in the illegalWords list and censorship is true, he will be kicked. Words in chat found in illegalWords will be replaced with *'s.");
            sb.AppendLine("censorship=false");
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
            sb.AppendLine("# Murder message that is displayed to all when a user is murdered by another user with an unknown cause (Tags: $VICTIM$, $KILLER$)");
            sb.AppendLine("murderMessageUnknown=$KILLER$ killed $VICTIM$.");
            sb.AppendLine("# Enables or disables the display of murder messages");
            sb.AppendLine("enableMurder=true");
            sb.AppendLine("# Murder message that is displayed to all when a user is killed by a mob (Tags: $VICTIM$, $KILLER$)");
            sb.AppendLine("deathMessage=$VICTIM$ was mauled by a $KILLER$.");
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
            sb.AppendLine("# Sets the delay for between the acceptance of a teleport request and the actual teleport. Default 10 seconds.");
            sb.AppendLine("requestDelay=10");
            sb.AppendLine("# Sets the delay for between the /warp and the actual warp. Default 10 seconds.");
            sb.AppendLine("warpDelay=10");
            sb.AppendLine("# Type of cooldown that will be used after a person uses a teleport request. 0 = No cooldown.");
            sb.AppendLine("# 1 = If you send a request to a player and it is accepted, you cannot tpa to him until the cooldown finishes.");
            sb.AppendLine("# 2 = If you send a request to any player and it is accepted, you cannot tpa at all until the cooldown finishes.");
            sb.AppendLine("requestCooldownType=0");
            sb.AppendLine("# Sets the cooldown (in (m)inutes or (s)econds) for teleport requests (if cooldown type is not 0). Default 15m.");
            sb.AppendLine("requestCooldown=15m");
            sb.AppendLine("# If true, players will not be able to warp or use tpa.");
            sb.AppendLine("denyRequestWarzone=true");
            //sb.AppendLine("");
            //sb.AppendLine("[MySQL]");
            //sb.AppendLine("# IP for the MySQL whitelist database.");
            //sb.AppendLine("host=localhost");
            //sb.AppendLine("# Port for the MySQL whitelist database.");
            //sb.AppendLine("port=3306");
            //sb.AppendLine("# Database name for the MySQL whitelist database.");
            //sb.AppendLine("database=RustEssentials");
            //sb.AppendLine("# Username for the MySQL whitelist database.");
            //sb.AppendLine("user=root");
            //sb.AppendLine("# Password for the MySQL whitelist database.");
            //sb.AppendLine("pass=");
            sb.AppendLine("");
            sb.AppendLine("[Inheritance]");
            sb.AppendLine("# If true, users will inherit their assigned commands plus the ones useable by those of lower ranks.");
            sb.AppendLine("inheritCommands=true");
            sb.AppendLine("# If true, users will inherit their assigned kits plus the ones useable by those of lower ranks.");
            sb.AppendLine("inheritKits=true");
            sb.AppendLine("# If true, users will inherit their assigned warps plus the ones useable by those of lower ranks.");
            sb.AppendLine("inheritWarps=true");
            sb.AppendLine("");
            sb.AppendLine("[Damage]");
            sb.AppendLine("# If false, friendly fire between users of the same faction will be negated and disabled when not in a war zone.");
            sb.AppendLine("friendlyFire=false");
            sb.AppendLine("# If false, allied fire between users of allied factions will be reduced.");
            sb.AppendLine("alliedFire=false");
            sb.AppendLine("# Damage multiplier for when a user is attacked by a non-allied user outside his/her own faction. Default 1.");
            sb.AppendLine("neutralDamage=1");
            sb.AppendLine("# Damage multiplier for when a user is attacked by a non-allied user outside his/her own faction while in a war zone. Default 1");
            sb.AppendLine("warDamage=1");
            sb.AppendLine("# Damage multiplier for when a user is attacked by a user inside the same faction while in a war zone. Default 0.");
            sb.AppendLine("warFriendlyDamage=0");
            sb.AppendLine("# Damage multiplier for when a user is attacked by an allied user while in a war zone. Default 0.70.");
            sb.AppendLine("warAllyDamage=0.70");
            sb.AppendLine("");
            sb.AppendLine("[Item Controller]");
            //sb.AppendLine("# If false, researching with research kits will not require a workbench");
            //sb.AppendLine("researchAtBench=true");
            sb.AppendLine("# If true, research kits will not disappear upon last use");
            sb.AppendLine("infiniteResearch=false");
            sb.AppendLine("# If true, researching will require paper");
            sb.AppendLine("researchPaper=false");
            //sb.AppendLine("# If false, items that usually require a nearby workbench to craft will no longer need one");
            //sb.AppendLine("craftAtBench=true");

            return sb;
        }

        public static StringBuilder itemControllerText()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("# This is the item controller file. Restrictions for crafting, researching, and blueprints should be placed here.");
            sb.AppendLine("# Please note that the # symbol resembles a comment and should not be used when configuring.");
            sb.AppendLine("# ");
            sb.AppendLine("# Item names should be placed directly under the category you want it restricted under.");
            sb.AppendLine("# In order to restrict crafting for an item, simply place it below the [Crafting Restrictions] section.");
            sb.AppendLine("# Be aware that item names are case-sensitive and must be spelled correctly in order to properly apply the restriction.");
            sb.AppendLine("# Example of restricting an item:");
            sb.AppendLine("#   [Item Restrictions]");
            sb.AppendLine("#   Explosive Charge");
            sb.AppendLine("#   ");
            sb.AppendLine("#   [Crafting Restrictions]");
            sb.AppendLine("#   Bolt Action Rifle");
            sb.AppendLine("#   ");
            sb.AppendLine("#   [Research Restrictions]");
            sb.AppendLine("#   M4");
            sb.AppendLine("#   ");
            sb.AppendLine("#   [Blueprint Restrictions]");
            sb.AppendLine("#   Kevlar Vest");
            sb.AppendLine("");
            sb.AppendLine("[Item Restrictions]");
            sb.AppendLine("");
            sb.AppendLine("[Crafting Restrictions]");
            sb.AppendLine("");
            sb.AppendLine("[Research Restrictions]");
            sb.AppendLine("");
            sb.AppendLine("[Blueprint Restrictions]");

            return sb;
        }

        public static StringBuilder warpsText()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("# This is the warps file. Warp locations should be named and assigned here.");
            sb.AppendLine("# Please note that the # symbol resembles a comment and should not be used when configuring.");
            sb.AppendLine("# ");
            sb.AppendLine("# Warps are 3 point vectors (x, y, and z) with an assigned name.");
            sb.AppendLine("# By adding them here, you will be able to type /warp *name* to teleport to that vector.");
            sb.AppendLine("# Warps are permission bound and can be attached to either rank prefixes or UIDs.");
            sb.AppendLine("# Warps that are attached to a rank will be inherited by ranks of higher authority unless inheritWarps in the config is false.");
            sb.AppendLine("# Example of a warp bound to Owners:");
            sb.AppendLine("#   [Village.O]");
            sb.AppendLine("#   (4986.2, 410.6, 5001.6)");
            sb.AppendLine("# ");
            sb.AppendLine("# Example of a warp bound to default users:");
            sb.AppendLine("#   [Spawn]");
            sb.AppendLine("#   (4986.2, 410.6, 5001.6)");
            sb.AppendLine("# ");
            sb.AppendLine("# Example of a warp bound to a user:");
            sb.AppendLine("#   [Skybase.76569811000000000]");
            sb.AppendLine("#   (4986.2, 410.6, 5001.6)");
            sb.AppendLine("");

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
            sb.AppendLine("# ");
            sb.AppendLine("# If you want to remove a user's assigned rank prefix, simply add his UID without specifying any kind of prefix. DO NOT ADD A ':'.");
            sb.AppendLine("# Example:");
            sb.AppendLine("#   <USER ID>");
            sb.AppendLine("#   76569811000000000");
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
            sb.AppendLine("# However, if you wanted to assign a kit for a specific user regardless of rank, just append \".<UID>\" after the kit name.");
            sb.AppendLine("# Example:");
            sb.AppendLine("#   [Awesomeness.76560000000000000] <--- This kit is usable by the person with that UID.");
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
            sb.AppendLine("# [Once.#(s/m/h)] is the MOTD that is broadcasted to all users once after the # elapses.");
            sb.AppendLine("# [Rules] is the MOTD that is broadcasted directly to the user that types /rules");
            sb.AppendLine("#");
            sb.AppendLine("# The #(s/m/h) in [Cycle.#(s/m/h)] resembles the interval in (s)econds, (m)inutes, or (h)ours.");
            sb.AppendLine("# Example:");
            sb.AppendLine("#   [Cycle.2h]");
            sb.AppendLine("#   This is the MOTD.");
            sb.AppendLine("# This MOTD will be broadcasted every 2 hours to all users.");
            sb.AppendLine("#");
            sb.AppendLine("# Remember that you can add and remove as many lines as you want for the two MOTDs.");
            sb.AppendLine("# All MOTD's can run commands with {/command name}.");
            sb.AppendLine("# Example:");
            sb.AppendLine("#   [Once.10m]");
            sb.AppendLine("#   {/save}");
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
            sb.AppendLine("[Cycle.20m]");
            sb.AppendLine("CycleMessage1 # DELETE THESE LINES TO REMOVE CYCLE MOTD");
            sb.AppendLine("CycleMessage2 # DELETE THESE LINES TO REMOVE CYCLE MOTD");
            sb.AppendLine("CycleMessage3 # DELETE THESE LINES TO REMOVE CYCLE MOTD");
            sb.AppendLine("");
            sb.AppendLine("[Once.1h]");
            sb.AppendLine("OnceMessage1 # DELETE THESE LINES TO REMOVE ONCE MOTD");
            sb.AppendLine("OnceMessage2 # DELETE THESE LINES TO REMOVE ONCE MOTD");
            sb.AppendLine("OnceMessage3 # DELETE THESE LINES TO REMOVE ONCE MOTD");
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
            sb.AppendLine("/giveall");
            sb.AppendLine("/reload");
            sb.AppendLine("/daylength");
            sb.AppendLine("/nightlength");
            sb.AppendLine("/kill");
            sb.AppendLine("/stop");
            sb.AppendLine("/access");
            sb.AppendLine("/remove");
            sb.AppendLine("/removeall");
            sb.AppendLine("/craft");
            sb.AppendLine("");
            sb.AppendLine("[Administrator]");
            sb.AppendLine("/airdrop");
            sb.AppendLine("/random");
            sb.AppendLine("/ban");
            sb.AppendLine("/bane");
            sb.AppendLine("/unban");
            sb.AppendLine("/kickall");
            sb.AppendLine("/pos");
            sb.AppendLine("/say");
            sb.AppendLine("/saypop");
            sb.AppendLine("/time");
            sb.AppendLine("/tp");
            sb.AppendLine("/tphere");
            sb.AppendLine("/god");
            sb.AppendLine("/ungod");
            sb.AppendLine("/whitelist");
            sb.AppendLine("/tppos");
            sb.AppendLine("/heal");
            sb.AppendLine("/fall");
            sb.AppendLine("/feed");
            sb.AppendLine("/f safezone");
            sb.AppendLine("/f warzone");
            sb.AppendLine("/f build");
            sb.AppendLine("/vanish");
            sb.AppendLine("/clearinv");
            sb.AppendLine("");
            sb.AppendLine("[Moderator]");
            sb.AppendLine("/kick");
            sb.AppendLine("/kicke");
            sb.AppendLine("/join");
            sb.AppendLine("/leave");
            sb.AppendLine("/mute");
            sb.AppendLine("/unmute");
            sb.AppendLine("/save");
            sb.AppendLine("/hide");
            sb.AppendLine("/owner");
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
            sb.AppendLine("/r");
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
            sb.AppendLine("/warps");
            sb.AppendLine("/warp");

            return sb;
        }

        public static void  loopNudity()
        {
            TimerPlus t = new TimerPlus();
            t.AutoReset = true;
            t.Interval = 1000;
            t.Elapsed += ((sender, e) => sendNudity());
            t.Start();
        }

        public static void loopItems()
        {
            TimerPlus t = new TimerPlus();
            t.AutoReset = true;
            t.Interval = 5000;
            t.Elapsed += ((sender, e) => checkItems());
            t.Start();
        }

        public static void sendNudity()
        {
            try
            {
                List<PlayerClient> playerClients = AllPlayerClients;
                foreach (PlayerClient playerClient in playerClients)
                {
                    if (forceNudity && playerClient != null && playerClient.netPlayer != null)
                        ConsoleNetworker.SendClientCommand(playerClient.netPlayer, "censor.nudity false");
                }
            }
            catch (Exception ex)
            {
                conLog.Error("SN: " + ex.ToString());
            }
        }

        public static void checkItems()
        {
            try
            {
                List<PlayerClient> playerClients = AllPlayerClients;
                foreach (PlayerClient playerClient in playerClients)
                {
                    if (playerClient != null && playerClient.netPlayer != null)
                    {
                        if (!craftList.Contains(playerClient.userID.ToString()))
                        {
                            foreach (string itemName in restrictItems)
                            {
                                if (hasItem(playerClient, itemName))
                                {
                                    Broadcast.broadcastTo(playerClient.netPlayer, "Illegal item \"" + itemName + "\" found. Item removed.");
                                    List<IInventoryItem> items;
                                    grabItem(playerClient, itemName, out items);
                                    foreach (IInventoryItem item in items)
                                    {
                                        removeItem(playerClient, item);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                conLog.Error("CI: " + ex.ToString());
            }
        }

        public static void loopRequestSaving()
        {
            TimerPlus t = new TimerPlus();
            t.AutoReset = true;
            t.Interval = 5000;
            t.Elapsed += ((sender, e) => saveRequestsPer());
            t.Start();
            TimerPlus t1 = new TimerPlus();
            t1.AutoReset = true;
            t1.Interval = 5000;
            t1.Elapsed += ((sender, e) => saveRequestsAll());
            t1.Start();
        }

        public static void saveRequestsPer()
        {
            Dictionary<string, List<string>> blockedPeoplePer = new Dictionary<string, List<string>>();

            foreach (KeyValuePair<string, Dictionary<string, TimerPlus>> kv in blockedRequestsPer)
            {
                string UID = kv.Key;
                if (!blockedPeoplePer.ContainsKey(UID))
                    blockedPeoplePer.Add(UID, new List<string>());
                foreach (KeyValuePair<string, TimerPlus> kv2 in kv.Value)
                {
                    string otherUID = kv2.Key;
                    blockedPeoplePer[UID].Add(otherUID);
                    updateRequestData(UID, otherUID, requestCooldown, kv2.Value);
                }
            }
            remOldRequests(blockedPeoplePer);
        }

        public static void saveRequestsAll()
        {
            List<string> blockedPeopleAll = new List<string>();

            foreach (KeyValuePair<string, TimerPlus> kv in blockedRequestsAll)
            {
                string UID = kv.Key;
                if (!blockedPeopleAll.Contains(UID))
                    blockedPeopleAll.Add(UID);

                updateRequestAllData(UID, requestCooldown, kv.Value);
            }
            remOldRequestsAll(blockedPeopleAll);
        }

        public static void readRequestData()
        {
            List<string> cooldownFileData = File.ReadAllLines(requestCooldownsFile).ToList();
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
                    t.Interval = Convert.ToInt32(cooldown);
                    t.Elapsed += (sender, e) => unblockRequests(otherUID, UID);
                    t.Start();
                    if (!blockedRequestsPer.ContainsKey(UID))
                        blockedRequestsPer.Add(UID, new Dictionary<string, TimerPlus>());

                    blockedRequestsPer[UID].Add(otherUID, t);
                }
            }
        }

        public static void readRequestAllData()
        {
            List<string> cooldownFileData = File.ReadAllLines(requestCooldownsAllFile).ToList();
            foreach (string s in cooldownFileData)
            {
                string UID = s.Split('=')[0];
                string cooldown = s.Split('=')[1];

                TimerPlus t = new TimerPlus();
                t.AutoReset = false;
                t.Interval = Convert.ToInt32(cooldown);
                t.Elapsed += (sender, e) => unblockRequests("", UID);
                t.Start();
                if (!blockedRequestsAll.ContainsKey(UID))
                    blockedRequestsAll.Add(UID, t);
            }
        }

        public static void remOldRequests(Dictionary<string, List<string>> oldRequests)
        {
            try
            {
                List<string> cooldownFileData = File.ReadAllLines(requestCooldownsFile).ToList();
                List<int> removeQueue1 = new List<int>();
                Dictionary<string, int> removeQueue2 = new Dictionary<string, int>();
                foreach (string str in cooldownFileData)
                {
                    string UID = str.Split('=')[0];
                    if (!oldRequests.ContainsKey(UID)) // If all my cooldowns are completed but the file still has me cooling down
                    {
                        int indexOfUID = Array.FindIndex(cooldownFileData.ToArray(), (string s) => s.StartsWith(UID));
                        removeQueue1.Add(indexOfUID);
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

                                int indexOfUID = Array.FindIndex(cooldownFileData.ToArray(), (string st) => st.StartsWith(UID));
                                removeQueue2.Add(fullString, indexOfUID);
                            }
                        }
                    }
                }
                try
                {
                    foreach (int i in removeQueue1)
                    {
                        cooldownFileData.RemoveAt(i);
                    }
                }
                catch { }
                foreach (KeyValuePair<string, int> kv in removeQueue2)
                {
                    cooldownFileData[kv.Value] = kv.Key;
                }
                using (StreamWriter sw = new StreamWriter(requestCooldownsFile, false))
                {
                    foreach (string s in cooldownFileData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                conLog.Error(ex.ToString());
            }
        }

        public static void remOldRequestsAll(List<string> oldRequests)
        {
            try
            {
                List<string> cooldownFileData = File.ReadAllLines(requestCooldownsAllFile).ToList();
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
                catch { }
                using (StreamWriter sw = new StreamWriter(requestCooldownsAllFile, false))
                {
                    foreach (string s in cooldownFileData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                conLog.Error(ex.ToString());
            }
        }

        public static void updateRequestData(string UID, string otherUID, int cooldown, TimerPlus t)
        {
            List<string> cooldownFileData = File.ReadAllLines(requestCooldownsFile).ToList();
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
            using (StreamWriter sw = new StreamWriter(requestCooldownsFile, false))
            {
                foreach (string s in cooldownFileData)
                {
                    sw.WriteLine(s);
                }
            }
        }

        public static void updateRequestAllData(string UID, int cooldown, TimerPlus t)
        {
            List<string> cooldownFileData = File.ReadAllLines(requestCooldownsAllFile).ToList();
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
            using (StreamWriter sw = new StreamWriter(requestCooldownsAllFile, false))
            {
                foreach (string s in cooldownFileData)
                {
                    sw.WriteLine(s);
                }
            }
        }

        public static void loopKitSaving()
        {
            TimerPlus t = new TimerPlus();
            t.AutoReset = true;
            t.Interval = 5000;
            t.Elapsed += ((sender, e) => saveCooldowns());
            t.Start();
        }

        public static void saveCooldowns()
        {
            try
            {
                Dictionary<string, List<string>> kits = new Dictionary<string, List<string>>();
                foreach (KeyValuePair<string, Dictionary<TimerPlus, string>> kv in playerCooldowns)
                {
                    string UID = kv.Key;
                    if (!kits.ContainsKey(UID))
                        kits.Add(UID, new List<string>());
                    foreach (KeyValuePair<TimerPlus, string> kv2 in kv.Value)
                    {
                        string kitName = kv2.Value;
                        kits[UID].Add(kitName);
                        string cooldown = kitCooldowns[kitName].ToString();
                        updateCooldownData(UID, kitName, cooldown, kv2.Key);
                    }
                }
                remOldCooldowns(kits);
            }
            catch (Exception ex)
            {
                conLog.Error("SCD: " + ex.ToString());
            }
        }

        public static void readCooldownData()
        {
            try
            {
                List<string> cooldownFileData = File.ReadAllLines(cooldownsFile).ToList();
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
                            t.Interval = Convert.ToInt32(cooldown);
                            t.Elapsed += (sender, e) => restoreKit(sender, e, kitName, UID);
                            t.Start();

                            if (!playerCooldowns.ContainsKey(UID))
                            {
                                playerCooldowns.Add(UID, new Dictionary<TimerPlus, string>() { { t, kitName } });
                            }
                            else
                            {
                                if (!playerCooldowns[UID].ContainsValue(kitName))
                                    playerCooldowns[UID].Add(t, kitName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                conLog.Error("RCDD: " + ex.ToString());
            }
        }

        public static void remOldCooldowns(Dictionary<string, List<string>> oldKits)
        {
            try
            {
                List<string> cooldownFileData = File.ReadAllLines(cooldownsFile).ToList();
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
                catch { }
                try
                {
                    foreach (KeyValuePair<string, string> kv in removeQueue2)
                    {
                        int indexOfUID = Array.FindIndex(cooldownFileData.ToArray(), (string st) => st.StartsWith(kv.Key));
                        cooldownFileData[indexOfUID] = kv.Value;
                    }
                }
                catch { }
                using (StreamWriter sw = new StreamWriter(cooldownsFile, false))
                {
                    foreach (string s in cooldownFileData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                conLog.Error("ROCD: " + ex.ToString());
            }
        }

        public static void updateCooldownData(string UID, string kitName, string cooldown, TimerPlus t)
        {
            try
            {
                List<string> cooldownFileData = File.ReadAllLines(cooldownsFile).ToList();
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

                        if (t.TimeLeft > 0)
                            allKits[index] = kitName + ":" + t.TimeLeft;
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
                using (StreamWriter sw = new StreamWriter(cooldownsFile, false))
                {
                    foreach (string s in cooldownFileData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                conLog.Error("UCDD: " + ex.ToString());
            }
        }

        public static void readAlliesData()
        {
            try
            {
                List<string> alliesFileData = File.ReadAllLines(alliesFile).ToList();
                foreach (string s in alliesFileData)
                {
                    if (s.Contains("="))
                    {
                        string factionName = s.Split('=')[0];
                        string alliesString = s.Split('=')[1];

                        if (!alliances.ContainsKey(factionName))
                            alliances.Add(factionName, new List<string>());
                        else
                            conLog.Error("Faction [" + factionName + "] and their alliances are already loaded!");

                        foreach (string s2 in alliesString.Split(';'))
                        {
                            string alliedFactionName = s2;

                            if (!alliances[factionName].Contains(alliedFactionName))
                                alliances[factionName].Add(alliedFactionName);
                            else
                                conLog.Error("Faction [" + factionName + "] already allied with faction [" + alliedFactionName + "]!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                conLog.Error("RAD #1: " + ex.ToString());
            }
        }

        public static void addAlliesData(string factionName, string alliedFactionName)
        {
            try
            {
                List<string> alliesFileData = File.ReadAllLines(alliesFile).ToList();
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
                using (StreamWriter sw = new StreamWriter(alliesFile, false))
                {
                    foreach (string s in alliesFileData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                conLog.Error("AAD: " + ex.ToString());
            }
        }

        public static void remAlliesData(string factionName, string alliedFactionName)
        {
            try
            {
                List<string> alliesFileData = File.ReadAllLines(alliesFile).ToList();
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
                using (StreamWriter sw = new StreamWriter(alliesFile, false))
                {
                    foreach (string s in alliesFileData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                conLog.Error("RAD #2: " + ex.ToString());
            }
        }

        public static void readFactionData()
        {
            try
            {
                List<string> factionFileData = File.ReadAllLines(factionsFile).ToList();
                foreach (string s in factionFileData)
                {
                    if (s.Contains("="))
                    {
                        string factionName = s.Split('=')[0];
                        string membersString = s.Split('=')[1];

                        if (!factions.ContainsKey(factionName))
                            factions.Add(factionName, new Dictionary<string, string>());
                        else
                            conLog.Error("#1: Faction [" + factionName + "] already loaded!");
                        if (!factionsByNames.ContainsKey(factionName))
                            factionsByNames.Add(factionName, new Dictionary<string, string>());
                        else
                            conLog.Error("#2: Faction [" + factionName + "] already loaded!");

                        foreach (string s2 in membersString.Split(';'))
                        {
                            string nameAndUID = s2.Split(':')[0];
                            string rank = s2.Split(':')[1];
                            string name = nameAndUID.Substring(1, nameAndUID.IndexOf(')') - 1);
                            string UID = nameAndUID.Substring(nameAndUID.LastIndexOf(')') + 1);

                            if (!factions[factionName].ContainsKey(UID))
                                factions[factionName].Add(UID, rank);
                            else
                                conLog.Error("A: Faction [" + factionName + "] already includes UID \"" + UID + "\"!");
                            if (!factionsByNames[factionName].ContainsKey(UID))
                                factionsByNames[factionName].Add(UID, name);
                            else
                                conLog.Error("B: Faction [" + factionName + "] already includes UID \"" + UID + "\"!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                conLog.Error("RFD #1: " + ex.ToString());
            }
        }

        public static void addFactionData(string factionName, string userName, string userID, string rank)
        {
            try
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
            catch (Exception ex)
            {
                conLog.Error("AFD: " + ex.ToString());
            }
        }

        public static void remFactionData(string factionName, string userName, string rank)
        {
            try
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
                using (StreamWriter sw = new StreamWriter(factionsFile, false))
                {
                    foreach (string s in factionFileData)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                conLog.Error("RFD #2: " + ex.ToString());
            }
        }

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
                conLog.Error("RDD #1: " + ex.ToString());
            }
        }

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
                conLog.Error("ADD: " + ex.ToString());
            }
        }

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
                conLog.Error("RDD #2: " + ex.ToString());
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
            sb.AppendLine("# Quotes (\"\") symbolize input that DOES require quotes.");
            sb.AppendLine("# Example (/uid \"name\" should be typed as):");
            sb.AppendLine("# /uid \"Brian\"");
            sb.AppendLine("#");
            sb.AppendLine("# Player names and item names are case sensitive. However, player names can be accepted as partials.");
            sb.AppendLine("");
            sb.AppendLine("/access {on} (Gives the sender access to all doors)");
            sb.AppendLine("/access {off} (Revokes access to all doors from the sender)");
            sb.AppendLine("/airdrop (Spawns an airdrop with a random drop location)");
            sb.AppendLine("/airdrop <player name> (Spawns an airdrop with a drop location at the specified player)");
            sb.AppendLine("/ban <player name> (Bans player with reason: \"Banned by a(n) <Your Rank>\")");
            sb.AppendLine("/ban <player name> *reason* (Bans player with the specified reason)");
            sb.AppendLine("/ban [player UID] (Bans player by UID with reason: \"Banned by a(n) <Your Rank>\")");
            sb.AppendLine("/ban [player UID] *reason* (Bans player by UID with the specified reason)");
            sb.AppendLine("/bane \"player name\" (Bans player by their exact name with reason: \"Banned by a(n) <Your Rank>\")");
            sb.AppendLine("/bane \"player name\" *reason* (Bans player by their exact name with the specified reason)");
            sb.AppendLine("/chan {g} (Joins the global chat)");
            sb.AppendLine("/chan {global} (Joins the global chat)");
            sb.AppendLine("/chan {d} (Joins the direct chat)");
            sb.AppendLine("/chan {direct} (Joins the direct chat)");
            sb.AppendLine("/clearinv *name* (Clears the inventory of the specified player)");
            sb.AppendLine("/clearinv \"name\" (Clears the inventory of the specified player by their exact name)");
            sb.AppendLine("/craft {on} (Turns on super craft mode. Crafting, research, and blueprint restrictions nullify for the sender)");
            sb.AppendLine("/craft {off} (Turns off super craft mode. Crafting, research, and blueprint restrictions re-activate for the sender)");
            sb.AppendLine("/daylength (Returns the amount of minutes in a full day)");
            sb.AppendLine("/daylength [#] (Sets the amount of minutes in a full day. Default 45.)");
            sb.AppendLine("/fall {on} (Turns on server-wide fall damage)");
            sb.AppendLine("/fall {off} (Turns off server-wide fall damage)");
            sb.AppendLine("/f {admin} *player name* (Gives faction admin to the specified faction member)");
            sb.AppendLine("/f {ally} *faction name* (Allies the specified faction)");
            sb.AppendLine("/f {build} {on} (Grants the sender build mode and allows them to build in zones)");
            sb.AppendLine("/f {build} {off} (Revokes build mode from the sender)");
            sb.AppendLine("/f {create} *name* (Creates and joins a faction with specified name)");
            sb.AppendLine("/f {deadmin} *player name* (Revokes faction admin from the specified faction member)");
            sb.AppendLine("/f {disband} (Disbands the current faction if user is the owner of said faction)");
            sb.AppendLine("/f {info} (Shows the sender's faction information)");
            sb.AppendLine("/f {info} *faction name* (Shows the faction information of a user or a faction)");
            sb.AppendLine("/f {invite} *name* (Invites the player with the specified name to your faction)");
            sb.AppendLine("/f {join} (Joins the faction of the last invitation received)");
            sb.AppendLine("/f {join} *name* (Joins the specified faction if invited)");
            sb.AppendLine("/f {kick} *name* (Kicks user with said name from faction)");
            sb.AppendLine("/f {leave} (Leaves current faction)");
            sb.AppendLine("/f {list} (Lists all factions on page 1)");
            sb.AppendLine("/f {list} [#] (Lists all factions on page #)");
            sb.AppendLine("/f {online} (Displays the statistics of members of the current faction)");
            sb.AppendLine("/f {players} (Lists the players of the current faction)");
            sb.AppendLine("/f {ownership} *player name* (Transfers ownership of faction to specified faction member)");
            sb.AppendLine("/f {safezone} {1} (Sets the first safe zone point)");
            sb.AppendLine("/f {safezone} {2} (Sets the second safe zone point)");
            sb.AppendLine("/f {safezone} {3} (Sets the third safe zone point)");
            sb.AppendLine("/f {safezone} {4} (Sets the forth safe zone point)");
            sb.AppendLine("/f {safezone} {set} (Establishes the safezone)");
            sb.AppendLine("/f {safezone} {clear} (Deletes the current safezone)");
            sb.AppendLine("/f {safezone} {clearall} (Deletes all safezones)");
            sb.AppendLine("/f {unally} *faction name* (Unallies the specified allied faction)");
            sb.AppendLine("/f {warzone} {1} (Sets the first safe zone point)");
            sb.AppendLine("/f {warzone} {2} (Sets the second safe zone point)");
            sb.AppendLine("/f {warzone} {3} (Sets the third safe zone point)");
            sb.AppendLine("/f {warzone} {4} (Sets the forth safe zone point)");
            sb.AppendLine("/f {warzone} {set} (Establishes the safezone)");
            sb.AppendLine("/f {warzone} {clear} (Deletes the current safezone)");
            sb.AppendLine("/f {warzone} {clearall} (Deletes all safezones)");
            sb.AppendLine("/feed (Feeds the sender)");
            sb.AppendLine("/feed *player name* (Feeds the designated player)");
            sb.AppendLine("/feed \"player name\" (Feeds the designated player by the exact name)");
            sb.AppendLine("/give <player name> <item name> (Gives the item to that player)");
            sb.AppendLine("/give <player name> <item name> [amount] (Gives the amount of the item to that player)");
            sb.AppendLine("/give <player name> [item id] (Gives 1 of the item with the corresponding id to that player)");
            sb.AppendLine("/give <player name> [item id] [amount] (Gives the amount of the item with the corresponding id to that player)");
            sb.AppendLine("/giveall <item name> (Gives the item to all players)");
            sb.AppendLine("/giveall <item name> [amount] (Gives the item to all players)");
            sb.AppendLine("/giveall [item id] (Gives the item to all players)");
            sb.AppendLine("/giveall [item id] [amount] (Gives the item to all players)");
            sb.AppendLine("/god (Gives god mode to the sender)");
            sb.AppendLine("/god *player name* (Gives the specified player god mode)");
            sb.AppendLine("/god \"player name\" (Gives the specified player with the exact name god mode)");
            sb.AppendLine("/heal (Heals the sender)");
            sb.AppendLine("/heal *player name* (Heals the designated player)");
            sb.AppendLine("/heal \"player name\" (Heals the designated player by the exact name)");
            sb.AppendLine("/help (Returns available commands for your current rank)");
            sb.AppendLine("/help [command without /] (Returns the documentation and syntax for the specified command)");
            sb.AppendLine("/hide {on} (Hides the sender from AI)");
            sb.AppendLine("/hide {off} (Reveals the sender to AI)");
            sb.AppendLine("/history {1-50} (Returns the the last # lines of the chat history)");
            sb.AppendLine("/i <item name> (Gives the item to you)");
            sb.AppendLine("/i <item name> [amount] (Gives the amount of the item to you)");
            sb.AppendLine("/i [item id] (Gives 1 of the item with the corresponding id to you)");
            sb.AppendLine("/i [item id] [amount] (Gives the amount of the item with the corresponding id to you)");
            sb.AppendLine("/join (Emulates the joining of yourself)");
            sb.AppendLine("/join <player name> (Emulates the joining of a fake player)");
            sb.AppendLine("/kick <player name> (Kick player with reason: \"Kicked by a(n) <Your Rank>\")");
            sb.AppendLine("/kick <player name> *reason* (Kick player with the specified reason)");
            sb.AppendLine("/kicke \"player name\" (Kick player by their exact name with reason: \"Kicked by a(n) <Your Rank>\")");
            sb.AppendLine("/kicke \"player name\" *reason* (Kick player by their exact name with the specified reason)");
            sb.AppendLine("/kickall (Kicks all users, except for the command executor, out of the server)");
            sb.AppendLine("/kill *player name* (Kills the specified player)");
            sb.AppendLine("/kill \"player name\" (Kills the specified player with that exact name)");
            sb.AppendLine("/kit [kit name] (Gives the user the specified kit if the user has the correct authority level)");
            sb.AppendLine("/kits (Lists the kits available to you)");
            sb.AppendLine("/leave (Emulates the joining of yourself)");
            sb.AppendLine("/leave *player name* (Emulates the leaving of a fake player)");
            sb.AppendLine("/mute *player name* (Mutes the player on global chat)");
            sb.AppendLine("/mute *player name* <time[s/m/h]>(Mutes the player on global chat for a period of time (time example: 15s or 30m))");
            sb.AppendLine("/nightlength (Returns the amount of minutes in a full night)");
            sb.AppendLine("/nightlength [#] (Sets the amount of minutes in a full night. Default 15.)");
            sb.AppendLine("/online (Returns the amount of players currently connected)");
            sb.AppendLine("/owner {on} (Gives access to display the owner of a structure upon hit)");
            sb.AppendLine("/owner {off} (Revokes access to display the owner of a structure upon hit)");
            sb.AppendLine("/players (Lists the names of all connected players)");
            sb.AppendLine("/pm <player name> *message* (Sends a private message to that player)");
            sb.AppendLine("/pos (Returns the player's position)");
            sb.AppendLine("/r (Replies to the last sent or received PM)");
            sb.AppendLine("/random <item name> (Gives 1 of the specified item to 1 random player)");
            sb.AppendLine("/random <item name> [amount] (Gives an amount of the specified item to 1 random player)");
            sb.AppendLine("/random <item name> [amount] [amount of winners] (Gives an amount of the specified item to random players)");
            sb.AppendLine("/random [item id] (Gives 1 of the specified item to 1 random player)");
            sb.AppendLine("/random [item id] [amount] (Gives an amount of the specified item to 1 random player)");
            sb.AppendLine("/random [item id] [amount] [amount of winners] (Gives an amount of the specified item to random players)");
            sb.AppendLine("/reload {config/whitelist/ranks/commands/kits/motd/bans/prefix/warps/controller/tables/all} (Reloads the specified file)");
            sb.AppendLine("/remove {on} (Gives access to delete entities (structures and AI entities) upon hit)");
            sb.AppendLine("/remove {off} (Revokes access to delete entities (structures and AI entities) upon hit)");
            sb.AppendLine("/removeall {on} (Gives access to delete entities (structures and AI entities) upon hit. If the entity is a structure, it will destroy all conntected structures too)");
            sb.AppendLine("/removeall {off} (Revokes access to delete entities (structures and AI entities) upon hit. If the entity is a structure, it will destroy all conntected structures too)");
            sb.AppendLine("/rules (Lists the server rules)");
            sb.AppendLine("/save (Saves all world data)");
            sb.AppendLine("/say *message* (Says a message through the plugin)");
            sb.AppendLine("/saypop *message* (Says a (!) dropdown message to all clients if the first word is more than 2 characters)");
            sb.AppendLine("/saypop [icon] *message* (Says a dropdown message to all clients with designated icon)");
            sb.AppendLine("/saypop [icon] *message* [#s] (Says a dropdown message to all clients with designated icon with a duration of # seconds (1-7 range))");
            sb.AppendLine("/share *player name* (Shares ownership of your doors with the designated user)");
            sb.AppendLine("/stop (Saves, deactivates, and effectively stops the server)");
            sb.AppendLine("/time (Returns current time of day)");
            sb.AppendLine("/time {0-24} (Sets time to a number between 0 and 24)");
            sb.AppendLine("/time {day} (Sets time to day)");
            sb.AppendLine("/time {freeze} (Freezes time)");
            sb.AppendLine("/time {night} (Sets time to night)");
            sb.AppendLine("/time {unfreeze} (Unfreezes time)");
            sb.AppendLine("/tp *player name* (Teleports the operator/sender to the designated user)");
            sb.AppendLine("/tp <player name 1> <player name 2> (Teleports player 1 to player 2)");
            sb.AppendLine("/tpa *player name* (Sends a teleport request to that user)");
            sb.AppendLine("/tpaccept (Accepts the last teleport request recieved)");
            sb.AppendLine("/tpaccept *player name* (Accepts the teleport request from that user)");
            sb.AppendLine("/tpdeny *player name* (Denies the teleport request from that user)");
            sb.AppendLine("/tpdeny {all} (Denies all current teleport requests)");
            sb.AppendLine("/tphere *player name (Teleports the specified player to you)");
            sb.AppendLine("/tppos [x] [y] [z] (Teleports your character to the designated vector)");
            sb.AppendLine("/uid (Returns your steam UID)");
            sb.AppendLine("/uid *player name* (Returns that user's steam UID)");
            sb.AppendLine("/uid \"player name\" (Returns the steam UID of the user with that exact name)");
            sb.AppendLine("/unban *player name* (Unbans the specified player)");
            sb.AppendLine("/ungod (Revokes god mode from the sender)");
            sb.AppendLine("/ungod *player name* (Revokes god mode from the specified player)");
            sb.AppendLine("/ungod \"player name\" (Revokes god mode from the specified player with that exact name)");
            sb.AppendLine("/unmute *player name* (Unmutes the player on global chat)");
            sb.AppendLine("/unshare {all} (Revokes ownership of your doors from everyone)");
            sb.AppendLine("/unshare *player name*(Revokes ownership of your doors from the designated user)");
            sb.AppendLine("/vanish {on} (Makes the sender vanish. If the sender reconnects, the name becomes invisible)");
            sb.AppendLine("/vanish {off} (Makes the sender appear. If the sender reconnects, the name becomes visible)");
            sb.AppendLine("/version (Returns the current running version of Rust Essentials)");
            sb.AppendLine("/warp *warp name* (Teleports you to the specified warp)");
            sb.AppendLine("/warps (Lists the warps available to you)");
            sb.AppendLine("/whitelist {add} [UID] (Adds the specified Steam UID to the whitelist)");
            sb.AppendLine("/whitelist {check} (Checks if you're currently on the whitelist)");
            sb.AppendLine("/whitelist {kick} (Kicks all players that are not whitelisted. This only work if whitelist is enabled)");
            sb.AppendLine("/whitelist {off} (Turns whitelist off)");
            sb.AppendLine("/whitelist {on} (Turns whitelist on)");
            sb.AppendLine("/whitelist {rem} [UID] (Removes the specified Steam UID to the whitelist)");

            return sb;
        }

        public static void reloadFileServer(string[] args)
        {
            if (args.Count() > 1)
            {
                string file = args[1];
                switch (file)
                {
                    case "config":
                        try { RustEssentialsBootstrap._load.loadConfig(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading config: " + ex.ToString()); }
                        break;
                    case "whitelist":
                        try { Whitelist.readWhitelist(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading whitelist: " + ex.ToString()); }
                        break;
                    case "ranks":
                        try { RustEssentialsBootstrap._load.loadRanks(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading ranks: " + ex.ToString()); }
                        break;
                    case "commands":
                        try { RustEssentialsBootstrap._load.loadCommands(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading commands: " + ex.ToString()); }
                        break;
                    case "kits":
                        try { RustEssentialsBootstrap._load.loadKits(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading kits: " + ex.ToString()); }
                        break;
                    case "motd":
                        try { RustEssentialsBootstrap._load.loadMOTD(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading MOTD: " + ex.ToString()); }
                        break;
                    case "bans":
                        try { RustEssentialsBootstrap._load.loadBans(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading bans: " + ex.ToString()); }
                        break;
                    case "prefix":
                        try { RustEssentialsBootstrap._load.loadPrefixes(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading prefixes: " + ex.ToString()); }
                        break;
                    case "warps":
                        try { RustEssentialsBootstrap._load.loadWarps(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading warps: " + ex.ToString()); }
                        break;
                    case "controller":
                        try { RustEssentialsBootstrap._load.loadController(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading controller: " + ex.ToString()); }
                        break;
                    case "tables":
                        try { RustEssentialsBootstrap._load.loadTables(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading loot tables: " + ex.ToString()); }
                        break;
                    case "all":
                        try { RustEssentialsBootstrap._load.loadConfig(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading config: " + ex.ToString()); }
                        try { Whitelist.readWhitelist(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading whitelist: " + ex.ToString()); }
                        try { RustEssentialsBootstrap._load.loadRanks(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading ranks: " + ex.ToString()); }
                        try { RustEssentialsBootstrap._load.loadCommands(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading commands: " + ex.ToString()); }
                        try { RustEssentialsBootstrap._load.loadKits(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading kits: " + ex.ToString()); }
                        try { RustEssentialsBootstrap._load.loadMOTD(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading MOTD: " + ex.ToString()); }
                        try { RustEssentialsBootstrap._load.loadBans(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading bans: " + ex.ToString()); }
                        try { RustEssentialsBootstrap._load.loadPrefixes(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading prefixes: " + ex.ToString()); }
                        try { RustEssentialsBootstrap._load.loadWarps(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading warps: " + ex.ToString()); }
                        try { RustEssentialsBootstrap._load.loadController(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading controller: " + ex.ToString()); }
                        try { RustEssentialsBootstrap._load.loadTables(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading loot tables: " + ex.ToString()); }
                        break;
                }
            }
        }

        public static void reloadFile(uLink.NetworkPlayer sender, string[] args)
        {
            if (args.Count() > 1)
            {
                string file = args[1].ToLower();
                switch (file)
                {
                    case "config":
                        if (RustEssentialsBootstrap._load.loadConfig())
                            Broadcast.broadcastTo(sender, "Config reloaded.");
                        else
                            Broadcast.broadcastTo(sender, "Config reloaded unsuccessfully! Possibly missing variables...");
                        break;
                    case "whitelist":
                        try { Whitelist.readWhitelist(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading whitelist: " + ex.ToString()); }
                        Broadcast.broadcastTo(sender, "Whitelist reloaded.");
                        break;
                    case "ranks":
                        try { RustEssentialsBootstrap._load.loadRanks(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading ranks: " + ex.ToString()); }
                        Broadcast.broadcastTo(sender, "Ranks reloaded.");
                        break;
                    case "commands":
                        try { RustEssentialsBootstrap._load.loadCommands(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading commands: " + ex.ToString()); }
                        Broadcast.broadcastTo(sender, "Command permissions reloaded.");
                        break;
                    case "kits":
                        try { RustEssentialsBootstrap._load.loadKits(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading kits: " + ex.ToString()); }
                        Broadcast.broadcastTo(sender, "Kits reloaded.");
                        break;
                    case "motd":
                        try { RustEssentialsBootstrap._load.loadMOTD(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading MOTD: " + ex.ToString()); }
                        Broadcast.broadcastTo(sender, "MOTD reloaded.");
                        break;
                    case "bans":
                        try { RustEssentialsBootstrap._load.loadBans(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading bans: " + ex.ToString()); }
                        Broadcast.broadcastTo(sender, "Bans reloaded.");
                        break;
                    case "prefix":
                        try { RustEssentialsBootstrap._load.loadPrefixes(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading prefixes: " + ex.ToString()); }
                        Broadcast.broadcastTo(sender, "Prefixes reloaded.");
                        break;
                    case "warps":
                        try { RustEssentialsBootstrap._load.loadWarps(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading warps: " + ex.ToString()); }
                        Broadcast.broadcastTo(sender, "Warps reloaded.");
                        break;
                    case "controller":
                        try { RustEssentialsBootstrap._load.loadController(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading controller: " + ex.ToString()); }
                        Broadcast.broadcastTo(sender, "Item Controller reloaded.");
                        break;
                    case "tables":
                        try { RustEssentialsBootstrap._load.loadTables(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading loot tables: " + ex.ToString()); }
                        Broadcast.broadcastTo(sender, "Loot tables reloaded.");
                        break;
                    case "all":
                        try { RustEssentialsBootstrap._load.loadConfig(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading config: " + ex.ToString()); }
                        try { Whitelist.readWhitelist(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading whitelist: " + ex.ToString()); }
                        try { RustEssentialsBootstrap._load.loadRanks(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading ranks: " + ex.ToString()); }
                        try { RustEssentialsBootstrap._load.loadCommands(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading commands: " + ex.ToString()); }
                        try { RustEssentialsBootstrap._load.loadKits(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading kits: " + ex.ToString()); }
                        try { RustEssentialsBootstrap._load.loadMOTD(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading MOTD: " + ex.ToString()); }
                        try { RustEssentialsBootstrap._load.loadBans(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading bans: " + ex.ToString()); }
                        try { RustEssentialsBootstrap._load.loadPrefixes(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading prefixes: " + ex.ToString()); }
                        try { RustEssentialsBootstrap._load.loadWarps(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading warps: " + ex.ToString()); }
                        try { RustEssentialsBootstrap._load.loadController(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading controller: " + ex.ToString()); }
                        try { RustEssentialsBootstrap._load.loadTables(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading loot tables: " + ex.ToString()); }
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

        public static void fakeLeaveServer(string[] args)
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

                PlayerClient[] possibleClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(fakeName));
                string rank = "";

                if (possibleClients.Count() == 1 && !removePrefix)
                    rank = "[" + findRank(possibleClients[0].userID.ToString()) + "] ";

                string leaveMessage = Vars.leaveMessage.Replace("$USER$", rank + fakeName);
                Broadcast.broadcastAll(leaveMessage);
            }
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

                PlayerClient[] possibleClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(fakeName));
                string rank = "";

                if (possibleClients.Count() == 1 && !removePrefix)
                    rank = "[" + findRank(possibleClients[0].userID.ToString()) + "] ";

                string leaveMessage = Vars.leaveMessage.Replace("$USER$", rank + fakeName);
                Broadcast.broadcastAll(leaveMessage);
            }
            else
            {
                string rank = "[" + findRank(senderClient.userID.ToString()) + "] ";

                string leaveMessage = Vars.leaveMessage.Replace("$USER$", (!removePrefix ? rank : "") + senderClient.userName);
                Broadcast.broadcastAll(leaveMessage);
            }
        }

        public static void setDayLengthServer(string[] args)
        {
            if (args.Count() > 1)
            {
                try
                {
                    if (!timeFrozen)
                    {
                        float length = Convert.ToSingle(args[1]);
                        Time.setDayLength(length);
                    }
                }
                catch (Exception ex)
                {
                    conLog.Error("SDLS: " + ex.ToString());
                }
            }
        }

        public static void setNightLengthServer(string[] args)
        {
            if (args.Count() > 1)
            {
                try
                {
                    if (!timeFrozen)
                    {
                        float length = Convert.ToSingle(args[1]);
                        Time.setNightLength(length);
                    }
                }
                catch (Exception ex)
                {
                    conLog.Error("SNLS: " + ex.ToString());
                }
            }
        }

        public static void setDayLength(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                try
                {
                    if (!timeFrozen)
                    {
                        float length = Convert.ToSingle(args[1]);
                        Time.setDayLength(length);
                        Broadcast.broadcastTo(senderClient.netPlayer, "Day length set to " + length.ToString() + " minutes.");
                    }
                    else
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "Time is currently frozen! Unfreeze time to change the day length.");
                    }
                }
                catch (Exception ex)
                {
                    Broadcast.broadcastTo(senderClient.netPlayer, "Day length must be a number!");
                    conLog.Error("SDL: " + ex.ToString());
                }
            }
            else
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "The day length is currently " + Time.getDayLength() + ".");
            }
        }

        public static void setNightLength(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                try
                {
                    if (!timeFrozen)
                    {
                        float length = Convert.ToSingle(args[1]);
                        Time.setNightLength(length);
                        Broadcast.broadcastTo(senderClient.netPlayer, "Night length set to " + length.ToString() + " minutes.");
                    }
                    else
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "Time is currently frozen! Unfreeze time to change the night length.");
                    }
                }
                catch (Exception ex)
                {
                    Broadcast.broadcastTo(senderClient.netPlayer, "Night length must be a number!");
                    conLog.Error("SNL: " + ex.ToString());
                }
            }
            else
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "The night length is currently " + Time.getDayLength() + ".");
            }
        }

        public static void setTimeServer(string[] args)
        {
            if (args.Count() > 1)
            {
                try
                {
                    float time = Convert.ToSingle(args[1]);
                    Time.setTime(time);
                }
                catch (Exception ex)
                {
                    if (args[1] == "freeze")
                    {
                        Time.freezeTime(true);
                    }
                    else if (args[1] == "unfreeze")
                    {
                        Time.freezeTime(false);
                    }
                    else if (args[1] == "day")
                    {
                        Time.setDay();
                    }
                    else if (args[1] == "night")
                    {
                        Time.setNight();
                    }
                    else
                    {
                        conLog.Error("STS: Unknown parameters \"" + string.Join(" ", args.ToArray()) + "\".");
                    }
                }
            }
        }

        public static void setTime(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                try
                {
                        float time = Convert.ToSingle(args[1]);
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
                    else
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "Unknown parameters \"" + string.Join(" ", args.ToArray()) + "\".");
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
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> kv in cycleMOTDList)
            {
                TimerPlus t = new TimerPlus();
                t.AutoReset = true;
                t.Interval = Convert.ToInt32(kv.Value.ElementAt(0).Key);
                t.Elapsed += ((sender, e) => cycleMOTDElapsed(kv.Key));
                t.Start();
            }
        }

        public static void onceMOTD()
        {
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> kv in onceMOTDList)
            {
                TimerPlus t = new TimerPlus();
                t.AutoReset = false;
                t.Interval = Convert.ToInt32(kv.Value.ElementAt(0).Key);
                t.Elapsed += ((sender, e) => onceMOTDElapsed(kv.Key));
                t.Start();
            }    
        }

        private static Dictionary<string, int> timeCycled = new Dictionary<string, int>();
        private static void cycleMOTDElapsed(string motdName)
        {
            if (!timeCycled.ContainsKey(motdName))
                timeCycled.Add(motdName, 0);

            timeCycled[motdName]++;
            if (timeCycled[motdName] > 1)
            {
                if (cycleMOTDList.ContainsKey(motdName))
                {
                    foreach (string s in cycleMOTDList[motdName].ElementAt(0).Value)
                    {
                        if (s.StartsWith("{/") && s.EndsWith("}"))
                        {
                            string command = s.Substring(1, s.Length - 2);
                            Commands.executeCMDServer(command);
                        }
                        else
                            Broadcast.broadcastAll(s);
                    }
                }
            }
        }
        private static void onceMOTDElapsed(string motdName)
        {
            if (onceMOTDList.ContainsKey(motdName))
            {
                foreach (string s in onceMOTDList[motdName].ElementAt(0).Value)
                {
                    if (s.StartsWith("{/") && s.EndsWith("}"))
                    {
                        string command = s.Substring(1, s.Length - 2);
                        Commands.executeCMDServer(command);
                    }
                    else
                        Broadcast.broadcastAll(s);
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

                if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                {
                    playerName = playerName.Substring(1, playerName.Length - 2);

                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(playerName));
                    if (possibleTargets.Count() == 0)
                        Broadcast.broadcastTo(sender, "No player names equal \"" + playerName + "\".");
                    else if (possibleTargets.Count() > 1)
                        Broadcast.broadcastTo(sender, "Too many player names equal \"" + playerName + "\".");
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

                        if (targetClient.netPlayer != sender)
                        {
                            Broadcast.noticeTo(sender, "♫", (b ? "God mode granted to " + targetClient.userName + "." : "Revoked " + targetClient.userName + "'s god mode."), 2, true);
                            Broadcast.noticeTo(targetClient.netPlayer, "♫", (b ? "God mode granted by " + senderName + "." : "God mode revoked by " + senderName + "."), 2, true);
                        }
                        else
                        {
                            Broadcast.noticeTo(sender, "♫", (b ? "God mode activated." : "God mode deactivated."), 2, true);
                        }
                    }
                }
                else
                {
                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
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

                        if (targetClient.netPlayer != sender)
                        {
                            Broadcast.noticeTo(sender, "♫", (b ? "God mode granted to " + targetClient.userName + "." : "Revoked " + targetClient.userName + "'s god mode."), 2, true);
                            Broadcast.noticeTo(targetClient.netPlayer, "♫", (b ? "God mode granted by " + senderName + "." : "God mode revoked by " + senderName + "."), 2, true);
                        }
                        else
                        {
                            Broadcast.noticeTo(sender, "♫", (b ? "God mode activated." : "God mode deactivated."), 2, true);
                        }
                    }
                }
            }
            else
            {
                PlayerClient senderClient = Array.Find(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == sender);
                TakeDamage component = senderClient.controllable.GetComponent<TakeDamage>();

                if (b)
                {
                    if (!godList.Contains(senderClient.userID.ToString()))
                        godList.Add(senderClient.userID.ToString());
                }
                else
                {
                    if (godList.Contains(senderClient.userID.ToString()))
                        godList.Remove(senderClient.userID.ToString());
                }

                Broadcast.noticeTo(sender, "♫", (b ? "God mode activated." : "God mode deactivated."), 2, true);
            }
        }

        //public static void OverheadPlayerName_Open(RPOS.InfoLabel label, HumanController HC)
        //{
        //    PlayerClient instantiatedPlayerClient = HC.instantiatedPlayerClient;
        //    if (instantiatedPlayerClient != null)
        //    {
        //        if (vanishedList.Contains(instantiatedPlayerClient.userID.ToString()))
        //            label.text = "";
        //        else
        //            label.text = instantiatedPlayerClient.userName;
        //    }
        //    label.transform = HC.headBone;
        //    label.color = Color.yellow;
        //}

        //public static bool OverheadPlayerName_Update(RPOS.InfoLabel label, HumanController HC)
        //{
        //    PlayerClient instantiatedPlayerClient = HC.instantiatedPlayerClient;
        //    if (instantiatedPlayerClient != null)
        //    {
        //        if (vanishedList.Contains(instantiatedPlayerClient.userID.ToString()))
        //            label.text = "";
        //        else
        //            label.text = instantiatedPlayerClient.userName;
        //        label.offset = (Vector3)(label.transform.InverseTransformDirection(HC.transform.up) * 0.3f);
        //        return true;
        //    }
        //    return false;
        //}

        public static void HostileScent(TakeDamage damage, HostileWildlifeAI HWAI)
        {
            try
            {
                if (HWAI != null)
                {
                    if (damage != null)
                    {
                        bool b = false;
                        try
                        {
                            foreach (PlayerClient pc in AllPlayerClients)
                            {
                                if (pc.controllable != null)
                                {
                                    if (pc.controllable.GetComponent<TakeDamage>() != null)
                                    {
                                        if (pc.controllable.GetComponent<TakeDamage>() == damage)
                                        {
                                            PlayerClient playerClient = pc;
                                            if (HWAI._targetTD != null)
                                            {
                                                b = hiddenList.Contains(playerClient.userID.ToString()) && HWAI._targetTD == playerClient.controllable.GetComponent<TakeDamage>();

                                                try
                                                {
                                                    if (!b)
                                                    {
                                                        if (isPlayer(damage.idMain))
                                                        {
                                                            Character character = damage.idMain as Character;
                                                            foreach (string UID in hiddenList)
                                                            {
                                                                if (UID == character.playerClient.userID.ToString() && HWAI._targetTD == character.playerClient.controllable.GetComponent<TakeDamage>())
                                                                    b = true;
                                                            }
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Vars.conLog.Error("HS #3: " + ex.ToString());
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Vars.conLog.Error("HS #2: " + ex.ToString());
                        }

                        if (!HWAI.IsScentBlind() && (((HWAI._state != 2) && (HWAI._state != 7)) && !HWAI.HasTarget()) && !b)
                        {
                            HWAI.ExitCurrentState();
                            HWAI.SetAttackTarget(damage);
                            HWAI.EnterState_Chase();
                        }

                        if (b && HWAI.HasTarget())
                        {
                            ignoringAIList.Add(HWAI);
                            HWAI._targetTD = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("HS #1: " + ex.ToString());
            }
        }

        public static List<HostileWildlifeAI> hadTarget = new List<HostileWildlifeAI>();
        public static void HostileChase(ulong millis, HostileWildlifeAI HWAI)
        {
            try
            {
                if (HWAI != null)
                {
                    if (millis != null)
                    {
                        if (HWAI._targetTD == null)
                        {
                            if (ignoringAIList.Contains(HWAI))
                            {
                                HWAI.GoScentBlind(10f);
                                HWAI.ExitCurrentState();
                                HWAI.idleClock.ResetRandomDurationSeconds(0, 0);
                                HWAI._state = 1;
                                if (!HWAI.idleSoundRefireClock.once)
                                {
                                    HWAI.idleSoundRefireClock.ResetRandomDurationSeconds(2.0, 4.0);
                                }
                                ignoringAIList.Remove(HWAI);
                            }
                            else
                                HWAI.LoseTarget();

                            if (hadTarget.Contains(HWAI))
                                hadTarget.Remove(HWAI);
                        }
                        else
                        {
                            bool b = false;
                            if (!hadTarget.Contains(HWAI))
                            {
                                try
                                {
                                    foreach (PlayerClient pc in AllPlayerClients)
                                    {
                                        if (pc.controllable != null)
                                        {
                                            if (pc.controllable.GetComponent<TakeDamage>() != null)
                                            {
                                                if (pc.controllable.GetComponent<TakeDamage>() == HWAI._targetTD)
                                                {
                                                    PlayerClient playerClient = pc;
                                                    b = hiddenList.Contains(playerClient.userID.ToString()) && HWAI._targetTD == playerClient.controllable.GetComponent<TakeDamage>();

                                                    try
                                                    {
                                                        if (!b)
                                                        {
                                                            if (isPlayer(HWAI._targetTD.idMain))
                                                            {
                                                                Character character = HWAI._targetTD.idMain as Character;
                                                                foreach (string UID in hiddenList)
                                                                {
                                                                    if (UID == character.playerClient.userID.ToString() && HWAI._targetTD == character.playerClient.controllable.GetComponent<TakeDamage>())
                                                                        b = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Vars.conLog.Error("HC #3: " + ex.ToString());
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Vars.conLog.Error("HC #2: " + ex.ToString());
                                }
                                hadTarget.Add(HWAI);
                            }

                            if (!b)
                            {
                                float num = HWAI.TargetRange();
                                if (num > HWAI.loseTargetRange)
                                {
                                    HWAI.LoseTarget();
                                }
                                else
                                {
                                    Vector3 vector = HWAI._targetTD.transform.position - HWAI.transform.position;
                                    vector.y = 0f;
                                    if (num <= HWAI.attackRange)
                                    {
                                        HWAI._wildMove.SetLookDirection(vector.normalized);
                                        HWAI.ExitCurrentState();
                                        HWAI.EnterState_Attack();
                                    }
                                    else
                                    {
                                        if (HWAI._wildMove.IsStuck())
                                        {
                                            if (HWAI.wasStuck)
                                            {
                                                if (HWAI.stuckClock.IntegrateTime_Reached(millis))
                                                {
                                                    Vector3 position = HWAI._targetTD.transform.position;
                                                    HWAI.LoseTarget();
                                                    HWAI.ExitCurrentState();
                                                    HWAI.EnterState_Flee(position, 0x2710L);
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                HWAI.wasStuck = true;
                                                HWAI.stuckClock.ResetRandomDurationSeconds(1.0, 2.0);
                                            }
                                        }
                                        else
                                        {
                                            HWAI.wasStuck = false;
                                        }
                                        if (HWAI.chaseSoundClock.IntegrateTime_Reached(millis))
                                        {
                                            BasicWildLifeAI.AISound chase = BasicWildLifeAI.AISound.Chase;
                                            if (num < 5f)
                                            {
                                                chase = BasicWildLifeAI.AISound.ChaseClose;
                                            }
                                            HWAI.NetworkSound(chase);
                                            HWAI.chaseSoundClock.ResetRandomDurationSeconds(1.5, 2.5);
                                        }
                                        HWAI._wildMove.SetMoveTarget(HWAI._targetTD.gameObject, HWAI.runSpeed);
                                        if (HWAI.targetReachClock.IntegrateTime_Reached(millis))
                                        {
                                            HWAI.LoseTarget();
                                            HWAI.ExitCurrentState();
                                            HWAI.EnterState_Idle();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                conLog.Error("HC #1: " + ex.ToString());
            }
        }

        public static void HostileHurt(DamageEvent damage, HostileWildlifeAI HWAI)
        {
            try
            {
                bool b = false;
                try
                {
                    if (isPlayer(damage.attacker.idMain))
                    {
                        b = hiddenList.Contains(damage.attacker.client.userID.ToString()) && HWAI._targetTD == damage.attacker.client.controllable.GetComponent<TakeDamage>();
                    }
                }
                catch (Exception ex)
                {
                    Vars.conLog.Error("HH: " + ex.ToString());
                }

                if (!HWAI.HasTarget() && (damage.attacker.character != null) && !b)
                {
                    HWAI.SetAttackTarget(damage.attacker.character.gameObject.GetComponent<TakeDamage>());
                    HWAI.ExitCurrentState();
                    HWAI.EnterState_Chase();
                }

                if (b && HWAI.HasTarget())
                    HWAI._targetTD = null;
            }
            catch (Exception ex)
            {
                Vars.conLog.Error(ex.ToString());
            }
        }

        public static void BasicHearFootstep(Vector3 origin, BasicWildLifeAI BWAI)
        {
            try
            {
                bool b = false;
                try
                {
                    foreach (PlayerClient pc in AllPlayerClients)
                    {
                        Character outChar;
                        Character.FindByUser(pc.userID, out outChar);

                        if (outChar != null && outChar.transform != null && outChar.transform.position != null && origin != null)
                        {
                            if (Vector3.Distance(outChar.transform.position, origin) < 1)
                            {
                                b = hiddenList.Contains(pc.userID.ToString());
                            }
                        }
                    }
                }
                catch (Exception ex2)
                {
                    conLog.Error("BHFS #2: " + ex2.ToString());
                }
                if (((BWAI._state != 2) && (BWAI._state != 7)) && BWAI.afraidOfFootsteps && !b)
                {
                    BWAI.ExitCurrentState();
                    BWAI.EnterState_Flee(origin);
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("BHFS #1: " + ex.ToString());
            }
        }

        public static void BasicHurt(DamageEvent damage, BasicWildLifeAI BWAI)
        {
            try
            {
                bool b = false;
                try
                {
                    if (damage.attacker.idMain is Character)
                    {
                        if (isPlayer(damage.attacker.idMain))
                        {
                            b = hiddenList.Contains(damage.attacker.client.userID.ToString());
                        }
                    }
                }
                catch { }

                if (((BWAI._state != 2) && (BWAI._state != 7)) && (damage.attacker.character != null) & !b)
                {
                    BWAI.ExitCurrentState();
                    BWAI.EnterState_Flee(BWAI.transform.position + new Vector3(UnityEngine.Random.Range((float)-1f, (float)1f), 0f, UnityEngine.Random.Range((float)-1f, (float)1f)));
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("BH: " + ex.ToString());
            }
        }

        public static void clearPlayer(PlayerClient senderClient, string[] args)
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

                if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                {
                    playerName = playerName.Substring(1, playerName.Length - 2);

                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(playerName));
                    if (possibleTargets.Count() == 0)
                        Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal \"" + playerName + "\".");
                    else if (possibleTargets.Count() > 1)
                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + playerName + "\".");
                    else
                    {
                        PlayerClient targetClient = possibleTargets[0];
                        clearInventory(targetClient);
                    }
                }
                else
                {
                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                    if (possibleTargets.Count() == 0)
                        Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                    else if (possibleTargets.Count() > 1)
                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                    else
                    {
                        PlayerClient targetClient = possibleTargets[0];
                        clearInventory(targetClient);
                    }
                }
            }
        }

        public static void feedPlayer(uLink.NetworkPlayer sender, string senderName, string[] args)
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

                if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                {
                    playerName = playerName.Substring(1, playerName.Length - 2);

                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(playerName));
                    if (possibleTargets.Count() == 0)
                        Broadcast.broadcastTo(sender, "No player names equal \"" + playerName + "\".");
                    else if (possibleTargets.Count() > 1)
                        Broadcast.broadcastTo(sender, "Too many player names equal \"" + playerName + "\".");
                    else
                    {
                        PlayerClient targetClient = possibleTargets[0];
                        Inventory inventory = targetClient.controllable.GetComponent<Inventory>();
                        Metabolism metabolism = inventory.GetComponent<Metabolism>();

                        metabolism.AddCalories(metabolism.GetRemainingCaloricSpace());

                        if (targetClient.netPlayer != sender)
                        {
                            Broadcast.noticeTo(sender, "♫", ("You fed " + targetClient.userName + "."), 2, true);
                            Broadcast.noticeTo(targetClient.netPlayer, "♫", ("You were fed by " + senderName + "."), 2, true);
                        }
                        else
                        {
                            Broadcast.noticeTo(sender, "♫", "You were fed.", 2, true);
                        }
                    }
                }
                else
                {
                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                    if (possibleTargets.Count() == 0)
                        Broadcast.broadcastTo(sender, "No player names equal or contain \"" + playerName + "\".");
                    else if (possibleTargets.Count() > 1)
                        Broadcast.broadcastTo(sender, "Too many player names contain \"" + playerName + "\".");
                    else
                    {
                        PlayerClient targetClient = possibleTargets[0];
                        Inventory inventory = targetClient.controllable.GetComponent<Inventory>();
                        Metabolism metabolism = inventory.GetComponent<Metabolism>();

                        metabolism.AddCalories(metabolism.GetRemainingCaloricSpace());

                        if (targetClient.netPlayer != sender)
                        {
                            Broadcast.noticeTo(sender, "♫", ("You fed " + targetClient.userName + "."), 2, true);
                            Broadcast.noticeTo(targetClient.netPlayer, "♫", ("You were fed by " + senderName + "."), 2, true);
                        }
                        else
                        {
                            Broadcast.noticeTo(sender, "♫", "You were fed.", 2, true);
                        }
                    }
                }
            }
            else
            {
                PlayerClient senderClient = Array.Find(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == sender);

                Inventory inventory = senderClient.controllable.GetComponent<Inventory>();
                Metabolism metabolism = inventory.GetComponent<Metabolism>();

                metabolism.AddCalories(metabolism.GetRemainingCaloricSpace());

                Broadcast.noticeTo(sender, "♫", "You were fed.", 2, true);
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

                if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                {
                    playerName = playerName.Substring(1, playerName.Length - 2);

                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(playerName));
                    if (possibleTargets.Count() == 0)
                        Broadcast.broadcastTo(sender, "No player names equal \"" + playerName + "\".");
                    else if (possibleTargets.Count() > 1)
                        Broadcast.broadcastTo(sender, "Too many player names equal \"" + playerName + "\".");
                    else
                    {
                        PlayerClient targetClient = possibleTargets[0];

                        TakeDamage component = targetClient.controllable.GetComponent<TakeDamage>();
                        Character targetChar;
                        Character.FindByUser(targetClient.userID, out targetChar);

                        component.Heal(targetChar.idMain, 100f);

                        Inventory inventory = targetClient.controllable.GetComponent<Inventory>();
                        Metabolism metabolism = inventory.GetComponent<Metabolism>();

                        metabolism.AddAntiRad(metabolism.GetRadLevel());

                        if (targetClient.netPlayer != sender)
                        {
                            Broadcast.noticeTo(sender, "♫", "You healed " + senderName + ".", 2, true);
                            Broadcast.noticeTo(targetClient.netPlayer, "♫", ("You were healed by " + senderName + "."), 2, true);
                        }
                        else
                        {
                            Broadcast.noticeTo(sender, "♫", "You were healed.", 2, true);
                        }
                    }
                }
                else
                {
                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
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

                        Inventory inventory = targetClient.controllable.GetComponent<Inventory>();
                        Metabolism metabolism = inventory.GetComponent<Metabolism>();

                        metabolism.AddAntiRad(metabolism.GetRadLevel());

                        if (targetClient.netPlayer != sender)
                        {
                            Broadcast.noticeTo(targetClient.netPlayer, "♫", ("You were healed by " + senderName + "."), 2, true);
                            Broadcast.noticeTo(sender, "♫", "You healed " + targetClient.userName + ".", 2, true);
                        }
                        else
                        {
                            Broadcast.noticeTo(sender, "♫", "You were healed.", 2, true);
                        }
                    }
                }
            }
            else
            {
                PlayerClient senderClient = Array.Find(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == sender);
                TakeDamage component = senderClient.controllable.GetComponent<TakeDamage>();
                Character targetChar;
                Character.FindByUser(senderClient.userID, out targetChar);

                component.Heal(targetChar.idMain, 100f);

                Inventory inventory = senderClient.controllable.GetComponent<Inventory>();
                Metabolism metabolism = inventory.GetComponent<Metabolism>();

                metabolism.AddAntiRad(metabolism.GetRadLevel());

                Broadcast.noticeTo(sender, "♫", "You were healed.", 2, true);
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
                        Broadcast.broadcastAll("Fall damage has been enabled for everyone.");
                        fallDamage = true;
                        break;
                    case "off":
                        Broadcast.broadcastAll("Fall damage has been disabled for everyone.");
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

                if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                {
                    playerName = playerName.Substring(1, playerName.Length - 2);

                    PlayerClient senderClient = Array.Find(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == sender);
                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(playerName));
                    if (possibleTargets.Count() == 0)
                        Broadcast.broadcastTo(sender, "No player names equal \"" + playerName + "\".");
                    else if (possibleTargets.Count() > 1)
                        Broadcast.broadcastTo(sender, "Too many player names equal \"" + playerName + "\".");
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
                                killList.Add(targetClient);

                                TakeDamage component = targetClient.controllable.GetComponent<TakeDamage>();
                                component.SetGodMode(false);
                                if (godList.Contains(targetClient.userID.ToString()))
                                    godList.Remove(targetClient.userID.ToString());
                                int result = (int)TakeDamage.Kill(idBase, idBase);
                            }
                            else
                            {
                                Broadcast.noticeTo(sender, "№", "You are not allowed to /kill those of higher authority.");
                                Broadcast.noticeTo(targetClient.netPlayer, "№", senderClient.userName + " tried to /kill you.");
                            }
                        }
                        catch (Exception ex) { Vars.conLog.Error("KT #1: " + ex.ToString()); }
                    }
                }
                else
                {
                    PlayerClient senderClient = Array.Find(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == sender);
                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
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
                                Broadcast.noticeTo(senderClient.netPlayer, "№", targetClient.userName + " fell victim to /kill.");
                                killList.Add(targetClient);

                                TakeDamage component = targetClient.controllable.GetComponent<TakeDamage>();
                                component.SetGodMode(false);
                                if (godList.Contains(targetClient.userID.ToString()))
                                    godList.Remove(targetClient.userID.ToString());
                                int result = (int)TakeDamage.Kill(idBase, idBase);
                            }
                            else
                            {
                                Broadcast.noticeTo(sender, "№", "You are not allowed to /kill those of higher authority.");
                                Broadcast.noticeTo(targetClient.netPlayer, "№", senderClient.userName + " tried to /kill you.");
                            }
                        }
                        catch (Exception ex) { Vars.conLog.Error("KT #2: " + ex.ToString()); }
                    }
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
                        string reason = currentBanReasons[kv.Key];
                        sw.WriteLine(kv.Value + "=" + kv.Key + " # " + reason);
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

                if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                {
                    playerName = playerName.Substring(1, playerName.Length - 2);

                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(playerName));
                    if (possibleTargets.Count() == 0)
                        Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal \"" + playerName + "\".");
                    else if (possibleTargets.Count() > 1)
                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + playerName + "\".");
                    else
                    {
                        PlayerClient targetClient = possibleTargets[0];
                        Broadcast.broadcastTo(senderClient.netPlayer, targetClient.userName + "'s UID is " + targetClient.userID.ToString() + ".");
                    }
                }
                else
                {
                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
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
                if (currentBans.ContainsValue(targetName)) // If the unban is by player name
                {
                    string UID = Array.Find(currentBans.ToArray(), (KeyValuePair<string, string> kv) => kv.Value == targetName).Key;
                    currentBans.Remove(UID);
                    currentBanReasons.Remove(UID);
                    saveBans();

                    Broadcast.noticeTo(playerClient.netPlayer, "☻", "Player " + targetName + " (" + UID + ") has been unbanned.", 2, true);
                }
                else if (currentBans.ContainsKey(targetName)) // If the unban is by UID
                {
                    try
                    {
                        string UID = Convert.ToInt64(targetName).ToString();
                        string playerName = "";
                        foreach (KeyValuePair<string, string> kv in currentBans)
                        {
                            if (kv.Key == UID)
                                playerName = kv.Value;
                        }
                        if (currentBans.ContainsValue(playerName))
                        {
                            currentBans.Remove(UID);
                            currentBanReasons.Remove(UID);
                            saveBans();

                            Broadcast.noticeTo(playerClient.netPlayer, "☻", "Player " + playerName + " (" + UID + ") has been unbanned.", 2, true);
                        }
                        else
                        {
                            Broadcast.noticeTo(playerClient.netPlayer, "№", "Player/UID " + targetName + " is not banned!");
                        }
                    }
                    catch (Exception ex)
                    {
                        Broadcast.noticeTo(playerClient.netPlayer, "№", "Player/UID " + targetName + " is not banned!");
                    }
                }
                else
                {
                    Broadcast.noticeTo(playerClient.netPlayer, "№", "Player/UID " + targetName + " is not banned!");
                }
            }
        }

        public static void banPlayer(PlayerClient senderClient, string[] args, bool exactName)
        {
            if (args.Count() > 1)
            {
                bool hadQuote = false;
                string targetName = "";
                int lastIndex = 0;
                if (!exactName)
                {
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
                }
                else
                {
                    List<string> playerNameList = new List<string>();
                    foreach (string s in args)
                    {
                        if (lastIndex > 0)
                        {
                            playerNameList.Add(s);
                        }
                        lastIndex++;
                    }

                    targetName = string.Join(" ", playerNameList.ToArray());
                }

                try
                {
                    string UID = Convert.ToInt64(targetName).ToString();
                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID.ToString() == UID);
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
                                    if (!currentBans.ContainsKey(target.userID.ToString()))
                                    {
                                        Broadcast.broadcastTo(target.networkPlayer, "You were banned! Reason: " + reason);
                                        target.Kick(NetError.Facepunch_Kick_Ban, false);
                                        Broadcast.broadcastAll("Player " + target.displayName + " was banned. Reason: " + reason);
                                        currentBans.Add(target.userID.ToString(), target.displayName);
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
                            if (!currentBans.ContainsKey(UID))
                            {
                                Broadcast.broadcastAll("UID " + UID + " was banned. Reason: " + reason);
                                ulong UIDLong = Convert.ToUInt64(UID);
                                currentBans.Add(UID, "Unknown Player");
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
                    if (exactName)
                    {
                        if (targetName.Split(' ').Count() > 1)
                            targetName = targetName.Substring(1, targetName.Length - 2);

                        PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(targetName));
                        if (possibleTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal \"" + targetName + "\".");
                        else if (possibleTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + targetName + "\".");
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
                                        if (!currentBans.ContainsKey(target.userID.ToString()))
                                        {
                                            Broadcast.broadcastTo(target.networkPlayer, "You were banned! Reason: " + reason);
                                            target.Kick(NetError.Facepunch_Kick_Ban, false);
                                            Broadcast.broadcastAll("Player " + target.displayName + " was banned. Reason: " + reason);
                                            currentBans.Add(target.userID.ToString(), target.displayName);
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
                    else
                    {
                        PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));
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
                                        if (!currentBans.ContainsKey(target.userID.ToString()))
                                        {
                                            Broadcast.broadcastTo(target.networkPlayer, "You were banned! Reason: " + reason);
                                            target.Kick(NetError.Facepunch_Kick_Ban, false);
                                            Broadcast.broadcastAll("Player " + target.displayName + " was banned. Reason: " + reason);
                                            currentBans.Add(target.userID.ToString(), target.displayName);
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
        }

        public static void kickPlayer(PlayerClient senderClient, string[] args, bool exactName)
        {
            if (args.Count() > 1)
            {
                bool hadQuote = false;
                string targetName = "";
                int lastIndex = 0;
                if (!exactName)
                {
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
                }
                else
                {
                    List<string> playerNameList = new List<string>();
                    foreach (string s in args)
                    {
                        if (lastIndex > 0)
                        {
                            playerNameList.Add(s);
                        }
                        lastIndex++;
                    }

                    targetName = string.Join(" ", playerNameList.ToArray());
                }

                if (exactName)
                {
                    if (targetName.Split(' ').Count() > 1)
                        targetName = targetName.Substring(1, targetName.Length - 2);

                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(targetName));
                    if (possibleTargets.Count() == 0)
                        Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal \"" + targetName + "\".");
                    else if (possibleTargets.Count() > 1)
                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + targetName + "\".");
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
                                    Broadcast.broadcastTo(target.networkPlayer, "You were kicked! Reason: " + reason);
                                    target.Kick(NetError.Facepunch_Kick_Ban, false);
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
                else
                {
                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));
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
                                    Broadcast.broadcastTo(target.networkPlayer, "You were kicked! Reason: " + reason);
                                    target.Kick(NetError.Facepunch_Kick_Ban, false);
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
        }

        public static void otherKick(NetUser target, string reason)
        {
            if (target != null)
            {
                kickQueue.Add(target.userID.ToString());
                Broadcast.broadcastTo(target.networkPlayer, "You were kicked! Reason: " + reason);
                Vars.conLog.Error("Player " + target.displayName + " (" + target.userID + ") was kicked for: " + reason);
                target.Kick(NetError.Facepunch_Kick_Ban, false);
            }
        }

        public static void whitelistKick(NetUser target, string reason)
        {
            if (target != null)
            {
                kickQueue.Add(target.userID.ToString());
                Broadcast.broadcastTo(target.networkPlayer, "You were kicked! Reason: " + reason);
                Vars.conLog.Error("Nonwhitelisted player " + target.displayName + " (" + target.userID + ") attempted to join.");
                target.Kick(NetError.Facepunch_Kick_Ban, false);
            }
        }

        public static void kickPlayer(NetUser target, string reason, bool isBan)
        {
            if (target != null)
            {
                kickQueue.Add(target.userID.ToString());
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

            return (target1Level < target2Level);
        }

        public static void getPlayerPos(PlayerClient senderClient)
        {
            Character senderChar;
            Character.FindByUser(senderClient.userID, out senderChar);

            string combineOutput = senderChar.transform.position.ToString();

            Broadcast.broadcastTo(senderClient.netPlayer, combineOutput);
        }

        public static void airdropServer(string[] args)
        {
            if (Vars.announceDrops)
                Broadcast.broadcastAll("Incoming airdrop!");
            SupplyDropZone.CallAirDrop();
        }

        public static void airdrop(uLink.NetworkPlayer sender, string[] args)
        {
            if (args.Count() > 1)
            {
                PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(args[1]));
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
                Time.setTime(Convert.ToSingle(Config.startTime));
                Time.setDayLength(Convert.ToSingle(Config.dayLength));
                Time.setNightLength(Convert.ToSingle(Config.nightLength));
                Time.freezeTime(Convert.ToBoolean(Config.freezeTime));
                
                conLog.Info("Overriding time...");
                conLog.Info("Overriding day length...");
                conLog.Info("Overriding night length...");

                if (Convert.ToBoolean(Config.freezeTime))
                    conLog.Info("Time frozen.");
            }
            catch (Exception ex)
            {
                conLog.Error("Something went wrong when overriding the enviroment! Skipping... Error: " + ex.ToString());
            }
        }

        public static void restoreKit(object sender, ElapsedEventArgs e, string kitName, string UID)
        {
            if (playerCooldowns.ContainsKey(UID))
            {
                if (playerCooldowns[UID].Count() > 1)
                {
                    TimerPlus tp = Array.Find(playerCooldowns[UID].ToArray(), (KeyValuePair<TimerPlus, string> kv) => kv.Value == kitName).Key;
                    playerCooldowns[UID].Remove(tp);
                }
                else if (playerCooldowns[UID].Count() == 1)
                    playerCooldowns.Remove(UID);
            }
        }

        public static void showPlayers(PlayerClient senderClient)
        {
            Broadcast.broadcastTo(senderClient.netPlayer, "All online players:", true);
            List<string> names = new List<string>();
            List<string> names2 = new List<string>();
            foreach (PlayerClient pc in AllPlayerClients.ToArray())
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

        public static void showWarps(PlayerClient senderClient)
        {
            string rank = Vars.findRank(senderClient.userID.ToString());

            Broadcast.broadcastTo(senderClient.netPlayer, "Available warps:", true);
            Vars.listWarps(rank, senderClient);
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
                                    {
                                        foreach (KeyValuePair<TimerPlus, string> kv in playerCooldowns[senderClient.userID.ToString()])
                                        {
                                            if (kv.Value == kitNameToLower)
                                            {
                                                if (Math.Round((kv.Key.TimeLeft / 1000)) > 0)
                                                    b = false;
                                            }
                                        }
                                    }
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
                                        t.Elapsed += (sender, e) => restoreKit(sender, e, kitNameToLower, senderClient.userID.ToString());
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
                                            Broadcast.noticeTo(senderClient.netPlayer, "✯", "You must wait " + (timeLeft > 999999999 ? "forever" : timeLeft.ToString() + " seconds") + " before using this.");
                                        }
                                    }
                                }
                            }
                            else // If I am not allowed to use this kit
                            {
                                if (kitsForUIDs.ContainsKey(senderClient.userID.ToString()))
                                {
                                    if (kitsForUIDs[senderClient.userID.ToString()].Contains(kitNameToLower))
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
                                                t.Elapsed += (sender, e) => restoreKit(sender, e, kitNameToLower, senderClient.userID.ToString());
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
                                                    Broadcast.noticeTo(senderClient.netPlayer, "✯", "You must wait " + (timeLeft > 999999999 ? "forever" : timeLeft.ToString() + " seconds") + " before using this.");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Broadcast.noticeTo(senderClient.netPlayer, ":(", "You do not have permission to do this.");
                                    }
                                }
                                else
                                {
                                    Broadcast.noticeTo(senderClient.netPlayer, ":(", "You do not have permission to do this.");
                                }
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
                                        t.Elapsed += (sender, e) => restoreKit(sender, e, kitNameToLower, senderClient.userID.ToString());
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
                                            Broadcast.noticeTo(senderClient.netPlayer, "✯", "You must wait " + (timeLeft > 999999999 ? "forever" : timeLeft.ToString() + " seconds") + " before using this.");
                                        }
                                    }
                                }
                            }
                            else // If the kit is truly assigned to rank, just not mine
                            {
                                if (kitsForUIDs.ContainsKey(senderClient.userID.ToString()))
                                {
                                    if (kitsForUIDs[senderClient.userID.ToString()].Contains(kitNameToLower))
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
                                                t.Elapsed += (sender, e) => restoreKit(sender, e, kitNameToLower, senderClient.userID.ToString());
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
                                                    Broadcast.noticeTo(senderClient.netPlayer, "✯", "You must wait " + (timeLeft > 999999999 ? "forever" : timeLeft.ToString() + " seconds") + " before using this.");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Broadcast.noticeTo(senderClient.netPlayer, ":(", "You do not have permission to do this.");
                                    }
                                }
                                else
                                {
                                    Broadcast.noticeTo(senderClient.netPlayer, ":(", "You do not have permission to do this.");
                                }
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
                    if (message.Contains("\""))
                    {
                        string targetName = "";
                        int lastIndex = 0;
                            List<string> nameList = new List<string>();

                        if (args[1].Contains("\""))
                        {
                            bool hadQuote = false;
                            foreach (string s in args)
                            {
                                if (s.StartsWith("\"")) hadQuote = true;
                                if (hadQuote)
                                {
                                    nameList.Add(s);
                                }
                                lastIndex++;
                                if (s.EndsWith("\""))
                                {
                                    hadQuote = false;
                                    break;
                                }
                            }

                            targetName = string.Join(" ", nameList.ToArray());
                        }
                        else
                        {
                            targetName = args[1];
                            nameList.Add(targetName);
                        }

                        if (targetName.StartsWith("\"") && targetName.EndsWith("\""))
                            targetName = targetName.Substring(1, targetName.Length - 2);

                        PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));
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
                                if (!nameList.Contains(s))
                                {
                                    newArgs.Add(s);
                                }
                            }

                            createItem(senderClient, targetClient, newArgs.ToArray(), message, true);
                        }
                    }
                    else
                    {
                        PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(args[1]));
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
                            createItem(senderClient, targetClient, newArgs.ToArray(), message, true);
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
                                    if (amount < 1)
                                        amount = 1;
                                }
                                catch (Exception ex) { Broadcast.broadcastTo(senderClient.netPlayer, "Amount must be an integer!"); }
                            }

                            addItem(targetClient, itemName, amount);
                            if (b)
                            {
                                if (senderClient != targetClient)
                                    Broadcast.broadcastTo(senderClient.netPlayer, "You gave " + targetClient.userName + " " + amount + " " + itemName);
                                Broadcast.noticeTo(targetClient.netPlayer, "☻", "You were given " + amount + " " + itemName + (senderClient.userName.Length > 0 && senderClient.userName != targetClient.userName ? " by " + senderClient.userName : ""));
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
                                    if (amount < 1)
                                        amount = 1;
                                }
                                catch (Exception ex) { Broadcast.broadcastTo(senderClient.netPlayer, "Amount must be an integer!"); }
                            }

                            if (Vars.itemIDs.Values.Contains(itemName))
                            {
                                addItem(targetClient, itemName, amount);
                                if (b)
                                {
                                    if (senderClient != targetClient)
                                        Broadcast.broadcastTo(senderClient.netPlayer, "You gave " + targetClient.userName + " " + amount + " " + itemName);
                                    Broadcast.noticeTo(targetClient.netPlayer, "☻", "You were given " + amount + " " + itemName + (senderClient.userName.Length > 0 && senderClient.userName != targetClient.userName ? " by " + senderClient.userName : ""));
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

        public static void giveAllServer(string[] args)
        {
            try
            {
                if (args.Count() > 1)
                {
                    if (args[1].Contains("\""))
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
                        if (Vars.itemIDs.Values.Contains(itemName))
                        {
                            int amount = 1;
                            if (args.Count() - 1 > lastIndex)
                            {
                                try
                                {
                                    amount = Convert.ToInt16(args[lastIndex + 1]);
                                    if (amount < 1)
                                        amount = 1;
                                }
                                catch (Exception ex) { }
                            }

                            foreach (PlayerClient targetClient in AllPlayerClients)
                            {
                                addItem(targetClient, itemName, amount);
                            }
                            Broadcast.noticeAll("☻", "All players were given " + amount + " " + itemName);
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
                            }

                            int amount = 1;
                            if (args.Count() > 2)
                            {
                                try
                                {
                                    amount = Convert.ToInt16(args[2]);
                                    if (amount < 1)
                                        amount = 1;
                                }
                                catch (Exception ex) { }
                            }

                            if (Vars.itemIDs.Values.Contains(itemName))
                            {
                                foreach (PlayerClient targetClient in AllPlayerClients)
                                {
                                    addItem(targetClient, itemName, amount);
                                }
                                Broadcast.noticeAll("☻", "All players were given " + amount + " " + itemName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void giveAll(PlayerClient senderClient, string[] args)
        {
            try
            {
                if (args.Count() > 1)
                {
                    if (args[1].Contains("\""))
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
                                    if (amount < 1)
                                        amount = 1;
                                }
                                catch (Exception ex) { Broadcast.broadcastTo(senderClient.netPlayer, "Amount must be an integer!"); }
                            }

                            foreach (PlayerClient targetClient in AllPlayerClients)
                            {
                                addItem(targetClient, itemName, amount);
                            }
                            Broadcast.noticeAll("☻", "All players were given " + amount + " " + itemName);
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
                                    if (amount < 1)
                                        amount = 1;
                                }
                                catch (Exception ex) { Broadcast.broadcastTo(senderClient.netPlayer, "Amount must be an integer!"); }
                            }

                            if (Vars.itemIDs.Values.Contains(itemName))
                            {
                                foreach (PlayerClient targetClient in AllPlayerClients)
                                {
                                    addItem(targetClient, itemName, amount);
                                }
                                Broadcast.noticeAll("☻", "All players were given " + amount + " " + itemName);
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

        public static void giveRandomServer(string[] args)
        {
            try
            {
                if (args.Count() > 1)
                {
                    if (args[1].Contains("\""))
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
                        if (Vars.itemIDs.Values.Contains(itemName))
                        {
                            int amount = 1;
                            int playerAmount = 1;
                            if (args.Count() - 2 > lastIndex)
                            {
                                try
                                {
                                    amount = Convert.ToInt16(args[lastIndex + 1]);
                                    if (amount < 1)
                                        amount = 1;
                                }
                                catch (Exception ex) { }

                                try
                                {
                                    playerAmount = Convert.ToInt16(args[lastIndex + 2]);
                                    if (playerAmount < 1)
                                        playerAmount = 1;
                                }
                                catch (Exception ex)
                                {
                                }
                            }

                            if (args.Count() - 1 > lastIndex && args.Count() - 2 == lastIndex)
                            {
                                try
                                {
                                    amount = Convert.ToInt16(args[lastIndex + 1]);
                                }
                                catch (Exception ex) { }
                            }   

                            Thread t = new Thread(() => giveawayItem(itemName, amount, playerAmount));
                            t.Start();
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
                            }

                            int amount = 1;
                            if (args.Count() > 2)
                            {
                                try
                                {
                                    amount = Convert.ToInt16(args[2]);
                                    if (amount < 1)
                                        amount = 1;
                                }
                                catch (Exception ex) {  }
                            }

                            int playerAmount = 1;
                            if (args.Count() > 3)
                            {
                                try
                                {
                                    playerAmount = Convert.ToInt16(args[3]);
                                    if (playerAmount < 1)
                                        playerAmount = 1;
                                }
                                catch (Exception ex) {  }
                            }

                            if (Vars.itemIDs.Values.Contains(itemName))
                            {
                                Thread t = new Thread(() => giveawayItem(itemName, amount, playerAmount));
                                t.Start();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void giveRandom(PlayerClient senderClient, string[] args)
        {
            try
            {
                if (args.Count() > 1)
                {
                    if (args[1].Contains("\""))
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
                            int playerAmount = 1;
                            if (args.Count() - 2 > lastIndex)
                            {
                                try
                                {
                                    amount = Convert.ToInt16(args[lastIndex + 1]);
                                    if (amount < 1)
                                        amount = 1;
                                }
                                catch (Exception ex) { Broadcast.broadcastTo(senderClient.netPlayer, "Amount must be an integer!"); }

                                try
                                {
                                    playerAmount = Convert.ToInt16(args[lastIndex + 2]);
                                    if (playerAmount < 1)
                                        playerAmount = 1;
                                }
                                catch (Exception ex)
                                {
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Player amount must be an integer or \"all\"!");
                                }
                            }

                            if (args.Count() - 1 > lastIndex && args.Count() - 2 == lastIndex)
                            {
                                try
                                {
                                    amount = Convert.ToInt16(args[lastIndex + 1]);
                                }
                                catch (Exception ex) { Broadcast.broadcastTo(senderClient.netPlayer, "Amount must be an integer!"); }
                            }

                            Thread t = new Thread(() => giveawayItem(itemName, amount, playerAmount));
                            t.Start();
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
                                    if (amount < 1)
                                        amount = 1;
                                }
                                catch (Exception ex) { Broadcast.broadcastTo(senderClient.netPlayer, "Amount must be an integer!"); }
                            }

                            int playerAmount = 1;
                            if (args.Count() > 3)
                            {
                                try
                                {
                                    playerAmount = Convert.ToInt16(args[3]);
                                    if (playerAmount < 1)
                                        playerAmount = 1;
                                }
                                catch (Exception ex) { Broadcast.broadcastTo(senderClient.netPlayer, "Player amount must be an integer!"); }
                            }

                            if (Vars.itemIDs.Values.Contains(itemName))
                            {
                                Thread t = new Thread(() => giveawayItem(itemName, amount, playerAmount));
                                t.Start();
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

        public static InventoryItem.MergeResult ResearchItemKit(IInventoryItem otherItem, IResearchToolItem researchItem)
        {
            BlueprintDataBlock block2;
            PlayerInventory inventory = researchItem.inventory as PlayerInventory;
            if ((inventory == null) || (otherItem.inventory != inventory))
            {
                return InventoryItem.MergeResult.Failed;
            }
            PlayerClient playerClient = Array.Find(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == inventory.networkView.owner);
            ItemDataBlock datablock = otherItem.datablock;
            if ((datablock == null) || !datablock.isResearchable || restrictResearch.Contains(otherItem.datablock.name))
            {
                if (datablock == null)
                    return InventoryItem.MergeResult.Failed;
                if (!datablock.isResearchable || restrictResearch.Contains(otherItem.datablock.name))
                {
                    if (!permitResearch.Contains(otherItem.datablock.name) && !craftList.Contains(playerClient.userID.ToString()))
                    {
                        Rust.Notice.Popup(inventory.networkView.owner, "", "You cannot research this item!", 4f);
                        return InventoryItem.MergeResult.Failed;
                    }
                }
            }
            if (!inventory.AtWorkBench() && researchAtBench && !craftList.Contains(playerClient.userID.ToString()))
            {
                Rust.Notice.Popup(inventory.networkView.owner, "", "You must be at a workbench to research.", 4f);
                return InventoryItem.MergeResult.Failed;
            }
            if (!BlueprintDataBlock.FindBlueprintForItem<BlueprintDataBlock>(otherItem.datablock, out block2))
            {
                Rust.Notice.Popup(inventory.networkView.owner, "", "There is no crafting recipe for this.", 4f);
                return InventoryItem.MergeResult.Failed;
            }
            if (inventory.KnowsBP(block2))
            {
                Rust.Notice.Popup(inventory.networkView.owner, "", "You already researched this.", 4f);
                return InventoryItem.MergeResult.Failed;
            }
            IInventoryItem paper;
            int numPaper = 1;
            if (researchPaper && !craftList.Contains(playerClient.userID.ToString()))
            {
                if (hasItem(playerClient, "Paper", out paper))
                {
                    if (paper.Consume(ref numPaper))
                        paper.inventory.RemoveItem(paper.slot);
                }
                else
                {
                    Rust.Notice.Popup(inventory.networkView.owner, "", "Researching requires paper.", 4f);
                    return InventoryItem.MergeResult.Failed;
                }
            }
            inventory.BindBlueprint(block2);
            Rust.Notice.Popup(inventory.networkView.owner, "", "You can now craft: " + otherItem.datablock.name, 4f);
            int numWant = 1;
            if (!infiniteResearch)
            {
                if (researchItem.Consume(ref numWant))
                    researchItem.inventory.RemoveItem(researchItem.slot);
            }
            return InventoryItem.MergeResult.Combined;
        }

        public static void ResearchItem(IBlueprintItem item, BlueprintDataBlock BDB)
        {
            PlayerInventory inventory = item.inventory as PlayerInventory;
            if (inventory != null)
            {
                bool b = false;
                PlayerClient playerClient = Array.Find(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == inventory.networkView.owner);
                if (!restrictBlueprints.Contains(BDB.resultItem.name) || craftList.Contains(playerClient.userID.ToString()))
                {
                    if (inventory.BindBlueprint(BDB))
                    {
                        int count = 1;
                        if (item.Consume(ref count))
                        {
                            inventory.RemoveItem(item.slot);
                        }
                        Rust.Notice.Popup(inventory.networkView.owner, "", "You can now craft: " + BDB.resultItem.name, 4f);
                    }
                    else
                    {
                        Rust.Notice.Popup(inventory.networkView.owner, "", "You already researched this.", 4f);
                    }
                }
                else
                    Broadcast.noticeTo(inventory.networkView.owner, "", "You cannot research this item!", 4);
            }
        }

        public static bool CanCraft(int amount, Inventory workbenchInv, BlueprintDataBlock BDB)
        {
            if (BDB.lastCanWorkResult == null)
            {
                BDB.lastCanWorkResult = new List<int>();
            }
            else
            {
                BDB.lastCanWorkResult.Clear();
            }
            if (BDB.lastCanWorkIngredientCount == null)
            {
                BDB.lastCanWorkIngredientCount = new List<int>(BDB.ingredients.Length);
            }
            else
            {
                BDB.lastCanWorkIngredientCount.Clear();
            }
            PlayerClient playerClient = Array.Find(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == workbenchInv.networkView.owner);
            if (playerClient != null)
            {
                if (!craftList.Contains(playerClient.userID.ToString()))
                {
                    if (BDB.RequireWorkbench && craftAtBench)
                    {
                        CraftingInventory component = workbenchInv.GetComponent<CraftingInventory>();
                        if ((component == null) || !component.AtWorkBench())
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                if (BDB.RequireWorkbench && craftAtBench)
                {
                    CraftingInventory component = workbenchInv.GetComponent<CraftingInventory>();
                    if ((component == null) || !component.AtWorkBench())
                    {
                        return false;
                    }
                }
            }
            if (playerClient != null)
            {
                if (!craftList.Contains(playerClient.userID.ToString()))
                {
                    foreach (BlueprintDataBlock.IngredientEntry entry in BDB.ingredients)
                    {
                        if (entry.amount != 0)
                        {
                            int item = workbenchInv.CanConsume(entry.Ingredient, entry.amount * amount, BDB.lastCanWorkResult);
                            if (item <= 0)
                            {
                                BDB.lastCanWorkResult.Clear();
                                BDB.lastCanWorkIngredientCount.Clear();
                                return false;
                            }
                            BDB.lastCanWorkIngredientCount.Add(item);
                        }
                    }
                }
            }
            else
            {
                foreach (BlueprintDataBlock.IngredientEntry entry in BDB.ingredients)
                {
                    if (entry.amount != 0)
                    {
                        int item = workbenchInv.CanConsume(entry.Ingredient, entry.amount * amount, BDB.lastCanWorkResult);
                        if (item <= 0)
                        {
                            BDB.lastCanWorkResult.Clear();
                            BDB.lastCanWorkIngredientCount.Clear();
                            return false;
                        }
                        BDB.lastCanWorkIngredientCount.Add(item);
                    }
                }
            }
            return true;
        }

        public static bool CraftItem(int amount, Inventory workbenchInv, BlueprintDataBlock BDB)
        {
            if (!BDB.CanWork(amount, workbenchInv))
            {
                return false;
            }
            int num = 0;
            PlayerClient playerClient = Array.Find(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == workbenchInv.networkView.owner);
            if (playerClient != null)
            {
                if (!restrictCrafting.Contains(BDB.resultItem.name) || craftList.Contains(playerClient.userID.ToString()))
                {
                    for (int i = 0; i < BDB.ingredients.Length; i++)
                    {
                        int count = BDB.ingredients[i].amount * amount;
                        if (count != 0)
                        {
                            int num4 = BDB.lastCanWorkIngredientCount[i];
                            for (int j = 0; j < num4; j++)
                            {
                                IInventoryItem item;
                                int slot = BDB.lastCanWorkResult[num++];
                                if (workbenchInv.GetItem(slot, out item) && item.Consume(ref count))
                                {
                                    workbenchInv.RemoveItem(slot);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Broadcast.noticeTo(workbenchInv.networkView.owner, "♨", "You cannot craft this item!", 4);
                    return false;
                }
            }
            else
            {
                if (!restrictCrafting.Contains(BDB.resultItem.name))
                {
                    for (int i = 0; i < BDB.ingredients.Length; i++)
                    {
                        int count = BDB.ingredients[i].amount * amount;
                        if (count != 0)
                        {
                            int num4 = BDB.lastCanWorkIngredientCount[i];
                            for (int j = 0; j < num4; j++)
                            {
                                IInventoryItem item;
                                int slot = BDB.lastCanWorkResult[num++];
                                if (workbenchInv.GetItem(slot, out item) && item.Consume(ref count))
                                {
                                    workbenchInv.RemoveItem(slot);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Broadcast.noticeTo(workbenchInv.networkView.owner, "♨", "You cannot craft this item!", 4);
                    return false;
                }
            }
            workbenchInv.AddItemAmount(BDB.resultItem, amount * BDB.numResultItem);
            return true;
        }

        public static void giveawayItem(string itemName, int amount, int playerAmount)
        {
            try
            {
                List<PlayerClient> winners = new List<PlayerClient>();
                List<string> winnerNames = new List<string>();
                List<PlayerClient> possibleWinners = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => !vanishedList.Contains(pc.userID.ToString()) && !lastWinners.Contains(pc.userID.ToString()) && pc.userName.Length > 0).ToList();
                lastWinners.Clear();
                string winnerList = "";
                System.Random rnd = new System.Random();
                playerAmount = Mathf.Clamp(playerAmount, 1, possibleWinners.Count);
                for (int i = 0; i < playerAmount; i++)
                {
                    PlayerClient randomClient = possibleWinners[rnd.Next(0, possibleWinners.Count)];
                    winners.Add(randomClient);
                    winnerNames.Add(randomClient.userName);
                    lastWinners.Add(randomClient.userID.ToString());
                    possibleWinners.Remove(randomClient);
                }

                if (winnerNames.Count == 2)
                    winnerList = string.Join(" and ", winnerNames.ToArray());
                else if (winnerNames.Count > 2)
                {
                    string lastName = winnerNames.Last();
                    winnerNames.Remove(lastName);

                    winnerList = string.Join(", ", winnerNames.ToArray());
                    winnerList += ", and " + lastName;
                }
                else if (winnerNames.Count == 1)
                    winnerList = winnerNames.First();

                Broadcast.noticeAll("?", "Starting item giveaway! Who will win?...", 4);
                Thread.Sleep(4500);
                Broadcast.noticeAll("?", "3", 1);
                Thread.Sleep(1500);
                Broadcast.noticeAll("?", "2", 1);
                Thread.Sleep(1500);
                Broadcast.noticeAll("?", "1", 1);
                Thread.Sleep(1500);
                Broadcast.noticeAll("!", "Congratulations to " + winnerList + " on winning " + amount + " " + itemName + "!", 4);
                foreach (PlayerClient targetClient in winners)
                {
                    addItem(targetClient, itemName, amount);
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error(ex.ToString());
            }
        }

        public static void DeployableKilled(DeployableObject DO)
        {
            if (DO.handleDeathHere)
            {
                if (DO.clientDeathEffect != null)
                {
                    NetCull.RPC(NetEntityID.Get((UnityEngine.MonoBehaviour)DO), "Client_OnKilled", uLink.RPCMode.OthersBuffered);
                }
                NetCull.Destroy(DO.gameObject);
                if (DO.corpseObject != null)
                {
                    bool b = true;
                    if (!doorStops && (DO.corpseObject.name == "MetalDoor_Corpse" || DO.corpseObject.name == "WoodDoor_Corpse"))
                        b = false;

                    if (b)
                        NetCull.InstantiateStatic(DO.corpseObject, DO.transform.position, DO.transform.rotation);
                }
            }
        }
    }
}
