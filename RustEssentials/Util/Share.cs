using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facepunch.Utility;
using uLink;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace RustEssentials.Util
{
    public static class Share
    {
        public static void handleBuild(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string subcmd = args[1];
                switch (subcmd)
                {
                    case "share":
                        shareBuildWith(senderClient, args);
                        break;
                    case "unshare":
                        unshareBuildWith(senderClient, args);
                        break;
                    case "unshareall":
                        unshareBuildWithAll(senderClient);
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "You must specify share, unshare, or unshareall.");
                        break;
                }
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You must specify share, unshare, or unshareall.");
        }

        public static void shareBuildWith(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 2)
            {
                PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(args[2]));
                if (possibleTargets.Count() == 0)
                    Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain '" + args[2] + "'.");
                else if (possibleTargets.Count() > 1)
                    Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain '" + args[2] + "'.");
                else
                {
                    PlayerClient targetClient = possibleTargets[0];
                    ulong senderUID = senderClient.userID;
                    ulong targetUID = targetClient.userID;

                    if (senderUID.ToString().Length == 17 && targetUID.ToString().Length == 17 && senderUID != targetUID)
                    {
                        if (Vars.buildSharingData.ContainsKey(senderUID))
                        {
                            if (!Vars.buildSharingData[senderUID].Contains(targetUID))
                            {
                                Vars.buildSharingData[senderUID].Add(targetUID);
                                Broadcast.broadcastTo(senderClient.netPlayer, targetClient.userName + " can now place beds, sleeping bags, and gateways near your houses.");
                                Broadcast.broadcastTo(targetClient.netPlayer, "You can now place beds, sleeping bags, and gateways near " + senderClient.userName + "'s houses.");
                                Data.addBuildData(senderUID.ToString(), targetUID.ToString());
                            }
                            else
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, "You can already place beds, sleeping bags, and gateways near " + targetClient.userName + "'s houses.");
                            }
                        }
                        else
                        {
                            Vars.buildSharingData.Add(senderUID, new List<ulong>(){targetUID});
                            Broadcast.broadcastTo(senderClient.netPlayer, targetClient.userName + " can now place beds & sleeping bags near your houses.");
                            Broadcast.broadcastTo(targetClient.netPlayer, "You can now place beds, sleeping bags, and gateways near " + senderClient.userName + "'s houses.");
                            Data.addBuildData(senderUID.ToString(), targetUID.ToString());
                        }
                    }
                }
            }
        }

        public static void unshareBuildWithAll(PlayerClient senderClient)
        {
            ulong senderUID = senderClient.userID;

            if (Vars.buildSharingData.ContainsKey(senderUID))
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "Other players can no longer place beds, sleeping bags, and gateways near your houses.");
                Data.remBuildData(senderUID.ToString(), "all");
                List<ulong> shareData = Vars.buildSharingData[senderUID];
                PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => shareData.Contains(pc.userID));
                foreach (var target in possibleTargets)
                {
                    Broadcast.broadcastTo(target.netPlayer, "You can no longer place beds, sleeping bags, and gateways near " + senderClient.userName + "'s objects.");
                }
                Vars.buildSharingData.Remove(senderUID);
            }
            else
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "No other players can place beds, sleeping bags, and gateways near your houses.");
            }
        }

        public static void unshareBuildWith(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 2)
            {
                PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(args[2]));
                if (possibleTargets.Count() == 0)
                    Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain '" + args[2] + "'.");
                else if (possibleTargets.Count() > 1)
                    Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain '" + args[2] + "'.");
                else
                {
                    PlayerClient targetClient = possibleTargets[0];
                    ulong senderUID = senderClient.userID;
                    ulong targetUID = targetClient.userID;

                    if (senderUID.ToString().Length == 17 && targetUID.ToString().Length == 17 && senderUID != targetUID)
                    {
                        if (Vars.buildSharingData.ContainsKey(senderUID))
                        {
                            if (Vars.buildSharingData[senderUID].Contains(targetUID))
                            {
                                Vars.buildSharingData[senderUID].Remove(targetUID);
                                Broadcast.broadcastTo(senderClient.netPlayer, targetClient.userName + " can no longer place beds, sleeping bags, and gateways near your houses.");
                                Broadcast.broadcastTo(targetClient.netPlayer, "You can no longer place beds, sleeping bags, and gateways near " + senderClient.userName + "'s houses.");
                                Data.remBuildData(senderUID.ToString(), targetUID.ToString());
                            }
                            else
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, targetClient.userName + " can't place beds, sleeping bags, and gateways near your houses.");
                            }
                        }
                        else
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, targetClient.userName + " can't place beds, sleeping bags, and gateways near your houses.");
                        }
                    }
                }
            }
        }

        public static void shareWith(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(args[1]));
                if (possibleTargets.Count() == 0)
                    Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain '" + args[1] + "'.");
                else if (possibleTargets.Count() > 1)
                    Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain '" + args[1] + "'.");
                else
                {
                    PlayerClient targetClient = possibleTargets[0];
                    ulong senderUID = senderClient.userID;
                    ulong targetUID = targetClient.userID;

                    if (senderUID.ToString().Length == 17 && targetUID.ToString().Length == 17 && senderUID != targetUID)
                    {
                        if (Vars.sharingData.ContainsKey(senderUID))
                        {
                            if (!Vars.sharingData[senderUID].Contains(targetUID))
                            {
                                Vars.sharingData[senderUID].Add(targetUID);
                                Broadcast.noticeTo(senderClient.netPlayer, ":D", "Doors shared with " + targetClient.userName + ".", 5);
                                Broadcast.noticeTo(targetClient.netPlayer, ":D", "You can now open " + senderClient.userName + "'s doors.", 5);
                                Data.addDoorData(senderUID.ToString(), targetUID.ToString());
                            }
                            else
                            {
                                Broadcast.noticeTo(senderClient.netPlayer, ":D", "You have already shared doors with " + targetClient.userName + ".", 5);
                            }
                        }
                        else
                        {
                            Vars.sharingData.Add(senderUID, new List<ulong>(){targetUID});
                            Broadcast.noticeTo(senderClient.netPlayer, ":D", "Doors shared with " + targetClient.userName + ".", 5);
                            Broadcast.noticeTo(targetClient.netPlayer, ":D", "You can now open " + senderClient.userName + "'s doors.", 5);
                            Data.addDoorData(senderUID.ToString(), targetUID.ToString());
                        }
                    }
                }
            }
        }

        public static void unshareWithAll(PlayerClient senderClient)
        {
            ulong senderUID = senderClient.userID;

            if (Vars.sharingData.ContainsKey(senderUID))
            {
                Broadcast.noticeTo(senderClient.netPlayer, ":(", "Doors unshared with everyone.", 5);
                Data.remDoorData(senderUID.ToString(), "all");
                List<ulong> shareData = Vars.sharingData[senderUID];
                PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => shareData.Contains(pc.userID));
                foreach (var target in possibleTargets)
                {
                    Broadcast.noticeTo(target.netPlayer, ":D", "You can no longer open " + senderClient.userName + "'s doors.", 5);
                }
                Vars.sharingData.Remove(senderUID);
            }
            else
            {
                Broadcast.noticeTo(senderClient.netPlayer, ":(", "You are not sharing doors with anyone.", 5);
            }
        }

        public static void unshareWith(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(args[1]));
                if (possibleTargets.Count() == 0)
                    Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain '" + args[1] + "'.");
                else if (possibleTargets.Count() > 1)
                    Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain '" + args[1] + "'.");
                else
                {
                    PlayerClient targetClient = possibleTargets[0];
                    ulong senderUID = senderClient.userID;
                    ulong targetUID = targetClient.userID;

                    if (senderUID.ToString().Length == 17 && targetUID.ToString().Length == 17 && senderUID != targetUID)
                    {
                        if (Vars.sharingData.ContainsKey(senderUID))
                        {
                            if (Vars.sharingData[senderUID].Contains(targetUID))
                            {
                                Vars.sharingData[senderUID].Remove(targetUID);
                                Broadcast.noticeTo(senderClient.netPlayer, ":(", "Doors unshared with " + targetClient.userName + ".", 5);
                                Broadcast.noticeTo(targetClient.netPlayer, ":D", "You can no longer open " + senderClient.userName + "'s doors.", 5);
                                Data.remDoorData(senderUID.ToString(), targetUID.ToString());
                            }
                            else
                            {
                                Broadcast.noticeTo(senderClient.netPlayer, ":(", "You are not sharing doors with " + targetClient.userName + ".", 5);
                            }
                        }
                        else
                        {
                            Broadcast.noticeTo(senderClient.netPlayer, ":(", "You are not sharing doors with " + targetClient.userName + ".", 5);
                        }
                    }
                }
            }
        }

        public static void shareRemoverWith(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 2)
            {
                PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(args[2]));
                if (possibleTargets.Count() == 0)
                    Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain '" + args[2] + "'.");
                else if (possibleTargets.Count() > 1)
                    Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain '" + args[2] + "'.");
                else
                {
                    PlayerClient targetClient = possibleTargets[0];
                    ulong senderUID = senderClient.userID;
                    ulong targetUID = targetClient.userID;
                    if (senderUID.ToString().Length == 17 && targetUID.ToString().Length == 17 && senderUID != targetUID)
                    {
                        if (Vars.removerSharingData.ContainsKey(senderUID))
                        {
                            if (!Vars.removerSharingData[senderUID].Contains(targetUID))
                            {
                                Vars.removerSharingData[senderUID].Add(targetUID);
                                Broadcast.noticeTo(senderClient.netPlayer, ":D", "Remover shared with " + targetClient.userName + ".", 5);
                                Broadcast.noticeTo(targetClient.netPlayer, ":D", "You can now remove " + senderClient.userName + "'s objects.", 5);
                                Data.addRemoverData(senderUID.ToString(), targetUID.ToString());
                            }
                            else
                            {
                                Broadcast.noticeTo(senderClient.netPlayer, ":D", "You have already shared remover with " + targetClient.userName + ".", 5);
                            }
                        }
                        else
                        {
                            Vars.removerSharingData.Add(senderUID, new List<ulong>(){targetUID});
                            Broadcast.noticeTo(senderClient.netPlayer, ":D", "Remover shared with " + targetClient.userName + ".", 5);
                            Broadcast.noticeTo(targetClient.netPlayer, ":D", "You can now remove " + senderClient.userName + "'s objects.", 5);
                            Data.addRemoverData(senderUID.ToString(), targetUID.ToString());
                        }
                    }
                }
            }
        }

        public static void unshareRemoverWithAll(PlayerClient senderClient)
        {
            ulong senderUID = senderClient.userID;

            if (Vars.removerSharingData.ContainsKey(senderUID))
            {
                Broadcast.noticeTo(senderClient.netPlayer, ":(", "Remover unshared with everyone.", 5);
                Data.remRemoverData(senderUID.ToString(), "all");
                PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => Vars.removerSharingData[senderUID].Contains(pc.userID));
                foreach (var target in possibleTargets)
                {
                    Broadcast.noticeTo(target.netPlayer, ":D", "You can no longer remove " + senderClient.userName + "'s objects.", 5);
                }
                Vars.removerSharingData.Remove(senderUID);
            }
            else
            {
                Broadcast.noticeTo(senderClient.netPlayer, ":(", "You are not sharing remover with anyone.", 5);
            }
        }

        public static void unshareRemoverWith(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 2)
            {
                PlayerClient[] possibleTargets = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName.Contains(args[2]));
                if (possibleTargets.Count() == 0)
                    Broadcast.broadcastTo(senderClient.netPlayer, "No player names equal or contain '" + args[2] + "'.");
                else if (possibleTargets.Count() > 1)
                    Broadcast.broadcastTo(senderClient.netPlayer, "Too many player names contain '" + args[2] + "'.");
                else
                {
                    PlayerClient targetClient = possibleTargets[0];
                    ulong senderUID = senderClient.userID;
                    ulong targetUID = targetClient.userID;

                    if (senderUID.ToString().Length == 17 && targetUID.ToString().Length == 17 && senderUID != targetUID)
                    {
                        if (Vars.removerSharingData.ContainsKey(senderUID))
                        {
                            if (Vars.removerSharingData[senderUID].Contains(targetUID))
                            {
                                Vars.removerSharingData[senderUID].Remove(targetUID);
                                Broadcast.noticeTo(senderClient.netPlayer, ":(", "Remover unshared with " + targetClient.userName + ".", 5);
                                Broadcast.noticeTo(targetClient.netPlayer, ":D", "You can no longer remover " + senderClient.userName + "'s objects.", 5);
                                Data.remRemoverData(senderUID.ToString(), targetUID.ToString());
                            }
                            else
                            {
                                Broadcast.noticeTo(senderClient.netPlayer, ":(", "You are not sharing remover with " + targetClient.userName + ".", 5);
                            }
                        }
                        else
                        {
                            Broadcast.noticeTo(senderClient.netPlayer, ":(", "You are not sharing remover with " + targetClient.userName + ".", 5);
                        }
                    }
                }
            }
        }

    }
}
