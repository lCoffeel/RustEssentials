using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RustEssentials.Util
{
    public static class Tools
    {
        public static void turnOffHitTools(PlayerClient senderClient, int type = 0)
        {
            string UID = senderClient.userID.ToString();
            if (Vars.destroyerList.Contains(UID))
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "Remover tool deactivated.");
                Vars.destroyerList.Remove(UID);
            }
            if (Vars.destroyerAllList.Contains(UID))
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "Advanced remover tool deactivated.");
                Vars.destroyerAllList.Remove(UID);
            }
            if (Vars.removerList.Contains(UID))
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "Minor remover tool deactivated.");
                Vars.removerList.Remove(UID);
            }
            if (Vars.portalList.Contains(UID) && ((type == 1 && Vars.wandName == Vars.portalName) || type == 0))
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "You have disabled the portal tool.");
                Vars.portalList.Remove(UID);
            }
            if (Vars.wandList.ContainsKey(UID) && ((type == 2 && Vars.wandName == Vars.portalName) || type == 0))
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "You have disabled the magic wand.");
                Vars.wandList.Remove(UID);
            }
            if (Vars.ownershipList.Contains(UID))
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "Ownership tool deactivated.");
                Vars.ownershipList.Remove(UID);
            }
            if (Vars.elevatorList.Contains(UID))
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "You have disabled the elevator tool.");
                Vars.elevatorList.Remove(UID);
            }
            if (Vars.oposList.Contains(UID))
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "You have disabled the object position tool.");
                Vars.oposList.Remove(UID);
            }
            if (Vars.explosiveBulletList.Contains(UID))
            {
                Broadcast.broadcastTo(senderClient.netPlayer, "You no longer have explosive bullets.");
                Vars.explosiveBulletList.Remove(UID);
            }
        }

        public static void showOwner(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();

                switch (mode)
                {
                    case "on":
                        if (!Vars.ownershipList.Contains(UID))
                        {
                            turnOffHitTools(senderClient);

                            Broadcast.broadcastTo(senderClient.netPlayer, "Ownership tool activated. Hit structures to show who owns them.");
                            Vars.ownershipList.Add(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You already have the ownership tool activated.");
                        break;
                    case "off":
                        if (Vars.ownershipList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "Ownership tool deactivated.");
                            Vars.ownershipList.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You do not have the ownership tool activated.");
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
                        break;
                }
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
        }
        public static void craftTool(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();
                Inventory senderInv = senderClient.controllable.GetComponent<Inventory>();

                switch (mode)
                {
                    case "on":
                        if (!Vars.craftList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are now in super craft mode. Crafting, research, and blueprint restrictions have been bypassed.");
                            Vars.craftList.Add(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are already in super craft mode.");
                        break;
                    case "off":
                        if (Vars.craftList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are now in normal craft mode. Crafting, research, and blueprint restrictions are in place.");
                            Vars.craftList.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are already in normal craft mode.");
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
                        break;
                }
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
        }

        public static void wandTool(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();

                switch (mode)
                {
                    case "on":
                        if (!Vars.wandList.ContainsKey(UID))
                        {
                            turnOffHitTools(senderClient, 1);

                            Broadcast.broadcastTo(senderClient.netPlayer, "You have enabled the magic wand. Teleport by using " + (Vars.wandName != "any" ? "a(n) " + Vars.wandName : "anything") + ".");
                            Vars.wandList.Add(UID, Vars.wandDistance);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You already have the magic wand enabled.");
                        break;
                    case "off":
                        if (Vars.wandList.ContainsKey(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You have disabled the magic wand.");
                            Vars.wandList.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You have not enabled the magic wand yet.");
                        break;
                    default:
                        float newWandDist;
                        if (Single.TryParse(args[1], out newWandDist))
                        {
                            if (Vars.wandList.ContainsKey(UID))
                            {
                                Broadcast.broadcastTo(senderClient.netPlayer, "Your wand distance has been changed from " + Vars.wandList[UID] + " to " + newWandDist + ".");
                                Vars.wandList[UID] = newWandDist;
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "You have not enabled the magic wand yet.");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "In order to change the wand distance for yourself, you must specify a number and only a number.");
                        break;
                }
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on, off, or a number.");
        }

        public static void explosiveBulletTool(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();

                switch (mode)
                {
                    case "on":
                        if (!Vars.explosiveBulletList.Contains(UID))
                        {
                            turnOffHitTools(senderClient);

                            Broadcast.broadcastTo(senderClient.netPlayer, "You have loaded explosive bullets.");
                            Vars.explosiveBulletList.Add(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You already have explosive bullets loaded.");
                        break;
                    case "off":
                        if (Vars.explosiveBulletList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You no longer have explosive bullets.");
                            Vars.explosiveBulletList.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You have not loaded explosive bullets yet.");
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
                        break;
                }
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
        }

        public static void followGhostTool(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();

                switch (mode)
                {
                    case "on":
                        if (!Vars.followGhostList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "Your body will now follow your ghost. Make sure you have vanish on!");
                            Vars.followGhostList.Add(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "Your body is already following your ghost.");
                        break;
                    case "off":
                        if (Vars.followGhostList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "Your body is no longer following your ghost.");
                            Vars.followGhostList.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "Your body is not currently following your ghost.");
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
                        break;
                }
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
        }

        public static void ghostTool(PlayerClient senderClient, Character senderChar, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();

                switch (mode)
                {
                    case "on":
                        if (!Vars.ghostList.ContainsKey(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are now a ghost...");
                            Vars.ghostList.Add(UID, senderChar.eyesOrigin);
                            Vars.ghostPositions.Add(UID, senderChar.eyesOrigin);
                            Vars.REB.StartCoroutine(ghostPositionUpdate(senderClient, senderChar));
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are already a ghost...");
                        break;
                    case "off":
                        if (Vars.ghostList.ContainsKey(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are no longer a ghost...");
                            Vars.simulateTeleport(senderClient, Vars.ghostList[UID], true);
                            Vars.ghostList.Remove(UID);
                            Vars.ghostPositions.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not already a ghost...");
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
                        break;
                }
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
        }

        public static IEnumerator ghostPositionUpdate(PlayerClient senderClient, Character senderChar)
        {
            string uid = senderClient.userID.ToString();
            while (senderChar != null && senderChar.alive && Vars.ghostList.ContainsKey(uid))
            {
                if (Vars.vanishedList.Contains(uid) && !Vars.ghostTeleporting.Contains(senderClient) && Vars.ghostList.ContainsKey(uid) && Vars.followGhostList.Contains(uid))
                    Vars.ghostTeleporting.Add(senderClient);
                yield return new WaitForSeconds(2);
            }
        }

        public static void notifyTool(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();

                switch (mode)
                {
                    case "on":
                        if (!Vars.notifyList.ContainsKey(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You will now receive messages from the antihack.");
                            Vars.notifyList.Add(UID, senderClient.netPlayer);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are already receiving messages from the antihack.");
                        break;
                    case "off":
                        if (Vars.notifyList.ContainsKey(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You will no longer receive messages from the antihack.");
                            Vars.notifyList.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not receiving messages from the antihack.");
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
                        break;
                }
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
        }

        public static void bypassTool(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();

                switch (mode)
                {
                    case "on":
                        if (!Vars.bypassList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are now under the antihack's radar.");
                            Vars.bypassList.Add(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are already bypassing the antihack.");
                        break;
                    case "off":
                        if (Vars.bypassList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are no longer under the antihack's radar.");
                            Vars.bypassList.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not currently bypassing the antihack.");
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
                        break;
                }
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
        }

        public static void infAmmoTool(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();

                switch (mode)
                {
                    case "on":
                        if (!Vars.infAmmoList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You now have infinite ammo. You must load atleast one of the ammo type you wish to be infinite.");
                            Vars.infAmmoList.Add(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You already have infinite ammo.");
                        break;
                    case "off":
                        if (Vars.infAmmoList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You no longer have infinite ammo.");
                            Vars.infAmmoList.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You have do not have infinite ammo.");
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
                        break;
                }
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
        }

        public static void unlAmmoTool(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();

                switch (mode)
                {
                    case "on":
                        if (!Vars.unlAmmoList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You now have unlimited ammo. You must carry atleast one of the ammo type you wish to be unlimited.");
                            Vars.unlAmmoList.Add(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You already have unlimited ammo.");
                        break;
                    case "off":
                        if (Vars.unlAmmoList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You no longer have unlimited ammo.");
                            Vars.unlAmmoList.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You have do not have unlimited ammo.");
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
                        break;
                }
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
        }

        public static void portalTool(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();

                switch (mode)
                {
                    case "on":
                        if (!Vars.portalList.Contains(UID))
                        {
                            turnOffHitTools(senderClient, 2);
                            
                            Broadcast.broadcastTo(senderClient.netPlayer, "You have enabled the portal tool. Teleport by using " + (Vars.portalName != "any" ? "a(n) " + Vars.portalName : "anything") + ".");
                            Vars.portalList.Add(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You already have the portal tool enabled.");
                        break;
                    case "off":
                        if (Vars.portalList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You have disabled the portal tool.");
                            Vars.portalList.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You have not enabled the portal tool yet.");
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
                        break;
                }
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
        }

        public static void vanishTool(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();

                switch (mode)
                {
                    case "on":
                        if (!Vars.vanishedList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You have vanished. Your body is invisible. Reconnect to make your name invisible.");
                            Vars.vanishedList.Add(UID);

                            List<IInventoryItem> armorList = new List<IInventoryItem>();
                            Vars.previousArmor.Add(UID, new List<string>());
                            if (Items.grabArmor(senderClient, out armorList))
                            {
                                foreach (IInventoryItem item in armorList)
                                {
                                    Vars.previousArmor[UID].Add(item.datablock.name);
                                }
                            }
                            Items.addArmor(senderClient, "Invisible Helmet", 1, true);
                            Items.addArmor(senderClient, "Invisible Vest", 1, true);
                            Items.addArmor(senderClient, "Invisible Pants", 1, true);
                            Items.addArmor(senderClient, "Invisible Boots", 1, true);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You have already vanished.");
                        break;
                    case "off":
                        if (Vars.vanishedList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You have appeared. Your body is visible. Reconnect to make your name visible.");
                            Vars.vanishedList.Remove(UID);
                            Items.clearArmor(senderClient);
                            foreach (string s in Vars.previousArmor[UID])
                            {
                                Items.addArmor(senderClient, s, 1);
                            }
                            Vars.previousArmor.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You have not vanished.");
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
                        break;
                }
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
        }

        public static void hideTool(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();

                switch (mode)
                {
                    case "on":
                        if (!Vars.hiddenList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are now hidden from AI.");
                            Vars.hiddenList.Add(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are already hidden from AI.");
                        break;
                    case "off":
                        if (Vars.hiddenList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are no longer hidden from AI.");
                            Vars.hiddenList.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not currently hidden from AI.");
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
                        break;
                }
            }
        }

        public static void giveBettyAccess(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();
                switch (mode)
                {
                    case "on":
                        if (Vars.bettyPickupAccess.Contains(UID))
                            Broadcast.broadcastTo(senderClient.netPlayer, "You already have access to pick up any bouncing betty.");
                        else
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You now have access to pick up any bouncing betty.");
                            Vars.bettyPickupAccess.Add(UID);
                        }
                        break;
                    case "off":
                        if (Vars.bettyPickupAccess.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You no longer have access to pick up any bouncing betty.");
                            Vars.bettyPickupAccess.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You do not currently have access to pick up any bouncing betty.");
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
                        break;
                }
            }
        }

        public static void giveAccess(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();
                switch (mode)
                {
                    case "on":
                        if (Vars.completeDoorAccess.Contains(UID))
                            Broadcast.broadcastTo(senderClient.netPlayer, "You already have access to all doors.");
                        else
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You now have access to all doors.");
                            Vars.completeDoorAccess.Add(UID);
                        }
                        break;
                    case "off":
                        if (Vars.completeDoorAccess.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "You no longer have access to all doors.");
                            Vars.completeDoorAccess.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You do not currently have access to all doors.");
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
                        break;
                }
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
        }

        public static void minorRemoverTool(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();

                switch (mode)
                {
                    case "on":
                        if (!Vars.removerList.Contains(UID))
                        {
                            if (Vars.enableRemover)
                            {
                                turnOffHitTools(senderClient);

                                Broadcast.broadcastTo(senderClient.netPlayer, "Minor remover tool activated. Hit indestructible structures that you own to begin removing them.");
                                Vars.removerList.Add(UID);
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "The minor remover tool is not enabled on this server!");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You already have the minor remover tool activated.");
                        break;
                    case "off":
                        if (Vars.removerList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "Minor remover tool deactivated.");
                            Vars.removerList.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You do not have the minor remover tool activated.");
                        break;
                    case "share":
                        Share.shareRemoverWith(senderClient, args);
                        break;
                    case "unshare":
                        Share.unshareRemoverWith(senderClient, args);
                        break;
                    case "unshareall":
                        Share.unshareRemoverWithAll(senderClient);
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on, off, share, unshare, or unshareall.");
                        break;
                }
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on, off, share, unshare, or unshareall.");
        }

        public static void elevatorTool(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();

                switch (mode)
                {
                    case "on":
                        if (!Vars.elevatorList.Contains(UID))
                        {
                            turnOffHitTools(senderClient);

                            Broadcast.broadcastTo(senderClient.netPlayer, "Elevator tool activated. Hit ceilings to turn them into elevators.");
                            Vars.elevatorList.Add(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You already have the elevator tool activated.");
                        break;
                    case "off":
                        if (Vars.elevatorList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "Elevator tool deactivated.");
                            Vars.elevatorList.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You do not have the elevator tool activated.");
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
                        break;
                }
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
        }

        public static void removerAllTool(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();

                switch (mode)
                {
                    case "on":
                        if (!Vars.destroyerAllList.Contains(UID))
                        {
                            turnOffHitTools(senderClient);

                            Broadcast.broadcastTo(senderClient.netPlayer, "Advanced remover tool activated. Hit AI or structures to delete them and their connected structures.");
                            Vars.destroyerAllList.Add(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You already have the advanced remover tool activated.");
                        break;
                    case "off":
                        if (Vars.destroyerAllList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "Advanced remover tool deactivated.");
                            Vars.destroyerAllList.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You do not have the advanced remover tool activated.");
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
                        break;
                }
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
        }

        public static void oposTool(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();

                switch (mode)
                {
                    case "on":
                        if (!Vars.oposList.Contains(UID))
                        {
                            turnOffHitTools(senderClient);

                            Broadcast.broadcastTo(senderClient.netPlayer, "You have activated the object position tool! Hit objects to get their positon.");
                            Vars.oposList.Add(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You already have the object position tool activated.");
                        break;
                    case "off":
                        if (Vars.oposList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "Object position tool deactivated.");
                            Vars.oposList.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You do not have the object position tool activated.");
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
                        break;
                }
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
        }

        public static void removerTool(PlayerClient senderClient, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                string UID = senderClient.userID.ToString();

                switch (mode)
                {
                    case "on":
                        if (!Vars.destroyerList.Contains(UID))
                        {
                            turnOffHitTools(senderClient);

                            Broadcast.broadcastTo(senderClient.netPlayer, "Remover tool activated. Hit AI or structures to delete them.");
                            Vars.destroyerList.Add(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You already have the remover tool activated.");
                        break;
                    case "off":
                        if (Vars.destroyerList.Contains(UID))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "Remover tool deactivated.");
                            Vars.destroyerList.Remove(UID);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You do not have the remover tool activated.");
                        break;
                    default:
                        Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
                        break;
                }
            }
            else
                Broadcast.broadcastTo(senderClient.netPlayer, "You must specify on or off.");
        }
    }
}
