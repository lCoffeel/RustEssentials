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
using System.Diagnostics;
//using MySql.Data;
//using MySql.Data.MySqlClient;

namespace RustEssentials.Util
{
    public class Vars
    {
        public static string rootDir = Directory.GetCurrentDirectory();
        public static string dataDir = Path.Combine(rootDir, "rust_server_Data");
        public static string dllDir = Path.Combine(dataDir, "Managed");
        public static string saveDir = Path.Combine(rootDir, "save");
        public static string essentialsDir = Path.Combine(saveDir, "RustEssentials");
        public static string logsDir = Path.Combine(essentialsDir, "Logs");
        public static string tablesDir = Path.Combine(essentialsDir, "Tables");
        public static string bigBrotherDir = Path.Combine(logsDir, "BigBrother");
        public static string storageLogsDir = Path.Combine(bigBrotherDir, "Storage Logs");
        public static string sleeperDeathLogsDir = Path.Combine(bigBrotherDir, "Sleeper Death Logs");
        public static string cfgFile = Path.Combine(essentialsDir, "config.ini");
        public static string whiteListFile = Path.Combine(essentialsDir, "whitelist.dat");
        public static string ranksFile = Path.Combine(essentialsDir, "ranks.ini");
        public static string commandsFile = Path.Combine(essentialsDir, "commands.ini");
        public static string allCommandsFile = Path.Combine(essentialsDir, "allCommands.txt");
        public static string itemsFile = Path.Combine(essentialsDir, "itemIDs.txt");
        public static string kitsFile = Path.Combine(essentialsDir, "kits.ini");
        public static string defaultLoadoutFile = Path.Combine(essentialsDir, "default_loadout.ini");
        public static string motdFile = Path.Combine(essentialsDir, "motd.ini");
        public static string bansFile = Path.Combine(essentialsDir, "bans.ini");
        public static string prefixFile = Path.Combine(essentialsDir, "prefix.ini");
        public static string warpsFile = Path.Combine(essentialsDir, "warps.ini");
        public static string doorsFile = Path.Combine(essentialsDir, "door_data.dat");
        public static string removerDataFile = Path.Combine(essentialsDir, "remover_data.dat");
        public static string removerBlacklistFile = Path.Combine(essentialsDir, "remover_blacklist.ini");
        public static string buildDataFile = Path.Combine(essentialsDir, "build_data.dat");
        public static string factionsFile = Path.Combine(essentialsDir, "factions.dat");
        public static string homesFile = Path.Combine(essentialsDir, "homes.dat");
        public static string bettyFile = Path.Combine(essentialsDir, "bouncing_betties.dat");
        public static string cooldownsFile = Path.Combine(essentialsDir, "kit_cooldowns.dat");
        public static string warpCooldownsFile = Path.Combine(essentialsDir, "warp_cooldowns.dat");
        public static string requestCooldownsFile = Path.Combine(essentialsDir, "tpaPer_cooldowns.dat");
        public static string requestCooldownsAllFile = Path.Combine(essentialsDir, "tpaAll_cooldowns.dat");
        public static string itemControllerFile = Path.Combine(essentialsDir, "controller.ini");
        public static string zonesFile = Path.Combine(essentialsDir, "zones.dat");
        public static string offenseFile = Path.Combine(essentialsDir, "offense.dat");
        public static string killsFile = Path.Combine(essentialsDir, "kills.dat");
        public static string deathsFile = Path.Combine(essentialsDir, "deaths.dat");
        public static string decayFile = Path.Combine(essentialsDir, "decay.ini");
        public static string donorKitsFile = Path.Combine(essentialsDir, "donorkits.ini");
        public static string pathsFile = Path.Combine(dataDir, "paths.ini");
        public static string defaultRank = "Default";
        public static string currentBuild = ""; // Change to an empty string on official release!
        public static string currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, 5) + currentBuild;
        public static string remoteVersion = "?.?.?";
        public static string currentLog;
        public static string currentChatLog;
        public static string currentStorageLog;
        public static string currentSleeperDeathsLog;

        public static Logging conLog = new Logging();

        public static Assembly API = null;

        public static AppDomain domain = AppDomain.CurrentDomain;

        public static RustEssentialsBootstrap REB = null;

        // SAVED VARIABLES START
        public static bool logStorageTransfer = false;
        public static bool enableConsoleLogging = true;
        public static bool enableChatLogging = true;
        public static bool logToFile = true;
        public static bool enableWhitelist = false;
        public static bool useMySQL = false;
        public static bool useSteamGroup = false;
        public static bool autoRefresh = true;
        public static bool useAsMembers = false;
        public static bool announceDrops = true;
        public static bool onFirstPlayer = false;
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
        public static bool enableRepair = true;
        public static bool forceNudity = false;
        public static bool onlyOnJoin = false;
        public static bool denyRequestWarzone = true;
        public static bool enableInHouse = true;
        public static bool doorStops = true;
        public static bool researchAtBench = true;
        public static bool infiniteResearch = false;
        public static bool researchPaper = false;
        public static bool craftAtBench = true;
        public static bool enableFallSound = true;
        public static bool enableDurability = true;
        public static bool enableRemover = true;
        public static bool returnItems = true;
        public static bool enableTRSVoting = false;
        public static bool enableRSVoting = false;
        public static bool noErrors = true;
        public static bool timeFrozen = false;
        public static bool firstPlayerJoined = false;
        public static bool firstPlayerInit = false;
        public static bool enableRank = true;
        public static bool enableLimitedSleepers = false;
        public static bool removeOnDeath = false;
        public static bool removeOnDisconnect = false;
        public static bool disregardCeilingWeight = true;
        public static bool disregardPillarWeight = true;
        public static bool disregardFoundationWeight = true;
        public static bool hideKills = false;
        public static bool killsToConsole = false;
        public static bool enableDecay = true;
        public static bool enableCustomDecay = false;
        public static bool checkIfInZone = true;
        public static bool enableAntiSpeed = true;
        public static bool enableAntiJump = true;
        public static bool enableAntiBP = true;
        public static bool enableAntiAW = true;
        public static bool enableAntiRange = true;
        public static bool enableWordWrap = true;
        public static bool constantFullMoon = false;
        public static bool logBroadcastErrors = true;
        public static bool catchBroadcastErrors = false;
        public static bool disconnectEvenIfNull = true;
        public static bool enableShopify = false;
        public static bool useDefaultPaths = true;
        public static bool enableDropItem = true;
        public static bool enableDoorHolding = false;
        public static bool includePositionsInLog = false;
        public static bool logBedPlacements = false;
        public static bool sunBasedCompass = false;
        public static bool enableStorageLogs = true;
        public static bool enableSleeperDeathLogs = true;
        public static bool enableKeepItems = false;
        public static bool sendChatToConsoles = true;
        public static bool enableDropdownKills = true;
        public static bool enableAllyName = false;
        public static bool enableDropdownFactionHits = true;
        public static bool enableDropdownAllyHits = false;
        public static bool runningAPI = false;
        public static bool isReady = false;
        public static bool factionActivateBetty = false;
        public static bool allyActivateBetty = false;
        public static bool ownerActivateBetty = false;
        public static bool bettyHurtFaction = true;
        public static bool bettyHurtAlly = true;
        public static bool bettyHurtOwner = false;
        public static bool ownerPickupBetty = true;
        public static bool factionPickupBetty = false;
        public static bool allyPickupBetty = false;
        public static bool neutralPickupBetty = false;
        public static bool bettyNearOtherHouses = false;
        public static bool enableBouncingBetty = true;
        public static bool returnBettyMaterials = true;
        public static bool bettyDeathDeleteItems = false;
        public static bool bettyOnlyOnFlat = true;
        public static bool enableAntiFamilyShare = false;
        public static bool checkModeIsSet = false;
        public static bool moveBackSpeed = false;
        public static bool moveBackJump = false;
        public static bool overrideWoodResources = false;
        public static bool overrideOreResources = false;
        public static bool overrideAIResources = false;
        public static bool multiplyMaxWood = false;
        public static bool multiplyMaxOre = false;
        public static bool multiplyMaxAIResources = false;
        public static bool sendAHToConsole = true;
        public static bool versionOnJoin = true;
        public static bool enableWithGuns = false;
        public static bool enableOneHit = false;
        public static bool confirmOneHit = true;
        public static bool enableTimedRemover = false;
        public static bool enableKickBanMessages = true;
        public static bool enableMuteMessageToAll = true;
        public static bool usingEssentialsDirOverride = false;

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
        public static string murderMessage = "$KILLER$ [ $WEAPON$ | $PART$ | $DISTANCE$ ] $VICTIM$";
        public static string murderMessageUnknown = "$KILLER$ killed $VICTIM$ [ $PART$ | $DISTANCE$ ].";
        public static string accidentMessage = "$VICTIM$ got mauled by a $KILLER$.";
        public static string dropdownKillMessage = "You killed $VICTIM$ [ $PART$ | $DISTANCE$ ]";
        public static string TRSvotingMessage = "$PLAYER$ was rewarded for voting for the server! Type /vote for more info.";
        public static string RSvotingMessage = "$PLAYER$ was rewarded for voting for the server! Type /vote for more info.";
        public static string wandName = "Stone Hatchet";
        public static string portalName = "P250";
        public static string TRSAPIKey = "0";
        public static string RSAPIKey = "0";
        public static string steamAPIKey = "0";
        public static string shopifyAPIKey = "";
        public static string TRSvoteLink = "http://nothingtoseehere.donotvisit/";
        public static string RSvoteLink = "http://nothingtoseehere.donotvisit/";
        public static string shopifyLink = "http://nothingtoseehere.donotvisit/";
        public static string defaultColor = "#66CCFF";
        public static string dropItemMessage = "Your inventory is full! All given items will now be dropped at your feet until otherwise.";
        public static string serverIP = "";

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
            { "ass" },{ "assfuck" }
        };

        public static double lastAirdropTime = 0;
        public static int minimumPlayers = 1;
        public static int planeCount = 1;
        public static int directDistance = 150;
        public static int chatLogCap = 15;
        public static int logCap = 15;
        public static int storageLogsCap = 15;
        public static int sleeperDeathLogsCap = 15;
        public static int minimumNameCount = 2;
        public static int maximumNameCount = 15;
        public static int requestDelay = 10;
        public static int warpDelay = 10;
        public static int requestCooldownType = 0;
        public static int dropMode = 0;
        public static int minimumCrates = 1;
        public static int maximumCrates = 3;
        public static int dropTime = 12;
        public static int loopIndex = 0;
        public static int lowerViolationInterval = 13;
        public static int violationLimit = 15;
        public static int offenseLimit = 3;
        public static int wordWrapLimit = 85;
        public static int infAmmoClipSize = -1;
        public static int bettiesPerPlayer = 3;
        public static int bettyArmingDelay = 5;
        public static int maxLightsPerPerson = 10;
        public static int maxLightsPerHouse = 30;
        public static int checkMode = 0;
        public static int maximumPing = 0;
        public static int memberLimit = 15;
        public static int homeLimit = 1;

        public static long requestCooldown = 900000;
        public static long factionHomeCooldown = 1800000;
        public static long homeCooldown = 900000;
        public static long factionHomeDelay = 30000;
        public static long homeDelay = 30000;
        public static double dropInterval = 3600000;
        public static long refreshInterval = 15000;
        public static double nudityRefreshInterval = 5000;
        public static long sleeperElapseInterval = 30000;
        public static long decayObjectInterval = 300;
        public static long decayStructureInterval = 2100000;
        public static long decayWoodDelayInterval = 172800;
        public static long decayMetalDelayInterval = 345600;
        public static long removerDeactivateInterval = 60000;

        public static float calculateInterval = 0.8f;
        public static float wandDistance = 50f;
        public static float maximumSpeed = 0.0128f;
        public static float maximumJumpSpeed = 0.004f;
        public static float rangeFlexibility = 5f;
        public static float bleedingRadius = 12f;
        public static float breakLegsRadius = 10f;
        public static float activateRadius = 5f;
        public static float hurtRadius = 5f;
        public static float maxBettyPlayerDamage = 600f;
        public static float maxBettyObjectDamage = 100f;
        public static float neutralDamage = 1f;
        public static float friendlyDamage = 0f;
        public static float allyDamage = 0.70f;
        public static float warDamage = 1f;
        public static float warFriendlyDamage = 0f;
        public static float warAllyDamage = 0.70f;
        public static float freezeRefreshDelay = 200f;
        public static float freezeDistance = 1f;
        public static float rockMultiplier = 1f;
        public static float sHatchetMultiplier = 1f;
        public static float hatchetMultiplier = 1f;
        public static float pickaxeMultiplier = 1f;
        public static float distanceFromOtherHouses = 15f;
        public static float defaultLightsRange = 50f;
        public static float maxLightsRange = 50f;
        public static float bedAndBagDistance = 0f;
        public static float gatewayDistance = 4.5f;
        public static float pingLimit = 0;
        public static float removerAttackDelay = 10f;
        // SAVED VARIABLES END

        //public static MySqlConnection mysqlConnection;

        public static List<string> allFiles = new List<string>()
        {
            { cfgFile },
            { whiteListFile },
            { ranksFile },
            { commandsFile },
            { itemsFile },
            { allCommandsFile },
            { kitsFile },
            { defaultLoadoutFile },
            { motdFile },
            { doorsFile },
            { removerBlacklistFile },
            { removerDataFile },
            { factionsFile },
            { homesFile },
            { bettyFile },
            { cooldownsFile },
            { warpCooldownsFile },
            { requestCooldownsFile },
            { requestCooldownsAllFile },
            { bansFile },
            { prefixFile },
            { warpsFile },
            { zonesFile },
            { itemControllerFile },
            { killsFile },
            { deathsFile },
            { decayFile },
            { buildDataFile },
            //{ donorKitsFile },
            { pathsFile },
            { offenseFile }
        };
        public static List<string> allDirs = new List<string>()
        {
            { saveDir },
            { essentialsDir },
            { logsDir },
            { tablesDir },
            { bigBrotherDir },
            { storageLogsDir },
            { sleeperDeathLogsDir }
        };
        public static List<ulong> whitelist = new List<ulong>();
        public static List<string> totalCommands = new List<string>();
        public static List<ulong> inGlobal = new List<ulong>();
        public static List<ulong> inDirect = new List<ulong>();
        public static List<ulong> inFaction = new List<ulong>();
        public static List<ulong> inGlobalV = new List<ulong>();
        public static List<ulong> inDirectV = new List<ulong>();
        public static List<ulong> inFactionV = new List<ulong>();
        public static List<ulong> mutedUsers = new List<ulong>();
        public static List<string> historyGlobal = new List<string>();
        public static List<string> historyDirect = new List<string>();
        public static List<ulong> kickQueue = new List<ulong>();
        public static List<ulong> groupMembers = new List<ulong>();
        public static List<ulong> completeDoorAccess = new List<ulong>();
        public static List<ulong> bettyPickupAccess = new List<ulong>();
        public static List<ulong> godList = new List<ulong>();
        public static List<ulong> destroyerList = new List<ulong>();
        public static List<ulong> oposList = new List<ulong>();
        public static List<ulong> removerList = new List<ulong>();
        public static List<ulong> destroyerAllList = new List<ulong>();
        public static List<ulong> elevatorList = new List<ulong>();
        public static List<ulong> ownershipList = new List<ulong>();
        public static List<ulong> hiddenList = new List<ulong>();
        public static List<string> unassignedWarps = new List<string>();
        public static List<string> unassignedKits = new List<string>();
        public static List<ulong> emptyPrefixes = new List<ulong>();
        public static List<ulong> vanishedList = new List<ulong>();
        public static List<ulong> portalList = new List<ulong>();
        public static List<ulong> bypassList = new List<ulong>();
        public static List<ulong> explosiveBulletList = new List<ulong>();
        public static List<ulong> unlAmmoList = new List<ulong>();
        public static List<ulong> infAmmoList = new List<ulong>();
        public static List<ulong> buildList = new List<ulong>();
        public static List<ulong> craftList = new List<ulong>();
        public static List<string> restrictItems = new List<string>();
        public static List<string> restrictCrafting = new List<string>();
        public static List<string> restrictResearch = new List<string>();
        public static List<string> permitResearch = new List<string>();
        public static List<string> restrictBlueprints = new List<string>();
        public static List<ulong> lastWinners = new List<ulong>();
        public static List<string> noninheritedKits = new List<string>();
        public static List<string> noninheritedWarps = new List<string>();
        public static List<string> excludeFromSleepers = new List<string>();
        public static List<string> excludeFromFamilyCheck = new List<string>();
        public static List<string> removerObjectBlacklist = new List<string>();
        public static List<ulong> wasHit = new List<ulong>();
        public static List<string> modMessageRanks = new List<string>();
        public static List<string> commandsToChat = new List<string>();
        public static List<GameObject> animalCorpses = new List<GameObject>();
        public static BettyKillList<BouncingBettyKill> diedToBetty = new BettyKillList<BouncingBettyKill>();
        public static List<Items.Item> TRSItems = new List<Items.Item>();
        public static List<Items.Item> bettyRecipe = new List<Items.Item>();
        public static List<Items.Item> RSItems = new List<Items.Item>();
        public static List<EnvDecay> envDecaysInZones = new List<EnvDecay>();
        public static List<DeployableObject> objectsInZones = new List<DeployableObject>();
        public static List<StructureComponent> structuresInZones = new List<StructureComponent>();
        public static Dictionary<ulong, List<Items.Item>> oldPlayerArmor = new Dictionary<ulong, List<Items.Item>>();
        public static Dictionary<ulong, List<Items.Item>> oldPlayerInventory = new Dictionary<ulong, List<Items.Item>>();
        public static Dictionary<PlayerClient, double> allyShotMessages = new Dictionary<PlayerClient, double>();
        public static Dictionary<GameObject, double> removerTimes = new Dictionary<GameObject, double>();
        public static Dictionary<ulong, TimerPlus> removerTimers = new Dictionary<ulong, TimerPlus>();
        public static List<HostileWildlifeAI> ignoringAIList = new List<HostileWildlifeAI>();
        public static List<PlayerClient> AllPlayerClients = new List<PlayerClient>();
        public static Dictionary<PlayerClient, string> playerIPs = new Dictionary<PlayerClient, string>();
        public static Dictionary<ulong, int> playerOffenses = new Dictionary<ulong, int>();
        public static Dictionary<ulong, uLink.NetworkPlayer> notifyList = new Dictionary<ulong, uLink.NetworkPlayer>();
        public static Zones safeZones = new Zones();
        public static Zones warZones = new Zones();
        public static List<ulong> followGhostList = new List<ulong>();
        public static Dictionary<ulong, Vector3> ghostList = new Dictionary<ulong, Vector3>();
        public static Dictionary<ulong, Vector3> ghostPositions = new Dictionary<ulong, Vector3>();
        public static Dictionary<PlayerClient, Vector2> firstPoints = new Dictionary<PlayerClient, Vector2>();
        public static Dictionary<PlayerClient, Vector2> secondPoints = new Dictionary<PlayerClient, Vector2>();
        public static Dictionary<PlayerClient, Vector2> thirdPoints = new Dictionary<PlayerClient, Vector2>();
        public static Dictionary<PlayerClient, Vector2> fourthPoints = new Dictionary<PlayerClient, Vector2>();
        public static Dictionary<PlayerClient, Zone> inWarZone = new Dictionary<PlayerClient, Zone>();
        public static Dictionary<PlayerClient, Character> AllCharacters = new Dictionary<PlayerClient, Character>();
        public static Dictionary<PlayerClient, int> violationCount = new Dictionary<PlayerClient, int>();
        public static Dictionary<Character, Vector3> lastPositions = new Dictionary<Character, Vector3>();
        public static Dictionary<PlayerClient, Zone> inSafeZone = new Dictionary<PlayerClient, Zone>();
        public static List<GameObject> beingDestroyed = new List<GameObject>();
        public static List<PlayerClient> killList = new List<PlayerClient>();
        public static List<PlayerClient> isTeleporting = new List<PlayerClient>();
        public static List<PlayerClient> currentlyTeleporting = new List<PlayerClient>();
        public static List<PlayerClient> ghostTeleporting = new List<PlayerClient>();
        public static List<PlayerClient> antihackTeleport = new List<PlayerClient>();
        public static List<PlayerClient> isAccepting = new List<PlayerClient>();
        public static List<string> currentIPBans = new List<string>();
        public static Dictionary<string, List<Items.Item>> defaultLoadout = new Dictionary<string, List<Items.Item>>()
        {
            {"Hotbar", new List<Items.Item>(){
                new Items.Item("Rock", 1),
                new Items.Item("Bandage", 2),
                new Items.Item("Torch", 1)
            }}
        };

        public static Dictionary<GameObject, double> removerDelayedObjects = new Dictionary<GameObject, double>();
        public static Dictionary<ulong, List<ulong>> sharingData = new Dictionary<ulong, List<ulong>>();
        public static Dictionary<ulong, List<ulong>> removerSharingData = new Dictionary<ulong, List<ulong>>();
        public static Dictionary<ulong, List<ulong>> buildSharingData = new Dictionary<ulong, List<ulong>>();
        public static Dictionary<ulong, string> playerPrefixes = new Dictionary<ulong, string>();
        public static Dictionary<string, string> rankPrefixes = new Dictionary<string, string>();
        public static Dictionary<string, string> currentBans = new Dictionary<string, string>();
        public static Dictionary<string, string> currentBanReasons = new Dictionary<string, string>();
        public static Dictionary<ulong, int> playerKills = new Dictionary<ulong, int>();
        public static Dictionary<ulong, int> playerDeaths = new Dictionary<ulong, int>();
        public static Dictionary<string, long> playerEcon = new Dictionary<string, long>();
        public static Dictionary<string, long> decayIntervals = new Dictionary<string, long>();
        public static Dictionary<ulong, TimerPlus> blockedRequestsAll = new Dictionary<ulong, TimerPlus>();
        public static Dictionary<ulong, TimerPlus> blockedFHomes = new Dictionary<ulong, TimerPlus>();
        public static Dictionary<ulong, TimerPlus> blockedHomes = new Dictionary<ulong, TimerPlus>();
        public static Dictionary<ulong, float> wandList = new Dictionary<ulong, float>();
        public static Dictionary<ulong, List<string>> previousArmor = new Dictionary<ulong, List<string>>();
        public static Dictionary<string, List<Items.Item>> donorKits = new Dictionary<string, List<Items.Item>>();
        public static Dictionary<string, List<ulong>> rankList = new Dictionary<string, List<ulong>>();
        public static Dictionary<string, List<string>> motdList = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> enabledCommands = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> kitsForRanks = new Dictionary<string, List<string>>();
        public static Dictionary<ulong, List<string>> kitsForUIDs = new Dictionary<ulong, List<string>>();
        public static Dictionary<string, List<string>> warpsForRanks = new Dictionary<string, List<string>>();
        public static Dictionary<ulong, List<string>> warpsForUIDs = new Dictionary<ulong, List<string>>();
        public static Dictionary<ulong, List<string>> factionInvites = new Dictionary<ulong, List<string>>();
        public static Dictionary<string, LootSpawnList> originalLootTables = new Dictionary<string, LootSpawnList>();
        public static Dictionary<string, List<string>> historyFaction = new Dictionary<string, List<string>>();
        public static Dictionary<ulong, TimerPlus> muteTimes = new Dictionary<ulong, TimerPlus>();
        public static Dictionary<ulong, Vector3> frozenPlayers = new Dictionary<ulong, Vector3>();
        public static Dictionary<PlayerClient, PlayerClient> latestPM = new Dictionary<PlayerClient, PlayerClient>();
        public static Dictionary<PlayerClient, PlayerClient> latestRequests = new Dictionary<PlayerClient, PlayerClient>();
        public static Dictionary<ulong, ulong> latestFactionRequests = new Dictionary<ulong, ulong>();
        public static Dictionary<PlayerClient, Dictionary<PlayerClient, TimerPlus>> teleportRequests = new Dictionary<PlayerClient, Dictionary<PlayerClient, TimerPlus>>();
        public static Dictionary<ulong, Dictionary<ulong, TimerPlus>> blockedRequestsPer = new Dictionary<ulong, Dictionary<ulong, TimerPlus>>();
        public static FactionList factions = new FactionList();
        public static Dictionary<int, string> itemIDs = new Dictionary<int, string>();
        public static Dictionary<string, List<Items.Item>> kits = new Dictionary<string, List<Items.Item>>();
        public static Dictionary<string, Vector3> warps = new Dictionary<string, Vector3>();
        public static MOTDList<MOTD> cycleMOTDList = new MOTDList<MOTD>();
        public static MOTDList<MOTD> onceMOTDList = new MOTDList<MOTD>();
        public static MOTDList<MOTD> listMOTDList = new MOTDList<MOTD>();
        public static Dictionary<string, TimerPlus> cycleMOTDTimers = new Dictionary<string, TimerPlus>();
        public static Dictionary<string, TimerPlus> onceMOTDTimers = new Dictionary<string, TimerPlus>();
        public static Dictionary<string, TimerPlus> listMOTDTimers = new Dictionary<string, TimerPlus>();
        public static Dictionary<string, long> kitCooldowns = new Dictionary<string, long>();
        public static Dictionary<string, long> warpCooldowns = new Dictionary<string, long>();
        public static Dictionary<Zone, List<DeployableObject>> zoneObjects = new Dictionary<Zone, List<DeployableObject>>();
        public static Dictionary<Zone, List<EnvDecay>> zoneEnvDecays = new Dictionary<Zone, List<EnvDecay>>();
        public static Dictionary<Zone, List<StructureComponent>> zoneStructures = new Dictionary<Zone, List<StructureComponent>>();
        public static Dictionary<ulong, Dictionary<TimerPlus, string>> activeKitCooldowns = new Dictionary<ulong, Dictionary<TimerPlus, string>>();
        public static Dictionary<ulong, Dictionary<TimerPlus, string>> activeWarpCooldowns = new Dictionary<ulong, Dictionary<TimerPlus, string>>();
        public static Dictionary<string, StringBuilder> textForFiles = new Dictionary<string, StringBuilder>()
        {
            { cfgFile, FileTexts.cfgText() },
            { ranksFile, FileTexts.ranksText() },
            { commandsFile, FileTexts.commandsText() },
            { allCommandsFile, FileTexts.allCommandsText() },
            { kitsFile, FileTexts.kitsText() },
            { defaultLoadoutFile, FileTexts.defaultLoadoutText() },
            { motdFile, FileTexts.motdText() },
            { prefixFile, FileTexts.prefixText() },
            { warpsFile, FileTexts.warpsText() },
            { itemControllerFile, FileTexts.itemControllerText() },
            { decayFile, FileTexts.decayText() },
            //{ donorKitsFile, FileTexts.donorKitsText() },
            { pathsFile, FileTexts.pathsText() },
            { removerBlacklistFile, FileTexts.removerBlacklistText() }
        };

        public static void shuttingDown(string reason)
        {
            Vars.conLog.Info("Shutdown executed. Reason:");
            Vars.conLog.Info(reason);
            Vars.callHook("RustEssentialsAPI.Hooks", "OnServerShutdown", false, reason);
        }

        public static string filterNames(string playerName, ulong uid)
        {
            if (!emptyPrefixes.Contains(uid))
            {
                if (!playerPrefixes.ContainsKey(uid))
                {
                    foreach (KeyValuePair<string, List<ulong>> kv in rankList)
                    {
                        if (kv.Value.Contains(uid))
                        {
                            if (rankPrefixes.ContainsKey(kv.Key))
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

        public static string capitalizeFirst(string s)
        {
            string result = "";

            result = s[0].ToString().ToUpper();
            int curIndex = 0;
            foreach (char c in s)
            {
                if (curIndex != 0)
                    result += c;
                curIndex++;
            }

            return result;
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

        public static IEnumerator delayRemover(GameObject GO)
        {
            yield return new WaitForSeconds(removerAttackDelay);

            if (Vars.removerDelayedObjects.ContainsKey(GO))
                Vars.removerDelayedObjects.Remove(GO);
        }

        public static double currentTime
        {
            get
            {
                return Math.Abs(Math.Round((DateTime.Now - Process.GetCurrentProcess().StartTime).TotalMilliseconds));
            }
        }

        public static double lastSpeedTime = 0;
        public static bool wasDeactivated = true;
        public static IEnumerator speedHackUpdate()
        {
            while (true)
            {
                try
                {
                    if (enableAntiJump || enableAntiSpeed)
                    {
                        if (wasDeactivated)
                        {
                            Antihack.collectiveMovementCheck();
                            wasDeactivated = false;
                        }
                        lastSpeedTime = currentTime;
                    }
                    else
                        wasDeactivated = true;
                }
                catch (Exception ex)
                {
                    conLog.Error("SHU: " + ex.ToString());
                }

                yield return new WaitForSeconds(Vars.calculateInterval);
            }
        }

        public static IEnumerator secondaryUpdate()
        {
            RustServerManagement RSM = RustServerManagement.Get();
            bool waitLonger = false;
            while (true)
            {
                foreach (KeyValuePair<ulong, Vector3> kv in frozenPlayers)
                {
                    try
                    {
                        uLink.NetworkPlayer netPlayer;
                        if (getNetPlayer(kv.Key, out netPlayer))
                        {
                            Vector3 playerPos = kv.Value;
                            bool shouldSetBack = true;
                            PlayerClient playerClient;
                            if (getPlayerClient(netPlayer, out playerClient))
                            {
                                Vector3 currentPos;
                                if (getPosition(playerClient, out currentPos))
                                {
                                    if (Vector3.Distance(currentPos, playerPos) < freezeDistance)
                                        shouldSetBack = false;
                                }
                            }

                            if (shouldSetBack)
                                RSM.TeleportPlayerToWorld(netPlayer, playerPos);
                        }
                    }
                    catch (Exception ex)
                    {
                        conLog.Error("SECOND: " + ex.ToString());
                        waitLonger = true;
                    }
                }
                if (waitLonger)
                    yield return new WaitForSeconds(1);
                else
                    yield return new WaitForSeconds((freezeRefreshDelay / 1000));
            }
        }

        public static IEnumerator mainUpdate()
        {
            double lastZoneExecute = currentTime;
            double lastNudityExecute = currentTime;
            double lastItemsExecute = currentTime;
            double lastRequestExecute = currentTime;
            double lastPlaneExecute = currentTime;

            bool hadError = false;
            while (true)
            {
                try
                {

                    if (currentTime - lastZoneExecute > 500)
                    {
                        lastZoneExecute = currentTime;
                        cycleZones();
                    }

                    if (!onlyOnJoin)
                    {
                        if (currentTime - lastNudityExecute > nudityRefreshInterval)
                        {
                            lastNudityExecute = currentTime;
                            sendNudity();
                        }
                    }

                    if (currentTime - lastItemsExecute > 5000)
                    {
                        lastItemsExecute = currentTime;
                        checkItems();
                        Data.saveRequestsPer();
                        Data.saveRequestsAll();
                        Data.saveCooldowns();
                        Data.saveWarpCooldowns();
                    }

                    if (currentTime - lastPlaneExecute > dropInterval)
                    {
                        lastPlaneExecute = currentTime;
                        lastAirdropTime = currentTime;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
                        if (AllPlayerClients.Count >= minimumPlayers && Vars.dropMode == 1)
                        {
                            for (int i = 0; i < planeCount; i++)
                            {
                                airdropServer();
                            }
                        }
                    }
                    hadError = false;
                }
                catch (Exception ex)
                {
                    conLog.Error("MAIN:" + ex.ToString());
                    hadError = true;
                }

                if (hadError)
                    yield return new WaitForSeconds(1);
                else
                    yield return new WaitForSeconds(0.05f);
            }
        }

        public static Zone safeZone = null;
        public static Zone warZone = null;
        public static void cycleZones()
        {
            List<PlayerClient> playerClients = new List<PlayerClient>();
            foreach (PlayerClient pc in Vars.AllPlayerClients) { playerClients.Add(pc); }

            foreach (PlayerClient pc in playerClients)
            {
                try
                {
                    if (pc != null)
                    {
                        Character playerChar;
                        Character.FindByUser(pc.userID, out playerChar);
                        if (playerChar != null)
                        {
                            safeZone = null;
                            warZone = null;
                            Vector2 playerPos = new Vector2(playerChar.transform.position.x, playerChar.transform.position.z);

                            bool inZoneW = false;
                            foreach (Zone zone in warZones)
                            {
                                Vector2 point1 = zone.firstPoint;
                                Vector2 point2 = zone.secondPoint;
                                Vector2 point3 = zone.thirdPoint;
                                Vector2 point4 = zone.fourthPoint;
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
                                {
                                    warZone = zone;
                                    break;
                                }
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

                            bool inZoneS = false;
                            foreach (Zone zone in safeZones)
                            {
                                Vector2 point1 = zone.firstPoint;
                                Vector2 point2 = zone.secondPoint;
                                Vector2 point3 = zone.thirdPoint;
                                Vector2 point4 = zone.fourthPoint;
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
                                {
                                    safeZone = zone;
                                    break;
                                }
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
                        }
                    }
                }
                catch (Exception ex)
                {
                    conLog.Error("CZ: " + ex.ToString());
                }
            }
        }

        //public static void clientSpeak(VoiceCom voiceCom, int setupData, byte[] data)
        //{
            //try
            //{
            //    PlayerClient client;
            //    if (((voice.distance > 0f) && PlayerClient.Find(voiceCom.networkViewOwner, out client)) && client.hasLastKnownPosition)
            //    {
            //        float num = inGlobalV.Contains(client.userID) ? (1000000000f * 1000000000f) : (voice.distance * voice.distance);
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
        //}

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

        public static Dictionary<ulong, decimal> sortDictionaryByValue(Dictionary<ulong, decimal> dictionary, bool ascending = false)
        {
            List<KeyValuePair<ulong, decimal>> tempList = new List<KeyValuePair<ulong, decimal>>(dictionary);

            tempList.Sort(delegate(KeyValuePair<ulong, decimal> firstPair, KeyValuePair<ulong, decimal> secondPair)
            {
                if (ascending)
                    return firstPair.Value.CompareTo(secondPair.Value);
                return secondPair.Value.CompareTo(firstPair.Value);
            });

            Dictionary<ulong, decimal> mySortedDictionary = new Dictionary<ulong, decimal>();
            foreach (KeyValuePair<ulong, decimal> pair in tempList)
            {
                mySortedDictionary.Add(pair.Key, pair.Value);
            }

            return mySortedDictionary;
        }

        public static Dictionary<ulong, int> sortDictionaryByValue(Dictionary<ulong, int> dictionary, bool ascending = false)
        {
            List<KeyValuePair<ulong, int>> tempList = new List<KeyValuePair<ulong, int>>(dictionary);

            tempList.Sort(delegate(KeyValuePair<ulong, int> firstPair, KeyValuePair<ulong, int> secondPair)
            {
                if (ascending)
                    return firstPair.Value.CompareTo(secondPair.Value);
                return secondPair.Value.CompareTo(firstPair.Value);
            });

            Dictionary<ulong, int> mySortedDictionary = new Dictionary<ulong, int>();
            foreach (KeyValuePair<ulong, int> pair in tempList)
            {
                mySortedDictionary.Add(pair.Key, pair.Value);
            }

            return mySortedDictionary;
        }


        public static uLink.NetworkPlayer getNetPlayer(ulong UID)
        {
            PlayerClient pc = Array.Find(AllPlayerClients.ToArray(), (PlayerClient pClient) => pClient.userID == UID);

            if (pc != null)
            {
                if (pc.netPlayer != null)
                {
                    return pc.netPlayer;
                }
            }
            return uLink.NetworkPlayer.unassigned;
        }

        public static bool getNetPlayer(ulong UID, out uLink.NetworkPlayer netPlayer)
        {
            PlayerClient pc = Array.Find(AllPlayerClients.ToArray(), (PlayerClient pClient) => pClient.userID == UID);

            if (pc != null)
            {
                if (pc.netPlayer != null)
                {
                    netPlayer = pc.netPlayer;
                    return true;
                }
            }
            netPlayer = uLink.NetworkPlayer.unassigned;
            return false;
        }

        public static PlayerClient getPlayerClient(ulong userID)
        {
            PlayerClient pc = Array.Find(AllPlayerClients.ToArray(), (PlayerClient pClient) => pClient.userID == userID);

            return pc;
        }

        public static bool getPlayerClient(ulong userID, out PlayerClient playerClient)
        {
            PlayerClient pc = Array.Find(AllPlayerClients.ToArray(), (PlayerClient pClient) => pClient.userID == userID);

            if (pc != null)
            {
                playerClient = pc;
                return true;
            }

            playerClient = null;
            return false;
        }

        public static PlayerClient getPlayerClient(uLink.NetworkPlayer netPlayer)
        {
            if (netPlayer != null)
            {
                PlayerClient pc = Array.Find(AllPlayerClients.ToArray(), (PlayerClient pClient) => pClient.netPlayer == netPlayer);

                if (pc != null)
                {
                    return pc;
                }
            }
            return null;
        }

        public static bool getPlayerClient(uLink.NetworkPlayer netPlayer, out PlayerClient playerClient)
        {
            if (netPlayer != null)
            {
                PlayerClient pc = Array.Find(AllPlayerClients.ToArray(), (PlayerClient pClient) => pClient.netPlayer == netPlayer);

                if (pc != null)
                {
                    playerClient = pc;
                    return true;
                }
            }
            playerClient = null;
            return false;
        }

        public static bool getPlayerChar(PlayerClient pc, out Character character)
        {
            if (pc != null)
            {
                if (Vars.AllCharacters.ContainsKey(pc))
                {
                    Character playerChar = Vars.AllCharacters[pc];
                    if (playerChar != null)
                    {
                        character = playerChar;
                        return true;
                    }
                }
                else
                {
                    Character playerChar;
                    Character.FindByUser(pc.userID, out playerChar);
                    if (playerChar != null)
                    {
                        character = playerChar;
                        return true;
                    }
                }
            }
            character = null;
            return false;
        }

        public static Character getPlayerChar(PlayerClient pc)
        {
            if (pc != null)
            {
                if (Vars.AllCharacters.ContainsKey(pc))
                {
                    Character playerChar = Vars.AllCharacters[pc];
                    if (playerChar != null)
                    {
                        return playerChar;
                    }
                }
                else
                {
                    Character playerChar;
                    Character.FindByUser(pc.userID, out playerChar);
                    if (playerChar != null)
                    {
                        return playerChar;
                    }
                }
            }
            return null;
        }

        public static Quaternion getRotation(PlayerClient pc)
        {
            if (pc != null)
            {
                Character playerChar;
                Character.FindByUser(pc.userID, out playerChar);
                if (playerChar != null)
                {
                    return playerChar.eyesRotation;
                }
            }
            return new Quaternion(0, 0, 0, 0);
        }

        public static bool getPosition(PlayerClient pc, out Vector3 newVector)
        {
            if (pc != null)
            {
                Character playerChar;
                Character.FindByUser(pc.userID, out playerChar);
                if (playerChar != null)
                {
                    newVector = playerChar.eyesOrigin;
                    return true;
                }
            }
            newVector = new Vector3(0,0,0);
            return false;
        }

        public static Vector3 getPosition(PlayerClient pc)
        {
            if (pc != null)
            {
                Character playerChar;
                Character.FindByUser(pc.userID, out playerChar);
                if (playerChar != null)
                {
                    return playerChar.eyesOrigin;
                }
            }
            return new Vector3(0, 0, 0);
        }

        public static void saveServer()
        {
            try
            {
                ConsoleSystem.Run("save.all", false);
                Broadcast.broadcastAll("All data has been saved.");
                conLog.Info("All data has been saved.");
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("SAVE #1: " + ex.ToString());
            }
        }

        public static void save(PlayerClient senderClient)
        {
            try
            {
                ConsoleSystem.Run("save.all", false);
                Broadcast.broadcastTo(senderClient.netPlayer, "All data has been saved.");
            } catch (Exception ex)
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "Save failed! Data has not been properly saved.");
                Vars.conLog.Error("SAVE #2: " + ex.ToString());
            }
        }

        public static void stopServer()
        {
            try
            {
                Broadcast.broadcastAll("Server shutting down...");
                Broadcast.broadcastToAllConsoles("               [color green]Server is rebooting! Type net.reconnect in 10 seconds to reconnect.");
                conLog.Info("Server shutting down...");
                ConsoleSystem.Run("save.all", false);
                Vars.REB.StartCoroutine(continueStop());
                global.Console_AllowClose();
                Application.Quit();
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("STOP: " + ex.ToString());
            }
        }

        public static IEnumerator continueStop()
        {
            yield return new WaitForSeconds(5f + (Vars.AllPlayerClients.Count / 5f));
            Process.GetCurrentProcess().Kill();
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
                if (warpsForUIDs.ContainsKey(senderClient.userID))
                {
                    foreach (string s in Vars.warpsForUIDs[senderClient.userID])
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
                Broadcast.broadcastTo(senderClient.netPlayer, string.Join(", ", warps.ToArray()));
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
                if (kitsForUIDs.ContainsKey(senderClient.userID))
                {
                    foreach (string s in Vars.kitsForUIDs[senderClient.userID])
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
                Broadcast.broadcastTo(senderClient.netPlayer, string.Join(", ", kits.ToArray()));
            }
        }

        public static void listCommands(string rank, PlayerClient senderClient)
        {
            List<string> commands1 = new List<string>();
            List<string> otherCommands = new List<string>();
            string rankToUse = rank;
            if (!Vars.enabledCommands.ContainsKey(rankToUse))
                rankToUse = Vars.defaultRank;

            if (Vars.enabledCommands.ContainsKey(rankToUse))
            {
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
                    Broadcast.broadcastTo(senderClient.netPlayer, string.Join(" ", commands1.ToArray()));
                }
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You do not have permission to use any commands.");
        }

        public static void handleWarps(PlayerClient senderClient, string[] args)
        {
            if (senderClient != null)
            {
                if (args.Count() > 1)
                {
                    string mode = args[1].ToLower();
                    switch (mode)
                    {
                        case "add":
                            List<string> warpNameList = new List<string>();
                            int curIndex = 0;
                            foreach (string s in args)
                            {
                                if (curIndex > 1)
                                {
                                    warpNameList.Add(s);
                                }
                                curIndex++;
                            }

                            string warpName = string.Join(" ", warpNameList.ToArray()).ToLower();
                            if (!warps.ContainsKey(warpName))
                            {
                                Vector3 senderPos;
                                if (getPosition(senderClient, out senderPos))
                                {
                                    if (!warps.ContainsKey(warpName))
                                        warps.Add(warpName, senderPos);

                                    if (!unassignedWarps.Contains(warpName))
                                        unassignedWarps.Add(warpName);
                                    if (inheritWarps)
                                        RustEssentialsBootstrap._load.inheritWarps();
                                    Data.addWarp(warpName, defaultRank, senderPos);
                                    Broadcast.noticeTo(senderClient.netPlayer, "➟", "Warp [" + warpName + "] established!", 4);
                                }
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "Warp [" + warpName + "] already exists!");
                            break;
                        case "uid":
                            List<string> warpNameListUID = new List<string>();
                            int curIndexUID = 0;
                            bool searchingForQuotesUID = false;
                            string UID = null;
                            foreach (string s in args)
                            {
                                if (curIndexUID > 1)
                                {
                                    if (s.StartsWith("\"") && !searchingForQuotesUID)
                                    {
                                        searchingForQuotesUID = true;
                                    }

                                    if (searchingForQuotesUID)
                                        warpNameListUID.Add(s);

                                    if (s.EndsWith("\"") && searchingForQuotesUID)
                                    {
                                        if (args.Count() - 1 >= curIndexUID + 1)
                                            UID = args[curIndexUID + 1];
                                        break;
                                    }
                                }
                                curIndexUID++;
                            }

                            string warpNameUID = string.Join(" ", warpNameListUID.ToArray()).ToLower();
                            warpNameUID = warpNameUID.Substring(1, warpNameUID.Length - 2);
                            if (UID != null)
                            {
                                if (warps.ContainsKey(warpNameUID))
                                {
                                    bool wasUnassigned = true;

                                    List<string> ranksToChange = new List<string>();

                                    foreach (KeyValuePair<string, List<string>> kv in warpsForRanks)
                                    {
                                        if (kv.Value.Contains(warpNameUID))
                                        {
                                            ranksToChange.Add(kv.Key);
                                            wasUnassigned = false;
                                        }
                                    }

                                    foreach (string s in ranksToChange)
                                    {
                                        warpsForRanks[s].Remove(warpNameUID);
                                    }

                                    List<ulong> UIDsToChange = new List<ulong>();

                                    foreach (KeyValuePair<ulong, List<string>> kv in warpsForUIDs)
                                    {
                                        if (kv.Value.Contains(warpNameUID))
                                        {
                                            UIDsToChange.Add(kv.Key);
                                            wasUnassigned = false;
                                        }
                                    }

                                    foreach (ulong s in UIDsToChange)
                                    {
                                        warpsForUIDs[s].Remove(warpNameUID);
                                    }

                                    if (unassignedWarps.Contains(warpNameUID))
                                    {
                                        unassignedWarps.Remove(warpNameUID);
                                        wasUnassigned = true;
                                    }

                                    ulong UIDLong;
                                    if (ulong.TryParse(UID, out UIDLong))
                                    {
                                        if (!warpsForUIDs.ContainsKey(UIDLong))
                                            warpsForUIDs.Add(UIDLong, new List<string>());

                                        if (!warpsForUIDs[UIDLong].Contains(warpNameUID))
                                        {
                                            warpsForUIDs[UIDLong].Add(warpNameUID);
                                            Data.editWarp(warpNameUID, UID, wasUnassigned);
                                            Broadcast.noticeTo(senderClient.netPlayer, "➟", "Warp [" + warpNameUID + "] is now owned by " + UID + "!", 4);
                                        }
                                        else
                                            Broadcast.broadcastTo(senderClient.netPlayer, "UID " + UID + " already owns the warp [" + warpNameUID + "].");
                                    }
                                    else
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Unable to parse \"" + UID + "\" as a UID!");
                                }
                                else
                                    Broadcast.broadcastTo(senderClient.netPlayer, "No such warp named [" + warpNameUID + "]!");
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "You must specify a UID!");
                            break;
                        case "rank":
                            List<string> warpNameListRank = new List<string>();
                            int curIndexRank = 0;
                            bool searchingForQuotes = false;
                            string rankPrefix = null;
                            foreach (string s in args)
                            {
                                if (curIndexRank > 1)
                                {
                                    if (s.StartsWith("\"") && !searchingForQuotes)
                                    {
                                        searchingForQuotes = true;
                                    }

                                    if (searchingForQuotes)
                                        warpNameListRank.Add(s);

                                    if (s.EndsWith("\"") && searchingForQuotes)
                                    {
                                        if (args.Count() - 1 >= curIndexRank + 1)
                                            rankPrefix = args[curIndexRank + 1];
                                        break;
                                    }
                                }
                                curIndexRank++;
                            }

                            string warpNameRank = string.Join(" ", warpNameListRank.ToArray()).ToLower();
                            warpNameRank = warpNameRank.Substring(1, warpNameRank.Length - 2);
                            if (rankPrefix != null)
                            {
                                if (warps.ContainsKey(warpNameRank))
                                {
                                    bool wasUnassigned = true;

                                    List<string> ranksToChange = new List<string>();

                                    foreach (KeyValuePair<string, List<string>> kv in warpsForRanks)
                                    {
                                        if (kv.Value.Contains(warpNameRank))
                                        {
                                            ranksToChange.Add(kv.Key);
                                            wasUnassigned = false;
                                        }
                                    }

                                    foreach (string s in ranksToChange)
                                    {
                                        warpsForRanks[s].Remove(warpNameRank);
                                    }

                                    List<ulong> UIDsToChange = new List<ulong>();

                                    foreach (KeyValuePair<ulong, List<string>> kv in warpsForUIDs)
                                    {
                                        if (kv.Value.Contains(warpNameRank))
                                        {
                                            UIDsToChange.Add(kv.Key);
                                            wasUnassigned = false;
                                        }
                                    }

                                    foreach (ulong s in UIDsToChange)
                                    {
                                        warpsForUIDs[s].Remove(warpNameRank);
                                    }

                                    if (unassignedWarps.Contains(warpNameRank))
                                    {
                                        unassignedWarps.Remove(warpNameRank);
                                        wasUnassigned = true;
                                    }
                                    if (rankPrefix.ToLower() != defaultRank.ToLower())
                                    {
                                        string rank = Array.Find(rankPrefixes.ToArray(), (KeyValuePair<string, string> kv) => kv.Value == "[" + rankPrefix + "]").Key;
                                        if (rank != null)
                                        {
                                            if (warpsForRanks.ContainsKey(rank))
                                            {
                                                if (!warpsForRanks[rank].Contains(warpNameRank))
                                                {
                                                    warpsForRanks[rank].Add(warpNameRank);
                                                    if (inheritWarps)
                                                        RustEssentialsBootstrap._load.inheritWarps();
                                                    Data.editWarp(warpNameRank, rankPrefix, wasUnassigned);
                                                    Broadcast.noticeTo(senderClient.netPlayer, "➟", "Warp [" + warpNameRank + "] is now owned by the \"" + rank + "\" rank!", 4);
                                                }
                                                else
                                                    Broadcast.broadcastTo(senderClient.netPlayer, "Rank \"" + rank + "\" already owns the warp [" + warpNameRank + "].");
                                            }
                                            else
                                                Broadcast.broadcastTo(senderClient.netPlayer, "No such rank with prefix \"" + rankPrefix + "\".");
                                        }
                                        else
                                            Broadcast.broadcastTo(senderClient.netPlayer, "No such rank with prefix \"" + rankPrefix + "\".");
                                    }
                                    else
                                    {
                                        unassignedWarps.Add(warpNameRank);
                                        if (inheritWarps)
                                            RustEssentialsBootstrap._load.inheritWarps();
                                        Data.editWarp(warpNameRank, rankPrefix, wasUnassigned);
                                        Broadcast.noticeTo(senderClient.netPlayer, "➟", "Warp [" + warpNameRank + "] is now owned by the \"" + defaultRank + "\" rank!", 4);
                                    }
                                }
                                else
                                    Broadcast.broadcastTo(senderClient.netPlayer, "No such warp named [" + warpNameRank + "]!");
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "You must specify a rank prefix!");
                            break;
                        case "rem":
                            List<string> warpNameListRem = new List<string>();
                            int curIndexRem = 0;
                            foreach (string s in args)
                            {
                                if (curIndexRem > 1)
                                {
                                    warpNameListRem.Add(s);
                                }
                                curIndexRem++;
                            }

                            string warpNameRem = string.Join(" ", warpNameListRem.ToArray()).ToLower();
                            if (warps.ContainsKey(warpNameRem))
                            {
                                warps.Remove(warpNameRem);

                                if (unassignedWarps.Contains(warpNameRem))
                                    unassignedWarps.Remove(warpNameRem);

                                List<string> ranksToChange = new List<string>();

                                foreach (KeyValuePair<string, List<string>> kv in warpsForRanks)
                                {
                                    if (kv.Value.Contains(warpNameRem))
                                        ranksToChange.Add(kv.Key);
                                }

                                foreach (string s in ranksToChange)
                                {
                                    warpsForRanks[s].Remove(warpNameRem);
                                }

                                List<ulong> UIDsToChange = new List<ulong>();

                                foreach (KeyValuePair<ulong, List<string>> kv in warpsForUIDs)
                                {
                                    if (kv.Value.Contains(warpNameRem))
                                        UIDsToChange.Add(kv.Key);
                                }

                                foreach (ulong s in UIDsToChange)
                                {
                                    warpsForUIDs[s].Remove(warpNameRem);
                                }
                                Data.remWarp(warpNameRem);
                                Broadcast.noticeTo(senderClient.netPlayer, "➟", "Warp [" + warpNameRem + "] removed!", 4);
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "No such warp named [" + warpNameRem + "]!");
                            break;
                    }
                }
            }
        }

        public static void amplifyFPS(PlayerClient senderClient)
        {
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "gfx.bloom false");
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "gfx.grain false");
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "gfx.ssao false");
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "gfx.shafts false");
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "gfx.tonemap false");
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "grass.on false");
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "grass.forceredraw false");
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "grass.displacement false");
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "grass.shadowcast false");
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "grass.shadowreceive false");
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "render.level 0");
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "render.vsync false");
            Broadcast.broadcastTo(senderClient.netPlayer, "Your graphics have been adjusted. FPS boost may occur.");
        }

        public static void amplifyQuality(PlayerClient senderClient)
        {
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "gfx.bloom true");
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "gfx.grain true");
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "gfx.ssao true");
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "gfx.shafts true");
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "gfx.tonemap true");
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "grass.on true");
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "grass.forceredraw true");
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "grass.displacement true");
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "grass.shadowcast true");
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "grass.shadowreceive true");
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "render.level 1");
            Broadcast.broadcastCommandTo(senderClient.netPlayer, "render.vsync false");
            Broadcast.broadcastTo(senderClient.netPlayer, "Your graphics have been adjusted. Quality boost may occur.");
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
                string message = string.Join(" ", messageList.ToArray());

                if (Vars.historyGlobal.Count > 50)
                    Vars.historyGlobal.RemoveAt(0);
                Vars.historyGlobal.Add("* " + (Vars.removeTag ? "" : "<G> ") + botName + "$:|:$" + message);
                Broadcast.broadcastAll(message);
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
            List<PlayerClient> playerClients = new List<PlayerClient>();
            foreach (PlayerClient pc in Vars.AllPlayerClients) { playerClients.Add(pc); }

            foreach (PlayerClient targetClient in playerClients)
            {
                kickPlayer(targetClient.netUser, "All users were kicked.", false);
            }
        }

        public static void kickAll(PlayerClient senderClient)
        {
            List<PlayerClient> playerClients = new List<PlayerClient>();
            foreach (PlayerClient pc in Vars.AllPlayerClients) { playerClients.Add(pc); }

            foreach (PlayerClient targetClient in playerClients)
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
                            Broadcast.broadcastTo(kv.Key.netPlayer, (senderClient.userName == "" ? senderClient.netUser.displayName : senderClient.userName) + " denied your teleport request.");
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
                                possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == "" && pc.netUser.displayName.Equals(playerName));
                            if (possibleTargets.Count() == 0)
                                Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal \"" + playerName + "\".");
                            else if (possibleTargets.Count() > 1)
                                Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + playerName + "\".");
                            else
                            {
                                PlayerClient targetClient = possibleTargets[0];

                                if (teleportRequests[senderClient].ContainsKey(targetClient))
                                {
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Denied " + (targetClient.userName == "" ? targetClient.netUser.displayName : targetClient.userName) + "'s teleport request.");
                                    Broadcast.broadcastTo(targetClient.netPlayer, (senderClient.userName == "" ? senderClient.netUser.displayName : senderClient.userName) + " denied your teleport request.");
                                    teleportRequests[senderClient][targetClient].dispose();
                                    teleportRequests[senderClient].Remove(targetClient);
                                }
                                else
                                {
                                    Broadcast.broadcastTo(senderClient.netPlayer, "You do not have a teleport request from " + (targetClient.userName == "" ? targetClient.netUser.displayName : targetClient.userName) + ".");
                                }
                            }
                        }
                        else
                        {
                            PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                            if (possibleTargets.Count() == 0)
                                possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == "" && pc.netUser.displayName.Contains(playerName));
                            if (possibleTargets.Count() == 0)
                                Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                            else if (possibleTargets.Count() > 1)
                                Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                            else
                            {
                                PlayerClient targetClient = possibleTargets[0];

                                if (teleportRequests[senderClient].ContainsKey(targetClient))
                                {
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Denied " + (targetClient.userName == "" ? targetClient.netUser.displayName : targetClient.userName) + "'s teleport request.");
                                    Broadcast.broadcastTo(targetClient.netPlayer, (senderClient.userName == "" ? senderClient.netUser.displayName : senderClient.userName) + " denied your teleport request.");
                                    teleportRequests[senderClient][targetClient].dispose();
                                    teleportRequests[senderClient].Remove(targetClient);
                                }
                                else
                                {
                                    Broadcast.broadcastTo(senderClient.netPlayer, "You do not have a teleport request from " + (targetClient.userName == "" ? targetClient.netUser.displayName : targetClient.userName) + ".");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("TPDEN: " + ex.ToString());
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
                bool isVanished = false;

                if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                {
                    playerName = playerName.Substring(1, playerName.Length - 2);

                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(playerName));
                    if (possibleTargets.Count() == 0)
                    {
                        possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == "" && pc.netUser.displayName.Equals(playerName));
                        isVanished = true;
                    }
                    if (possibleTargets.Count() == 0)
                        Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal \"" + playerName + "\".");
                    else if (possibleTargets.Count() > 1)
                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + playerName + "\".");
                    else
                    {
                        PlayerClient targetClient = possibleTargets[0];

                        if (!Checks.isAlive(targetClient))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport " + (isVanished ? targetClient.netUser.displayName : targetClient.userName) + " to you because he/she are dead.");
                            return;
                        }
                        if (!Checks.isAlive(senderClient))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport " + (isVanished ? targetClient.netUser.displayName : targetClient.userName) + " to you because you are dead.");
                            return;
                        }
                        if (teleportRequests[senderClient].ContainsKey(targetClient))
                        {
                            if (!isTeleporting.Contains(targetClient))
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, (requestDelay == 0 ? "Teleporting..." : "Teleporting " + (isVanished ? targetClient.netUser.displayName : targetClient.userName) + " in " + requestDelay + " seconds..."));
                                Broadcast.broadcastTo(targetClient.netPlayer, (requestDelay == 0 ? "Teleporting..." : "Teleporting to " + (senderClient.userName == "" ? senderClient.netUser.displayName : senderClient.userName) + " in " + requestDelay + " seconds. Do not move..."));
                                REB.StartCoroutine(teleporting(targetClient, senderClient));
                                isTeleporting.Add(targetClient);
                                isAccepting.Add(senderClient);
                                if (requestCooldownType == 1 && !blockedRequestsPer[targetClient.userID].ContainsKey(senderClient.userID))
                                {
                                    TimerPlus t1 = TimerPlus.Create(requestCooldown, false, unblockRequests, senderClient.userID, targetClient.userID);

                                    blockedRequestsPer[targetClient.userID].Add(senderClient.userID, t1);
                                }
                                if (requestCooldownType == 2 && !blockedRequestsAll.ContainsKey(targetClient.userID))
                                {
                                    TimerPlus t1 = TimerPlus.Create(requestCooldown, false, unblockRequests, senderClient.userID, targetClient.userID);

                                    blockedRequestsAll.Add(targetClient.userID, t1);
                                }
                            }
                            else
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, (isVanished ? targetClient.netUser.displayName : targetClient.userName) + " is already teleporting from another request or a warp.");
                            }
                        }
                        else
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You do not have a teleport request from " + (isVanished ? targetClient.netUser.displayName : targetClient.userName) + ".");
                        }
                    }
                }
                else
                {
                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                    if (possibleTargets.Count() == 0)
                    {
                        possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == "" && pc.netUser.displayName.Contains(playerName));
                        isVanished = true;
                    }
                    if (possibleTargets.Count() == 0)
                        Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                    else if (possibleTargets.Count() > 1)
                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                    else
                    {
                        PlayerClient targetClient = possibleTargets[0];

                        if (!Checks.isAlive(targetClient))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport " + (isVanished ? targetClient.netUser.displayName : targetClient.userName) + " to you because he/she are dead.");
                            return;
                        }
                        if (!Checks.isAlive(senderClient))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport " + (isVanished ? targetClient.netUser.displayName : targetClient.userName) + " to you because you are dead.");
                            return;
                        }

                        if (teleportRequests[senderClient].ContainsKey(targetClient))
                        {
                            if (!isTeleporting.Contains(targetClient))
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, (requestDelay == 0 ? "Teleporting..." : "Teleporting " + (isVanished ? targetClient.netUser.displayName : targetClient.userName) + " in " + requestDelay + " seconds..."));
                                Broadcast.broadcastTo(targetClient.netPlayer, (requestDelay == 0 ? "Teleporting..." : "Teleporting to " + (senderClient.userName == "" ? senderClient.netUser.displayName : senderClient.userName) + " in " + requestDelay + " seconds. Do not move..."));
                                REB.StartCoroutine(teleporting(targetClient, senderClient));
                                isTeleporting.Add(targetClient);
                                isAccepting.Add(senderClient);

                                if (requestCooldownType == 1 && !blockedRequestsPer[targetClient.userID].ContainsKey(senderClient.userID))
                                {
                                    TimerPlus t1 = TimerPlus.Create(requestCooldown, false, unblockRequests, senderClient.userID, targetClient.userID);

                                    blockedRequestsPer[targetClient.userID].Add(senderClient.userID, t1);
                                }
                                if (requestCooldownType == 2 && !blockedRequestsAll.ContainsKey(targetClient.userID))
                                {
                                    TimerPlus t1 = TimerPlus.Create(requestCooldown, false, unblockRequests, senderClient.userID, targetClient.userID);

                                    blockedRequestsAll.Add(targetClient.userID, t1);
                                }
                            }
                            else
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, (isVanished ? targetClient.netUser.displayName : targetClient.userName) + " is already teleporting from another request or a warp.");
                            }
                        }
                        else
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You do not have a teleport request from " + (isVanished ? targetClient.netUser.displayName : targetClient.userName) + ".");
                        }
                    }
                }
            }
            else
            {
                if (latestRequests[senderClient] != null)
                {
                    PlayerClient targetClient = latestRequests[senderClient];

                    if (!Checks.isAlive(targetClient))
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport " + (targetClient.userName == "" ? targetClient.netUser.displayName : targetClient.userName) + " to you because he/she are dead.");
                        return;
                    }
                    if (!Checks.isAlive(senderClient))
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport " + (targetClient.userName == "" ? targetClient.netUser.displayName : targetClient.userName) + " to you because you are dead.");
                        return;
                    }
                    if (!isTeleporting.Contains(targetClient))
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, (requestDelay == 0 ? "Teleporting..." : "Teleporting " + (targetClient.userName == "" ? targetClient.netUser.displayName : targetClient.userName) + " in " + requestDelay + " seconds..."));
                        Broadcast.broadcastTo(targetClient.netPlayer, (requestDelay == 0 ? "Teleporting..." : "Teleporting to " + (senderClient.userName == "" ? senderClient.netUser.displayName : senderClient.userName) + " in " + requestDelay + " seconds. Do not move..."));
                        REB.StartCoroutine(teleporting(targetClient, senderClient));
                        isTeleporting.Add(targetClient);
                        if (requestCooldownType == 1 && !blockedRequestsPer[targetClient.userID].ContainsKey(senderClient.userID))
                        {
                            TimerPlus t1 = TimerPlus.Create(requestCooldown, false, unblockRequests, senderClient.userID, targetClient.userID);

                            blockedRequestsPer[targetClient.userID].Add(senderClient.userID, t1);
                        }
                        if (requestCooldownType == 2 && !blockedRequestsAll.ContainsKey(targetClient.userID))
                        {
                            TimerPlus t1 = TimerPlus.Create(requestCooldown, false, unblockRequests, senderClient.userID, targetClient.userID);
                            
                            blockedRequestsAll.Add(targetClient.userID, t1);
                        }
                    }
                    else
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, (targetClient.userName == "" ? targetClient.netUser.displayName : targetClient.userName) + " is already teleporting from another request or a warp.");
                    }
                }
                else
                    Broadcast.broadcastTo(senderClient.netPlayer, "You have no teleport requests to accept!");
            }
        }

        public static IEnumerator warping(PlayerClient senderClient, Vector3 destination)
        {
            if (Vars.wasHit.Contains(senderClient.userID))
                Vars.wasHit.Remove(senderClient.userID);
            int timeElapsed = 0;
            RustServerManagement serverManagement = RustServerManagement.Get();

            Vector3 oldPos = getPosition(senderClient);
            while (true)
            {
                if (wasHit.Contains(senderClient.userID))
                {
                    isTeleporting.Remove(senderClient);
                    wasHit.Remove(senderClient.userID);
                    Broadcast.broadcastTo(senderClient.netPlayer, "Warp was interrupted due to damage.");
                    break;
                }
                if (timeElapsed >= warpDelay)
                {
                    simulateTeleport(senderClient, destination);
                    isTeleporting.Remove(senderClient);
                    break;
                }
                Vector3 newPos = getPosition(senderClient);
                if (Vector3.Distance(oldPos, newPos) > 3)
                {
                    Broadcast.broadcastTo(senderClient.netPlayer, "You moved. Warp cancelled.");
                    isTeleporting.Remove(senderClient);
                    break;
                }
                yield return new WaitForSeconds(1);
                timeElapsed++;
            }
        }

        public static void teleportToWorldElapsed(uLink.NetworkPlayer np, Vector3 v3)
        {
            PlayerClient playerClient = Array.Find(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == np);
            if (playerClient != null)
            {
                Character playerChar;
                if (getPlayerChar(playerClient, out playerChar))
                {
                    if (playerChar != null)
                    {
                        if (playerChar.netUser.userID == playerClient.userID)
                        {
                            if (!Checks.aboveGround(playerClient))
                            {
                                simulateTeleport(playerClient, v3);
                            }
                        }
                    }
                }
            }
        }

        public static IEnumerator homeTeleporting(PlayerClient senderClient, Vector3 home, bool isFactionHome = false)
        {
            if (Vars.wasHit.Contains(senderClient.userID))
                Vars.wasHit.Remove(senderClient.userID);
            Vector3 oldPos = getPosition(senderClient);
            int timeElapsed = 0;
            RustServerManagement serverManagement = RustServerManagement.Get();
            while (true)
            {
                if (wasHit.Contains(senderClient.userID))
                {
                    isTeleporting.Remove(senderClient);
                    wasHit.Remove(senderClient.userID);
                    Broadcast.broadcastTo(senderClient.netPlayer, (isFactionHome ? "Faction home " : "Home ") + "teleportation was interrupted due to damage.");
                    break;
                }
                if (timeElapsed >= requestDelay)
                {
                    simulateTeleport(senderClient, home);
                    isTeleporting.Remove(senderClient);
                    break;
                }
                Vector3 newPos = getPosition(senderClient);
                if (Vector3.Distance(oldPos, newPos) > 3)
                {
                    Broadcast.broadcastTo(senderClient.netPlayer, "You moved. Teleportation cancelled.");
                    isTeleporting.Remove(senderClient);
                    break;
                }
                yield return new WaitForSeconds(1);
                timeElapsed++;
            }
        }

        public static IEnumerator teleporting(PlayerClient targetClient, PlayerClient senderClient)
        {
            if (Vars.wasHit.Contains(senderClient.userID))
                Vars.wasHit.Remove(senderClient.userID);
            if (Vars.wasHit.Contains(targetClient.userID))
                Vars.wasHit.Remove(targetClient.userID);
            Vector3 oldPos = getPosition(targetClient);
            int timeElapsed = 0;
            RustServerManagement serverManagement = RustServerManagement.Get();
            while (true)
            {
                if (wasHit.Contains(targetClient.userID))
                {
                    teleportRequests[senderClient][targetClient].dispose();
                    teleportRequests[senderClient].Remove(targetClient);
                    latestRequests[senderClient] = null;
                    isTeleporting.Remove(targetClient);
                    isAccepting.Remove(senderClient);
                    wasHit.Remove(targetClient.userID);
                    Broadcast.broadcastTo(senderClient.netPlayer, "Teleport request from " + (targetClient.userName == "" ? targetClient.netUser.displayName : targetClient.userName) + " was interrupted because they took damage.");
                    Broadcast.broadcastTo(targetClient.netPlayer, "Teleport request to " + (senderClient.userName == "" ? senderClient.netUser.displayName : senderClient.userName) + " was interrupted due to damage.");
                    break;
                }
                if (wasHit.Contains(senderClient.userID))
                {
                    teleportRequests[senderClient][targetClient].dispose();
                    teleportRequests[senderClient].Remove(targetClient);
                    latestRequests[senderClient] = null;
                    isTeleporting.Remove(targetClient);
                    isAccepting.Remove(senderClient);
                    wasHit.Remove(senderClient.userID);
                    Broadcast.broadcastTo(senderClient.netPlayer, "Teleport request from " + (targetClient.userName == "" ? targetClient.netUser.displayName : targetClient.userName) + " was interrupted due to damage.");
                    Broadcast.broadcastTo(targetClient.netPlayer, "Teleport request to " + (senderClient.userName == "" ? senderClient.netUser.displayName : senderClient.userName) + " was interrupted because they took damage.");
                    break;
                }
                if (timeElapsed >= requestDelay)
                {
                    Vector3 destinationPos = getPosition(senderClient);
                    simulateTeleport(targetClient, destinationPos);
                    teleportRequests[senderClient][targetClient].dispose();
                    teleportRequests[senderClient].Remove(targetClient);
                    latestRequests[senderClient] = null;
                    isTeleporting.Remove(targetClient);
                    isAccepting.Remove(senderClient);
                    break;
                }
                Vector3 newPos = getPosition(targetClient);
                if (Vector3.Distance(oldPos, newPos) > 3)
                {
                    Broadcast.broadcastTo(senderClient.netPlayer, (targetClient.userName == "" ? targetClient.netUser.displayName : targetClient.userName) + " moved. Teleportation cancelled.");
                    Broadcast.broadcastTo(targetClient.netPlayer, "You moved. Teleportation cancelled.");
                    teleportRequests[senderClient][targetClient].dispose();
                    teleportRequests[senderClient].Remove(targetClient);
                    latestRequests[senderClient] = null;
                    isTeleporting.Remove(targetClient);
                    isAccepting.Remove(senderClient);
                    break;
                }
                yield return new WaitForSeconds(1);
                timeElapsed++;
            }
        }

        public static void teleportRequest(PlayerClient senderClient, string[] args)
        {
            if (teleportRequestOn)
            {
                if (!isTeleporting.Contains(senderClient))
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
                                    possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == "" && pc.netUser.displayName.Equals(playerName));
                                if (possibleTargets.Count() == 0)
                                    Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal \"" + playerName + "\".");
                                else if (possibleTargets.Count() > 1)
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + playerName + "\".");
                                else
                                {
                                    PlayerClient targetClient = possibleTargets[0];
                                    if (requestCooldownType == 1 && blockedRequestsPer[senderClient.userID].ContainsKey(targetClient.userID))
                                    {
                                        double timeLeft = Math.Round((blockedRequestsPer[senderClient.userID][targetClient.userID].timeLeft / 1000));
                                        if (timeLeft > 0)
                                        {
                                            TimeSpan timeSpan = TimeSpan.FromMilliseconds(Vars.blockedFHomes[senderClient.userID].timeLeft);
                                            string timeString = timeSpan.Minutes + " minutes, and " + timeSpan.Seconds + " seconds.";
                                            Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport to that player for " + timeString);
                                            return;
                                        }
                                    }
                                    if (requestCooldownType == 2 && blockedRequestsAll.ContainsKey(senderClient.userID))
                                    {
                                        double timeLeft = Math.Round((blockedRequestsAll[senderClient.userID].timeLeft / 1000));
                                        if (timeLeft > 0)
                                        {
                                            TimeSpan timeSpan = TimeSpan.FromMilliseconds(Vars.blockedFHomes[senderClient.userID].timeLeft);
                                            string timeString = timeSpan.Minutes + " minutes, and " + timeSpan.Seconds + " seconds.";
                                            Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport to anyone for " + timeString);
                                            return;
                                        }
                                    }
                                    if (!Checks.isAlive(targetClient))
                                    {
                                        Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport to " + (targetClient.userName == "" ? targetClient.netUser.displayName : targetClient.userName) + " because they are dead.");
                                        return;
                                    }
                                    if (!Checks.isAlive(senderClient))
                                    {
                                        Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport to " + (targetClient.userName == "" ? targetClient.netUser.displayName : targetClient.userName) + " because you are dead.");
                                        return;
                                    }
                                    if (!teleportRequests[targetClient].ContainsKey(senderClient) && !isTeleporting.Contains(senderClient))
                                    {
                                        if (!enableInHouse && Checks.inHouse(senderClient))
                                        {
                                            Broadcast.broadcastTo(senderClient.netPlayer, "You cannot send a teleport request while in a house.");
                                        }
                                        else
                                        {
                                            TimerPlus t = TimerPlus.Create(30000, false, requestTimeout, targetClient, senderClient);

                                            teleportRequests[targetClient].Add(senderClient, t);
                                            latestRequests[targetClient] = senderClient;
                                            Broadcast.broadcastTo(senderClient.netPlayer, "Teleport request sent to " + (targetClient.userName == "" ? targetClient.netUser.displayName : targetClient.userName) + ".");
                                            Broadcast.broadcastTo(targetClient.netPlayer, (senderClient.userName == "" ? senderClient.netUser.displayName : senderClient.userName) + " requested to teleport to you. Type /tpaccept, /tpaccept <name>, or /tpdeny <name>.");
                                        }
                                    }
                                    else
                                    {
                                        Broadcast.broadcastTo(senderClient.netPlayer, "You already sent a teleport request to " + (targetClient.userName == "" ? targetClient.netUser.displayName : targetClient.userName) + ".");
                                    }
                                }
                            }
                            else
                            {
                                PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                                if (possibleTargets.Count() == 0)
                                    possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == "" && pc.netUser.displayName.Contains(playerName));
                                if (possibleTargets.Count() == 0)
                                    Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                                else if (possibleTargets.Count() > 1)
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                                else
                                {
                                    PlayerClient targetClient = possibleTargets[0];
                                    if (requestCooldownType == 1 && blockedRequestsPer[senderClient.userID].ContainsKey(targetClient.userID))
                                    {
                                        double timeLeft = Math.Round((blockedRequestsPer[senderClient.userID][targetClient.userID].timeLeft / 1000));
                                        if (timeLeft > 0)
                                        {
                                            TimeSpan timeSpan = TimeSpan.FromMilliseconds(Vars.blockedFHomes[senderClient.userID].timeLeft);
                                            string timeString = timeSpan.Minutes + " minutes, and " + timeSpan.Seconds + " seconds.";
                                            Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport to that player for " + timeString);
                                            return;
                                        }
                                    }
                                    if (requestCooldownType == 2 && blockedRequestsAll.ContainsKey(senderClient.userID))
                                    {
                                        double timeLeft = Math.Round((blockedRequestsAll[senderClient.userID].timeLeft / 1000));
                                        if (timeLeft > 0)
                                        {
                                            TimeSpan timeSpan = TimeSpan.FromMilliseconds(Vars.blockedFHomes[senderClient.userID].timeLeft);
                                            string timeString = timeSpan.Minutes + " minutes, and " + timeSpan.Seconds + " seconds.";
                                            Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport to anyone for " + timeString);
                                            return;
                                        }
                                    }
                                    if (!Checks.isAlive(targetClient))
                                    {
                                        Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport to " + (targetClient.userName == "" ? targetClient.netUser.displayName : targetClient.userName) + " because they are dead.");
                                        return;
                                    }
                                    if (!Checks.isAlive(senderClient))
                                    {
                                        Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport to " + (targetClient.userName == "" ? targetClient.netUser.displayName : targetClient.userName) + " because you are dead.");
                                        return;
                                    }
                                    if (!teleportRequests[targetClient].ContainsKey(senderClient) && !isTeleporting.Contains(senderClient))
                                    {
                                        if (!enableInHouse && Checks.inHouse(senderClient))
                                        {
                                            Broadcast.broadcastTo(senderClient.netPlayer, "You cannot send a teleport request while in a house.");
                                        }
                                        else
                                        {
                                            TimerPlus t = TimerPlus.Create(30000, false, requestTimeout, targetClient, senderClient);

                                            teleportRequests[targetClient].Add(senderClient, t);
                                            latestRequests[targetClient] = senderClient;

                                            Broadcast.broadcastTo(senderClient.netPlayer, "Teleport request sent to " + (targetClient.userName == "" ? targetClient.netUser.displayName : targetClient.userName) + ".");
                                            Broadcast.broadcastTo(targetClient.netPlayer, (senderClient.userName == "" ? senderClient.netUser.displayName : senderClient.userName) + " requested to teleport to you. Type /tpaccept, /tpaccept <name>, or /tpdeny <name>.");
                                        }
                                    }
                                    else
                                    {
                                        Broadcast.broadcastTo(senderClient.netPlayer, "You already sent a teleport request to " + (targetClient.userName == "" ? targetClient.netUser.displayName : targetClient.userName) + ".");
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
                    Broadcast.broadcastTo(senderClient.netPlayer, "You are already teleporting from another request or a warp.");
                }
            }
            else
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "Teleport requesting is disabled on this server.");
            }
        }

        public static void unblockRequests(params object[] args)
        {
            if (args.Count() > 0)
            {
                string toRemove = (string)args[0];
                string removeFrom = (string)args[1];
                ulong toRemoveUID;
                ulong removeUIDFrom;

                if (ulong.TryParse(toRemove, out toRemoveUID) && ulong.TryParse(removeFrom, out removeUIDFrom))
                {
                    if (blockedRequestsAll.ContainsKey(removeUIDFrom))
                    {
                        blockedRequestsAll.Remove(removeUIDFrom);
                    }
                    if (blockedRequestsPer[removeUIDFrom].ContainsKey(toRemoveUID))
                    {
                        blockedRequestsPer[removeUIDFrom].Remove(toRemoveUID);
                    }
                }
            }
        }

        public static void unblockFactionHomeTP(params object[] args)
        {
            if (args.Count() > 0)
            {
                ulong toRemoveUID = (ulong)args[0];

                if (blockedFHomes.ContainsKey(toRemoveUID))
                {
                    blockedFHomes[toRemoveUID].dispose();
                    blockedFHomes.Remove(toRemoveUID);
                }
            }
        }

        public static void unblockHomeTP(params object[] args)
        {
            if (args.Count() > 0)
            {
                ulong toRemoveUID = (ulong)args[0];

                if (blockedHomes.ContainsKey(toRemoveUID))
                {
                    blockedHomes[toRemoveUID].dispose();
                    blockedHomes.Remove(toRemoveUID);
                }
            }
        }

        public static void requestTimeout(params object[] args)
        {
            if (args.Count() > 0)
            {
                PlayerClient targetClient = (PlayerClient)args[0];
                PlayerClient senderClient = (PlayerClient)args[1];
                if (teleportRequests.ContainsKey(targetClient))
                {
                    if (teleportRequests[targetClient].ContainsKey(senderClient) && !isTeleporting.Contains(senderClient))
                    {
                        teleportRequests[targetClient].Remove(senderClient);
                        Broadcast.broadcastTo(senderClient.netPlayer, "Request to teleport to " + (targetClient.userName == "" ? targetClient.netUser.displayName : targetClient.userName) + " has timed out.");
                    }
                }
                else
                {
                    Broadcast.broadcastTo(senderClient.netPlayer, "Request to teleport to " + (targetClient.userName == "" ? targetClient.netUser.displayName : targetClient.userName) + " has timed out.");
                }
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
                    xpos = float.Parse(args[1].Replace("(", "").Replace(",", "").Replace(")", ""));
                    ypos = float.Parse(args[2].Replace("(", "").Replace(",", "").Replace(")", ""));
                    zpos = float.Parse(args[3].Replace("(", "").Replace(",", "").Replace(")", ""));
                }
                catch (Exception ex)
                {
                    Broadcast.broadcastTo(senderClient.netPlayer, "X, Y, and Z must be numerical!");
                }

                if (xpos != -1 && ypos != -1 && zpos != -1)
                {
                    simulateTeleport(senderClient, new Vector3(xpos, ypos, zpos));
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
                            possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == "" && pc.netUser.displayName.Equals(playerName));
                        if (possibleTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal \"" + playerName + "\".");
                        else if (possibleTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + playerName + "\".");
                        else
                        {
                            PlayerClient targetClient = possibleTargets[0];
                            Vector3 destination = getPosition(senderClient);

                            simulateTeleport(targetClient, destination);

                            Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting " + (targetClient.userName == "" ? targetClient.netUser.displayName : targetClient.userName) + " here...");
                        }
                    }
                    else
                    {
                        PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                        if (possibleTargets.Count() == 0)
                            possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == "" && pc.netUser.displayName.Contains(playerName));
                        if (possibleTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                        else if (possibleTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                        else
                        {
                            PlayerClient targetClient = possibleTargets[0];
                            Vector3 destination = getPosition(senderClient);

                            simulateTeleport(targetClient, destination);

                            Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting " + (targetClient.userName == "" ? targetClient.netUser.displayName : targetClient.userName) + " here...");
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
                try
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
                            if (warpsForRanks.ContainsKey(findRank(senderClient.userID)))
                            {
                                if (warpsForRanks[findRank(senderClient.userID)].Contains(warpNameToLower))
                                {
                                    if (!enableInHouse && Checks.inHouse(senderClient))
                                    {
                                        Broadcast.broadcastTo(senderClient.netPlayer, "You cannot warp while in a house.");
                                    }
                                    else
                                    {
                                        bool onCooldown = false;
                                        if (Vars.activeWarpCooldowns.ContainsKey(senderClient.userID))
                                        {
                                            if (Vars.activeWarpCooldowns[senderClient.userID].ContainsValue(warpNameToLower))
                                            {
                                                foreach (KeyValuePair<TimerPlus, string> kv in Vars.activeWarpCooldowns[senderClient.userID])
                                                {
                                                    if (kv.Value == warpNameToLower)
                                                    {
                                                        if (Math.Round((kv.Key.timeLeft / 1000)) > 0)
                                                            onCooldown = true;
                                                    }
                                                }
                                            }
                                        }

                                        if (!onCooldown)
                                        {
                                            Broadcast.broadcastTo(senderClient.netPlayer, (warpDelay == 0 ? "Warping..." : "Warping in " + warpDelay + " seconds..."));
                                            REB.StartCoroutine(warping(senderClient, warps[warpNameToLower]));
                                            isTeleporting.Add(senderClient);

                                            if (Vars.warpCooldowns.ContainsKey(warpNameToLower)) // If a cooldown is set for this kit, set my cool down
                                            {
                                                TimerPlus tp = TimerPlus.Create(Vars.warpCooldowns[warpNameToLower], false, Vars.restoreWarp, warpNameToLower, senderClient.userID);

                                                if (!Vars.activeWarpCooldowns.ContainsKey(senderClient.userID))
                                                    Vars.activeWarpCooldowns.Add(senderClient.userID, new Dictionary<TimerPlus, string>() { { tp, warpNameToLower } });
                                                else
                                                    Vars.activeWarpCooldowns[senderClient.userID].Add(tp, warpNameToLower);
                                            }
                                        }
                                        else
                                        {
                                            if (Vars.activeWarpCooldowns.ContainsKey(senderClient.userID))
                                            {
                                                foreach (KeyValuePair<TimerPlus, string> kv in Vars.activeWarpCooldowns[senderClient.userID])
                                                {
                                                    if (kv.Value == warpNameToLower)
                                                    {
                                                        double timeLeft = Math.Round((kv.Key.timeLeft / 1000));
                                                        TimeSpan timeSpan = TimeSpan.FromMilliseconds(kv.Key.timeLeft);

                                                        string timeString = "";

                                                        timeString += timeSpan.Hours + " hours, ";
                                                        timeString += timeSpan.Minutes + " minutes, and ";
                                                        timeString += timeSpan.Seconds + " seconds";
                                                        Broadcast.noticeTo(senderClient.netPlayer, "⌛", "You must wait " + (timeLeft > 999999999 ? "forever" : timeString) + " before using this.", 4);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (warpsForUIDs.ContainsKey(senderClient.userID))
                                    {
                                        if (warpsForUIDs[senderClient.userID].Contains(warpNameToLower))
                                        {
                                            if (!enableInHouse && Checks.inHouse(senderClient))
                                            {
                                                Broadcast.broadcastTo(senderClient.netPlayer, "You cannot warp while in a house.");
                                            }
                                            else
                                            {
                                                bool onCooldown = false;
                                                if (Vars.activeWarpCooldowns.ContainsKey(senderClient.userID))
                                                {
                                                    if (Vars.activeWarpCooldowns[senderClient.userID].ContainsValue(warpNameToLower))
                                                    {
                                                        foreach (KeyValuePair<TimerPlus, string> kv in Vars.activeWarpCooldowns[senderClient.userID])
                                                        {
                                                            if (kv.Value == warpNameToLower)
                                                            {
                                                                if (Math.Round((kv.Key.timeLeft / 1000)) > 0)
                                                                    onCooldown = true;
                                                            }
                                                        }
                                                    }
                                                }

                                                if (!onCooldown)
                                                {
                                                    Broadcast.broadcastTo(senderClient.netPlayer, (warpDelay == 0 ? "Warping..." : "Warping in " + warpDelay + " seconds..."));
                                                    REB.StartCoroutine(warping(senderClient, warps[warpNameToLower]));
                                                    isTeleporting.Add(senderClient);

                                                    if (Vars.warpCooldowns.ContainsKey(warpNameToLower)) // If a cooldown is set for this kit, set my cool down
                                                    {
                                                        if (Vars.kitCooldowns[warpNameToLower] > -1)
                                                        {
                                                            TimerPlus tp = TimerPlus.Create(Vars.warpCooldowns[warpNameToLower], false, Vars.restoreWarp, warpNameToLower, senderClient.userID);

                                                            if (!Vars.activeWarpCooldowns.ContainsKey(senderClient.userID))
                                                                Vars.activeWarpCooldowns.Add(senderClient.userID, new Dictionary<TimerPlus, string>() { { tp, warpNameToLower } });
                                                            else
                                                                Vars.activeWarpCooldowns[senderClient.userID].Add(tp, warpNameToLower);
                                                        }
                                                        else
                                                        {
                                                            if (!Vars.activeWarpCooldowns.ContainsKey(senderClient.userID))
                                                                Vars.activeWarpCooldowns.Add(senderClient.userID, new Dictionary<TimerPlus, string>() { { null, warpNameToLower } });
                                                            else
                                                                Vars.activeWarpCooldowns[senderClient.userID].Add(null, warpNameToLower);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (Vars.activeWarpCooldowns.ContainsKey(senderClient.userID))
                                                    {
                                                        foreach (KeyValuePair<TimerPlus, string> kv in Vars.activeWarpCooldowns[senderClient.userID])
                                                        {
                                                            if (kv.Value == warpNameToLower)
                                                            {
                                                                double timeLeft = Math.Round((kv.Key.timeLeft / 1000));
                                                                TimeSpan timeSpan = TimeSpan.FromMilliseconds(kv.Key.timeLeft);

                                                                string timeString = "";

                                                                timeString += timeSpan.Hours + " hours, ";
                                                                timeString += timeSpan.Minutes + " minutes, and ";
                                                                timeString += timeSpan.Seconds + " seconds";
                                                                Broadcast.noticeTo(senderClient.netPlayer, "⌛", "You must wait " + (timeLeft > 999999999 ? "forever" : timeString) + " before using this.", 4);
                                                            }
                                                        }
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
                            else
                            {
                                if (unassignedWarps.Contains(warpNameToLower))
                                {
                                    if (!enableInHouse && Checks.inHouse(senderClient))
                                    {
                                        Broadcast.broadcastTo(senderClient.netPlayer, "You cannot warp while in a house.");
                                    }
                                    else
                                    {
                                        bool onCooldown = false;
                                        if (Vars.activeWarpCooldowns.ContainsKey(senderClient.userID))
                                        {
                                            if (Vars.activeWarpCooldowns[senderClient.userID].ContainsValue(warpNameToLower))
                                            {
                                                foreach (KeyValuePair<TimerPlus, string> kv in Vars.activeWarpCooldowns[senderClient.userID])
                                                {
                                                    if (kv.Value == warpNameToLower)
                                                    {
                                                        if (Math.Round((kv.Key.timeLeft / 1000)) > 0)
                                                            onCooldown = true;
                                                    }
                                                }
                                            }
                                        }

                                        if (!onCooldown)
                                        {
                                            Broadcast.broadcastTo(senderClient.netPlayer, (warpDelay == 0 ? "Warping..." : "Warping in " + warpDelay + " seconds..."));
                                            REB.StartCoroutine(warping(senderClient, warps[warpNameToLower]));
                                            isTeleporting.Add(senderClient);

                                            if (Vars.warpCooldowns.ContainsKey(warpNameToLower)) // If a cooldown is set for this kit, set my cool down
                                            {
                                                TimerPlus tp = TimerPlus.Create(Vars.warpCooldowns[warpNameToLower], false, Vars.restoreWarp, warpNameToLower, senderClient.userID);

                                                if (!Vars.activeWarpCooldowns.ContainsKey(senderClient.userID))
                                                    Vars.activeWarpCooldowns.Add(senderClient.userID, new Dictionary<TimerPlus, string>() { { tp, warpNameToLower } });
                                                else
                                                    Vars.activeWarpCooldowns[senderClient.userID].Add(tp, warpNameToLower);
                                            }
                                        }
                                        else
                                        {
                                            if (Vars.activeWarpCooldowns.ContainsKey(senderClient.userID))
                                            {
                                                foreach (KeyValuePair<TimerPlus, string> kv in Vars.activeWarpCooldowns[senderClient.userID])
                                                {
                                                    if (kv.Value == warpNameToLower)
                                                    {
                                                        double timeLeft = Math.Round((kv.Key.timeLeft / 1000));
                                                        TimeSpan timeSpan = TimeSpan.FromMilliseconds(kv.Key.timeLeft);

                                                        string timeString = "";

                                                        timeString += timeSpan.Hours + " hours, ";
                                                        timeString += timeSpan.Minutes + " minutes, and ";
                                                        timeString += timeSpan.Seconds + " seconds";
                                                        Broadcast.noticeTo(senderClient.netPlayer, "⌛", "You must wait " + (timeLeft > 999999999 ? "forever" : timeString) + " before using this.", 4);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (warpsForUIDs.ContainsKey(senderClient.userID))
                                    {
                                        if (warpsForUIDs[senderClient.userID].Contains(warpNameToLower))
                                        {
                                            if (!enableInHouse && Checks.inHouse(senderClient))
                                            {
                                                Broadcast.broadcastTo(senderClient.netPlayer, "You cannot warp while in a house.");
                                            }
                                            else
                                            {
                                                bool onCooldown = false;
                                                if (Vars.activeWarpCooldowns.ContainsKey(senderClient.userID))
                                                {
                                                    if (Vars.activeWarpCooldowns[senderClient.userID].ContainsValue(warpNameToLower))
                                                    {
                                                        foreach (KeyValuePair<TimerPlus, string> kv in Vars.activeWarpCooldowns[senderClient.userID])
                                                        {
                                                            if (kv.Value == warpNameToLower)
                                                            {
                                                                if (Math.Round((kv.Key.timeLeft / 1000)) > 0)
                                                                    onCooldown = true;
                                                            }
                                                        }
                                                    }
                                                }

                                                if (!onCooldown)
                                                {
                                                    Broadcast.broadcastTo(senderClient.netPlayer, (warpDelay == 0 ? "Warping..." : "Warping in " + warpDelay + " seconds..."));
                                                    REB.StartCoroutine(warping(senderClient, warps[warpNameToLower]));
                                                    isTeleporting.Add(senderClient);

                                                    if (Vars.warpCooldowns.ContainsKey(warpNameToLower)) // If a cooldown is set for this kit, set my cool down
                                                    {
                                                        TimerPlus tp = TimerPlus.Create(Vars.warpCooldowns[warpNameToLower], false, Vars.restoreWarp, warpNameToLower, senderClient.userID);

                                                        if (!Vars.activeWarpCooldowns.ContainsKey(senderClient.userID))
                                                            Vars.activeWarpCooldowns.Add(senderClient.userID, new Dictionary<TimerPlus, string>() { { tp, warpNameToLower } });
                                                        else
                                                            Vars.activeWarpCooldowns[senderClient.userID].Add(tp, warpNameToLower);
                                                    }
                                                }
                                                else
                                                {
                                                    if (Vars.activeWarpCooldowns.ContainsKey(senderClient.userID))
                                                    {
                                                        foreach (KeyValuePair<TimerPlus, string> kv in Vars.activeWarpCooldowns[senderClient.userID])
                                                        {
                                                            if (kv.Value == warpNameToLower)
                                                            {
                                                                double timeLeft = Math.Round((kv.Key.timeLeft / 1000));
                                                                TimeSpan timeSpan = TimeSpan.FromMilliseconds(kv.Key.timeLeft);

                                                                string timeString = "";

                                                                timeString += timeSpan.Hours + " hours, ";
                                                                timeString += timeSpan.Minutes + " minutes, and ";
                                                                timeString += timeSpan.Seconds + " seconds";
                                                                Broadcast.noticeTo(senderClient.netPlayer, "⌛", "You must wait " + (timeLeft > 999999999 ? "forever" : timeString) + " before using this.", 4);
                                                            }
                                                        }
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
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "No such warp named \"" + warpName + "\".");
                    }
                }
                catch (Exception ex)
                {
                    conLog.Error("WP: " + ex.ToString());
                }
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You cannot warp while in a war zone!");
        }

        public static void teleportGhost(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                RustServerManagement serverManagement = RustServerManagement.Get();
                List<string> playerNameList = new List<string>();
                List<string> playerNames = new List<string>();
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

                        if (!searchingQuotes)
                        {
                            playerNames.Add(string.Join(" ", playerNameList.ToArray()));
                            playerNameList.Clear();
                        }

                        if (searchingQuotes && s.EndsWith("\""))
                        {
                            quotedNames++;
                            playerNames.Add(string.Join(" ", playerNameList.ToArray()));
                            playerNameList.Clear();
                            searchingQuotes = false;
                        }
                    }

                    curIndex++;
                }
                string playerName = playerNames[0];

                bool isVanished = false;
                if (playerNames.Count == 1)
                {
                    if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                    {
                        playerName = playerName.Substring(1, playerName.Length - 2);

                        PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(playerName));
                        if (possibleTargets.Count() == 0)
                        {
                            possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == "" && pc.netUser.displayName.Equals(playerName));
                            isVanished = true;
                        }
                        if (possibleTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal \"" + playerName + "\".");
                        else if (possibleTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + playerName + "\".");
                        else
                        {
                            PlayerClient targetClient = possibleTargets[0];
                            Vector3 destination = getPosition(targetClient);

                            simulateTeleport(senderClient, destination);

                            if (Vars.ghostList.ContainsKey(targetClient.userID))
                                Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting to " + (isVanished ? targetClient.netUser.displayName : targetClient.userName) + "'s ghost...");
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting to " + (isVanished ? targetClient.netUser.displayName : targetClient.userName) + "'s body...");
                        }
                    }
                    else
                    {
                        PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                        if (possibleTargets.Count() == 0)
                        {
                            possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == "" && pc.netUser.displayName.Contains(playerName));
                            isVanished = true;
                        }
                        if (possibleTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                        else if (possibleTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                        else
                        {
                            PlayerClient targetClient = possibleTargets[0];
                            Vector3 destination = getPosition(targetClient);

                            simulateTeleport(senderClient, destination);

                            if (Vars.ghostList.ContainsKey(targetClient.userID))
                                Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting to " + (isVanished ? targetClient.netUser.displayName : targetClient.userName) + "'s ghost...");
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting to " + (isVanished ? targetClient.netUser.displayName : targetClient.userName) + "'s body...");
                        }
                    }
                }
                else
                {
                    if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                    {
                        playerName = playerName.Substring(1, playerName.Length - 2);
                    }

                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                    if (possibleTargets.Count() == 0)
                    {
                        possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == "" && pc.netUser.displayName.Contains(playerName));
                        isVanished = true;
                    }
                    if (possibleTargets.Count() == 0)
                        Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                    else if (possibleTargets.Count() > 1)
                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                    else
                    {
                        PlayerClient targetClient = possibleTargets[0];

                        playerName = playerNames[1];
                        if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                        {
                            playerName = playerName.Substring(1, playerName.Length - 2);
                        }

                        PlayerClient[] otherTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                        bool isVanished2 = false;
                        if (otherTargets.Count() == 0)
                        {
                            otherTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == "" && pc.netUser.displayName.Contains(playerName));
                            isVanished2 = true;
                        }
                        if (otherTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                        else if (otherTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                        else
                        {
                            PlayerClient targetClient2 = otherTargets[0];
                            Vector3 destination = getPosition(targetClient2);

                            simulateTeleport(targetClient, destination);

                            Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting \"" + (isVanished ? targetClient.netUser.displayName : targetClient.userName) + "\" to \"" + (isVanished2 ? targetClient2.netUser.displayName : targetClient2.userName) + "\"...");
                        }
                    }
                }
            }
        }

        public static void teleportAbove(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                RustServerManagement serverManagement = RustServerManagement.Get();
                List<string> playerNameList = new List<string>();
                List<string> playerNames = new List<string>();
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

                        if (!searchingQuotes)
                        {
                            playerNames.Add(string.Join(" ", playerNameList.ToArray()));
                            playerNameList.Clear();
                        }

                        if (searchingQuotes && s.EndsWith("\""))
                        {
                            quotedNames++;
                            playerNames.Add(string.Join(" ", playerNameList.ToArray()));
                            playerNameList.Clear();
                            searchingQuotes = false;
                        }
                    }

                    curIndex++;
                }
                string playerName = playerNames[0];

                bool isVanished = false;
                if (playerNames.Count == 1)
                {
                    if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                    {
                        playerName = playerName.Substring(1, playerName.Length - 2);

                        PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(playerName));
                        if (possibleTargets.Count() == 0)
                        {
                            possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == "" && pc.netUser.displayName.Equals(playerName));
                            isVanished = true;
                        }
                        if (possibleTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal \"" + playerName + "\".");
                        else if (possibleTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + playerName + "\".");
                        else
                        {
                            PlayerClient targetClient = possibleTargets[0];
                            Vector3 destination = getPosition(targetClient);
                            destination.y += 100;

                            simulateTeleport(senderClient, destination);

                            Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting to " + (isVanished ? targetClient.netUser.displayName : targetClient.userName) + "...");
                        }
                    }
                    else
                    {
                        PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                        if (possibleTargets.Count() == 0)
                        {
                            possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == "" && pc.netUser.displayName.Contains(playerName));
                            isVanished = true;
                        }
                        if (possibleTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                        else if (possibleTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                        else
                        {
                            PlayerClient targetClient = possibleTargets[0];
                            Vector3 destination = getPosition(targetClient);
                            destination.y += 100;

                            simulateTeleport(senderClient, destination);

                            Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting to " + (isVanished ? targetClient.netUser.displayName : targetClient.userName) + "...");
                        }
                    }
                }
                else
                {
                    if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                    {
                        playerName = playerName.Substring(1, playerName.Length - 2);
                    }

                    bool isVanished2 = false;
                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                    if (possibleTargets.Count() == 0)
                    {
                        possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == "" && pc.netUser.displayName.Contains(playerName));
                        isVanished2 = true;
                    }
                    if (possibleTargets.Count() == 0)
                        Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                    else if (possibleTargets.Count() > 1)
                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                    else
                    {
                        PlayerClient targetClient = possibleTargets[0];

                        playerName = playerNames[1];
                        if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                        {
                            playerName = playerName.Substring(1, playerName.Length - 2);
                        }

                        PlayerClient[] otherTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                        if (otherTargets.Count() == 0)
                            otherTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == "" && pc.netUser.displayName.Contains(playerName));
                        if (otherTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                        else if (otherTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                        else
                        {
                            PlayerClient targetClient2 = otherTargets[0];
                            Vector3 destination = getPosition(targetClient2);
                            destination.y += 100;

                            simulateTeleport(targetClient, destination);

                            Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting \"" + (isVanished ? targetClient.netUser.displayName : targetClient.userName) + "\" to \"" + (isVanished2 ? targetClient2.netUser.displayName : targetClient2.userName) + "\"...");
                        }
                    }
                }
            }
        }

        public static void teleport(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                RustServerManagement serverManagement = RustServerManagement.Get();
                List<string> playerNameList = new List<string>();
                List<string> playerNames = new List<string>();
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

                        if (!searchingQuotes)
                        {
                            playerNames.Add(string.Join(" ", playerNameList.ToArray()));
                            playerNameList.Clear();
                        }
                        
                        if (searchingQuotes && s.EndsWith("\""))
                        {
                            quotedNames++;
                            playerNames.Add(string.Join(" ", playerNameList.ToArray()));
                            playerNameList.Clear();
                            searchingQuotes = false;
                        }
                    }

                    curIndex++;
                }
                string playerName = playerNames[0];

                bool isVanished = false;
                if (playerNames.Count == 1)
                {
                    if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                    {
                        playerName = playerName.Substring(1, playerName.Length - 2);

                        PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Equals(playerName));
                        if (possibleTargets.Count() == 0)
                        {
                            possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == "" && pc.netUser.displayName.Equals(playerName));
                            isVanished = true;
                        }
                        if (possibleTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal \"" + playerName + "\".");
                        else if (possibleTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names equal \"" + playerName + "\".");
                        else
                        {
                            PlayerClient targetClient = possibleTargets[0];
                            Vector3 destination = getPosition(targetClient);

                            simulateTeleport(senderClient, destination);

                            Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting to " + (isVanished ? targetClient.netUser.displayName : targetClient.userName) + "...");
                        }
                    }
                    else
                    {
                        PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                        if (possibleTargets.Count() == 0)
                        {
                            possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == "" && pc.netUser.displayName.Contains(playerName));
                            isVanished = true;
                        }
                        if (possibleTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                        else if (possibleTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                        else
                        {
                            PlayerClient targetClient = possibleTargets[0];
                            Vector3 destination = getPosition(targetClient);

                            simulateTeleport(senderClient, destination);

                            Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting to " + (isVanished ? targetClient.netUser.displayName : targetClient.userName) + "...");
                        }
                    }
                }
                else
                {
                    if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                    {
                        playerName = playerName.Substring(1, playerName.Length - 2);
                    }

                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                    if (possibleTargets.Count() == 0)
                    {
                        possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == "" && pc.netUser.displayName.Contains(playerName));
                        isVanished = true;
                    }
                    if (possibleTargets.Count() == 0)
                        Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                    else if (possibleTargets.Count() > 1)
                        Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                    else
                    {
                        PlayerClient targetClient = possibleTargets[0];

                        playerName = playerNames[1];
                        if (playerName.StartsWith("\"") && playerName.EndsWith("\""))
                        {
                            playerName = playerName.Substring(1, playerName.Length - 2);
                        }

                        bool isVanished2 = false;
                        PlayerClient[] otherTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                        if (otherTargets.Count() == 0)
                        {
                            otherTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == "" && pc.netUser.displayName.Contains(playerName));
                            isVanished2 = true;
                        }
                        if (otherTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                        else if (otherTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                        else
                        {
                            PlayerClient targetClient2 = otherTargets[0];
                            Vector3 destination = getPosition(targetClient2);

                            simulateTeleport(targetClient, destination);

                            Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting \"" + (isVanished ? targetClient.netUser.displayName : targetClient.userName) + "\" to \"" + (isVanished2 ? targetClient2.netUser.displayName : targetClient2.userName) + "\"...");
                        }
                    }
                }
            }
        }

        public static void mute(PlayerClient senderClient, string[] args, bool mode)
        {
            if (args.Count() > 1)
            {
                long time;
                if (mode && long.TryParse(args.Last().Remove(args.Last().Length - 1), out time) && (args.Last().EndsWith("s") || args.Last().EndsWith("m") || args.Last().EndsWith("h")))
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
                            if (!mutedUsers.Contains(targetClient.userID))
                            {
                                if (enableMuteMessageToAll)
                                    Broadcast.broadcastAll("Player \"" + targetClient.userName + "\" has been muted on global chat for " + timeString + timeMode);
                                else
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Player \"" + targetClient.userName + "\" has been muted on global chat for " + timeString + timeMode);
                                mutedUsers.Add(targetClient.userID);
                                TimerPlus tp = TimerPlus.Create(time, false, unmuteElapsed, targetClient.userID);

                                muteTimes.Add(targetClient.userID, tp);
                            }
                            else
                            {
                                TimeSpan timeSpan = TimeSpan.FromMilliseconds(Vars.muteTimes[senderClient.userID].timeLeft);

                                timeString = timeSpan.Minutes + " minutes, and " + timeSpan.Seconds + " seconds.";

                                Broadcast.broadcastTo(senderClient.netPlayer, targetClient.userName + " is already muted for " + timeString);
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
                            if (!mutedUsers.Contains(targetClient.userID))
                            {
                                if (enableMuteMessageToAll)
                                    Broadcast.broadcastAll("Player \"" + targetClient.userName + "\" has been muted on global chat for " + timeString + timeMode);
                                else
                                    Broadcast.broadcastTo(senderClient.netPlayer, "Player \"" + targetClient.userName + "\" has been muted on global chat for " + timeString + timeMode);
                                mutedUsers.Add(targetClient.userID);
                                TimerPlus tp = TimerPlus.Create(time, false, unmuteElapsed, targetClient.userID);

                                muteTimes.Add(targetClient.userID, tp);
                            }
                            else
                            {
                                TimeSpan timeSpan = TimeSpan.FromMilliseconds(Vars.muteTimes[senderClient.userID].timeLeft);

                                timeString = timeSpan.Minutes + " minutes, and " + timeSpan.Seconds + " seconds.";

                                Broadcast.broadcastTo(senderClient.netPlayer, targetClient.userName + " is already muted for " + timeString);
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
                                if (!mutedUsers.Contains(targetClient.userID))
                                {
                                    if (enableMuteMessageToAll)
                                        Broadcast.broadcastAll("Player \"" + targetClient.userName + "\" has been muted on global chat.");
                                    else
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Player \"" + targetClient.userName + "\" has been muted on global chat.");
                                    mutedUsers.Add(targetClient.userID);
                                }
                                else
                                {
                                    Broadcast.broadcastTo(senderClient.netPlayer, targetClient.userName + " is already muted.");
                                }
                            }
                            else
                            {
                                if (mutedUsers.Contains(targetClient.userID))
                                {
                                    if (enableMuteMessageToAll)
                                        Broadcast.broadcastAll("Player \"" + targetClient.userName + "\" has been unmuted on global chat.");
                                    else
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Player \"" + targetClient.userName + "\" has been unmuted on global chat.");
                                    mutedUsers.Remove(targetClient.userID);
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
                                if (!mutedUsers.Contains(targetClient.userID))
                                {
                                    if (enableMuteMessageToAll)
                                        Broadcast.broadcastAll("Player \"" + targetClient.userName + "\" has been muted on global chat.");
                                    else
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Player \"" + targetClient.userName + "\" has been muted on global chat.");
                                    mutedUsers.Add(targetClient.userID);
                                }
                                else
                                {
                                    Broadcast.broadcastTo(senderClient.netPlayer, targetClient.userName + " is already muted.");
                                }
                            }
                            else
                            {
                                if (mutedUsers.Contains(targetClient.userID))
                                {
                                    if (enableMuteMessageToAll)
                                        Broadcast.broadcastAll("Player \"" + targetClient.userName + "\" has been unmuted on global chat.");
                                    else
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Player \"" + targetClient.userName + "\" has been unmuted on global chat.");
                                    mutedUsers.Remove(targetClient.userID);
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

        static void unmuteElapsed(params object[] args)
        {
            if (args.Count() > 0)
            {
                string UIDString = (string)args[0];
                ulong UID;
                if (ulong.TryParse(UIDString, out UID))
                {
                    mutedUsers.Remove(UID);
                    muteTimes.Remove(UID);
                }
            }
        }

        public static void whitelistCheck(PlayerClient senderClient)
        {
            ulong UID = senderClient.userID;
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
                string UIDString = "";
                ulong UID;

                if (args.Count() > 2)
                    UIDString = args[2];

                if (!ulong.TryParse(UIDString, out UID))
                {
                    Broadcast.broadcastTo(sender, "You must enter a proper 17 digit Steam UID.");
                    return;
                }

                PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID == UID);
                string targetName = "Unknown Player";
                if (possibleTargets.Count() > 0)
                    targetName = possibleTargets[0].userName;

                switch (action)
                {
                    case "add":
                        whitelist.Add(UID);
                        if (!rankList.ContainsKey("Member"))
                            rankList.Add("Member", new List<ulong>());

                        rankList["Member"] = whitelist;
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
                                    foreach (ulong s in whitelist)
                                    {
                                        sw.WriteLine(s + " # " + targetName);
                                    }
                                }
                            }
                        }
                        break;
                    case "rem":
                        whitelist.Remove(UID);
                        if (!rankList.ContainsKey("Member"))
                            rankList.Add("Member", new List<ulong>());

                        rankList["Member"] = whitelist;
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
                                    foreach (ulong s in whitelist)
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
                                PlayerClient[] targetUsers = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => !groupMembers.Contains(pc.userID));
                                foreach (PlayerClient targetClient in targetUsers)
                                {
                                    whitelistKick(targetClient.netUser, whitelistKickCMD);
                                }
                            }
                            else
                            {
                                PlayerClient[] targetUsers = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => !whitelist.Contains(pc.userID));
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
                ulong UID = senderClient.userID;
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
                ulong UID = senderClient.userID;
                Faction faction = Vars.factions.GetByMember(UID);

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
                        if (faction != null)
                        {
                            if (!inFaction.Contains(UID))
                            {
                                inFaction.Add(UID);
                                if (inGlobal.Contains(UID))
                                    inGlobal.Remove(UID);
                                if (inDirect.Contains(UID))
                                    inDirect.Remove(UID);
                                Broadcast.broadcastTo(senderClient.netPlayer, "You are now talking in the faction chat of [" + faction.name + "].");
                            }
                            else
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, "You are already talking in the faction chat of [" + faction.name + "].");
                            }
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not in a faction.");
                        break;
                    case "faction":
                        if (faction != null)
                        {
                            if (!inFaction.Contains(UID))
                            {
                                inFaction.Add(UID);
                                if (inGlobal.Contains(UID))
                                    inGlobal.Remove(UID);
                                if (inDirect.Contains(UID))
                                    inDirect.Remove(UID);
                                Broadcast.broadcastTo(senderClient.netPlayer, "You are now talking in the faction chat of " + faction.name + ".");
                            }
                            else
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, "You are already talking in the faction chat of " + faction.name + ".");
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

        public static string filterFullNames(string playerName, ulong uid)
        {
            if (!emptyPrefixes.Contains(uid))
            {
                foreach (KeyValuePair<string, List<ulong>> kv in rankList)
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
                    rank = "[" + findRank(possibleClients[0].userID) + "] ";

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
                    rank = "[" + findRank(possibleClients[0].userID) + "] ";

                string joinMessage = Vars.joinMessage.Replace("$USER$", rank + fakeName);
                Broadcast.broadcastAll(joinMessage);
            }
            else
            {
                string rank = "[" + findRank(senderClient.userID) + "] ";

                string joinMessage = Vars.joinMessage.Replace("$USER$", (!removePrefix ? rank : "") + senderClient.userName);
                Broadcast.broadcastAll(joinMessage);
            }
        }

        public static IEnumerator sendToFaction(PlayerClient senderClient, string playerName, List<string> messages, string consoleMessage)
        {
            Faction faction = Vars.factions.GetByMember(senderClient.userID);
            List<uLink.NetworkPlayer> targets = new List<uLink.NetworkPlayer>();
            foreach (var m in faction.members)
            {
                PlayerClient playerClient;
                if (getPlayerClient(m.userID, out playerClient))
                    targets.Add(playerClient.netPlayer);
            }

            if (Vars.sendChatToConsoles)
                Broadcast.broadcastToConsoles(targets, "[color #FFA154]<F [" + faction.name + "]> [color #66CCFF]" + playerName + ": [color white]" + consoleMessage);
            foreach (string s in messages)
            {
                Broadcast.broadcastCustomTo(targets, "<F> " + playerName, s.Trim());
            }

            yield return null;
        }

        public static IEnumerator sendToFaction(PlayerClient senderClient, string playerName, string message, string consoleMessage)
        {
            Faction faction = Vars.factions.GetByMember(senderClient.userID);
            List<uLink.NetworkPlayer> targets = new List<uLink.NetworkPlayer>();
            foreach (var m in faction.members)
            {
                PlayerClient playerClient;
                if (getPlayerClient(m.userID, out playerClient))
                    targets.Add(playerClient.netPlayer);
            }

            if (Vars.sendChatToConsoles)
                Broadcast.broadcastToConsoles(targets, "[color #FFA154]<F [" + faction.name + "]> [color #66CCFF]" + playerName + ": [color white]" + consoleMessage);
            Broadcast.broadcastCustomTo(targets, "<F> " + playerName, message);

            yield return null;
        }

        public static IEnumerator sendToSurrounding(PlayerClient senderClient, string playerName, List<string> messages, string consoleMessage)
        {
            Character senderChar;
            Character.FindByUser(senderClient.userID, out senderChar);

            Vector3 senderPos = senderChar.eyesOrigin;

            KeyValuePair<PlayerClient, Character>[] targetChars = Array.FindAll(AllCharacters.ToArray(), (KeyValuePair<PlayerClient, Character> kv) => kv.Value.alive && Vector3.Distance(senderPos, kv.Value.eyesOrigin) < directDistance);
            List<uLink.NetworkPlayer> targets = new List<uLink.NetworkPlayer>();
            foreach (var target in targetChars)
            {
                var targetChar = target.Value;
                if (targetChar.playerClient != null && targetChar.playerClient.netPlayer != null)
                {
                    targets.Add(targetChar.playerClient.netPlayer);
                }
            }

            if (Vars.sendChatToConsoles)
                Broadcast.broadcastToConsoles(targets, "[color #FFA154]<D> [color #66CCFF]" + playerName + ": [color white]" + consoleMessage);
            foreach (string s in messages)
            {
                Broadcast.broadcastCustomTo(targets, playerName, s.Trim());
            }

            yield return null;
        }

        public static IEnumerator sendToSurrounding(PlayerClient senderClient, string playerName, string message, string consoleMessage)
        {
            Character senderChar;
            Character.FindByUser(senderClient.userID, out senderChar);

            Vector3 senderPos = senderChar.eyesOrigin;

            KeyValuePair<PlayerClient, Character>[] targetChars = Array.FindAll(AllCharacters.ToArray(), (KeyValuePair<PlayerClient, Character> kv) => kv.Value.alive && Vector3.Distance(senderPos, kv.Value.eyesOrigin) < directDistance);
            List<uLink.NetworkPlayer> targets = new List<uLink.NetworkPlayer>();
            foreach (var target in targetChars)
            {
                var targetChar = target.Value;
                if (targetChar.playerClient != null && targetChar.playerClient.netPlayer != null)
                {
                    targets.Add(targetChar.playerClient.netPlayer);
                }
            }

            if (Vars.sendChatToConsoles)
                Broadcast.broadcastToConsoles(targets, "[color #FFA154]<D> [color #66CCFF]" + playerName + ": [color white]" + consoleMessage);
            Broadcast.broadcastCustomTo(targets, playerName, message);

            yield return null;
        }

        public static void startTimer()
        {
            TimerPlus.Create(refreshInterval, true, grabGroupMembers);
        }

        public static bool grabNameByUID(ulong UID, out string playerName)
        {
            PlayerClient[] possibleClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID == UID);

            if (possibleClients.Count() > 0) // If the player is online, grab his name through the game
            {
                playerName = possibleClients[0].userName;
                return true;
            }
            else // If he is not, grab his name through steam
            {
                try
                {
                    string profileURL = "http://steamcommunity.com/profiles/" + UID + "/?xml=1\\";
                    WebRequest request = WebRequest.Create(profileURL);
                    request.Proxy = null;
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
                                    playerName = reader.Value;
                                    return true;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Vars.conLog.logToFile("GNBU #2: " + ex.Message, "error");
                }
                playerName = "";
                return false;
            }
        }

        public static string grabNameByUID(ulong UID)
        {
            PlayerClient[] possibleClients = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID == UID);

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
                    request.Proxy = null;
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
                    Vars.conLog.logToFile("GNBU: " + ex.Message, "error");
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
                                ulong UID;
                                if (ulong.TryParse(reader.Value, out UID))
                                    groupMembers.Add(UID);
                            }
                        }
                    }
                    if (Vars.useAsMembers)
                        rankList["Member"] = groupMembers;
                }
            }
            catch (Exception ex)
            {
                //Vars.conLog.Error(ex.ToString());
                Vars.conLog.logToFile("GGM: " + ex.Message, "error");
            }
        }

        public static void sendNudity()
        {
            try
            {
                List<PlayerClient> playerClients = new List<PlayerClient>();
                foreach (PlayerClient pc in Vars.AllPlayerClients) { playerClients.Add(pc); }

                if (!onlyOnJoin)
                {
                    foreach (PlayerClient playerClient in playerClients)
                    {
                        try
                        {
                            if (forceNudity && !onlyOnJoin && playerClient != null)
                            {
                                if (playerClient.netPlayer != null)
                                    Broadcast.broadcastCommandTo(playerClient.netPlayer, "censor.nudity false");
                            }
                        }
                        catch (Exception ex)
                        {
                            conLog.Error("SN #2: " + ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                conLog.Error("SN: " + ex.ToString());
            }
        }

        public static void unfreezeAll(PlayerClient senderClient)
        {
            if (senderClient != null)
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "All frozen players have been unfrozen!");
                frozenPlayers.Clear();
            }
        }

        public static void unfreezePlayer(PlayerClient senderClient, string[] args)
        {
            if (senderClient != null)
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

                            if (frozenPlayers.ContainsKey(targetClient.userID))
                            {
                                frozenPlayers.Remove(targetClient.userID);
                                Broadcast.broadcastTo(senderClient.netPlayer, "Player " + targetClient.userName + " has been unfrozen!");
                                Broadcast.noticeTo(targetClient.netPlayer, "⛄", "You were unfrozen!", 3);
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "Player " + targetClient.userName + " is not frozen.");
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

                            if (frozenPlayers.ContainsKey(targetClient.userID))
                            {
                                frozenPlayers.Remove(targetClient.userID);
                                Broadcast.broadcastTo(senderClient.netPlayer, "Player " + targetClient.userName + " has been unfrozen!");
                                Broadcast.noticeTo(targetClient.netPlayer, "⛄", "You were unfrozen!", 3);
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "Player " + targetClient.userName + " is not frozen.");
                        }
                    }
                }
                else
                {
                    if (frozenPlayers.ContainsKey(senderClient.userID))
                    {
                        frozenPlayers.Remove(senderClient.userID);
                        Broadcast.noticeTo(senderClient.netPlayer, "⛄", "You were unfrozen!", 3);
                    }
                    else
                        Broadcast.broadcastTo(senderClient.netPlayer, "You are not frozen.");
                }
            }
        }

        public static void seeInventory(PlayerClient senderClient, string[] args)
        {
            if (senderClient != null)
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

                            try
                            {
                                if (targetClient != senderClient)
                                {
                                    List<Items.Item> targetInventory;
                                    List<Items.Item> targetArmor;
                                    List<Items.Item> oldInventory = new List<Items.Item>();
                                    List<Items.Item> oldArmor = new List<Items.Item>();
                                    Items.getInventory(targetClient, out targetInventory, out targetArmor);
                                    if (targetInventory != null && targetArmor != null)
                                    {
                                        Items.giveInventory(senderClient, targetInventory, targetArmor, out oldInventory, out oldArmor);
                                    }

                                    if (!oldPlayerArmor.ContainsKey(senderClient.userID))
                                    {
                                        oldPlayerArmor.Add(senderClient.userID, new List<Items.Item>());
                                        oldPlayerArmor[senderClient.userID] = oldArmor;
                                    }
                                    if (!oldPlayerInventory.ContainsKey(senderClient.userID))
                                    {
                                        oldPlayerInventory.Add(senderClient.userID, new List<Items.Item>());
                                        oldPlayerInventory[senderClient.userID] = oldInventory;
                                    }

                                    Broadcast.broadcastTo(senderClient.netPlayer, "To stop viewing " + targetClient.userName + "'s inventory, simply type /invsee.");
                                    Broadcast.noticeTo(senderClient.netPlayer, "☯", "You now have " + targetClient.userName + "'s inventory!", 4);
                                }
                                else
                                    Broadcast.broadcastTo(senderClient.netPlayer, "You cannot mirror your own inventory.");
                            }
                            catch (Exception ex)
                            {
                                conLog.Error("SeeI: " + ex.ToString());
                                Broadcast.broadcastTo(senderClient.netPlayer, "Something went wrong when attempting to view " + targetClient.userName + "'s inventory!");
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

                            try
                            {
                                if (targetClient != senderClient)
                                {
                                    List<Items.Item> targetInventory;
                                    List<Items.Item> targetArmor;
                                    List<Items.Item> oldInventory = new List<Items.Item>();
                                    List<Items.Item> oldArmor = new List<Items.Item>();
                                    Items.getInventory(targetClient, out targetInventory, out targetArmor);
                                    if (targetInventory != null && targetArmor != null)
                                    {
                                        Items.giveInventory(senderClient, targetInventory, targetArmor, out oldInventory, out oldArmor);
                                    }

                                    if (!oldPlayerArmor.ContainsKey(senderClient.userID))
                                    {
                                        oldPlayerArmor.Add(senderClient.userID, new List<Items.Item>());
                                        oldPlayerArmor[senderClient.userID] = oldArmor;
                                    }
                                    if (!oldPlayerInventory.ContainsKey(senderClient.userID))
                                    {
                                        oldPlayerInventory.Add(senderClient.userID, new List<Items.Item>());
                                        oldPlayerInventory[senderClient.userID] = oldInventory;
                                    }

                                    Broadcast.broadcastTo(senderClient.netPlayer, "To stop viewing " + targetClient.userName + "'s inventory, simply type /invsee.");
                                    Broadcast.noticeTo(senderClient.netPlayer, "☯", "You now have " + targetClient.userName + "'s inventory!", 4);
                                }
                                else
                                    Broadcast.broadcastTo(senderClient.netPlayer, "You cannot mirror your own inventory.");
                            }
                            catch (Exception ex)
                            {
                                conLog.Error("SeeI: " + ex.ToString());
                                Broadcast.broadcastTo(senderClient.netPlayer, "Something went wrong when attempting to view " + targetClient.userName + "'s inventory!");
                            }
                        }
                    }
                }
                else
                {
                    if (oldPlayerArmor.ContainsKey(senderClient.userID) && oldPlayerInventory.ContainsKey(senderClient.userID))
                    {
                        Items.giveInventory(senderClient, oldPlayerInventory[senderClient.userID], oldPlayerArmor[senderClient.userID]);
                        oldPlayerArmor.Remove(senderClient.userID);
                        oldPlayerInventory.Remove(senderClient.userID);
                        Broadcast.broadcastTo(senderClient.netPlayer, "Your inventory has been restored.");
                    }
                    else
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "You are not currently viewing anyone elses inventory!");
                    }
                }
            }
        }

        public static void freezePlayer(PlayerClient senderClient, string[] args)
        {
            if (senderClient != null)
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

                            if (!frozenPlayers.ContainsKey(targetClient.userID))
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, "Player " + targetClient.userName + " has been frozen! Make sure to unfreeze them!");
                                Broadcast.noticeTo(targetClient.netPlayer, "⛄", "You were frozen!", 3);
                                Vector3 playerPos = getPosition(targetClient);
                                frozenPlayers.Add(targetClient.userID, playerPos);
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "Player " + targetClient.userName + " is already frozen.");
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

                            if (!frozenPlayers.ContainsKey(targetClient.userID))
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, "Player " + targetClient.userName + " has been frozen! Make sure to unfreeze them!");
                                Broadcast.noticeTo(targetClient.netPlayer, "⛄", "You were frozen!", 3);
                                Vector3 playerPos = getPosition(targetClient);
                                frozenPlayers.Add(targetClient.userID, playerPos);
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "Player " + targetClient.userName + " is already frozen.");
                        }
                    }
                }
                else
                {
                    if (!frozenPlayers.ContainsKey(senderClient.userID))
                    {
                        Broadcast.noticeTo(senderClient.netPlayer, "⛄", "You were frozen!", 3);
                        Vector3 playerPos;
                        if (getPosition(senderClient, out playerPos))
                            frozenPlayers.Add(senderClient.userID, playerPos);
                    }
                    else
                        Broadcast.broadcastTo(senderClient.netPlayer, "You are already frozen.");
                }
            }
        }

        internal static void callAPI(string typeName, string methodName, bool isPublic, params object[] args)
        {
            if (Vars.runningAPI)
            {
                try
                {
                    Type type = Vars.API.GetType(typeName);
                    MethodInfo method = type.GetMethod(methodName, (isPublic ? BindingFlags.Public : BindingFlags.NonPublic) | BindingFlags.Static);
                    method.Invoke(null, args);
                }
                catch {}
            }
        }

        internal static Hook callHook(string typeName, string methodName, bool isPublic, params object[] args)
        {
            Hook hook = Hook.Continue;
            if (Vars.runningAPI)
            {
                try
                {
                    Type type = Vars.API.GetType(typeName);
                    MethodInfo method = type.GetMethod(methodName, (isPublic ? BindingFlags.Public : BindingFlags.NonPublic) | BindingFlags.Static);
                    hook = (Hook)method.Invoke(null, args);
                }
                catch { hook = Hook.Continue; }
            }

            return hook;
        }

        public static void freezeNearby(PlayerClient senderClient, string[] args)
        {
            if (senderClient != null)
            {
                Character senderChar = getPlayerChar(senderClient);
                if (senderChar != null)
                {
                    Vector3 senderPos = senderChar.transform.position;
                    float radius;
                    if (Single.TryParse(args[1], out radius))
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "All players in a " + radius + " meter radius have been frozen:");
                        foreach (PlayerClient pc in AllPlayerClients)
                        {
                            if (pc != null)
                            {
                                if (pc != senderClient)
                                {
                                    Character playerChar = getPlayerChar(pc);
                                    if (playerChar && playerChar.alive)
                                    {
                                        Vector3 playerPos = playerChar.eyesOrigin;
                                        if (Vector3.Distance(senderPos, playerPos) <= radius)
                                        {
                                            if (!frozenPlayers.ContainsKey(pc.userID))
                                            {
                                                frozenPlayers.Add(pc.userID, playerPos);
                                                Broadcast.noticeTo(pc.netPlayer, "⛄", "You were frozen!", 3);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        List<string> names = new List<string>();
                        List<string> names2 = new List<string>();
                        foreach (var player in frozenPlayers)
                        {
                            ulong UID = player.Key;
                            uLink.NetworkPlayer np = getNetPlayer(UID);
                            PlayerClient playerClient = getPlayerClient(np);
                            if (playerClient != null)
                            {
                                if (playerClient != senderClient)
                                {
                                    if (playerClient.userName.Length > 0)
                                        names.Add(playerClient.userName);
                                }
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
                    else
                        Broadcast.broadcastTo(senderClient.netPlayer, "Radius must be a number!");
                }
            }
        }

        public static void checkItems()
        {
            try
            {
                List<PlayerClient> playerClients = new List<PlayerClient>();
                foreach (PlayerClient pc in Vars.AllPlayerClients) { playerClients.Add(pc); }

                foreach (PlayerClient playerClient in playerClients)
                {
                    if (playerClient != null)
                    {
                        if (playerClient.netPlayer != null)
                        {
                            if (playerClient.controllable != null)
                            {
                                if (playerClient.controllable.GetComponent<Inventory>() != null)
                                {
                                    if (!craftList.Contains(playerClient.userID))
                                    {
                                        foreach (string itemName in restrictItems)
                                        {
                                            if (Items.hasItem(playerClient, itemName))
                                            {
                                                Broadcast.broadcastTo(playerClient.netPlayer, "Illegal item \"" + itemName + "\" found. Item removed.");
                                                List<IInventoryItem> items;
                                                Items.grabItem(playerClient, itemName, out items);
                                                foreach (IInventoryItem item in items)
                                                {
                                                    Items.removeItem(playerClient, item);
                                                }
                                            }
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
                conLog.Error("CI: " + ex.ToString());
            }
        }

        public static void reloadFileServer(string[] args)
        {
            if (args.Count() > 1)
            {
                string file = args[1];
                switch (file)
                {
                    case "offenses":
                        playerOffenses.Clear();
                        Data.readOffenseData();
                        break;
                    case "config":
                        RustEssentialsBootstrap._load.loadConfig();
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
                    case "loadout":
                        try { RustEssentialsBootstrap._load.loadDefaultLoadout(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading default loadout: " + ex.ToString()); }
                        break;
                    case "decay":
                        try { RustEssentialsBootstrap._load.loadDecay(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading decay: " + ex.ToString()); }
                        break;
                    case "remover":
                        try { RustEssentialsBootstrap._load.loadRemoverBlacklist(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading remover blacklist: " + ex.ToString()); }
                        break;
                    case "factions":
                        Data.readFactions();
                        break;
                    case "homes":
                        Data.readHomes();
                        break;
                    case "all":
                        playerOffenses.Clear();
                        Data.readOffenseData();
                        RustEssentialsBootstrap._load.loadConfig();
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
                        try { RustEssentialsBootstrap._load.loadDefaultLoadout(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading default loadout: " + ex.ToString()); }
                        try { RustEssentialsBootstrap._load.loadDecay(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading decay: " + ex.ToString()); }
                        try { RustEssentialsBootstrap._load.loadRemoverBlacklist(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading remover blacklist: " + ex.ToString()); }
                        Data.readFactions();
                        Data.readHomes();
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
                    case "offenses":
                        playerOffenses.Clear();
                        Data.readOffenseData();
                        break;
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
                    case "loadout":
                        try { RustEssentialsBootstrap._load.loadDefaultLoadout(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading default loadout: " + ex.ToString()); }
                        Broadcast.broadcastTo(sender, "Default loadout reloaded.");
                        break;
                    case "decay":
                        try { RustEssentialsBootstrap._load.loadDecay(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading decay: " + ex.ToString()); }
                        Broadcast.broadcastTo(sender, "Decay reloaded.");
                        break;
                    case "remover":
                        try { RustEssentialsBootstrap._load.loadRemoverBlacklist(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading remover blacklist: " + ex.ToString()); }
                        Broadcast.broadcastTo(sender, "Remover blacklist reloaded.");
                        break;
                    case "factions":
                        Data.readFactions();
                        Broadcast.broadcastTo(sender, "Factions reloaded.");
                        break;
                    case "homes":
                        Data.readHomes();
                        Broadcast.broadcastTo(sender, "Homes reloaded.");
                        break;
                    case "all":
                        playerOffenses.Clear();
                        Data.readOffenseData();
                        RustEssentialsBootstrap._load.loadConfig();
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
                        try { RustEssentialsBootstrap._load.loadDefaultLoadout(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading default loadout: " + ex.ToString()); }
                        try { RustEssentialsBootstrap._load.loadDecay(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading decay: " + ex.ToString()); }
                        try { RustEssentialsBootstrap._load.loadRemoverBlacklist(); }
                        catch (Exception ex) { Vars.conLog.Error("Error when loading remover blacklist: " + ex.ToString()); }
                        Data.readFactions();
                        Data.readHomes();
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
                    rank = "[" + findRank(possibleClients[0].userID) + "] ";

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
                    rank = "[" + findRank(possibleClients[0].userID) + "] ";

                string leaveMessage = Vars.leaveMessage.Replace("$USER$", rank + fakeName);
                Broadcast.broadcastAll(leaveMessage);
            }
            else
            {
                string rank = "[" + findRank(senderClient.userID) + "] ";

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
                double timeDecimal = Math.Round(Time.getTime(), 2);
                TimeSpan ts = TimeSpan.FromHours(timeDecimal);
                Broadcast.broadcastTo(senderClient.netPlayer, "The time is currently " + timeDecimal + " [" + ts.Hours + ":" + ts.Minutes + "].");
            }
        }

        public static void cycleMOTD()
        {
            foreach (MOTD motd in cycleMOTDList)
            {
                if (motd != null)
                {
                    TimerPlus t = TimerPlus.Create(Convert.ToInt64(motd.interval), true, cycleMOTDElapsed, motd.name);

                    if (!cycleMOTDTimers.ContainsKey(motd.name))
                        cycleMOTDTimers.Add(motd.name, t);
                }
            }
        }

        public static void onceMOTD()
        {
            foreach (MOTD motd in onceMOTDList)
            {
                if (motd != null)
                {
                    TimerPlus t = TimerPlus.Create(Convert.ToInt64(motd.interval), false, onceMOTDElapsed, motd.name);

                    if (!onceMOTDTimers.ContainsKey(motd.name))
                        onceMOTDTimers.Add(motd.name, t);
                }
            }    
        }

        public static void listMOTD()
        {
            foreach (MOTD motd in listMOTDList)
            {
                if (motd != null)
                {
                    TimerPlus t = TimerPlus.Create(Convert.ToInt64(motd.interval), true, listMOTDElapsed, motd.name);

                    if (!listMOTDTimers.ContainsKey(motd.name))
                        listMOTDTimers.Add(motd.name, t);
                }
            }
        }

        private static Dictionary<string, int> timeCycled = new Dictionary<string, int>();
        private static void cycleMOTDElapsed(params object[] args)
        {
            if (args.Count() > 0)
            {
                string motdName = (string)args[0];
                if (!timeCycled.ContainsKey(motdName))
                    timeCycled.Add(motdName, 0);

                if (timeCycled[motdName] > 50)
                    timeCycled[motdName] = 2;

                timeCycled[motdName]++;
                if (timeCycled[motdName] > 1)
                {
                    if (cycleMOTDList.ContainsMOTD(motdName))
                    {
                        foreach (string s in cycleMOTDList.GetMOTD(motdName).messages)
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
        }

        private static void onceMOTDElapsed(params object[] args)
        {
            if (args.Count() > 0)
            {
                string motdName = (string)args[0];
                if (onceMOTDList.ContainsMOTD(motdName))
                {
                    foreach (string s in onceMOTDList.GetMOTD(motdName).messages)
                    {
                        if (s.StartsWith("{/") && s.EndsWith("}"))
                        {
                            string command = s.Substring(1, s.Length - 2);
                            Commands.executeCMDServer(command);
                        }
                        else
                            Broadcast.broadcastAll(s);
                    }
                    onceMOTDTimers.Remove(motdName);
                }
            }
        }

        private static Dictionary<string, int> currentMessage = new Dictionary<string, int>();
        private static void listMOTDElapsed(params object[] args)
        {
            if (args.Count() > 0)
            {
                string motdName = (string)args[0];
                if (!currentMessage.ContainsKey(motdName))
                    currentMessage.Add(motdName, 0);

                if (!timeCycled.ContainsKey(motdName))
                    timeCycled.Add(motdName, 0);

                if (timeCycled[motdName] > 50)
                    timeCycled[motdName] = 2;

                timeCycled[motdName]++;
                if (timeCycled[motdName] > 1)
                {
                    if (listMOTDList.ContainsMOTD(motdName))
                    {
                        string message = listMOTDList.GetMOTD(motdName).messages[currentMessage[motdName]];

                        if (message.StartsWith("{/") && message.EndsWith("}"))
                        {
                            string command = message.Substring(1, message.Length - 2);
                            Commands.executeCMDServer(command);
                        }
                        else
                            Broadcast.broadcastAll(message);

                        currentMessage[motdName]++;

                        if (currentMessage[motdName] > listMOTDList.GetMOTD(motdName).messages.Count - 1)
                            currentMessage[motdName] = 0;
                    }
                }
            }
        }

        public static string findRank(ulong UID)
        {
            string rank = defaultRank;
            foreach (KeyValuePair<string, List<ulong>> kv in rankList)
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
            int rankPriority = (rankList.Count - rankList.Keys.ToList().FindIndex((string s) => s == rank));
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
                            if (!godList.Contains(targetClient.userID))
                                godList.Add(targetClient.userID);
                        }
                        else
                        {
                            if (godList.Contains(targetClient.userID))
                                godList.Remove(targetClient.userID);
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
                            if (!godList.Contains(targetClient.userID))
                                godList.Add(targetClient.userID);
                        }
                        else
                        {
                            if (godList.Contains(targetClient.userID))
                                godList.Remove(targetClient.userID);
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
                    if (!godList.Contains(senderClient.userID))
                        godList.Add(senderClient.userID);
                }
                else
                {
                    if (godList.Contains(senderClient.userID))
                        godList.Remove(senderClient.userID);
                }

                Broadcast.noticeTo(sender, "♫", (b ? "God mode activated." : "God mode deactivated."), 2, true);
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
                        Items.clearInventory(targetClient);
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
                        Items.clearInventory(targetClient);
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

                        metabolism.AddRads(-metabolism.GetRadLevel());

                        targetChar.controllable.GetComponent<HumanBodyTakeDamage>().Bandage(100f);

                        if (targetChar.controllable.GetComponent<FallDamage>() != null)
                            targetChar.controllable.GetComponent<FallDamage>().ClearInjury();

                        if (targetClient.netPlayer != sender)
                        {
                            Broadcast.noticeTo(sender, "✙", "You healed " + senderName + ".", 2, true);
                            Broadcast.noticeTo(targetClient.netPlayer, "✙", ("You were healed by " + senderName + "."), 2, true);
                        }
                        else
                        {
                            Broadcast.noticeTo(sender, "✙", "You were healed.", 2, true);
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

                        targetChar.controllable.GetComponent<HumanBodyTakeDamage>().Bandage(100f);

                        if (targetChar.controllable.GetComponent<FallDamage>() != null)
                            targetChar.controllable.GetComponent<FallDamage>().ClearInjury();

                        if (targetClient.netPlayer != sender)
                        {
                            Broadcast.noticeTo(targetClient.netPlayer, "✙", ("You were healed by " + senderName + "."), 2, true);
                            Broadcast.noticeTo(sender, "✙", "You healed " + targetClient.userName + ".", 2, true);
                        }
                        else
                        {
                            Broadcast.noticeTo(sender, "✙", "You were healed.", 2, true);
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

                targetChar.controllable.GetComponent<HumanBodyTakeDamage>().Bandage(100f);

                if (targetChar.controllable.GetComponent<FallDamage>() != null)
                    targetChar.controllable.GetComponent<FallDamage>().ClearInjury();

                Broadcast.noticeTo(sender, "✙", "You were healed.", 2, true);
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
                        Broadcast.broadcastTo(sender, "Fall damage has been enabled for everyone.");
                        fallDamage = true;
                        break;
                    case "off":
                        Broadcast.broadcastTo(sender, "Fall damage has been disabled for everyone.");
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

                            if (Checks.ofLowerRank(targetClient.userID, senderClient.userID) || sender == targetClient.netPlayer)
                            {
                                IDBase idBase = (IDBase)targetChar;
                                Broadcast.noticeTo(senderClient.netPlayer, "№", targetClient.userName + " fell victim to /kill.");
                                killList.Add(targetClient);

                                TakeDamage component = targetClient.controllable.GetComponent<TakeDamage>();
                                component.SetGodMode(false);
                                if (godList.Contains(targetClient.userID))
                                    godList.Remove(targetClient.userID);
                                int result = (int)TakeDamage.Kill(idBase, idBase);
                            }
                            else if (sender != targetClient.netPlayer)
                            {
                                Broadcast.noticeTo(sender, "№", "You are not allowed to /kill those of higher authority.", 5);
                                Broadcast.noticeTo(targetClient.netPlayer, "№", senderClient.userName + " tried to /kill you.", 5);
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

                            if (Checks.ofLowerRank(targetClient.userID, senderClient.userID) || sender == targetClient.netPlayer)
                            {
                                IDBase idBase = (IDBase)targetChar;
                                Broadcast.noticeTo(senderClient.netPlayer, "№", targetClient.userName + " fell victim to /kill.");
                                killList.Add(targetClient);

                                TakeDamage component = targetClient.controllable.GetComponent<TakeDamage>();
                                component.SetGodMode(false);
                                if (godList.Contains(targetClient.userID))
                                    godList.Remove(targetClient.userID);
                                int result = (int)TakeDamage.Kill(idBase, idBase);
                            }
                            else if (sender != targetClient.netPlayer)
                            {
                                Broadcast.noticeTo(sender, "№", "You are not allowed to /kill those of higher authority.", 5);
                                Broadcast.noticeTo(targetClient.netPlayer, "№", senderClient.userName + " tried to /kill you.", 5);
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
                    foreach (string ip in currentIPBans)
                    {
                        string reason = currentBanReasons[ip];
                        sw.WriteLine(ip + " # " + reason);
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
                        Broadcast.broadcastTo(senderClient.netPlayer, targetClient.userName + "'s UID is " + targetClient.userID + ".");
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
                        Broadcast.broadcastTo(senderClient.netPlayer, targetClient.userName + "'s UID is " + targetClient.userID + ".");
                    }
                }
            }
            else
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "Your UID is " + senderClient.userID + ".");
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

                    Broadcast.noticeTo(playerClient.netPlayer, "☻", "Player " + targetName + " (" + UID + ") has been unbanned.", 5, true);
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

                            Broadcast.noticeTo(playerClient.netPlayer, "☻", "Player " + playerName + " (" + UID + ") has been unbanned.", 5, true);
                        }
                        else
                        {
                            Broadcast.noticeTo(playerClient.netPlayer, "№", "UID " + targetName + " is not banned!", 5, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        Broadcast.noticeTo(playerClient.netPlayer, "№", "UID " + targetName + " is not banned!", 5, true);
                    }
                }
                else if (currentIPBans.Contains(targetName))
                {
                    currentIPBans.Remove(targetName);
                    currentBanReasons.Remove(targetName);
                    saveBans();

                    Broadcast.noticeTo(playerClient.netPlayer, "☻", "IP " + targetName + " has been unbanned.", 5, true);
                }
                else
                {
                    Broadcast.noticeTo(playerClient.netPlayer, "№", "Player/UID/IP " + targetName + " is not banned!", 5, true);
                }
            }
        }

        public static void kickIP(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string targetIP = args[1];

                try
                {
                    KeyValuePair<PlayerClient, string>[] possibleTargets = Array.FindAll(playerIPs.ToArray(), (KeyValuePair<PlayerClient, string> kv) => kv.Value == targetIP);
                    string reason = "";
                    List<string> reasonList = new List<string>();
                    if (args.Count() > 2)
                    {
                        int curIndex = 0;
                        foreach (string s in args)
                        {
                            if (curIndex > 1)
                            {
                                reasonList.Add(s);
                            }
                            curIndex++;
                        }

                        reason = string.Join(" ", reasonList.ToArray());
                    }
                    else
                    {
                        reason = "Kicked by a(n) " + findRank(senderClient.userID) + ".";
                    }

                    if (possibleTargets.Count() > 0)
                    {
                        if (Vars.enableKickBanMessages)
                            Broadcast.broadcastAll("IP " + targetIP + " was kicked.");
                        Vars.conLog.Error("IP " + targetIP + " was kicked.");

                        try
                        {
                            foreach (var kv in possibleTargets)
                            {
                                NetUser target = kv.Key.netUser;
                                if (target != null)
                                {
                                    if (Checks.ofLowerRank(target.userID, senderClient.userID))
                                    {
                                        kickPlayer(target, reason, false);
                                    }
                                    else if (senderClient.netPlayer == target.networkPlayer)
                                        Broadcast.noticeTo(senderClient.netPlayer, "№", "You can't kick yourself.", 5);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Vars.conLog.Error("KICKIP #1: " + ex.ToString());
                        }
                    }
                    else
                    {
                        Broadcast.broadcastTo(senderClient.netPlayer, "There are no players connected under the IP " + targetIP + ".");
                    }
                }
                catch (Exception ex)
                {
                    Vars.conLog.Error("KICKIP #2: " + ex.ToString());
                }
            }
        }

        public static void banIP(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string targetIP = args[1];

                try
                {
                    KeyValuePair<PlayerClient, string>[] possibleTargets = Array.FindAll(playerIPs.ToArray(), (KeyValuePair<PlayerClient, string> kv) => kv.Value == targetIP);
                    string reason = "";
                    List<string> reasonList = new List<string>();
                    if (args.Count() > 2)
                    {
                        int curIndex = 0;
                        foreach (string s in args)
                        {
                            if (curIndex > 1)
                            {
                                reasonList.Add(s);
                            }
                            curIndex++;
                        }

                        reason = string.Join(" ", reasonList.ToArray());
                    }
                    else
                    {
                        reason = "Banned by a(n) " + findRank(senderClient.userID) + ".";
                    }

                    if (Vars.enableKickBanMessages)
                        Broadcast.broadcastAll("IP " + targetIP + " was banned.");
                    Vars.conLog.Error("IP " + targetIP + " was banned.");

                    if (possibleTargets.Count() > 0)
                    {
                        try
                        {
                            RustEssentialsBootstrap._load.loadBans();
                            if (!currentBans.ContainsKey(targetIP))
                            {
                                foreach (var kv in possibleTargets)
                                {
                                    NetUser target = kv.Key.netUser;
                                    if (target != null)
                                    {
                                        if (Checks.ofLowerRank(target.userID, senderClient.userID))
                                        {
                                            Broadcast.broadcastTo(target.networkPlayer, "You were banned! Reason:");
                                            Broadcast.broadcastTo(target.networkPlayer, reason);
                                            Broadcast.broadcastToConsole(target.networkPlayer, "[color #FFA154][RustEssentials] [color white]You were [color #FB5A36]banned[color white]! Reason:");
                                            Broadcast.broadcastToConsole(target.networkPlayer, "[color white]" + reason);
                                            if (!kickQueue.Contains(target.userID))
                                                kickQueue.Add(target.userID);
                                            target.Kick(NetError.NoError, false);
                                            if (Vars.enableKickBanMessages)
                                            {
                                                Broadcast.broadcastAll("Player " + target.displayName + " (" + target.userID + ") was banned. Reason:");
                                                Broadcast.broadcastAll(reason);
                                            }
                                            Vars.conLog.Error("Player " + target.displayName + " (" + target.userID + ") was banned. Reason:");
                                            Vars.conLog.Error(reason);
                                        }
                                        else if (senderClient.netPlayer == target.networkPlayer)
                                            Broadcast.noticeTo(senderClient.netPlayer, "№", "You can't ban yourself.", 5);
                                    }
                                }
                                currentIPBans.Add(targetIP);
                                currentBanReasons.Add(targetIP, reason);
                                saveBans();
                            }
                            else
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, "IP " + targetIP + " is already banned!");
                            }
                        }
                        catch (Exception ex)
                        {
                            Vars.conLog.Error("BANIP #1: " + ex.ToString());
                        }
                    }
                    else
                    {
                        RustEssentialsBootstrap._load.loadBans();
                        if (!currentBans.ContainsKey(targetIP))
                        {
                            currentIPBans.Add(targetIP);
                            currentBanReasons.Add(targetIP, reason);
                            saveBans();
                        }
                        else
                        {
                            Broadcast.noticeTo(senderClient.netPlayer, "!", "IP " + targetIP + " is already banned!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Vars.conLog.Error("BANIP #2: " + ex.ToString());
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
                    ulong UID = Convert.ToUInt64(targetName);
                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID == UID);
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
                                    reason = "Banned by a(n) " + findRank(senderClient.userID) + ".";
                                }

                                if (Checks.ofLowerRank(target.userID, senderClient.userID))
                                {
                                    RustEssentialsBootstrap._load.loadBans();
                                    if (!currentBans.ContainsKey(target.userID.ToString()))
                                    {
                                        Broadcast.broadcastTo(target.networkPlayer, "You were banned! Reason:");
                                        Broadcast.broadcastTo(target.networkPlayer, reason);
                                        Broadcast.broadcastToConsole(target.networkPlayer, "[color #FFA154][RustEssentials] [color white]You were [color #FB5A36]banned[color white]! Reason:");
                                        Broadcast.broadcastToConsole(target.networkPlayer, "[color white]" + reason);
                                        target.Kick(NetError.NoError, false);
                                        if (Vars.enableKickBanMessages)
                                        {
                                            Broadcast.broadcastAll("Player " + target.displayName + " (" + target.userID + ") was banned. Reason:");
                                            Broadcast.broadcastAll(reason);
                                        }
                                        Vars.conLog.Error("Player " + target.displayName + " (" + target.userID + ") was banned. Reason:");
                                        Vars.conLog.Error(reason);
                                        currentBans.Add(target.userID.ToString(), target.displayName);
                                        currentBanReasons.Add(target.userID.ToString(), reason);
                                        saveBans();
                                    }
                                    else
                                    {
                                        Broadcast.noticeTo(senderClient.netPlayer, "!", "Player " + target.displayName + " (" + target.userID + ") is already banned!", 5);
                                    }
                                }
                                else if (senderClient.netPlayer != target.networkPlayer)
                                {
                                    Broadcast.noticeTo(senderClient.netPlayer, "№", "You are not allowed to ban those of higher authority.", 5);
                                    Broadcast.noticeTo(target.networkPlayer, "№", senderClient.userName + " tried to ban you.", 5);
                                }
                                else if (senderClient.netPlayer == target.networkPlayer)
                                    Broadcast.noticeTo(senderClient.netPlayer, "№", "You can't ban yourself.", 5);
                            }
                        }
                        catch (Exception ex)
                        {
                            Vars.conLog.Error("BANP #1: " + ex.ToString());
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
                            reason = "Banned by a(n) " + findRank(senderClient.userID) + ".";
                        }

                        if (Checks.ofLowerRank(UID, senderClient.userID))
                        {
                            RustEssentialsBootstrap._load.loadBans();
                            if (!currentBans.ContainsKey(UID.ToString()))
                            {
                                Thread t = new Thread(() =>
                                {
                                    string userName;
                                    if (grabNameByUID(UID, out userName))
                                    {
                                        if (Vars.enableKickBanMessages)
                                        {
                                            Broadcast.broadcastAll("UID " + UID + " (" + userName + ") was banned. Reason:");
                                            Broadcast.broadcastAll(reason);
                                        }
                                        currentBans.Add(UID.ToString(), userName);
                                    }
                                    else
                                    {
                                        if (Vars.enableKickBanMessages)
                                        {
                                            Broadcast.broadcastAll("UID " + UID + " was banned. Reason:");
                                            Broadcast.broadcastAll(reason);
                                        }
                                        currentBans.Add(UID.ToString(), "Unknown Player");
                                    }
                                    currentBanReasons.Add(UID.ToString(), reason);
                                    saveBans();
                                });
                                t.IsBackground = true;
                                t.Start();
                            }
                            else
                            {
                                string userName;
                                if (grabNameByUID(UID, out userName))
                                    Broadcast.noticeTo(senderClient.netPlayer, "!", "UID " + UID + " (" + userName + ") is already banned!");
                                else
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
                                        reason = "Banned by a(n) " + findRank(senderClient.userID) + ".";
                                    }

                                    if (Checks.ofLowerRank(target.userID, senderClient.userID))
                                    {
                                        RustEssentialsBootstrap._load.loadBans();
                                        if (!currentBans.ContainsKey(target.userID.ToString()))
                                        {
                                            Broadcast.broadcastTo(target.networkPlayer, "You were banned! Reason:");
                                            Broadcast.broadcastTo(target.networkPlayer, reason);
                                            Broadcast.broadcastToConsole(target.networkPlayer, "[color #FFA154][RustEssentials] [color white]You were [color #FB5A36]banned[color white]! Reason:");
                                            Broadcast.broadcastToConsole(target.networkPlayer, "[color white]" + reason);
                                            target.Kick(NetError.NoError, false);
                                            if (Vars.enableKickBanMessages)
                                            {
                                                Broadcast.broadcastAll("Player " + target.displayName + " (" + target.userID + ") was banned. Reason:");
                                                Broadcast.broadcastAll(reason);
                                            }
                                            Vars.conLog.Error("Player " + target.displayName + " (" + target.userID + ") was banned. Reason:");
                                            Vars.conLog.Error(reason);
                                            currentBans.Add(target.userID.ToString(), target.displayName);
                                            currentBanReasons.Add(target.userID.ToString(), reason);
                                            saveBans();
                                        }
                                        else
                                        {
                                            Broadcast.noticeTo(senderClient.netPlayer, "!", "Player " + target.displayName + " (" + target.userID + ") is already banned!", 5);
                                        }
                                    }
                                    else if (senderClient.netPlayer != target.networkPlayer)
                                    {
                                        Broadcast.noticeTo(senderClient.netPlayer, "№", "You are not allowed to ban those of higher authority.", 5);
                                        Broadcast.noticeTo(target.networkPlayer, "№", senderClient.userName + " tried to ban you.", 5);
                                    }
                                    else if (senderClient.netPlayer == target.networkPlayer)
                                        Broadcast.noticeTo(senderClient.netPlayer, "№", "You can't ban yourself.", 5);
                                }
                            }
                            catch (Exception ex2)
                            {
                                Vars.conLog.Error("BANP #2: " + ex2.ToString());
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
                                        reason = "Banned by a(n) " + findRank(senderClient.userID) + ".";
                                    }

                                    if (Checks.ofLowerRank(target.userID, senderClient.userID))
                                    {
                                        RustEssentialsBootstrap._load.loadBans();
                                        if (!currentBans.ContainsKey(target.userID.ToString()))
                                        {
                                            Broadcast.broadcastTo(target.networkPlayer, "You were banned! Reason:");
                                            Broadcast.broadcastTo(target.networkPlayer, reason);
                                            Broadcast.broadcastToConsole(target.networkPlayer, "[color #FFA154][RustEssentials] [color white]You were [color #FB5A36]banned[color white]! Reason:");
                                            Broadcast.broadcastToConsole(target.networkPlayer, "[color white]" + reason);
                                            target.Kick(NetError.NoError, false);
                                            if (Vars.enableKickBanMessages)
                                            {
                                                Broadcast.broadcastAll("Player " + target.displayName + " (" + target.userID + ") was banned. Reason:");
                                                Broadcast.broadcastAll(reason);
                                            }
                                            Vars.conLog.Error("Player " + target.displayName + " (" + target.userID + ") was banned. Reason:");
                                            Vars.conLog.Error(reason);
                                            currentBans.Add(target.userID.ToString(), target.displayName);
                                            currentBanReasons.Add(target.userID.ToString(), reason);
                                            saveBans();
                                        }
                                        else
                                        {
                                            Broadcast.noticeTo(senderClient.netPlayer, "!", "Player " + target.displayName + " (" + target.userID + ") is already banned!", 5);
                                        }
                                    }
                                    else if (senderClient.netPlayer != target.networkPlayer)
                                    {
                                        Broadcast.noticeTo(senderClient.netPlayer, "№", "You are not allowed to ban those of higher authority.", 5);
                                        Broadcast.noticeTo(target.networkPlayer, "№", senderClient.userName + " tried to ban you.", 5);
                                    }
                                    else if (senderClient.netPlayer == target.networkPlayer)
                                        Broadcast.noticeTo(senderClient.netPlayer, "№", "You can't ban yourself.", 5);
                                }
                            }
                            catch (Exception ex2)
                            {
                                Vars.conLog.Error("BANP #3: " + ex.ToString());
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
                                    reason = "Kicked by a(n) " + findRank(senderClient.userID) + ".";
                                }

                                if (Checks.ofLowerRank(target.userID, senderClient.userID))
                                {
                                    kickPlayer(target, reason, false);
                                }
                                else if (senderClient.netPlayer != target.networkPlayer)
                                {
                                    Broadcast.noticeTo(senderClient.netPlayer, "№", "You are not allowed to kick those of higher authority.", 5);
                                    Broadcast.noticeTo(target.networkPlayer, "№", senderClient.userName + " tried to kick you.", 5);
                                }
                                else if (senderClient.netPlayer == target.networkPlayer)
                                    Broadcast.noticeTo(senderClient.netPlayer, "№", "You can't kick yourself.", 5);
                            }
                        }
                        catch (Exception ex)
                        {
                            Vars.conLog.Error("KICKP #1: " + ex.ToString());
                        }
                    }
                }
                else
                {
                    PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));
                    if (possibleTargets.Count() == 0)
                    {
                        ulong UIDLong;
                        if (ulong.TryParse(targetName, out UIDLong))
                        {
                            PlayerClient possibleClient = Array.Find(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userID == UIDLong);
                            if (possibleClient != null)
                            {
                                NetUser target = possibleClient.netUser;
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
                                            reason = "Kicked by a(n) " + findRank(senderClient.userID) + ".";
                                        }

                                        if (Checks.ofLowerRank(target.userID, senderClient.userID))
                                        {
                                            kickPlayer(target, reason, false);
                                        }
                                        else if (senderClient.netPlayer != target.networkPlayer)
                                        {
                                            Broadcast.noticeTo(senderClient.netPlayer, "№", "You are not allowed to kick those of higher authority.", 5);
                                            Broadcast.noticeTo(target.networkPlayer, "№", senderClient.userName + " tried to kick you.", 5);
                                        }
                                        else if (senderClient.netPlayer == target.networkPlayer)
                                            Broadcast.noticeTo(senderClient.netPlayer, "№", "You can't kick yourself.", 5);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Vars.conLog.Error("KICKP #2: " + ex.ToString());
                                }
                            }
                            else
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, "No player UIDs equal \"" + targetName + "\".");
                            }
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names/UIDs equal or contain \"" + targetName + "\".");
                    }
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
                                    reason = "Kicked by a(n) " + findRank(senderClient.userID) + ".";
                                }

                                if (Checks.ofLowerRank(target.userID, senderClient.userID))
                                {
                                    kickPlayer(target, reason, false);
                                }
                                else if (senderClient.netPlayer != target.networkPlayer)
                                {
                                    Broadcast.noticeTo(senderClient.netPlayer, "№", "You are not allowed to kick those of higher authority.", 5);
                                    Broadcast.noticeTo(target.networkPlayer, "№", senderClient.userName + " tried to kick you.", 5);
                                }
                                else if (senderClient.netPlayer == target.networkPlayer)
                                    Broadcast.noticeTo(senderClient.netPlayer, "№", "You can't kick yourself.", 5);
                            }
                        }
                        catch (Exception ex)
                        {
                            Vars.conLog.Error("KICKP #3: " + ex.ToString());
                        }
                    }
                }
            }
        }

        public static void otherKick(NetUser target, string reason, bool isThreaded = false)
        {
            if (target != null)
            {
                Vars.conLog.Error("Player " + target.displayName + " (" + target.userID + ") was kicked for:", isThreaded);
                Vars.conLog.Error(reason, isThreaded);
                if (!kickQueue.Contains(target.userID))
                    kickQueue.Add(target.userID);
                Broadcast.broadcastTo(target.networkPlayer, "You were kicked! Reason:");
                Broadcast.broadcastTo(target.networkPlayer, reason);
                Broadcast.broadcastToConsole(target.networkPlayer, "[color #FFA154][RustEssentials] [color white]You were [color #FB5A36]kicked[color white]! Reason:");
                Broadcast.broadcastToConsole(target.networkPlayer, "[color white]" + reason);
                target.Kick(NetError.NoError, false);
            }
        }

        public static void whitelistKick(NetUser target, string reason)
        {
            if (target != null)
            {
                Vars.conLog.Error("Nonwhitelisted player " + target.displayName + " (" + target.userID + ") attempted to join.");
                if (!kickQueue.Contains(target.userID))
                    kickQueue.Add(target.userID);
                Broadcast.broadcastTo(target.networkPlayer, "You were kicked! Reason:");
                Broadcast.broadcastTo(target.networkPlayer, reason);
                Broadcast.broadcastToConsole(target.networkPlayer, "[color #FFA154][RustEssentials] [color white]You were [color #FB5A36]kicked[color white]! Reason:");
                Broadcast.broadcastToConsole(target.networkPlayer, "[color white]" + reason);
                target.Kick(NetError.NoError, false);
            }
        }

        public static void kickPlayer(NetUser target, string reason, bool isBan)
        {
            if (target != null)
            {
                if (!isBan)
                {
                    Vars.conLog.Error("Player " + target.displayName + " (" + target.userID + ") was kicked. Reason:");
                    Vars.conLog.Error(reason);
                }
                if (!kickQueue.Contains(target.userID))
                    kickQueue.Add(target.userID);
                Broadcast.broadcastTo(target.networkPlayer, (isBan ? "You were banned! Reason:" : "You were kicked! Reason:"));
                Broadcast.broadcastTo(target.networkPlayer, reason);
                Broadcast.broadcastToConsole(target.networkPlayer, "[color #FFA154][RustEssentials] [color white]You were [color #FB5A36]" + (isBan ? "banned" : "kicked") + "[color white]! Reason:");
                Broadcast.broadcastToConsole(target.networkPlayer, "[color white]" + reason);
                if (isBan)
                    Vars.conLog.Error("Banned player " + target.displayName + " (" + target.userID + ") attempted to join.");
                target.Kick(NetError.NoError, false);
                if (!isBan)
                {
                    if (Vars.enableKickBanMessages)
                    {
                        Broadcast.broadcastAll("Player " + target.displayName + " was kicked. Reason:");
                        Broadcast.broadcastAll(reason);
                    }
                }
            }
        }

        public static void getPlayerPos(PlayerClient senderClient, string[] args, bool isAlternative)
        {
            if (args.Count() > 1 && isAlternative)
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

                string targetName = string.Join(" ", playerNameList.ToArray());
                
                PlayerClient[] possibleTargets = Array.FindAll(AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));
                if (possibleTargets.Count() == 0)
                    Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + targetName + "\".");
                else if (possibleTargets.Count() > 1)
                    Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + targetName + "\".");
                else
                {
                    PlayerClient targetClient = possibleTargets[0];
                    string combineOutput = getPosition(targetClient).ToString();

                    Broadcast.broadcastTo(senderClient.netPlayer, targetClient.userName + "'s " + (Vars.ghostList.ContainsKey(targetClient.userID) ? "body's " : "") + "position: " + combineOutput);
                    if (Vars.ghostList.ContainsKey(targetClient.userID))
                        Broadcast.broadcastTo(senderClient.netPlayer, targetClient.userName + "'s ghost's position: " + Vars.ghostPositions[targetClient.userID]);
                }
            }
            else
            {
                string combineOutput = getPosition(senderClient).ToString();

                Broadcast.broadcastTo(senderClient.netPlayer, "Your " + (Vars.ghostList.ContainsKey(senderClient.userID) ? "body's " : "") + "position: " + combineOutput);
                if (Vars.ghostList.ContainsKey(senderClient.userID))
                    Broadcast.broadcastTo(senderClient.netPlayer, "Your ghost's position: " + Vars.ghostPositions[senderClient.userID]);
            }
        }

        public static void airdropServer()
        {
            if (AllPlayerClients.Count > 0)
            {
                if (Vars.announceDrops)
                    Broadcast.broadcastAll("Incoming airdrop!");
                SupplyDropZone.CallAirDrop();
            }
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
                    if (announceDrops)
                    {
                        Broadcast.broadcastAll("Incoming airdrop!");
                    }
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
                float time = 11.9f;
                float dayLength = 45f;
                float nightLength = 15f;
                bool freezeTime = false;

                if (float.TryParse(Config.startTime, out time))
                    Time.setTime(time);
                if (float.TryParse(Config.dayLength, out dayLength))
                    Time.setDayLength(dayLength);
                if (float.TryParse(Config.nightLength, out nightLength))
                    Time.setNightLength(nightLength);
                if (bool.TryParse(Config.freezeTime, out freezeTime))
                    Time.freezeTime(freezeTime);
                
                conLog.Info("Overriding time...");
                conLog.Info("Overriding day length...");
                conLog.Info("Overriding night length...");

                if (Convert.ToBoolean(Config.freezeTime))
                    conLog.Info("Time frozen...");
            }
            catch (Exception ex)
            {
                conLog.Error("Something went wrong when overriding the enviroment! Skipping... Error: " + ex.ToString());
            }
        }

        public static void restoreKit(params object[] args)
        {
            if (args.Count() > 0)
            {
                string kitName = (string)args[0];
                string UIDString = (string)args[1];
                ulong UID;
                if (ulong.TryParse(UIDString, out UID))
                {
                    if (activeKitCooldowns.ContainsKey(UID))
                    {
                        if (activeKitCooldowns[UID].Count() > 1)
                        {
                            TimerPlus tp = Array.Find(activeKitCooldowns[UID].ToArray(), (KeyValuePair<TimerPlus, string> kv) => kv.Value == kitName).Key;
                            activeKitCooldowns[UID].Remove(tp);
                        }
                        else if (activeKitCooldowns[UID].Count() == 1)
                            activeKitCooldowns.Remove(UID);
                    }
                }
            }
        }

        public static void restoreWarp(params object[] args)
        {
            if (args.Count() > 0)
            {
                string warpName = (string)args[0];
                string UIDString = (string)args[1];
                ulong UID;
                if (ulong.TryParse(UIDString, out UID))
                {
                    if (activeWarpCooldowns.ContainsKey(UID))
                    {
                        if (activeWarpCooldowns[UID].Count() > 1)
                        {
                            TimerPlus tp = Array.Find(activeWarpCooldowns[UID].ToArray(), (KeyValuePair<TimerPlus, string> kv) => kv.Value == warpName).Key;
                            activeWarpCooldowns[UID].Remove(tp);
                        }
                        else if (activeWarpCooldowns[UID].Count() == 1)
                            activeWarpCooldowns.Remove(UID);
                    }
                }
            }
        }

        public static void sleeperTimerElapsed(params object[] args)
        {
            if (args.Count() > 0)
            {
                NetUser user = (NetUser)args[0];
                ulong userID = user.userID;
                if (user.userID == userID)
                {
                    if (SleepingAvatar.IsOpen(user))
                    {
                        SleepingAvatar.Close(user);
                    }
                }
            }
        }


        public static void updatePlaneMovement(SupplyDropPlane SDP)
        {
            try
            {
                SDP.DoMovement();
                object[] args = new object[] { SDP.transform.position, SDP.transform.rotation };
                SDP.networkView.RPC("GetNetworkUpdate", uLink.RPCMode.OthersExceptOwner, args);
            }
            catch (Exception ex)
            {
                if (!SDP.droppedPayload)
                {
                    SDP.droppedPayload = true;
                    int num = UnityEngine.Random.Range(Vars.minimumCrates, Vars.maximumCrates + 1);
                    for (int i = 0; i < num; i++)
                    {
                        SDP.DropCrate();
                    }
                }
                SDP.NetDestroy();
                conLog.Error("Plane destroyed due to a movement RPC error:");
                conLog.Error(ex.ToString());
            }
        }

        public static Vector3 getGroundPos(DeployableObject DO)
        {
            RaycastHit hit;
            if (Physics.Raycast(new Ray(DO.transform.position, Vector3.down), out hit, 30005f, -472317957))
            {
                if (hit.collider is TerrainCollider || hit.collider.gameObject.layer == 10 || hit.collider.gameObject.layer == 0)
                {
                    return Vector3.MoveTowards(hit.point, DO.transform.position, 1);
                }
            }
            return DO.transform.position;
        }

        public static string getFullExplosiveName(string instanceName)
        {
            instanceName = instanceName.Replace("(Clone)", "");
            switch (instanceName)
            {
                case "ExplosiveCharge":
                    return "Explosive Charge";

                case "F1Grenade":
                    return "F1 Grenade";

                case "F1GrenadeWorld":
                    return "F1 Grenade";

                default:
                    return null;
            }
        }

        public static List<string> objectNames = new List<string>()
        {
            "Wood Storage Box",
            "Workbench",
            "Wood Barricade",
            "Camp Fire",
            "Furnace",
            "Large Wood Storage",
            "Spike Wall",
            "Large Spike Wall",
            "Wood Gateway",
            "Small Stash",
            "Repair Bench",
            "Wood Shelter"
        };
        public static string getFullObjectName(string instanceName)
        {
            instanceName = instanceName.Replace("(Clone)", "");
            switch (instanceName)
            {
                case "WoodBox":
                    return "Wood Storage Box";

                case "Workbench":
                    return "Workbench";

                case "Barricade_Fence_Deployable":
                    return "Wood Barricade";

                case "Campfire":
                    return "Camp Fire";

                case "Furnace":
                    return "Furnace";

                case "WoodBoxLarge":
                    return "Large Wood Storage";

                case "WoodSpikeWall":
                    return "Spike Wall";

                case "LargeWoodSpikeWall":
                    return "Large Spike Wall";

                case "WoodGateway":
                    return "Wood Gateway";

                case "WoodGate":
                    return "Wood Gate";

                case "WoodenDoor":
                    return "Wooden Door";

                case "MetalDoor":
                    return "Metal Door";

                case "MetalBarsWindow":
                    return "Metal Window Bars";

                case "SmallStash":
                    return "Small Stash";

                case "RepairBench":
                    return "Repair Bench";

                case "Wood_Shelter":
                    return "Wood Shelter";

                default:
                    return null;
            }
        }

        public static string getFullStructureName(string instanceName)
        {
            instanceName = instanceName.Replace("(Clone)", "");
            switch (instanceName)
            {
                case "WoodFoundation":
                    return "Wood Foundation";

                case "WoodPillar":
                    return "Wood Pillar";

                case "WoodRamp":
                    return "Wood Ramp";

                case "WoodCeiling":
                    return "Wood Ceiling";

                case "WoodStairs":
                    return "Wood Stairs";

                case "WoodWindowFrame":
                    return "Wood Window";

                case "WoodDoorFrame":
                    return "Wood Doorway";

                case "WoodWall":
                    return "Wood Wall";

                case "MetalFoundation":
                    return "Metal Foundation";

                case "MetalPillar":
                    return "Metal Pillar";

                case "MetalRamp":
                    return "Metal Ramp";

                case "MetalCeiling":
                    return "Metal Ceiling";

                case "MetalStairs":
                    return "Metal Stairs";

                case "MetalWindowFrame":
                    return "Metal Window";

                case "MetalDoorFrame":
                    return "Metal Doorway";

                case "MetalWall":
                    return "Metal Wall";

                default:
                    return null;
            }
        }

        public static void checkPings()
        {
            List<PlayerClient> playerClients = new List<PlayerClient>();
            foreach (PlayerClient pc in Vars.AllPlayerClients) { playerClients.Add(pc); }

            foreach (PlayerClient pc in playerClients)
            {
                if (pc.netPlayer.averagePing >= pingLimit && pingLimit > 0)
                {
                    otherKick(pc.netUser, "Your ping exceeds the limit of " + pingLimit + " ms.");
                }
            }
        }

        public static bool simulateTeleport(PlayerClient playerClient, Vector3 destination, bool allowDoubleTP = true)
        {
            if (!currentlyTeleporting.Contains(playerClient))
                currentlyTeleporting.Add(playerClient);

            if (!antihackTeleport.Contains(playerClient) && (Vars.enableAntiJump || Vars.enableAntiSpeed))
                antihackTeleport.Add(playerClient);

            StructureMaster master;
            bool teleportTwice = false;
            if (Vars.AllCharacters.ContainsKey(playerClient))
            {
                Character playerChar = Vars.AllCharacters[playerClient];
                if (playerChar != null && Vector3.Distance(playerChar.eyesOrigin, destination) > 375 && Checks.onStructure(destination, out master) && allowDoubleTP)
                {
                    teleportTwice = true;
                }

                Hook hook = callHook("RustEssentialsAPI.Hooks", "OnEssentialsTeleport", false, playerClient.userID, destination);

                if (Checks.ContinueHook(hook))
                {
                    if (playerChar != null && lastPositions.ContainsKey(playerChar))
                        lastPositions[playerChar] = destination;

                    RustServerManagement RSM = RustServerManagement.Get();
                    if (playerClient.netUser != null)
                    {
                        playerClient.netUser.truthDetector.NoteTeleported(destination, 2.0);
                    }
                    if (playerClient.controllable != null)
                    {
                        RSM.networkView.RPC<Vector3>("UnstickMove", playerClient.netPlayer, destination);
                        if (!teleportTwice)
                            currentlyTeleporting.Remove(playerClient);
                        else if (playerChar != null)
                            Vars.REB.StartCoroutine(continueTeleport(playerChar, destination));
                        return true;
                    }
                }
            }
            currentlyTeleporting.Remove(playerClient);
            return false;
        }

        private static IEnumerator continueTeleport(Character playerChar, Vector3 destination)
        {
            yield return new WaitForSeconds(2.5f);
            if (playerChar.eyesOrigin.y < destination.y)
            {
                simulateTeleport(playerChar.playerClient, destination, false);
            }
        }
    }

    public class MOTD
    {
        public string name;
        public string interval;
        public List<string> messages = new List<string>();

        public MOTD(string name, string interval, List<string> messages)
        {
            this.name = name;
            this.interval = interval;
            this.messages = messages;
        }
    }

    public class MOTDList<T> : List<T> where T : MOTD
    {
        public MOTD GetMOTD(string motdName)
        {
            foreach (var obj in this)
            {
                if (obj is MOTD)
                {
                    MOTD possibleMOTD = (MOTD)obj;
                    if (possibleMOTD.name == motdName)
                        return possibleMOTD;
                }
            }
            return new MOTD(null, null, new List<string>());
        }

        public bool ContainsMOTD(string motdName)
        {
            foreach (var obj in this)
            {
                if (obj is MOTD)
                {
                    MOTD possibleMOTD = (MOTD)obj;
                    if (possibleMOTD.name == motdName)
                        return true;
                }
            }
            return false;
        }
    }

    public enum Chat
    {
        Global = 0,
        Direct = 1,
        Faction = 2
    }

    public enum Hook
    {
        Continue = 0,
        Success = 1,
        Failure = 2
    }
}
