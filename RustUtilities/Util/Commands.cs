/**
 * @file: Commands.cs
 * @author: Team Cerionn (https://github.com/Team-Cerionn)
 * @version: 1.0.0.0
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
            PlayerClient playerClient = Array.Find(PlayerClient.All.ToArray(), (PlayerClient pc) => pc.netPlayer == player);
            Character playerChar;
            Character.FindByUser(playerClient.userID, out playerChar);

            string[] commandArgs = message.Split(' ');
            string command = commandArgs[0];

            string sendMSG = "";
            int curIndex = 0;
            foreach (string s in commandArgs)
            {
                if (curIndex > 1)
                {
                    sendMSG += s + " ";
                }
                curIndex++;
            }

            if (Vars.totalCommands.Contains(command))
            {
                if (Vars.enabledCommands[Vars.findRank(playerClient.userID.ToString())].Contains(command))
                {
                    if (commandArgs.Count() == 2 && commandArgs[0] + commandArgs[1] == "/whitelistcheck")
                        Vars.whitelistCheck(playerClient);
                    else
                    {
                        switch (command)
                        {
                            case "/rules":
                                Vars.showRules(playerClient);
                                break;
                            case "/players":
                                Vars.showPlayers(playerClient);
                                break;
                            case "/kits":
                                Vars.showKits(playerClient);
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
                                Vars.banPlayer(playerClient, commandArgs);
                                break;
                            case "/kick":
                                Vars.kickPlayer(playerClient, commandArgs, false);
                                break;
                            case "/timescale":
                                Vars.setScale(playerClient, commandArgs);
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
                            case "/kit":
                                Vars.giveKit(playerClient, commandArgs, message);
                                break;
                            case "/airdrop":
                                Vars.airdrop(player, commandArgs);
                                break;
                            case "/share":
                                Share.shareWith(playerClient, commandArgs); // Needs work
                                break;
                            case "/unshare":
                                Share.unshareWith(playerClient, commandArgs); // Needs work
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
                                Vars.setFall(player, commandArgs); // Needs work
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
