/**
 * @file: Broadcast.cs
 * @author: Team Cerionn (https://github.com/Team-Cerionn)

 * @description: Broadcast class for Rust Essentials
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facepunch;
using LeatherLoader;
using UnityEngine;
using uLink;
using Rust;
using RustProto;
using System.Threading;

namespace RustEssentials.Util
{
    public class Broadcast
    {
        public static void broadcastCustomTo(uLink.NetworkPlayer sender, string name, string message)
        {
            ConsoleNetworker.SendClientCommand(sender, "chat.add \"" + name + "\" \"" + Vars.replaceQuotes(message) + "\"");
        }

        public static void broadcastCustom(string name, string message)
        {
            ConsoleNetworker.Broadcast("chat.add \"" + name + "\" \"" + Vars.replaceQuotes(message) + "\"");
        }

        public static void broadcastTo(uLink.NetworkPlayer player, string message, bool b)
        {
            PlayerClient playerClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == player);
            ConsoleNetworker.SendClientCommand(player, "chat.add \"[PM] " + Vars.botName + "\" \"" + Vars.replaceQuotes(message) + "\"");
        }

        public static void broadcastTo(uLink.NetworkPlayer player, string message)
        {
            PlayerClient playerClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == player);
            Vars.conLog.Chat("<TO " + playerClient.userName + "> " + Vars.botName + ": " + message);
            ConsoleNetworker.SendClientCommand(player, "chat.add \"[PM] " + Vars.botName + "\" \"" + Vars.replaceQuotes(message) + "\"");
        }

        public static void broadcastJoinLeave(string message)
        {
            ConsoleNetworker.Broadcast("chat.add \"" + Vars.botName + "\" \"" + Vars.replaceQuotes(message) + "\"");
        }

        public static void broadcastAll(string message)
        {
            Vars.conLog.Chat("<BROADCAST ALL> " + Vars.botName + ": " + message);
            ConsoleNetworker.Broadcast("chat.add \"" + Vars.botName + "\" \"" + Vars.replaceQuotes(message) + "\"");
        }

        public static void noticeTo(uLink.NetworkPlayer sender, string icon, string message, int duration = 2)
        {
            Vars.conLog.Chat("<NOTICE> " + Vars.botName + ": " + message);
            ConsoleNetworker.SendClientCommand(sender, "notice.popup " + duration + " \"" + icon + "\" \"" + Vars.replaceQuotes(message) + "\"");
        }

        public static void noticeAll(string icon, string message, int duration = 2)
        {
            Vars.conLog.Chat("<NOTICE ALL> " + Vars.botName + ": " + message);
            ConsoleNetworker.Broadcast("notice.popup " + duration + " \"" + icon + "\" \"" + Vars.replaceQuotes(message) + "\"");
        }

        public static void sideNoticeTo(uLink.NetworkPlayer player, string message)
        {
            Notice.Inventory(player, message);
        }

        public static void sideNoticeAll(string icon, string message)
        {
            ConsoleNetworker.Broadcast("notice.inventory \"" + Vars.replaceQuotes(message) + "\"");
        }

        public static void reply(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string message = "";
                int curIndex = 0;
                List<string> messageList = new List<string>();
                foreach (string s in args)
                {
                    if (curIndex > 0)
                    {
                        messageList.Add(s);
                    }
                    curIndex++;
                }
                message = string.Join(" ", messageList.ToArray());

                if (Vars.latestPM.ContainsKey(senderClient))
                {
                    PlayerClient targetClient = Vars.latestPM[senderClient];

                    if (targetClient.netPlayer.isConnected)
                    {
                        message = Vars.replaceQuotes(message);
                        Vars.conLog.Chat("<PM FROM> " + senderClient.userName + ": " + message);
                        Vars.conLog.Chat("<PM TO> " + targetClient.userName + ": " + message);
                        string namePrefixTo = (Vars.nextToName ? "[PM to] " : "");
                        string msgPrefixTo = (!Vars.nextToName ? "[PM to] " : "");
                        string namePrefixFrom = (Vars.nextToName ? "[PM from] " : "");
                        string msgPrefixFrom = (!Vars.nextToName ? "[PM from] " : "");
                        ConsoleNetworker.SendClientCommand(senderClient.netPlayer, "chat.add \"" + namePrefixTo + targetClient.userName + "\" \"" + msgPrefixTo + message + "\"");
                        ConsoleNetworker.SendClientCommand(targetClient.netPlayer, "chat.add \"" + namePrefixFrom + senderClient.userName + "\" \"" + msgPrefixFrom + message + "\"");
                    }
                    else
                    {
                        broadcastTo(senderClient.netPlayer, "Player \"" + targetClient.userName + "\" is not online.");
                    }
                }
                else
                {
                    broadcastTo(senderClient.netPlayer, "You do not have anyone to reply to.");
                }
            }
        }

        public static void sendPM(string sender, string[] args)
        {
            PlayerClient senderClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == sender);
            if (args.Length > 2)
            {
                bool hadQuote = false;
                int lastIndex = 0;
                string playerName = "";
                List<string> playerNameList = new List<string>();
                if (args[1].Contains("\""))
                {
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
                    playerName = string.Join(" ", playerNameList.ToArray());
                }
                else
                {
                    playerName = args[1];
                    lastIndex = 1;
                }
                playerName = playerName.Replace("\"", "").Trim();

                if (playerName != senderClient.userName)
                {
                    string message = "";
                    int curIndex = 0;
                    List<string> messageList = new List<string>();
                    foreach (string s in args)
                    {
                        if (curIndex > lastIndex)
                        {
                            messageList.Add(s);
                        }
                        curIndex++;
                    }
                    message = string.Join(" ", messageList.ToArray());
                    if (playerName != null && message != null)
                    {
                        PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(playerName));
                        if (possibleTargets.Count() == 0)
                            Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain \"" + playerName + "\".");
                        else if (possibleTargets.Count() > 1)
                            Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain \"" + playerName + "\".");
                        else
                        {
                            PlayerClient targetClient = possibleTargets[0];
                            message = Vars.replaceQuotes(message);
                            Vars.conLog.Chat("<PM FROM> " + senderClient.userName + ": " + message);
                            Vars.conLog.Chat("<PM TO> " + targetClient.userName + ": " + message);
                            string namePrefixTo = (Vars.nextToName ? "[PM to] " : "");
                            string msgPrefixTo = (!Vars.nextToName ? "[PM to] " : "");
                            string namePrefixFrom = (Vars.nextToName ? "[PM from] " : "");
                            string msgPrefixFrom = (!Vars.nextToName ? "[PM from] " : "");
                            ConsoleNetworker.SendClientCommand(senderClient.netPlayer, "chat.add \"" + namePrefixTo + targetClient.userName + "\" \"" + msgPrefixTo + message + "\"");
                            ConsoleNetworker.SendClientCommand(targetClient.netPlayer, "chat.add \"" + namePrefixFrom + senderClient.userName + "\" \"" + msgPrefixFrom + message + "\"");

                            if (Vars.latestPM.ContainsKey(senderClient))
                                Vars.latestPM[senderClient] = targetClient;
                            else
                                Vars.latestPM.Add(senderClient, targetClient);

                            if (Vars.latestPM.ContainsKey(targetClient))
                                Vars.latestPM[targetClient] = senderClient;
                            else
                                Vars.latestPM.Add(targetClient, senderClient);
                        }
                    }
                }
                else
                {
                    broadcastTo(senderClient.netPlayer, "You can't PM yourself!");
                }
            }
        }

        public static void sendPlayers(uLink.NetworkPlayer sender)
        {
            broadcastTo(sender, Vars.AllPlayerClients.Count + "/" + NetCull.maxConnections + " players currently connected.");
        }

        public static void help(uLink.NetworkPlayer sender, string[] args)
        {
            PlayerClient playerClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == sender);
            if (args.Length == 1)
            {
                string rank = Vars.findRank(playerClient.userID.ToString());

                broadcastTo(sender, "Do \"/help <command name without />\" to view syntax.", true);
                broadcastTo(sender, "Available commands:", true);
                Vars.listCommands(rank, playerClient);
            }
            else if (args.Length > 1)
            {
                string command = args[1];

                switch (command)
                {
                    case "vanish":
                        broadcastTo(sender, "/vanish: Toggles the vanish tool. Syntax: /vanish {on/off}");
                        break;
                    case "hide":
                        broadcastTo(sender, "/hide: Toggles the hider tool. Syntax: /hide {on/off}");
                        break;
                    case "owner":
                        broadcastTo(sender, "/owner: Toggles the ownership tool. Syntax: /owner {on/off}");
                        break;
                    case "remove":
                        broadcastTo(sender, "/remove: Toggles the remover tool. Syntax: /remove {on/off}");
                        break;
                    case "f":
                        broadcastTo(sender, "/f: Manages faction actions. Syntax: /f {help}");
                        break;
                    case "r":
                        broadcastTo(sender, "/r: Replies to your last sent or received PM. Syntax: /r *message*");
                        break;
                    case "rules":
                        broadcastTo(sender, "/rules: Lists the server rules. Syntax: /rules");
                        break;
                    case "players":
                        broadcastTo(sender, "/players: Lists all connected players. Syntax: /players");
                        break;
                    case "warps":
                        broadcastTo(sender, "/warps: Lists warps available to you. Syntax: /warps");
                        break;
                    case "kits":
                        broadcastTo(sender, "/kits: Lists kits available to you. Syntax: /kits");
                        break;
                    case "feed":
                        broadcastTo(sender, "/feed: Feeds the specified player or self. Syntax: /feed [player name]");
                        break;
                    case "heal":
                        broadcastTo(sender, "/heal: Heals the specified player or self. Syntax: /heal [player name]");
                        break;
                    case "access":
                        broadcastTo(sender, "/access: Gives the sender access to all doors. Syntax: /access {on/off}");
                        break;
                    case "version":
                        broadcastTo(sender, "/version: Returns the current running version of Rust Essentials. Syntax: /version");
                        break;
                    case "save":
                        broadcastTo(sender, "/save: Saves all world data of the server. Syntax: /save");
                        break;
                    case "saypop":
                        broadcastTo(sender, "/saypop: Says a drop down message through the plugin. Syntax: /say [message]");
                        break;
                    case "tppos":
                        broadcastTo(sender, "/tppos: Teleports a user to the said vector. Syntax: /tppos [x] [y] [z]");
                        break;
                    case "tphere":
                        broadcastTo(sender, "/tphere: Teleports a user to you. Syntax: /tphere *name*");
                        break;
                    case "tpaccept":
                        broadcastTo(sender, "/tpa: Accepts a user's teleport request. Syntax: /tpaccept [name]");
                        break;
                    case "tpdeny":
                        broadcastTo(sender, "/tpdeny: Denies a user's teleport request (or all requests). Syntax: /tpdeny [name/all]");
                        break;
                    case "tpa":
                        broadcastTo(sender, "/tpa: Sends a teleport request to the target user. Syntax: /tpa [name]");
                        break;
                    case "tp":
                        broadcastTo(sender, "/tp: Teleports the sender, or someone else, to a target user. Syntax: /tp [name] or /tp [name1] [name2]");
                        break;
                    case "history":
                        broadcastTo(sender, "/history: Shows the last # lines of chat history (1-50). Syntax: /history [1-50]");
                        break;
                    case "unmute":
                        broadcastTo(sender, "/unmute: Unmutes a player on global chat. Syntax: /unmute [name]");
                        break;
                    case "mute":
                        broadcastTo(sender, "/mute: Mutes a player on global chat. Syntax: /mute [name]");
                        break;
                    case "stop":
                        broadcastTo(sender, "/stop: Saves, deactivates, and effectively stops the server. Syntax: /stop");
                        break;
                    case "say":
                        broadcastTo(sender, "/say: Says a message through the plugin. Syntax: /say [message]");
                        break;
                    case "chan":
                        broadcastTo(sender, "/chan: Switches text communication channels. Syntax: /chan {g/global/d/direct/f/faction}");
                        break;
                    case "kickall":
                        broadcastTo(sender, "/kickall: Kicks all players from the server except the command executor. Syntax: /kickall");
                        break;
                    case "whitelist":
                        broadcastTo(sender, "/whitelist: Manages the whitelist. UID only for add/rem. Syntax: /whitelist {add/rem/on/off/kick/check} [UID]");
                        break;
                    case "reload":
                        broadcastTo(sender, "/reload: Reloads the specified config file. Syntax: /reload {config/whitelist/ranks/commands/kits/motd/bans/all}");
                        break;
                    case "unban":
                        broadcastTo(sender, "/unban: Unbans the specified player. Syntax: /unban \"full name or UID\"");
                        break;
                    case "ban":
                        broadcastTo(sender, "/ban: Bans the specified player. Syntax: /ban \"player or UID\" [reason]");
                        break;
                    case "bane":
                        broadcastTo(sender, "/bane: Bans the specified player by the exact name. Syntax: /bane \"player\" [reason]");
                        break;
                    case "kick":
                        broadcastTo(sender, "/kick: Kicks the specified player. Syntax: /kick \"player\" [reason]");
                        break;
                    case "kicke":
                        broadcastTo(sender, "/kicke: Kicks the specified player by the exact name. Syntax: /kicke \"player\" [reason]");
                        break;
                    case "timescale":
                        broadcastTo(sender, "/timescale: Sets or gets the server time scale. Syntax: /timescale [#.##]");
                        break;
                    case "time":
                        broadcastTo(sender, "/time: Sets or gets the server time. Syntax: /time [0-24/day/night/(un)freeze]");
                        break;
                    case "join":
                        broadcastTo(sender, "/join: Emulate the joining of a fake user. Syntax: /join *player*");
                        break;
                    case "leave":
                        broadcastTo(sender, "/leave: Emulate the leaving of a fake user. Syntax: /leave *player*");
                        break;
                    case "pos":
                        broadcastTo(sender, "/pos: Returns the players position. Syntax: /pos");
                        break;
                    case "i":
                        broadcastTo(sender, "/i: Gives the player the requested item. Syntax: /i <\"item name\"/id> [amount]");
                        break;
                    case "give":
                        broadcastTo(sender, "/give: Gives a player an item. Syntax: /give \"player\" \"item name\" [amount]");
                        break;
                    case "giveall":
                        broadcastTo(sender, "/giveall: Gives the amount of that item to all players. Syntax: /giveall \"item\" [amount]");
                        break;
                    case "random":
                        broadcastTo(sender, "/random: Starts the random giveaway for the specified item, amount, and winners. Syntax: /random \"item\" [amount] [winners]");
                        break;
                    case "warp":
                        broadcastTo(sender, "/warp: Teleports you the the named location. Syntax: /warp *warp name*");
                        break;
                    case "kit":
                        broadcastTo(sender, "/kit: Gives the specified kit. Syntax: /kit <kit>");
                        break;
                    case "airdrop":
                        broadcastTo(sender, "/airdrop: Spawns an airdrop at a random or specified players position. Syntax: /airdrop [*player*]");
                        break;
                    case "share":
                        broadcastTo(sender, "/share: Shares doors and gates with other players. Syntax: /share [player name] (CASE SENSITIVE)");
                        break;
                    case "unshare":
                        broadcastTo(sender, "/unshare: Unshares doors and gates with other players. Syntax: /unshare [player name/all] (CASE SENSITIVE)");
                        break;
                    case "pm":
                        broadcastTo(sender, "/pm: Sends a private message. Syntax: /pm \"name\" *message*");
                        break;
                    case "online":
                        broadcastTo(sender, "/online: Reports the number of online players. Syntax: /online");
                        break;
                    case "uid":
                        broadcastTo(sender, "/uid: Displays your Steam UID. Syntax: /uid");
                        break;
                    case "god":
                        broadcastTo(sender, "/god: Makes a player or self invulnerable to damage. Syntax: /god [name]");
                        break;
                    case "ungod":
                        broadcastTo(sender, "/ungod: Makes a player or self vulnerable to damage. Syntax: /ungod [name]");
                        break;
                    case "fall":
                        broadcastTo(sender, "/fall: Toggles server-wide fall damage Syntax: /fall [on/off]");
                        break;
                    case "kill":
                        broadcastTo(sender, "/kill: Kills the specified player. Syntax: /kill *player*");
                        break;
                    case "help":
                        broadcastTo(sender, "/help: Displays help for commands. Syntax: /help *command*");
                        break;
                    default:
                        broadcastTo(sender, "Unable to find help for the command: \"" + command + "\".");
                        break;
                }
            }
        }
    }
}
