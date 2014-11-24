using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;

namespace RustEssentials.Util
{
    public class Homes
    {
        public static HomeList homes = new HomeList();

        public static void setHome(PlayerClient senderClient, string[] args)
        {
            HomeList myHomes = homes.GetByUID(senderClient.userID);
            Character playerChar;
            if (Vars.getPlayerChar(senderClient, out playerChar))
            {
                if (args.Count() > 1)
                {
                    Home home = myHomes.GetByName(args[1]);
                    if (home != null)
                    {
                        homes.Remove(home);
                        home.x = playerChar.eyesOrigin.x;
                        home.y = playerChar.eyesOrigin.y;
                        home.z = playerChar.eyesOrigin.z;
                        homes.Add(home);
                        Broadcast.broadcastTo(senderClient.netPlayer, "Home " + (Vars.homeLimit == 1 ? ("[" + args[1] + "] ") : "") + "reset! " + playerChar.eyesOrigin);
                        Data.saveHomes();
                    }
                    else if (myHomes.Count < Vars.homeLimit || Vars.homeLimit == 0)
                    {
                        home = new Home(args[1], senderClient.userID, playerChar.eyesOrigin.x, playerChar.eyesOrigin.y, playerChar.eyesOrigin.z);
                        homes.Add(home);
                        myHomes = homes.GetByUID(senderClient.userID);
                        Broadcast.broadcastTo(senderClient.netPlayer, "Home [" + home.name + "] set! " + playerChar.eyesOrigin);
                        Broadcast.broadcastTo(senderClient.netPlayer, "You now have " + myHomes.Count + (Vars.homeLimit > 0 ? "/" + Vars.homeLimit : "") + " homes.");
                        Data.saveHomes();
                    }
                    else
                        Broadcast.broadcastTo(senderClient.netPlayer, "You have " + myHomes.Count + (Vars.homeLimit > 0 ? "/" + Vars.homeLimit : "") + " home(s). You can't set anymore homes.");
                }
                else if (Vars.homeLimit == 1 && myHomes.Count == 1)
                {
                    Home home = myHomes.GetByName("1");
                    if (home != null)
                    {
                        homes.Remove(home);
                        home.x = playerChar.eyesOrigin.x;
                        home.y = playerChar.eyesOrigin.y;
                        home.z = playerChar.eyesOrigin.z;
                        homes.Add(home);
                        Broadcast.broadcastTo(senderClient.netPlayer, "Home reset! " + playerChar.eyesOrigin);
                        Data.saveHomes();
                    }
                    else
                        Broadcast.broadcastTo(senderClient.netPlayer, "You must specify a home name.");
                }
                else if (Vars.homeLimit == 1 && myHomes.Count == 0)
                {
                    Home home = new Home("1", senderClient.userID, playerChar.eyesOrigin.x, playerChar.eyesOrigin.y, playerChar.eyesOrigin.z);
                    homes.Add(home);
                    myHomes = homes.GetByUID(senderClient.userID);
                    Broadcast.broadcastTo(senderClient.netPlayer, "Home set! " + playerChar.eyesOrigin);
                    Broadcast.broadcastTo(senderClient.netPlayer, "You now have " + myHomes.Count + (Vars.homeLimit > 0 ? "/" + Vars.homeLimit : "") + " homes.");
                    Data.saveHomes();
                }
                else
                    Broadcast.broadcastTo(senderClient.netPlayer, "You must specify a home name.");
            }
        }

        public static void teleportHome(PlayerClient senderClient, string[] args)
        {
            HomeList myHomes = homes.GetByUID(senderClient.userID);
            if (myHomes.Count > 0)
            {
                if (args.Count() > 1)
                {
                    if (myHomes.Contains(args[1]))
                    {
                        if (Vars.blockedHomes.ContainsKey(senderClient.userID))
                        {
                            double timeLeft = Math.Round((Vars.blockedHomes[senderClient.userID].timeLeft / 1000));
                            if (timeLeft > 0)
                            {
                                TimeSpan timeSpan = TimeSpan.FromMilliseconds(Vars.blockedHomes[senderClient.userID].timeLeft);

                                string timeString = timeSpan.Minutes + " minutes, and " + timeSpan.Seconds + " seconds";

                                Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport to " + (myHomes.Count > 1 ? "any of your homes" : "your home") + " for " + timeString);
                                return;
                            }
                        }
                        if (!Vars.enableInHouse && Checks.inHouse(senderClient))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport to " + (myHomes.Count > 1 ? "any of your homes" : "your home") + " while in a house.");
                        }
                        else if (Vars.isTeleporting.Contains(senderClient))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are already mid-teleport!");
                        }
                        else
                        {
                            if (Vars.homeDelay > 0)
                                Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting to your home [" + args[1] + "] in " + Vars.homeDelay + " seconds...");
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting to your home [" + args[1] + "]...");
                            myHomes.GetByName(args[1]).teleport(senderClient);
                        }
                    }
                    else
                        Broadcast.broadcastTo(senderClient.netPlayer, "You don't have a home named \"" + args[1] + "\".");
                }
                else if (Vars.homeLimit == 1 && myHomes.Count == 1)
                {
                    Home home = myHomes.GetByName("1");
                    if (home != null)
                    {
                        if (Vars.blockedHomes.ContainsKey(senderClient.userID))
                        {
                            double timeLeft = Math.Round((Vars.blockedHomes[senderClient.userID].timeLeft / 1000));
                            if (timeLeft > 0)
                            {
                                TimeSpan timeSpan = TimeSpan.FromMilliseconds(Vars.blockedHomes[senderClient.userID].timeLeft);

                                string timeString = timeSpan.Minutes + " minutes, and " + timeSpan.Seconds + " seconds";

                                Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport to " + (myHomes.Count > 1 ? "any of your homes" : "your home") + " for " + timeString);
                                return;
                            }
                        }
                        if (!Vars.enableInHouse && Checks.inHouse(senderClient))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You cannot teleport to " + (myHomes.Count > 1 ? "any of your homes" : "your home") + " while in a house.");
                        }
                        else if (Vars.isTeleporting.Contains(senderClient))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are already mid-teleport!");
                        }
                        else
                        {
                            if (Vars.homeDelay > 0)
                                Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting to your home in " + Vars.homeDelay + " seconds...");
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "Teleporting to your home...");
                            myHomes.GetByName("1").teleport(senderClient);
                        }
                    }
                    else
                        Broadcast.broadcastTo(senderClient.netPlayer, "You must specify a home name.");
                }
                else
                    Broadcast.broadcastTo(senderClient.netPlayer, "You must specify a home name.");
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You don't have any homes to telepot to. Type /sethome to set one.");
        }

        public static void listHomes(PlayerClient senderClient, string[] args)
        {
            HomeList myHomes = homes.GetByUID(senderClient.userID);
            if (myHomes.Count > 1)
            {
                List<string> homeNames = new List<string>();
                List<string> toDisplay = new List<string>();
                List<string> toRemove = new List<string>();
                foreach (Home home in myHomes)
                {
                    homeNames.Add(home.name);
                }

                Broadcast.broadcastTo(senderClient.netPlayer, "Your homes:");
                while (homeNames.Count > 0)
                {
                    toDisplay.Clear();
                    toRemove.Clear();
                    foreach (string s in homeNames)
                    {
                        toDisplay.Add(s);
                        toRemove.Add(s);

                        if ((string.Join(", ", toDisplay.ToArray())).Length > 80)
                        {
                            toDisplay.Remove(s);
                            toRemove.Remove(s);
                            break;
                        }
                    }
                    foreach (string s in toRemove)
                    {
                        homeNames.Remove(s);
                    }
                    Broadcast.broadcastTo(senderClient.netPlayer, string.Join(", ", toDisplay.ToArray()));
                }

            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You don't have any homes. Type /sethome to set one.");
        }
    }

    public class HomeList : List<Home>
    {
        public HomeList GetByUID(ulong userID)
        {
            HomeList homes = new HomeList();
            foreach (var obj in this)
            {
                if (obj.ownerID == userID)
                    homes.Add(obj);
            }

            return homes;
        }

        public Home GetByName(string homeName)
        {
            foreach (var obj in this)
            {
                if (obj.name == homeName)
                    return obj;
            }
            return null;
        }

        public bool Contains(string homeName)
        {
            foreach (var obj in this)
            {
                if (obj.name == homeName)
                    return true;
            }
            return false;
        }
    }

    public class Home
    {
        public string name;
        public ulong ownerID;
        public float x;
        public float y;
        public float z;

        public Home(string name, ulong ownerID, float x, float y, float z)
        {
            this.name = name;
            this.ownerID = ownerID;
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void teleport(PlayerClient senderClient)
        {
            Vector3 origin = new Vector3(x, y, z);
            TimerPlus tp = TimerPlus.Create(Vars.homeCooldown, false, Vars.unblockHomeTP, senderClient.userID);
            Vars.blockedHomes.Add(senderClient.userID, tp);
            Vars.REB.StartCoroutine(Vars.homeTeleporting(senderClient, origin));
            Vars.isTeleporting.Add(senderClient);
        }
    }
}
