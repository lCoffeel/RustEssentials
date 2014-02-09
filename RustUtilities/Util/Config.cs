/**
 * @file: Config.cs
 * @author: Team Cerionn (https://github.com/Team-Cerionn)

 * @description: Config class for Rust Essentials
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace RustEssentials.Util
{
    public class Config
    {
        private static string configInput;

        // Whitelist
        private static Match MatchEnabledWhitelist;
        private static Match MatchMySQL;
        private static Match MatchUseSteamGroup;
        private static Match MatchSteamGroup;
        private static Match MatchAutoRefresh;
        private static Match MatchRefreshInterval;
        private static Match MatchUseAsMembers;
        private static Match MatchWhitelistKickCMD;
        private static Match MatchWhitelistKickJoin;
        private static Match MatchWhitelistGood;
        private static Match MatchWhitelistBad;
        // Airdrop
        private static Match MatchAnnounceDrops;
        // Environment
        private static Match MatchStartTime;
        private static Match MatchTimeScale;
        private static Match MatchFreezeTime;
        private static Match MatchFallDamage;
        private static Match MatchVoiceDistance;
        private static Match MatchEnableRepair;
        private static Match MatchForceNudity;
        // Chat
        private static Match MatchDirectChat;
        private static Match MatchGlobalChat;
        private static Match MatchRemoveTag;
        private static Match MatchDefaultChat;
        private static Match MatchDirectDistance;
        private static Match MatchAllowedChars;
        private static Match MatchRestrictChars;
        private static Match MatchMinimumNameCount;
        private static Match MatchMaximumNameCount;
        private static Match MatchKickDuplicate;
        private static Match MatchLowerAuthority;
        private static Match MatchIllegalWords;
        private static Match MatchCensorship;
        // Messages
        private static Match MatchBotName;
        private static Match MatchJoinMessage;
        private static Match MatchEnableJoin;
        private static Match MatchLeaveMessage;
        private static Match MatchEnableLeave;
        private static Match MatchSuicideMessage;
        private static Match MatchEnableSuicide;
        private static Match MatchMurderMessage;
        private static Match MatchMurderMessageUnknown;
        private static Match MatchEnableMurder;
        private static Match MatchDeathMessage;
        private static Match MatchEnableDeath;
        private static Match MatchUnknownCommand;
        private static Match MatchNextToName;
        private static Match MatchRemovePrefix;
        // Chat Logs
        private static Match MatchPluginChat;
        private static Match MatchChatLogCap;
        private static Match MatchLogCap;
        // Movement
        private static Match MatchTeleportRequest;
        private static Match MatchRequestDelay;
        private static Match MatchWarpDelay;
        private static Match MatchRequestCooldownType;
        private static Match MatchRequestCooldown;
        private static Match MatchDenyRequestWarzone;
        // MySQL
        private static Match MatchHost;
        private static Match MatchPort;
        private static Match MatchDatabase;
        private static Match MatchUser;
        private static Match MatchPass;
        // Inheritance
        private static Match MatchCommandInheritance;
        private static Match MatchKitInheritance;
        private static Match MatchWarpInheritance;
        // Damage
        private static Match MatchFriendlyFire;
        private static Match MatchAlliedFire;
        private static Match MatchNeutralDamage;
        private static Match MatchWarDamage;
        private static Match MatchWarFriendlyDamage;
        private static Match MatchWarAllyDamage;

        // Whitelist
        public static string enabledWhitelist;
        public static string MySQL;
        public static string useSteamGroup;
        public static string steamGroup;
        public static string autoRefresh;
        public static string refreshInterval;
        public static string whitelistToMembers;
        public static string whitelistKickCMD;
        public static string whitelistKickJoin;
        public static string whitelistCheckGood;
        public static string whitelistCheckBad;
        // Airdrop
        public static string announceDrops;
        // Environment
        public static string startTime;
        public static string timeScale;
        public static string freezeTime;
        public static string fallDamage;
        public static string voiceDistance;
        public static string enableRepair;
        public static string forceNudity;
        // Chat
        public static string directChat;
        public static string globalChat;
        public static string removeTag;
        public static string defaultChat;
        public static string directDistance;
        public static string allowedChars;
        public static string restrictChars;
        public static string minimumNameCount;
        public static string maximumNameCount;
        public static string kickDuplicate;
        public static string lowerAuthority;
        public static string illegalWords;
        public static string censorship;
        // Messages
        public static string botName;
        public static string joinMessage;
        public static string enableJoin;
        public static string leaveMessage;
        public static string enableLeave;
        public static string suicideMessage;
        public static string enableSuicide;
        public static string murderMessage;
        public static string murderMessageUnknown;
        public static string enableMurder;
        public static string deathMessage;
        public static string enableDeath;
        public static string unknownCommand;
        public static string nextToName;
        public static string removePrefix;
        // Chat Logs
        public static string logPluginChat;
        public static string chatLogCap;
        public static string logCap;
        // Movement
        public static string teleportRequest;
        public static string requestDelay;
        public static string warpDelay;
        public static string requestCooldownType;
        public static string requestCooldown;
        public static string denyRequestWarzone;
        // MySQL
        public static string host;
        public static string port;
        public static string database;
        public static string user;
        public static string pass;
        // Inheritance
        public static string inheritCommands;
        public static string inheritKits;
        public static string inheritWarps;
        // Damage
        public static string friendlyFire;
        public static string alliedFire;
        public static string neutralDamage;
        public static string warDamage;
        public static string warFriendlyDamage;
        public static string warAllyDamage;

        public static void setVariables()
        {
            configInput = File.ReadAllText(Vars.cfgFile);

            MatchEnabledWhitelist = Regex.Match(configInput, @"enableWhitelist=\w+", RegexOptions.IgnoreCase);
            MatchMySQL = Regex.Match(configInput, @"useMySQL=\w+", RegexOptions.IgnoreCase);
            MatchUseSteamGroup = Regex.Match(configInput, @"useSteamGroup=\w+", RegexOptions.IgnoreCase);
            MatchSteamGroup = Regex.Match(configInput, @"steamGroupName=[^\n]*", RegexOptions.IgnoreCase);
            MatchAutoRefresh = Regex.Match(configInput, @"autoRefresh=\w+", RegexOptions.IgnoreCase);
            MatchRefreshInterval = Regex.Match(configInput, @"refreshInterval=\w+", RegexOptions.IgnoreCase);
            MatchUseAsMembers = Regex.Match(configInput, @"useAsMembers=\w+", RegexOptions.IgnoreCase);
            MatchWhitelistKickCMD = Regex.Match(configInput, @"whitelistKickCMD=[^\n]*", RegexOptions.IgnoreCase);
            MatchWhitelistKickJoin = Regex.Match(configInput, @"whitelistKickJoin=[^\n]*", RegexOptions.IgnoreCase);
            MatchWhitelistGood = Regex.Match(configInput, @"whitelistCheckGood=[^\n]*", RegexOptions.IgnoreCase);
            MatchWhitelistBad = Regex.Match(configInput, @"whitelistCheckBad=[^\n]*", RegexOptions.IgnoreCase);
            MatchAnnounceDrops = Regex.Match(configInput, @"announceDrops=\w+", RegexOptions.IgnoreCase);
            MatchStartTime = Regex.Match(configInput, @"startTime=\w+", RegexOptions.IgnoreCase);
            MatchTimeScale = Regex.Match(configInput, @"timeScale=\w+", RegexOptions.IgnoreCase);
            MatchFreezeTime = Regex.Match(configInput, @"freezeTime=\w+", RegexOptions.IgnoreCase);
            MatchFallDamage = Regex.Match(configInput, @"fallDamage=\w+", RegexOptions.IgnoreCase);
            MatchVoiceDistance = Regex.Match(configInput, @"voiceDistance=\w+", RegexOptions.IgnoreCase);
            MatchEnableRepair = Regex.Match(configInput, @"enableRepair=\w+", RegexOptions.IgnoreCase);
            MatchForceNudity = Regex.Match(configInput, @"forceNudity=\w+", RegexOptions.IgnoreCase);
            MatchDirectChat = Regex.Match(configInput, @"directChat=\w+", RegexOptions.IgnoreCase);
            MatchGlobalChat = Regex.Match(configInput, @"globalChat=\w+", RegexOptions.IgnoreCase);
            MatchRemoveTag = Regex.Match(configInput, @"removeTag=\w+", RegexOptions.IgnoreCase);
            MatchDefaultChat = Regex.Match(configInput, @"defaultChat=\w+", RegexOptions.IgnoreCase);
            MatchDirectDistance = Regex.Match(configInput, @"directDistance=\w+", RegexOptions.IgnoreCase);
            MatchAllowedChars = Regex.Match(configInput, @"allowedChars=[^\n]*", RegexOptions.IgnoreCase);
            MatchRestrictChars = Regex.Match(configInput, @"restrictChars=\w+", RegexOptions.IgnoreCase);
            MatchMinimumNameCount = Regex.Match(configInput, @"minimumNameCount=\w+", RegexOptions.IgnoreCase);
            MatchMaximumNameCount = Regex.Match(configInput, @"maximumNameCount=\w+", RegexOptions.IgnoreCase);
            MatchKickDuplicate = Regex.Match(configInput, @"kickDuplicate=\w+", RegexOptions.IgnoreCase);
            MatchLowerAuthority = Regex.Match(configInput, @"lowerAuthority=\w+", RegexOptions.IgnoreCase);
            MatchIllegalWords = Regex.Match(configInput, @"illegalWords=[^\n]*", RegexOptions.IgnoreCase);
            MatchCensorship = Regex.Match(configInput, @"censorship=\w+", RegexOptions.IgnoreCase);
            MatchBotName = Regex.Match(configInput, @"botName=[^\n]*", RegexOptions.IgnoreCase);
            MatchJoinMessage = Regex.Match(configInput, @"joinMessage=[^\n]*", RegexOptions.IgnoreCase);
            MatchEnableJoin = Regex.Match(configInput, @"enableJoin=\w+", RegexOptions.IgnoreCase);
            MatchLeaveMessage = Regex.Match(configInput, @"leaveMessage=[^\n]*", RegexOptions.IgnoreCase);
            MatchEnableLeave = Regex.Match(configInput, @"enableLeave=\w+", RegexOptions.IgnoreCase);
            MatchSuicideMessage = Regex.Match(configInput, @"suicideMessage=[^\n]*", RegexOptions.IgnoreCase);
            MatchEnableSuicide = Regex.Match(configInput, @"enableSuicide=\w+", RegexOptions.IgnoreCase);
            MatchMurderMessage = Regex.Match(configInput, @"murderMessage=[^\n]*", RegexOptions.IgnoreCase);
            MatchMurderMessageUnknown = Regex.Match(configInput, @"murderMessageUnknown=[^\n]*", RegexOptions.IgnoreCase);
            MatchEnableMurder = Regex.Match(configInput, @"enableMurder=\w+", RegexOptions.IgnoreCase);
            MatchDeathMessage = Regex.Match(configInput, @"deathMessage=[^\n]*", RegexOptions.IgnoreCase);
            MatchEnableDeath = Regex.Match(configInput, @"enableDeath=\w+", RegexOptions.IgnoreCase);
            MatchUnknownCommand = Regex.Match(configInput, @"unknownCommand=\w+", RegexOptions.IgnoreCase);
            MatchNextToName = Regex.Match(configInput, @"nextToName=\w+", RegexOptions.IgnoreCase);
            MatchRemovePrefix = Regex.Match(configInput, @"removePrefix=\w+", RegexOptions.IgnoreCase);
            MatchPluginChat = Regex.Match(configInput, @"logPluginChat=\w+", RegexOptions.IgnoreCase);
            MatchChatLogCap = Regex.Match(configInput, @"chatLogCap=\w+", RegexOptions.IgnoreCase);
            MatchLogCap = Regex.Match(configInput, @"logCap=\w+", RegexOptions.IgnoreCase);
            MatchTeleportRequest = Regex.Match(configInput, @"teleportRequest=\w+", RegexOptions.IgnoreCase);
            MatchRequestDelay = Regex.Match(configInput, @"requestDelay=\w+", RegexOptions.IgnoreCase);
            MatchWarpDelay = Regex.Match(configInput, @"warpDelay=\w+", RegexOptions.IgnoreCase);
            MatchRequestCooldownType = Regex.Match(configInput, @"requestCooldownType=\w+", RegexOptions.IgnoreCase);
            MatchRequestCooldown = Regex.Match(configInput, @"requestCooldown=\w+", RegexOptions.IgnoreCase);
            MatchDenyRequestWarzone = Regex.Match(configInput, @"denyRequestWarzone=\w+", RegexOptions.IgnoreCase);
            MatchHost = Regex.Match(configInput, @"host=\w+", RegexOptions.IgnoreCase);
            MatchPort = Regex.Match(configInput, @"port=\w+", RegexOptions.IgnoreCase);
            MatchDatabase = Regex.Match(configInput, @"database=\w+", RegexOptions.IgnoreCase);
            MatchUser = Regex.Match(configInput, @"user=\w+", RegexOptions.IgnoreCase);
            MatchPass = Regex.Match(configInput, @"pass=\w+", RegexOptions.IgnoreCase);
            MatchCommandInheritance = Regex.Match(configInput, @"inheritCommands=\w+", RegexOptions.IgnoreCase);
            MatchKitInheritance = Regex.Match(configInput, @"inheritKits=\w+", RegexOptions.IgnoreCase);
            MatchWarpInheritance = Regex.Match(configInput, @"inheritWarps=\w+", RegexOptions.IgnoreCase);
            MatchFriendlyFire = Regex.Match(configInput, @"friendlyFire=\w+", RegexOptions.IgnoreCase);
            MatchAlliedFire = Regex.Match(configInput, @"alliedFire=\w+", RegexOptions.IgnoreCase);
            MatchNeutralDamage = Regex.Match(configInput, @"neutralDamage=\w+", RegexOptions.IgnoreCase);
            MatchWarDamage = Regex.Match(configInput, @"warDamage=\w+", RegexOptions.IgnoreCase);
            MatchWarFriendlyDamage = Regex.Match(configInput, @"warFriendlyDamage=\w+", RegexOptions.IgnoreCase);
            MatchWarAllyDamage = Regex.Match(configInput, @"warAllyDamage=\w+", RegexOptions.IgnoreCase);

            enabledWhitelist = MatchEnabledWhitelist.ToString().Split('=')[1];
            MySQL = MatchMySQL.ToString().Split('=')[1];
            useSteamGroup = MatchUseSteamGroup.ToString().Split('=')[1];
            steamGroup = MatchSteamGroup.ToString().Split('=')[1].Replace("\r", "");
            autoRefresh = MatchAutoRefresh.ToString().Split('=')[1];
            refreshInterval = MatchRefreshInterval.ToString().Split('=')[1];
            whitelistToMembers = MatchUseAsMembers.ToString().Split('=')[1];
            whitelistKickCMD = MatchWhitelistKickCMD.ToString().Split('=')[1].Replace("\r", "");
            whitelistKickJoin = MatchWhitelistKickJoin.ToString().Split('=')[1].Replace("\r", "");
            whitelistCheckGood = MatchWhitelistGood.ToString().Split('=')[1].Replace("\r", "");
            whitelistCheckBad = MatchWhitelistBad.ToString().Split('=')[1].Replace("\r", "");
            announceDrops = MatchAnnounceDrops.ToString().Split('=')[1];
            startTime = MatchStartTime.ToString().Split('=')[1];
            timeScale = MatchTimeScale.ToString().Split('=')[1];
            freezeTime = MatchFreezeTime.ToString().Split('=')[1];
            fallDamage = MatchFallDamage.ToString().Split('=')[1];
            voiceDistance = MatchVoiceDistance.ToString().Split('=')[1];
            enableRepair = MatchEnableRepair.ToString().Split('=')[1];
            forceNudity = MatchForceNudity.ToString().Split('=')[1];
            directChat = MatchDirectChat.ToString().Split('=')[1];
            globalChat = MatchGlobalChat.ToString().Split('=')[1];
            removeTag = MatchRemoveTag.ToString().Split('=')[1];
            defaultChat = MatchDefaultChat.ToString().Split('=')[1];
            directDistance = MatchDirectDistance.ToString().Split('=')[1];
            allowedChars = MatchAllowedChars.ToString().Substring(MatchAllowedChars.ToString().IndexOf('=') + 1).Replace("\r", "");
            restrictChars = MatchRestrictChars.ToString().Split('=')[1];
            minimumNameCount = MatchMinimumNameCount.ToString().Split('=')[1];
            maximumNameCount = MatchMaximumNameCount.ToString().Split('=')[1];
            kickDuplicate = MatchKickDuplicate.ToString().Split('=')[1];
            lowerAuthority = MatchLowerAuthority.ToString().Split('=')[1];
            illegalWords = MatchIllegalWords.ToString().Split('=')[1].Replace("\r", "");
            censorship = MatchCensorship.ToString().Split('=')[1];
            botName = MatchBotName.ToString().Split('=')[1].Replace("\r", "");
            joinMessage = MatchJoinMessage.ToString().Split('=')[1].Replace("\r", "");
            enableJoin = MatchEnableJoin.ToString().Split('=')[1];
            leaveMessage = MatchLeaveMessage.ToString().Split('=')[1].Replace("\r", "");
            enableLeave = MatchEnableLeave.ToString().Split('=')[1];
            suicideMessage = MatchSuicideMessage.ToString().Split('=')[1].Replace("\r", "");    
            enableSuicide = MatchEnableSuicide.ToString().Split('=')[1];
            murderMessage = MatchMurderMessage.ToString().Split('=')[1].Replace("\r", "");
            murderMessageUnknown = MatchMurderMessageUnknown.ToString().Split('=')[1].Replace("\r", "");
            enableMurder = MatchEnableMurder.ToString().Split('=')[1];
            deathMessage = MatchDeathMessage.ToString().Split('=')[1].Replace("\r", "");
            enableDeath = MatchEnableDeath.ToString().Split('=')[1];
            unknownCommand = MatchUnknownCommand.ToString().Split('=')[1];
            nextToName = MatchNextToName.ToString().Split('=')[1];
            removePrefix = MatchRemovePrefix.ToString().Split('=')[1];
            logPluginChat = MatchPluginChat.ToString().Split('=')[1];
            chatLogCap = MatchChatLogCap.ToString().Split('=')[1];
            logCap = MatchLogCap.ToString().Split('=')[1];
            teleportRequest = MatchTeleportRequest.ToString().Split('=')[1];
            requestDelay = MatchRequestDelay.ToString().Split('=')[1];
            warpDelay = MatchWarpDelay.ToString().Split('=')[1];
            requestCooldownType = MatchRequestCooldownType.ToString().Split('=')[1];
            requestCooldown = MatchRequestCooldown.ToString().Split('=')[1];
            denyRequestWarzone = MatchDenyRequestWarzone.ToString().Split('=')[1];
            //host = MatchHost.ToString().Split('=')[1];
            //port = MatchPort.ToString().Split('=')[1];
            //database = MatchDatabase.ToString().Split('=')[1];
            //user = MatchUser.ToString().Split('=')[1];
            //pass = MatchPass.ToString().Split('=')[1];
            inheritCommands = MatchCommandInheritance.ToString().Split('=')[1];
            inheritKits = MatchKitInheritance.ToString().Split('=')[1];
            inheritWarps = MatchWarpInheritance.ToString().Split('=')[1];
            friendlyFire = MatchFriendlyFire.ToString().Split('=')[1];
            alliedFire = MatchAlliedFire.ToString().Split('=')[1];
            neutralDamage = MatchNeutralDamage.ToString().Split('=')[1];
            warDamage = MatchWarDamage.ToString().Split('=')[1];
            warFriendlyDamage = MatchWarFriendlyDamage.ToString().Split('=')[1];
            warAllyDamage = MatchWarAllyDamage.ToString().Split('=')[1];
        }
    }
}