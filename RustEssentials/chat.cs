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
    public static bool serverlog = true;

    static chat()
    {
    }

    [ConsoleSystem.User]
    public static void say(Arg arg)
    {
        if (!enabled)
            return;

        string playerName = arg.argUser.user.Displayname;
        string clientName = arg.argUser.playerClient.userName;
        string UID = arg.argUser.user.Userid.ToString();
        string message = arg.GetString(0, "text");
        List<string> dividedMessage = null;


        if (playerName != null && message.Length > 0)
        {
            if (message.StartsWith("/"))
            {
                Vars.conLog.Chat("<CMD> " + playerName + ": " + message);
                if (serverlog)
                {
                    Debug.Log("[CHAT] <CMD> " + playerName + ": " + message);
                }
                //Thread t = new Thread(() => Commands.CMD(arg));
                //t.Start();
                Commands.CMD(arg);
                return;
            }
            else
            {
                playerName = Vars.filterNames(playerName, UID);
                string consoleMessage = message.Replace("[PM]", "").Replace("[PM to]", "").Replace("[PM from]", "").Replace("[PM From]", "").Replace("[PM To]", "").Replace("[F]", "");
                message = message.Replace("\"", "\\\"").Replace("[PM]", "").Replace("[PM to]", "").Replace("[PM from]", "").Replace("[PM From]", "").Replace("[PM To]", "").Replace("[F]", "");
                if (Vars.censorship)
                {
                    List<string> splitMessage = new List<string>(message.Split(' '));
                    foreach (string s in splitMessage)
                    {
                        if (Vars.illegalWords.Contains(s.ToLower().Replace(".", "").Replace("!", "").Replace(",", "").Replace("?", "").Replace(";", "")))
                        {
                            string curseWord = Array.Find(Vars.illegalWords.ToArray(), (string str) => str.Equals(s.ToLower().Replace(".", "").Replace("!", "").Replace(",", "").Replace("?", "").Replace(";", "")));
                            string asterisks = "";
                            for (int i = 0; i < s.Replace(".", "").Replace("!", "").Replace(",", "").Replace("?", "").Replace(";", "").Length; i++)
                            {
                                asterisks += "*";
                            }
                            string theRest = s.Replace(s, "");
                            string fullString = (s.StartsWith(theRest) ? theRest + asterisks : asterisks + theRest);
                            splitMessage[splitMessage.IndexOf(s)] = fullString;
                        }
                    }
                    message = string.Join(" ", splitMessage.ToArray());
                }

                string newResult = Broadcast.replaceColorCodes(message);
                if (newResult.Length > Vars.wordWrapLimit && Vars.enableWordWrap)
                {
                    dividedMessage = new List<string>();
                    string oldMessage = message;
                    string lastColorCode = "";

                    while (oldMessage.Length > 0)
                    {
                        string substring;
                        if (newResult.Length > Vars.wordWrapLimit)
                        {
                            substring = Broadcast.disregardColorCodes(oldMessage, out newResult);
                            oldMessage = oldMessage.Substring(substring.Length);
                        }
                        else
                        {
                            substring = oldMessage;
                            oldMessage = "";
                        }
                        dividedMessage.Add((lastColorCode != "" ? lastColorCode : "") + substring);
                        if (substring.Contains("[color"))
                        {
                            int colorIndex = substring.LastIndexOf("[color");
                            string startingFromColor = substring.Substring(colorIndex);
                            if (startingFromColor.Contains("]"))
                            {
                                int bracketIndex = startingFromColor.IndexOf("]");
                                lastColorCode = startingFromColor.Substring(0, bracketIndex + 1);
                            }
                        }
                    }
                }
                bool didBroadcast = false;
                if (!Vars.inDirect.Contains(UID) && !Vars.inGlobal.Contains(UID) && !Vars.inFaction.Contains(UID))
                {
                    Vars.inGlobal.Add(UID);
                }
                if (!Vars.inDirectV.Contains(UID) && !Vars.inGlobalV.Contains(UID))
                {
                    Vars.inDirectV.Add(UID);
                }
                if (clientName.Length == 0)
                {
                    Broadcast.broadcastTo(arg.argUser.networkPlayer, "You cannot chat while vanished!");
                    return;
                }
                if (Vars.catchBroadcastErrors)
                {
                    try
                    {
                        continueSending(UID, playerName, message, consoleMessage, arg, dividedMessage, out didBroadcast);
                    }
                    catch (Exception ex)
                    {
                        Vars.conLog.logToFile("Something went wrong when submitting chat message:", "error");
                        Vars.conLog.logToFile(ex.ToString(), "error");
                        if (!didBroadcast)
                        {
                            if (!Vars.mutedUsers.Contains(UID))
                            {
                                Hook hook = Vars.callHook("RustEssentials.Hooks", "OnSendChat", false, UID, message, Broadcast.replaceColorCodes(message), Chat.Global);
                                if (Checks.ContinueHook(hook))
                                {
                                    if (dividedMessage != null)
                                    {
                                        foreach (string s in dividedMessage)
                                        {
                                            Broadcast.BroadcastChat("chat.add \"" + (Vars.removeTag ? "" : "<G> ") + playerName + "\" \"" + s.Trim() + "\"");
                                        }
                                    }
                                    else
                                    {
                                        Broadcast.BroadcastChat("chat.add \"" + (Vars.removeTag ? "" : "<G> ") + playerName + "\" \"" + message + "\"");
                                    }
                                    Vars.conLog.Chat("<G> " + playerName + ": " + message);
                                    if (serverlog)
                                    {
                                        Debug.Log("[CHAT] <G> " + playerName + ": " + consoleMessage);
                                    }
                                    if (Vars.sendChatToConsoles)
                                        Broadcast.broadcastToAllConsoles("[color #FFA154]<G> [color #66CCFF]" + playerName + ": [color white]" + consoleMessage);
                                }
                            }
                            else
                            {
                                if (Vars.muteTimes.ContainsKey(UID))
                                {
                                    double timeLeft = Math.Round((Vars.muteTimes[UID].TimeLeft / 1000));
                                    TimeSpan timeSpan = TimeSpan.FromMilliseconds(timeLeft);

                                    string timeString = "";

                                    timeString += timeSpan.Hours + " hours, ";
                                    timeString += timeSpan.Minutes + " minutes, and ";
                                    timeString += timeSpan.Seconds + " seconds";
                                    string secondsLeft = "You have been muted for " + timeString + " on global chat.";
                                    Broadcast.broadcastTo(arg.argUser.networkPlayer, secondsLeft);
                                }
                                else
                                    Broadcast.broadcastTo(arg.argUser.networkPlayer, "You have been muted on global chat.");
                            }
                        }
                        return;
                    }
                }
                else
                {
                    continueSending(UID, playerName, message, consoleMessage, arg, dividedMessage, out didBroadcast);
                }
            }
        }
    }

    public static void continueSending(string UID, string playerName, string message, string consoleMessage, Arg arg, List<string> dividedMessage, out bool didBroadcast)
    {
        didBroadcast = false;
        if (Vars.inDirect.Contains(UID))
        {
            if (Vars.directChat)
            {
                Hook hook = Vars.callHook("RustEssentials.Hooks", "OnSendChat", false, UID, message, Broadcast.replaceColorCodes(message), Chat.Direct);
                if (Checks.ContinueHook(hook))
                {
                    if (dividedMessage != null)
                    {
                        Vars.REB.StartCoroutine(Vars.sendToSurrounding(arg.argUser.playerClient, playerName, dividedMessage, consoleMessage));
                        didBroadcast = true;
                    }
                    else
                    {
                        Vars.REB.StartCoroutine(Vars.sendToSurrounding(arg.argUser.playerClient, playerName, message, consoleMessage));
                        didBroadcast = true;
                    }
                    Vars.conLog.Chat("<D> " + playerName + ": " + message);
                    if (serverlog)
                    {
                        Debug.Log("[CHAT] <D> " + playerName + ": " + consoleMessage);
                    }
                }
                return;
            }
            else
            {
                Vars.inGlobal.Add(UID);
                Vars.inDirect.Remove(UID);

                if (!Vars.mutedUsers.Contains(UID))
                {
                    Broadcast.broadcastTo(arg.argUser.networkPlayer, "Direct chat has been disabled! You are now talking in global chat.");
                    Hook hook = Vars.callHook("RustEssentials.Hooks", "OnSendChat", false, UID, message, Broadcast.replaceColorCodes(message), Chat.Global);
                    if (Checks.ContinueHook(hook))
                    {
                        if (dividedMessage != null)
                        {
                            foreach (string s in dividedMessage)
                            {
                                Broadcast.BroadcastChat("chat.add \"" + (Vars.removeTag ? "" : "<G> ") + playerName + "\" \"" + s.Trim() + "\"");
                                if (Vars.historyGlobal.Count > 50)
                                    Vars.historyGlobal.RemoveAt(0);
                                Vars.historyGlobal.Add("* " + (Vars.removeTag ? "" : "<G> ") + playerName + "$:|:$" + s.Trim());
                            }
                            didBroadcast = true;
                        }
                        else
                        {
                            Broadcast.BroadcastChat("chat.add \"" + (Vars.removeTag ? "" : "<G> ") + playerName + "\" \"" + message + "\"");
                            if (Vars.historyGlobal.Count > 50)
                                Vars.historyGlobal.RemoveAt(0);
                            Vars.historyGlobal.Add("* " + (Vars.removeTag ? "" : "<G> ") + playerName + "$:|:$" + message);
                            didBroadcast = true;
                        }
                        Vars.conLog.Chat("<G> " + playerName + ": " + message);
                        if (serverlog)
                        {
                            Debug.Log("[CHAT] <G> " + playerName + ": " + consoleMessage);
                        }
                        if (Vars.sendChatToConsoles)
                            Broadcast.broadcastToAllConsoles("[color #FFA154]<G> [color #66CCFF]" + playerName + ": [color white]" + consoleMessage);
                    }
                }
                else
                {
                    if (Vars.muteTimes.ContainsKey(UID))
                    {
                        double timeLeft = Math.Round((Vars.muteTimes[UID].TimeLeft / 1000));
                        TimeSpan timeSpan = TimeSpan.FromMilliseconds(timeLeft);

                        string timeString = "";

                        timeString += timeSpan.Hours + " hours, ";
                        timeString += timeSpan.Minutes + " minutes, and ";
                        timeString += timeSpan.Seconds + " seconds";
                        string secondsLeft = "You have been muted for " + timeString + " seconds on global chat.";
                        Broadcast.broadcastTo(arg.argUser.networkPlayer, secondsLeft);
                    }
                    else
                        Broadcast.broadcastTo(arg.argUser.networkPlayer, "You have been muted on global chat.");
                }
                return;
            }
        }
        if (Vars.inGlobal.Contains(UID))
        {
            if (Vars.globalChat)
            {
                if (!Vars.mutedUsers.Contains(UID))
                {
                    Hook hook = Vars.callHook("RustEssentials.Hooks", "OnSendChat", false, UID, message, Broadcast.replaceColorCodes(message), Chat.Global);
                    if (Checks.ContinueHook(hook))
                    {
                        if (dividedMessage != null)
                        {
                            foreach (string s in dividedMessage)
                            {
                                Broadcast.BroadcastChat("chat.add \"" + (Vars.removeTag ? "" : "<G> ") + playerName + "\" \"" + s.Trim() + "\"");
                                if (Vars.historyGlobal.Count > 50)
                                    Vars.historyGlobal.RemoveAt(0);
                                Vars.historyGlobal.Add("* " + (Vars.removeTag ? "" : "<G> ") + playerName + "$:|:$" + s.Trim());
                            }
                            didBroadcast = true;
                        }
                        else
                        {
                            Broadcast.BroadcastChat("chat.add \"" + (Vars.removeTag ? "" : "<G> ") + playerName + "\" \"" + message + "\"");
                            if (Vars.historyGlobal.Count > 50)
                                Vars.historyGlobal.RemoveAt(0);
                            Vars.historyGlobal.Add("* " + (Vars.removeTag ? "" : "<G> ") + playerName + "$:|:$" + message);
                            didBroadcast = true;
                        }
                        Vars.conLog.Chat("<G> " + playerName + ": " + message);
                        if (serverlog)
                        {
                            Debug.Log("[CHAT] <G> " + playerName + ": " + consoleMessage);
                        }
                        if (Vars.sendChatToConsoles)
                            Broadcast.broadcastToAllConsoles("[color #FFA154]<G> [color #66CCFF]" + playerName + ": [color white]" + consoleMessage);
                    }
                }
                else
                {
                    if (Vars.muteTimes.ContainsKey(UID))
                    {
                        double timeLeft = Math.Round((Vars.muteTimes[UID].TimeLeft / 1000));
                        TimeSpan timeSpan = TimeSpan.FromMilliseconds(timeLeft);

                        string timeString = "";

                        timeString += timeSpan.Hours + " hours, ";
                        timeString += timeSpan.Minutes + " minutes, and ";
                        timeString += timeSpan.Seconds + " seconds";
                        string secondsLeft = "You have been muted for " + timeString + " seconds on global chat.";
                        Broadcast.broadcastTo(arg.argUser.networkPlayer, secondsLeft);
                    }
                    else
                        Broadcast.broadcastTo(arg.argUser.networkPlayer, "You have been muted on global chat.");
                }
            }
            else
            {
                Vars.inDirect.Add(UID);
                Vars.inGlobal.Remove(UID);

                Broadcast.broadcastTo(arg.argUser.networkPlayer, "Global chat has been disabled! You are now talking in direct chat.");

                Hook hook = Vars.callHook("RustEssentials.Hooks", "OnSendChat", false, UID, message, Broadcast.replaceColorCodes(message), Chat.Direct);
                if (Checks.ContinueHook(hook))
                {
                    if (dividedMessage != null)
                    {
                        Vars.REB.StartCoroutine(Vars.sendToSurrounding(arg.argUser.playerClient, playerName, dividedMessage, consoleMessage));
                        didBroadcast = true;
                    }
                    else
                    {
                        Vars.REB.StartCoroutine(Vars.sendToSurrounding(arg.argUser.playerClient, playerName, message, consoleMessage));
                        didBroadcast = true;
                    }
                    Vars.conLog.Chat("<D> " + playerName + ": " + message);
                    if (serverlog)
                    {
                        Debug.Log("[CHAT] <D> " + playerName + ": " + consoleMessage);
                    }
                }
            }
            return;
        }
        if (Vars.inFaction.Contains(UID))
        {
            KeyValuePair<string, Dictionary<string, string>>[] possibleFactions = Array.FindAll(Vars.factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(arg.argUser.userID.ToString()));

            if (possibleFactions.Length > 0)
            {
                Hook hook = Vars.callHook("RustEssentials.Hooks", "OnSendChat", false, UID, message, Broadcast.replaceColorCodes(message), Chat.Faction);
                if (Checks.ContinueHook(hook))
                {
                    if (dividedMessage != null)
                    {
                        Vars.REB.StartCoroutine(Vars.sendToFaction(arg.argUser.playerClient, playerName, dividedMessage, consoleMessage));
                        didBroadcast = true;
                    }
                    else
                    {
                        Vars.REB.StartCoroutine(Vars.sendToFaction(arg.argUser.playerClient, playerName, message, consoleMessage));
                        didBroadcast = true;
                    }
                    Vars.conLog.Chat("<F [" + possibleFactions[0].Key + "]> " + playerName + ": " + message);
                    if (serverlog)
                    {
                        Debug.Log("[CHAT] <F [" + possibleFactions[0].Key + "]> " + playerName + ": " + consoleMessage);
                    }
                    if (!Vars.historyFaction.ContainsKey(possibleFactions[0].Key))
                        Vars.historyFaction.Add(possibleFactions[0].Key, new List<string>());
                    if (dividedMessage != null)
                    {
                        foreach (string s in dividedMessage)
                        {
                            if (Vars.historyFaction[possibleFactions[0].Key].Count > 50)
                                Vars.historyFaction[possibleFactions[0].Key].RemoveAt(0);
                            if (!Vars.historyFaction.ContainsKey(possibleFactions[0].Key))
                                Vars.historyFaction.Add(possibleFactions[0].Key, new List<string>() { { "* <F> " + playerName + "$:|:$" + s.Trim() } });
                            else
                                ((List<string>)Vars.historyFaction[possibleFactions[0].Key]).Add("* <F> " + playerName + "$:|:$" + s.Trim());
                        }
                    }
                    else
                    {
                        if (Vars.historyFaction[possibleFactions[0].Key].Count > 50)
                            Vars.historyFaction[possibleFactions[0].Key].RemoveAt(0);
                        if (!Vars.historyFaction.ContainsKey(possibleFactions[0].Key))
                            Vars.historyFaction.Add(possibleFactions[0].Key, new List<string>() { { "* <F> " + playerName + "$:|:$" + message } });
                        else
                            ((List<string>)Vars.historyFaction[possibleFactions[0].Key]).Add("* <F> " + playerName + "$:|:$" + message);
                    }
                }
            }
            else
            {
                if (Vars.globalChat)
                {
                    Vars.inGlobal.Add(UID);
                    Broadcast.broadcastTo(arg.argUser.networkPlayer, "You are not in a faction! You are now talking in global chat.");
                }
                else
                {
                    Vars.inDirect.Add(UID);
                    Broadcast.broadcastTo(arg.argUser.networkPlayer, "You are not in a faction! You are now talking in direct chat.");
                }

                Vars.inFaction.Remove(UID);

                if (!Vars.mutedUsers.Contains(UID))
                {
                    Hook hook = Vars.callHook("RustEssentials.Hooks", "OnSendChat", false, UID, message, Broadcast.replaceColorCodes(message), Chat.Global);
                    if (Checks.ContinueHook(hook))
                    {
                        if (dividedMessage != null)
                        {
                            foreach (string s in dividedMessage)
                            {
                                Broadcast.BroadcastChat("chat.add \"" + (Vars.removeTag ? "" : "<G> ") + playerName + "\" \"" + s.Trim() + "\"");
                                if (Vars.historyGlobal.Count > 50)
                                    Vars.historyGlobal.RemoveAt(0);
                                Vars.historyGlobal.Add("* " + (Vars.removeTag ? "" : "<G> ") + playerName + "$:|:$" + s.Trim());
                            }
                            didBroadcast = true;
                        }
                        else
                        {
                            Broadcast.BroadcastChat("chat.add \"" + (Vars.removeTag ? "" : "<G> ") + playerName + "\" \"" + message + "\"");
                            if (Vars.historyGlobal.Count > 50)
                                Vars.historyGlobal.RemoveAt(0);
                            Vars.historyGlobal.Add("* " + (Vars.removeTag ? "" : "<G> ") + playerName + "$:|:$" + message);
                            didBroadcast = true;
                        }
                        Vars.conLog.Chat("<G> " + playerName + ": " + message);
                        if (serverlog)
                        {
                            Debug.Log("[CHAT] <G> " + playerName + ": " + consoleMessage);
                        }
                        if (Vars.sendChatToConsoles)
                            Broadcast.broadcastToAllConsoles("[color #FFA154]<G> [color #66CCFF]" + playerName + ": [color white]" + consoleMessage);
                    }
                }
                else
                {
                    if (Vars.muteTimes.ContainsKey(UID))
                    {
                        double timeLeft = Math.Round((Vars.muteTimes[UID].TimeLeft / 1000));
                        TimeSpan timeSpan = TimeSpan.FromMilliseconds(timeLeft);

                        string timeString = "";

                        timeString += timeSpan.Hours + " hours, ";
                        timeString += timeSpan.Minutes + " minutes, and ";
                        timeString += timeSpan.Seconds + " seconds";
                        string secondsLeft = "You have been muted for " + timeString + " seconds on global chat.";
                        Broadcast.broadcastTo(arg.argUser.networkPlayer, secondsLeft);
                    }
                    else
                        Broadcast.broadcastTo(arg.argUser.networkPlayer, "You have been muted on global chat.");
                }
            }
            return;
        }
    }
}
