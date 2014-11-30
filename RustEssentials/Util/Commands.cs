using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RustEssentials.Util
{
    public class Commands : ConsoleSystem
    {
        public static bool CMD(Arg arg)
        {
            string playerName = arg.argUser.user.Displayname;
            string message = arg.GetString(0, "text").Trim();
            uLink.NetworkPlayer networkPlayer = arg.argUser.networkPlayer;
            PlayerClient playerClient = arg.playerClient();
            Character playerChar = arg.playerCharacter();

            if (Vars.runningAPI)
            {
                try
                {
                    Type Command = Vars.API.GetType("RustEssentialsAPI.Command");
                    MethodInfo Exists = Command.GetMethod("Exists");
                    MethodInfo Execute = Command.GetMethod("Execute", BindingFlags.NonPublic | BindingFlags.Static);
                    string[] commandArgs = message.Split(' ');
                    string command = commandArgs[0];

                    bool containsCMD = (bool)Exists.Invoke(null, new object[] { command });

                    if (containsCMD)
                    {
                        if (Vars.totalCommands.Contains(command))
                        {
                            string rankToUse = Vars.findRank(playerClient.userID);
                            if (!Vars.enabledCommands.ContainsKey(rankToUse))
                                rankToUse = Vars.defaultRank;
                            if (Vars.enabledCommands[rankToUse].Contains(command))
                            {
                                Execute.Invoke(null, new object[] { new object[] { command, playerName, message, networkPlayer, playerClient, playerChar } });
                                return true;
                            }
                            else
                            {
                                if (Vars.unknownCommand)
                                    Broadcast.broadcastTo(networkPlayer, "Unknown command \"" + command + "\"!");
                                return false;
                            }
                        }
                        else
                        {
                            if (Vars.unknownCommand)
                                Broadcast.broadcastTo(networkPlayer, "Unknown command \"" + command + "\"!");
                            return false;
                        }
                    }
                    else
                        return executeCMD(playerName, message, networkPlayer, playerClient, playerChar);
                }
                catch (Exception ex)
                {
                    Vars.conLog.Error("CMD: " + ex.ToString());
                    return false;
                }
            }
            else
                return executeCMD(playerName, message, networkPlayer, playerClient, playerChar);
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
                    Items.giveAllServer(commandArgs);
                    break;
                case "/random":
                    Items.giveRandomServer(commandArgs);
                    break;
                case "/airdrop":
                    Vars.airdropServer();
                    break;
            }
        }

        public static bool executeCMD(string playerName, string message, uLink.NetworkPlayer player, PlayerClient playerClient, Character playerChar)
        {
            string[] commandArgs = message.Split(' ');
            string command = commandArgs[0].ToLower();

            if (Vars.totalCommands.Contains(command) || (playerChar.netUser != null && playerChar.netUser.CanAdmin() && commandArgs[0] == "/reload"))
            {
                string rankToUse = Vars.findRank(playerClient.userID);
                if (!Vars.enabledCommands.ContainsKey(rankToUse))
                    rankToUse = Vars.defaultRank;

                if (Vars.enabledCommands[rankToUse].Contains(command) || (playerChar.netUser.CanAdmin() && commandArgs[0] == "/reload"))
                {
                    if (Vars.enabledCommands[rankToUse].Contains("/whitelist check") && message.StartsWith("/whitelist check"))
                        Vars.whitelistCheck(playerClient);
                    else if (Vars.enabledCommands[rankToUse].Contains("/f safezone") && message.StartsWith("/f safezone"))
                        Factions.manageZones(playerClient, commandArgs, true);
                    else if (Vars.enabledCommands[rankToUse].Contains("/f warzone") && message.StartsWith("/f warzone"))
                        Factions.manageZones(playerClient, commandArgs, false);
                    //else if (Vars.enabledCommands[Vars.findRank(playerClient.userID)].Contains("/f build") && message.StartsWith("/f build"))
                    //    Factions.handleFactions(playerClient, commandArgs);
                    else
                    {
                        switch (command)
                        {
                            case "/sethome":
                                Homes.setHome(playerClient, commandArgs);
                                break;
                            case "/home":
                                Homes.teleportHome(playerClient, commandArgs);
                                break;
                            case "/homes":
                                Homes.listHomes(playerClient, commandArgs);
                                break;
                            case "/build":
                                Share.handleBuild(playerClient, commandArgs);
                                break;
                            case "/tpg":
                                Vars.teleportGhost(playerClient, commandArgs);
                                break;
                            case "/followghost":
                                Tools.followGhostTool(playerClient, commandArgs);
                                break;
                            case "/tpabove":
                                Vars.teleportAbove(playerClient, commandArgs);
                                break;
                            case "/ghost":
                                Tools.ghostTool(playerClient, playerChar, commandArgs);
                                break;
                            case "/notify":
                                Tools.notifyTool(playerClient, commandArgs);
                                break;
                            case "/lights":
                                Lights.handle(playerClient.userID, playerChar, commandArgs);
                                break;
                            case "/baccess":
                                Tools.giveBettyAccess(playerClient, commandArgs);
                                break;
                            case "/opos":
                                Tools.oposTool(playerClient, commandArgs);
                                break;
                            case "/elevup":
                                Elevators.moveElevatorUp(playerClient, playerChar, commandArgs);
                                break;
                            case "/elevdown":
                                Elevators.moveElevatorDown(playerClient, playerChar, commandArgs);
                                break;
                            case "/elevator":
                                Tools.elevatorTool(playerClient, commandArgs);
                                break;
                            case "/donated":
                                Items.giveDonorKit(playerClient);
                                break;
                            case "/donate":
                                Display.displayShopifyURL(playerClient);
                                break;
                            case "/betty":
                                Vars.REB.StartCoroutine(Explosions.dropMine(playerClient, commandArgs));
                                break;
                            case "/ebullet":
                                Tools.explosiveBulletTool(playerClient, commandArgs);
                                break;
                            case "/bypass":
                                Tools.bypassTool(playerClient, commandArgs);
                                break;
                            case "/compass":
                                Display.displayDirection(playerClient);
                                break;
                            case "/clock":
                                Display.displayClock(playerClient);
                                break;
                            case "/check":
                                Display.displayTools(playerClient, commandArgs);
                                break;
                            case "/spawnlist":
                                Display.displayEntities(playerClient);
                                break;
                            case "/spawn":
                                SpawnEntity.spawnEntity(playerClient, commandArgs);
                                break;
                            case "/ewarp":
                                Vars.handleWarps(playerClient, commandArgs);
                                break;
                            case "/vote":
                                Display.displayVoteURL(playerClient);
                                break;
                            case "/voted":
                                Items.redeemVoteKit(playerClient);
                                break;
                            case "/invsee":
                                Vars.seeInventory(playerClient, commandArgs);
                                break;
                            case "/fps":
                                Vars.amplifyFPS(playerClient);
                                break;
                            case "/quality":
                                Vars.amplifyQuality(playerClient);
                                break;
                            case "/ping":
                                Display.displayPing(playerClient);
                                break;
                            case "/whois":
                                Display.displayPlayerInfo(playerClient, commandArgs, false);
                                break;
                            case "/awhois":
                                Display.displayPlayerInfo(playerClient, commandArgs, true);
                                break;
                            case "/info":
                                Display.displayServerInfo(playerClient);
                                break;
                            case "/frozen":
                                Display.displayFrozen(playerClient, commandArgs);
                                break;
                            case "/unfreezeall":
                                Vars.unfreezeAll(playerClient);
                                break;
                            case "/unfreeze":
                                Vars.unfreezePlayer(playerClient, commandArgs);
                                break;
                            case "/freeze":
                                Vars.freezePlayer(playerClient, commandArgs);
                                break;
                            case "/rfreeze":
                                Vars.freezeNearby(playerClient, commandArgs);
                                break;
                            case "/radius":
                                Display.displayNearby(playerClient, commandArgs);
                                break;
                            case "/client":
                                Vars.grabClient(playerClient, commandArgs);
                                break;
                            case "/clearinv":
                                Vars.clearPlayer(playerClient, commandArgs);
                                break;
                            case "/craft":
                                Tools.craftTool(playerClient, commandArgs);
                                break;
                            case "/leaderboard":
                                Display.displayLeaderboard(playerClient, commandArgs);
                                break;
                            case "/drop":
                                Display.displayDropTime(playerClient);
                                break;
                            case "/uammo":
                                Tools.unlAmmoTool(playerClient, commandArgs);
                                break;
                            case "/iammo":
                                Tools.infAmmoTool(playerClient, commandArgs);
                                break;
                            case "/portal":
                                Tools.portalTool(playerClient, commandArgs);
                                break;
                            case "/wand":
                                Tools.wandTool(playerClient, commandArgs);
                                break;
                            case "/vanish":
                                Tools.vanishTool(playerClient, commandArgs);
                                break;
                            case "/hide":
                                Tools.hideTool(playerClient, commandArgs);
                                break;
                            case "/dist":
                                Display.displayDistance(playerClient, commandArgs);
                                break;
                            case "/owner":
                                Tools.showOwner(playerClient, commandArgs);
                                break;
                            case "/remover":
                                Tools.minorRemoverTool(playerClient, commandArgs);
                                break;
                            case "/removeall":
                                Tools.removerAllTool(playerClient, commandArgs);
                                break;
                            case "/remove":
                                Tools.removerTool(playerClient, commandArgs);
                                break;
                            case "/f":
                                Factions.handleFactions(playerClient, commandArgs);
                                break;
                            case "/r":
                                Broadcast.reply(playerClient, commandArgs);
                                break;
                            case "/rules":
                                Display.displayRules(playerClient);
                                break;
                            case "/players":
                                Display.displayPlayers(playerClient);
                                break;
                            case "/warps":
                                Display.displayWarps(playerClient);
                                break;
                            case "/kits":
                                Display.displayKits(playerClient);
                                break;
                            case "/feed":
                                Vars.feedPlayer(player, playerName, commandArgs);
                                break;
                            case "/heal":
                                Vars.healPlayer(player, playerName, commandArgs);
                                break;
                            case "/access":
                                Tools.giveAccess(playerClient, commandArgs);
                                break;
                            case "/version":
                                Broadcast.broadcastTo(player, "The server is running RustEssentials v" + Vars.currentVersion + ".");
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
                                Display.displayHistory(playerClient, commandArgs);
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
                            case "/banip":
                                Vars.banIP(playerClient, commandArgs);
                                break;
                            case "/bane":
                                Vars.banPlayer(playerClient, commandArgs, true);
                                break;
                            case "/kick":
                                Vars.kickPlayer(playerClient, commandArgs, false);
                                break;
                            case "/kickip":
                                Vars.kickIP(playerClient, commandArgs);
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
                                Vars.getPlayerPos(playerClient, commandArgs, false);
                                break;
                            case "/apos":
                                Vars.getPlayerPos(playerClient, commandArgs, true);
                                break;
                            case "/i":
                                Items.createItem(playerClient, playerClient, commandArgs, message, true);
                                break;
                            case "/give":
                                Items.createItem(playerClient, commandArgs, message);
                                break;
                            case "/giveall":
                                Items.giveAll(playerClient, commandArgs);
                                break;
                            case "/random":
                                Items.giveRandom(playerClient, commandArgs);
                                break;
                            case "/warp":
                                Vars.warpPlayer(playerClient, commandArgs);
                                break;
                            case "/kit":
                                Items.giveKit(playerClient, commandArgs, message);
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
                            case "/unshareall":
                                Share.unshareWithAll(playerClient);
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
                            default:
                                if (Vars.unknownCommand)
                                    Broadcast.broadcastTo(player, "Unknown command \"" + command + "\"!");
                                break;
                        }
                    }

                    return true;
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
            return false;
        }
    }
}
