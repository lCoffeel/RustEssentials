/**
 * @file: chat.cs
 * @author: Facepunch Studios
 * @modified: Team Cerionn (https://github.com/Team-Cerionn)
 * @version: 1.0.0.0
 * @description: chat class for Rust Essentials.cs
 */
using Facepunch.Utility;
using RustEssentials.Util;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System;

public class chat : ConsoleSystem
{
    [ConsoleSystem.Help("Enable or disable chat displaying", "")]
    [ConsoleSystem.Client]
    [ConsoleSystem.Admin]
    public static bool enabled = true;
    [ConsoleSystem.Admin]
    public static bool serverlog;

    static chat()
    {
    }

    [ConsoleSystem.User]
    public static void say(Arg arg)
    {
        if (!enabled)
            return;

        string playerName = arg.argUser.user.Displayname;
        string UID = arg.argUser.user.Userid.ToString();
        string message = arg.GetString(0, "text");

        if (playerName != null && message.Length > 0)
        {
            if (message.StartsWith("/"))
            {
                Vars.conLog.Chat("<CMD> " + playerName + ": " + message);
                //Thread t = new Thread(() => Commands.CMD(arg));
                //t.Start();
                Commands.CMD(arg);
            }
            else
            {
                playerName = Vars.filterNames(playerName, UID);
                message = message.Replace("\"", "\\\"").Replace("[PM]", "").Replace("[PM to]", "").Replace("[PM from]", "").Replace("[PM From]", "").Replace("[PM To]", "").Replace("[F]", "");
                if (!Vars.inDirect.Contains(UID) && !Vars.inGlobal.Contains(UID))
                {
                    Vars.inGlobal.Add(UID);
                }
                if (!Vars.inDirectV.Contains(UID) && !Vars.inGlobalV.Contains(UID))
                {
                    Vars.inDirectV.Add(UID);
                }
                if (Vars.inDirect.Contains(UID))
                {
                    if (Vars.directChat)
                    {
                        Thread t = new Thread(() => Vars.sendToSurrounding(arg.argUser.playerClient, message));
                        t.Start();
                        Vars.conLog.Chat("<D> " + playerName + ": " + message);
                    }
                    else
                    {
                        if (!Vars.mutedUsers.Contains(UID))
                        {
                            Vars.inDirect.Remove(UID);
                            Vars.inGlobal.Add(UID);
                            Broadcast.broadcastTo(arg.argUser.networkPlayer, "Direct chat has been disabled! You are now talking in global chat.");
                            ConsoleNetworker.Broadcast("chat.add \"" + (Vars.removeTag ? "" : "<G> ") + playerName + "\" \"" + message + "\"");
                            Vars.conLog.Chat("<G> " + playerName + ": " + message);
                            if (Vars.historyGlobal.Count > 50)
                                Vars.historyGlobal.RemoveAt(0);
                            Vars.historyGlobal.Add("* " + (Vars.removeTag ? "" : "<G> ") + playerName + "$:|:$" + message);
                        }
                        else
                        {
                            if (Vars.muteTimes.ContainsKey(UID))
                            {
                                string secondsLeft = "You have been muted for " + Math.Round(Vars.muteTimes[UID].TimeLeft / 1000).ToString() + " seconds on global chat.";
                                Broadcast.broadcastTo(arg.argUser.networkPlayer, secondsLeft);
                            }
                            else
                                Broadcast.broadcastTo(arg.argUser.networkPlayer, "You have been muted on global chat.");
                        }
                    }
                }
                else if (Vars.inGlobal.Contains(UID))
                {
                    if (Vars.globalChat)
                    {
                        if (!Vars.mutedUsers.Contains(UID))
                        {
                            ConsoleNetworker.Broadcast("chat.add \"" + (Vars.removeTag ? "" : "<G> ") + playerName + "\" \"" + message + "\"");
                            Vars.conLog.Chat("<G> " + playerName + ": " + message);
                            if (Vars.historyGlobal.Count > 50)
                                Vars.historyGlobal.RemoveAt(0);
                            Vars.historyGlobal.Add("* " + (Vars.removeTag ? "" : "<G> ") + playerName + "$:|:$" + message);
                        }
                        else
                        {
                            if (Vars.muteTimes.ContainsKey(UID))
                            {
                                string secondsLeft = "You have been muted for " + Math.Round(Vars.muteTimes[UID].TimeLeft / 1000).ToString() + " seconds on global chat.";
                                Broadcast.broadcastTo(arg.argUser.networkPlayer, secondsLeft);
                            }
                            else
                                Broadcast.broadcastTo(arg.argUser.networkPlayer, "You have been muted on global chat.");
                        }
                    }
                    else
                    {
                        Vars.inGlobal.Remove(UID);
                        Vars.inDirect.Add(UID);
                        Broadcast.broadcastTo(arg.argUser.networkPlayer, "Global chat has been disabled! You are now talking in direct chat.");


                        Thread t = new Thread(() => Vars.sendToSurrounding(arg.argUser.playerClient, message));
                        t.Start();
                        Vars.conLog.Chat("<D> " + playerName + ": " + message);
                    }
                }
            }
        }
    }
}
