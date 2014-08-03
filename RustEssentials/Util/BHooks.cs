using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Facepunch;
using UnityEngine;
using Facepunch.MeshBatch;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace RustEssentials.Util
{
    public static class BHooks
    {
        public static ContextServerResponse ExecuteClientResponse_Quick(Controllable control, Contextual contextual, ulong timestamp, bool immediate, GameObject doorObj)
        {
            if (control != null)
            {
                IContextRequestableQuick quick = null;
                if (contextual != null && !contextual.AsQuick(out quick))
                {
                    Debug.LogError("instance did not implement IContextRequestableQuick!:" + contextual.implementor, contextual);
                    return ContextServerResponse.InvalidCast;
                }
                else if (contextual == null)
                {
                    if (doorObj != null && doorObj.GetComponent<BasicDoor>() != null)
                    {
                        if (!Vars.enableDoorHolding)
                        {
                            menu_useDoor(control, timestamp, doorObj.GetComponent<BasicDoor>());
                        }
                        else
                            Broadcast.noticeTo(control.netPlayer, "", "Someone is holding the door!", 3, true);
                    }
                }
                if (contextual != null && quick != null)
                {
                    ContextResponse response = quick.ContextRespondQuick(control, timestamp);
                    if ((response != ContextResponse.DoneBreak) && (response != ContextResponse.DoneContinue))
                    {
                        return (!immediate ? ContextServerResponse.SelectionFail : ContextServerResponse.ImmediateFail);
                    }
                }
            }
            return (!immediate ? ContextServerResponse.SelectionSuccess : ContextServerResponse.ImmediateSuccess);
        }

        public static ContextServerResponse quickDoor(ref uLink.NetworkMessageInfo info, Context context)
        {
            Controllable controllable;
            Contextual contextual;
            ContextServerResponse? nullable = Context.DEQUEUE(info.sender, out controllable, out contextual);
            GameObject doorObj;
            try
            {
                if (Checks.isPointingAtDoor(controllable.character, out doorObj))
                {
                    if (controllable != null && controllable.netPlayer != null && doorObj != null)
                    {
                        ContextServerResponse response = ExecuteClientResponse_Quick(controllable, contextual, info.timestampInMillis, false, doorObj);
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("QUICKD: " + ex.ToString());
            }
            return (!nullable.HasValue ? ExecuteClientResponse_Quick(controllable, contextual, info.timestampInMillis, false, null) : nullable.Value);
        }

        public static ContextResponse menu_useDoor(Controllable controllable, ulong timestamp, BasicDoor BD)
        {
            ulong? lastToggleTimeStamp = BD.lastToggleTimeStamp;
            ulong? nullable3 = !lastToggleTimeStamp.HasValue ? null : new ulong?(timestamp - lastToggleTimeStamp.Value);
            if (((!nullable3.HasValue || (((float)((double)nullable3.Value)) > BD.minimumTimeBetweenOpenClose)) || (BD.lastToggleTimeStamp == 0L)) && toggleDoorState1(timestamp, controllable, BD))
            {
                BD.lastToggleTimeStamp = new ulong?(timestamp);
                return ContextResponse.DoneBreak;
            }
            return ContextResponse.FailBreak;
        }

        public static bool toggleDoorState1(ulong timestamp, Controllable controllable, BasicDoor BD)
        {
            //Vars.conLog.Info("4");
            if (controllable == null)
            {
                return toggleDoorState2(null, timestamp, BD, null);
            }
            Character character = controllable.GetComponent<Character>();
            DeployableObject deployable = BD.GetComponent<DeployableObject>();
            LockableObject lockable = BD.GetComponent<LockableObject>();
            Hook hook = Hook.Continue;
            if (BD.state != State.Opening && BD.state != State.Closing)
                hook = Vars.callHook("RustEssentialsAPI.Hooks", "OnUseDoor", false, character.netUser.userID.ToString(), BD, BD.state == State.Closed);

            if (Checks.ContinueHook(hook))
            {
                if ((deployable != null && belongsTo(controllable, deployable.ownerID)) || (((lockable == null) || !lockable.IsLockActive()) || lockable.HasAccess(controllable)) || hook == Hook.Success)
                {
                    if (deployable != null)
                    {
                        deployable.Touched();
                    }
                    if (character != null)
                    {
                        return toggleDoorState2(new Vector3?(character.eyesOrigin), timestamp, BD, null);
                    }
                    return toggleDoorState2(new Vector3?(controllable.transform.position), timestamp, BD, null);
                }
            }
            Rust.Notice.Popup(character.playerClient.netPlayer, "", "The door is locked!", 4f);
            return false;
        }

        public static bool toggleDoorState2(Vector3? openerPoint, ulong timestamp, BasicDoor BD, bool? fallbackReverse = new bool?())
        {
            //Vars.conLog.Info("5");
            if (!BD.serverLastTimeStamp.HasValue || (timestamp > BD.serverLastTimeStamp.Value))
            {
                if (BD.waitsTarget && ((BD.state == State.Opening) || (BD.state == State.Closing)))
                {
                    return false;
                }
                BD.serverLastTimeStamp = timestamp;
                State target = BD.target;
                bool openingInReverse = BD.openingInReverse;
                if (BD.target == State.Closed)
                {
                    if (openerPoint.HasValue || !fallbackReverse.HasValue)
                    {
                        if (BD.CalculateOpenWay(openerPoint) == Side.Forward)
                        {
                            BD.StartOpeningOrClosing(1, timestamp);
                        }
                        else
                        {
                            BD.StartOpeningOrClosing(2, timestamp);
                        }
                    }
                    else
                    {
                        BD.StartOpeningOrClosing(!(!fallbackReverse.HasValue ? BD.defaultReversed : fallbackReverse.Value) ? (sbyte)1 : (sbyte)2, timestamp);
                    }
                }
                else
                {
                    BD.StartOpeningOrClosing(0, timestamp);
                }
                if ((target != BD.target) || (openingInReverse != BD.openingInReverse))
                {
                    BD.InvalidateState(timestamp);
                    return true;
                }
            }
            return false;
        }

        public static bool belongsTo(Controllable controllable, ulong ownerID)
        {
            if (controllable == null)
                return false;

            PlayerClient playerClient = controllable.playerClient;

            if (playerClient == null)
                return false;

            ulong userID = playerClient.userID;

            if (Vars.completeDoorAccess.Contains(userID.ToString()))
                return true;

            if (!Vars.sharingData.ContainsKey(ownerID.ToString()))
            {
                if (userID == ownerID)
                    return true;
            }
            else
            {
                string shareData = Vars.sharingData[ownerID.ToString()];
                if (shareData.Contains(":"))
                {
                    if (shareData.Split(':').Contains(userID.ToString()) || ownerID == userID)
                        return true;
                }
                else
                {
                    if (shareData.Contains(userID.ToString()) || ownerID == userID)
                        return true;
                }
            }
            return false;
        }

        public static void deployableKilled(DeployableObject DO)
        {
            if (DO.handleDeathHere)
            {
                if (DO.clientDeathEffect != null)
                {
                    NetCull.RPC(NetEntityID.Get((UnityEngine.MonoBehaviour)DO), "Client_OnKilled", uLink.RPCMode.OthersBuffered);
                }
                NetCull.Destroy(DO.gameObject);
                if (DO.corpseObject != null)
                {
                    bool b = true;
                    if (!Vars.doorStops && (DO.corpseObject.name == "MetalDoor_Corpse" || DO.corpseObject.name == "WoodDoor_Corpse"))
                        b = false;

                    if (b)
                        NetCull.InstantiateStatic(DO.corpseObject, DO.transform.position, DO.transform.rotation);
                }
            }
        }

        public static bool tryConditionLoss(float probability, float percentLoss, InventoryItem item)
        {
            bool inCraftList = false;
            PlayerClient playerClient;
            if (Vars.getPlayerClient(item.inventory.networkView.owner, out playerClient))
            {
                if (Vars.craftList.Contains(playerClient.userID.ToString()))
                    inCraftList = true;
            }

            if (item.datablock.doesLoseCondition && Vars.enableDurability && !inCraftList)
            {
                float num = UnityEngine.Random.Range(0f, 1f);
                if (probability >= num)
                {
                    float condition = item.condition;
                    item.SetCondition(item.condition - (percentLoss * conditionloss.damagemultiplier));
                    return true;
                }
            }
            return false;
        }

        public static RustProto.User.Builder setNameOnJoin(string value, RustProto.User.Builder builder)
        {
            Google.ProtocolBuffers.ThrowHelper.ThrowIfNull(value, "value");
            builder.PrepareBuilder();
            builder.result.hasDisplayname = true;

            string userName = value;
            try
            {
                foreach (KeyValuePair<string, string> kv in Vars.rankPrefixes)
                {
                    userName = userName.Replace("[" + kv.Key + "]", "");
                    userName = userName.Replace(kv.Value, "");
                }

                foreach (KeyValuePair<string, string> kv in Vars.playerPrefixes)
                {
                    userName = userName.Replace("[" + kv.Value + "]", "");
                }

                userName = userName.Replace("<G> ", "");
                userName = userName.Replace("* <G> ", "");
                userName = userName.Replace("<D> ", "");
                userName = userName.Replace("* <D> ", "");
                userName = userName.Replace("<F> ", "");
                userName = userName.Replace("* <F> ", "");

                bool nameOccupied = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == userName).Count() > 0;
                int instanceNum = 0;
                if (nameOccupied && !Vars.kickDuplicate)
                {
                    instanceNum = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == userName).Count();

                    nameOccupied = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == userName + " (" + instanceNum + ")").Count() > 0;
                    while (nameOccupied)
                    {
                        instanceNum++;
                        nameOccupied = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == userName + " (" + instanceNum + ")").Count() > 0;
                    }
                    userName = userName + " (" + instanceNum + ")";
                }

                builder.result.displayname_ = userName;
                return builder;
            }
            catch (Exception ex) { Vars.conLog.Error("SNOJ: " + ex.ToString()); }

            builder.result.displayname_ = userName;
            return builder;
        }

        public static void onOpenStorage(LootableObject LO, Useable use)
        {
            if (LO.GetComponent<DeployableObject>() != null && use != null && use.user != null && use.user.netUser != null && use.user.playerClient != null)
            {
                Vars.conLog.StorageEmpty(use.user.netUser.displayName + " (" + use.user.playerClient.userID + ") opened " + LO.GetComponent<DeployableObject>().ownerID + "'s " + Vars.getFullObjectName(LO.gameObject.name) + " at " + LO.gameObject.transform.position + ".");
            }
            if (LO.GetComponent<SupplyCrate>() != null && use != null && use.user != null && use.user.netUser != null && use.user.playerClient != null)
            {
                Vars.conLog.StorageEmpty(use.user.netUser.displayName + " (" + use.user.playerClient.userID + ") opened a supply crate at " + LO.gameObject.transform.position + ".");
            }
        }

        public static void onCloseStorage(LootableObject LO, Useable use)
        {
            if (LO.GetComponent<DeployableObject>() != null && use != null && use.user != null && use.user.netUser != null && use.user.playerClient != null)
            {
                Vars.conLog.StorageEmpty(use.user.netUser.displayName + " (" + use.user.playerClient.userID + ") closed " + LO.GetComponent<DeployableObject>().ownerID + "'s " + Vars.getFullObjectName(LO.gameObject.name) + " at " + LO.gameObject.transform.position + ".");
            }
            if (LO.GetComponent<SupplyCrate>() != null && use != null && use.user != null && use.user.netUser != null && use.user.playerClient != null)
            {
                Vars.conLog.StorageEmpty(use.user.netUser.displayName + " (" + use.user.playerClient.userID + ") closed a supply crate at " + LO.gameObject.transform.position + ".");
            }
        }

        public static void mergeItem(byte fromSlot, byte toSlot, bool tryCombine, uLink.NetworkMessageInfo info, Inventory inventory)
        {
            mergeItemStart(fromSlot, toSlot, tryCombine, info, inventory);
            Inventory.SlotOperationResult message = inventory.SlotOperation(fromSlot, toSlot, Inventory.SlotOperationsMerge(tryCombine, info.sender));
            if ((int)message <= 0)
            {
                Debug.LogWarning(message);
            }
            mergeItemEnd(fromSlot, toSlot, tryCombine, info, inventory);
        }

        public static void mergeItemStart(byte fromSlot, byte toSlot, bool tryCombine, uLink.NetworkMessageInfo info, Inventory inventory)
        {
        }

        public static void mergeItemEnd(byte fromSlot, byte toSlot, bool tryCombine, uLink.NetworkMessageInfo info, Inventory inventory)
        {
        }

        public static void mergeItemInStorage(NetEntityID toInvID, byte fromSlot, byte toSlot, bool tryCombine, uLink.NetworkMessageInfo info, Inventory fromInv)
        {
            mergeItemInStorageStart(toInvID, fromSlot, toSlot, tryCombine, info, fromInv);
            Inventory component = toInvID.GetComponent<Inventory>();
            Inventory.SlotOperationResult message = fromInv.SlotOperation(fromSlot, component, toSlot, Inventory.SlotOperationsMerge(tryCombine, info.sender));
            if ((int)message <= 0)
            {
                Debug.LogWarning(message);
            }
            mergeItemInStorageEnd(toInvID, fromSlot, toSlot, tryCombine, info, fromInv);
        }

        public static void mergeItemInStorageStart(NetEntityID toInvID, byte fromSlot, byte toSlot, bool tryCombine, uLink.NetworkMessageInfo info, Inventory fromInv)
        {
            try
            {
                if (toInvID != null && info != null && fromInv != null)
                {
                    Inventory toInv = toInvID.GetComponent<Inventory>();
                    if (toInv != null)
                    {
                        if (fromInv.gameObject != null && fromInv.gameObject.name != null && toInv.gameObject != null && toInv.gameObject.name != null)
                        {
                            if (SpawnEntity.isLoot(fromInv.gameObject.name.Replace("(Clone)", "")) || SpawnEntity.isLoot(toInv.gameObject.name.Replace("(Clone)", "")) || SpawnEntity.isSack(fromInv.gameObject.name.Replace("(Clone)", "")) || SpawnEntity.isSack(toInv.gameObject.name.Replace("(Clone)", "")))
                                return;

                            PlayerClient client;
                            bool playerIsFrom = (fromInv != null ? (fromInv.networkView != null ? (fromInv.networkView.owner != null ? Vars.getPlayerClient(fromInv.networkView.owner) != null : false) : false) : false);
                            bool toInvNull = false;
                            if (toInv != null)
                            {
                                if (toInv.networkView != null)
                                {
                                    if (toInv.networkView.owner != null)
                                    {
                                        // Hello? Is this thing on?
                                    }
                                    else
                                        toInvNull = true;
                                }
                                else
                                    toInvNull = true;
                            }
                            else
                                toInvNull = true;


                            if (!playerIsFrom && toInvNull)
                            {
                                return;
                            }

                            uLink.NetworkPlayer netPlayer = (playerIsFrom ? fromInv.networkView.owner : toInv.networkView.owner);
                            if (netPlayer != null && Vars.getPlayerClient(netPlayer, out client))
                            {
                                if (client.netUser != null)
                                {
                                    string playerName = client.netUser.displayName;
                                    ulong UID = client.userID;
                                    if (fromInv != null)
                                    {
                                        IInventoryItem fromItem;
                                        IInventoryItem toItem;
                                        fromInv.GetItem(fromSlot, out fromItem);
                                        toInv.GetItem(toSlot, out toItem);

                                        if (fromItem != null && fromItem.datablock != null)
                                        {
                                            int amount = fromItem.uses;
                                            if (toItem != null && toItem.datablock != null)
                                            {
                                                InventoryItem item = toItem as InventoryItem;
                                                if (toItem.datablock == fromItem.datablock && toItem.uses + fromItem.uses > item.maxUses)
                                                    amount = Math.Abs(item.maxUses - toItem.uses);
                                            }

                                            LootableObject LO = fromInv.GetComponent<LootableObject>();
                                            if (LO != null && LO.gameObject != null && LO.gameObject.transform != null && LO.gameObject.transform.position != null && LO.GetComponent<DeployableObject>() != null)
                                            {
                                                DeployableObject deployable = LO.GetComponent<DeployableObject>();
                                                Vars.conLog.StorageRemoveItem(playerName + " (" + UID + ") withdrew " + amount + "x " + fromItem.datablock.name + " from " + deployable.ownerID + "'s " + Vars.getFullObjectName(LO.gameObject.name) + " at " + LO.gameObject.transform.position + ".");
                                            }
                                            else
                                            {
                                                LO = toInv.GetComponent<LootableObject>();
                                                if (LO != null && LO.gameObject != null && LO.gameObject.transform != null && LO.gameObject.transform.position != null && LO.GetComponent<DeployableObject>() != null)
                                                {
                                                    DeployableObject deployable = LO.GetComponent<DeployableObject>();
                                                    Vars.conLog.StorageAddItem(playerName + " (" + UID + ") deposited " + amount + "x " + fromItem.datablock.name + " into " + LO.GetComponent<DeployableObject>().ownerID + "'s " + Vars.getFullObjectName(LO.gameObject.name) + " at " + LO.gameObject.transform.position + ".");
                                                }
                                            }

                                            LO = fromInv.GetComponent<LootableObject>();
                                            if (LO != null && LO.gameObject != null && LO.gameObject.transform != null && LO.gameObject.transform.position != null && LO.GetComponent<SupplyCrate>() != null)
                                            {
                                                Vars.conLog.StorageRemoveItem(playerName + " (" + UID + ") withdrew " + amount + "x " + fromItem.datablock.name + " from a supply crate at " + LO.gameObject.transform.position + ".");
                                            }
                                            else
                                            {
                                                LO = toInv.GetComponent<LootableObject>();
                                                if (LO != null && LO.gameObject != null && LO.gameObject.transform != null && LO.gameObject.transform.position != null && LO.GetComponent<SupplyCrate>() != null)
                                                {
                                                    Vars.conLog.StorageAddItem(playerName + " (" + UID + ") deposited " + amount + "x " + fromItem.datablock.name + " into a supply crate at " + LO.gameObject.transform.position + ".");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("MERGING: " + ex.ToString());
            }
        }

        public static void mergeItemInStorageEnd(NetEntityID toInvID, byte fromSlot, byte toSlot, bool tryCombine, uLink.NetworkMessageInfo info, Inventory fromInv)
        {
        }

        public static void moveItem(byte fromSlot, byte toSlot, uLink.NetworkMessageInfo info, Inventory inventory)
        {
            IInventoryItem fromItem;
            inventory.GetItem(fromSlot, out fromItem);
            int uses = 0;

            if (fromItem != null && fromItem is IBulletWeaponItem)
            {
                uses = fromItem.uses;
            }

            moveItemStart(fromSlot, toSlot, info, inventory);
            Inventory.SlotOperationResult message = inventory.SlotOperation(fromSlot, toSlot, Inventory.SlotOperationsMove(info.sender));
            if ((int)message <= 0)
            {
                Debug.LogWarning(message);
            }
            moveItemEnd(fromSlot, toSlot, info, inventory);

            if (fromItem != null && fromItem is IBulletWeaponItem && fromItem.uses != uses)
                Items.setUses(fromItem, uses);
        }

        public static void moveItemStart(byte fromSlot, byte toSlot, uLink.NetworkMessageInfo info, Inventory inventory)
        {
        }

        public static void moveItemEnd(byte fromSlot, byte toSlot, uLink.NetworkMessageInfo info, Inventory inventory)
        {
        }

        public static void moveItemInStorage(NetEntityID toInvID, byte fromSlot, byte toSlot, uLink.NetworkMessageInfo info, Inventory fromInv)
        {
            IInventoryItem fromItem;
            fromInv.GetItem(fromSlot, out fromItem);
            int uses = 0;

            if (fromItem != null && fromItem is IBulletWeaponItem)
            {
                uses = fromItem.uses;
            }

            moveItemInStorageStart(toInvID, fromSlot, toSlot, info, fromInv);
            Inventory component = toInvID.GetComponent<Inventory>();
            Inventory.SlotOperationResult message = fromInv.SlotOperation(fromSlot, component, toSlot, Inventory.SlotOperationsMove(info.sender));
            if ((int)message <= 0)
            {
                Debug.LogWarning(message);
            }
            moveItemInStorageEnd(toInvID, fromSlot, toSlot, info, fromInv);

            if (fromItem != null && fromItem is IBulletWeaponItem && fromItem.uses != uses)
                Items.setUses(fromItem, uses);
        }

        public static void moveItemInStorageStart(NetEntityID toInvID, byte fromSlot, byte toSlot, uLink.NetworkMessageInfo info, Inventory fromInv)
        {
            try
            {
                if (toInvID != null && info != null && fromInv != null)
                {
                    Inventory toInv = toInvID.GetComponent<Inventory>();
                    if (toInv != null)
                    {
                        if (fromInv.gameObject != null && fromInv.gameObject.name != null && toInv.gameObject != null && toInv.gameObject.name != null)
                        {
                            if (SpawnEntity.isLoot(fromInv.gameObject.name.Replace("(Clone)", "")) || SpawnEntity.isLoot(toInv.gameObject.name.Replace("(Clone)", "")) || SpawnEntity.isSack(fromInv.gameObject.name.Replace("(Clone)", "")) || SpawnEntity.isSack(toInv.gameObject.name.Replace("(Clone)", "")))
                                return;

                            PlayerClient client;
                            bool playerIsFrom = (fromInv != null ? (fromInv.networkView != null ? (fromInv.networkView.owner != null ? Vars.getPlayerClient(fromInv.networkView.owner) != null : false) : false) : false);
                            bool toInvNull = false;
                            if (toInv != null)
                            {
                                if (toInv.networkView != null)
                                {
                                    if (toInv.networkView.owner != null)
                                    {
                                        // Hello? Is this thing on?
                                    }
                                    else
                                        toInvNull = true;
                                }
                                else
                                    toInvNull = true;
                            }
                            else
                                toInvNull = true;


                            if (!playerIsFrom && toInvNull)
                            {
                                return;
                            }

                            uLink.NetworkPlayer netPlayer = (playerIsFrom ? fromInv.networkView.owner : toInv.networkView.owner);
                            if (netPlayer != null && Vars.getPlayerClient(netPlayer, out client))
                            {
                                if (client.netUser != null)
                                {
                                    string playerName = client.netUser.displayName;
                                    ulong UID = client.userID;
                                    if (fromInv != null)
                                    {
                                        IInventoryItem fromItem;
                                        fromInv.GetItem(fromSlot, out fromItem);

                                        if (fromItem != null && fromItem.datablock != null)
                                        {
                                            LootableObject LO = fromInv.GetComponent<LootableObject>();
                                            if (LO != null && LO.gameObject != null && LO.gameObject.transform != null && LO.gameObject.transform.position != null && LO.GetComponent<DeployableObject>() != null)
                                            {
                                                DeployableObject deployable = LO.GetComponent<DeployableObject>();
                                                Vars.conLog.StorageRemoveItem(playerName + " (" + UID + ") withdrew " + fromItem.uses + "x " + fromItem.datablock.name + " from " + deployable.ownerID + "'s " + Vars.getFullObjectName(LO.gameObject.name) + " at " + LO.gameObject.transform.position + ".");
                                            }
                                            else
                                            {
                                                LO = toInv.GetComponent<LootableObject>();
                                                if (LO != null && LO.gameObject != null && LO.gameObject.transform != null && LO.gameObject.transform.position != null && LO.GetComponent<DeployableObject>() != null)
                                                {
                                                    DeployableObject deployable = LO.GetComponent<DeployableObject>();
                                                    Vars.conLog.StorageAddItem(playerName + " (" + UID + ") deposited " + fromItem.uses + "x " + fromItem.datablock.name + " into " + deployable.ownerID + "'s " + Vars.getFullObjectName(LO.gameObject.name) + " at " + LO.gameObject.transform.position + ".");
                                                }
                                            }

                                            LO = fromInv.GetComponent<LootableObject>();
                                            if (LO != null && LO.gameObject != null && LO.gameObject.transform != null && LO.gameObject.transform.position != null && LO.GetComponent<SupplyCrate>() != null)
                                            {
                                                Vars.conLog.StorageRemoveItem(playerName + " (" + UID + ") withdrew " + fromItem.uses + "x " + fromItem.datablock.name + " from a supply crate at " + LO.gameObject.transform.position + ".");
                                            }
                                            else
                                            {
                                                LO = toInv.GetComponent<LootableObject>();
                                                if (LO != null && LO.gameObject != null && LO.gameObject.transform != null && LO.gameObject.transform.position != null && LO.GetComponent<SupplyCrate>() != null)
                                                {
                                                    Vars.conLog.StorageAddItem(playerName + " (" + UID + ") deposited " + fromItem.uses + "x " + fromItem.datablock.name + " into a supply crate at " + LO.gameObject.transform.position + ".");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("MOVING: " + ex.ToString());
            }
        }

        public static void moveItemInStorageEnd(NetEntityID toInvID, byte fromSlot, byte toSlot, uLink.NetworkMessageInfo info, Inventory fromInv)
        {
        }

        public static void onHurt(TakeDamage takeDamage, ref DamageEvent damage)
        {
            if (damage.victim.idMain is Character)
            {
                if (Checks.isPlayer(damage.victim.idMain))
                {
                    onHumanHurt(ref damage);
                }
                else
                {
                    onAIHurt(ref damage);
                }
            }
            else
            {
                if (damage.victim.idMain is StructureComponent || damage.victim.idMain is DeployableObject)
                {
                    if (Checks.isPlayer(damage.attacker.idMain))
                    {
                        bool hasWand = false;
                        bool hasPortal = false;
                        PlayerClient playerClient = damage.attacker.client;
                        if (playerClient != null)
                        {
                            if (playerClient.controllable.GetComponent<Inventory>() != null)
                            {
                                if (playerClient.controllable.GetComponent<Inventory>().activeItem != null)
                                {
                                    if (playerClient.controllable.GetComponent<Inventory>().activeItem.datablock != null)
                                    {
                                        string heldItem = playerClient.controllable.GetComponent<Inventory>().activeItem.datablock.name;

                                        if (Vars.wandList.ContainsKey(playerClient.userID.ToString()))
                                        {
                                            hasWand = true;
                                            if (heldItem != Vars.wandName && Vars.wandName != "any")
                                                hasWand = false;
                                        }
                                        if (Vars.portalList.Contains(playerClient.userID.ToString()))
                                        {
                                            hasPortal = true;
                                            if (heldItem != Vars.portalName && Vars.portalName != "any")
                                                hasPortal = false;
                                        }
                                    }
                                }
                            }
                        }
                        if (hasWand || hasPortal)
                        {
                            damage.amount = 0f;
                            damage.status = LifeStatus.IsAlive;
                        }

                        if (Vars.destroyerList.Contains(damage.attacker.userID.ToString()))
                        {
                            Vars.beingDestroyed.Add(damage.victim.idMain.gameObject);
                            if (damage.victim.idMain is StructureComponent)
                            {
                                StructureComponent comp = (StructureComponent)damage.victim.idMain;
                                if (Elevators.isElevator(comp))
                                    Elevators.removeElevator(comp, damage.attacker.character, true);
                            }
                            if (damage.victim.idMain is DeployableObject)
                            {
                                DeployableObject comp = (DeployableObject)damage.victim.idMain;
                                if (comp.name == "WoodBox(Clone)" || comp.name == "WoodBoxLarge(Clone)" || comp.name == "SmallStash(Clone)" || comp.name == "Campfire(Clone)" || comp.name == "Furnace(Clone)")
                                    Vars.conLog.Storage(playerClient.userName + " (" + playerClient.userID + ") /remove'd " + comp.ownerID + "'s " + Vars.getFullObjectName(comp.gameObject.name) + " at " + comp.transform.position + ".");
                                string fancyName = Vars.getFullObjectName(comp.name);

                                if (fancyName == "Camp Fire" || fancyName == "Furnace")
                                {
                                    Lights.remove(comp);
                                }
                            }
                            NetCull.Destroy(damage.victim.idMain.gameObject);
                        }
                        else if (Vars.destroyerAllList.Contains(damage.attacker.userID.ToString()))
                        {
                            if (damage.victim.idMain is StructureComponent)
                            {
                                StructureMaster master = ((StructureComponent)damage.victim.idMain)._master;
                                HashSet<StructureComponent> structureComponents = master._structureComponents;
                                List<GameObject> toDestroy = new List<GameObject>();
                                foreach (var kv in structureComponents)
                                {
                                    if (kv.gameObject != null)
                                    {
                                        if (Elevators.isElevator(kv))
                                            Elevators.removeElevator(kv, damage.attacker.character, true);
                                        toDestroy.Add(kv.gameObject);
                                    }
                                }
                                foreach (GameObject GO in toDestroy)
                                {
                                    if (GO != null)
                                    {
                                        Vars.beingDestroyed.Add(GO);
                                        NetCull.Destroy(GO);
                                    }
                                }
                            }
                            else
                            {
                                if (damage.victim.idMain is DeployableObject)
                                {
                                    DeployableObject comp = (DeployableObject)damage.victim.idMain;
                                    if (comp.name == "WoodBox(Clone)" || comp.name == "WoodBoxLarge(Clone)" || comp.name == "SmallStash(Clone)" || comp.name == "Campfire(Clone)" || comp.name == "Furnace(Clone)")
                                        Vars.conLog.Storage(playerClient.userName + " (" + playerClient.userID + ") /removeall'd " + comp.ownerID + "'s " + Vars.getFullObjectName(comp.gameObject.name) + " at " + comp.transform.position + ".");
                                    string fancyName = Vars.getFullObjectName(comp.name);

                                    if (fancyName == "Camp Fire" || fancyName == "Furnace")
                                    {
                                        Lights.remove(comp);
                                    }
                                }
                                Vars.beingDestroyed.Add(damage.victim.idMain.gameObject);
                                NetCull.Destroy(damage.victim.idMain.gameObject);
                            }
                        }
                        else if (Vars.removerList.Contains(damage.attacker.userID.ToString()))
                        {
                            if (damage.victim.idMain is DeployableObject)
                            {
                                DeployableObject comp = (DeployableObject)damage.victim.idMain;

                                if (!Vars.onlyOnIndesctructibles)
                                {
                                    if (damage.extraData != null)
                                    {
                                        WeaponImpact extraData = damage.extraData as WeaponImpact;
                                        if (extraData != null)
                                        {
                                            bool isShared = false;
                                            if (Vars.removerSharingData.ContainsKey(comp.ownerID.ToString()))
                                            {
                                                isShared = Vars.removerSharingData[comp.ownerID.ToString()].Contains(damage.attacker.userID.ToString());
                                            }

                                            if (comp.ownerID == damage.attacker.userID || isShared)
                                            {
                                                float currentHealth = comp.GetComponent<TakeDamage>().health;
                                                float maxHealth = comp.GetComponent<TakeDamage>().maxHealth;
                                                string weaponName = extraData.dataBlock.name;
                                                if (Items.allMelees.Contains(weaponName))
                                                {
                                                    damage.amount = maxHealth;
                                                    if (Vars.returnItems)
                                                    {
                                                        if (damage.attacker.client != null)
                                                        {
                                                            if (comp.name == "WoodBox(Clone)" || comp.name == "WoodBoxLarge(Clone)" || comp.name == "SmallStash(Clone)" || comp.name == "Campfire(Clone)" || comp.name == "Furnace(Clone)")
                                                                Vars.conLog.Storage(playerClient.userName + " (" + playerClient.userID + ") /remover'd a " + Vars.getFullObjectName(comp.gameObject.name) + " at " + comp.transform.position + ".");
                                                            Items.addItem(damage.attacker.client, Vars.getFullObjectName(comp.name), 1);
                                                            if (comp.name.Contains("MetalBarsWindow"))
                                                            {
                                                                Vars.beingDestroyed.Add(damage.victim.idMain.gameObject);
                                                                NetCull.Destroy(damage.victim.idMain.gameObject);
                                                            }
                                                            string fancyName = Vars.getFullObjectName(comp.name);

                                                            if (fancyName == "Camp Fire" || fancyName == "Furnace")
                                                            {
                                                                Lights.remove(comp);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                                Broadcast.noticeTo(damage.attacker.networkView.owner, "✚", "You do not own this!");
                                        }
                                    }
                                }
                            }
                            if (damage.victim.idMain is StructureComponent)
                            {
                                StructureComponent structure = (StructureComponent)damage.victim.idMain;
                                bool canRemove = false;
                                bool isCeiling = structure.type == StructureComponent.StructureComponentType.Ceiling;
                                bool isPillar = structure.type == StructureComponent.StructureComponentType.Pillar;
                                bool isFoundation = structure.type == StructureComponent.StructureComponentType.Foundation;

                                if (Vars.onlyOnIndesctructibles)
                                {
                                    if (isCeiling && Vars.removerOnCeiling)
                                        canRemove = true;

                                    if (isPillar && Vars.removerOnPillar)
                                        canRemove = true;

                                    if (isFoundation && Vars.removerOnFoundation)
                                        canRemove = true;
                                }
                                else
                                    canRemove = true;

                                if (canRemove)
                                {
                                    if (damage.extraData != null)
                                    {
                                        WeaponImpact extraData = damage.extraData as WeaponImpact;
                                        if (extraData != null)
                                        {
                                            bool isShared = false;
                                            if (Vars.removerSharingData.ContainsKey(structure._master.ownerID.ToString()))
                                            {
                                                isShared = Vars.removerSharingData[structure._master.ownerID.ToString()].Contains(damage.attacker.userID.ToString());
                                            }

                                            if (structure._master.ownerID == damage.attacker.userID || isShared)
                                            {
                                                if ((isCeiling && !Vars.disregardCeilingWeight) || (isFoundation && !Vars.disregardFoundationWeight) || (isPillar && !Vars.disregardPillarWeight))
                                                {
                                                    if (structure._master.ComponentCarryingWeight(structure))
                                                    {
                                                        Broadcast.noticeTo(damage.attacker.networkView.owner, "✚", "You cannot remove a structure that is carrying weight!", 4);
                                                        return;
                                                    }
                                                }
                                                float currentHealth = structure.GetComponent<TakeDamage>().health;
                                                float maxHealth = structure.GetComponent<TakeDamage>().maxHealth;
                                                string weaponName = extraData.dataBlock.name;
                                                if (Items.allMelees.Contains(weaponName))
                                                {
                                                    switch (weaponName)
                                                    {
                                                        case "Hatchet":
                                                            damage.amount = UnityEngine.Random.Range(maxHealth * .15f, maxHealth * .3f);
                                                            break;
                                                        case "Pick Axe":
                                                            damage.amount = UnityEngine.Random.Range(maxHealth * .25f, maxHealth * .45f);
                                                            break;
                                                        case "Stone Hatchet":
                                                            damage.amount = UnityEngine.Random.Range(maxHealth * 0.05f, maxHealth * 0.15f);
                                                            break;
                                                        case "Rock":
                                                            damage.amount = UnityEngine.Random.Range(maxHealth * 0.02f, maxHealth * 0.1f);
                                                            break;
                                                    }
                                                    damage.amount = Mathf.Round(damage.amount);
                                                    float newHealth = (currentHealth - damage.amount);

                                                    if (currentHealth - damage.amount > 0)
                                                        Broadcast.sideNoticeTo(damage.attacker.networkView.owner, "[✚] -" + damage.amount + " (" + newHealth + "/" + structure.GetComponent<TakeDamage>().maxHealth + ")");
                                                    else
                                                    {
                                                        if (Vars.returnItems)
                                                        {
                                                            if (damage.attacker.client != null)
                                                            {
                                                                Items.addItem(damage.attacker.client, Vars.getFullStructureName(structure.name), 1);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                                Broadcast.noticeTo(damage.attacker.networkView.owner, "✚", "You do not own this!");
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Zone zone;
                            if (Checks.inZone(damage.victim.idMain.transform.position, out zone))
                            {
                                bool canHurt = true;
                                if (damage.victim.idMain is SleepingAvatar && zone.sleepersCanDie)
                                    canHurt = true;
                                else
                                    canHurt = false;

                                if (canHurt)
                                {
                                    damage.amount = 0f;
                                    damage.status = LifeStatus.IsAlive;
                                }
                            }

                            if (Vars.ownershipList.Contains(damage.attacker.userID.ToString()))
                            {
                                damage.amount = 0f;
                                damage.status = LifeStatus.IsAlive;
                                string ownerUID = "";
                                if (damage.victim.idMain is StructureComponent)
                                    ownerUID = (damage.victim.idMain as StructureComponent)._master.ownerID.ToString();

                                if (damage.victim.idMain is DeployableObject)
                                    ownerUID = (damage.victim.idMain as DeployableObject).ownerID.ToString();

                                Thread t = new Thread((DE) =>
                                {
                                    Broadcast.noticeTo(((DamageEvent)DE).attacker.client.netPlayer, "✲", "This is owned by " + Vars.grabNameByUID(ownerUID) + " (" + ownerUID + ")!", 7);
                                });
                                t.IsBackground = true;
                                t.Start(damage);
                            }

                            if (damage.victim.idMain is DeployableObject)
                            {
                                DeployableObject comp = (DeployableObject)damage.victim.idMain;
                                float health = comp.GetComponent<TakeDamage>().health;
                                if (health - damage.amount <= 0)
                                {
                                    if (comp.name == "WoodBox(Clone)" || comp.name == "WoodBoxLarge(Clone)" || comp.name == "SmallStash(Clone)" || comp.name == "Campfire(Clone)" || comp.name == "Furnace(Clone)")
                                        Vars.conLog.Storage(playerClient.userName + " (" + playerClient.userID + ") destroyed " + comp.ownerID + "'s " + Vars.getFullObjectName(comp.gameObject.name) + " at " + comp.transform.position + ".");

                                    string fancyName = Vars.getFullObjectName(comp.name);

                                    if (fancyName == "Camp Fire" || fancyName == "Furnace")
                                    {
                                        Lights.remove(comp);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        StructureComponent component = null;
                        DeployableObject obj = null;
                        if (damage.victim.idMain is StructureComponent)
                        {
                            component = (StructureComponent)damage.victim.idMain;

                            Zone zone = null;
                            if (Vars.structuresInZones.Contains(component) || Checks.inZone(damage.victim.idMain.transform.position, out zone))
                            {
                                if (!Vars.structuresInZones.Contains(component))
                                {
                                    Vars.structuresInZones.Add(component);
                                    if (!Vars.zoneStructures.ContainsKey(zone))
                                        Vars.zoneStructures.Add(zone, new List<StructureComponent>());

                                    if (!Vars.zoneStructures[zone].Contains(component))
                                        Vars.zoneStructures[zone].Add(component);
                                }

                                damage.amount = 0f;
                                damage.status = LifeStatus.IsAlive;
                            }
                        }

                        if (damage.victim.idMain is DeployableObject)
                        {
                            obj = (DeployableObject)damage.victim.idMain;
                            EnvDecay ED = obj.GetComponent<EnvDecay>();

                            float health = obj.GetComponent<TakeDamage>().health;
                            if (health - damage.amount <= 0 && (obj.name == "WoodBox(Clone)" || obj.name == "WoodBoxLarge(Clone)" || obj.name == "SmallStash(Clone)" || obj.name == "Campfire(Clone)" || obj.name == "Furnace(Clone)"))
                                Vars.conLog.Storage(obj.ownerID + "'s " + Vars.getFullObjectName(obj.gameObject.name) + " at " + obj.transform.position + " was destroyed by [a(n)] " + (damage.attacker.idMain.gameObject.name == obj.gameObject.name ? "decay" : Vars.getFullExplosiveName(damage.attacker.idMain.gameObject.name)) + ".");

                            string fancyName = Vars.getFullObjectName(obj.name);

                            if (fancyName == "Camp Fire" || fancyName == "Furnace")
                            {
                                Lights.remove(obj);
                            }

                            bool hasED = false;
                            if (ED != null)
                            {
                                if (Vars.envDecaysInZones.Contains(ED))
                                    hasED = true;
                            }
                            Zone zone = null;
                            if (hasED || Vars.objectsInZones.Contains(obj) || Checks.inZone(damage.victim.idMain.transform.position, out zone))
                            {
                                if (!Vars.objectsInZones.Contains(obj))
                                {
                                    Vars.objectsInZones.Add(obj);
                                    if (!Vars.zoneObjects.ContainsKey(zone))
                                        Vars.zoneObjects.Add(zone, new List<DeployableObject>());

                                    if (!Vars.zoneObjects[zone].Contains(obj))
                                        Vars.zoneObjects[zone].Add(obj);
                                }

                                damage.amount = 0f;
                                damage.status = LifeStatus.IsAlive;
                            }
                        }
                    }
                }
            }
        }

        public static void onAIHurt(ref DamageEvent damage)
        {
            if (damage.attacker.idMain != null)
            {
                if (Checks.isPlayer(damage.attacker.idMain))
                {
                    PlayerClient attackerClient = damage.attacker.client;

                    bool hasWand = false;
                    bool hasPortal = false;
                    if (attackerClient != null)
                    {
                        if (attackerClient.controllable.GetComponent<Inventory>() != null)
                        {
                            if (attackerClient.controllable.GetComponent<Inventory>().activeItem != null)
                            {
                                if (attackerClient.controllable.GetComponent<Inventory>().activeItem.datablock != null)
                                {
                                    string heldItem = attackerClient.controllable.GetComponent<Inventory>().activeItem.datablock.name;

                                    if (Vars.wandList.ContainsKey(attackerClient.userID.ToString()))
                                    {
                                        hasWand = true;
                                        if (heldItem != Vars.wandName && Vars.wandName != "any")
                                            hasWand = false;
                                    }
                                    if (Vars.portalList.Contains(attackerClient.userID.ToString()))
                                    {
                                        hasPortal = true;
                                        if (heldItem != Vars.portalName && Vars.portalName != "any")
                                            hasPortal = false;
                                    }
                                }
                            }
                        }
                    }
                    if (hasWand || hasPortal)
                    {
                        damage.amount = 0f;
                        damage.status = LifeStatus.IsAlive;
                    }

                    if (Vars.destroyerList.Contains(attackerClient.userID.ToString()))
                    {
                        Vars.beingDestroyed.Add(damage.victim.idMain.gameObject);
                        damage.amount = 1000f;
                        damage.status = LifeStatus.WasKilled;
                    }
                }
            }
        }

        public static bool pickupItem(Controllable controllable, ItemPickup itemObj)
        {
            IInventoryItem item;
            Inventory local = controllable.GetLocal<Inventory>();
            if (local == null)
            {
                return false;
            }

            BouncingBetty betty = itemObj.gameObject.GetComponent<BouncingBetty>();
            if (betty != null)
            {
                bool canPickup = false;

                if (betty.ownerID == controllable.netUser.userID || Vars.bettyPickupAccess.Contains(controllable.netUser.userID.ToString()))
                {
                    canPickup = Vars.ownerPickupBetty;
                }
                else
                {
                    KeyValuePair<string, Dictionary<string, string>>[] ownersFaction = Array.FindAll(Vars.factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(betty.ownerID.ToString()));
                    if (ownersFaction.Count() > 0)
                    {
                        if (ownersFaction[0].Value.ContainsKey(controllable.netUser.userID.ToString()))
                        {
                            canPickup = Vars.factionPickupBetty;
                        }
                        else
                        {
                            if (Vars.alliances.ContainsKey(ownersFaction[0].Key))
                            {
                                KeyValuePair<string, Dictionary<string, string>>[] targetsFaction = Array.FindAll(Vars.factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(controllable.netUser.userID.ToString()));
                                if (targetsFaction.Count() > 0 && Vars.alliances[ownersFaction[0].Key].Contains(targetsFaction[0].Key))
                                    canPickup = Vars.allyPickupBetty;
                                else
                                    canPickup = Vars.neutralPickupBetty;
                            }
                            else
                                canPickup = Vars.neutralPickupBetty;
                        }
                    }
                }

                if (canPickup)
                {
                    Data.remBettyData(betty.ownerID.ToString(), betty.bettyPos);
                    Explosions.bettyList.Remove(betty);
                    itemObj.RemoveThis();

                    Broadcast.sideNoticeTo(controllable.netPlayer, "1 x Bouncing Betty");

                    foreach (Items.Item iitem in Vars.bettyRecipe)
                    {
                        Items.addItem(controllable.playerClient, iitem.name, iitem.amount);
                    }
                    return true;
                }
                else
                    Broadcast.noticeTo(controllable.netPlayer, "➣", "You cannot pickup a bouncing betty!", 4);

                return false;
            }

            Inventory inventory2 = itemObj.GetLocal<Inventory>();
            if ((inventory2 == null) || (item = inventory2.firstItem) == null)
            {
                itemObj.RemoveThis();
                return false;
            }

            switch (local.AddExistingItem(item, false))
            {
                case Inventory.AddExistingItemResult.CompletlyStacked:
                    inventory2.RemoveItem(item);
                    break;

                case Inventory.AddExistingItemResult.Moved:
                    break;

                case Inventory.AddExistingItemResult.PartiallyStacked:
                    itemObj.UpdateItemInfo(item);
                    return true;

                case Inventory.AddExistingItemResult.Failed:
                    return false;

                case Inventory.AddExistingItemResult.BadItemArgument:
                    itemObj.RemoveThis();
                    return false;

                default:
                    throw new NotImplementedException();
            }
            itemObj.RemoveThis();
            return true;
        }

        public static void onSleeperKilled(DamageEvent damage, SleepingAvatar avatar)
        {
            if (damage.attacker.client != null)
            {
                PlayerClient attacker = damage.attacker.client;
                Vars.conLog.SleeperDeath("Player " + attacker.netUser.displayName + " (" + damage.attacker.character.eyesOrigin + ") [" + attacker.userID + "] killed " + avatar.ownerID + "'s sleeper at " + avatar.gameObject.transform.position + ".");
            }
        }

        public static void onAIKilled(BasicWildLifeAI BWAI, DamageEvent damage)
        {
            BWAI.ExitCurrentState();
            BWAI.EnterState_Dead();
            Facepunch.NetworkView networkView = BWAI.networkView;
            NetCull.RemoveRPCsByName(networkView, "GetNetworkUpdate");
            object[] args = new object[] { BWAI._myTransform.position, damage.attacker.networkViewID };
            networkView.RPC("ClientDeath", uLink.RPCMode.Others, args);
            if (Vars.beingDestroyed.Contains(damage.victim.idMain.gameObject))
                NetCull.Destroy(damage.victim.idMain.gameObject);
            else
                BWAI.Invoke("DelayedDestroy", 90f);
            WildlifeManager.RemoveWildlifeInstance(BWAI);
        }

        public static void onHumanHurt(ref DamageEvent damage)
        {
            try
            {
                if (damage.victim.client != null && damage.victim.client.controllable != null)
                {
                    PlayerClient victim = damage.victim.client;
                    TakeDamage takeDamage = victim.controllable.GetComponent<TakeDamage>();
                    if (takeDamage == null)
                        return;

                    PlayerClient attacker = null;

                    if (victim != null)
                    {
                        if (Vars.godList.Contains(victim.userID.ToString()) || Vars.frozenPlayers.ContainsKey(victim.userID) || Vars.vanishedList.Contains(victim.userID.ToString()))
                        {
                            damage.amount = 0f;
                            damage.status = LifeStatus.IsAlive;
                            if (damage.attacker.client != null && damage.attacker.client.netPlayer != null)
                            {
                                if (Vars.frozenPlayers.ContainsKey(victim.userID) && !Vars.vanishedList.Contains(victim.userID.ToString()))
                                {
                                    double curTime = Vars.currentTime;
                                    if (!Vars.allyShotMessages.ContainsKey(victim))
                                    {
                                        Vars.allyShotMessages.Add(victim, curTime - 1500);
                                    }

                                    if (curTime - Vars.allyShotMessages[victim] > 1000)
                                    {
                                        Broadcast.noticeTo(damage.attacker.client.netPlayer, "☃", "You hit a frozen player!");
                                        Vars.allyShotMessages[victim] = Vars.currentTime;
                                    }
                                }
                            }
                        }

                        string victimFaction = "Neutral";
                        string attackerFaction = "";

                        KeyValuePair<string, Dictionary<string, string>>[] possibleFactions = Array.FindAll(Vars.factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(victim.userID.ToString()));
                        if (possibleFactions != null && possibleFactions.Count() > 0)
                        {
                            victimFaction = possibleFactions[0].Key;
                        }

                        if (damage.attacker.idMain != null && Checks.isPlayer(damage.attacker.idMain))
                        {
                            attacker = damage.attacker.client;

                            bool hasWand = false;
                            bool hasPortal = false;
                            if (attacker != null)
                            {
                                ItemDataBlock currentItem = Items.getHeldItem(attacker);
                                float distance = Vector3.Distance(damage.attacker.character.eyesOrigin, damage.victim.character.eyesOrigin);
                                if (Vars.enableAntiRange && attacker.netPlayer != null && victim.netPlayer != null  && distance > (currentItem != null && currentItem is BulletWeaponDataBlock ? (currentItem as BulletWeaponDataBlock).bulletRange + Vars.rangeFlexibility : (currentItem != null && currentItem is MeleeWeaponDataBlock ? (currentItem as MeleeWeaponDataBlock).range + Vars.rangeFlexibility : (currentItem != null && currentItem is BowWeaponDataBlock ? 250 + Vars.rangeFlexibility : 0))))
                                {
                                    if (Items.allGuns.Contains(currentItem.name) || Items.allMelees.Contains(currentItem.name))
                                    {
                                        damage.amount = 0f;
                                        damage.status = LifeStatus.IsAlive;
                                        Broadcast.broadcastTo(attacker.netPlayer, "[AH] You cannot hit " + victim.userName + " from " + Math.Round(distance, 2) + "m! Possibly a phantom body or bleeding? Reconnect if this continues.");
                                        Broadcast.broadcastTo(victim.netPlayer, "[AH] " + attacker.userName + " tried to hit you from " + Math.Round(distance, 2) + "m with a " + currentItem.name + "! Possibly a phantom body or bleeding? Reconnect if this continues.");
                                    }
                                }

                                if (Vars.frozenPlayers.ContainsKey(attacker.userID))
                                {
                                    damage.amount = 0f;
                                    damage.status = LifeStatus.IsAlive;
                                }

                                if (attacker.controllable.GetComponent<Inventory>() != null)
                                {
                                    if (attacker.controllable.GetComponent<Inventory>().activeItem != null)
                                    {
                                        if (attacker.controllable.GetComponent<Inventory>().activeItem.datablock != null)
                                        {
                                            string heldItem = attacker.controllable.GetComponent<Inventory>().activeItem.datablock.name;

                                            if (Vars.wandList.ContainsKey(attacker.userID.ToString()))
                                            {
                                                hasWand = true;
                                                if (heldItem != Vars.wandName && Vars.wandName != "any")
                                                    hasWand = false;
                                            }
                                            if (Vars.portalList.Contains(attacker.userID.ToString()))
                                            {
                                                hasPortal = true;
                                                if (heldItem != Vars.portalName && Vars.portalName != "any")
                                                    hasPortal = false;
                                            }
                                        }
                                    }
                                }
                                if (hasWand || hasPortal)
                                {
                                    damage.amount = 0f;
                                    damage.status = LifeStatus.IsAlive;
                                }

                                possibleFactions = Array.FindAll(Vars.factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(attacker.userID.ToString()));
                                if (possibleFactions != null && possibleFactions.Count() > 0)
                                    attackerFaction = possibleFactions[0].Key;

                                double curTime = Vars.currentTime;
                                if (!Vars.allyShotMessages.ContainsKey(attacker))
                                    Vars.allyShotMessages.Add(attacker, curTime - 1500);

                                if (Vars.inSafeZone.ContainsKey(attacker))
                                {
                                    if (curTime - Vars.allyShotMessages[attacker] > 1000)
                                    {
                                        Broadcast.noticeTo(attacker.netPlayer, "☃", "You cannot hurt players while in a safe zone!");
                                        Vars.allyShotMessages[attacker] = Vars.currentTime;
                                    }
                                    damage.amount = 0f;
                                }
                                else if (Vars.inSafeZone.ContainsKey(victim))
                                {
                                    if (curTime - Vars.allyShotMessages[attacker] > 1000)
                                    {
                                        Broadcast.noticeTo(attacker.netPlayer, "☃", "You cannot hurt players in a safe zone!");
                                        Vars.allyShotMessages[attacker] = Vars.currentTime;
                                    }
                                    damage.amount = 0f;
                                }

                                if (victimFaction == attackerFaction && damage.attacker.IsDifferentPlayer(victim))
                                {
                                    object[] args = new object[] { victim.userID.ToString(), damage };
                                    Hook hook = Vars.callHook("RustEssentialsAPI.Hooks", "OnPlayerHurt", false, args);
                                    if (hook == Hook.Continue)
                                    {
                                        curTime = Vars.currentTime;
                                        if (!Vars.allyShotMessages.ContainsKey(attacker))
                                            Vars.allyShotMessages.Add(attacker, curTime - 1500);

                                        if (!Vars.allyShotMessages.ContainsKey(victim))
                                            Vars.allyShotMessages.Add(victim, curTime - 1500);
                                        if (damage.damageTypes == DamageTypeFlags.damage_bullet)
                                        {
                                            if (curTime - Vars.allyShotMessages[attacker] > 1000)
                                            {
                                                if (!Vars.enableDropdownFactionHits)
                                                    Broadcast.sideNoticeTo(attacker.netPlayer, "You shot " + victim.userName);
                                                else
                                                    Broadcast.noticeTo(attacker.netPlayer, "!", "You shot " + victim.userName, 3);

                                                Vars.allyShotMessages[attacker] = Vars.currentTime;
                                            }
                                            if (curTime - Vars.allyShotMessages[victim] > 1000)
                                            {
                                                if (!Vars.enableDropdownFactionHits)
                                                    Broadcast.sideNoticeTo(victim.netPlayer, attacker.userName + " shot you");
                                                else
                                                    Broadcast.noticeTo(victim.netPlayer, "!", attacker.userName + " shot you", 3);
                                                Vars.allyShotMessages[victim] = Vars.currentTime;
                                            }
                                        }
                                        else
                                        {
                                            if (curTime - Vars.allyShotMessages[attacker] > 1000)
                                            {
                                                if (!Vars.enableDropdownFactionHits)
                                                    Broadcast.sideNoticeTo(attacker.netPlayer, "You hit " + victim.userName);
                                                else
                                                    Broadcast.noticeTo(attacker.netPlayer, "!", "You hit " + victim.userName, 3);
                                                Vars.allyShotMessages[attacker] = Vars.currentTime;
                                            }
                                            if (curTime - Vars.allyShotMessages[victim] > 1000)
                                            {
                                                if (!Vars.enableDropdownFactionHits)
                                                    Broadcast.sideNoticeTo(victim.netPlayer, attacker.userName + " hit you");
                                                else
                                                    Broadcast.noticeTo(victim.netPlayer, "!", attacker.userName + " hit you", 3);
                                                Vars.allyShotMessages[victim] = Vars.currentTime;
                                            }
                                        }
                                    }
                                    else if (hook == Hook.Success && args[1] != null && args[1] is DamageEvent)
                                        damage = (DamageEvent)args[1];
                                    else if (hook == Hook.Failure)
                                        damage.amount = 0;

                                    if (!Vars.inWarZone.ContainsKey(attacker) && !Vars.inWarZone.ContainsKey(victim))
                                    {
                                        if (hook == Hook.Continue)
                                            damage.amount *= Vars.friendlyDamage;
                                    }
                                    else if (Vars.inWarZone.ContainsKey(attacker) && Vars.inWarZone.ContainsKey(victim))
                                    {
                                        damage.amount *= Vars.warFriendlyDamage;
                                    }
                                }
                                else
                                {
                                    object[] args = new object[] { victim.userID.ToString(), damage };
                                    Hook hook = Vars.callHook("RustEssentialsAPI.Hooks", "OnPlayerHurt", false, args);
                                    if (damage.attacker.IsDifferentPlayer(victim) && Vars.alliances.ContainsKey(attackerFaction))
                                    {
                                        if (Vars.alliances[attackerFaction].Contains(victimFaction))
                                        {
                                            if (hook == Hook.Continue)
                                            {
                                                curTime = Vars.currentTime;
                                                if (!Vars.allyShotMessages.ContainsKey(attacker))
                                                    Vars.allyShotMessages.Add(attacker, curTime - 1500);

                                                if (!Vars.allyShotMessages.ContainsKey(victim))
                                                    Vars.allyShotMessages.Add(victim, curTime - 1500);
                                                if (damage.damageTypes == DamageTypeFlags.damage_bullet)
                                                {
                                                    if (curTime - Vars.allyShotMessages[attacker] > 1000)
                                                    {
                                                        if (!Vars.enableDropdownAllyHits)
                                                            Broadcast.sideNoticeTo(attacker.netPlayer, "You shot " + (!Vars.enableAllyName ? "an ally" : victim.userName));
                                                        else
                                                            Broadcast.noticeTo(attacker.netPlayer, "!", "You shot " + (!Vars.enableAllyName ? "an ally" : victim.userName), 3);
                                                        Vars.allyShotMessages[attacker] = Vars.currentTime;
                                                    }
                                                    if (curTime - Vars.allyShotMessages[victim] > 1000)
                                                    {
                                                        if (!Vars.enableDropdownAllyHits)
                                                            Broadcast.sideNoticeTo(victim.netPlayer, (!Vars.enableAllyName ? "An ally" : attacker.userName) + " shot you");
                                                        else
                                                            Broadcast.noticeTo(victim.netPlayer, "!", (!Vars.enableAllyName ? "An ally" : attacker.userName) + " shot you", 3);
                                                        Vars.allyShotMessages[victim] = Vars.currentTime;
                                                    }
                                                }
                                                else
                                                {
                                                    if (curTime - Vars.allyShotMessages[attacker] > 1000)
                                                    {
                                                        if (!Vars.enableDropdownAllyHits)
                                                            Broadcast.sideNoticeTo(attacker.netPlayer, "You hit " + (!Vars.enableAllyName ? "an ally" : victim.userName));
                                                        else
                                                            Broadcast.noticeTo(attacker.netPlayer, "!", "You hit " + (!Vars.enableAllyName ? "an ally" : victim.userName), 3);
                                                        Vars.allyShotMessages[attacker] = Vars.currentTime;
                                                    }
                                                    if (curTime - Vars.allyShotMessages[victim] > 1000)
                                                    {
                                                        if (!Vars.enableDropdownAllyHits)
                                                            Broadcast.sideNoticeTo(victim.netPlayer, (!Vars.enableAllyName ? "An ally" : attacker.userName) + " hit you");
                                                        else
                                                            Broadcast.noticeTo(victim.netPlayer, "!", (!Vars.enableAllyName ? "An ally" : attacker.userName) + " hit you", 3);
                                                        Vars.allyShotMessages[victim] = Vars.currentTime;
                                                    }
                                                }
                                            }
                                            else if (hook == Hook.Success && args[1] != null && args[1] is DamageEvent)
                                                damage = (DamageEvent)args[1];
                                            else if (hook == Hook.Failure)
                                                damage.amount = 0;

                                            if (!Vars.inWarZone.ContainsKey(attacker) && !Vars.inWarZone.ContainsKey(victim))
                                            {
                                                if (hook == Hook.Continue)
                                                    damage.amount *= Vars.allyDamage;
                                            }
                                            else if (Vars.inWarZone.ContainsKey(attacker) && Vars.inWarZone.ContainsKey(victim))
                                            {
                                                damage.amount *= Vars.warAllyDamage;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!Vars.inWarZone.ContainsKey(attacker) && !Vars.inWarZone.ContainsKey(victim))
                                        {
                                            if (hook == Hook.Continue)
                                                damage.amount *= Vars.neutralDamage;
                                            else if (hook == Hook.Success && args[1] != null && args[1] is DamageEvent)
                                                damage = (DamageEvent)args[1];
                                            else if (hook == Hook.Failure)
                                                damage.amount = 0;
                                        }
                                        else if (Vars.inWarZone.ContainsKey(attacker) && Vars.inWarZone.ContainsKey(victim))
                                        {
                                            damage.amount *= Vars.warDamage;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            object[] args = new object[] { victim.userID.ToString(), damage };
                            Hook hook = Vars.callHook("RustEssentialsAPI.Hooks", "OnPlayerHurt", false, args);
                            if (hook == Hook.Success && args[1] != null && args[1] is DamageEvent)
                                damage = (DamageEvent)args[1];
                            else if (hook == Hook.Failure)
                                damage.amount = 0;

                            if (Vars.inSafeZone.ContainsKey(victim) && !Vars.inWarZone.ContainsKey(victim))
                            {
                                damage.amount = 0f;
                            }
                        }

                        if (!Vars.godList.Contains(victim.userID.ToString()) && !Vars.frozenPlayers.ContainsKey(victim.userID) && !Vars.vanishedList.Contains(victim.userID.ToString()) && damage.amount > 0)
                        {
                            if (!Vars.wasHit.Contains(victim.userID.ToString()))
                            {
                                if (Vars.isTeleporting.Contains(victim))
                                {
                                    Vars.wasHit.Add(victim.userID.ToString());
                                }
                                else if (Vars.isAccepting.Contains(victim))
                                {
                                    Vars.wasHit.Add(victim.userID.ToString());
                                }
                            }
                        }
                    }

                    if (takeDamage.health > damage.amount)
                    {
                        damage.status = LifeStatus.IsAlive;
                        if (victim != null && Vars.diedToBetty.KilledByBetty(victim.userID))
                            Vars.diedToBetty.RemoveByID(victim.userID);
                    }
                    else
                    {
                        damage.status = LifeStatus.WasKilled;
                    }
                }
            }
            catch (Exception ex) { Vars.conLog.Error("OHH: " + ex.ToString()); }
        }

        public delegate void Callback(int status, string result, params object[] args);
        private static void steamAPICallback(int status, string result, params object[] args)
        {
            if (status == 200)
            {
                NetUser user = (NetUser)args[0];
                if (result.Contains("unauthorized"))
                {
                    Vars.conLog.Error("OUI: Unable to connect to the Steam Web API for " + user.playerClient.userName + " (" + user.userID + "). Error:");
                    Vars.conLog.Error("Invalid steamAPIKey.");
                }
                else
                {
                    SteamAPIFamilyShare jsonArray = JsonConvert.DeserializeObject<SteamAPIFamilyShare>(result);
                    if (jsonArray.response.lender_steamid != "0")
                    {
                        Vars.otherKick(user, "Family Shared accounts are not allowed on this server.");
                    }
                }
            }
        }
        
        private static void ReponseCallback(IAsyncResult result)
        {
            object[] info = (object[])result.AsyncState;
            Callback callback = (Callback)info[0];
            HttpWebRequest request = (HttpWebRequest)info[1];
            NetUser user = (NetUser)info[2];
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            HttpStatusCode code = (HttpStatusCode)response.StatusCode;
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);

            callback((int)code, reader.ReadToEnd(), user);

            reader.Close();
            stream.Close();
            response.Close();
        }

        public static void onUserInitialize(NetUser user)
        {
            try
            {
                if (user != null)
                {
                    string userName = user.displayName;

                    if (Vars.enableAntiFamilyShare && !Vars.excludeFromFamilyCheck.Contains(Vars.findRank(user.userID.ToString())))
                    {
                        try
                        {
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://api.steampowered.com/IPlayerService/IsPlayingSharedGame/v0001/?key=" + Vars.steamAPIKey + "&steamid=" + user.userID + "&appid_playing=252490");
                            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.153 Safari/537.36";
                            request.Credentials = CredentialCache.DefaultCredentials;
                            request.Proxy = null;
                            request.Timeout = 60000;
                            Callback callback = steamAPICallback;
                            object[] info = new object[] { callback, request, user };

                            request.BeginGetResponse(ReponseCallback, info);
                        }
                        catch (Exception ex)
                        {
                            Vars.conLog.Error("OUI: Unable to connect to the Steam Web API. Error:");
                            Vars.conLog.Error(ex.Message);
                        }
                    }

                    RustProto.Avatar objB = user.LoadAvatar();
                    if (objB != null)
                    {
                        RustServerManagement RSM = RustServerManagement.Get();
                        if (RSM != null)
                        {
                            RSM.UpdateConnectingUserAvatar(user, ref user.avatar);
                            if (!object.ReferenceEquals(user.avatar, objB))
                            {
                                user.SaveAvatar();
                            }
                            if (user.playerClient != null)
                            {
                                if (ServerManagement.Get().SpawnPlayer(user.playerClient, false, user.avatar) != null)
                                {
                                    user.did_join = true;
                                    Vars.conLog.Info((Vars.vanishedList.Contains(user.userID.ToString()) ? "Vanished player " : "Player ") + userName + " (" + user.userID + ") has joined the game world. Avatar loaded. [" + user.networkPlayer.ipAddress + "]");
                                    if (user.networkPlayer != null && Vars.onlyOnJoin)
                                        Broadcast.broadcastCommandTo(user.networkPlayer, "censor.nudity false");
                                    if (Vars.vanishedList.Contains(user.userID.ToString()))
                                    {
                                        Items.addArmor(user.playerClient, "Invisible Helmet", 1, true);
                                        Items.addArmor(user.playerClient, "Invisible Vest", 1, true);
                                        Items.addArmor(user.playerClient, "Invisible Pants", 1, true);
                                        Items.addArmor(user.playerClient, "Invisible Boots", 1, true);
                                    }

                                    if (Vars.onFirstPlayer && Vars.dropMode == 1)
                                    {
                                        Vars.conLog.Info("Spawning first airdrop(s)...");
                                        for (int i = 0; i < Vars.planeCount; i++)
                                        {
                                            Vars.airdropServer();
                                        }
                                    }
                                    Vars.lastAirdropTime = Vars.currentTime;

                                    if (!Vars.firstPlayerInit)
                                    {
                                        Vars.firstPlayerInit = true;

                                        if (Explosions.bettyList.Update(user.userID, user.displayName))
                                            Data.updateBettyData(user.playerClient);

                                        Data.readBettyData();
                                        Lights.loadList();
                                    }

                                    Vars.callHook("RustEssentialsAPI.Hooks", "OnUserInit", false, user.userID.ToString());
                                }
                            }
                            else
                                Vars.conLog.Info("User " + userName + " (" + user.userID.ToString() + ") joined but the instance of PlayerClient is null! DO NOT IGNORE THIS!");
                        }
                        else
                            Vars.conLog.Info("User " + userName + " (" + user.userID.ToString() + ") joined but the RSM is null!");
                    }
                    else
                        Vars.conLog.Info("User " + userName + " (" + user.userID.ToString() + ") joined but the avatar could not be loaded!");
                }
                else
                    Vars.conLog.Info("User was null when joining! DO NOT IGNORE THIS!");
            }
            catch (Exception ex) { Vars.conLog.Error("OUInit: " + ex.ToString()); }
        }

        public static void onUserEssentialsDisconnect(uLink.NetworkPlayer player, NetUser user, PlayerClient playerClient)
        {
            try
            {
                string playerIP = "0.0.0.0";
                ServerManagement SM = ServerManagement.Get();

                List<PlayerClient> possibleClient = new List<PlayerClient>();
                try
                {
                    if (playerClient == null || playerClient.netUser == null || playerClient.netPlayer == null || playerClient.userName == null)
                        possibleClient = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netUser == user || pc.userID == user.userID || pc.netPlayer == user.networkPlayer || pc.netPlayer == player).ToList();

                    if (possibleClient.Count() == 1)
                        playerClient = possibleClient[0];

                    if (possibleClient.Count == 0 || playerClient == null || playerClient.netUser == null || playerClient.netPlayer == null || playerClient.userName == null)
                    {
                        possibleClient.Clear();
                        LockedList<PlayerClient> playerClients = PlayerClient.All;
                        possibleClient = Array.FindAll(playerClients.ToArray(), (PlayerClient pc) => pc.netUser == user || pc.userID == user.userID || pc.netPlayer == user.networkPlayer || pc.netPlayer == player).ToList();
                    }

                    if (possibleClient.Count() == 1)
                        playerClient = possibleClient[0];

                    if (possibleClient.Count == 0 || playerClient == null || playerClient.netUser == null || playerClient.netPlayer == null || playerClient.userName == null)
                    {
                        possibleClient.Clear();
                        List<PlayerClient> playerClients = PlayerClient.All.ToList();
                        possibleClient = Array.FindAll(playerClients.ToArray(), (PlayerClient pc) => pc.netUser == user || pc.userID == user.userID || pc.netPlayer == user.networkPlayer || pc.netPlayer == player).ToList();
                    }

                    if (possibleClient.Count() == 1)
                        playerClient = possibleClient[0];

                    if (possibleClient.Count == 0 || playerClient == null || playerClient.netUser == null || playerClient.netPlayer == null || playerClient.userName == null)
                        playerClient = null;

                    if (playerClient == null)
                        Vars.conLog.Error("Could not find a proper playerclient after many tries!");
                }
                catch (Exception ex)
                {
                    Vars.conLog.Error("Could not find a proper playerclient: " + ex.ToString());
                }

                if (user != null && playerClient != null)
                {
                    if (Vars.latestPM.ContainsKey(playerClient))
                        Vars.latestPM.Remove(playerClient);

                    if (Vars.latestRequests.ContainsKey(playerClient))
                        Vars.latestRequests.Remove(playerClient);

                    if (Vars.killList.Contains(playerClient))
                        Vars.killList.Remove(playerClient);

                    if (Vars.isTeleporting.Contains(playerClient))
                        Vars.isTeleporting.Remove(playerClient);

                    if (Vars.isAccepting.Contains(playerClient))
                        Vars.isAccepting.Remove(playerClient);

                    if (Vars.inSafeZone.ContainsKey(playerClient))
                        Vars.inSafeZone.Remove(playerClient);

                    if (Vars.inWarZone.ContainsKey(playerClient))
                        Vars.inWarZone.Remove(playerClient);

                    if (Vars.firstPoints.ContainsKey(playerClient))
                        Vars.firstPoints.Remove(playerClient);

                    if (Vars.secondPoints.ContainsKey(playerClient))
                        Vars.secondPoints.Remove(playerClient);

                    if (Vars.blockedRequestsPer.ContainsKey(playerClient.userID.ToString()))
                    {
                        if (Vars.blockedRequestsPer[playerClient.userID.ToString()].Count < 1)
                            Vars.blockedRequestsPer.Remove(playerClient.userID.ToString());
                    }

                    if (Vars.teleportRequests.ContainsKey(playerClient))
                        Vars.teleportRequests.Remove(playerClient);

                    if (Vars.violationCount.ContainsKey(playerClient))
                        Vars.violationCount.Remove(playerClient);

                    if (Vars.AllCharacters.ContainsKey(playerClient))
                        Vars.AllCharacters.Remove(playerClient);

                    if (Vars.AllPlayerClients.Contains(playerClient))
                        Vars.AllPlayerClients.Remove(playerClient);

                    if (Vars.removeOnDisconnect)
                    {
                        UnityEngine.Object[] objects = Array.FindAll(UnityEngine.Object.FindObjectsOfType(typeof(DeployableObject)), (UnityEngine.Object obj) => obj.name == "Barricade_Fence_Deployable(Clone)");
                        foreach (var obj in objects)
                        {
                            DeployableObject DO = obj as DeployableObject;
                            if (DO.ownerID == playerClient.userID)
                            {
                                NetCull.Destroy(DO.gameObject);
                            }
                        }
                    }

                    if (Vars.playerIPs.ContainsKey(playerClient))
                    {
                        playerIP = Vars.playerIPs[playerClient];
                        Vars.playerIPs.Remove(playerClient);
                    }

                    string leaveMessage = "";
                    try
                    {
                        if (Vars.enableLeave && !Vars.kickQueue.Contains(playerClient.userID.ToString()) && playerClient.userName.Length > 0)
                        {
                            leaveMessage = Vars.leaveMessage.Replace("$USER$", Vars.filterFullNames(playerClient.userName, playerClient.userID.ToString()));
                            Broadcast.broadcastJoinLeave(leaveMessage);
                            Vars.conLog.Chat("<BROADCAST ALL> " + Vars.botName + ": " + leaveMessage + " (" + user.userID + ") [" + playerIP + "]");
                        }
                    }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("OUDJL: " + ex.ToString());
                    }

                    bool b = true;
                    if (Vars.kickQueue.Contains(playerClient.userID.ToString()) || playerClient.userName.Length == 0)
                    {
                        if (Vars.kickQueue.Contains(playerClient.userID.ToString()))
                            Vars.kickQueue.Remove(playerClient.userID.ToString());
                        b = false;
                    }

                    if (b)
                        Vars.conLog.Info("Player " + user.displayName + " (" + user.userID + ") has disconnected. Data unloaded. [" + playerIP + "]");

                    try
                    {
                        if (Vars.oldPlayerArmor.ContainsKey(playerClient.userID) && Vars.oldPlayerInventory.ContainsKey(playerClient.userID))
                        {
                            Items.giveInventory(playerClient, Vars.oldPlayerInventory[playerClient.userID], Vars.oldPlayerArmor[playerClient.userID]);
                            Vars.oldPlayerArmor.Remove(playerClient.userID);
                            Vars.oldPlayerInventory.Remove(playerClient.userID);
                        }
                    }
                    catch (Exception exception)
                    {
                        Vars.conLog.Error("SA: " + exception);
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("OUED: " + ex.ToString());
            }
        }

        public static void onUserDisconnected(uLink.NetworkPlayer player, ConnectionAcceptor CA)
        {
            try
            {
                object localData = player.GetLocalData();
                if (localData is NetUser)
                {
                    RustServerManagement RSM = RustServerManagement.Get();
                    ServerManagement SM = ServerManagement.Get();
                    NetUser user = (NetUser)localData;

                    Vars.callAPI("RustEssentialsAPI.APIPlayer", "RemovePlayer", false, user.userID.ToString());

                    if (Vars.notifyList.ContainsKey(user.userID.ToString()))
                        Vars.notifyList.Remove(user.userID.ToString());

                    PlayerClient playerClient = user.playerClient;
                    onUserEssentialsDisconnect(player, user, playerClient);
                    user.connection.netUser = null;
                    CA.m_Connections.Remove(user.connection);
                    try
                    {
                        if (playerClient != null)
                        {
                            //SM.EraseCharactersForClient(playerClient, true, user);
                            Controllable controllable = playerClient.controllable;
                            if (controllable != null)
                            {
                                Character forCharacter = controllable.character;
                                string uid = playerClient.userID.ToString();
                                if (Vars.ghostList.ContainsKey(uid))
                                {
                                    Vars.simulateTeleport(playerClient, Vars.ghostList[uid], true);
                                    Vars.ghostList.Remove(uid);
                                    Vars.ghostPositions.Remove(uid);
                                }
                                try
                                {
                                    SM.SaveAvatar(forCharacter);
                                }
                                catch (Exception exception)
                                {
                                    Vars.conLog.Error("Something went wrong when " + playerClient.userName + " disconnected (3):");
                                    Vars.conLog.Error(exception.Message);
                                }
                                if (forCharacter != null)
                                {
                                    SM.ShutdownAvatar(forCharacter);
                                    Character.DestroyCharacter(forCharacter);
                                }
                            }
                            SM.RemovePlayerClientFromList(playerClient);
                        }
                        if (Vars.disconnectEvenIfNull)
                        {
                            NetCull.DestroyPlayerObjects(player);
                            CullGrid.ClearPlayerCulling(user);
                            NetCull.RemoveRPCs(player);
                        }
                        else if (player != null && user != null)
                        {
                            NetCull.DestroyPlayerObjects(player);
                            CullGrid.ClearPlayerCulling(user);
                            NetCull.RemoveRPCs(player);
                        }
                    }
                    catch (Exception exception)
                    {
                        Vars.conLog.Error("Something went wrong when " + playerClient.userName + " disconnected (1):");
                        Vars.conLog.Error(exception.ToString());
                    }
                    ConsoleSystem.Print("User Disconnected: " + user.displayName, false);
                    Rust.Steam.Server.OnUserLeave(user.connection.UserID);
                    try
                    {
                        user.Dispose();
                    }
                    catch (Exception exception2)
                    {
                        Vars.conLog.Error("Something went wrong when " + playerClient.userName + " disconnected (2):");
                        Vars.conLog.Error(exception2.Message);
                    }
                }
                else if (localData is ClientConnection)
                {
                    ClientConnection item = (ClientConnection)localData;
                    CA.m_Connections.Remove(item);
                    ConsoleSystem.Print("User Disconnected: (unconnected " + player.ipAddress + ")", false);
                }
                player.SetLocalData(null);
                Rust.Steam.Server.OnPlayerCountChanged();
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("OUD: " + ex.ToString());
            }
        }

        public static void readClientMove(HumanController human, uLink.NetworkMessageInfo info, object[] args)
        {
            string uid = human.idMain.netUser.userID.ToString();
            if (!Vars.ghostList.ContainsKey(uid))
                human.networkView.RPC("ReadClientMove", info.sender, args);
        }

        public static void getClientMove(HumanController human, Vector3 origin, int encoded, ushort stateFlags, uLink.NetworkMessageInfo info)
        {
            string uid = human.idMain.netUser.userID.ToString();

            if (!Vars.ghostList.ContainsKey(uid) || (human != null && human.idMain != null && human.idMain.netUser != null && Vars.ghostTeleporting.Contains(human.idMain.netUser.playerClient)))
            {
                Angle2 ang = new Angle2
                {
                    encoded = encoded
                };
                uLink.RPCMode othersExceptOwner = uLink.RPCMode.OthersExceptOwner;
                TruthDetector.ActionTaken taken = human.idMain.netUser.truthDetector.NoteMoved(ref origin, ang, info.timestamp);
                if (taken != TruthDetector.ActionTaken.Kicked)
                {
                    if (taken == TruthDetector.ActionTaken.Moved && human != null && human.idMain != null && human.idMain.netUser != null && !Vars.ghostTeleporting.Contains(human.idMain.netUser.playerClient))
                    {
                        othersExceptOwner = uLink.RPCMode.Others;
                    }
                    if (human != null && human.idMain != null && human.idMain.netUser != null && Vars.ghostTeleporting.Contains(human.idMain.netUser.playerClient))
                        origin.y += 100;

                    human.idMain.origin = origin;
                    human.idMain.eyesAngles = ang;
                    human.idMain.stateFlags.flags = stateFlags;
                    if (human.networkView.viewID != uLink.NetworkViewID.unassigned)
                    {
                        object[] objArray2 = new object[] { origin, ang.encoded, stateFlags, (float)(NetCull.time - info.timestamp) };
                        human.networkView.RPC("ReadClientMove", othersExceptOwner, objArray2);
                    }
                    human.ServerFrame();
                }
                if (human != null && human.idMain != null && human.idMain.netUser != null && Vars.ghostTeleporting.Contains(human.idMain.netUser.playerClient))
                    Vars.ghostTeleporting.Remove(human.idMain.netUser.playerClient);
            }
            else if (Vars.ghostPositions.ContainsKey(uid))
            {
                Vars.ghostPositions[uid] = origin;
            }
        }

        public static object[] setPlayerName(NetUser user)
        {
            try
            {
                string userName = user.user.Displayname;
                if (Vars.vanishedList.Contains(user.userID.ToString()))
                    userName = "";

                return new object[] { user.user.Userid, userName };
            }
            catch (Exception ex) { Vars.conLog.Error("CPCFU: " + ex.ToString()); }
            return null;
        }

        public static void setDeathReason(PlayerClient playerClient, ref DamageEvent damage)
        {
            try
            {
                if (playerClient != null)
                {
                    if (playerClient.netPlayer != null)
                    {
                        if (NetCheck.PlayerValid(playerClient.netPlayer))
                        {
                            if (Vars.wasHit.Contains(playerClient.userID.ToString()))
                                Vars.wasHit.Remove(playerClient.userID.ToString());

                            IDMain idMain = damage.attacker.idMain;
                            string message = "";
                            if (idMain != null)
                            {
                                if (Vars.removeOnDeath)
                                {
                                    UnityEngine.Object[] objects = Array.FindAll(UnityEngine.Object.FindObjectsOfType(typeof(DeployableObject)), (UnityEngine.Object obj) => obj.name == "Barricade_Fence_Deployable(Clone)");
                                    foreach (var obj in objects)
                                    {
                                        DeployableObject DO = obj as DeployableObject;
                                        if (DO.ownerID == playerClient.userID)
                                        {
                                            NetCull.Destroy(DO.gameObject);
                                        }
                                    }
                                }

                                idMain = idMain.idMain;
                                if (idMain != null)
                                {
                                    if (idMain is Character)
                                    {
                                        Character character = idMain as Character;
                                        Controller playerControlledController = character.playerControlledController;
                                        if (playerControlledController != null && playerControlledController.playerClient != null)
                                        {
                                            if (playerControlledController.playerClient == playerClient)
                                            {
                                                if (Vars.killList.Contains(playerClient))
                                                {
                                                    Vars.killList.Remove(playerClient);
                                                    DeathScreen.SetReason(playerClient.netPlayer, "You fell victim to /kill");
                                                    if (!Vars.hideKills)
                                                    {
                                                        if (Vars.includePositionsInLog)
                                                            Broadcast.broadcastKill(playerClient.userName + " " + Vars.getPosition(playerClient) + " fell victim to /kill.", playerClient.userName + " fell victim to /kill.");
                                                        else
                                                            Broadcast.broadcastKill(playerClient.userName + " fell victim to /kill.");
                                                    }
                                                    else
                                                    {
                                                        if (Vars.includePositionsInLog)
                                                            Vars.conLog.Chat("<SILENT BROADCAST> " + Vars.botName + ": " + playerClient.netPlayer + " " + Vars.getPosition(playerClient) + " fell victim to /kill");
                                                        else
                                                            Vars.conLog.Chat("<SILENT BROADCAST> " + Vars.botName + ": " + playerClient.netPlayer + " fell victim to /kill");
                                                    }

                                                    if (Vars.killsToConsole)
                                                    {
                                                        if (Vars.includePositionsInLog)
                                                            Vars.conLog.Info(playerClient.userName + " " + Vars.getPosition(playerClient) + " fell victim to /kill.");
                                                        else
                                                            Vars.conLog.Info(playerClient.userName + " fell victim to /kill.");
                                                    }
                                                }
                                                else
                                                {
                                                    DeathScreen.SetReason(playerClient.netPlayer, "You killed yourself. You silly sod.");
                                                    if (Vars.suicideMessages)
                                                    {
                                                        message = Vars.suicideMessage.Replace("$VICTIM$", playerClient.userName);

                                                        if (!Vars.hideKills)
                                                        {
                                                            if (Vars.includePositionsInLog)
                                                                Broadcast.broadcastKill(Vars.suicideMessage.Replace("$VICTIM$", playerClient.userName + " " + Vars.getPosition(playerClient)), message);
                                                            else
                                                                Broadcast.broadcastKill(message);
                                                        }
                                                        else
                                                        {
                                                            if (Vars.includePositionsInLog)
                                                                Vars.conLog.Chat("<SILENT BROADCAST> " + Vars.botName + ": " + Vars.suicideMessage.Replace("$VICTIM$", playerClient.userName + " " + Vars.getPosition(playerClient)));
                                                            else
                                                                Vars.conLog.Chat("<SILENT BROADCAST> " + Vars.botName + ": " + message);
                                                        }

                                                        if (Vars.killsToConsole)
                                                        {
                                                            if (Vars.includePositionsInLog)
                                                                Vars.conLog.Info(Vars.suicideMessage.Replace("$VICTIM$", playerClient.userName + " " + Vars.getPosition(playerClient)));
                                                            else
                                                                Vars.conLog.Info(message);
                                                        }
                                                    }
                                                }
                                                return;
                                            }

                                            Character killerChar;
                                            Character victimChar;
                                            Character.FindByUser(playerControlledController.playerClient.userID, out killerChar);
                                            Character.FindByUser(playerClient.userID, out victimChar);
                                            double distance = -1;
                                            if (killerChar != null && victimChar != null)
                                            {
                                                Vector3 killerPos = killerChar.transform.position;
                                                Vector3 victimPos = victimChar.transform.position;
                                                distance = Math.Round(Vector3.Distance(killerPos, victimPos));
                                            }
                                            try
                                            {
                                                WeaponImpact extraData = damage.extraData as WeaponImpact;
                                                if (damage.extraData != null && extraData != null && extraData.dataBlock != null && extraData.dataBlock.name != null)
                                                {
                                                    if (Vars.murderMessages)
                                                    {
                                                        message = Vars.murderMessage.Replace("$VICTIM$", playerClient.userName).Replace("$KILLER$", playerControlledController.playerClient.userName).Replace("$WEAPON$", extraData.dataBlock.name).Replace("$PART$", Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart))).Replace("$DISTANCE$", Convert.ToString(distance) + "m");

                                                        if (!Vars.hideKills)
                                                        {
                                                            if (Vars.includePositionsInLog)
                                                                Broadcast.broadcastKill(Vars.murderMessage.Replace("$VICTIM$", playerClient.userName + " " + Vars.getPosition(playerClient)).Replace("$KILLER$", playerControlledController.playerClient.userName + " " + Vars.getPosition(playerControlledController.playerClient)).Replace("$WEAPON$", extraData.dataBlock.name).Replace("$PART$", Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart))).Replace("$DISTANCE$", Convert.ToString(distance) + "m"), message);
                                                            else
                                                                Broadcast.broadcastKill(message);
                                                        }
                                                        else
                                                        {
                                                            if (Vars.includePositionsInLog)
                                                                Vars.conLog.Chat("<SILENT BROADCAST> " + Vars.botName + ": " + Vars.murderMessage.Replace("$VICTIM$", playerClient.userName + " " + Vars.getPosition(playerClient)).Replace("$KILLER$", playerControlledController.playerClient.userName + " " + Vars.getPosition(playerControlledController.playerClient)).Replace("$WEAPON$", extraData.dataBlock.name).Replace("$PART$", Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart))).Replace("$DISTANCE$", Convert.ToString(distance) + "m"));
                                                            else
                                                                Vars.conLog.Chat("<SILENT BROADCAST> " + Vars.botName + ": " + message);
                                                        }

                                                        if (Vars.killsToConsole)
                                                        {
                                                            if (Vars.includePositionsInLog)
                                                                Vars.conLog.Info(Vars.murderMessage.Replace("$VICTIM$", playerClient.userName + " " + Vars.getPosition(playerClient)).Replace("$KILLER$", playerControlledController.playerClient.userName + " " + Vars.getPosition(playerControlledController.playerClient)).Replace("$WEAPON$", extraData.dataBlock.name).Replace("$PART$", Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart))).Replace("$DISTANCE$", Convert.ToString(distance) + "m"));
                                                            else
                                                                Vars.conLog.Info(message);
                                                        }
                                                    }
                                                    if (Vars.enableDropdownKills)
                                                    {
                                                        string dropdownMessage = Vars.dropdownKillMessage.Replace("$VICTIM$", playerClient.userName).Replace("$PART$", Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart))).Replace("$DISTANCE$", Convert.ToString(distance) + "m");

                                                        Broadcast.noticeTo(playerControlledController.playerClient.netPlayer, "♞", dropdownMessage, 5);
                                                    }
                                                    DeathScreen.SetReason(playerClient.netPlayer, playerControlledController.playerClient.userName + " killed you using a " + extraData.dataBlock.name + " with a hit to your " + Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart)));
                                                    if (!Vars.playerDeaths.ContainsKey(playerClient.userID.ToString()))
                                                        Vars.playerDeaths.Add(playerClient.userID.ToString(), 1);
                                                    else
                                                        Vars.playerDeaths[playerClient.userID.ToString()] += 1;

                                                    Data.updateDeathsData(playerClient.userID.ToString(), Vars.playerDeaths[playerClient.userID.ToString()]);

                                                    if (!Vars.playerKills.ContainsKey(playerControlledController.playerClient.userID.ToString()))
                                                        Vars.playerKills.Add(playerControlledController.playerClient.userID.ToString(), 1);
                                                    else
                                                        Vars.playerKills[playerControlledController.playerClient.userID.ToString()] += 1;

                                                    Data.updateKillsData(playerControlledController.playerClient.userID.ToString(), Vars.playerKills[playerControlledController.playerClient.userID.ToString()]);
                                                    return;
                                                }
                                                // If we were killed by someone, but not by a known item... then the weapon has to have been a bow (maybe?)
                                                if ((damage.damageTypes & DamageTypeFlags.damage_melee) != 0)
                                                {
                                                    if (Vars.murderMessages)
                                                    {
                                                        message = Vars.murderMessage.Replace("$VICTIM$", playerClient.userName).Replace("$KILLER$", playerControlledController.playerClient.userName).Replace("$WEAPON$", "Hunting Bow").Replace("$PART$", Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart))).Replace("$DISTANCE$", Convert.ToString(distance) + "m");

                                                        if (!Vars.hideKills)
                                                        {
                                                            Broadcast.broadcastKill(Vars.murderMessage.Replace("$VICTIM$", playerClient.userName + " " + Vars.getPosition(playerClient)).Replace("$KILLER$", playerControlledController.playerClient.userName + " " + Vars.getPosition(playerControlledController.playerClient)).Replace("$WEAPON$", "Hunting Bow").Replace("$PART$", Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart))).Replace("$DISTANCE$", Convert.ToString(distance) + "m"), message);
                                                        }
                                                        else
                                                        {
                                                            if (Vars.includePositionsInLog)
                                                                Vars.conLog.Chat("<SILENT BROADCAST> " + Vars.botName + ": " + Vars.murderMessage.Replace("$VICTIM$", playerClient.userName + " " + Vars.getPosition(playerClient)).Replace("$KILLER$", playerControlledController.playerClient.userName + " " + Vars.getPosition(playerControlledController.playerClient)).Replace("$WEAPON$", "Hunting Bow").Replace("$PART$", Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart))).Replace("$DISTANCE$", Convert.ToString(distance) + "m"));
                                                            else
                                                                Vars.conLog.Chat("<SILENT BROADCAST> " + Vars.botName + ": " + message);
                                                        }

                                                        if (Vars.killsToConsole)
                                                        {
                                                            if (Vars.includePositionsInLog)
                                                                Vars.conLog.Info(Vars.murderMessage.Replace("$VICTIM$", playerClient.userName + " " + Vars.getPosition(playerClient)).Replace("$KILLER$", playerControlledController.playerClient.userName + " " + Vars.getPosition(playerControlledController.playerClient)).Replace("$WEAPON$", "Hunting Bow").Replace("$PART$", Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart))).Replace("$DISTANCE$", Convert.ToString(distance) + "m"));
                                                            else
                                                                Vars.conLog.Info(message);
                                                        }
                                                    }
                                                    if (Vars.enableDropdownKills)
                                                    {
                                                        string dropdownMessage = Vars.dropdownKillMessage.Replace("$VICTIM$", playerClient.userName).Replace("$PART$", Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart))).Replace("$DISTANCE$", Convert.ToString(distance) + "m");

                                                        Broadcast.noticeTo(playerControlledController.playerClient.netPlayer, "♞", dropdownMessage, 5);
                                                    }
                                                    DeathScreen.SetReason(playerClient.netPlayer, playerControlledController.playerClient.userName + " killed you using a Bow with a hit to your " + Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart)));
                                                    if (!Vars.playerDeaths.ContainsKey(playerClient.userID.ToString()))
                                                        Vars.playerDeaths.Add(playerClient.userID.ToString(), 1);
                                                    else
                                                        Vars.playerDeaths[playerClient.userID.ToString()] += 1;

                                                    Data.updateDeathsData(playerClient.userID.ToString(), Vars.playerDeaths[playerClient.userID.ToString()]);

                                                    if (!Vars.playerKills.ContainsKey(playerControlledController.playerClient.userID.ToString()))
                                                        Vars.playerKills.Add(playerControlledController.playerClient.userID.ToString(), 1);
                                                    else
                                                        Vars.playerKills[playerControlledController.playerClient.userID.ToString()] += 1;

                                                    Data.updateKillsData(playerControlledController.playerClient.userID.ToString(), Vars.playerKills[playerControlledController.playerClient.userID.ToString()]);
                                                    return;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Vars.conLog.Error("SDR #2: " + ex.ToString());
                                            }
                                            try
                                            {
                                                if (Vars.murderMessages)
                                                {
                                                    message = Vars.murderMessageUnknown.Replace("$VICTIM$", playerClient.userName).Replace("$KILLER$", playerControlledController.playerClient.userName).Replace("$PART$", Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart))).Replace("$DISTANCE$", Convert.ToString(distance) + "m");

                                                    if (!Vars.hideKills)
                                                    {
                                                        if (Vars.includePositionsInLog)
                                                            Broadcast.broadcastKill(Vars.murderMessageUnknown.Replace("$VICTIM$", playerClient.userName + " " + Vars.getPosition(playerClient)).Replace("$KILLER$", playerControlledController.playerClient.userName + " " + Vars.getPosition(playerControlledController.playerClient)).Replace("$PART$", Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart))).Replace("$DISTANCE$", Convert.ToString(distance) + "m"), message);
                                                        else
                                                            Broadcast.broadcastKill(message);
                                                    }
                                                    else
                                                    {
                                                        if (Vars.includePositionsInLog)
                                                            Vars.conLog.Chat("<SILENT BROADCAST> " + Vars.botName + ": " + Vars.murderMessageUnknown.Replace("$VICTIM$", playerClient.userName + " " + Vars.getPosition(playerClient)).Replace("$KILLER$", playerControlledController.playerClient.userName + " " + Vars.getPosition(playerControlledController.playerClient)).Replace("$PART$", Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart))).Replace("$DISTANCE$", Convert.ToString(distance) + "m"));
                                                        else
                                                            Vars.conLog.Chat("<SILENT BROADCAST> " + Vars.botName + ": " + message);
                                                    }

                                                    if (Vars.killsToConsole)
                                                    {
                                                        if (Vars.includePositionsInLog)
                                                            Vars.conLog.Info(Vars.murderMessageUnknown.Replace("$VICTIM$", playerClient.userName + " " + Vars.getPosition(playerClient)).Replace("$KILLER$", playerControlledController.playerClient.userName + " " + Vars.getPosition(playerControlledController.playerClient)).Replace("$PART$", Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart))).Replace("$DISTANCE$", Convert.ToString(distance) + "m"));
                                                        else
                                                            Vars.conLog.Info(message);
                                                    }
                                                }
                                                if (Vars.enableDropdownKills)
                                                {
                                                    string dropdownMessage = Vars.dropdownKillMessage.Replace("$VICTIM$", playerClient.userName).Replace("$PART$", Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart))).Replace("$DISTANCE$", Convert.ToString(distance) + "m");

                                                    Broadcast.noticeTo(playerControlledController.playerClient.netPlayer, "♞", dropdownMessage, 5);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Vars.conLog.Error("SDR #3: " + ex.ToString());
                                            }
                                            try
                                            {
                                                if (BodyParts.GetNiceName(damage.bodyPart) != null)
                                                    DeathScreen.SetReason(playerClient.netPlayer, playerControlledController.playerClient.userName + " killed you with a hit to your " + Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart)));
                                                else
                                                    DeathScreen.SetReason(playerClient.netPlayer, playerControlledController.playerClient.userName + " killed you.");
                                            }
                                            catch (Exception ex)
                                            {
                                                Vars.conLog.Error("SDR #4: " + ex.ToString());
                                            }
                                            if (!Vars.playerDeaths.ContainsKey(playerClient.userID.ToString()))
                                                Vars.playerDeaths.Add(playerClient.userID.ToString(), 1);
                                            else
                                                Vars.playerDeaths[playerClient.userID.ToString()] += 1;

                                            Data.updateDeathsData(playerClient.userID.ToString(), Vars.playerDeaths[playerClient.userID.ToString()]);

                                            if (!Vars.playerKills.ContainsKey(playerControlledController.playerClient.userID.ToString()))
                                                Vars.playerKills.Add(playerControlledController.playerClient.userID.ToString(), 1);
                                            else
                                                Vars.playerKills[playerControlledController.playerClient.userID.ToString()] += 1;

                                            Data.updateKillsData(playerControlledController.playerClient.userID.ToString(), Vars.playerKills[playerControlledController.playerClient.userID.ToString()]);
                                            return;
                                        }
                                    }
                                }

                                if (Vars.diedToBetty.KilledByBetty(playerClient.userID))
                                {
                                    BouncingBettyKill bettyKill = Vars.diedToBetty.GetByID(playerClient.userID);
                                    Vector3 bettyPos = bettyKill.bettyPos;
                                    double distance = -1;
                                    Character playerChar;
                                    if (Vars.getPlayerChar(playerClient, out playerChar))
                                    {
                                        Vector3 playerPos = playerChar.eyesOrigin;
                                        distance = Math.Round(Vector3.Distance(bettyPos, playerPos));
                                    }

                                    NetUser user = NetUser.FindByUserID(bettyKill.killerID);
                                    string killerName = "";
                                    if (user != null)
                                        killerName = user.playerClient.userName;
                                    else
                                    {
                                        // Grab name from file
                                    }
                                    string killerUID = bettyKill.killerID.ToString();
                                    if (Vars.murderMessages)
                                    {
                                        message = Vars.murderMessage.Replace("$VICTIM$", playerClient.userName).Replace("$KILLER$", killerName).Replace("$WEAPON$", "Bouncing Betty").Replace("$PART$", Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart))).Replace("$DISTANCE$", distance + "m");

                                        if (!Vars.hideKills)
                                        {
                                            Broadcast.broadcastKill(Vars.murderMessage.Replace("$VICTIM$", playerClient.userName + " " + Vars.getPosition(playerClient)).Replace("$KILLER$", killerName).Replace("$WEAPON$", "Bouncing Betty " + bettyPos).Replace("$PART$", Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart))).Replace("$DISTANCE$", Convert.ToString(distance) + "m"), message);
                                        }
                                        else
                                        {
                                            if (Vars.includePositionsInLog)
                                                Vars.conLog.Chat("<SILENT BROADCAST> " + Vars.botName + ": " + Vars.murderMessage.Replace("$VICTIM$", playerClient.userName + " " + Vars.getPosition(playerClient)).Replace("$KILLER$", killerName).Replace("$WEAPON$", "Bouncing Betty " + bettyPos).Replace("$PART$", Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart))).Replace("$DISTANCE$", Convert.ToString(distance) + "m"));
                                            else
                                                Vars.conLog.Chat("<SILENT BROADCAST> " + Vars.botName + ": " + message);
                                        }

                                        if (Vars.killsToConsole)
                                        {
                                            if (Vars.includePositionsInLog)
                                                Vars.conLog.Info(Vars.murderMessage.Replace("$VICTIM$", playerClient.userName + " " + Vars.getPosition(playerClient)).Replace("$KILLER$", killerName).Replace("$WEAPON$", "Bouncing Betty " + bettyPos).Replace("$PART$", Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart))).Replace("$DISTANCE$", Convert.ToString(distance) + "m"));
                                            else
                                                Vars.conLog.Info(message);
                                        }
                                    }
                                    if (Vars.enableDropdownKills && user != null)
                                    {
                                        string dropdownMessage = Vars.dropdownKillMessage.Replace("$VICTIM$", playerClient.userName).Replace("$PART$", Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart))).Replace("$DISTANCE$", Convert.ToString(distance) + "m");

                                        Broadcast.noticeTo(user.playerClient.netPlayer, "♞", dropdownMessage, 5);
                                    }
                                    DeathScreen.SetReason(playerClient.netPlayer, killerName + " killed you with a Bouncing Betty with a hit to your " + Vars.capitalizeFirst(BodyParts.GetNiceName(damage.bodyPart)));
                                    if (!Vars.playerDeaths.ContainsKey(playerClient.userID.ToString()))
                                        Vars.playerDeaths.Add(playerClient.userID.ToString(), 1);
                                    else
                                        Vars.playerDeaths[playerClient.userID.ToString()] += 1;

                                    Data.updateDeathsData(playerClient.userID.ToString(), Vars.playerDeaths[playerClient.userID.ToString()]);

                                    if (!Vars.playerKills.ContainsKey(killerUID))
                                        Vars.playerKills.Add(killerUID, 1);
                                    else
                                        Vars.playerKills[killerUID] += 1;

                                    Data.updateKillsData(killerUID, Vars.playerKills[killerUID]);

                                    Vars.diedToBetty.RemoveByID(playerClient.userID);
                                    return;
                                }

                                string killer = idMain.ToString();
                                if (killer.Contains("(Clone)"))
                                    killer = killer.Substring(0, killer.IndexOf("(Clone)"));

                                switch (killer)
                                {
                                    case "MutantBear":
                                        killer = "Mutant Bear";
                                        break;
                                    case "MutantWolf":
                                        killer = "Mutant Wolf";
                                        break;
                                    case "ExplosiveCharge":
                                        killer = "Explosive Charge";
                                        break;
                                    case "F1Grenade":
                                        killer = "F1 Grenade";
                                        break;
                                    case "F1GrenadeWorld":
                                        killer = "F1 Grenade";
                                        break;
                                    case "WoodSpikeWall":
                                        killer = "Small Spike Wall";
                                        break;
                                    case "LargeWoodSpikeWall":
                                        killer = "Large Spike Wall";
                                        break;
                                }
                                if (Vars.accidentMessages)
                                {
                                    message = Vars.accidentMessage.Replace("$VICTIM$", playerClient.userName).Replace("$KILLER$", killer);

                                    if (!Vars.hideKills)
                                    {
                                        if (Vars.includePositionsInLog)
                                            Broadcast.broadcastKill(Vars.accidentMessage.Replace("$VICTIM$", playerClient.userName + " " + Vars.getPosition(playerClient)).Replace("$KILLER$", killer), message);
                                        else
                                            Broadcast.broadcastKill(message);
                                    }
                                    else
                                    {
                                        if (Vars.includePositionsInLog)
                                            Vars.conLog.Chat("<SILENT BROADCAST> " + Vars.botName + ": " + Vars.accidentMessage.Replace("$VICTIM$", playerClient.userName + " " + Vars.getPosition(playerClient)).Replace("$KILLER$", killer));
                                        else
                                            Vars.conLog.Chat("<SILENT BROADCAST> " + Vars.botName + ": " + message);
                                    }

                                    if (Vars.killsToConsole)
                                    {
                                        if (Vars.includePositionsInLog)
                                            Vars.conLog.Info(Vars.accidentMessage.Replace("$VICTIM$", playerClient.userName + " " + Vars.getPosition(playerClient)).Replace("$KILLER$", killer));
                                        else
                                            Vars.conLog.Info(message);
                                    }
                                }

                                DeathScreen.SetReason(playerClient.netPlayer, "You were killed by a " + killer);
                                if (!Vars.playerDeaths.ContainsKey(playerClient.userID.ToString()))
                                    Vars.playerDeaths.Add(playerClient.userID.ToString(), 1);
                                else
                                    Vars.playerDeaths[playerClient.userID.ToString()] += 1;

                                Data.updateDeathsData(playerClient.userID.ToString(), Vars.playerDeaths[playerClient.userID.ToString()]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("SDR: " + ex.ToString());
            }
        }

        public static void onUserConnected(NetUser user)
        {
            try
            {
                if (!Vars.AllPlayerClients.Contains(user.playerClient))
                    Vars.AllPlayerClients.Add(user.playerClient);

                if (!Vars.teleportRequests.ContainsKey(user.playerClient))
                {
                    Vars.teleportRequests.Add(user.playerClient, new Dictionary<PlayerClient, TimerPlus>());
                    Vars.latestRequests.Add(user.playerClient, null);
                }

                string steamUID = user.userID.ToString();
                if (!Vars.firstPlayerJoined)
                {
                    Vars.firstPlayerJoined = true;
                    Vars.loadEnvironment();
                    //truth.punish = false;
                    //truth.threshold = 999999999;
                }
                RustEssentialsBootstrap._load.loadBans();
                if (Vars.currentBans.ContainsKey(steamUID))
                {
                    Vars.kickPlayer(user, Vars.currentBanReasons[steamUID], true);
                }
                else
                {
                    bool containsIllegalWord = false;
                    List<string> illegalWords = new List<string>();
                    foreach (string s in illegalWords)
                    {
                        if (user.displayName.ToLower().Contains(s.ToLower()))
                        {
                            containsIllegalWord = true;
                            illegalWords.Add(s);
                        }
                    }

                    bool containsIllegalChar = false;
                    List<string> illegalChars = new List<string>();
                    foreach (char c in user.displayName)
                    {
                        if (!Vars.allowedChars.Contains(c.ToString().ToLower()) && !Vars.allowedChars.Contains(c.ToString()) && c != ' ' && c != ',')
                        {
                            illegalChars.Add(c.ToString());
                            containsIllegalChar = true;
                        }
                    }
                    bool nameOccupied = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == user.displayName).Count() > 1;
                    PlayerClient connectedClient = null;
                    int instanceNum = 0;
                    if (nameOccupied)
                    {
                        connectedClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == user.displayName && pc.userID != user.userID);
                        instanceNum = Array.FindAll(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.userName == user.displayName).Count();
                    }

                    if ((Vars.censorship && containsIllegalWord) || (Vars.restrictChars && containsIllegalChar) || (user.displayName.Length > Vars.maximumNameCount) || (user.displayName.Length < Vars.minimumNameCount) || (Vars.kickDuplicate && nameOccupied) || (user.displayName == Vars.botName || user.displayName == "[pm] " + Vars.botName || user.displayName == "[PM]" + Vars.botName) || user.displayName.ToLower().Contains("[color"))
                    {
                        if (containsIllegalWord)
                            Vars.otherKick(user, "Illegal words in name: " + string.Join(", ", illegalWords.ToArray()));
                        else
                        {
                            if (containsIllegalChar)
                                Vars.otherKick(user, "Illegal characters in name: " + string.Join(", ", illegalChars.ToArray()));
                            else
                            {
                                if (user.displayName.Length > Vars.maximumNameCount)
                                    Vars.otherKick(user, "Name must be less than " + Vars.maximumNameCount + " characters.");
                                else
                                {
                                    if (user.displayName.Length < Vars.minimumNameCount)
                                        Vars.otherKick(user, "Name must be more than " + Vars.minimumNameCount + " characters.");
                                    else
                                    {
                                        if (nameOccupied)
                                        {
                                            if (Vars.lowerAuthority)
                                            {
                                                if (Checks.ofLowerRank(user.userID.ToString(), true, connectedClient.userID.ToString(), true))
                                                    Vars.otherKick(user, "Player name \"" + user.displayName + "\" already in use.");
                                                else if (Checks.ofLowerRank(connectedClient.userID.ToString(), true, user.userID.ToString(), true))
                                                    Vars.otherKick(connectedClient.netUser, "Player of higher authority with name \"" + user.displayName + "\" joined.");
                                                else if (Checks.ofEqualRank(connectedClient.userID.ToString(), true, user.userID.ToString(), true))
                                                    Vars.otherKick(user, "Player name \"" + user.displayName + "\" already in use.");
                                            }
                                            else
                                                Vars.otherKick(user, "Player name \"" + user.displayName + "\" already in use.");
                                        }
                                        else
                                        {
                                            if (user.displayName == Vars.botName || user.displayName == "[pm] " + Vars.botName || user.displayName == "[PM]" + Vars.botName)
                                                Vars.otherKick(user, "You cannot impersonate the server bot.");
                                            else
                                            {
                                                if (user.displayName.ToLower().Contains("[color"))
                                                    Vars.otherKick(user, "You are not allowed to have color tags in your name.");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!Vars.blockedRequestsPer.ContainsKey(user.userID.ToString()))
                            Vars.blockedRequestsPer.Add(user.userID.ToString(), new Dictionary<string, TimerPlus>());

                        if (Vars.useSteamGroup && Vars.groupMembers.Contains(steamUID))
                        {
                            Vars.conLog.Info("Player " + user.displayName + " (" + steamUID + ") has connected through steam group \"" + Vars.steamGroup + "\".");

                            Broadcast.broadcastTo(user.networkPlayer, "The server is running RustEssentials v" + Vars.currentVersion + ".");
                            if (Vars.motdList.ContainsKey("Join"))
                            {
                                foreach (string s in Vars.motdList["Join"])
                                {
                                    Broadcast.broadcastTo(user.networkPlayer, s);
                                }
                            }
                            switch (Vars.defaultChat)
                            {
                                case "global":
                                    if (!Vars.inGlobal.Contains(user.userID.ToString()))
                                        Vars.inGlobal.Add(user.userID.ToString());
                                    break;
                                case "direct":
                                    if (!Vars.inDirect.Contains(user.userID.ToString()))
                                        Vars.inDirect.Add(user.userID.ToString());
                                    break;
                            }
                            if (!Vars.inDirectV.Contains(user.userID.ToString()))
                                Vars.inDirectV.Add(user.userID.ToString());
                            string joinMessage = "";
                            if (Vars.enableJoin && user.playerClient.userName.Length > 0)
                            {
                                joinMessage = Vars.joinMessage.Replace("$USER$", Vars.filterFullNames(user.displayName, steamUID));
                                Broadcast.broadcastJoinLeave(joinMessage);
                                Vars.conLog.Chat("<BROADCAST ALL> " + Vars.botName + ": " + joinMessage + " (" + steamUID + ") [" + user.networkPlayer.ipAddress + "]");
                            }
                            if (!Vars.playerIPs.ContainsKey(user.playerClient))
                                Vars.playerIPs.Add(user.playerClient, user.networkPlayer.ipAddress);
                            Vars.conLog.logToFile("Player " + user.displayName + " (" + steamUID + ") has joined. [" + user.networkPlayer.ipAddress + "]", "info");

                            Vars.callAPI("RustEssentialsAPI.APIPlayer", "AddPlayer", false, user.userID.ToString(), user.displayName, user.playerClient.userName, user.networkPlayer, user.playerClient, user);
                        }
                        else
                        {
                            if (!Vars.whitelist.Contains(steamUID) && Vars.enableWhitelist)
                            {
                                Vars.whitelistKick(user, Vars.whitelistKickJoin);
                            }
                            else
                            {
                                Broadcast.broadcastTo(user.networkPlayer, "The server is running RustEssentials v" + Vars.currentVersion + ".");
                                if (Vars.motdList.Keys.Contains("Join"))
                                {
                                    foreach (string s in Vars.motdList["Join"])
                                    {
                                        Broadcast.broadcastTo(user.networkPlayer, s);
                                    }
                                }
                                switch (Vars.defaultChat)
                                {
                                    case "global":
                                        if (!Vars.inGlobal.Contains(user.userID.ToString()))
                                            Vars.inGlobal.Add(user.userID.ToString());
                                        break;
                                    case "direct":
                                        if (!Vars.inDirect.Contains(user.userID.ToString()))
                                            Vars.inDirect.Add(user.userID.ToString());
                                        break;
                                }
                                if (!Vars.inDirectV.Contains(user.userID.ToString()))
                                    Vars.inDirectV.Add(user.userID.ToString());
                                string joinMessage = "";
                                if (Vars.enableJoin && user.playerClient.userName.Length > 0)
                                {
                                    joinMessage = Vars.joinMessage.Replace("$USER$", Vars.filterFullNames(user.displayName, steamUID));
                                    Broadcast.broadcastJoinLeave(joinMessage);
                                    Vars.conLog.Chat("<BROADCAST ALL> " + Vars.botName + ": " + joinMessage + " (" + steamUID + ") [" + user.networkPlayer.ipAddress + "]");
                                }
                                if (!Vars.playerIPs.ContainsKey(user.playerClient))
                                    Vars.playerIPs.Add(user.playerClient, user.networkPlayer.ipAddress);
                                Vars.conLog.logToFile("Player " + user.displayName + " (" + user.userID + ") has joined. [" + user.networkPlayer.ipAddress + "]", "info");

                                Vars.callAPI("RustEssentialsAPI.APIPlayer", "AddPlayer", false, user.userID.ToString(), user.displayName, user.playerClient.userName, user.networkPlayer, user.playerClient, user);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("OUCon: " + ex.ToString());
            }
        }

        public static void onServerInit()
        {
            Vars.callAPI("RustEssentialsAPI.Hooks", "OnServerInit", false);
        }

        public static void callAirDropAt(Vector3 targetPos)
        {
            Vector3 pos = targetPos;
            SupplyDropPlane component = NetCull.LoadPrefab("C130").GetComponent<SupplyDropPlane>();
            float num = 20f * component.maxSpeed;
            Vector3 position = pos + ((Vector3)(SupplyDropZone.RandomDirectionXZ() * num));
            pos += new Vector3(0f, 300f, 0f);
            position += new Vector3(0f, 400f, 0f);
            Vector3 vector3 = pos - position;
            Quaternion rotation = Quaternion.LookRotation(vector3.normalized);
            int group = 0;
            GameObject GO = NetCull.InstantiateClassic("C130", position, rotation, group);
            SupplyDropPlane plane = GO.GetComponent<SupplyDropPlane>();
            plane.SetDropTarget(pos);
            Hook hook = Vars.callHook("RustEssentialsAPI.Hooks", "OnAirdrop", false, plane, pos);
            if (!Checks.ContinueHook(hook))
                NetCull.Destroy(GO);
        }

        public static void runAction(int number, uLink.BitStream stream, ref uLink.NetworkMessageInfo info, ItemRepresentation IR)
        {
            switch (number)
            {
                case 1:
                    IR.datablock.DoAction1(stream, IR, ref info);
                    break;

                case 2:
                    IR.datablock.DoAction2(stream, IR, ref info);
                    break;

                case 3:
                    IR.datablock.DoAction3(stream, IR, ref info);
                    break;
            }
        }

        public static void sendFallImpact(Vector3 velocity, uLink.NetworkMessageInfo info, FallDamage FD)
        {
            if (info.sender != FD.networkView.owner)
            {
                NetUser user = NetUser.Find(info.sender);
                if (user != null)
                {
                    FeedbackLog.Start(FeedbackLog.TYPE.SimpleExploit);
                    FeedbackLog.Writer.Write("fall");
                    FeedbackLog.Writer.Write(user.userID);
                    FeedbackLog.End(FeedbackLog.TYPE.SimpleExploit);
                }
            }
            else
            {
                float num;
                if (FD.ValidateFallVelocity(velocity, out num))
                {

                    PlayerClient playerClient = Vars.getPlayerClient(info.networkView.owner);

                    if (playerClient != null)
                    {
                        bool hasWand = false;
                        bool hasPortal = false;
                        if (playerClient.controllable.GetComponent<Inventory>() != null)
                        {
                            if (playerClient.controllable.GetComponent<Inventory>().activeItem != null)
                            {
                                if (playerClient.controllable.GetComponent<Inventory>().activeItem.datablock != null)
                                {
                                    string heldItem = playerClient.controllable.GetComponent<Inventory>().activeItem.datablock.name;

                                    if (Vars.wandList.ContainsKey(playerClient.userID.ToString()))
                                    {
                                        hasWand = true;
                                        if (heldItem != Vars.wandName && Vars.wandName != "any")
                                            hasWand = false;
                                    }
                                    if (Vars.portalList.Contains(playerClient.userID.ToString()))
                                    {
                                        hasPortal = true;
                                        if (heldItem != Vars.portalName && Vars.portalName != "any")
                                            hasPortal = false;
                                    }
                                }
                            }
                        }
                        if (!Vars.godList.Contains(playerClient.userID.ToString()) && !Vars.frozenPlayers.ContainsKey(playerClient.userID) && !Vars.vanishedList.Contains(playerClient.userID.ToString()) && !hasWand && !hasPortal)
                        {
                            if (Vars.fallDamage)
                            {
                                FD.networkView.RPC<float>("fIc", uLink.RPCMode.Others, num);
                                FD.FallImpact(num);
                            }
                            else
                            {
                                if (Vars.enableFallSound)
                                    FD.networkView.RPC<float>("fIc", uLink.RPCMode.Others, num);
                            }
                        }
                    }
                }
            }
        }

        public static void fallImpact(float fallspeed, float min_vel, float max_vel, float healthFraction, Character idMain, float maxHealth, FallDamage fallDamage)
        {
            try
            {
                if (Vars.fallDamage)
                {
                    float num = (fallspeed - min_vel) / (max_vel - min_vel);
                    bool flag = num > 0.25f;
                    bool flag2 = ((num > 0.35f) || (UnityEngine.Random.Range(0, 3) == 0)) || (healthFraction < 0.5f);
                    if (!Vars.godList.Contains(idMain.playerClient.userID.ToString()) && !Vars.frozenPlayers.ContainsKey(idMain.playerClient.userID) && !Vars.vanishedList.Contains(idMain.playerClient.userID.ToString()))
                    {
                        if (flag)
                        {
                            idMain.controllable.GetComponent<HumanBodyTakeDamage>().AddBleedingLevel(3f + ((num - 0.25f) * 10f));
                        }
                        if (flag2)
                        {
                            fallDamage.AddLegInjury(1f);
                        }
                        TakeDamage.HurtSelf(idMain, (float)(10f + (num * maxHealth)));
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("FALLI: " + ex.ToString());
            }
        }

        public static void structureUCH(IDMain idMain, StructureComponent structureComponent)
        {
            NetEntityID entID = NetEntityID.Get(structureComponent);
            if (!Vars.beingDestroyed.Contains(idMain.gameObject))
                structureComponent.healthBuffer.CheckAuto(structureComponent, "ClientHealthUpdate", false, false, 0.01f, 0);
            else
            {
                NetCull.RemoveRPCsByName(entID, "ClientHealthUpdate");
                Vars.beingDestroyed.Remove(idMain.gameObject);
            }
        }

        public static void deployableUCH(IDMain idMain, DeployableObject deployableObject)
        {
            NetEntityID entID = NetEntityID.Get(deployableObject);
            if (!Vars.beingDestroyed.Contains(idMain.gameObject))
                deployableObject.healthBuffer.CheckAuto(deployableObject, "ClientHealthUpdate", false, false, 0.01f, 0);
            else
            {
                NetCull.RemoveRPCsByName(entID, "ClientHealthUpdate");
                Vars.beingDestroyed.Remove(idMain.gameObject);
            }
        }

        public static void giveSpawnItems(Inventory inventory, Loadout loadout)
        {
            if (inventory.noOccupiedSlots) // Completely empty inventory, just spawned
            {
                try
                {
                    PlayerClient playerClient = Vars.getPlayerClient(inventory.networkView.owner);
                    if (playerClient != null)
                    {
                        foreach (KeyValuePair<string, List<Items.Item>> kv in Vars.defaultLoadout)
                        {
                            if (kv.Key == "Hotbar")
                            {
                                foreach (Items.Item item in kv.Value)
                                {
                                    Items.addHotbarItem(playerClient, item.name, item.amount, item.uses, item.modSlots, item.mods);
                                }
                            }
                            else if (kv.Key == "Inventory")
                            {
                                foreach (Items.Item item in kv.Value)
                                {
                                    Items.addItemThroughKit(playerClient, item.name, item.amount, item.uses, item.modSlots, item.mods);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Vars.conLog.Error("GSI #1: " + ex.ToString());
                }
                //Inventory.Addition[] emptyInventoryAdditions = loadout.emptyInventoryAdditions;
                //for (int i = 0; i < emptyInventoryAdditions.Length; i++)
                //{
                //    inventory.AddItem(ref emptyInventoryAdditions[i]);
                //}
            }
            else // Simply logging in, inventory contains items
            {
                try
                {
                    PlayerClient playerClient = Vars.getPlayerClient(inventory.networkView.owner);
                    if (playerClient != null)
                    {
                        if (!Items.hasMelee(playerClient))
                        {
                            Items.addHotbarItem(Vars.getPlayerClient(inventory.networkView.owner), "Rock", 1, -1, -1, null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Vars.conLog.Error("GSI #2: " + ex.ToString());
                }
                //loadout.GetMinimumRequirementArray(ref loadout._minimumRequirements, false);
                //foreach (Loadout.Entry entry in loadout._minimumRequirements)
                //{
                //    int num3;
                //    if (inventory.FindItem(entry.item, out num3) != null) // If the item I was going to add is in my inventory
                //    {
                //        if (entry.item.IsSplittable()) // If it is possible to stack the item (Wood, ore, etc)
                //        {
                //            int useCount = entry.useCount;
                //            if (num3 < useCount)
                //            {
                //                inventory.AddItemAmount(entry.item, useCount - num3);
                //            }
                //        }
                //    }
                //    else if (!inventory.noVacantSlots) // If my inventory has space and the item I was going to add is NOT in my inventory
                //    {
                //        Broadcast.broadcastTo(inventory.networkView.owner, entry.item.name);
                //        inventory.AddItemSomehow(entry.item, new Inventory.Slot.Kind?(entry.inferredSlotKind), entry.inferredSlotOfKind, entry.useCount);
                //    }
                //}
            }
        }

        public static void updateDropTimer(SupplyDropTimer SDT)
        {
            if (EnvironmentControlCenter.Singleton != null && Vars.dropMode == 0)
            {
                float time = EnvironmentControlCenter.Singleton.GetTime();
                if (SDT.nextDropTime != -1f && (time > Vars.dropTime))
                {
                    if (NetCull.connections.Length >= Vars.minimumPlayers)
                    {
                        for (int i = 0; i < Vars.planeCount; i++)
                        {
                            Vars.airdropServer();
                        }
                    }
                    SDT.nextDropTime = -1f;
                }
                if (SDT.nextDropTime == -1f && (time < Vars.dropTime))
                {
                    resetDropTime(SDT);
                }
            }
        }

        public static void resetDropTime(SupplyDropTimer SDT)
        {
            if (Vars.dropMode == 0)
                SDT.nextDropTime = Vars.dropTime;
                //SDT.nextDropTime = UnityEngine.Random.Range(SDT.dropTimeDayMin, SDT.dropTimeDayMax);
        }

        public static void targetReached(SupplyDropPlane SDP)
        {
            if (!SDP.droppedPayload)
            {
                SDP.droppedPayload = true;
                int num = UnityEngine.Random.Range(Vars.minimumCrates, Vars.maximumCrates + 1);
                float time = 0f;
                for (int i = 0; i < num; i++)
                {
                    SDP.Invoke("DropCrate", time);
                    time += UnityEngine.Random.Range(0.3f, 0.6f);
                }
                SDP.targetPos += (Vector3)((SDP.transform.forward * SDP.maxSpeed) * 30f);
                SDP.targetPos.y += 800f;
                SDP.Invoke("NetDestroy", 20f);
            }
        }

        public static void structureComponentAction(uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info, StructureComponentDataBlock SCDB)
        {
            uLink.NetworkPlayer netPlayer = info.sender;
            PlayerClient playerClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == netPlayer);
            IStructureComponentItem item;
            NetCull.VerifyRPC(ref info);
            if (rep.Item<IStructureComponentItem>(out item) && (item.uses > 0))
            {
                StructureComponent structureToPlacePrefab = SCDB.structureToPlacePrefab;
                Vector3 origin = stream.ReadVector3();
                Vector3 direction = stream.ReadVector3();
                Vector3 position = stream.ReadVector3();
                Vector3 checkPos = position;
                checkPos.y += 5f;
                Quaternion rotation = stream.ReadQuaternion();
                uLink.NetworkViewID viewID = stream.ReadNetworkViewID();
                StructureMaster component = null;
                if (SCDB.structureToPlacePrefab.type == StructureComponent.StructureComponentType.Foundation || SCDB.structureToPlacePrefab.type == StructureComponent.StructureComponentType.Ramp)
                {
                    RaycastHit[] hits = Physics.SphereCastAll(checkPos, 2.6f, Vector3.down, 4f);
                    foreach (var hit in hits)
                    {
                        if (hit.collider.gameObject.GetComponent<DeployableObject>() != null && Mathf.Abs(hit.collider.gameObject.transform.position.y - position.y) < 4)
                        {
                            //Vars.conLog.Info("Hit Pos: " + hit.collider.gameObject.transform.position + " - Foundation Pos: " + position);
                            Rust.Notice.Popup(info.sender, "", "You can't place that on top of objects", 4f);
                            return;
                        }
                    }
                }

                if (SCDB.structureToPlacePrefab.type == StructureComponent.StructureComponentType.Foundation)
                {
                    RaycastHit[] hits = Physics.SphereCastAll(checkPos, 2.8f, Vector3.down, 15f);
                    foreach (var hit in hits)
                    {
                        if (hit.collider.gameObject.GetComponent<BouncingBetty>() != null)
                        {
                            //Vars.conLog.Info("Hit Pos: " + hit.collider.gameObject.transform.position + " - Foundation Pos: " + position);
                            Rust.Notice.Popup(info.sender, "", "You can't place that near a bouncing betty", 5f);
                            return;
                        }
                    }
                }

                if (SCDB.structureToPlacePrefab.type == StructureComponent.StructureComponentType.Pillar)
                {
                    RaycastHit[] hits = Physics.SphereCastAll(checkPos + new Vector3(0, -4.4f, 0), 0.15f, Vector3.down, 1f);
                    foreach (var hit in hits)
                    {
                        if (hit.collider.gameObject.GetComponent<DeployableObject>() != null && hit.collider.gameObject.GetComponent<DeployableObject>().name.Contains("Stash") && Mathf.Abs(hit.collider.gameObject.transform.position.y - position.y) < 4)
                        {
                            Rust.Notice.Popup(info.sender, "", "You can't place that on top of stashes", 4f);
                            return;
                        }
                    }
                }
                Zone zone;
                if (Checks.nearZone(position, out zone) && !Vars.buildList.Contains(playerClient.userID.ToString()) && !zone.buildable)
                {
                    Rust.Notice.Popup(info.sender, "", "You can't place that near safe zones or war zones.", 4f);
                }
                else
                {
                    if (viewID == uLink.NetworkViewID.unassigned)
                    {
                        if (SCDB.MasterFromRay(new Ray(origin, direction)))
                        {
                            return;
                        }
                        if (structureToPlacePrefab.type != StructureComponent.StructureComponentType.Foundation)
                        {
                            Debug.Log("ERROR, tried to place non foundation structure on terrain!");
                        }
                        else
                        {
                            component = NetCull.InstantiateClassic<StructureMaster>(Bundling.Load<StructureMaster>("content/structures/StructureMasterPrefab"), position, rotation, 0);
                            component.SetupCreator(item.controllable);
                        }
                    }
                    else
                    {
                        component = uLink.NetworkView.Find(viewID).gameObject.GetComponent<StructureMaster>();
                    }
                    if (component == null)
                    {
                        Debug.Log("NO master, something seriously wrong");
                    }
                    else if (SCDB._structureToPlace.CheckLocation(component, position, rotation) && SCDB.CheckBlockers(position))
                    {
                        //Vars.conLog.Info(SCDB.structureToPlaceName);
                        StructureComponent comp = NetCull.InstantiateStatic(SCDB.structureToPlaceName, position, rotation).GetComponent<StructureComponent>();
                        if (comp != null)
                        {
                            component.AddStructureComponent(comp);
                            int count = 1;
                            if (item.Consume(ref count))
                            {
                                item.inventory.RemoveItem(item.slot);
                            }
                        }
                    }
                }
            }
        }

        public static void deployableItemAction(uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info, DeployableItemDataBlock DIDB)
        {
            try
            {
                uLink.NetworkPlayer netPlayer = info.sender;
                PlayerClient playerClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == netPlayer);
                IDeployableItem item;
                NetCull.VerifyRPC(ref info);
                if (rep.Item<IDeployableItem>(out item) && (item.uses > 0))
                {
                    Vector3 vector3;
                    Quaternion quaternion;
                    TransCarrier carrier;
                    Vector3 origin = stream.ReadVector3();
                    Vector3 direction = stream.ReadVector3();
                    Ray ray = new Ray(origin, direction);
                    if (!DIDB.CheckPlacement(ray, out vector3, out quaternion, out carrier))
                    {
                        Rust.Notice.Popup(info.sender, "", "You can't place that here.", 4f);
                    }
                    else
                    {
                        Zone zone;
                        if (Checks.nearZone(vector3, out zone) && !Vars.buildList.Contains(playerClient.userID.ToString()) && !zone.buildable)
                        {
                            Rust.Notice.Popup(info.sender, "", "You can't place that near safe zones or war zones.", 4f);
                        }
                        else
                        {
                            DeployableObject component = NetCull.InstantiateStatic(DIDB.DeployableObjectPrefabName, vector3, quaternion).GetComponent<DeployableObject>(); // Creates model in world space
                            if (component != null)
                            {
                                if (Vars.logBedPlacements && (component.name == "SleepingBagA(Clone)" || component.name == "SingleBed(Clone)"))
                                    Vars.conLog.Info(playerClient.userName + " (" + playerClient.userID + ") placed a " + (component.name.Contains("Sleeping") ? "Sleeping Bag" : "Bed") + " at " + vector3);

                                if (component.name == "WoodBox(Clone)" || component.name == "WoodBoxLarge(Clone)" || component.name == "SmallStash(Clone)" || component.name == "Campfire(Clone)" || component.name == "Furnace(Clone)")
                                {
                                    Vars.conLog.Storage(playerClient.userName + " (" + playerClient.userID + ") placed a " + DIDB.name + " at " + vector3 + ".");
                                }

                                string fancyName = Vars.getFullObjectName(component.name);

                                if (fancyName == "Camp Fire" || fancyName == "Furnace")
                                {
                                    StructureMaster master;
                                    if (Checks.onStructure(component.transform.position, out master))
                                        Lights.add(playerClient.userID, component, master);
                                    else
                                        Lights.add(playerClient.userID, component);
                                }

                                try
                                {
                                    component.SetupCreator(item.controllable); // Sets object variables such as ownerID
                                    DIDB.SetupDeployableObject(stream, rep, ref info, component, carrier);
                                }
                                finally
                                {
                                    int count = 1;
                                    if (item.Consume(ref count))
                                    {
                                        item.inventory.RemoveItem(item.slot);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("DEP: " + ex.ToString());
            }
        }

        public static void reloadWeapon(BulletWeaponItem<BulletWeaponDataBlock> BWI)
        {
            BWI.reloadStartTime = UnityEngine.Time.time;
            BWI.nextPrimaryAttackTime = UnityEngine.Time.time + BWI.datablock.reloadDuration;
            Inventory inventory = BWI.inventory;
            int uses = BWI.uses;
            int maxClipAmmo = BWI.datablock.maxClipAmmo;
            if (uses != maxClipAmmo)
            {
                int count = maxClipAmmo - uses;
                int num4 = 0;
                bool breakEarly = false;

                PlayerClient playerClient;
                if (Vars.getPlayerClient(inventory.networkView.owner, out playerClient))
                {
                    if (!Items.hasItem(playerClient, BWI.datablock.ammoType.name))
                    {
                        if (Vars.unlAmmoList.Contains(playerClient.userID.ToString()))
                        {
                            Items.addItem(playerClient, BWI.datablock.ammoType.name, count);
                            breakEarly = true;
                        }
                    }
                }

                while (uses < maxClipAmmo)
                {
                    IInventoryItem item = inventory.FindItem(BWI.datablock.ammoType);
                    if (item == null)
                    {
                        break;
                    }
                    int num5 = count;
                    if (item.Consume(ref count))
                    {
                        inventory.RemoveItem(item.slot);
                    }

                    if (breakEarly)
                        break;

                    PlayerClient playerClient2;
                    if (Vars.getPlayerClient(inventory.networkView.owner, out playerClient2))
                    {
                        if (Vars.unlAmmoList.Contains(playerClient2.userID.ToString()))
                            Items.addItem(playerClient2, BWI.datablock.ammoType.name, num5 - count);
                    }

                    num4 += num5 - count;
                    if (count == 0)
                    {
                        break;
                    }
                }
                if (num4 > 0)
                {
                    BWI.AddUses(num4);
                }
            }
        }

        public static void shootWeapon(uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info, BulletWeaponDataBlock BWDB)
        {
            GameObject obj2 = null;
            NetEntityID yid;
            IDRemoteBodyPart part;
            bool flag;
            bool flag2;
            bool flag3;
            BodyPart part2;
            Vector3 vector;
            Vector3 vector2;
            Transform transform;
            IBulletWeaponItem item;
            NetCull.VerifyRPC(ref info, false);
            BWDB.ReadHitInfo(stream, out obj2, out flag, out flag2, out part2, out part, out yid, out transform, out vector, out vector2, out flag3);
            if ((rep.Item<IBulletWeaponItem>(out item) && item.ValidatePrimaryMessageTime(info.timestamp)) && (item.uses > 0))
            {
                TakeDamage local = item.inventory.GetLocal<TakeDamage>();
                if ((local == null) || !local.dead)
                {

                    uLink.NetworkPlayer netPlayer = info.networkView.owner;

                    bool hasWand = false;
                    bool hasPortal = false;
                    bool hasEBullets = false;
                    PlayerClient playerClient = null;

                    try
                    {
                        playerClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == netPlayer);

                        if (playerClient != null)
                        {
                            if (Vars.explosiveBulletList.Contains(playerClient.userID.ToString()))
                            {
                                hasEBullets = true;
                            }
                            if (playerClient.controllable != null)
                            {
                                if (playerClient.controllable.GetComponent<Inventory>() != null)
                                {
                                    if (playerClient.controllable.GetComponent<Inventory>().activeItem != null)
                                    {
                                        if (playerClient.controllable.GetComponent<Inventory>().activeItem.datablock != null)
                                        {
                                            string heldItem = playerClient.controllable.GetComponent<Inventory>().activeItem.datablock.name;
                                            if (Vars.portalList.Contains(playerClient.userID.ToString()) && Vars.wandName == Vars.portalName)
                                            {
                                                Broadcast.broadcastTo(netPlayer, "Portal tool and wand tool cannot be the same item! Portal tool disabled.");
                                                Vars.portalList.Remove(playerClient.userID.ToString());
                                            }
                                            if (Vars.wandList.ContainsKey(playerClient.userID.ToString()))
                                            {
                                                hasWand = true;
                                                if (heldItem != Vars.wandName && Vars.wandName != "any")
                                                    hasWand = false;
                                            }
                                            if (Vars.portalList.Contains(playerClient.userID.ToString()))
                                            {
                                                hasPortal = true;
                                                if (heldItem != Vars.portalName && Vars.portalName != "any")
                                                    hasPortal = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex) { Vars.conLog.Error("SHOOT: " + ex.ToString()); }

                    int curAmmo = item.uses;
                    int count = 1;

                    if (Vars.infAmmoList.Contains(playerClient.userID.ToString()))
                    {
                        Items.setUses(item, Vars.infAmmoClipSize < 0 ? BWDB._maxUses : Vars.infAmmoClipSize);
                    }
                    else
                    {
                        item.Consume(ref count);
                    }

                    if (hasWand)
                    {
                        Character playerChar;
                        if (playerClient != null)
                        {
                            Character.FindByUser(playerClient.userID, out playerChar);

                            if (playerChar != null)
                            {
                                Vector3 newVector = playerChar.eyesOrigin + (playerChar.eyesAngles.forward * Vars.wandList[playerClient.userID.ToString()]);
                                if (newVector.y > 5000)
                                    newVector.y = 5000;
                                int curIndex = 0;
                                if (!Checks.aboveGround(newVector))
                                {
                                    while (true)
                                    {
                                        if (curIndex >= 50)
                                            break;

                                        if (!Checks.aboveGround(newVector))
                                        {
                                            curIndex++;
                                            newVector.y += 5;
                                        }
                                        else
                                            break;
                                    }
                                }

                                RustServerManagement RSM = RustServerManagement.Get();
                                if (!Vars.frozenPlayers.ContainsKey(playerClient.userID))
                                {
                                    if (playerClient.netPlayer != null)
                                        RSM.TeleportPlayerToWorld(playerClient.netPlayer, newVector);
                                }
                                else
                                {
                                    Vars.frozenPlayers[playerClient.userID] = newVector;
                                }
                            }
                        }
                    }
                    else if (hasPortal)
                    {
                        Thread t = new Thread(() =>
                            {
                                Character playerChar;
                                if (playerClient != null)
                                {
                                    Character.FindByUser(playerClient.userID, out playerChar);

                                    if (playerChar != null)
                                    {
                                        Vector3 newVector;
                                        if (Checks.isPointingAtObject(playerChar, out newVector))
                                        {
                                            if (newVector.y > 5000)
                                                newVector.y = 5000;
                                            int curIndex = 0;
                                            if (!Checks.aboveGround(newVector))
                                            {
                                                while (true)
                                                {
                                                    if (curIndex >= 50)
                                                        break;

                                                    if (!Checks.aboveGround(newVector))
                                                    {
                                                        curIndex++;
                                                        newVector.y += 2.5f;
                                                    }
                                                    else
                                                        break;
                                                }
                                            }
                                            RustServerManagement RSM = RustServerManagement.Get();
                                            if (playerClient != null)
                                            {
                                                if (playerClient.netPlayer != null)
                                                {
                                                    RSM.TeleportPlayerToWorld(playerClient.netPlayer, newVector);
                                                }
                                            }
                                        }
                                    }
                                }
                            });
                        t.IsBackground = true;
                        t.Start();
                    }
                    else
                    {
                        rep.ActionStream(1, uLink.RPCMode.AllExceptOwner, stream);
                        if (obj2 != null)
                        {
                            if (hasEBullets)
                                Explosions.explode(vector);
                            if (Vars.oposList.Contains(playerClient.userID.ToString()))
                            {
                                Broadcast.broadcastTo(playerClient.netPlayer, obj2.transform.position.ToString());
                            }
                            else
                                BWDB.ApplyDamage(obj2, transform, flag3, vector, part2, rep);
                        }
                        else if (hasEBullets)
                        {
                            Thread t = new Thread(() =>
                            {
                                Character playerChar;
                                if (playerClient != null)
                                {
                                    Character.FindByUser(playerClient.userID, out playerChar);

                                    if (playerChar != null)
                                    {
                                        Vector3 newVector;
                                        if (Checks.isPointingAtObject(playerChar, out newVector))
                                        {
                                            if (newVector.y > 5000)
                                                newVector.y = 5000;
                                            int curIndex = 0;
                                            if (!Checks.aboveGround(newVector))
                                            {
                                                while (true)
                                                {
                                                    if (curIndex >= 50)
                                                        break;

                                                    if (!Checks.aboveGround(newVector))
                                                    {
                                                        curIndex++;
                                                        newVector.y += 0.2f;
                                                    }
                                                    else
                                                        break;
                                                }
                                            }
                                            Explosions.explode(newVector);
                                        }
                                    }
                                }
                            });
                            t.IsBackground = true;
                            t.Start();
                        }
                        if (gunshots.aiscared)
                        {
                            local.GetComponent<Character>().AudibleMessage(20f, "HearDanger", local.transform.position);
                            local.GetComponent<Character>().AudibleMessage(10f, "HearDanger", vector);
                        }
                        item.TryConditionLoss(0.33f, 0.01f);
                    }
                }
            }
        }

        public static void shootShotgun(uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info, ShotgunDataBlock SDB)
        {
            NetCull.VerifyRPC(ref info, false);
            IBulletWeaponItem found = null;
            if (rep.Item<IBulletWeaponItem>(out found) && (found.uses > 0))
            {
                TakeDamage local = found.inventory.GetLocal<TakeDamage>();
                if (((local == null) || !local.dead) && found.ValidatePrimaryMessageTime(info.timestamp))
                {

                    uLink.NetworkPlayer netPlayer = info.networkView.owner;

                    bool hasWand = false;
                    bool hasPortal = false;
                    PlayerClient playerClient = null;

                    try
                    {
                        playerClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == netPlayer);

                        if (playerClient != null)
                        {
                            if (playerClient.controllable != null)
                            {
                                if (playerClient.controllable.GetComponent<Inventory>() != null)
                                {
                                    if (playerClient.controllable.GetComponent<Inventory>().activeItem != null)
                                    {
                                        if (playerClient.controllable.GetComponent<Inventory>().activeItem.datablock != null)
                                        {
                                            string heldItem = playerClient.controllable.GetComponent<Inventory>().activeItem.datablock.name;
                                            if (Vars.portalList.Contains(playerClient.userID.ToString()) && Vars.wandName == Vars.portalName)
                                            {
                                                Broadcast.broadcastTo(netPlayer, "Portal tool and wand tool cannot be the same item! Portal tool disabled.");
                                                Vars.portalList.Remove(playerClient.userID.ToString());
                                            }
                                            if (Vars.wandList.ContainsKey(playerClient.userID.ToString()))
                                            {
                                                hasWand = true;
                                                if (heldItem != Vars.wandName && Vars.wandName != "any")
                                                    hasWand = false;
                                            }
                                            if (Vars.portalList.Contains(playerClient.userID.ToString()))
                                            {
                                                hasPortal = true;
                                                if (heldItem != Vars.portalName && Vars.portalName != "any")
                                                    hasPortal = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex) { Vars.conLog.Error("SHOOTS: " + ex.ToString()); }


                    int count = 1;
                    if (Vars.infAmmoList.Contains(playerClient.userID.ToString()))
                    {
                        Items.setUses(found, Vars.infAmmoClipSize < 0 ? SDB._maxUses : Vars.infAmmoClipSize);
                    }
                    else
                    {
                        if (found.uses > SDB._maxUses)
                        {
                            Items.setUses(found, SDB._maxUses);
                        }
                        found.Consume(ref count);
                    }

                    if (hasWand)
                    {
                        Character playerChar;
                        if (playerClient != null)
                        {
                            Character.FindByUser(playerClient.userID, out playerChar);

                            if (playerChar != null)
                            {
                                Vector3 newVector = playerChar.eyesOrigin + (playerChar.eyesAngles.forward * Vars.wandList[playerClient.userID.ToString()]);
                                if (newVector.y > 5000)
                                    newVector.y = 5000;
                                int curIndex = 0;
                                if (!Checks.aboveGround(newVector))
                                {
                                    while (true)
                                    {
                                        if (curIndex >= 50)
                                            break;

                                        if (!Checks.aboveGround(newVector))
                                        {
                                            curIndex++;
                                            newVector.y += 5;
                                        }
                                        else
                                            break;
                                    }
                                }

                                RustServerManagement RSM = RustServerManagement.Get();
                                if (!Vars.frozenPlayers.ContainsKey(playerClient.userID))
                                {
                                    if (playerClient.netPlayer != null)
                                        RSM.TeleportPlayerToWorld(playerClient.netPlayer, newVector);
                                }
                                else
                                {
                                    Vars.frozenPlayers[playerClient.userID] = newVector;
                                }
                            }
                        }
                    }
                    else if (hasPortal)
                    {
                        Thread t = new Thread(() =>
                            {
                                Character playerChar;
                                if (playerClient != null)
                                {
                                    Character.FindByUser(playerClient.userID, out playerChar);

                                    if (playerChar != null)
                                    {
                                        Vector3 newVector;
                                        if (Checks.isPointingAtObject(playerChar, out newVector))
                                        {
                                            if (newVector.y > 5000)
                                                newVector.y = 5000;
                                            int curIndex = 0;
                                            if (!Checks.aboveGround(newVector))
                                            {
                                                while (true)
                                                {
                                                    if (curIndex >= 50)
                                                        break;

                                                    if (!Checks.aboveGround(newVector))
                                                    {
                                                        curIndex++;
                                                        newVector.y += 2.5f;
                                                    }
                                                    else
                                                        break;
                                                }
                                            }
                                            RustServerManagement RSM = RustServerManagement.Get();
                                            if (playerClient != null)
                                            {
                                                if (playerClient.netPlayer != null)
                                                    RSM.TeleportPlayerToWorld(playerClient.netPlayer, newVector);
                                            }
                                        }
                                    }
                                }
                            });
                        t.IsBackground = true;
                        t.Start();
                    }
                    else
                    {
                        found.itemRepresentation.ActionStream(1, uLink.RPCMode.AllExceptOwner, stream);
                        float bulletRange = SDB.GetBulletRange(rep);
                        for (int i = 0; i < SDB.numPellets; i++)
                        {
                            GameObject obj2;
                            NetEntityID yid;
                            IDRemoteBodyPart part;
                            bool flag;
                            bool flag2;
                            bool flag3;
                            BodyPart part2;
                            Vector3 vector;
                            Vector3 vector2;
                            Transform transform;
                            SDB.ReadHitInfo(stream, out obj2, out flag, out flag2, out part2, out part, out yid, out transform, out vector, out vector2, out flag3);
                            if (obj2 != null)
                            {
                                    SDB.ApplyDamage(obj2, transform, flag3, vector, part2, rep);
                            }
                        }
                        found.TryConditionLoss(0.5f, 0.02f);
                    }
                }
            }
        }

        public static void swingWeaponStart(uLink.BitStream stream, ItemRepresentation itemRep, ref uLink.NetworkMessageInfo info)
        {
            NetCull.VerifyRPC(ref info, false);
            itemRep.ActionStream(3, uLink.RPCMode.OthersExceptOwner, stream);

            uLink.NetworkPlayer netPlayer = info.networkView.owner;

            bool hasWand = false;
            bool hasPortal = false;
            PlayerClient playerClient = null;

            try
            {
                playerClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == netPlayer);

                if (playerClient != null)
                {
                    if (playerClient.controllable != null)
                    {
                        if (playerClient.controllable.GetComponent<Inventory>() != null)
                        {
                            if (playerClient.controllable.GetComponent<Inventory>().activeItem != null)
                            {
                                if (playerClient.controllable.GetComponent<Inventory>().activeItem.datablock != null)
                                {
                                    string heldItem = playerClient.controllable.GetComponent<Inventory>().activeItem.datablock.name;
                                    if (Vars.portalList.Contains(playerClient.userID.ToString()) && Vars.wandName == Vars.portalName)
                                    {
                                        Broadcast.broadcastTo(netPlayer, "Portal tool and wand tool cannot be the same item! Portal tool disabled.");
                                        Vars.portalList.Remove(playerClient.userID.ToString());
                                    }
                                    if (Vars.wandList.ContainsKey(playerClient.userID.ToString()))
                                    {
                                        hasWand = true;
                                        if (heldItem != Vars.wandName && Vars.wandName != "any")
                                            hasWand = false;
                                    }
                                    if (Vars.portalList.Contains(playerClient.userID.ToString()))
                                    {
                                        hasPortal = true;
                                        if (heldItem != Vars.portalName && Vars.portalName != "any")
                                            hasPortal = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Vars.conLog.Error("SWS: " + ex.ToString()); }

            if (hasWand)
            {
                Character playerChar;
                if (playerClient != null)
                {
                    Character.FindByUser(playerClient.userID, out playerChar);

                    if (playerChar != null)
                    {
                        Vector3 newVector = playerChar.eyesOrigin + (playerChar.eyesAngles.forward * Vars.wandList[playerClient.userID.ToString()]);
                        if (newVector.y > 5000)
                            newVector.y = 5000;
                        int curIndex = 0;
                        if (!Checks.aboveGround(newVector))
                        {
                            while (true)
                            {
                                if (curIndex >= 50)
                                    break;

                                if (!Checks.aboveGround(newVector))
                                {
                                    curIndex++;
                                    newVector.y += 5;
                                }
                                else
                                    break;
                            }
                        }

                        RustServerManagement RSM = RustServerManagement.Get();
                        if (!Vars.frozenPlayers.ContainsKey(playerClient.userID))
                        {
                            if (playerClient.netPlayer != null)
                                RSM.TeleportPlayerToWorld(playerClient.netPlayer, newVector);
                        }
                        else
                        {
                            Vars.frozenPlayers[playerClient.userID] = newVector;
                        }
                    }
                }
            }
            else if (hasPortal)
            {
                Thread t = new Thread(() =>
                    {
                        Character playerChar;
                        if (playerClient != null)
                        {
                            Character.FindByUser(playerClient.userID, out playerChar);

                            if (playerChar != null)
                            {
                                Vector3 newVector;
                                if (Checks.isPointingAtObject(playerChar, out newVector))
                                {
                                    if (newVector.y > 5000)
                                        newVector.y = 5000;
                                    int curIndex = 0;
                                    if (!Checks.aboveGround(newVector))
                                    {
                                        while (true)
                                        {
                                            if (curIndex >= 50)
                                                break;

                                            if (!Checks.aboveGround(newVector))
                                            {
                                                curIndex++;
                                                newVector.y += 2.5f;
                                            }
                                            else
                                                break;
                                        }
                                    }
                                    RustServerManagement RSM = RustServerManagement.Get();
                                    if (playerClient != null)
                                    {
                                        if (playerClient.netPlayer != null)
                                            RSM.TeleportPlayerToWorld(playerClient.netPlayer, newVector);
                                    }
                                }
                            }
                        }
                    });
                t.IsBackground = true;
                t.Start();
            }
        }

        public static void swingWeaponDone(uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info, MeleeWeaponDataBlock MWDB)
        {
            GameObject gameObject;
            NetEntityID unassigned;
            IMeleeWeaponItem item;
            NetCull.VerifyRPC(ref info, false);
            if (stream.ReadBoolean())
            {
                unassigned = stream.Read<NetEntityID>(new object[0]);
                if (!unassigned.isUnassigned)
                {
                    gameObject = unassigned.gameObject;
                    if (gameObject == null)
                    {
                        unassigned = NetEntityID.unassigned;
                    }
                }
                else
                {
                    gameObject = null;
                }
            }
            else
            {
                unassigned = NetEntityID.unassigned;
                gameObject = null;
            }
            Vector3 vector = stream.ReadVector3();
            bool flag2 = stream.ReadBoolean();
            if (rep.Item<IMeleeWeaponItem>(out item))
            {
                TakeDamage local = item.inventory.GetLocal<TakeDamage>();
                if (((local == null) || !local.dead) && item.ValidatePrimaryMessageTime(info.timestamp))
                {
                    IDBase victim = null;
                    TakeDamage damage2 = null;
                    if (gameObject != null)
                    {
                        victim = IDBase.Get(gameObject);
                        damage2 = (victim == null) ? null : victim.idMain.GetLocal<TakeDamage>();
                    }
                    if ((gameObject == null) || (Vector3.Distance(local.transform.position, gameObject.transform.position) < 6f))
                    {
                        rep.ActionStream(1, uLink.RPCMode.AllExceptOwner, stream);
                        ResourceTarget target = ((victim != null) || (gameObject != null)) ? ((victim != null) ? victim.gameObject : gameObject).GetComponent<ResourceTarget>() : null;
                        string heldItem = item.datablock.name;
                        float range = ((MeleeWeaponDataBlock)item.datablock).range;

                        bool hasWand = false;
                        bool hasPortal = false;
                        PlayerClient playerClient = null;

                        try
                        {
                            if (item != null && item.inventory != null && Vars.AllPlayerClients != null)
                            {
                                playerClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc != null && pc.controllable != null && pc.controllable.GetComponent<Inventory>() != null && pc.controllable.GetComponent<Inventory>() == item.inventory);

                                if (playerClient != null)
                                {
                                    if (Vars.wandList.ContainsKey(playerClient.userID.ToString()))
                                    {
                                        hasWand = true;
                                        if (heldItem != Vars.wandName && Vars.wandName != "any")
                                            hasWand = false;
                                    }
                                    if (Vars.portalList.Contains(playerClient.userID.ToString()))
                                    {
                                        hasPortal = true;
                                        if (heldItem != Vars.portalName && Vars.portalName != "any")
                                            hasPortal = false;
                                    }
                                }
                            }
                        }
                        catch (Exception ex) { Vars.conLog.Error("SWD: " + ex.ToString()); }

                        if (!hasWand && !hasPortal)
                        {
                            bool withinDistance = true;
                            bool isDead = false;
                            
                            Character playerChar = null;
                            if (playerClient != null)
                            {
                                if (Vars.AllCharacters.ContainsKey(playerClient))
                                {
                                    playerChar = Vars.AllCharacters[playerClient];

                                    if (playerChar != null && playerChar.alive && Vars.enableAntiAW && ((victim != null && victim.gameObject != null) || gameObject != null))
                                    {
                                        float distance = Vector3.Distance(playerChar.transform.position, (victim != null ? victim.gameObject.transform.position : (gameObject != null ? gameObject.transform.position : vector)));
                                        if ((distance > 6 && victim.idMain is StructureComponent) || (distance > 7 && !(victim.idMain is StructureComponent)))
                                            withinDistance = false;
                                    }
                                    else
                                        isDead = false;
                                }
                                else
                                {
                                    if (Vars.getPlayerChar(playerClient, out playerChar))
                                    {
                                        if (!Vars.AllCharacters.ContainsKey(playerClient))
                                            Vars.AllCharacters.Add(playerClient, playerChar);

                                        if (playerChar != null && playerChar.alive && Vars.enableAntiAW && ((victim != null && victim.gameObject != null) || gameObject != null))
                                        {
                                            float distance = Vector3.Distance(playerChar.transform.position, (victim != null ? victim.gameObject.transform.position : (gameObject != null ? gameObject.transform.position : vector)));
                                            if ((distance > 6 && victim.idMain is StructureComponent) || (distance > 7 && !(victim.idMain is StructureComponent)))
                                                withinDistance = false;
                                        }
                                        else
                                            isDead = false;
                                    }
                                }
                            }

                            if (withinDistance)
                            {
                                Metabolism component = item.inventory.GetComponent<Metabolism>();
                                if (component != null)
                                {
                                    component.SubtractCalories(UnityEngine.Random.Range((float)(MWDB.caloriesPerSwing * 0.8f), (float)(MWDB.caloriesPerSwing * 1.2f)));
                                }
                                if (flag2 || ((target != null) && ((damage2 == null) || damage2.dead)))
                                {
                                    ResourceTarget.ResourceTargetType staticTree;
                                    if (flag2)
                                    {
                                        staticTree = ResourceTarget.ResourceTargetType.StaticTree;
                                    }
                                    else
                                    {
                                        staticTree = target.type;
                                    }
                                    float efficiency = MWDB.efficiencies[(int)staticTree];
                                    if (flag2) // if a tree
                                    {
                                        MWDB.resourceGatherLevel += efficiency;
                                        if (MWDB.resourceGatherLevel >= 1f)
                                        {
                                            int num4;
                                            int amount = Mathf.FloorToInt(MWDB.resourceGatherLevel);
                                            string name = "Wood";
                                            ItemDataBlock byName = DatablockDictionary.GetByName(name);
                                            if (byName != null)
                                            {
                                                int num5 = item.inventory.AddItemAmount(byName, amount);
                                                num4 = amount - num5;
                                            }
                                            else
                                            {
                                                num4 = 0;
                                            }
                                            if (num4 > 0)
                                            {
                                                MWDB.resourceGatherLevel -= num4;
                                                Rust.Notice.Inventory(info.sender, num4.ToString() + " x " + name);
                                            }
                                        }
                                    }
                                    else if (target != null) // if a resource
                                    {
                                        target.DoGather(item.inventory, efficiency);
                                    }
                                }
                                if (victim != null)
                                {
                                    float damage = MWDB.GetDamage();
                                    if (Vars.elevatorList.Contains(playerClient.userID.ToString()) && gameObject.GetComponent<StructureComponent>() != null && gameObject.GetComponent<StructureComponent>().type == StructureComponent.StructureComponentType.Ceiling && playerChar != null)
                                    {
                                        bool isWood = gameObject.GetComponent<StructureComponent>()._materialType == StructureMaster.StructureMaterialType.Wood;
                                        StructureComponent comp = NetCull.InstantiateStatic(isWood ? ";struct_metal_ceiling" : ";struct_wood_ceiling", gameObject.transform.position, gameObject.transform.rotation).GetComponent<StructureComponent>();
                                        if (comp != null) // If successfully recreated, restore its StructureMaster instance and make it an elevator if it was one
                                        {
                                            StructureComponent SC = comp.GetComponent<StructureComponent>();
                                            SC._master.AddStructureComponent(comp);
                                            if (!Elevators.isElevator(SC))
                                                Elevators.addElevator(SC, playerChar);
                                            else
                                                Elevators.removeElevator(SC, playerChar);
                                        }

                                        //NetCull.Destroy(gameObject);
                                    }
                                    else if (Vars.oposList.Contains(playerClient.userID.ToString()))
                                    {
                                        Broadcast.broadcastTo(playerClient.netPlayer, gameObject.transform.position.ToString());
                                    }
                                    else
                                        TakeDamage.Hurt(item.inventory, victim, new DamageTypeList(0f, 0f, damage, 0f, 0f, 0f), new WeaponImpact(MWDB, item, rep));
                                }
                                if (gameObject != null)
                                {
                                    item.TryConditionLoss(0.25f, 0.025f);
                                }
                            }
                            else if (!isDead)
                            {
                                // Add notifications/alerts here
                            }
                        }
                    }
                }
            }
        }

        public static bool DoGather(Inventory reciever, float efficiency, ResourceTarget RT)
        {
            if (RT.resourcesAvailable.Count == 0)
            {
                return false;
            }
            ResourceGivePair item = RT.resourcesAvailable[UnityEngine.Random.Range(0, RT.resourcesAvailable.Count)];

            switch (reciever.activeItem.datablock.name)
            {
                case "Rock":
                    RT.gatherProgress += efficiency * RT.gatherEfficiencyMultiplier * Vars.rockMultiplier;
                    break;
                case "Stone Hatchet":
                    RT.gatherProgress += efficiency * RT.gatherEfficiencyMultiplier * Vars.sHatchetMultiplier;
                    break;
                case "Hatchet":
                    RT.gatherProgress += efficiency * RT.gatherEfficiencyMultiplier * Vars.hatchetMultiplier;
                    break;
                case "Pick Axe":
                    RT.gatherProgress += efficiency * RT.gatherEfficiencyMultiplier * Vars.pickaxeMultiplier;
                    break;
                default:
                    RT.gatherProgress += efficiency * RT.gatherEfficiencyMultiplier;
                    break;
            }
            int a = Mathf.Abs((int)RT.gatherProgress);

            RT.gatherProgress = Mathf.Clamp(RT.gatherProgress, 0f, a);
            a = Mathf.Min(a, item.AmountLeft());
            if (a > 0)
            {
                int num2 = reciever.AddItemAmount(item.ResourceItemDataBlock, a);
                if (num2 < a)
                {
                    int amount = a - num2;

                    item.Subtract(amount);
                    RT.gatherProgress -= amount;
                    Rust.Notice.Inventory(reciever.networkView.owner, amount.ToString() + " x " + item.ResourceItemName);
                    RT.SendMessage("ResourcesGathered", SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    Rust.Notice.Popup(reciever.networkView.owner, "", "Your inventory is full!", 3f);
                }
            }
            if (!item.AnyLeft())
            {
                RT.resourcesAvailable.Remove(item);
            }
            if (RT.resourcesAvailable.Count == 0)
            {
                RT.SendMessage("ResourcesDepletedMsg", SendMessageOptions.DontRequireReceiver);
            }
            return true;
        }

        public static InventoryItem.MergeResult researchItem(IInventoryItem otherItem, IResearchToolItem researchItem)
        {
            BlueprintDataBlock block2;
            PlayerInventory inventory = researchItem.inventory as PlayerInventory;
            if ((inventory == null) || (otherItem.inventory != inventory))
            {
                return InventoryItem.MergeResult.Failed;
            }
            PlayerClient playerClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == inventory.networkView.owner);
            ItemDataBlock datablock = otherItem.datablock;
            if ((datablock == null) || !datablock.isResearchable || Vars.restrictResearch.Contains(otherItem.datablock.name))
            {
                if (datablock == null)
                    return InventoryItem.MergeResult.Failed;
                if (!datablock.isResearchable || Vars.restrictResearch.Contains(otherItem.datablock.name))
                {
                    if (!Vars.permitResearch.Contains(otherItem.datablock.name) && !Vars.craftList.Contains(playerClient.userID.ToString()))
                    {
                        Rust.Notice.Popup(inventory.networkView.owner, "", "You cannot research this item!", 4f);
                        return InventoryItem.MergeResult.Failed;
                    }
                }
            }
            if (!inventory.AtWorkBench() && Vars.researchAtBench && !Vars.craftList.Contains(playerClient.userID.ToString()))
            {
                Rust.Notice.Popup(inventory.networkView.owner, "", "You must be at a workbench to research.", 4f);
                return InventoryItem.MergeResult.Failed;
            }
            if (!BlueprintDataBlock.FindBlueprintForItem<BlueprintDataBlock>(otherItem.datablock, out block2))
            {
                Rust.Notice.Popup(inventory.networkView.owner, "", "There is no crafting recipe for this.", 4f);
                return InventoryItem.MergeResult.Failed;
            }
            if (inventory.KnowsBP(block2))
            {
                Rust.Notice.Popup(inventory.networkView.owner, "", "You already researched this.", 4f);
                return InventoryItem.MergeResult.Failed;
            }
            Hook hook = Vars.callHook("RustEssentialsAPI.Hooks", "OnResearchItem", false, playerClient.userID.ToString(), otherItem, researchItem);
            if (Checks.ContinueHook(hook))
            {
                IInventoryItem paper;
                int numPaper = 1;
                if (Vars.researchPaper && !Vars.craftList.Contains(playerClient.userID.ToString()))
                {
                    if (Items.hasItem(playerClient, "Paper", out paper))
                    {
                        if (paper.Consume(ref numPaper))
                            paper.inventory.RemoveItem(paper.slot);
                    }
                    else
                    {
                        Rust.Notice.Popup(inventory.networkView.owner, "", "Researching requires paper.", 4f);
                        return InventoryItem.MergeResult.Failed;
                    }
                }
                inventory.BindBlueprint(block2);
                Rust.Notice.Popup(inventory.networkView.owner, "", "You can now craft: " + otherItem.datablock.name, 4f);
                int numWant = 1;
                if (!Vars.infiniteResearch && !Vars.craftList.Contains(playerClient.userID.ToString()))
                {
                    if (researchItem.Consume(ref numWant))
                        researchItem.inventory.RemoveItem(researchItem.slot);
                }
                return InventoryItem.MergeResult.Combined;
            }

            return InventoryItem.MergeResult.Failed;
        }

        public static void researchItemBP(IBlueprintItem item, BlueprintDataBlock BDB)
        {
            PlayerInventory inventory = item.inventory as PlayerInventory;
            if (inventory != null)
            {
                bool b = false;
                PlayerClient playerClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == inventory.networkView.owner);
                if (!Vars.restrictBlueprints.Contains(BDB.resultItem.name) || Vars.craftList.Contains(playerClient.userID.ToString()))
                {
                    if (!inventory.GetBoundBPs().Contains(BDB))
                    {
                        Hook hook = Vars.callHook("RustEssentialsAPI.Hooks", "OnResearchBlueprint", false, playerClient.userID.ToString(), item, BDB);
                        if (Checks.ContinueHook(hook))
                        {
                            inventory.BindBlueprint(BDB);
                            int count = 1;
                            if (item.Consume(ref count))
                            {
                                inventory.RemoveItem(item.slot);
                            }
                            Rust.Notice.Popup(inventory.networkView.owner, "", "You can now craft: " + BDB.resultItem.name, 4f);
                        }
                    }
                    else
                    {
                        Rust.Notice.Popup(inventory.networkView.owner, "", "You already researched this.", 4f);
                    }
                }
                else
                    Broadcast.noticeTo(inventory.networkView.owner, "", "You cannot research this item!", 4);
            }
        }

        public static bool canCraft(int amount, Inventory workbenchInv, BlueprintDataBlock BDB)
        {
            if (BDB.lastCanWorkResult == null)
            {
                BDB.lastCanWorkResult = new List<int>();
            }
            else
            {
                BDB.lastCanWorkResult.Clear();
            }
            if (BDB.lastCanWorkIngredientCount == null)
            {
                BDB.lastCanWorkIngredientCount = new List<int>(BDB.ingredients.Length);
            }
            else
            {
                BDB.lastCanWorkIngredientCount.Clear();
            }
            PlayerClient playerClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == workbenchInv.networkView.owner);
            if (playerClient != null)
            {
                if (!Vars.craftList.Contains(playerClient.userID.ToString()))
                {
                    if (BDB.RequireWorkbench && Vars.craftAtBench)
                    {
                        CraftingInventory component = workbenchInv.GetComponent<CraftingInventory>();
                        if ((component == null) || !component.AtWorkBench())
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                if (BDB.RequireWorkbench && Vars.craftAtBench)
                {
                    CraftingInventory component = workbenchInv.GetComponent<CraftingInventory>();
                    if ((component == null) || !component.AtWorkBench())
                    {
                        return false;
                    }
                }
            }
            if (Vars.restrictCrafting.Contains(BDB.resultItem.name))
            {
                Broadcast.noticeTo(workbenchInv.networkView.owner, "♨", "You cannot craft this item!", 4);
                return false;
            }
            if (playerClient != null)
            {
                if (!Vars.craftList.Contains(playerClient.userID.ToString()))
                {
                    foreach (BlueprintDataBlock.IngredientEntry entry in BDB.ingredients)
                    {
                        if (entry.amount != 0)
                        {
                            int item = workbenchInv.CanConsume(entry.Ingredient, entry.amount * amount, BDB.lastCanWorkResult);
                            if (item <= 0)
                            {
                                BDB.lastCanWorkResult.Clear();
                                BDB.lastCanWorkIngredientCount.Clear();
                                return false;
                            }
                            BDB.lastCanWorkIngredientCount.Add(item);
                        }
                    }
                }
            }
            else
            {
                foreach (BlueprintDataBlock.IngredientEntry entry in BDB.ingredients)
                {
                    if (entry.amount != 0)
                    {
                        int item = workbenchInv.CanConsume(entry.Ingredient, entry.amount * amount, BDB.lastCanWorkResult);
                        if (item <= 0)
                        {
                            BDB.lastCanWorkResult.Clear();
                            BDB.lastCanWorkIngredientCount.Clear();
                            return false;
                        }
                        BDB.lastCanWorkIngredientCount.Add(item);
                    }
                }
            }
            return true;
        }

        public static bool craftItem(int amount, Inventory workbenchInv, BlueprintDataBlock BDB)
        {
            if (!BDB.CanWork(amount, workbenchInv))
            {
                return false;
            }
            int num = 0;
            if (Vars.restrictCrafting.Contains(BDB.resultItem.name))
            {
                Broadcast.noticeTo(workbenchInv.networkView.owner, "♨", "You cannot craft this item!", 4);
                return false;
            }
            PlayerClient playerClient = Array.Find(Vars.AllPlayerClients.ToArray(), (PlayerClient pc) => pc.netPlayer == workbenchInv.networkView.owner);
            if (playerClient != null)
            {
                if (workbenchInv.GetComponent<PlayerInventory>() != null)
                {
                    if (!workbenchInv.GetComponent<PlayerInventory>().KnowsBP(BDB) && Vars.enableAntiBP && !Vars.bypassList.Contains(playerClient.userID.ToString()))
                    {
                        //Broadcast.noticeTo(workbenchInv.networkView.owner, "♨", "You have not researched this item!", 4, true);
                        RustEssentialsBootstrap._load.loadBans();
                        if (!Vars.currentBans.ContainsKey(playerClient.userID.ToString()))
                        {
                            Broadcast.broadcastTo(playerClient.netPlayer, "You were banned! Reason:");
                            Broadcast.broadcastTo(playerClient.netPlayer, "[AH] Crafted item without researching it.");
                            playerClient.netUser.Kick(NetError.NoError, false);
                            Broadcast.broadcastAll("Player " + playerClient.userName + " (" + playerClient.userID + ") was banned. Reason:");
                            Broadcast.broadcastAll("[AH] Crafted item without researching it.");
                            Vars.currentBans.Add(playerClient.userID.ToString(), playerClient.userName);
                            Vars.currentBanReasons.Add(playerClient.userID.ToString(), "[AH] Crafted item without researching it.");
                            Vars.saveBans();
                        }
                        return false;
                    }
                }
                if (!Vars.craftList.Contains(playerClient.userID.ToString()))
                {
                    Hook hook = Vars.callHook("RustEssentialsAPI.Hooks", "OnCraftItem", false, playerClient.userID.ToString(), BDB.resultItem, BDB.numResultItem, amount);
                    if (Checks.ContinueHook(hook))
                    {
                        for (int i = 0; i < BDB.ingredients.Length; i++)
                        {
                            int count = BDB.ingredients[i].amount * amount;
                            if (count != 0)
                            {
                                int num4 = BDB.lastCanWorkIngredientCount[i];
                                for (int j = 0; j < num4; j++)
                                {
                                    IInventoryItem item;
                                    int slot = BDB.lastCanWorkResult[num++];
                                    if (workbenchInv.GetItem(slot, out item) && item.Consume(ref count))
                                    {
                                        workbenchInv.RemoveItem(slot);
                                    }
                                }
                            }
                        }
                        workbenchInv.AddItemAmount(BDB.resultItem, amount * BDB.numResultItem);
                    }
                }
                else
                    workbenchInv.AddItemAmount(BDB.resultItem, amount * BDB.numResultItem);
            }
            return true;
        }

        private static float lastDecayThink = 0;
        private static float lastDecayTime2 = 0;
        public static EnvDecay.ThinkResult decayThink(EnvDecay ED)
        {
            try
            {
                if (ED != null)
                {
                    if (ED._takeDamage == null)
                    {
                        return EnvDecay.ThinkResult.Done;
                    }
                    if ((ED._deployable == null) || (ED._deployable.GetCarrier() == null))
                    {
                        Vector3 position = ED.transform.position;

                        if (Vars.enableDecay)
                        {
                            if (Vars.enableCustomDecay)
                            {
                                if (lastDecayThink == 0)
                                    lastDecayThink = UnityEngine.Time.time;
                                lastDecayTime2 += UnityEngine.Time.time - lastDecayThink;
                                lastDecayThink = UnityEngine.Time.time;

                                string fullName = Vars.getFullObjectName(ED.name);
                                double interval = Vars.decayObjectInterval;
                                if (Vars.decayIntervals.ContainsKey(fullName))
                                    interval = Vars.decayIntervals[fullName];

                                if (lastDecayTime2 < interval)
                                {
                                    return EnvDecay.ThinkResult.TooEarly;
                                }
                                if (ED.CanApplyDecayDamage())
                                {
                                    lastDecayTime2 = 0f;
                                    if (!Vars.envDecaysInZones.Contains(ED))
                                    {
                                        Zone zone = null;
                                        if (!Vars.checkIfInZone || (Vars.checkIfInZone && !Checks.inZone(position, out zone)))
                                        {
                                            float damageQuantity = ((Mathf.Clamp(UnityEngine.Time.time - ED.lastDecayThink, 0f, Convert.ToSingle(interval)) / (Convert.ToSingle(interval) * 144f)) * ED._takeDamage.maxHealth) * ED.decayMultiplier;

                                            if (TakeDamage.HurtSelf(ED, damageQuantity, null) == LifeStatus.WasKilled)
                                            {
                                                return EnvDecay.ThinkResult.Done;
                                            }
                                        }
                                        else if (Vars.checkIfInZone && Checks.inZone(position, out zone))
                                        {
                                            if (!Vars.envDecaysInZones.Contains(ED))
                                            {
                                                Vars.envDecaysInZones.Add(ED);
                                                if (!Vars.zoneEnvDecays.ContainsKey(zone))
                                                    Vars.zoneEnvDecays.Add(zone, new List<EnvDecay>());

                                                if (!Vars.zoneEnvDecays[zone].Contains(ED))
                                                    Vars.zoneEnvDecays[zone].Add(ED);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                float num = UnityEngine.Time.time - ED.lastDecayThink;
                                if (num < decay.decaytickrate)
                                {
                                    return EnvDecay.ThinkResult.TooEarly;
                                }
                                if (ED.CanApplyDecayDamage())
                                {
                                    Zone zone;
                                    if (!Vars.checkIfInZone || (Vars.checkIfInZone && !Checks.inZone(position, out zone)))
                                    {
                                        float damageQuantity = ((Mathf.Clamp(UnityEngine.Time.time - ED.lastDecayThink, 0f, decay.decaytickrate) / decay.deploy_maxhealth_sec) * ED._takeDamage.maxHealth) * ED.decayMultiplier;
                                        if (TakeDamage.HurtSelf(ED, damageQuantity, null) == LifeStatus.WasKilled)
                                        {
                                            return EnvDecay.ThinkResult.Done;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("DECAYT: " + ex.ToString());
            }
            return EnvDecay.ThinkResult.AgainLater;
        }

        public static void resourceUseItem(IResourceTypeItem rs, ResourceTypeItemDataBlock RTIDB)
        {
            RaycastHit hit;
            bool flag;
            Facepunch.MeshBatch.MeshBatchInstance instance;
            if (MeshBatchPhysics.SphereCast(rs.character.eyesRay, 0.5f, out hit, 4f, out flag, out instance))
            {
                IDMain idMain;
                if (flag)
                {
                    idMain = instance.idMain;
                }
                else
                {
                    idMain = IDBase.GetMain(hit.collider.gameObject);
                }
                if (idMain != null)
                {
                    RepairReceiver local = idMain.GetLocal<RepairReceiver>();
                    TakeDamage damage = idMain.GetLocal<TakeDamage>();
                    if ((((local != null) && (damage != null)) && (local.GetRepairAmmo() == RTIDB)) && (damage.health != damage.maxHealth))
                    {
                        if (Vars.enableRepair)
                        {
                            if (damage.TimeSinceHurt() < 5f)
                            {
                                int timeToWait = 5 - (int)Math.Round(damage.TimeSinceHurt());
                                Rust.Notice.Popup(rs.character.netUser.networkPlayer, "⌛", "You must wait " + timeToWait + " seconds.");
                            }
                            else
                            {
                                float amount = damage.maxHealth / ((float)local.ResForMaxHealth);
                                if (amount > (damage.maxHealth - damage.health))
                                {
                                    amount = damage.maxHealth - damage.health;
                                }
                                damage.Heal(rs.character.idMain, amount);
                                rs.lastUseTime = UnityEngine.Time.time;
                                int count = 1;
                                if (rs.Consume(ref count))
                                {
                                    rs.inventory.RemoveItem(rs.slot);
                                }
                                string strText = string.Format("[✚] +{0} ({1}/{2})", (int)amount, (int)damage.health, (int)damage.maxHealth);
                                Rust.Notice.Inventory(rs.inventory.networkViewOwner, strText);
                            }
                        }
                        else
                            Broadcast.broadcastTo(rs.inventory.networkViewOwner, "[✚] Repairing is disabled on this server.");
                    }
                }
            }
        }

        public static float lastDecayTime = 0;
        public static float timeSinceDecay = 0;
        public static StructureMaster.DecayStatus doDecay(StructureMaster SM)
        {
            try
            {
                if (Vars.enableDecay)
                {
                    if (Vars.enableCustomDecay)
                    {
                        // Get the amount of time since we last decayed
                        float secondsSinceDecay = UnityEngine.Time.time - SM._lastDecayTime;
                        SM._lastDecayTime = UnityEngine.Time.time;
                        //if (SM._decayRate <= 0f)
                        //{
                        //    return StructureMaster.DecayStatus.Delaying;
                        //}
                        SM._decayDelayRemaining -= secondsSinceDecay;
                        secondsSinceDecay = -SM._decayDelayRemaining;
                        if (SM._decayDelayRemaining < 0f)
                        {
                            SM._decayDelayRemaining = 0f;
                        }
                        if (secondsSinceDecay <= 0f) // If the decay interval has not elapsed yet
                        {
                            return StructureMaster.DecayStatus.Delaying;
                        }
                        float num3 = 0.1f;
                        if (lastDecayTime == 0)
                            lastDecayTime = UnityEngine.Time.time;
                        timeSinceDecay += UnityEngine.Time.time - lastDecayTime;
                        lastDecayTime = UnityEngine.Time.time;

                        if (timeSinceDecay < 1)
                            return StructureMaster.DecayStatus.PentUpDecay;

                        timeSinceDecay = 0;


                        //SM._pentUpDecayTime += secondsSinceDecay;
                        //float decayTimeMaxHealth = SM.GetDecayTimeMaxHealth();
                        //float num3 = SM._pentUpDecayTime / decayTimeMaxHealth;
                        //if (num3 < structure.minpercentdmg)
                        //{
                        //    Vars.conLog.Info("Failed: " + num3 + " [" + SM._pentUpDecayTime + "/m" + decayTimeMaxHealth + "]");
                        //    return StructureMaster.DecayStatus.PentUpDecay;
                        //}
                        //SM._pentUpDecayTime = 0f;
                        HashSet<StructureComponent> componentsToDecay = grabComponentsToDecay(SM, SM._structureComponents);
                        foreach (StructureComponent component in componentsToDecay)
                        {
                            try
                            {
                                if (!waitingForNextDecay.ContainsKey(component))
                                {
                                    string fullName = Vars.getFullStructureName(component.name);
                                    long interval = Vars.decayStructureInterval;
                                    if (Vars.decayIntervals.ContainsKey(fullName))
                                        interval = Vars.decayIntervals[fullName];

                                    TimerPlus t = new TimerPlus();
                                    t.AutoReset = false;
                                    t.Interval = interval;
                                    t.timerCallback = new TimerCallback((senderObject) => reactivateDecay(component));
                                    t.Start();
                                    waitingForNextDecay.Add(component, t);
                                    Vector3 position = component.transform.position;
                                    if (!Vars.structuresInZones.Contains(component)) // If the structure is not known to be in a zone
                                    {
                                        Zone zone = null;
                                        if (!Vars.checkIfInZone || (Vars.checkIfInZone && !Checks.inZone(position, out zone)))
                                        {
                                            TakeDamage damage = component.GetComponent<TakeDamage>();
                                            if (damage != null)
                                            {
                                                float damageQuantity = ((damage.maxHealth * num3) * UnityEngine.Random.Range(0.75f, 1.25f)) /* * SM._decayRate */;
                                                if (((component.type == StructureComponent.StructureComponentType.Wall) || (component.type == StructureComponent.StructureComponentType.Doorway)) || (component.type == StructureComponent.StructureComponentType.WindowWall))
                                                {
                                                    RaycastHit hit;
                                                    bool flag4;
                                                    Facepunch.MeshBatch.MeshBatchInstance instance;
                                                    Ray ray = new Ray(component.transform.position + new Vector3(0f, 2.5f, 0f), component.transform.forward);
                                                    Ray ray2 = new Ray(component.transform.position + new Vector3(0f, 2.5f, 0f), -component.transform.forward);
                                                    bool flag3 = false;
                                                    if (Facepunch.MeshBatch.MeshBatchPhysics.Raycast(ray, out hit, 25f, out flag4, out instance))
                                                    {
                                                        RaycastHit hit2;
                                                        IDMain main = !flag4 ? IDBase.GetMain(hit.collider.gameObject) : instance.idMain;
                                                        if (((main != null) && ((main is StructureComponent) || main.CompareTag("Door"))) && Facepunch.MeshBatch.MeshBatchPhysics.Raycast(ray2, out hit2, 25f, out flag4, out instance))
                                                        {
                                                            main = !flag4 ? IDBase.GetMain(hit2.collider.gameObject) : instance.idMain;
                                                            if ((main != null) && ((main is StructureComponent) || main.CompareTag("Door")))
                                                            {
                                                                flag3 = true;
                                                            }
                                                        }
                                                    }
                                                    if (flag3)
                                                    {
                                                        damageQuantity *= 0.2f;
                                                    }
                                                    TakeDamage.HurtSelf(component, damageQuantity, null);
                                                }
                                                else if (component.type == StructureComponent.StructureComponentType.Pillar)
                                                {
                                                    if (!SM.ComponentCarryingWeight(component))
                                                    {
                                                        TakeDamage.HurtSelf(component, damageQuantity, null);
                                                    }
                                                }
                                                else if (component.type == StructureComponent.StructureComponentType.Ceiling)
                                                {
                                                    if (!SM.ComponentCarryingWeight(component))
                                                    {
                                                        TakeDamage.HurtSelf(component, damageQuantity, null);
                                                    }
                                                }
                                                else if (component.type == StructureComponent.StructureComponentType.Foundation)
                                                {
                                                    if (!SM.ComponentCarryingWeight(component))
                                                    {
                                                        TakeDamage.HurtSelf(component, damageQuantity, null);
                                                    }
                                                }
                                                else
                                                {
                                                    TakeDamage.HurtSelf(component, damageQuantity, null);
                                                }
                                            }
                                        }
                                        else if (Vars.checkIfInZone && Checks.inZone(position, out zone)) // If the structure was actually in a zone, register it
                                        {
                                            Vars.structuresInZones.Add(component);

                                            if (!Vars.zoneStructures.ContainsKey(zone))
                                                Vars.zoneStructures.Add(zone, new List<StructureComponent>());

                                            if (!Vars.zoneStructures[zone].Contains(component))
                                                Vars.zoneStructures[zone].Add(component);
                                        }
                                    }
                                }
                                else
                                {
                                    reactivateDecay(component);
                                }
                            }
                            catch (Exception ex)
                            {
                                Vars.conLog.Error("DODEC #2: " + ex.ToString());
                            }
                        }
                    }
                    else
                    {
                        SM.DoOldDecay();
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("DODEC #1: " + ex.ToString());
            }
            return StructureMaster.DecayStatus.Decaying;
        }

        private static void reactivateDecay(StructureComponent component)
        {
            waitingForNextDecay[component].Stop();
            waitingForNextDecay.Remove(component);
        }

        public static Dictionary<StructureComponent, TimerPlus> waitingForNextDecay = new Dictionary<StructureComponent, TimerPlus>();
        public static HashSet<StructureComponent>grabComponentsToDecay(StructureMaster SM, HashSet<StructureComponent> Components)
        {
            HashSet<StructureComponent> newComponents = new HashSet<StructureComponent>();
            Dictionary<StructureComponent, TimerPlus> waitingList = waitingForNextDecay;
            foreach (StructureComponent component in Components)
            {
                if (!waitingList.ContainsKey(component))
                    newComponents.Add(component);
                else
                {
                    double timeLeft = waitingList[component].TimeLeft;
                    if (timeLeft <= 0)
                    {
                        waitingForNextDecay[component].Stop();
                        waitingList[component].Stop();
                        waitingForNextDecay.Remove(component);
                        newComponents.Add(component);
                    }
                }
            }
            return newComponents;
        }

        public static void basicHearFootstep(Vector3 origin, BasicWildLifeAI BWAI)
        {
            try
            {
                bool b = false;
                try
                {
                    List<PlayerClient> playerClients = new List<PlayerClient>();
                    foreach (PlayerClient pc in Vars.AllPlayerClients) { playerClients.Add(pc); }

                    foreach (PlayerClient pc in playerClients)
                    {
                        if (pc != null)
                        {
                            Character outChar;
                            Character.FindByUser(pc.userID, out outChar);

                            if (outChar != null && outChar.transform != null && outChar.transform.position != null && origin != null)
                            {
                                if (Vector3.Distance(outChar.transform.position, origin) < 1)
                                {
                                    b = Vars.hiddenList.Contains(pc.userID.ToString());
                                }
                            }
                        }
                    }
                }
                catch (Exception ex2)
                {
                    Vars.conLog.Error("BHFS #2: " + ex2.ToString());
                }
                if (((BWAI._state != 2) && (BWAI._state != 7)) && BWAI.afraidOfFootsteps && !b)
                {
                    BWAI.ExitCurrentState();
                    BWAI.EnterState_Flee(origin);
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("BHFS #1: " + ex.ToString());
            }
        }

        public static void basicHurt(DamageEvent damage, BasicWildLifeAI BWAI)
        {
            try
            {
                bool b = false;
                try
                {
                    if (damage.attacker.idMain is Character)
                    {
                        if (Checks.isPlayer(damage.attacker.idMain))
                        {
                            b = Vars.hiddenList.Contains(damage.attacker.client.userID.ToString());
                        }
                    }
                }
                catch (Exception ex) { Vars.conLog.Error("BH #2: " + ex.ToString()); }

                if (((BWAI._state != 2) && (BWAI._state != 7)) && (damage.attacker.character != null) & !b)
                {
                    BWAI.ExitCurrentState();
                    BWAI.EnterState_Flee(BWAI.transform.position + new Vector3(UnityEngine.Random.Range((float)-1f, (float)1f), 0f, UnityEngine.Random.Range((float)-1f, (float)1f)));
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("BH: " + ex.ToString());
            }
        }

        public static List<HostileWildlifeAI> hadTarget = new List<HostileWildlifeAI>();
        public static void hostileChase(ulong millis, HostileWildlifeAI HWAI)
        {
            try
            {
                if (HWAI != null)
                {
                    try
                    {
                        if (HWAI._targetTD == null)
                        {
                            if (Vars.ignoringAIList.Contains(HWAI))
                            {
                                try
                                {
                                    HWAI.GoScentBlind(10f);
                                    HWAI.ExitCurrentState();
                                    HWAI.idleClock.ResetRandomDurationSeconds(0, 0);
                                    HWAI._state = 1;
                                    if (!HWAI.idleSoundRefireClock.once)
                                    {
                                        HWAI.idleSoundRefireClock.ResetRandomDurationSeconds(2.0, 4.0);
                                    }
                                    Vars.ignoringAIList.Remove(HWAI);
                                }
                                catch (Exception ex)
                                {
                                    Vars.conLog.Error("HC #2: " + ex.ToString());
                                }
                            }
                            else
                            {
                                try
                                {
                                    HWAI.LoseTarget();
                                }
                                catch (Exception ex)
                                {
                                    Vars.conLog.Error("HC #3: " + ex.ToString());
                                }
                            }

                            if (hadTarget.Contains(HWAI))
                                hadTarget.Remove(HWAI);
                        }
                        else
                        {
                            if (HWAI._targetTD.transform != null)
                            {
                                if (HWAI._targetTD.transform.position != null)
                                {
                                    if (HWAI.transform != null)
                                    {
                                        if (HWAI.transform.position != null)
                                        {
                                            if (HWAI._wildMove != null)
                                            {
                                                bool b = false;
                                                if (!hadTarget.Contains(HWAI))
                                                {
                                                    try
                                                    {
                                                        List<PlayerClient> playerClients = new List<PlayerClient>();
                                                        foreach (PlayerClient pc in Vars.AllPlayerClients) { playerClients.Add(pc); }

                                                        foreach (PlayerClient pc in playerClients)
                                                        {
                                                            if (pc != null)
                                                            {
                                                                if (pc.controllable != null)
                                                                {
                                                                    if (pc.controllable.GetComponent<TakeDamage>() != null)
                                                                    {
                                                                        if (pc.controllable.GetComponent<TakeDamage>() == HWAI._targetTD)
                                                                        {
                                                                            PlayerClient playerClient = pc;
                                                                            b = Vars.hiddenList.Contains(playerClient.userID.ToString()) && HWAI._targetTD == playerClient.controllable.GetComponent<TakeDamage>();

                                                                            try
                                                                            {
                                                                                if (!b)
                                                                                {
                                                                                    if (Checks.isPlayer(HWAI._targetTD.idMain))
                                                                                    {
                                                                                        Character character = HWAI._targetTD.idMain as Character;
                                                                                        foreach (string UID in Vars.hiddenList)
                                                                                        {
                                                                                            if (UID == character.playerClient.userID.ToString() && HWAI._targetTD == character.playerClient.controllable.GetComponent<TakeDamage>())
                                                                                                b = true;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            catch (Exception ex)
                                                                            {
                                                                                Vars.conLog.Error("HC #4: " + ex.ToString());
                                                                            }
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Vars.conLog.Error("HC #5: " + ex.ToString());
                                                    }
                                                    hadTarget.Add(HWAI);
                                                }

                                                if (!b)
                                                {
                                                    float num = HWAI.TargetRange();
                                                    if (num > HWAI.loseTargetRange)
                                                    {
                                                        try
                                                        {
                                                            HWAI.LoseTarget();
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Vars.conLog.Error("HC #6: " + ex.ToString());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Vector3 vector = HWAI._targetTD.transform.position - HWAI.transform.position;
                                                        vector.y = 0f;
                                                        if (num <= HWAI.attackRange)
                                                        {
                                                            try
                                                            {
                                                                HWAI._wildMove.SetLookDirection(vector.normalized);
                                                                HWAI.ExitCurrentState();
                                                                HWAI.EnterState_Attack();
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                Vars.conLog.Error("HC #7: " + ex.ToString());
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (HWAI._wildMove.IsStuck())
                                                            {
                                                                try
                                                                {
                                                                    if (HWAI.wasStuck)
                                                                    {
                                                                        if (HWAI.stuckClock.IntegrateTime_Reached(millis))
                                                                        {
                                                                            Vector3 position = HWAI._targetTD.transform.position;
                                                                            HWAI.LoseTarget();
                                                                            HWAI.ExitCurrentState();
                                                                            HWAI.EnterState_Flee(position, 0x2710L);
                                                                            return;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        HWAI.wasStuck = true;
                                                                        HWAI.stuckClock.ResetRandomDurationSeconds(1.0, 2.0);
                                                                    }
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    Vars.conLog.Error("HC #8: " + ex.ToString());
                                                                }
                                                            }
                                                            else
                                                            {
                                                                HWAI.wasStuck = false;
                                                            }
                                                            try
                                                            {
                                                                if (HWAI.chaseSoundClock.IntegrateTime_Reached(millis))
                                                                {
                                                                    BasicWildLifeAI.AISound chase = BasicWildLifeAI.AISound.Chase;
                                                                    if (num < 5f)
                                                                    {
                                                                        chase = BasicWildLifeAI.AISound.ChaseClose;
                                                                    }
                                                                    HWAI.NetworkSound(chase);
                                                                    HWAI.chaseSoundClock.ResetRandomDurationSeconds(1.5, 2.5);
                                                                }
                                                                HWAI._wildMove.SetMoveTarget(HWAI._targetTD.gameObject, HWAI.runSpeed);
                                                                if (HWAI.targetReachClock.IntegrateTime_Reached(millis))
                                                                {
                                                                    HWAI.LoseTarget();
                                                                    HWAI.ExitCurrentState();
                                                                    HWAI.EnterState_Idle();
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                Vars.conLog.Error("HC #9: " + ex.ToString());
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("HC #10: " + ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("HC #1: " + ex.ToString());
            }
        }

        public static void hostileHurt(DamageEvent damage, HostileWildlifeAI HWAI)
        {
            try
            {
                bool b = false;
                try
                {
                    if (Checks.isPlayer(damage.attacker.idMain))
                    {
                        b = Vars.hiddenList.Contains(damage.attacker.client.userID.ToString()) && HWAI._targetTD == damage.attacker.client.controllable.GetComponent<TakeDamage>();
                    }
                }
                catch (Exception ex)
                {
                    Vars.conLog.Error("HH #2: " + ex.ToString());
                }

                if (!HWAI.HasTarget() && (damage.attacker.character != null) && !b)
                {
                    HWAI.SetAttackTarget(damage.attacker.character.gameObject.GetComponent<TakeDamage>());
                    HWAI.ExitCurrentState();
                    HWAI.EnterState_Chase();
                }

                if (b && HWAI.HasTarget())
                    HWAI._targetTD = null;
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("HH #1: " + ex.ToString());
            }
        }

        public static void hostileScent(TakeDamage damage, HostileWildlifeAI HWAI)
        {
            try
            {
                if (HWAI != null)
                {
                    if (damage != null)
                    {
                        bool b = false;
                        try
                        {
                            List<PlayerClient> playerClients = new List<PlayerClient>();
                            foreach (PlayerClient pc in Vars.AllPlayerClients) { playerClients.Add(pc); }

                            foreach (PlayerClient pc in playerClients)
                            {
                                if (pc != null)
                                {
                                    if (pc.controllable != null)
                                    {
                                        if (pc.controllable.GetComponent<TakeDamage>() != null)
                                        {
                                            if (pc.controllable.GetComponent<TakeDamage>() == damage)
                                            {
                                                PlayerClient playerClient = pc;
                                                if (HWAI._targetTD != null)
                                                {
                                                    b = Vars.hiddenList.Contains(playerClient.userID.ToString()) && HWAI._targetTD == playerClient.controllable.GetComponent<TakeDamage>();

                                                    try
                                                    {
                                                        if (!b)
                                                        {
                                                            if (Checks.isPlayer(damage.idMain))
                                                            {
                                                                Character character = damage.idMain as Character;
                                                                foreach (string UID in Vars.hiddenList)
                                                                {
                                                                    if (UID == character.playerClient.userID.ToString() && HWAI._targetTD == character.playerClient.controllable.GetComponent<TakeDamage>())
                                                                        b = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Vars.conLog.Error("HS #3: " + ex.ToString());
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Vars.conLog.Error("HS #2: " + ex.ToString());
                        }

                        if (!HWAI.IsScentBlind() && (((HWAI._state != 2) && (HWAI._state != 7)) && !HWAI.HasTarget()) && !b)
                        {
                            HWAI.ExitCurrentState();
                            HWAI.SetAttackTarget(damage);
                            HWAI.EnterState_Chase();
                        }

                        if (b && HWAI.HasTarget())
                        {
                            Vars.ignoringAIList.Add(HWAI);
                            HWAI._targetTD = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("HS #1: " + ex.ToString());
            }
        }

        public static bool createSleeper(Character character)
        {
            Vector3 vector;
            Vector3 vector2;
            if (sleepers.on)
            {
                NetUser netUser = character.netUser;
                if (netUser != null)
                {
                    CharacterSleepingAvatarTrait trait = character.GetTrait<CharacterSleepingAvatarTrait>();
                    if (trait.valid)
                    {
                        if (!SleepingAvatar.IsOpen(netUser))
                        {
                            TransformHelpers.GetGroundInfoNoTransform(character.origin, out vector, out vector2);
                            Quaternion groundInfoRotation = TransformHelpers.GetGroundInfoRotation(character.rotation, vector2);
                            if (!Vars.excludeFromSleepers.Contains(Vars.findRank(netUser.userID.ToString())) || (Vars.excludeFromSleepers.Contains(Vars.findRank(netUser.userID.ToString())) && !Vars.enableLimitedSleepers))
                            {
                                GameObject sleeperObject = NetCull.InstantiateStatic(trait.prefab, trait.SolvePlacement(vector, groundInfoRotation, sleepers.pointsolver), groundInfoRotation);
                                if (sleeperObject != null)
                                {
                                    if (Vars.enableLimitedSleepers)
                                    {
                                        TimerPlus tp = new TimerPlus();
                                        tp.Interval = Vars.sleeperElapseInterval;
                                        tp.AutoReset = false;
                                        tp.timerCallback = new TimerCallback((senderObj) => Vars.sleeperTimerElapsed(netUser, netUser.userID));
                                        tp.Start();
                                    }
                                    SleepingAvatar component = sleeperObject.GetComponent<SleepingAvatar>();
                                    component.SetupCharacter(character, netUser, trait);
                                    return component != null;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static float GetDecayDelayForType(StructureMaster.StructureMaterialType type)
        {
            switch (type)
            {
                case StructureMaster.StructureMaterialType.Wood:
                    return Vars.decayWoodDelayInterval;

                case StructureMaster.StructureMaterialType.Metal:
                    return Vars.decayMetalDelayInterval;

                case StructureMaster.StructureMaterialType.Brick:
                    return 259200f;

                case StructureMaster.StructureMaterialType.Concrete:
                    return 432000f;
            }
            return 0f;
        }

        public static Character spawnPlayer(PlayerClient playerFor, bool useCamp, RustProto.Avatar avatar = null)
        {
            NetUser user;
            Vector3 zero = Vector3.zero;
            Quaternion identity = Quaternion.identity;
            RustServerManagement RSM = RustServerManagement.Get();
            ServerManagement SM = ServerManagement.Get();
            if (((avatar != null) && avatar.HasPos) && avatar.HasAng)
            {
                zero = new Vector3(avatar.Pos.X, avatar.Pos.Y, avatar.Pos.Z);
                identity = new Quaternion(avatar.Ang.X, avatar.Ang.Y, avatar.Ang.Z, avatar.Ang.W);
                if ((float.IsNaN(zero.x) || float.IsNaN(zero.y)) || float.IsNaN(zero.y))
                {
                    Debug.LogWarning("SpawnPlayer: position was NAN!");
                    Debug.LogWarning("Was spawning from avatar position!!");
                    SpawnManager.GetRandomSpawn(out zero, out identity);
                }
            }
            else if (useCamp)
            {
                RSM.GetCampSpawnForPlayer(playerFor, out zero, out identity);
                if ((float.IsNaN(zero.x) || float.IsNaN(zero.y)) || float.IsNaN(zero.y))
                {
                    Debug.LogWarning("SpawnPlayer: position was NAN!!!");
                    Debug.LogWarning("Was spawning from camp!!!!!");
                    SpawnManager.GetRandomSpawn(out zero, out identity);
                }
            }
            else
            {
                SpawnManager.GetRandomSpawn(out zero, out identity);
                if ((float.IsNaN(zero.x) || float.IsNaN(zero.y)) || float.IsNaN(zero.y))
                {
                    Debug.LogWarning("SpawnPlayer: position was NAN!!!");
                    Debug.LogWarning("Was spawning from RANDOM SPAWN!!!!!");
                    zero = Vector3.zero;
                }
            }
            if (!NetUser.Find(playerFor, out user))
            {
                Debug.LogWarning("No NetUser for client", playerFor);
            }
            user.truthDetector.NoteTeleported(zero, 0.0);
            Character forCharacter = Character.SummonCharacter(user.networkPlayer, SM.defaultPlayerControllableKey, zero, identity);
            if (forCharacter != null)
            {
                if (Vars.ghostList.ContainsKey(user.userID.ToString()))
                {
                    Vars.ghostList.Remove(user.userID.ToString());
                    Vars.ghostPositions.Remove(user.userID.ToString());
                }

                RSM.LoadAvatar(forCharacter);
                playerFor.lastKnownPosition = forCharacter.eyesOrigin;
                playerFor.hasLastKnownPosition = true;
                if (!Vars.AllCharacters.ContainsKey(user.playerClient))
                    Vars.AllCharacters.Add(user.playerClient, forCharacter);
                else
                    Vars.AllCharacters[user.playerClient] = forCharacter;

                if (Vars.checkMode == 1)
                    Vars.REB.StartCoroutine(Antihack.individualMovementCheck(forCharacter));
            }
            return forCharacter;
        }

        public static void setupCharacter(Character character, NetUser user, CharacterSleepingAvatarTrait trait, SleepingAvatar sleeper)
        {
            sleeper.timeStamp = POSIX.Time.NowStamp;
            if (user != null)
            {
                sleeper.creatorID = user.user.Userid;
                sleeper.ownerID = user.user.Userid;
                sleeper.CacheCreator();
            }
            sleeper.CreatorSet();
            PlayerInventory component = character.GetComponent<PlayerInventory>();
            sleeper.footArmor = null;
            sleeper.legArmor = null;
            sleeper.torsoArmor = null;
            sleeper.headArmor = null;
            List<IInventoryItem> armorItems = new List<IInventoryItem>();
            Items.grabArmor(character.playerClient, out armorItems);

            foreach (IInventoryItem item in armorItems)
            {
                if (item is IArmorItem)
                {
                    ArmorDataBlock datablock = item.datablock as ArmorDataBlock;
                    switch (item.slot)
                    {
                        case 39:
                            sleeper.footArmor = datablock;
                            break;
                        case 38:
                            sleeper.legArmor = datablock;
                            break;
                        case 37:
                            sleeper.torsoArmor = datablock;
                            break;
                        case 36:
                            sleeper.headArmor = datablock;
                            break;
                    }
                }
            }
            updateBufferedArmor(sleeper); // Sends armor update to all clients
            try
            {
                if (sleeper.takeDamage != null)
                {
                    TakeDamage takeDamage = character.takeDamage;
                    if (takeDamage != null)
                    {
                        takeDamage.CopyStateTo(sleeper.takeDamage);
                    }
                }
                sleeper.ngcView.RPC<NetEntityID>("SACH", uLink.RPCMode.Others, NetEntityID.Get(character));
                if (trait.grabsCarrierOnCreate)
                {
                    sleeper.GrabCarrier();
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("SETUPC: " + ex.ToString());
            }
        }

        public static void updateBufferedArmor(SleepingAvatar sleeper)
        {
            NetEntityID entID = NetEntityID.Get(sleeper);
            bool bufferedArmor = sleeper.bufferedArmor;
            if (sleeper.bufferedArmor)
            {
                NetCull.RemoveRPCsByName(entID, "SAAM");
                sleeper.bufferedArmor = false;
            }
            int num = (sleeper.footArmor == null) ? 0 : sleeper.footArmor.uniqueID;
            int num2 = (sleeper.legArmor == null) ? 0 : sleeper.legArmor.uniqueID;
            int num3 = (sleeper.torsoArmor == null) ? 0 : sleeper.torsoArmor.uniqueID;
            int num4 = (sleeper.headArmor == null) ? 0 : sleeper.headArmor.uniqueID;
            try
            {
                if (bufferedArmor || (1 != 0))
                {
                    NetCull.RPC<int, int, int, int>(entID, "SAAM", uLink.RPCMode.OthersBuffered, num, num2, num3, num4);
                    sleeper.bufferedArmor = true;
                }
                if (sleeper.takeDamage is ProtectionTakeDamage)
                {
                    ProtectionTakeDamage takeDamage = (ProtectionTakeDamage)sleeper.takeDamage;
                    DamageTypeList damageList = new DamageTypeList();
                    if (num != 0)
                    {
                        sleeper.footArmor.AddToDamageTypeList(damageList);
                    }
                    if (num2 != 0)
                    {
                        sleeper.legArmor.AddToDamageTypeList(damageList);
                    }
                    if (num3 != 0)
                    {
                        sleeper.torsoArmor.AddToDamageTypeList(damageList);
                    }
                    if (num4 != 0)
                    {
                        sleeper.headArmor.AddToDamageTypeList(damageList);
                    }
                    takeDamage.SetArmorValues(damageList);
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("UBA: " + ex.ToString());
            }
        }

        public static void skyUpdate(EnvironmentControlCenter ECC)
        {
            if (ECC.sky == null)
            {
                ECC.sky = (TOD_Sky)UnityEngine.Object.FindObjectOfType(typeof(TOD_Sky));
                if (ECC.sky == null)
                {
                    return;
                }
            }
            float num = env.daylength * 60f;
            if (ECC.sky.IsNight)
            {
                num = env.nightlength * 60f;
            }
            float num2 = num / 24f;
            float num3 = UnityEngine.Time.deltaTime / num2;
            float num4 = (UnityEngine.Time.deltaTime / (30f * num)) * 2f;
            ECC.sky.Cycle.Hour += num3;

            if (Vars.constantFullMoon)
                ECC.sky.Cycle.MoonPhase = 0;
            else
                ECC.sky.Cycle.MoonPhase += num4;

            if (ECC.sky.Cycle.MoonPhase < -1f)
            {
                ECC.sky.Cycle.MoonPhase += 2f;
            }
            else if (ECC.sky.Cycle.MoonPhase > 1f)
            {
                ECC.sky.Cycle.MoonPhase -= 2f;
            }
            if (ECC.sky.Cycle.Hour >= 24f)
            {
                ECC.sky.Cycle.Hour = 0f;
                int num5 = DateTime.DaysInMonth(ECC.sky.Cycle.Year, ECC.sky.Cycle.Month);
                if (++ECC.sky.Cycle.Day > num5)
                {
                    ECC.sky.Cycle.Day = 1;
                    if (++ECC.sky.Cycle.Month > 12)
                    {
                        ECC.sky.Cycle.Month = 1;
                        ECC.sky.Cycle.Year++;
                    }
                }
            }
        }

        public static bool dropInventoryContents(Inventory inventory, out Inventory droppedTo)
        {
            bool flag;
            droppedTo = null;
            if ((inventory == null) || !inventory.anyOccupiedSlots || Vars.enableKeepItems)
            {
                return false;
            }
            inventory.BlockFutureArmorUpdates();
            inventory.DeactivateItem();
            Character idMain = inventory.idMain as Character;
            if (idMain == null)
            {
                return false;
            }
            if (!Vars.bettyDeathDeleteItems)
            {
                try
                {
                    Vector3 vector;
                    Vector3 vector2;
                    string instantiateString = idMain.GetTrait<CharacterDeathDropPrefabTrait>().instantiateString;
                    Inventory.Transfer[] items = inventory.GenerateOptimizedInventoryListing(Inventory.Slot.KindFlags.Armor | Inventory.Slot.KindFlags.Belt | Inventory.Slot.KindFlags.Default, true);
                    if (items.Length == 0)
                    {
                        return false;
                    }
                    idMain.transform.GetGroundInfo(out vector, out vector2);
                    Quaternion rotation = TransformHelpers.LookRotationForcedUp(idMain.forward, vector2);
                    GameObject context = NetCull.InstantiateStatic(instantiateString, vector, rotation);
                    LootableObject component = context.GetComponent<LootableObject>();
                    if (component != null)
                    {
                        component.lifeTime = 1800f;
                    }
                    droppedTo = context.GetComponent<Inventory>();
                    if (droppedTo == null)
                    {
                        Debug.LogError("Items lost.", context);
                        return false;
                    }
                    droppedTo.ResetToReport(items);
                    flag = true;
                }
                finally
                {
                    if (inventory != null)
                    {
                        inventory.Clear();
                    }
                }
                return flag;
            }
            else
            {
                if (inventory != null)
                {
                    inventory.Clear();
                }
                return true;
            }
        }

        public static TruthDetector.ActionTaken NoteMoved(ref Vector3 pos, Angle2 ang, double time, TruthDetector td)
        {
            if (td.prevSnap.time > 0.0)
            {
                if (td.ignoreSeconds > 0.0)
                {
                    if (time > td.prevSnap.time)
                    {
                        td.ignoreSeconds -= time - td.prevSnap.time;
                    }
                }
                else
                {
                    if (td.Test_MovementSpeed(td.prevSnap.pos, pos, time - td.prevSnap.time))
                    {
                        return TruthDetector.ActionTaken.None;
                    }
                    if (td.Test_MovementTrace(td.prevSnap.pos, ref pos))
                    {
                        return TruthDetector.ActionTaken.Moved;
                    }
                }
            }
            td.prevSnap.pos = pos;
            td.prevSnap.time = time;
            td.Record();
            if (td.violation > 0)
            {
                td.violation--;
                if (truth.punish && (td.violation > truth.threshold) && !Vars.bypassList.Contains(td.netUser.userID.ToString()) && !Vars.ghostList.ContainsKey(td.netUser.userID.ToString()))
                {
                    td.LogPunishment("kicked for violation " + td.violation);
                    td.netUser.Kick(NetError.Facepunch_Kick_Violation, true);
                    return TruthDetector.ActionTaken.Kicked;
                }
                if (Vars.bypassList.Contains(td.netUser.userID.ToString()) || Vars.ghostList.ContainsKey(td.netUser.userID.ToString()))
                    td.violation = 0;
            }

            return TruthDetector.ActionTaken.None;
        }

        public static void fireBarrelConsumeFuel(FireBarrel FB)
        {
            bool isLight = false;
            bool isLit = false;
            DeployableObject deployable = FB._deployable;
            if (deployable != null)
            {
                if (Lights.isLight(deployable))
                {
                    isLight = true;
                    Light light = Lights.getLight(deployable);
                    if (light != null)
                    {
                        isLit = light.isLit;
                    }
                }
            }

            IFlammableItem item = FB.FindFuel();
            if (isLight && isLit && item == null)
            {
                EnvDecay.RefreshRadialDecay(FB.transform.position, FireBarrel.decayResetRange);
                FB.DecayTouch();
            }
            else if (item == null)
            {
                FB.SetOn(false);
            }
            else if (isLight && isLit && item != null)
            {
                EnvDecay.RefreshRadialDecay(FB.transform.position, FireBarrel.decayResetRange);
                for (int i = 3; i < 6; i++)
                {
                    IInventoryItem rawItem;
                    if ((!FB._inventory.IsSlotFree(i) && FB._inventory.GetItem(i, out rawItem)) && (rawItem is ICookableItem))
                    {
                        ItemDataBlock cookedItemDB;
                        int amountToCook;
                        int amountCooked;
                        int cookTempMin;
                        int burnTemp;
                        ICookableItem rawCookable = rawItem as ICookableItem;
                        if (rawCookable != null && rawCookable.GetCookableInfo(out amountToCook, out cookedItemDB, out amountCooked, out cookTempMin, out burnTemp) && (amountCooked <= 0 || cookedItemDB != null) && rawCookable.uses >= amountToCook && FB.myTemp >= cookTempMin)
                        {
                            if (rawCookable.Consume(ref amountToCook))
                            {
                                FB._inventory.RemoveItem(rawCookable.slot);
                            }
                            if (amountCooked > 0)
                            {
                                FB._inventory.AddItem(cookedItemDB, i - 3, amountCooked);
                            }
                        }
                    }
                }

                int count = 1;
                if (item.Consume(ref count))
                {
                    item.inventory.RemoveItem(item.slot);
                }
                FB._inventory.AddItem(ref FireBarrel.DefaultItems.byProduct, 7, 3);

                FB.DecayTouch();
            }
            else
            {
                bool flag;
                int count = 1;
                if (item.Consume(ref count))
                {
                    flag = true;
                    item.inventory.RemoveItem(item.slot);
                }
                else
                {
                    flag = false;
                }
                FB._inventory.AddItem(ref FireBarrel.DefaultItems.byProduct, 7, 3);
                EnvDecay.RefreshRadialDecay(FB.transform.position, FireBarrel.decayResetRange);
                for (int i = 3; i < 6; i++)
                {
                    IInventoryItem item2;
                    if ((!FB._inventory.IsSlotFree(i) && FB._inventory.GetItem(i, out item2)) && (item2 is ICookableItem))
                    {
                        ItemDataBlock block;
                        int num3;
                        int num4;
                        int num5;
                        int num6;
                        ICookableItem item3 = item2 as ICookableItem;
                        if (((((item3 != null) && item3.GetCookableInfo(out num3, out block, out num4, out num5, out num6)) && ((num4 <= 0) || (block != null))) && (item3.uses >= num3)) && (FB.myTemp >= num5))
                        {
                            if (item3.Consume(ref num3))
                            {
                                FB._inventory.RemoveItem(item3.slot);
                            }
                            if (num4 > 0)
                            {
                                FB._inventory.AddItem(block, i - 3, num4);
                            }
                        }
                    }
                }
                bool flag2 = false;
                for (int j = 0; j < 3; j++)
                {
                    if (!FB._inventory.IsSlotFree(j))
                    {
                        IInventoryItem item4;
                        FB._inventory.GetItem(j, out item4);
                        if (item4.uses == item4.datablock._maxUses)
                        {
                            flag2 = true;
                            break;
                        }
                    }
                }
                if (!flag2 && FB._inventory.IsSlotOccupied(7))
                {
                    IInventoryItem item5;
                    FB._inventory.GetItem(7, out item5);
                    if (item5.uses == item5.datablock._maxUses)
                    {
                        flag2 = true;
                    }
                }
                if ((flag && !FB.HasFuel()) || flag2)
                {
                    FB.SetOn(false);
                }
                FB.DecayTouch();
            }
        }

        public static void furnaceConsumeFuel(Furnace F)
        {
            bool isLight = false;
            bool isLit = false;
            DeployableObject deployable = F._deployable;
            if (deployable != null)
            {
                if (Lights.isLight(deployable))
                {
                    isLight = true;
                    Light light = Lights.getLight(deployable);
                    if (light != null)
                    {
                        isLit = light.isLit;
                    }
                }
            }

            IFlammableItem item = F.FindFuel();
            if (isLight && isLit && item == null)
            {
                EnvDecay.RefreshRadialDecay(F.transform.position, FireBarrel.decayResetRange);
                F.DecayTouch();
            }
            else if (item == null)
            {
                F.SetOn(false);
            }
            else if (isLight && isLit && item != null)
            {
                int count = 2;
                if (item.Consume(ref count))
                {
                    item.inventory.RemoveItem(item.slot);
                }
                F._inventory.AddItemAmount(ref FireBarrel.DefaultItems.byProduct, 5);
                EnvDecay.RefreshRadialDecay(F.transform.position, FireBarrel.decayResetRange);
                List<ICookableItem> list = new List<ICookableItem>(F._inventory.FindItems<ICookableItem>());
                HashSet<ItemDataBlock> set = null;
                foreach (ICookableItem item2 in list)
                {
                    ItemDataBlock block;
                    int num2;
                    int num3;
                    int num4;
                    int num5;
                    if (((((item2 != null) && item2.GetCookableInfo(out num2, out block, out num3, out num4, out num5)) && ((num3 <= 0) || (block != null))) && (item2.uses >= num2)) && (F.myTemp >= num4))
                    {
                        if (num3 > 0)
                        {
                            if (set == null)
                            {
                                set = new HashSet<ItemDataBlock> {
                            block
                        };
                            }
                            else if (!set.Add(block))
                            {
                                continue;
                            }
                        }
                        if (item2.Consume(ref num2))
                        {
                            F._inventory.RemoveItem(item2.slot);
                        }
                        if (num3 > 0)
                        {
                            if (F.myTemp >= num5)
                            {
                                F._inventory.AddItemAmount(ref FireBarrel.DefaultItems.byProduct, num3);
                            }
                            else
                            {
                                F._inventory.AddItem(block, Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Default, true, Inventory.Slot.KindFlags.Belt), num3);
                            }
                        }
                    }
                }
                F.DecayTouch();
            }
            else
            {
                bool flag;
                int count = 2;
                if (item.Consume(ref count))
                {
                    flag = true;
                    item.inventory.RemoveItem(item.slot);
                }
                else
                {
                    flag = false;
                }
                F._inventory.AddItemAmount(ref FireBarrel.DefaultItems.byProduct, 5);
                EnvDecay.RefreshRadialDecay(F.transform.position, FireBarrel.decayResetRange);
                List<ICookableItem> list = new List<ICookableItem>(F._inventory.FindItems<ICookableItem>());
                HashSet<ItemDataBlock> set = null;
                foreach (ICookableItem item2 in list)
                {
                    ItemDataBlock block;
                    int num2;
                    int num3;
                    int num4;
                    int num5;
                    if (((((item2 != null) && item2.GetCookableInfo(out num2, out block, out num3, out num4, out num5)) && ((num3 <= 0) || (block != null))) && (item2.uses >= num2)) && (F.myTemp >= num4))
                    {
                        if (num3 > 0)
                        {
                            if (set == null)
                            {
                                set = new HashSet<ItemDataBlock> {
                            block
                        };
                            }
                            else if (!set.Add(block))
                            {
                                continue;
                            }
                        }
                        if (item2.Consume(ref num2))
                        {
                            F._inventory.RemoveItem(item2.slot);
                        }
                        if (num3 > 0)
                        {
                            if (F.myTemp >= num5)
                            {
                                F._inventory.AddItemAmount(ref FireBarrel.DefaultItems.byProduct, num3);
                            }
                            else
                            {
                                F._inventory.AddItem(block, Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Default, true, Inventory.Slot.KindFlags.Belt), num3);
                            }
                        }
                    }
                }
                if (flag && !F.HasFuel())
                {
                    F.SetOn(false);
                }
                F.DecayTouch();
            }
        }
    }

    public class SteamAPIFamilyShare
    {
        public Response response;

        public class Response
        {
            public string lender_steamid;
        }
    }
}
