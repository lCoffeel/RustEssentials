/**
 * @file: Commands.cs
 * @author: Team Cerionn (https://github.com/Team-Cerionn)

 * @description: Commands class for Rust Essentials
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RustEssentials.Util
{
    public class Commands : ConsoleSystem
    {
        public static void CMD(Arg arg)
        {
            string playerName = arg.argUser.user.Displayname;
            string message = arg.GetString(0, "text").Trim();
            uLink.NetworkPlayer player = arg.argUser.networkPlayer;
            PlayerClient playerClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == player);
            Character playerChar;
            Character.FindByUser(playerClient.userID, out playerChar);

            executeCMD(playerName, message, player, playerClient, playerChar);
        }

        public static void executeCMDServer(string message)
        {
            string[] commandArgs = message.Split(' ');
            string command = commandArgs[0];

            switch (command)
            {
                case "/save":
                    Vars.saveServer();
                    break;
                case "/saypop":
                    Vars.saypop(commandArgs);
                    break;
                case "/stop":
                    Vars.stopServer();
                    break;
                case "/kickall":
                    Vars.kickAllServer();
                    break;
                case "/reload":
                    Vars.reloadFileServer(commandArgs);
                    break;
                case "/daylength":
                    Vars.setDayLengthServer(commandArgs);
                    break;
                case "/nightlength":
                    Vars.setNightLengthServer(commandArgs);
                    break;
                case "/time":
                    Vars.setTimeServer(commandArgs);
                    break;
                case "/join":
                    Vars.fakeJoinServer(commandArgs);
                    break;
                case "/leave":
                    Vars.fakeLeaveServer(commandArgs);
                    break;
                case "/giveall":
                    Vars.giveAllServer(commandArgs);
                    break;
                case "/random":
                    Vars.giveRandomServer(commandArgs);
                    break;
                case "/airdrop":
                    Vars.airdropServer(commandArgs);
                    break;
            }
        }

        public static void executeCMD(string playerName, string message, uLink.NetworkPlayer player, PlayerClient playerClient, Character playerChar)
        {
            string[] commandArgs = message.Split(' ');
            string command = commandArgs[0];

            if (Vars.totalCommands.Contains(command) || (playerChar.netUser.CanAdmin() && commandArgs[0] == "/reload"))
            {
                if (Vars.enabledCommands[Vars.findRank(playerClient.userID.ToString())].Contains(command) || (playerChar.netUser.CanAdmin() && commandArgs[0] == "/reload"))
                {
                    if (Vars.enabledCommands[Vars.findRank(playerClient.userID.ToString())].Contains("/whitelist check") && message.StartsWith("/whitelist check"))
                        Vars.whitelistCheck(playerClient);
                    else if (Vars.enabledCommands[Vars.findRank(playerClient.userID.ToString())].Contains("/f safezone") && message.StartsWith("/f safezone"))
                        Vars.manageZones(playerClient, commandArgs, true);
                    else if (Vars.enabledCommands[Vars.findRank(playerClient.userID.ToString())].Contains("/f warzone") && message.StartsWith("/f warzone"))
                        Vars.manageZones(playerClient, commandArgs, false);
                    else if (Vars.enabledCommands[Vars.findRank(playerClient.userID.ToString())].Contains("/f build") && message.StartsWith("/f build"))
                        Vars.handleFactions(playerClient, commandArgs);
                    else
                    {
                        switch (command)
                        {
                            case "/client":
                                Vars.grabClient(playerClient, commandArgs);
                                break;
                            case "/clearinv":
                                Vars.clearPlayer(playerClient, commandArgs);
                                break;
                            case "/craft":
                                Vars.craftTool(playerClient, commandArgs);
                                break;
                            case "/vanish":
                                Vars.vanishTool(playerClient, commandArgs);
                                break;
                            case "/hide":
                                Vars.hideTool(playerClient, commandArgs);
                                break;
                            case "/dist":
                                Vars.showDistance(playerClient, commandArgs);
                                break;
                            case "/owner":
                                Vars.showOwner(playerClient, commandArgs);
                                break;
                            case "/removeall":
                                Vars.removerAllTool(playerClient, commandArgs);
                                break;
                            case "/remove":
                                Vars.removerTool(playerClient, commandArgs);
                                break;
                            case "/f":
                                Vars.handleFactions(playerClient, commandArgs);
                                break;
                            case "/r":
                                Broadcast.reply(playerClient, commandArgs);
                                break;
                            case "/rules":
                                Vars.showRules(playerClient);
                                break;
                            case "/players":
                                Vars.showPlayers(playerClient);
                                break;
                            case "/warps":
                                Vars.showWarps(playerClient);
                                break;
                            case "/kits":
                                Vars.showKits(playerClient);
                                break;
                            case "/feed":
                                Vars.feedPlayer(player, playerName, commandArgs);
                                break;
                            case "/heal":
                                Vars.healPlayer(player, playerName, commandArgs);
                                break;
                            case "/access":
                                Vars.giveAccess(playerClient, commandArgs);
                                break;
                            case "/version":
                                Broadcast.broadcastTo(player, "The server is running Rust Essentials v" + Vars.currentVersion + ".");
                                break;
                            case "/save":
                                Vars.save(playerClient);
                                break;
                            case "/saypop":
                                Vars.saypop(commandArgs);
                                break;
                            case "/tppos":
                                Vars.teleportPos(playerClient, commandArgs);
                                break;
                            case "/tphere":
                                Vars.teleportHere(playerClient, commandArgs);
                                break;
                            case "/tpaccept":
                                Vars.teleportAccept(playerClient, commandArgs);
                                break;
                            case "/tpdeny":
                                Vars.teleportDeny(playerClient, commandArgs);
                                break;
                            case "/tpa":
                                Vars.teleportRequest(playerClient, commandArgs);
                                break;
                            case "/tp":
                                Vars.teleport(playerClient, commandArgs);
                                break;
                            case "/history":
                                Vars.showHistory(playerClient, commandArgs);
                                break;
                            case "/unmute":
                                Vars.mute(playerClient, commandArgs, false);
                                break;
                            case "/mute":
                                Vars.mute(playerClient, commandArgs, true);
                                break;
                            case "/stop":
                                Vars.stopServer();
                                break;
                            case "/say":
                                Vars.say(commandArgs);
                                break;
                            case "/chan":
                                Vars.channels(playerClient, commandArgs);
                                break;
                            case "/kickall":
                                Vars.kickAll(playerClient);
                                break;
                            case "/whitelist":
                                Vars.whitelistPlayer(player, commandArgs);
                                break;
                            case "/reload":
                                Vars.reloadFile(player, commandArgs);
                                break;
                            case "/unban":
                                Vars.unbanPlayer(playerClient, commandArgs);
                                break;
                            case "/ban":
                                Vars.banPlayer(playerClient, commandArgs, false);
                                break;
                            case "/bane":
                                Vars.banPlayer(playerClient, commandArgs, true);
                                break;
                            case "/kick":
                                Vars.kickPlayer(playerClient, commandArgs, false);
                                break;
                            case "/kicke":
                                Vars.kickPlayer(playerClient, commandArgs, true);
                                break;
                            case "/daylength":
                                Vars.setDayLength(playerClient, commandArgs);
                                break;
                            case "/nightlength":
                                Vars.setNightLength(playerClient, commandArgs);
                                break;
                            case "/time":
                                Vars.setTime(playerClient, commandArgs);
                                break;
                            case "/join":
                                Vars.fakeJoin(playerClient, commandArgs);
                                break;
                            case "/leave":
                                Vars.fakeLeave(playerClient, commandArgs);
                                break;
                            case "/pos":
                                Vars.getPlayerPos(playerClient);
                                break;
                            case "/i":
                                Vars.createItem(playerClient, playerClient, commandArgs, message, true);
                                break;
                            case "/give":
                                Vars.createItem(playerClient, commandArgs, message);
                                break;
                            case "/giveall":
                                Vars.giveAll(playerClient, commandArgs);
                                break;
                            case "/random":
                                Vars.giveRandom(playerClient, commandArgs);
                                break;
                            case "/warp":
                                Vars.warpPlayer(playerClient, commandArgs);
                                break;
                            case "/kit":
                                Vars.giveKit(playerClient, commandArgs, message);
                                break;
                            case "/airdrop":
                                Vars.airdrop(player, commandArgs);
                                break;
                            case "/share":
                                Share.shareWith(playerClient, commandArgs);
                                break;
                            case "/unshare":
                                Share.unshareWith(playerClient, commandArgs);
                                break;
                            case "/pm":
                                Broadcast.sendPM(playerName, commandArgs);
                                break;
                            case "/online":
                                Broadcast.sendPlayers(player);
                                break;
                            case "/uid":
                                Vars.grabUID(playerClient, commandArgs);
                                break;
                            case "/god":
                                Vars.godMode(player, playerName, commandArgs, true);
                                break;
                            case "/ungod":
                                Vars.godMode(player, playerName, commandArgs, false);
                                break;
                            case "/fall":
                                Vars.setFall(player, commandArgs);
                                break;
                            case "/kill":
                                Vars.killTarget(player, commandArgs);
                                break;
                            case "/help":
                                Broadcast.help(player, commandArgs);
                                break;
                        }
                    }
                }
                else
                {
                    Broadcast.noticeTo(player, ":(", "You do not have permission to do this.");
                }
            }
            else
            {
                if (Vars.unknownCommand)
                    Broadcast.broadcastTo(player, "Unknown command \"" + command + "\"!");
            }
        }
    }
}
