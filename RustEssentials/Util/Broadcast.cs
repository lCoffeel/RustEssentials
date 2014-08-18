using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facepunch;
using UnityEngine;
using uLink;
using Rust;
using RustProto;
using System.Threading;

namespace RustEssentials.Util
{
    public static class Broadcast
    {
        public static void broadcastToAllConsoles(string fullString)
        {
            if (ConsoleNetworker.singleton != null)
            {
                ConsoleNetworker.singleton.networkView.RPC<string>("CL_ConsoleMessage", uLink.RPCMode.All, fullString);
            }
        }

        public static void broadcastToConsoles(List<uLink.NetworkPlayer> players, string fullString)
        {
            if (ConsoleNetworker.singleton != null)
            {
                ConsoleNetworker.singleton.networkView.RPC<string>("CL_ConsoleMessage", players, fullString);
            }
        }

        public static void broadcastToConsole(uLink.NetworkPlayer player, string fullString)
        {
            if (ConsoleNetworker.singleton != null)
            {
                ConsoleNetworker.singleton.networkView.RPC<string>("CL_ConsoleMessage", player, fullString);
            }
        }

        public static void BroadcastChat(string fullString)
        {
            if (Vars.catchBroadcastErrors)
            {
                try
                {
                    ConsoleNetworker.Broadcast(fullString);
                }
                catch (Exception ex)
                {
                    if (Vars.logBroadcastErrors)
                    {
                        Vars.conLog.logToFile("Not very important, feel free to ignore: Something went wrong when sending a chat message through BC:", "error");
                        Vars.conLog.logToFile(ex.ToString(), "error");
                    }
                }
            }
            else
                ConsoleNetworker.Broadcast(fullString);
        }

        public static void BroadcastAllRPC(string cmd)
        {
            if (Vars.catchBroadcastErrors)
            {
                try
                {
                    if (ConsoleNetworker.singleton != null && ConsoleNetworker.singleton.networkView != null)
                    {
                        ConsoleNetworker.singleton.networkView.RPC<string>("CL_ConsoleCommand", uLink.RPCMode.All, cmd);
                    }
                }
                catch (Exception ex)
                {
                    if (Vars.logBroadcastErrors)
                    {
                        Vars.conLog.logToFile("Not very important, feel free to ignore: Something went wrong when sending CL_ConsoleCommand through BARPC:", "error");
                        Vars.conLog.logToFile(ex.ToString(), "error");
                    }
                }
            }
            else
            {
                if (ConsoleNetworker.singleton != null && ConsoleNetworker.singleton.networkView != null)
                {
                    ConsoleNetworker.singleton.networkView.RPC<string>("CL_ConsoleCommand", uLink.RPCMode.All, cmd);
                }
            }
        }

        public static void BroadcastRPC(List<uLink.NetworkPlayer> players, string cmd)
        {
            if (Vars.catchBroadcastErrors)
            {
                try
                {
                    if (players != null && ConsoleNetworker.singleton != null && ConsoleNetworker.singleton.networkView != null)
                    {
                        ConsoleNetworker.singleton.networkView.RPC<string>("CL_ConsoleCommand", players, cmd);
                    }
                }
                catch (Exception ex)
                {
                    if (Vars.logBroadcastErrors)
                    {
                        Vars.conLog.Error("Not very important, feel free to ignore: Something went wrong when sending CL_ConsoleCommand through BRPC:");
                        Vars.conLog.Error(ex.ToString());
                    }
                }
            }
            else
            {
                if (players != null && ConsoleNetworker.singleton != null && ConsoleNetworker.singleton.networkView != null)
                {
                    ConsoleNetworker.singleton.networkView.RPC<string>("CL_ConsoleCommand", players, cmd);
                }
            }
        }

        public static void BroadcastRPC(uLink.NetworkPlayer player, string cmd)
        {
            if (Vars.catchBroadcastErrors)
            {
                try
                {
                    if (player != null && ConsoleNetworker.singleton != null && ConsoleNetworker.singleton.networkView != null)
                    {
                        ConsoleNetworker.singleton.networkView.RPC<string>("CL_ConsoleCommand", player, cmd);
                    }
                }
                catch (Exception ex)
                {
                    if (Vars.logBroadcastErrors)
                    {
                        Vars.conLog.Error("Not very important, feel free to ignore: Something went wrong when sending CL_ConsoleCommand through BRPC:");
                        Vars.conLog.Error(ex.ToString());
                    }
                }
            }
            else
            {
                if (player != null && ConsoleNetworker.singleton != null && ConsoleNetworker.singleton.networkView != null)
                {
                    ConsoleNetworker.singleton.networkView.RPC<string>("CL_ConsoleCommand", player, cmd);
                }
            }
        }

        public static void broadcastCustomTo(List<uLink.NetworkPlayer> senders, string name, string message)
        {
            try
            {
                if (senders != null)
                {
                    string result = Vars.replaceQuotes(message);
                    List<string> dividedMessage = null;
                    string newResult = replaceColorCodes(result);
                    if (newResult.Length > Vars.wordWrapLimit && Vars.enableWordWrap)
                    {
                        dividedMessage = new List<string>();
                        string oldMessage = result;
                        string lastColorCode = "";

                        while (oldMessage.Length > 0)
                        {
                            string substring;
                            if (newResult.Length > Vars.wordWrapLimit)
                            {
                                substring = disregardColorCodes(oldMessage, out newResult);
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
                    if (dividedMessage != null)
                    {
                        foreach (string s in dividedMessage)
                        {
                            BroadcastRPC(senders, "chat.add \"" + name + "\" \"" + s + "\"");
                        }
                    }
                    else
                        BroadcastRPC(senders, "chat.add \"" + name + "\" \"" + result + "\"");
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("Could not send personal custom broadcast: " + ex.Message);
            }
        }

        public static void broadcastCustomTo(uLink.NetworkPlayer sender, string name, string message)
        {
            try
            {
                if (sender != null)
                {
                    string result = Vars.replaceQuotes(message);
                    List<string> dividedMessage = null;
                    string newResult = replaceColorCodes(result);
                    if (newResult.Length > Vars.wordWrapLimit && Vars.enableWordWrap)
                    {
                        dividedMessage = new List<string>();
                        string oldMessage = result;
                        string lastColorCode = "";

                        while (oldMessage.Length > 0)
                        {
                            string substring;
                            if (newResult.Length > Vars.wordWrapLimit)
                            {
                                substring = disregardColorCodes(oldMessage, out newResult);
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
                    if (dividedMessage != null)
                    {
                        foreach (string s in dividedMessage)
                        {
                            BroadcastRPC(sender, "chat.add \"" + name + "\" \"" + s + "\"");
                        }
                    }
                    else
                        BroadcastRPC(sender, "chat.add \"" + name + "\" \"" + result + "\"");
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("Could not send broadcastCustomTo: " + ex.Message);
            }
        }

        public static void broadcastCustom(string name, string message)
        {
            try
            {
                string result = Vars.replaceQuotes(message);
                List<string> dividedMessage = null;
                string newResult = replaceColorCodes(result);
                if (newResult.Length > Vars.wordWrapLimit && Vars.enableWordWrap)
                {
                    dividedMessage = new List<string>();
                    string oldMessage = result;
                    string lastColorCode = "";

                    while (oldMessage.Length > 0)
                    {
                        string substring;
                        if (newResult.Length > Vars.wordWrapLimit)
                        {
                            substring = disregardColorCodes(oldMessage, out newResult);
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
                if (dividedMessage != null)
                {
                    foreach (string s in dividedMessage)
                    {
                        BroadcastAllRPC("chat.add \"" + name + "\" \"" + s + "\"");
                    }
                }
                else
                    BroadcastAllRPC("chat.add \"" + name + "\" \"" + result + "\"");
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("Could not send broadcastCustom: " + ex.Message);
            }
            //foreach (PlayerClient pc in Vars.AllPlayerClients)
            //{
            //    ConsoleNetworker.SendClientCommand(pc.netPlayer, "chat.add \"" + name + "\" \"" + Vars.replaceQuotes(message) + "\"");
            //}
        }

        public static void broadcastTo(List<uLink.NetworkPlayer> players, string message)
        {
            try
            {
                if (Vars.defaultColor != "" && !message.StartsWith("[color"))
                    message = "[color " + Vars.defaultColor + "]" + message;
                if (players != null && players.Count > 0)
                {
                    foreach (var netPlayer in players)
                    {
                        if (netPlayer != null)
                        {
                            PlayerClient playerClient;
                            if (Vars.getPlayerClient(netPlayer, out playerClient))
                            {
                                if (Vars.logPluginChat)
                                    Vars.conLog.Chat("<TO " + playerClient.userName + "> " + Vars.botName + ": " + message);
                            }
                        }
                    }

                    string result = Vars.replaceQuotes(message);
                    List<string> dividedMessage = null;
                    string newResult = replaceColorCodes(result);
                    if (newResult.Length > Vars.wordWrapLimit && Vars.enableWordWrap)
                    {
                        dividedMessage = new List<string>();
                        string oldMessage = result;
                        string lastColorCode = "";

                        while (oldMessage.Length > 0)
                        {
                            string substring;
                            if (newResult.Length > Vars.wordWrapLimit)
                            {
                                substring = disregardColorCodes(oldMessage, out newResult);
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
                    if (dividedMessage != null)
                    {
                        foreach (string s in dividedMessage)
                        {
                            BroadcastRPC(players, "chat.add \"[PM] " + Vars.botName + "\" \"" + s + "\"");
                        }
                    }
                    else
                        BroadcastRPC(players, "chat.add \"[PM] " + Vars.botName + "\" \"" + result + "\"");
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("Could not send personal broadcast #3: " + ex.Message);
            }
        }

        public static void broadcastTo(uLink.NetworkPlayer player, string message)
        {
            try
            {
                if (Vars.defaultColor != "" && !message.StartsWith("[color"))
                    message = "[color " + Vars.defaultColor + "]" + message;
                if (player != null)
                {
                    PlayerClient playerClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == player);
                    if (playerClient != null)
                    {
                        if (Vars.logPluginChat)
                            Vars.conLog.Chat("<TO " + playerClient.userName + "> " + Vars.botName + ": " + message);

                        string result = Vars.replaceQuotes(message);
                        List<string> dividedMessage = null;
                        string newResult = replaceColorCodes(result);
                        if (newResult.Length > Vars.wordWrapLimit && Vars.enableWordWrap)
                        {
                            dividedMessage = new List<string>();
                            string oldMessage = result;
                            string lastColorCode = "";

                            while (oldMessage.Length > 0)
                            {
                                string substring;
                                if (newResult.Length > Vars.wordWrapLimit)
                                {
                                    substring = disregardColorCodes(oldMessage, out newResult);
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
                        if (dividedMessage != null)
                        {
                            foreach (string s in dividedMessage)
                            {
                                BroadcastRPC(player, "chat.add \"[PM] " + Vars.botName + "\" \"" + s + "\"");
                            }
                        }
                        else
                            BroadcastRPC(player, "chat.add \"[PM] " + Vars.botName + "\" \"" + result + "\"");
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("Could not send personal broadcast #2: " + ex.Message);
            }
        }

        public static void broadcastCommandTo(uLink.NetworkPlayer player, string command)
        {
            try
            {
                if (player != null)
                {
                    PlayerClient playerClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == player);
                    if (playerClient != null)
                    {
                        BroadcastRPC(player, command);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("Could not send command broadcast: " + ex.Message);
            }
        }

        public static void broadcastJoinLeave(string message)
        {
            try
            {
                if (Vars.defaultColor != "" && !message.StartsWith("[color"))
                    message = "[color " + Vars.defaultColor + "]" + message;
                string result = Vars.replaceQuotes(message);
                List<string> dividedMessage = null;
                string newResult = replaceColorCodes(result);
                if (newResult.Length > Vars.wordWrapLimit && Vars.enableWordWrap)
                {
                    dividedMessage = new List<string>();
                    string oldMessage = result;
                    string lastColorCode = "";

                    while (oldMessage.Length > 0)
                    {
                        string substring;
                        if (newResult.Length > Vars.wordWrapLimit)
                        {
                            substring = disregardColorCodes(oldMessage, out newResult);
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
                if (dividedMessage != null)
                {
                    foreach (string s in dividedMessage)
                    {
                        BroadcastAllRPC("chat.add \"" + Vars.botName + "\" \"" + s + "\"");
                    }
                }
                else
                    BroadcastAllRPC("chat.add \"" + Vars.botName + "\" \"" + result + "\"");
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("Could not send join/leave broadcast: " + ex.Message);
            }
            //foreach (PlayerClient pc in Vars.AllPlayerClients)
            //{
            //    ConsoleNetworker.SendClientCommand(pc.netPlayer, "chat.add \"" + Vars.botName + "\" \"" + Vars.replaceQuotes(message) + "\"");
            //}
        }

        public static void broadcastKill(string message, string actualMessageToSend = "")
        {
            try
            {
                if (actualMessageToSend == "")
                    actualMessageToSend = message;
                if (Vars.defaultColor != "" && !message.StartsWith("[color"))
                    actualMessageToSend = "[color " + Vars.defaultColor + "]" + actualMessageToSend;
                if (Vars.logPluginChat)
                    Vars.conLog.Chat("<BROADCAST ALL> " + Vars.botName + ": " + message);

                string result = Vars.replaceQuotes(actualMessageToSend);
                List<string> dividedMessage = null;
                string newResult = replaceColorCodes(result);
                if (newResult.Length > Vars.wordWrapLimit && Vars.enableWordWrap)
                {
                    dividedMessage = new List<string>();
                    string oldMessage = result;
                    string lastColorCode = "";

                    while (oldMessage.Length > 0)
                    {
                        string substring;
                        if (newResult.Length > Vars.wordWrapLimit)
                        {
                            substring = disregardColorCodes(oldMessage, out newResult);
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
                if (dividedMessage != null)
                {
                    foreach (string s in dividedMessage)
                    {
                        BroadcastAllRPC("chat.add \"" + Vars.botName + "\" \"" + s + "\"");
                    }
                }
                else
                    BroadcastAllRPC("chat.add \"" + Vars.botName + "\" \"" + result + "\"");
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("Could not send broadcast to all: " + ex.ToString());
            }
        }

        public static void broadcastAll(string message)
        {
            try
            {
                if (Vars.defaultColor != "" && !message.StartsWith("[color"))
                    message = "[color " + Vars.defaultColor + "]" + message;
                if (Vars.logPluginChat)
                    Vars.conLog.Chat("<BROADCAST ALL> " + Vars.botName + ": " + message);

                string result = Vars.replaceQuotes(message);
                List<string> dividedMessage = null;
                string newResult = replaceColorCodes(result);
                if (newResult.Length > Vars.wordWrapLimit && Vars.enableWordWrap)
                {
                    dividedMessage = new List<string>();
                    string oldMessage = result;
                    string lastColorCode = "";

                    while (oldMessage.Length > 0)
                    {
                        string substring;
                        if (newResult.Length > Vars.wordWrapLimit)
                        {
                            substring = disregardColorCodes(oldMessage, out newResult);
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
                if (dividedMessage != null)
                {
                    foreach (string s in dividedMessage)
                    {
                        BroadcastAllRPC("chat.add \"" + Vars.botName + "\" \"" + s + "\"");
                    }
                }
                else
                    BroadcastAllRPC("chat.add \"" + Vars.botName + "\" \"" + result + "\"");
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("Could not send broadcast to all: " + ex.ToString());
            }
            //foreach (PlayerClient pc in Vars.AllPlayerClients)
            //{
            //    ConsoleNetworker.SendClientCommand(pc.netPlayer, "chat.add \"" + Vars.botName + "\" \"" + Vars.replaceQuotes(message) + "\"");
            //}
        }

        public static void noticeTo(uLink.NetworkPlayer sender, string icon, string message, int duration = 2, bool log = false)
        {
            if (log)
            {
                if (sender != null)
                {
                    PlayerClient playerClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == sender);
                    if (playerClient != null)
                    {
                        if (Vars.logPluginChat)
                            Vars.conLog.Chat("<NOTICE TO " + playerClient.userName +"> " + message);
                    }
                }
            }
            BroadcastRPC(sender, "notice.popup " + duration + " \"" + icon + "\" \"" + Vars.replaceQuotes(message) + "\"");
        }

        public static void noticeAll(string icon, string message, int duration = 2, bool log = false)
        {
            if (log)
            {
                if (Vars.logPluginChat)
                    Vars.conLog.Chat("<NOTICE ALL> " + message);
            }
            BroadcastAllRPC("notice.popup " + duration + " \"" + icon + "\" \"" + Vars.replaceQuotes(message) + "\"");

            //foreach (PlayerClient pc in Vars.AllPlayerClients)
            //{
            //    ConsoleNetworker.SendClientCommand(pc.netPlayer, "notice.popup " + duration m \"" + icon + "\" \"" + Vars.replaceQuotes(message) + "\"");
            //}
        }

        public static void sideNoticeTo(uLink.NetworkPlayer player, string message)
        {
            Notice.Inventory(player, Vars.replaceQuotes(message));
        }

        public static void sideNoticeAll(string icon, string message)
        {
            BroadcastAllRPC("notice.inventory \"" + Vars.replaceQuotes(message) + "\"");

            //foreach (PlayerClient pc in Vars.AllPlayerClients)
            //{
            //    ConsoleNetworker.SendClientCommand(pc.netPlayer, "notice.inventory \"" + Vars.replaceQuotes(message) + "\"");
            //}
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

                        if (Vars.logPluginChat)
                        {
                            Vars.conLog.Chat("<PM FROM> " + senderClient.userName + ": " + message);
                            Vars.conLog.Chat("<PM TO> " + targetClient.userName + ": " + message);
                        }
                        string namePrefixTo = (Vars.nextToName ? "[PM to] " : "");
                        string msgPrefixTo = (!Vars.nextToName ? "[PM to] " : "");
                        string namePrefixFrom = (Vars.nextToName ? "[PM from] " : "");
                        string msgPrefixFrom = (!Vars.nextToName ? "[PM from] " : "");

                        string result = msgPrefixTo + message;
                        List<string> dividedMessage = null;
                        string newResult = replaceColorCodes(result);
                        if (newResult.Length > Vars.wordWrapLimit && Vars.enableWordWrap)
                        {
                            dividedMessage = new List<string>();
                            string oldMessage = result;
                            string lastColorCode = "";

                            while (oldMessage.Length > 0)
                            {
                                string substring;
                                if (newResult.Length > Vars.wordWrapLimit)
                                {
                                    substring = disregardColorCodes(oldMessage, out newResult);
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
                        Vars.callHook("RustEssentials.Hooks", "OnSendPM", false, senderClient.userID.ToString(), targetClient.userID.ToString(), message, Broadcast.replaceColorCodes(message));
                        if (dividedMessage != null)
                        {
                            foreach (string s in dividedMessage)
                            {
                                BroadcastRPC(senderClient.netPlayer, "chat.add \"" + namePrefixTo + targetClient.userName + "\" \"" + result + "\"");
                                BroadcastRPC(targetClient.netPlayer, "chat.add \"" + namePrefixFrom + senderClient.userName + "\" \"" + result + "\"");
                            }
                        }
                        else
                        {
                            BroadcastRPC(senderClient.netPlayer, "chat.add \"" + namePrefixTo + targetClient.userName + "\" \"" + result + "\"");
                            BroadcastRPC(targetClient.netPlayer, "chat.add \"" + namePrefixFrom + senderClient.userName + "\" \"" + result + "\"");
                        }
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
                            Hook hook = Vars.callHook("RustEssentials.Hooks", "OnSendPM", false, senderClient.userID.ToString(), targetClient.userID.ToString(), message, Broadcast.replaceColorCodes(message));

                            if (Checks.ContinueHook(hook))
                            {
                                if (Vars.logPluginChat)
                                {
                                    Vars.conLog.Chat("<PM FROM> " + senderClient.userName + ": " + message);
                                    Vars.conLog.Chat("<PM TO> " + targetClient.userName + ": " + message);
                                }
                                string namePrefixTo = (Vars.nextToName ? "[PM to] " : "");
                                string msgPrefixTo = (!Vars.nextToName ? "[PM to] " : "");
                                string namePrefixFrom = (Vars.nextToName ? "[PM from] " : "");
                                string msgPrefixFrom = (!Vars.nextToName ? "[PM from] " : "");

                                string result = msgPrefixTo + message;
                                List<string> dividedMessage = null;
                                string newResult = replaceColorCodes(result);
                                if (newResult.Length > Vars.wordWrapLimit && Vars.enableWordWrap)
                                {
                                    dividedMessage = new List<string>();
                                    string oldMessage = result;
                                    string lastColorCode = "";

                                    while (oldMessage.Length > 0)
                                    {
                                        string substring;
                                        if (newResult.Length > Vars.wordWrapLimit)
                                        {
                                            substring = disregardColorCodes(oldMessage, out newResult);
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
                                if (dividedMessage != null)
                                {
                                    foreach (string s in dividedMessage)
                                    {
                                        BroadcastRPC(senderClient.netPlayer, "chat.add \"" + namePrefixTo + targetClient.userName + "\" \"" + result + "\"");
                                        BroadcastRPC(targetClient.netPlayer, "chat.add \"" + namePrefixFrom + senderClient.userName + "\" \"" + result + "\"");
                                    }
                                }
                                else
                                {
                                    BroadcastRPC(senderClient.netPlayer, "chat.add \"" + namePrefixTo + targetClient.userName + "\" \"" + result + "\"");
                                    BroadcastRPC(targetClient.netPlayer, "chat.add \"" + namePrefixFrom + senderClient.userName + "\" \"" + result + "\"");
                                }

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

                broadcastTo(sender, "Do \"/help <command name without />\" to view syntax.");
                broadcastTo(sender, "Available commands:");
                Vars.listCommands(rank, playerClient);
            }
            else if (args.Length > 1)
            {
                string command = args[1];

                switch (command)
                {
                    case "build":
                        broadcastTo(sender, "/build: Allows another player to place beds, sleeping bags, and gateways near your houses. Syntax: /build {share/unshare} *player name* or /build {unshareall}");
                        break;
                    case "tpg":
                        broadcastTo(sender, "/tpg: Teleports to another user's ghost (or body if they are not ghosting). Syntax: /tpg [name] or /tpg [name1] [name2]");
                        break;
                    case "followghost":
                        broadcastTo(sender, "/followghost: If /ghost and /vanish are on, your body will follow your ghost and allow you to render new objects. Syntax: /followghost {on/off}");
                        break;
                    case "tpabove":
                        broadcastTo(sender, "/tpabove: Teleports the sender, or someone else, 100 meters above the target user. Syntax: /tpabove [name] or /tp [name1] [name2]");
                        break;
                    case "ghost":
                        broadcastTo(sender, "/ghost: Turns you into a ghost. Moving only affects your camera view, not your body. Syntax: /ghost {on/off}");
                        break;
                    case "notify":
                        broadcastTo(sender, "/notify: Toggles display of antihack notifications to yourself. Syntax: /notify {on/off}");
                        break;
                    case "lights":
                        broadcastTo(sender, "/lights: Toggles nearby furnaces and camp fires. Syntax: /lights {on/off/alloff/serveroff}");
                        break;
                    case "baccess":
                        broadcastTo(sender, "/baccess: Gives access to pick up any bouncing betty. Syntax: /baccess {on/off}");
                        break;
                    case "opos":
                        broadcastTo(sender, "/opos: Displays the position of any object you hit. Syntax: /opos {on/off}");
                        break;
                    case "betty":
                        broadcastTo(sender, "/betty: Places a bouncing betty on the ground. Syntax: /betty");
                        break;
                    case "ebullet":
                        broadcastTo(sender, "/ebullet: Loads explosive bullets for yourself. Syntax: /ebullet {on/off}");
                        break;
                    //case "donated":
                    //    broadcastTo(sender, "/donated: Redeems your reward for donating. Syntax: /donated");
                    //    break;
                    //case "donate":
                    //    broadcastTo(sender, "/donate: Displays the link for donations. Syntax: /donate");
                    //    break;
                    case "bypass":
                        broadcastTo(sender, "/bypass: Bypasses the anti-speedhack and anti-jumphack. Syntax: /bypass {on/off}");
                        break;
                    case "compass":
                        broadcastTo(sender, "/compass: Displays the direction you are facing. Syntax: /compass");
                        break;
                    case "clock":
                        broadcastTo(sender, "/clock: Displays the current time in game. Syntax: /clock");
                        break;
                    case "check":
                        broadcastTo(sender, "/check: Displays the status of all tools of a user. Syntax: /check *player name*");
                        break;
                    case "spawnlist":
                        broadcastTo(sender, "/spawnlist: Lists all entities that can be used with /spawn. Syntax: /spawnlist");
                        break;
                    case "spawn":
                        broadcastTo(sender, "/spawn: Spawns entities from the /spawnlist. Syntax: /spawn [entity] [amount]");
                        break;
                    case "ewarp":
                        broadcastTo(sender, "/ewarp: Adds, removes, or changes the ownership of warps. Syntax: /ewarp {add/rank/rem/uid}");
                        break;
                    case "vote":
                        broadcastTo(sender, "/vote: Displays the server's voting page. Syntax: /vote");
                        break;
                    case "voted":
                        broadcastTo(sender, "/voted: Redeems your reward for voting for the server. Syntax: /voted");
                        break;
                    case "invsee":
                        broadcastTo(sender, "/invsee: Mirrors the inventory of the specified user. Syntax: /invsee *player name*");
                        break;
                    case "fps":
                        broadcastTo(sender, "/fps: Amplifies your fps. Syntax: /fps");
                        break;
                    case "quality":
                        broadcastTo(sender, "/quality: Amplifies your graphics. Syntax: /quality");
                        break;
                    case "ping":
                        broadcastTo(sender, "/ping: Displays your ping to the server. Syntax: /ping");
                        break;
                    case "whois":
                        broadcastTo(sender, "/whois: Displays the user's information. Syntax: /whois *player name*");
                        break;
                    case "awhois":
                        broadcastTo(sender, "/awhois: Displays all of the user's information. Syntax: /awhois *player name*");
                        break;
                    case "info":
                        broadcastTo(sender, "/info: Displays the server's name and ip. Syntax: /info");
                        break;
                    case "frozen":
                        broadcastTo(sender, "/frozen: Displays the names of all frozen users. Syntax: /frozen");
                        break;
                    case "unfreezeall":
                        broadcastTo(sender, "/unfreezeall: Unfreezes all frozen users. Syntax: /unfreezeall");
                        break;
                    case "unfreeze":
                        broadcastTo(sender, "/unfreeze: Unfreezes the specified user. Syntax: /unfreeze *player name*");
                        break;
                    case "freeze":
                        broadcastTo(sender, "/freeze: Freezes the specified user. Syntax: /freeze *player name*");
                        break;
                    case "rfreeze":
                        broadcastTo(sender, "/rfreeze: Freezes all users within a radius. Syntax: /rfreeze [radius]");
                        break;
                    case "radius":
                        broadcastTo(sender, "/radius: Displays the names of all users within a radius. Syntax: /radius [radius]");
                        break;
                    case "clearinv":
                        broadcastTo(sender, "/clearinv: Clears the inventory of the specified player. Syntax: /clearinv *player name*");
                        break;
                    case "craft":
                        broadcastTo(sender, "/craft: Bypasses restrictions and allows for infinite research kits/crafting. Syntax: /craft {on/off}");
                        break;
                    case "leaderboard":
                        broadcastTo(sender, "/drop: Displays the leaderboard. Syntax: /leaderboard {deaths/kills/kdr}");
                        break;
                    case "drop":
                        broadcastTo(sender, "/drop: Displays the time until the next airdrop. Syntax: /drop");
                        break;
                    case "uammo":
                        broadcastTo(sender, "/uammo: Gives you unlimited magazines. Syntax: /uammo {on/off}");
                        break;
                    case "iammo":
                        broadcastTo(sender, "/iammo: Gives you infinite ammo; must have at least 1 bullet loaded. Syntax: /iammo {on/off}");
                        break;
                    case "portal":
                        broadcastTo(sender, "/portal: Toggles the portal tool. Syntax: /portal {on/off}");
                        break;
                    case "wand":
                        broadcastTo(sender, "/wand: Toggles the wand tool or sets the wand distance. Syntax: /wand {distance/on/off}");
                        break;
                    case "vanish":
                        broadcastTo(sender, "/vanish: Makes you invisible. Syntax: /vanish {on/off}");
                        break;
                    case "hide":
                        broadcastTo(sender, "/hide: Hides you from AI. Syntax: /hide {on/off}");
                        break;
                    case "owner":
                        broadcastTo(sender, "/owner: Hitting structures will display the owner of them. Syntax: /owner {on/off}");
                        break;
                    case "remover":
                        broadcastTo(sender, "/remover: Toggles the minor remover tool. Turn on for more details. Syntax: /remover {on/off/share/unshare/unshareall}");
                        break;
                    case "removeall":
                        broadcastTo(sender, "/removeall: Toggles the advanced remover tool. Turn on for more details. Syntax: /removeall {on/off}");
                        break;
                    case "remove":
                        broadcastTo(sender, "/remove: Toggles the remover tool. Turn on for more details. Syntax: /remove {on/off}");
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
                        broadcastTo(sender, "/tppos: Teleports you to the said vector. Syntax: /tppos [x] [y] [z]");
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
                        broadcastTo(sender, "/history: Shows the last # lines of chat history (1-50). Syntax: /history {g/f} [1-50]");
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
                        broadcastTo(sender, "/reload: Reloads the specified config file. Syntax: /reload {config/whitelist/ranks/commands/kits/motd/prefixes/warps/controller /tables/loadout/decay/remover/bans/all}");
                        break;
                    case "unban":
                        broadcastTo(sender, "/unban: Unbans the specified player. Syntax: /unban \"full name, UID, or IP\"");
                        break;
                    case "ban":
                        broadcastTo(sender, "/ban: Bans the specified player. Syntax: /ban [player or UID] *reason*");
                        break;
                    case "banip":
                        broadcastTo(sender, "/banip: Bans the specified IP. Syntax: /banip [player ip] *reason*");
                        break;
                    case "bane":
                        broadcastTo(sender, "/bane: Bans the specified player by the exact name. Syntax: /bane <player> *reason*");
                        break;
                    case "kick":
                        broadcastTo(sender, "/kick: Kicks the specified player. Syntax: /kick \"player\" [reason]");
                        break;
                    case "kickip":
                        broadcastTo(sender, "/kickip: Kicks all players with the specified ip. Syntax: /kickip [ip] [reason]");
                        break;
                    case "kicke":
                        broadcastTo(sender, "/kicke: Kicks the specified player by the exact name. Syntax: /kicke \"player\" [reason]");
                        break;
                    case "daylength":
                        broadcastTo(sender, "/daylength: Sets or gets the length of the day cycle in minutes. Syntax: /daylength [#.##]");
                        break;
                    case "nightlength":
                        broadcastTo(sender, "/nightlength: Sets or gets the length of the night cycle in minutes. Syntax: /nightlength [#.##]");
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
                        broadcastTo(sender, "/pos: Returns the current position. Syntax: /pos");
                        break;
                    case "apos":
                        broadcastTo(sender, "/apos: Returns the player's position. Syntax: /apos *player name*");
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
                        broadcastTo(sender, "/share: Shares doors and gates with another player (CASE SENSITIVE). Syntax: /share [player name]");
                        break;
                    case "unshare":
                        broadcastTo(sender, "/unshare: Unshares doors and gates with another player (CASE SENSITIVE). Syntax: /unshare [player name]");
                        break;
                    case "unshareall":
                        broadcastTo(sender, "/unsharell: Unshares all doors and gates. Syntax: /unshareall");
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

        public static string replaceColorCodes(string message)
        {
            string newResult = message;
            while (newResult.Contains("[color"))
            {
                int colorIndex = newResult.IndexOf("[color");
                string startingFromColor = newResult.Substring(colorIndex);
                if (startingFromColor.Contains("]"))
                {
                    int bracketIndex = startingFromColor.IndexOf("]");
                    if (colorIndex > 0)
                    {
                        newResult = newResult.Substring(0, colorIndex) + startingFromColor.Substring(bracketIndex + 1);
                    }
                    else
                    {
                        newResult = startingFromColor.Substring(bracketIndex + 1);
                    }
                }
                else
                {
                    if (colorIndex > 0)
                    {
                        newResult = newResult.Substring(0, colorIndex);
                    }
                    else
                    {
                        newResult = "";
                    }
                }
            }
            return newResult;
        }

        public static string disregardColorCodes(string message, out string remainingMessage)
        {
            string newMessage = message;
            List<string> messagesAndColors = new List<string>();
            while (newMessage.Contains("[color"))
            {
                int colorIndex = newMessage.IndexOf("[color");
                string startingFromColor = newMessage.Substring(colorIndex);
                if (startingFromColor.Contains("]"))
                {
                    int bracketIndex = startingFromColor.IndexOf("]");
                    if (colorIndex > 0)
                    {
                        messagesAndColors.Add(newMessage.Substring(0, colorIndex)); // Add what is leading up to the color code
                        messagesAndColors.Add(startingFromColor.Substring(0, bracketIndex + 1)); // Add the color code
                        newMessage = startingFromColor.Substring(bracketIndex + 1);
                        if (!newMessage.Contains("[color"))
                        {
                            messagesAndColors.Add(newMessage); // Add the remaining text
                        }
                    }
                    else
                    {
                        messagesAndColors.Add(startingFromColor.Substring(0, bracketIndex + 1)); // Add the color code
                        newMessage = startingFromColor.Substring(bracketIndex + 1);
                        if (!newMessage.Contains("[color"))
                        {
                            messagesAndColors.Add(newMessage); // Add the remaining text
                        }
                    }
                }
                else
                {
                    if (colorIndex > 0)
                    {
                        newMessage = newMessage.Substring(0, colorIndex);
                        messagesAndColors.Add(newMessage); // Add what is leading up to the broken color code
                    }
                    else
                    {
                        newMessage = "";
                    }
                }
            }

            string result = "";
            string resultNoColors = "";

            foreach (string s in messagesAndColors)
            {
                if (resultNoColors.Length >= Vars.wordWrapLimit)
                    break;
                if (s.StartsWith("[color"))
                {
                    result += s;
                }
                else
                {
                    string[] separated = s.Split(' ');
                    if (separated.Count() > 1)
                    {
                        foreach (string str in separated)
                        {
                            if (!String.IsNullOrEmpty(str))
                            {
                                if (str.Length < Vars.wordWrapLimit)
                                {
                                    if ((resultNoColors + (resultNoColors.Length > 0 ? " " : "") + str).Length <= Vars.wordWrapLimit)
                                    {
                                        string complete = str + " ";
                                        result += complete;
                                        resultNoColors += complete;
                                    }
                                    else
                                        break;
                                }
                                else
                                {
                                    foreach (char c in str)
                                    {
                                        if ((resultNoColors + c).Length <= Vars.wordWrapLimit)
                                        {
                                            result += c;
                                            resultNoColors += c;
                                        }
                                        else
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                result += " ";
                                resultNoColors += " ";
                            }
                        }
                        if (result[result.Length - 1] == ' ')
                            result = result.Substring(0, result.Length - 1);
                        if (resultNoColors[resultNoColors.Length - 1] == ' ')
                            resultNoColors = resultNoColors.Substring(0, resultNoColors.Length - 1);
                    }
                    else
                    {
                        if ((resultNoColors + s).Length <= Vars.wordWrapLimit)
                        {
                            result += s;
                            resultNoColors += s;
                        }
                        else
                        {
                            foreach (char c in s)
                            {
                                if ((resultNoColors + c).Length <= Vars.wordWrapLimit)
                                {
                                    result += c;
                                    resultNoColors += c;
                                }
                                else
                                    break;
                            }
                        }
                    }
                }
            }

            if (messagesAndColors.Count == 0) // If there are no color codes
            {
                string[] separated = message.Split(' ');
                List<string> toCombine = new List<string>();
                if (separated.Count() > 1)
                {
                    foreach (string str in separated)
                    {
                        if (!String.IsNullOrEmpty(str))
                        {
                            if (str.Length < Vars.wordWrapLimit)
                            {
                                if ((resultNoColors + (resultNoColors.Length > 0 ? " " : "") + str).Length <= Vars.wordWrapLimit)
                                {
                                    string complete = str + " ";
                                    result += complete;
                                    resultNoColors += complete;
                                }
                                else
                                    break;
                            }
                            else
                            {
                                foreach (char c in str)
                                {
                                    if ((resultNoColors + c).Length <= Vars.wordWrapLimit)
                                    {
                                        result += c;
                                        resultNoColors += c;
                                    }
                                    else
                                        break;
                                }
                                break;
                            }
                        }
                        else
                        {
                            result += " ";
                            resultNoColors += " ";
                        }
                    }
                    if (result[result.Length - 1] == ' ')
                        result = result.Substring(0, result.Length - 1);
                    if (resultNoColors[resultNoColors.Length - 1] == ' ')
                        resultNoColors = resultNoColors.Substring(0, resultNoColors.Length - 1);
                }
                else
                {
                    if ((resultNoColors + message).Length <= Vars.wordWrapLimit)
                    {
                        result += message;
                        resultNoColors += message;
                    }
                    else
                    {
                        foreach (char c in message)
                        {
                            if ((resultNoColors + c).Length <= Vars.wordWrapLimit)
                            {
                                result += c;
                                resultNoColors += c;
                            }
                            else
                                break;
                        }
                    }
                }
            }

            remainingMessage = Broadcast.replaceColorCodes(message).Substring(resultNoColors.Length);
            return result;
        }
    }
}
