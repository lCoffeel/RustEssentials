using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

namespace RustEssentials.Util
{
    public static class Items
    {
        public class Item
        {
            public string name;
            public int amount;
            public int uses;
            public int modSlots;
            public ItemModDataBlock[] mods;

            public Item(string name, int amount)
            {
                this.name = name;
                this.amount = amount;
                this.uses = -1;
                this.modSlots = -1;
                this.mods = null;
            }

            public Item(string name, int amount, int uses, int modSlots, ItemModDataBlock[] mods)
            {
                this.name = name;
                this.amount = amount;
                this.uses = uses;
                this.modSlots = modSlots;
                this.mods = mods;
            }
        }

        public static void giveDonorKit(PlayerClient senderClient)
        {
            try
            {
                if (senderClient != null && Vars.enableShopify /* && Vars.shopifyAPIKey != "" */ )
                {
                    ulong UID = senderClient.userID;
                    bool didDropItems = false;
                    using (WebClient wc = new WebClient())
                    {
                        try
                        {
                            string result = wc.DownloadString(@"http://107.170.162.97:1137/v1/getOrder?UID=" + UID);
                            //List<string> donorKits = JsonConvert.DeserializeObject<List<string>>(result);
                            //foreach (string donorKitID in donorKits)
                            //{
                            //    if (Vars.donorKits.ContainsKey(donorKitID))
                            //    {
                            //        List<Item> donorKit = Vars.donorKits[donorKitID];
                            //        foreach (Item item in donorKit)
                            //        {
                            //            addItemThroughKit(senderClient, item.name, item.amount, item.uses, item.modSlots, item.mods, out didDropItems);
                            //        }
                            //    }
                            //}
                        }
                        catch { }
                    }
                    if (didDropItems && Vars.dropItemMessage != "")
                        Broadcast.broadcastTo(senderClient.netPlayer, Vars.dropItemMessage);
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("GDK: " + ex.ToString());
            }
        }

        public static void giveKit(PlayerClient senderClient, string[] args, string message)
        {
            try
            {
                if (args.Count() > 1)
                {
                    string kitName = args[1];
                    string kitNameToLower = kitName.ToLower();
                    List<Item> kitItems = new List<Item>();
                    if (Vars.kits.ContainsKey(kitNameToLower)) // If kit exists
                    {
                        if (Vars.kitsForRanks.ContainsKey(Vars.findRank(senderClient.userID))) // If kit exist for my rank
                        {
                            if (Vars.kitsForRanks[Vars.findRank(senderClient.userID)].Contains(kitNameToLower)) // If the kit I requested is permitted for my rank
                            {
                                bool b = true;
                                if (Vars.activeKitCooldowns.ContainsKey(senderClient.userID))
                                {
                                    if (Vars.activeKitCooldowns[senderClient.userID].ContainsValue(kitNameToLower))
                                    {
                                        foreach (KeyValuePair<TimerPlus, string> kv in Vars.activeKitCooldowns[senderClient.userID])
                                        {
                                            if (kv.Value == kitNameToLower)
                                            {
                                                if (kv.Key.isNull || Math.Round((kv.Key.timeLeft / 1000)) > 0)
                                                    b = false;
                                            }
                                        }
                                    }
                                }

                                if (b) // If I am not on cool down for this kit
                                {
                                    kitItems = Vars.kits[kitNameToLower];
                                    Broadcast.noticeTo(senderClient.netPlayer, "☻", "You were given the " + kitName + " kit.");

                                    if (Vars.kitCooldowns.ContainsKey(kitNameToLower)) // If a cooldown is set for this kit, set my cool down
                                    {
                                        if (Vars.kitCooldowns[kitNameToLower] > -1)
                                        {
                                            TimerPlus t = TimerPlus.Create(Vars.kitCooldowns[kitNameToLower], false, Vars.restoreKit, kitNameToLower, senderClient.userID);

                                            if (!Vars.activeKitCooldowns.ContainsKey(senderClient.userID))
                                                Vars.activeKitCooldowns.Add(senderClient.userID, new Dictionary<TimerPlus, string>() { { t, kitNameToLower } });
                                            else
                                                Vars.activeKitCooldowns[senderClient.userID].Add(t, kitNameToLower);
                                        }
                                        else
                                        {
                                            TimerPlus t = new TimerPlus();
                                            t.isNull = true;
                                            if (!Vars.activeKitCooldowns.ContainsKey(senderClient.userID))
                                                Vars.activeKitCooldowns.Add(senderClient.userID, new Dictionary<TimerPlus, string>() { { t, kitNameToLower } });
                                            else
                                                Vars.activeKitCooldowns[senderClient.userID].Add(t, kitNameToLower);
                                        }
                                    }
                                }
                                else // If I am on cool down
                                {
                                    foreach (KeyValuePair<TimerPlus, string> kv in Vars.activeKitCooldowns[senderClient.userID])
                                    {
                                        if (kv.Value == kitNameToLower)
                                        {
                                            // Return how long I have to wait
                                            if (!kv.Key.isNull)
                                            {
                                                double timeLeft = Math.Round((kv.Key.timeLeft / 1000));
                                                TimeSpan timeSpan = TimeSpan.FromMilliseconds(kv.Key.timeLeft);

                                                string timeString = "";

                                                timeString += timeSpan.Hours + " hours, ";
                                                timeString += timeSpan.Minutes + " minutes, and ";
                                                timeString += timeSpan.Seconds + " seconds";
                                                Broadcast.noticeTo(senderClient.netPlayer, "⌛", "You must wait " + (timeLeft > 999999999 ? "forever" : timeString) + " before using this.", 4);
                                            }
                                            else
                                            {
                                                Broadcast.noticeTo(senderClient.netPlayer, "⌛", "You can no longer use this kit!", 4);
                                            }
                                        }
                                    }
                                }
                            }
                            else // If I am not allowed to use this kit
                            {
                                if (Vars.kitsForUIDs.ContainsKey(senderClient.userID))
                                {
                                    if (Vars.kitsForUIDs[senderClient.userID].Contains(kitNameToLower))
                                    {
                                        bool b = true;
                                        if (Vars.activeKitCooldowns.ContainsKey(senderClient.userID))
                                        {
                                            if (Vars.activeKitCooldowns[senderClient.userID].ContainsValue(kitNameToLower))
                                            {
                                                foreach (KeyValuePair<TimerPlus, string> kv in Vars.activeKitCooldowns[senderClient.userID])
                                                {
                                                    if (kv.Value == kitNameToLower)
                                                    {
                                                        if (kv.Key.isNull || Math.Round((kv.Key.timeLeft / 1000)) > 0)
                                                            b = false;
                                                    }
                                                }
                                            }
                                        }

                                        if (b) // If I am not on cool down for this kit
                                        {
                                            kitItems = Vars.kits[kitNameToLower];
                                            Broadcast.noticeTo(senderClient.netPlayer, "☻", "You were given the " + kitName + " kit.");

                                            if (Vars.kitCooldowns.ContainsKey(kitNameToLower)) // If a cooldown is set for this kit, set my cool down
                                            {
                                                if (Vars.kitCooldowns[kitNameToLower] > -1)
                                                {
                                                    TimerPlus t = TimerPlus.Create(Vars.kitCooldowns[kitNameToLower], false, Vars.restoreKit, kitNameToLower, senderClient.userID);

                                                    if (!Vars.activeKitCooldowns.ContainsKey(senderClient.userID))
                                                        Vars.activeKitCooldowns.Add(senderClient.userID, new Dictionary<TimerPlus, string>() { { t, kitNameToLower } });
                                                    else
                                                        Vars.activeKitCooldowns[senderClient.userID].Add(t, kitNameToLower);
                                                }
                                                else
                                                {
                                                    TimerPlus t = new TimerPlus();
                                                    t.isNull = true;
                                                    if (!Vars.activeKitCooldowns.ContainsKey(senderClient.userID))
                                                        Vars.activeKitCooldowns.Add(senderClient.userID, new Dictionary<TimerPlus, string>() { { t, kitNameToLower } });
                                                    else
                                                        Vars.activeKitCooldowns[senderClient.userID].Add(t, kitNameToLower);
                                                }
                                            }
                                        }
                                        else // If I am on cool down
                                        {
                                            foreach (KeyValuePair<TimerPlus, string> kv in Vars.activeKitCooldowns[senderClient.userID])
                                            {
                                                if (kv.Value == kitNameToLower)
                                                {
                                                    if (!kv.Key.isNull)
                                                    {
                                                        // Return how long I have to wait
                                                        double timeLeft = Math.Round((kv.Key.timeLeft / 1000));
                                                        TimeSpan timeSpan = TimeSpan.FromMilliseconds(kv.Key.timeLeft);

                                                        string timeString = "";

                                                        timeString += timeSpan.Hours + " hours, ";
                                                        timeString += timeSpan.Minutes + " minutes, and ";
                                                        timeString += timeSpan.Seconds + " seconds";
                                                        Broadcast.noticeTo(senderClient.netPlayer, "", "You must wait " + (timeLeft > 999999999 ? "forever" : timeString) + " before using this.", 4);
                                                    }
                                                    else
                                                    {
                                                        Broadcast.noticeTo(senderClient.netPlayer, "⌛", "You can no longer use this kit!", 4);
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
                        else // If I do not have any kits assigned to my rank
                        {
                            if (Vars.unassignedKits.Contains(kitNameToLower)) // If the kit is actually unassigned to a rank
                            {
                                bool b = true;
                                if (Vars.activeKitCooldowns.ContainsKey(senderClient.userID))
                                {
                                    if (Vars.activeKitCooldowns[senderClient.userID].ContainsValue(kitNameToLower))
                                    {
                                        foreach (KeyValuePair<TimerPlus, string> kv in Vars.activeKitCooldowns[senderClient.userID])
                                        {
                                            if (kv.Value == kitNameToLower)
                                            {
                                                if (kv.Key.isNull || Math.Round((kv.Key.timeLeft / 1000)) > 0)
                                                    b = false;
                                            }
                                        }
                                    }
                                }

                                if (b) // If I am not on cool down for this kit
                                {
                                    kitItems = Vars.kits[kitNameToLower];
                                    Broadcast.noticeTo(senderClient.netPlayer, "☻", "You were given the " + kitName + " kit.");

                                    if (Vars.kitCooldowns.ContainsKey(kitNameToLower)) // If a cooldown is set for this kit, set my cool down
                                    {
                                        if (Vars.kitCooldowns[kitNameToLower] > -1)
                                        {
                                            TimerPlus t = TimerPlus.Create(Vars.kitCooldowns[kitNameToLower], false, Vars.restoreKit, kitNameToLower, senderClient.userID);

                                            if (!Vars.activeKitCooldowns.ContainsKey(senderClient.userID))
                                                Vars.activeKitCooldowns.Add(senderClient.userID, new Dictionary<TimerPlus, string>() { { t, kitNameToLower } });
                                            else
                                                Vars.activeKitCooldowns[senderClient.userID].Add(t, kitNameToLower);
                                        }
                                        else
                                        {
                                            TimerPlus t = new TimerPlus();
                                            t.isNull = true;
                                            if (!Vars.activeKitCooldowns.ContainsKey(senderClient.userID))
                                                Vars.activeKitCooldowns.Add(senderClient.userID, new Dictionary<TimerPlus, string>() { { t, kitNameToLower } });
                                            else
                                                Vars.activeKitCooldowns[senderClient.userID].Add(t, kitNameToLower);
                                        }
                                    }
                                }
                                else // If I am on cool down
                                {
                                    foreach (KeyValuePair<TimerPlus, string> kv in Vars.activeKitCooldowns[senderClient.userID])
                                    {
                                        if (kv.Value == kitNameToLower)
                                        {
                                            if (!kv.Key.isNull)
                                            {
                                                // Return how long I have to wait
                                                double timeLeft = Math.Round((kv.Key.timeLeft / 1000));
                                                TimeSpan timeSpan = TimeSpan.FromMilliseconds(kv.Key.timeLeft);

                                                string timeString = "";

                                                timeString += timeSpan.Hours + " hours, ";
                                                timeString += timeSpan.Minutes + " minutes, and ";
                                                timeString += timeSpan.Seconds + " seconds";
                                                Broadcast.noticeTo(senderClient.netPlayer, "⌛", "You must wait " + (timeLeft > 999999999 ? "forever" : timeString) + " before using this.", 4);
                                            }
                                            else
                                            {
                                                Broadcast.noticeTo(senderClient.netPlayer, "⌛", "You can no longer use this kit!", 4);
                                            }
                                        }
                                    }
                                }
                            }
                            else // If the kit is truly assigned to rank, just not mine
                            {
                                if (Vars.kitsForUIDs.ContainsKey(senderClient.userID))
                                {
                                    if (Vars.kitsForUIDs[senderClient.userID].Contains(kitNameToLower))
                                    {
                                        bool b = true;
                                        if (Vars.activeKitCooldowns.ContainsKey(senderClient.userID))
                                        {
                                            if (Vars.activeKitCooldowns[senderClient.userID].ContainsValue(kitNameToLower))
                                            {
                                                foreach (KeyValuePair<TimerPlus, string> kv in Vars.activeKitCooldowns[senderClient.userID])
                                                {
                                                    if (kv.Value == kitNameToLower)
                                                    {
                                                        if (kv.Key.isNull || Math.Round((kv.Key.timeLeft / 1000)) > 0)
                                                            b = false;
                                                    }
                                                }
                                            }
                                        }

                                        if (b) // If I am not on cool down for this kit
                                        {
                                            kitItems = Vars.kits[kitNameToLower];
                                            Broadcast.noticeTo(senderClient.netPlayer, "☻", "You were given the " + kitName + " kit.");

                                            if (Vars.kitCooldowns.ContainsKey(kitNameToLower)) // If a cooldown is set for this kit, set my cool down
                                            {
                                                if (Vars.kitCooldowns[kitNameToLower] > -1)
                                                {
                                                    TimerPlus t = TimerPlus.Create(Vars.kitCooldowns[kitNameToLower], false, Vars.restoreKit, kitNameToLower, senderClient.userID);

                                                    if (!Vars.activeKitCooldowns.ContainsKey(senderClient.userID))
                                                        Vars.activeKitCooldowns.Add(senderClient.userID, new Dictionary<TimerPlus, string>() { { t, kitNameToLower } });
                                                    else
                                                        Vars.activeKitCooldowns[senderClient.userID].Add(t, kitNameToLower);
                                                }
                                                else
                                                {
                                                    TimerPlus t = new TimerPlus();
                                                    t.isNull = true;
                                                    if (!Vars.activeKitCooldowns.ContainsKey(senderClient.userID))
                                                        Vars.activeKitCooldowns.Add(senderClient.userID, new Dictionary<TimerPlus, string>() { { t, kitNameToLower } });
                                                    else
                                                        Vars.activeKitCooldowns[senderClient.userID].Add(t, kitNameToLower);
                                                }
                                            }
                                        }
                                        else // If I am on cool down
                                        {
                                            foreach (KeyValuePair<TimerPlus, string> kv in Vars.activeKitCooldowns[senderClient.userID])
                                            {
                                                if (kv.Value == kitNameToLower)
                                                {
                                                    if (!kv.Key.isNull)
                                                    {
                                                        // Return how long I have to wait
                                                        double timeLeft = Math.Round((kv.Key.timeLeft / 1000));
                                                        TimeSpan timeSpan = TimeSpan.FromMilliseconds(kv.Key.timeLeft);

                                                        string timeString = "";

                                                        timeString += timeSpan.Hours + " hours, ";
                                                        timeString += timeSpan.Minutes + " minutes, and ";
                                                        timeString += timeSpan.Seconds + " seconds";
                                                        Broadcast.noticeTo(senderClient.netPlayer, "⌛", "You must wait " + (timeLeft > 999999999 ? "forever" : timeString) + " before using this.", 4);
                                                    }
                                                    else
                                                    {
                                                        Broadcast.noticeTo(senderClient.netPlayer, "⌛", "You can no longer use this kit!", 4);
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
                    foreach (Item item in kitItems)
                    {
                        addItemThroughKit(senderClient, item.name, item.amount, item.uses, item.modSlots, item.mods);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("giveK: " + ex.ToString());
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

                        PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(targetName));
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
                        PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(args[1]));
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
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("CREATEI: " + ex.ToString());
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
                        if (!Vars.itemIDs.ContainsValue(itemName))
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
                                if (!Vars.itemIDs.ContainsValue(itemName))
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

                            if (Vars.itemIDs.ContainsValue(itemName))
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
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("CREATEI #2: " + ex.ToString());
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
                        if (Vars.itemIDs.ContainsValue(itemName))
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
                                catch (Exception ex) { Vars.conLog.Error("GAS: " + ex.ToString()); }
                            }

                            List<PlayerClient> playerClients = new List<PlayerClient>();
                            foreach (PlayerClient pc in Vars.AllPlayerClients) { playerClients.Add(pc); }

                            foreach (PlayerClient targetClient in playerClients)
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
                                catch (Exception ex) { Vars.conLog.Error("GAS #2: " + ex.ToString()); }
                            }

                            if (Vars.itemIDs.ContainsValue(itemName))
                            {
                                List<PlayerClient> playerClients = new List<PlayerClient>();
                                foreach (PlayerClient pc in Vars.AllPlayerClients) { playerClients.Add(pc); }

                                foreach (PlayerClient targetClient in playerClients)
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
                Vars.conLog.Error("GAS #3: " + ex.ToString());
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
                        if (!Vars.itemIDs.ContainsValue(itemName))
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
                            List<PlayerClient> playerClients = new List<PlayerClient>();
                            foreach (PlayerClient pc in Vars.AllPlayerClients) { playerClients.Add(pc); }

                            foreach (PlayerClient targetClient in playerClients)
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
                                if (!Vars.itemIDs.ContainsValue(itemName))
                                {
                                    Broadcast.broadcastTo(senderClient.netPlayer, "No such item name \"" + itemName + "\".");
                                    return;
                                }
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

                            if (Vars.itemIDs.ContainsValue(itemName))
                            {
                                List<PlayerClient> playerClients = new List<PlayerClient>();
                                foreach (PlayerClient pc in Vars.AllPlayerClients) { playerClients.Add(pc); }

                                foreach (PlayerClient targetClient in playerClients)
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
                Vars.conLog.Error("GA: " + ex.ToString());
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
                        if (Vars.itemIDs.ContainsValue(itemName))
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
                                catch (Exception ex) { Vars.conLog.Error("GRS: " + ex.ToString()); }

                                try
                                {
                                    playerAmount = Convert.ToInt16(args[lastIndex + 2]);
                                    if (playerAmount < 1)
                                        playerAmount = 1;
                                }
                                catch (Exception ex)
                                {
                                    Vars.conLog.Error("GRS #2: " + ex.ToString());
                                }
                            }

                            if (args.Count() - 1 > lastIndex && args.Count() - 2 == lastIndex)
                            {
                                try
                                {
                                    amount = Convert.ToInt16(args[lastIndex + 1]);
                                }
                                catch (Exception ex) { Vars.conLog.Error("GRS #3: " + ex.ToString()); }
                            }

                            Vars.REB.StartCoroutine(giveawayItem(itemName, amount, playerAmount));
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
                                catch (Exception ex) { Vars.conLog.Error("GRS #4: " + ex.ToString()); }
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
                                catch (Exception ex) { Vars.conLog.Error("GRS #5: " + ex.ToString()); }
                            }

                            if (Vars.itemIDs.ContainsValue(itemName))
                            {
                                Vars.REB.StartCoroutine(giveawayItem(itemName, amount, playerAmount));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("GRS #6: " + ex.ToString());
            }
        }

        public static bool inventoryFull(PlayerClient playerClient)
        {
            if (playerClient == null)
                return true;
            Inventory inventory = playerClient.controllable.GetComponent<Inventory>();
            return inventory.noVacantSlots;
        }

        public static void clearInventory(PlayerClient playerClient)
        {
            if (playerClient == null)
                return;
            Inventory inventory = playerClient.controllable.GetComponent<Inventory>();

            for (int i = 0; i < inventory.slotCount; i++)
            {
                inventory.RemoveItem(i);
            }
        }

        public static void removeItem(PlayerClient playerClient, IInventoryItem item)
        {
            if (playerClient == null)
                return;
            Inventory inventory = playerClient.controllable.GetComponent<Inventory>();
            inventory.RemoveItem(item.slot);
        }

        public static void clearArmor(PlayerClient playerClient)
        {
            if (playerClient == null)
                return;
            Inventory inventory = playerClient.controllable.GetComponent<Inventory>();

            for (int i = 36; i < 40; i++)
            {
                inventory.RemoveItem(i);
            }
        }

        public static bool isItem(string itemName)
        {
            return Array.FindAll(DatablockDictionary.All.ToArray(), (ItemDataBlock idb) => idb.name == itemName).Count() > 0;
        }

        public static bool addHotbarItem(PlayerClient playerClient, string itemName, int amount, int uses, int modSlots, ItemModDataBlock[] mods)
        {
            try
            {
                if (playerClient != null)
                {
                    if (playerClient.controllable != null)
                    {
                        if (playerClient.controllable.GetComponent<Inventory>() != null)
                        {
                            Inventory inventory = playerClient.controllable.GetComponent<Inventory>();

                            if (hasVacantSlots(inventory))
                            {
                                // If I have no free slots on my hot bar or the item is an armor item
                                if (hotbarOccupied(playerClient) || itemName.Contains("Helmet") || itemName.Contains("Vest") || itemName.Contains("Pants") || itemName.Contains("Boots"))
                                {
                                    addItemThroughKit(playerClient, itemName, amount, uses, modSlots, mods);
                                }
                                else
                                {
                                    for (int i = 30; i < 36; i++)
                                    {
                                        if (!inventory.IsSlotOccupied(i))
                                        {
                                            ItemDataBlock itemDB = DatablockDictionary.GetByName(itemName);
                                            if (allGuns.Contains(itemName)) // If the item is a gun
                                            {
                                                Inventory.Addition addition = new Inventory.Addition
                                                {
                                                    ItemDataBlock = itemDB,
                                                    UsesQuantity = (uses > -1 ? uses : itemDB._maxUses),
                                                    SlotPreference = Inventory.Slot.Preference.Define(Inventory.Slot.KindFlags.Belt, Inventory.Slot.KindFlags.Armor | Inventory.Slot.KindFlags.Default).CloneStackChange(itemDB.IsSplittable())
                                                };
                                                for (int i2 = 1; i2 <= amount; i2++)
                                                {
                                                    var item = inventory.AddItem(ref addition);
                                                    if (item is IBulletWeaponItem)
                                                    {
                                                        var gun = (IBulletWeaponItem)item;
                                                        if (modSlots > -1)
                                                            gun.SetTotalModSlotCount(modSlots);
                                                        if (mods != null && mods.Length > 0)
                                                        {
                                                            if (gun.totalModSlots < mods.Length)
                                                                gun.SetTotalModSlotCount(mods.Length);
                                                            foreach (var modDB in mods)
                                                            {
                                                                gun.AddMod(modDB);
                                                            }
                                                        }
                                                        if (uses > -1)
                                                            setUses(item, uses);
                                                    }
                                                }
                                            }
                                            else // If it is anything else (except for armor)
                                            {
                                                inventory.AddItemAmount(itemDB, amount, Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Belt, false, Inventory.Slot.KindFlags.Belt));
                                            }
                                            break;
                                        }
                                    }
                                }
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("AddHI: " + ex.ToString());
            }
            return false;
        }

        public static bool hotbarOccupied(PlayerClient playerClient)
        {
            try
            {
                if (playerClient != null)
                {
                    if (playerClient.controllable != null)
                    {
                        if (playerClient.controllable.GetComponent<Inventory>() != null)
                        {
                            Inventory inventory = playerClient.controllable.GetComponent<Inventory>();

                            bool allTaken = true;
                            for (int i = 30; i < 36; i++)
                            {
                                if (!inventory.IsSlotOccupied(i))
                                    allTaken = false;
                            }
                            return allTaken;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("HBO: " + ex.ToString());
            }
            return true;
        }

        public static void giveInventory(PlayerClient playerClient, List<Items.Item> playerInventory, List<Items.Item> playerArmor)
        {
            List<Items.Item> oldInventory = new List<Items.Item>();
            List<Items.Item> oldArmor = new List<Items.Item>();
            giveInventory(playerClient, playerInventory, playerArmor, out oldInventory, out oldArmor);
        }

        public static void giveInventory(PlayerClient playerClient, List<Items.Item> playerInventory, List<Items.Item> playerArmor, out List<Items.Item> oldInventory, out List<Items.Item> oldArmor)
        {
            List<int> emptySlots = new List<int>();
            oldInventory = new List<Items.Item>();
            oldArmor = new List<Items.Item>();
            try
            {
                if (playerClient != null)
                {
                    if (playerClient.controllable != null)
                    {
                        if (playerClient.controllable.GetComponent<Inventory>() != null)
                        {
                            Inventory inventory = playerClient.controllable.GetComponent<Inventory>();

                            for (int i = 0; i < inventory.slotCount; i++)
                            {
                                IInventoryItem item;
                                if (inventory.GetItem(i, out item))
                                {
                                    if (item != null)
                                    {
                                        if (item.datablock != null)
                                        {
                                            if (i > 35 && i < 40)
                                                oldArmor.Add(new Items.Item(item.datablock.name, item.uses));
                                            else
                                                oldInventory.Add(new Items.Item(item.datablock.name, item.uses));
                                        }
                                    }
                                }
                                else
                                {
                                    if (i > 35 && i < 40)
                                        oldArmor.Add(null);
                                    else
                                        oldInventory.Add(null);
                                }
                            }

                            clearInventory(playerClient);

                            foreach (Item item in playerArmor)
                            {
                                if (item != null)
                                {
                                    addArmor(playerClient, item.name, item.amount);
                                }
                            }

                            int curIndex = 0;
                            foreach (Item item in playerInventory)
                            {
                                if (item != null)
                                {
                                    if (!addGun(playerClient, item.name, 1, item.amount, item.modSlots, item.mods))
                                    {
                                        inventory.AddItemAmount(DatablockDictionary.GetByName(item.name), (item.name == "Torch" ? 1 : item.amount));
                                    }
                                }
                                else
                                {
                                    emptySlots.Add(curIndex);
                                    inventory.AddItemAmount(DatablockDictionary.GetByName("Armor Part 1 BP"), 1);
                                }
                                curIndex++;
                            }

                            foreach (int i in emptySlots)
                            {
                                inventory.RemoveItem(i);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("GetI: " + ex.ToString());
            }
        }

        public static void getInventory(PlayerClient playerClient, out List<Items.Item> playerInventory, out List<Items.Item> playerArmor)
        {
            playerInventory = new List<Items.Item>();
            playerArmor = new List<Items.Item>();
            try
            {
                if (playerClient != null)
                {
                    if (playerClient.controllable != null)
                    {
                        if (playerClient.controllable.GetComponent<Inventory>() != null)
                        {
                            Inventory inventory = playerClient.controllable.GetComponent<Inventory>();

                            for (int i = 0; i < inventory.slotCount; i++)
                            {
                                IInventoryItem item;
                                if (inventory.GetItem(i, out item))
                                {
                                    if (item != null)
                                    {
                                        if (item.datablock != null)
                                        {
                                            if (i > 35 && i < 40)
                                                playerArmor.Add(new Items.Item(item.datablock.name, item.uses));
                                            else
                                            {
                                                if (allGuns.Contains(item.datablock.name))
                                                {
                                                    var gun = (IBulletWeaponItem)item;
                                                    playerInventory.Add(new Items.Item(item.datablock.name, 1, item.uses, gun.totalModSlots, gun.itemMods));
                                                }
                                                else
                                                    playerInventory.Add(new Items.Item(item.datablock.name, item.uses));
                                            }
                                        }
                                        else
                                        {
                                            if (i > 35 && i < 40)
                                                playerArmor.Add(null);
                                            else
                                                playerInventory.Add(null);
                                        }
                                    }
                                    else
                                    {
                                        if (i > 35 && i < 40)
                                            playerArmor.Add(null);
                                        else
                                            playerInventory.Add(null);
                                    }
                                }
                                else
                                {
                                    if (i > 35 && i < 40)
                                        playerArmor.Add(null);
                                    else
                                        playerInventory.Add(null);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("GetI #2: " + ex.ToString());
            }
        }

        public static bool addItemThroughKit(PlayerClient playerClient, string itemName, int amount, int uses, int modSlots, ItemModDataBlock[] mods)
        {
            try
            {
                if (playerClient != null)
                {
                    if (playerClient.controllable != null)
                    {
                        if (playerClient.controllable.GetComponent<Inventory>() != null)
                        {
                            Inventory inventory = playerClient.controllable.GetComponent<Inventory>();

                            if (addArmor(playerClient, itemName, amount))
                            {
                                return true;
                            }
                            else
                            {
                                if (addGun(playerClient, itemName, amount, uses, modSlots, mods))
                                {
                                    return true;
                                }
                                else
                                {
                                    if (hasVacantSlots(inventory) || (hasStackableSlots(playerClient, inventory, DatablockDictionary.GetByName(itemName), uses, modSlots, mods, amount) && !Vars.enableDropItem))
                                    {
                                        inventory.AddItemAmount(DatablockDictionary.GetByName(itemName), amount);
                                        return true;
                                    }
                                    else if (Vars.enableDropItem)
                                    {
                                        return hasStackableSlotsAndDrop(playerClient, inventory, DatablockDictionary.GetByName(itemName), -1, -1, null, ref amount);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("AddITK: " + ex.ToString());
            }
            return false;
        }

        public static bool addItem(PlayerClient playerClient, string itemName, int amount)
        {
            try
            {
                if (playerClient != null)
                {
                    if (playerClient.controllable != null)
                    {
                        if (playerClient.controllable.GetComponent<Inventory>() != null)
                        {
                            Inventory inventory = playerClient.controllable.GetComponent<Inventory>();

                            if (addArmor(playerClient, itemName, amount))
                            {
                                return true;
                            }
                            else
                            {
                                if (addGun(playerClient, itemName, amount, -1, -1, null))
                                {
                                    return true;
                                }
                                else
                                {
                                    if (hasVacantSlots(inventory) || (hasStackableSlots(playerClient, inventory, DatablockDictionary.GetByName(itemName), -1, -1, null, amount) && !Vars.enableDropItem))
                                    {
                                        inventory.AddItemAmount(DatablockDictionary.GetByName(itemName), amount);
                                        return true;
                                    }
                                    else if (Vars.enableDropItem)
                                    {
                                        return hasStackableSlotsAndDrop(playerClient, inventory, DatablockDictionary.GetByName(itemName), -1, -1, null, ref amount);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("AddI: " + ex.ToString());
            }
            return false;
        }

        public static bool hasStackableSlots(PlayerClient playerClient, Inventory inventory, ItemDataBlock itemDB, int uses, int modSlots, ItemModDataBlock[] mods, int amount)
        {
            bool hasStackableSlot = false;

            if (itemDB is BulletWeaponDataBlock || itemDB is ArmorDataBlock)
                return false;

            List<IInventoryItem> items = new List<IInventoryItem>();
            grabItem(playerClient, itemDB.name, out items);
            if (items != null)
            {
                items = Array.FindAll(items.ToArray(), (IInventoryItem item) => item.uses < ((InventoryItem)item).maxUses).ToList();
                if (items != null)
                    hasStackableSlot = items.Count > 0;
            }

            return hasStackableSlot;
        }

        public static bool hasStackableSlotsAndDrop(PlayerClient playerClient, Inventory inventory, ItemDataBlock itemDB, int uses, int modSlots, ItemModDataBlock[] mods, ref int amount)
        {
            bool hasStackableSlot = false;

            if (itemDB is BulletWeaponDataBlock || itemDB is ArmorDataBlock)
                return false;

            List<IInventoryItem> items = new List<IInventoryItem>();
            grabItem(playerClient, itemDB.name, out items);
            items = Array.FindAll(items.ToArray(), (IInventoryItem item) => item.uses < ((InventoryItem)item).maxUses).ToList();
            hasStackableSlot = items.Count > 0;
            int amountToDrop = amount;

            foreach (IInventoryItem item in items)
            {
                int takeAway = ((InventoryItem)item).maxUses - item.uses;

                if (amountToDrop >= takeAway)
                {
                    amountToDrop -= takeAway;
                }
                else
                    amountToDrop = 0;

                if (amountToDrop == 0)
                    break;
            }

            amount -= amountToDrop;

            if (hasStackableSlot)
                inventory.AddItemAmount(itemDB, amount);

            if (Vars.enableDropItem)
            {
                if (Vars.dropItemMessage != "")
                    Broadcast.broadcastTo(playerClient.netPlayer, Vars.dropItemMessage);
                if (amountToDrop > 0)
                    dropItem(playerClient, itemDB.name, amountToDrop, uses, modSlots, mods);
            }

            return hasStackableSlot;
        }

        public static bool hasVacantSlots(Inventory inventory)
        {
            bool hasEmptySlot = false;

            for (int i = 0; i < inventory.slotCount; i++)
            {
                if (i < 36 || i > 39)
                {
                    IInventoryItem item;
                    inventory.GetItem(i, out item);
                    if (item == null)
                    {
                        hasEmptySlot = true;
                        break;
                    }
                }
            }

            return hasEmptySlot;
        }

        public static void setUses(IInventoryItem item, int uses)
        {
            ((InventoryItem)item).SetUsesBypass(uses);
            item.MarkDirty();
        }

        public static void dropItem(PlayerClient playerClient, string itemName, int amount, int uses, int modSlots, ItemModDataBlock[] mods)
        {
            try
            {
                if (playerClient != null && playerClient.controllable != null)
                {
                    Inventory inventory = playerClient.controllable.GetComponent<Inventory>();

                    Character idMain = inventory.idMain as Character;
                    if (idMain != null)
                    {
                        IInventoryItem item = null;

                        IInventoryItem originalItem = null;
                        inventory.GetItem(0, out originalItem);

                        inventory.RemoveItem(0);
                        Inventory.Addition addition = new Inventory.Addition
                        {
                            ItemDataBlock = DatablockDictionary.GetByName(itemName),
                            UsesQuantity = (DatablockDictionary.GetByName(itemName) is BulletWeaponDataBlock ? 1 : amount),
                            SlotPreference = Inventory.Slot.Preference.Define(Inventory.Slot.KindFlags.Default, Inventory.Slot.KindFlags.Armor | Inventory.Slot.KindFlags.Belt).CloneStackChange(false)
                        };
                        inventory.AddItem(ref addition);
                        inventory.GetItem(0, out item);

                        if (item == null)
                            Vars.conLog.Info("DI: Item is null!");

                        if (item is IBulletWeaponItem)
                        {
                            var gun = (IBulletWeaponItem)item;
                            if (modSlots > -1)
                                gun.SetTotalModSlotCount(modSlots);
                            if (mods != null && mods.Length > 0)
                            {
                                if (gun.totalModSlots < mods.Length)
                                    gun.SetTotalModSlotCount(mods.Length);
                                foreach (ItemModDataBlock modDB in mods)
                                {
                                    gun.AddMod(modDB);
                                }
                            }

                            setUses(item, uses > -1 ? uses : DatablockDictionary.GetByName(itemName)._maxUses);
                        }
                        else if (!(item is IArmorItem))
                        {
                            setUses(item, amount);
                        }

                        CharacterItemDropPrefabTrait trait = idMain.GetTrait<CharacterItemDropPrefabTrait>();
                        //Vector3 arg = (Vector3)(idMain.eyesAngles.forward * UnityEngine.Random.Range((float)4f, (float)6f));
                        //Vector3 position = idMain.eyesOrigin - new Vector3(0f, 0.25f, 0f);
                        Vector3 position = idMain.transform.position;
                        Quaternion rotation = Quaternion.LookRotation(Vector3.forward);

                        List<string> modNames = new List<string>();
                        if (mods != null)
                        {
                            foreach (ItemModDataBlock mod in mods)
                            {
                                modNames.Add(mod.name);
                            }
                        }

                        for (int i = 0; i < ((item is IArmorItem || item is IBulletWeaponItem) ? amount : 1); i++)
                        {
                            GameObject go = NetCull.InstantiateDynamicWithArgs<Vector3>(trait.prefab, position, rotation, new Vector3());
                            ItemPickup dropped = go.GetComponent<ItemPickup>();
                            if (!dropped.SetPickupItem(item))
                            {
                                Vars.conLog.Error("Could not create and drop [" + itemName + ":" + amount + ":" + uses + ":" + modSlots + ":" + string.Join(",", modNames.ToArray()) + "] for " + playerClient.userName + " (" + playerClient.userID + "):");
                                NetCull.Destroy(go);
                            }
                        }

                        inventory.RemoveItem(0);
                        if (originalItem != null)
                        {
                            if (originalItem is IBulletWeaponItem)
                            {
                                IBulletWeaponItem gun = (IBulletWeaponItem)item;
                                addItemThroughKit(playerClient, originalItem.datablock.name, 1, gun.uses, gun.totalModSlots, (originalItem is IBulletWeaponItem ? (gun.usedModSlots > 0 ? gun.itemMods : null) : null));
                            }
                            else
                            {
                                addition = new Inventory.Addition
                                {
                                    ItemDataBlock = originalItem.datablock,
                                    UsesQuantity = originalItem.uses,
                                    SlotPreference = Inventory.Slot.Preference.Define(Inventory.Slot.KindFlags.Default, Inventory.Slot.KindFlags.Armor | Inventory.Slot.KindFlags.Belt).CloneStackChange(false)
                                };
                                inventory.AddItem(ref addition);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("Could not create and drop [" + itemName + ":" + amount + ":" + uses + ":" + modSlots + "] for " + playerClient.userName + " (" + playerClient.userID + "):");
                Vars.conLog.Error(ex.ToString());
            }
        }

        public static void updateItemInfo(ItemPickup IP, ItemDataBlock datablock, int uses)
        {
            NetEntityID entID = NetEntityID.Get(IP);
            if (IP.sentItemInfo != 0)
            {
                if (IP.sentItemInfo == 1)
                {
                    NetCull.RemoveRPCsByName(entID, "PKIS");
                }
                else
                {
                    NetCull.RemoveRPCsByName(entID, "PKIF");
                }
                IP.sentItemInfo = 0;
            }
            if (datablock.IsSplittable() && uses > 1)
            {
                NetCull.RPC<int, byte>(entID, "PKIF", uLink.RPCMode.OthersBuffered, datablock.uniqueID, (byte)uses);
                IP.sentItemInfo = 2;
            }
            else
            {
                NetCull.RPC<int>(entID, "PKIS", uLink.RPCMode.OthersBuffered, datablock.uniqueID);
                IP.sentItemInfo = 1;
            }
        }

        public static List<string> allGuns = new List<string>()
        {
            "P250",
            "Shotgun",
            "HandCannon",
            "Revolver",
            "Pipe Shotgun",
            "MP5A4",
            "M4",
            "9mm Pistol",
            "Bolt Action Rifle"
        };

        public static bool addGun(PlayerClient playerClient, string itemName, int amount, int uses, int modslots, ItemModDataBlock[] mods)
        {
            bool didDropItems;
            bool result = addGunThroughKit(playerClient, itemName, amount, uses, modslots, mods, out didDropItems);
            if (didDropItems && Vars.dropItemMessage != "")
                Broadcast.broadcastTo(playerClient.netPlayer, Vars.dropItemMessage);
            return result;
        }

        public static bool addGunThroughKit(PlayerClient playerClient, string itemName, int amount, int uses, int modslots, ItemModDataBlock[] mods, out bool didDropItems)
        {
            didDropItems = false;
            if (playerClient != null && playerClient.controllable != null)
            {
                Inventory inventory = playerClient.controllable.GetComponent<Inventory>();
                if (hasVacantSlots(inventory) && allGuns.Contains(itemName))
                {
                    ItemDataBlock itemDB = DatablockDictionary.GetByName(itemName);
                    amount = Math.Min(amount, 50);
                    for (int i = 1; i <= amount; i++)
                    {
                        if (hasVacantSlots(inventory))
                        {
                            Inventory.Addition addition = new Inventory.Addition
                            {
                                ItemDataBlock = itemDB,
                                UsesQuantity = (uses > -1 ? uses : itemDB._maxUses),
                                SlotPreference = Inventory.Slot.Preference.Define(Inventory.Slot.KindFlags.Default, Inventory.Slot.KindFlags.Armor | Inventory.Slot.KindFlags.Belt).CloneStackChange(itemDB.IsSplittable())
                            };

                            var item = inventory.AddItem(ref addition);
                            if (item is IBulletWeaponItem)
                            {
                                var gun = (IBulletWeaponItem)item;
                                if (modslots > -1)
                                    gun.SetTotalModSlotCount(modslots);
                                if (mods != null && mods.Length > 0)
                                {
                                    if (gun.totalModSlots < mods.Length)
                                        gun.SetTotalModSlotCount(mods.Length);
                                    foreach (ItemModDataBlock modDB in mods)
                                    {
                                        gun.AddMod(modDB);
                                    }
                                }

                                if (uses > -1)
                                    setUses(item, uses);
                            }
                        }
                        else if (Vars.enableDropItem)
                        {
                            dropItem(playerClient, itemName, 1, uses, modslots, mods);
                            didDropItems = true;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public static bool addArmor(PlayerClient playerClient, string itemName, int amount, bool replaceCurrent = false, bool autoEquip = true)
        {
            bool didDropItems;
            bool result = addArmorThroughKit(playerClient, itemName, amount, out didDropItems, replaceCurrent, autoEquip);
            if (didDropItems && Vars.dropItemMessage != "")
                Broadcast.broadcastTo(playerClient.netPlayer, Vars.dropItemMessage);
            return result;
        }

        public static bool addArmorThroughKit(PlayerClient playerClient, string itemName, int amount, out bool didDropItems, bool replaceCurrent = false, bool autoEquip = true)
        {
            didDropItems = false;
            if (playerClient != null && playerClient.controllable != null)
            {
                Inventory inventory = playerClient.controllable.GetComponent<Inventory>();
                if (inventory != null)
                {
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

                        amount = Math.Min(amount, 50);
                        for (int i = 1; i <= amount; i++)
                        {
                            if (!inventory.IsSlotOccupied(slot) && autoEquip)
                                inventory.AddItemAmount(DatablockDictionary.GetByName(itemName), 1, Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Armor, false, Inventory.Slot.KindFlags.Armor));
                            else
                            {
                                if (hasVacantSlots(inventory))
                                    inventory.AddItemAmount(DatablockDictionary.GetByName(itemName), 1);
                                else if (Vars.enableDropItem)
                                {
                                    dropItem(playerClient, itemName, 1, -1, -1, null);
                                    didDropItems = true;
                                }
                            }
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool grabArmor(PlayerClient playerClient, out List<IInventoryItem> items)
        {
            items = new List<IInventoryItem>();
            if (playerClient != null && playerClient.controllable != null)
            {
                Inventory inventory = playerClient.controllable.GetComponent<Inventory>();
                for (int i = 36; i < 40; i++)
                {
                    IInventoryItem item;
                    if (inventory.GetItem(i, out item))
                    {
                        try
                        {
                            if (item != null)
                                items.Add(item);
                        }
                        catch (Exception ex) { Vars.conLog.Error("GRABA: " + ex.ToString()); }
                    }
                }
            }
            return items.Count() > 0;
        }

        public static bool grabItem(PlayerClient playerClient, string itemName, out List<IInventoryItem> items)
        {
            items = new List<IInventoryItem>();
            if (playerClient != null && playerClient.controllable != null)
            {
                Inventory inventory = playerClient.controllable.GetComponent<Inventory>();
                for (int i = 0; i < inventory.slotCount; i++)
                {
                    IInventoryItem item;
                    if (inventory.GetItem(i, out item))
                    {
                        try
                        {
                            if (item.datablock.name == itemName)
                                items.Add(item);
                        }
                        catch (Exception ex) { Vars.conLog.Error("GRABI: " + ex.ToString()); }
                    }
                }
            }
            return items.Count() > 0;
        }

        public static List<string> allMelees = new List<string>()
        {
            "Rock",
            "Stone Hatchet",
            "Hatchet",
            "Pick Axe"
        };

        public static bool hasMelee(PlayerClient playerClient)
        {
            if (playerClient != null)
            {
                if (playerClient.controllable != null)
                {
                    Inventory inventory = playerClient.controllable.GetComponent<Inventory>();
                    if (inventory != null)
                    {
                        for (int i = 0; i < inventory.slotCount; i++)
                        {
                            IInventoryItem item2;
                            if (inventory.GetItem(i, out item2))
                            {
                                try
                                {
                                    if (allMelees.Contains(item2.datablock.name))
                                        return true;
                                }
                                catch (Exception ex) { Vars.conLog.Error("HMEL: " + ex.ToString()); }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static bool hasItem(PlayerClient playerClient, string itemName)
        {
            if (playerClient != null)
            {
                if (playerClient.controllable != null)
                {
                    Inventory inventory = playerClient.controllable.GetComponent<Inventory>();
                    if (inventory != null)
                    {
                        for (int i = 0; i < inventory.slotCount; i++)
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
                                catch (Exception ex) { Vars.conLog.Error("HASI: " + ex.ToString()); }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static void listItems(PlayerClient playerClient)
        {
            if (playerClient != null)
            {
                if (playerClient.controllable != null)
                {
                    Inventory inventory = playerClient.controllable.GetComponent<Inventory>();
                    if (inventory != null)
                    {
                        for (int i = 0; i < inventory.slotCount; i++)
                        {
                            IInventoryItem item2;
                            if (inventory.GetItem(i, out item2))
                            {
                                try
                                {
                                    Broadcast.broadcastTo(playerClient.netPlayer, i + ": " + item2.datablock.name);
                                }
                                catch (Exception ex) { Vars.conLog.Error("LISTI: " + ex.ToString()); }
                            }
                        }
                    }
                }
            }
        }

        public static bool hasItem(PlayerClient playerClient, string itemName, out IInventoryItem item)
        {
            if (playerClient != null)
            {
                if (playerClient.controllable != null)
                {
                    Inventory inventory = playerClient.controllable.GetComponent<Inventory>();
                    if (inventory != null)
                    {
                        for (int i = 0; i < inventory.slotCount; i++)
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
                                catch (Exception ex) { Vars.conLog.Error("HASI #2: " + ex.ToString()); }
                            }
                        }
                    }
                }
            }
            item = null;
            return false;
        }

        public static void giveRandom(PlayerClient senderClient, string[] args)
        {
            try
            {
                if (senderClient != null && senderClient.netPlayer != null)
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
                            if (!Vars.itemIDs.ContainsValue(itemName))
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
                                        Broadcast.broadcastTo(senderClient.netPlayer, "Player amount must be an integer!");
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

                                Vars.REB.StartCoroutine(giveawayItem(itemName, amount, playerAmount));
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
                                    if (!Vars.itemIDs.ContainsValue(itemName))
                                    {
                                        Broadcast.broadcastTo(senderClient.netPlayer, "No such item name \"" + itemName + "\".");
                                        return;
                                    }
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

                                if (Vars.itemIDs.ContainsValue(itemName))
                                {
                                    Vars.REB.StartCoroutine(giveawayItem(itemName, amount, playerAmount));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("GIVER: " + ex.ToString());
            }
        }

        public static ItemDataBlock getHeldItem(PlayerClient playerClient)
        {
            if (playerClient.controllable.GetComponent<Inventory>() != null)
            {
                if (playerClient.controllable.GetComponent<Inventory>().activeItem != null)
                {
                    if (playerClient.controllable.GetComponent<Inventory>().activeItem.datablock != null)
                    {
                        return playerClient.controllable.GetComponent<Inventory>().activeItem.datablock;
                    }
                }
            }
            return null;
        }

        public static void redeemVoteKit(PlayerClient senderClient)
        {
            try
            {
                if (senderClient != null)
                {
                    if (Vars.enableRSVoting && !Vars.enableTRSVoting)
                    {
                        try
                        {
                            Thread t = new Thread(() =>
                            {
                                string result = "0";
                                try
                                {
                                    using (WebClient wc = new WebClient())
                                    {
                                        wc.Proxy = null;
                                        result = wc.DownloadString("http://rust-servers.net/api/?object=votes&element=claim&key=" + Vars.RSAPIKey + "&steamid=" + senderClient.userID);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Vars.conLog.Error("VOTED #12: " + ex.ToString());
                                }
                                if (result == "1")
                                {
                                    try
                                    {
                                        if (Vars.RSItems.Count() > 0)
                                        {
                                            Broadcast.noticeTo(senderClient.netPlayer, ":)", "Thank you for voting! Your reward is in your inventory.", 5, true);
                                            string message = Vars.RSvotingMessage.Replace("$PLAYER$", senderClient.netUser.displayName);
                                            Broadcast.broadcastAll(message);
                                            try
                                            {
                                                foreach (Item item in Vars.RSItems)
                                                {
                                                    addItemThroughKit(senderClient, item.name, item.amount, item.uses, item.modSlots, item.mods);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Vars.conLog.Error("VOTED #14: " + ex.ToString());
                                            }
                                            try
                                            {
                                                using (WebClient wc = new WebClient())
                                                {
                                                    wc.Proxy = null;
                                                    wc.DownloadString("http://rust-servers.net/api/?action=post&object=votes&element=claim&key=" + Vars.RSAPIKey + "&steamid=" + senderClient.userID);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Vars.conLog.Error("VOTED #19: " + ex.ToString());
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Vars.conLog.Error("VOTED #13: " + ex.ToString());
                                    }
                                }
                                else if (result == "Error: no server key" || result == "Error: incorrect server key")
                                {
                                    Broadcast.broadcastTo(senderClient.netPlayer, "The voter RSAPIKey is invalid! If you are not a server admin, let them know!");
                                }
                                else if (result == "0")
                                {
                                    Broadcast.noticeTo(senderClient.netPlayer, ":(", "You have not voted yet. Type /vote!", 5, true);
                                }
                                else if (result == "2")
                                {
                                    Broadcast.noticeTo(senderClient.netPlayer, ":(", "You have already redeemed your reward!", 5, true);
                                }
                            });
                            t.IsBackground = true;
                            t.Start();
                        }
                        catch (Exception ex)
                        {
                            Vars.conLog.Error("VOTED #11: " + ex.ToString());
                        }
                    }
                    else if (Vars.enableRSVoting && Vars.enableTRSVoting)
                    {
                        try
                        {
                            Thread t = new Thread(() =>
                            {
                                string TRSresult = "0";
                                string result = "0";
                                try
                                {
                                    using (WebClient wc = new WebClient())
                                    {
                                        wc.Proxy = null;
                                        result = wc.DownloadString("http://rust-servers.net/api/?object=votes&element=claim&key=" + Vars.RSAPIKey + "&steamid=" + senderClient.userID);
                                        TRSresult = wc.DownloadString("http://api.toprustservers.com/api/get?plugin=voter&key=" + Vars.TRSAPIKey + "&uid=" + senderClient.userID);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Vars.conLog.Error("VOTED #15: " + ex.ToString());
                                }

                                if (TRSresult == "1" || result == "1")
                                    Broadcast.noticeTo(senderClient.netPlayer, ":)", "Thank you for voting! Your reward is in your inventory.", 5, true);

                                if (TRSresult != "1" && result != "1" && result != "Error: no server key" && result != "Error: incorrect server key" && TRSresult != "invalid_api")
                                    Broadcast.noticeTo(senderClient.netPlayer, ":(", "You either already redeemed your reward or have not voted yet!", 5, true);

                                if (TRSresult == "1")
                                {
                                    try
                                    {
                                        if (Vars.TRSItems.Count() > 0)
                                        {
                                            string message = Vars.TRSvotingMessage.Replace("$PLAYER$", senderClient.netUser.displayName);
                                            Broadcast.broadcastAll(message);
                                            try
                                            {
                                                foreach (Item item in Vars.TRSItems)
                                                {
                                                    addItemThroughKit(senderClient, item.name, item.amount, item.uses, item.modSlots, item.mods);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Vars.conLog.Error("VOTED #9: " + ex.ToString());
                                            }
                                            try
                                            {
                                                using (WebClient wc = new WebClient())
                                                {
                                                    wc.Proxy = null;
                                                    wc.DownloadString("http://api.toprustservers.com/api/put?plugin=voter&key=" + Vars.TRSAPIKey + "&uid=" + senderClient.userID);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Vars.conLog.Error("VOTED #10: " + ex.ToString());
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Vars.conLog.Error("VOTED #8: " + ex.ToString());
                                    }
                                }
                                else if (TRSresult == "invalid_api")
                                {
                                    Broadcast.broadcastTo(senderClient.netPlayer, "The voter TRSAPIKey is invalid! If you are not a server admin, let one know!");
                                }

                                if (result == "1")
                                {
                                    try
                                    {
                                        if (Vars.RSItems.Count() > 0)
                                        {
                                            string message = Vars.RSvotingMessage.Replace("$PLAYER$", senderClient.netUser.displayName);
                                            Broadcast.broadcastAll(message);
                                            try
                                            {
                                                foreach (Item item in Vars.RSItems)
                                                {
                                                    addItemThroughKit(senderClient, item.name, item.amount, item.uses, item.modSlots, item.mods);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Vars.conLog.Error("VOTED #17: " + ex.ToString());
                                            }
                                            try
                                            {
                                                using (WebClient wc = new WebClient())
                                                {
                                                    wc.Proxy = null;
                                                    wc.DownloadString("http://rust-servers.net/api/?action=post&object=votes&element=claim&key=" + Vars.RSAPIKey + "&steamid=" + senderClient.userID);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Vars.conLog.Error("VOTED #18: " + ex.ToString());
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Vars.conLog.Error("VOTED #16: " + ex.ToString());
                                    }
                                }
                                else if (result == "Error: no server key" || result == "Error: incorrect server key")
                                {
                                    Broadcast.broadcastTo(senderClient.netPlayer, "The voter RSAPIKey is invalid! If you are not a server admin, let one know!");
                                }
                            });
                            t.IsBackground = true;
                            t.Start();
                        }
                        catch (Exception ex)
                        {
                            Vars.conLog.Error("VOTED #7: " + ex.ToString());
                        }
                    }
                    else if (Vars.enableTRSVoting && !Vars.enableRSVoting)
                    {
                        try
                        {
                            Thread t = new Thread(() =>
                            {
                                string result = "0";
                                try
                                {
                                    using (WebClient wc = new WebClient())
                                    {
                                        wc.Proxy = null;
                                        result = wc.DownloadString("http://api.toprustservers.com/api/get?plugin=voter&key=" + Vars.TRSAPIKey + "&uid=" + senderClient.userID);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Vars.conLog.Error("VOTED #3: " + ex.ToString());
                                }
                                if (result == "1")
                                {
                                    try
                                    {
                                        if (Vars.TRSItems.Count() > 0)
                                        {
                                            Broadcast.noticeTo(senderClient.netPlayer, ":)", "Thank you for voting! Your reward is in your inventory.", 5, true);
                                            string message = Vars.TRSvotingMessage.Replace("$PLAYER$", senderClient.netUser.displayName);
                                            Broadcast.broadcastAll(message);
                                            try
                                            {
                                                foreach (Item item in Vars.TRSItems)
                                                {
                                                    addItemThroughKit(senderClient, item.name, item.amount, item.uses, item.modSlots, item.mods);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Vars.conLog.Error("VOTED #5: " + ex.ToString());
                                            }
                                            try
                                            {
                                                using (WebClient wc = new WebClient())
                                                {
                                                    wc.Proxy = null;
                                                    result = wc.DownloadString("http://api.toprustservers.com/api/put?plugin=voter&key=" + Vars.TRSAPIKey + "&uid=" + senderClient.userID);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Vars.conLog.Error("VOTED #6: " + ex.ToString());
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Vars.conLog.Error("VOTED #4: " + ex.ToString());
                                    }
                                }
                                else if (result == "invalid_api")
                                {
                                    Broadcast.broadcastTo(senderClient.netPlayer, "The voter TRSAPIKey is invalid! If you are not a server admin, let them know!");
                                }
                                else
                                {
                                    Broadcast.noticeTo(senderClient.netPlayer, ":(", "You either already redeemed your reward or have not voted yet!", 5, true);
                                }
                            });
                            t.IsBackground = true;
                            t.Start();
                        }
                        catch (Exception ex)
                        {
                            Vars.conLog.Error("VOTED #2: " + ex.ToString());
                        }
                    }
                    else
                        Broadcast.broadcastTo(senderClient.netPlayer, "Voting is not enabled on this server.");
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("VOTED #1: " + ex.ToString());
            }
        }

        public static IEnumerator giveawayItem(string itemName, int amount, int playerAmount)
        {
            Broadcast.noticeAll("?", "Starting item giveaway! " + playerAmount + " winners will be picked at random!", 5);
            yield return new WaitForSeconds(5);
            Broadcast.noticeAll("?", "3", 1);
            yield return new WaitForSeconds(1);
            Broadcast.noticeAll("?", "2", 1);
            yield return new WaitForSeconds(1);
            Broadcast.noticeAll("?", "1", 1);
            yield return new WaitForSeconds(1);

            try
            {
                List<PlayerClient> winners = new List<PlayerClient>();
                List<string> winnerNames = new List<string>();
                List<PlayerClient> possibleWinners = new List<PlayerClient>();
                List<PlayerClient> playerClients = new List<PlayerClient>();
                foreach (PlayerClient pc in Vars.AllPlayerClients) { playerClients.Add(pc); }

                foreach (PlayerClient pc in playerClients)
                {
                    if (pc != null)
                    {
                        if (pc.controllable != null)
                        {
                            if (pc.controllable.GetComponent<Inventory>() != null)
                            {
                                if (!Vars.vanishedList.Contains(pc.userID) && !Vars.lastWinners.Contains(pc.userID) && pc.userName.Length > 0)
                                {
                                    possibleWinners.Add(pc);
                                }
                            }
                        }
                    }
                }
                Vars.lastWinners.Clear();
                string winnerList = "";
                if (playerAmount < 1) playerAmount = 1;
                if (playerAmount > possibleWinners.Count) playerAmount = possibleWinners.Count;
                int curIndex = 0;
                while (curIndex < playerAmount)
                {
                    if (Vars.AllPlayerClients.Count < playerAmount) break;
                    PlayerClient randomClient = possibleWinners[UnityEngine.Random.Range(0, possibleWinners.Count)];

                    if (randomClient != null)
                    {
                        if (Vars.AllPlayerClients.Contains(randomClient))
                        {
                            winners.Add(randomClient);
                            winnerNames.Add(randomClient.userName);
                            Vars.lastWinners.Add(randomClient.userID);
                            possibleWinners.Remove(randomClient);
                            curIndex++;
                        }
                    }
                }

                if (winnerNames.Count == 2)
                    winnerList = string.Join(" and ", winnerNames.ToArray());
                else if (winnerNames.Count > 2 && winnerNames.Count <= 10)
                {
                    string lastName = winnerNames.Last();
                    winnerNames.Remove(lastName);

                    winnerList = string.Join(", ", winnerNames.ToArray());
                    winnerList += ", and " + lastName;
                }
                else if (winnerNames.Count > 10)
                {
                    List<string> firstTen = new List<string>();
                    int eachIndex = 0;
                    foreach (string name in winnerNames)
                    {
                        eachIndex++;
                        if (eachIndex > 10)
                        {
                            firstTen.Add(name);
                        }
                        else
                            break;
                    }

                    winnerList = string.Join(", ", firstTen.ToArray());
                    winnerList += ", and " + (winnerNames.Count - firstTen.Count) + " others";
                    Broadcast.broadcastAll("Congratulations to these players on winning " + amount + " " + itemName + ":");
                    List<string> names = new List<string>();
                    List<string> names2 = new List<string>();
                    foreach (string name in winnerNames)
                    {
                        if (name.Length > 0)
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
                        Broadcast.broadcastAll(string.Join(", ", names2.ToArray()));
                    }
                }
                else if (winnerNames.Count == 1)
                    winnerList = winnerNames.First();
                Broadcast.noticeAll("!", "Congratulations to " + winnerList + " on winning " + amount + "x " + itemName + "!", 5);
                foreach (PlayerClient targetClient in winners)
                {
                    if (targetClient != null)
                    {
                        addItem(targetClient, itemName, amount);
                        if (targetClient.netPlayer != null)
                            Broadcast.sideNoticeTo(targetClient.netPlayer, "You won " + amount + "x " + itemName + "!");
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("GAI: " + ex.ToString());
            }
        }
    }
}
