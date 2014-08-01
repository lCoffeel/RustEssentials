using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace RustEssentials.Util
{
    public static class Explosions
    {
        public static BettyList<BouncingBetty> bettyList = new BettyList<BouncingBetty>();
        public static Dictionary<ulong, int> strappedUserIDs = new Dictionary<ulong, int>();
        public static void explode(Vector3 position)
        {
            try
            {
                TimedExplosive TE = NetCull.InstantiateStatic(";explosive_charge", position, default(Quaternion)).GetComponent<TimedExplosive>();
                TE.Explode();
            }
            catch (Exception ex) { Vars.conLog.Error("EXPLODE: " + ex.ToString()); }
        }

        public static void fakeExplode(Vector3 position)
        {
            try
            {
                TimedExplosive TE = NetCull.InstantiateStatic(";explosive_charge", position, default(Quaternion)).GetComponent<TimedExplosive>();
                TE.collider.enabled = false;
                TE.testView.RPC("ClientExplode", uLink.RPCMode.Others);
                NetCull.Destroy(TE.gameObject);
            }
            catch (Exception ex) { Vars.conLog.Error("FEXPLODE: " + ex.ToString()); }
        }

        public static void bettyExplode(Vector3 bettyPos, Vector3 position, ulong ownerUID)
        {
            try
            {
                //damage = 600;
                //range = 3;
                TimedExplosive TE = NetCull.InstantiateStatic(";explosive_charge", position, default(Quaternion)).GetComponent<TimedExplosive>();
                TE.collider.enabled = false;
                TE.testView.RPC("ClientExplode", uLink.RPCMode.Others);

                TransCarrier carrier;
                IDMain main = null;
                bool flag = false;
                DeployableObject component = TE.GetComponent<DeployableObject>();
                if (component != null)
                {
                    carrier = component.GetCarrier();
                    if (carrier != null)
                    {
                        flag = (bool)(main = IDBase.GetMain((IDRelative)carrier));
                    }
                }
                else
                {
                    carrier = null;
                }
                Vector3 center = TE.collider.bounds.center;
                if (center == Vector3.zero)
                {
                    center = TE.transform.TransformPoint(new Vector3(0f, 0f, 0.1f));
                }
                KeyValuePair<string, Dictionary<string, string>>[] ownersFaction = Array.FindAll(Vars.factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(ownerUID.ToString()));

                float highestValue = Vars.bleedingRadius;
                if (Vars.breakLegsRadius > highestValue)
                    highestValue = Vars.breakLegsRadius;
                if (Vars.hurtRadius > highestValue)
                    highestValue = Vars.hurtRadius;

                List<KeyValuePair<IDBase, float>> list = new List<KeyValuePair<IDBase, float>>();
                foreach (ExplosionHelper.Surface surface in ExplosionHelper.OverlapExplosionUnique(center, highestValue, 0x10360401, -1, null))
                {
                    if (!flag || (surface.idMain != main))
                    {
                        float num2 = (1f - Mathf.Clamp01(surface.work.distanceToCenter / Vars.hurtRadius)) * Vars.maxBettyObjectDamage;
                        if (surface.blocked)
                        {
                            num2 *= 0.1f;
                        }

                        if (Checks.isPlayer(surface.idMain) && !Checks.hasObstruction(center, ((Character)surface.idMain).eyesOrigin, surface.idMain.gameObject))
                        {
                            Character targetChar = surface.idMain as Character;

                            bool canHurt = false;

                            if (ownerUID == targetChar.netUser.userID)
                            {
                                canHurt = Vars.bettyHurtOwner;
                            }
                            else
                            {
                                if (ownersFaction.Count() > 0)
                                {
                                    if (ownersFaction[0].Value.ContainsKey(targetChar.netUser.userID.ToString()))
                                    {
                                        canHurt = Vars.bettyHurtFaction;
                                    }
                                    else
                                    {
                                        if (Vars.alliances.ContainsKey(ownersFaction[0].Key))
                                        {
                                            KeyValuePair<string, Dictionary<string, string>>[] targetsFaction = Array.FindAll(Vars.factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(targetChar.netUser.userID.ToString()));
                                            if (targetsFaction.Count() > 0 && Vars.alliances[ownersFaction[0].Key].Contains(targetsFaction[0].Key))
                                                canHurt = Vars.bettyHurtAlly;
                                            else
                                                canHurt = true;
                                        }
                                        else
                                            canHurt = true;
                                    }
                                }
                                else
                                    canHurt = true;
                            }

                            if (canHurt)
                            {
                                num2 = 0f;
                                if (surface.work.distanceToCenter <= Vars.bleedingRadius)
                                {
                                    targetChar.controllable.GetComponent<HumanBodyTakeDamage>().AddBleedingLevel(5f);
                                    num2 = 5f;
                                }
                                if (surface.work.distanceToCenter <= Vars.breakLegsRadius)
                                {
                                    if (targetChar.controllable.GetComponent<FallDamage>() != null)
                                        targetChar.controllable.GetComponent<FallDamage>().AddLegInjury(1f);
                                    num2 = 10f;
                                }
                                if (surface.work.distanceToCenter <= Vars.hurtRadius)
                                {
                                    num2 = (1f - Mathf.Clamp01(surface.work.distanceToCenter / Vars.hurtRadius)) * Vars.maxBettyPlayerDamage;
                                }
                                list.Add(new KeyValuePair<IDBase, float>(surface.idBase, num2));
                                if (num2 >= targetChar.health && !Vars.diedToBetty.KilledByBetty(targetChar.netUser.userID))
                                    Vars.diedToBetty.Add(new BouncingBettyKill(targetChar.netUser.userID, ownerUID, bettyPos));
                            }
                        }
                        else if (!Checks.isPlayer(surface.idMain))
                            list.Add(new KeyValuePair<IDBase, float>(surface.idBase, num2));
                    }
                }
                if (flag && (main != null))
                {
                    list.Add(new KeyValuePair<IDBase, float>(carrier, Vars.maxBettyObjectDamage));
                }
                foreach (KeyValuePair<IDBase, float> pair in list)
                {
                    TakeDamage.Hurt(TE, pair.Key, new DamageTypeList(0f, 0f, 0f, pair.Value, 0f, 0f), null);
                }

                NetCull.Destroy(TE.gameObject);
            }
            catch (Exception ex) { Vars.conLog.Error("CEXPLODE: " + ex.ToString()); }
        }

        public static IEnumerator dropMine(Vector3 bettyPos, string ownerName, ulong ownerUID)
        {
            if (Vars.enableBouncingBetty)
            {
                float jumpHeight = 6f;
                float deleteTime = 0.8f;

                GameObject go = null;
                Quaternion rotation = default(Quaternion);
                BouncingBetty betty = null;

                try
                {
                    rotation = Quaternion.LookRotation(Vector3.forward);
                    go = NetCull.InstantiateDynamicWithArgs<Vector3>("GenericItemPickup", bettyPos, rotation, new Vector3());
                    betty = go.AddComponent<BouncingBetty>();
                    betty.ownerName = ownerName;
                    betty.ownerID = ownerUID;
                    betty.bettyPos = bettyPos;
                    bettyList.Add(betty);
                    go.GetComponent<ItemPickup>().CancelInvoke();
                }
                catch (Exception ex)
                {
                    Vars.conLog.Error("DM: " + ex.ToString());
                }

                if (go != null)
                {
                    yield return Vars.REB.StartCoroutine(playerIsNear(go));

                    Vector3 explosionPos = new Vector3();
                    try
                    {
                        if (go != null)
                        {
                            Vector3 arg = Vector3.up * jumpHeight;

                            if (go != null)
                            {
                                bettyList.Remove(go.GetComponent<BouncingBetty>());
                                Data.remBettyData(ownerUID.ToString(), bettyPos);
                                NetCull.Destroy(go);
                            }

                            go = NetCull.InstantiateDynamicWithArgs<Vector3>("GenericItemPickup", bettyPos, rotation, arg);

                        }
                    }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("DM #2: " + ex.ToString());
                    }

                    yield return new WaitForSeconds(deleteTime);

                    try
                    {
                        if (go != null)
                        {
                            explosionPos = go.transform.position;
                            explosionPos.y += 1.3f;
                            bettyExplode(bettyPos, explosionPos, ownerUID);

                            if (go != null)
                                NetCull.Destroy(go);
                        }
                    }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("DM #3: " + ex.ToString());
                    }
                }
            }
            yield return null;
        }

        public static IEnumerator dropMine(PlayerClient playerClient, string[] args)
        {
            if (Vars.enableBouncingBetty)
            {
                if (!bettyList.IsAtLimit(playerClient.userID))
                {
                    bool hasItems = true;
                    foreach (Items.Item item in Vars.bettyRecipe)
                    {
                        List<IInventoryItem> items;
                        if (Items.grabItem(playerClient, item.name, out items))
                        {
                            int totalAmount = 0;
                            foreach (IInventoryItem invItem in items)
                            {
                                totalAmount += invItem.uses;
                            }

                            if (totalAmount < item.amount)
                                hasItems = false;
                        }
                        else
                            hasItems = false;
                    }

                    if (hasItems)
                    {
                        Character playerChar;

                        bool nearHouse = false;
                        if (!Vars.bettyNearOtherHouses && Vars.getPlayerChar(playerClient, out playerChar))
                        {
                            RaycastHit[] hits = Physics.SphereCastAll(new Ray(playerChar.eyesOrigin + new Vector3(0, 5, 0), Vector3.down), Vars.distanceFromOtherHouses, 30f, -472317957);
                            foreach (var hit in hits)
                            {
                                IDMain main = IDBase.GetMain(hit.collider);

                                //Vars.conLog.Info(hit.collider.gameObject.layer.ToString());
                                if ((hit.collider.gameObject.layer == 10 && hit.collider.gameObject.name.Contains("Shelter")) || (main != null))
                                {
                                    string extra = "";
                                    if (main != null)
                                    {
                                        extra = " : " + main.name;
                                        if (main is StructureMaster)
                                        {
                                            StructureMaster master = main as StructureMaster;
                                            nearHouse = master.ownerID != playerClient.userID;
                                        }
                                    }
                                    else if (hit.collider.gameObject.GetComponent<DeployableObject>() != null)
                                    {
                                        DeployableObject deployable = hit.collider.gameObject.GetComponent<DeployableObject>();
                                        nearHouse = deployable.ownerID != playerClient.userID;
                                    }

                                    //Vars.conLog.Info(hit.collider.gameObject.layer + ": " + hit.collider.gameObject.name + extra);
                                }
                            }
                        }

                        if (!nearHouse)
                        {
                            foreach (Items.Item item in Vars.bettyRecipe)
                            {
                                List<IInventoryItem> items;
                                if (Items.grabItem(playerClient, item.name, out items))
                                {
                                    int totalAmount = 0;
                                    foreach (IInventoryItem invItem in items)
                                    {
                                        totalAmount += invItem.uses;
                                    }

                                    //Vars.conLog.Info(item.name + ": " + totalAmount + "/" + item.amount);
                                    if (totalAmount >= item.amount)
                                    {
                                        int amountToRemove = item.amount;
                                        foreach (IInventoryItem invItem in items)
                                        {
                                            if (invItem.Consume(ref amountToRemove))
                                                Items.removeItem(playerClient, invItem);

                                            if (amountToRemove <= 0)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            float jumpHeight = 6f;
                            //int mineLevel = 1;
                            float deleteTime = 0.8f;
                            //if (args.Count() > 1)
                            //{
                            //    float.TryParse(args[1], out jumpHeight);
                            //    if (args.Count() > 2)
                            //    {
                            //        int.TryParse(args[2], out mineLevel);
                            //        if (args.Count() > 3)
                            //        {
                            //            float.TryParse(args[3], out deleteTime);
                            //        }
                            //    }
                            //}

                            Character idMain = null;
                            GameObject go = null;
                            CharacterItemDropPrefabTrait trait = null;
                            Vector3 position = new Vector3();
                            Quaternion rotation = default(Quaternion);
                            BouncingBetty betty = null;

                            try
                            {
                                //Broadcast.broadcastTo(playerClient.netPlayer, "You planted a level " + mineLevel + " land mine.");
                                Broadcast.noticeTo(playerClient.netPlayer, "☢", "Bouncing betty placed!" + (Vars.bettyArmingDelay > 0 ? " Arming betty in " + Vars.bettyArmingDelay + " seconds..." : ""), 5);
                                idMain = playerClient.controllable.GetComponent<Inventory>().idMain as Character;
                                if (idMain != null)
                                {
                                    trait = idMain.GetTrait<CharacterItemDropPrefabTrait>();
                                    position = idMain.transform.position;
                                    rotation = Quaternion.LookRotation(Vector3.forward);
                                    go = NetCull.InstantiateDynamicWithArgs(trait.prefab, position, rotation, new Vector3());
                                    betty = go.AddComponent<BouncingBetty>();
                                    betty.ownerName = playerClient.userName;
                                    betty.ownerID = playerClient.userID;
                                    betty.bettyPos = position;
                                    bettyList.Add(betty);
                                    Data.addBettyData(playerClient.userName, playerClient.userID.ToString(), go.GetComponent<BouncingBetty>().bettyPos);
                                    ItemPickup itemPickup = go.GetComponent<ItemPickup>();
                                    itemPickup.CancelInvoke();
                                }
                            }
                            catch (Exception ex)
                            {
                                Vars.conLog.Error("DM: " + ex.ToString());
                            }

                            Vector3 lastPos = go.transform.position;
                            yield return new WaitForSeconds(0.9f);
                            if (Vars.bettyOnlyOnFlat && Vector3.Distance(lastPos, go.transform.position) > 0.5f)
                            {
                                Broadcast.noticeTo(playerClient.netPlayer, "☢", "You can only place a bouncing betty on mostly flat surfaces!", 5);
                                if (go != null)
                                {
                                    Data.remBettyData(playerClient.userID.ToString(), go.GetComponent<BouncingBetty>().bettyPos);
                                    bettyList.Remove(go.GetComponent<BouncingBetty>());
                                    NetCull.Destroy(go);

                                    Broadcast.sideNoticeTo(playerClient.netPlayer, "1 x Bouncing Betty");

                                    foreach (Items.Item iitem in Vars.bettyRecipe)
                                    {
                                        Items.addItem(playerClient, iitem.name, iitem.amount);
                                    }
                                }
                            }
                            else
                            {
                                if (Vars.bettyArmingDelay > 0)
                                {
                                    yield return new WaitForSeconds(Vars.bettyArmingDelay - 0.9f);

                                    if (go != null)
                                        Broadcast.noticeTo(playerClient.netPlayer, "☢", "Bouncing betty armed!", 2);
                                }

                                if (go != null)
                                {
                                    yield return Vars.REB.StartCoroutine(playerIsNear(go));

                                    Vector3 explosionPos = new Vector3();
                                    Vector3 bettyPos = new Vector3();
                                    if (go != null)
                                    {
                                        bettyPos = go.GetComponent<BouncingBetty>().bettyPos;
                                    }

                                    try
                                    {
                                        if (idMain != null && go != null)
                                        {
                                            Vector3 arg = Vector3.up * jumpHeight;

                                            if (go != null)
                                            {
                                                Data.remBettyData(playerClient.userID.ToString(), bettyPos);
                                                bettyList.Remove(go.GetComponent<BouncingBetty>());
                                                NetCull.Destroy(go);
                                            }

                                            go = NetCull.InstantiateDynamicWithArgs<Vector3>(trait.prefab, position, rotation, arg);

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Vars.conLog.Error("DM #2: " + ex.ToString());
                                    }

                                    yield return new WaitForSeconds(deleteTime);

                                    try
                                    {
                                        if (idMain != null && go != null)
                                        {
                                            explosionPos = bettyPos;
                                            explosionPos.y += 1.3f;
                                            bettyExplode(bettyPos, explosionPos, playerClient.userID);

                                            if (go != null)
                                                NetCull.Destroy(go);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Vars.conLog.Error("DM #3: " + ex.ToString());
                                    }
                                }
                            }
                        }
                        else
                            Broadcast.broadcastTo(playerClient.netPlayer, "You cannot place that near another player's house!");
                    }
                    else
                    {
                        Broadcast.broadcastTo(playerClient.netPlayer, "You must have these items to place a bouncing betty:");
                        foreach (Items.Item item in Vars.bettyRecipe)
                        {
                            Broadcast.broadcastTo(playerClient.netPlayer, item.amount + " x " + item.name);
                        }
                    }
                }
                else
                    Broadcast.broadcastTo(playerClient.netPlayer, "You have reached the maximum of " + Vars.bettiesPerPlayer + " bouncing betties!");
            }
            else
                Broadcast.broadcastTo(playerClient.netPlayer, "Bouncing betties are disabled on this server!");
            yield return null;
        }

        public static IEnumerator playerIsNear(GameObject betty)
        {
            bool hasNearby = false;
            Vector3 bettyPos = betty.transform.position;
            BouncingBetty bouncingBetty = betty.GetComponent<BouncingBetty>();
            ulong ownerUID = bouncingBetty.ownerID;
            
            while (!hasNearby)
            {
                if (betty != null && bouncingBetty != null)
                {
                    try
                    {
                        KeyValuePair<string, Dictionary<string, string>>[] ownersFaction = Array.FindAll(Vars.factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(bouncingBetty.ownerID.ToString()));

                        foreach (Character c in Vars.AllCharacters.Values.ToArray())
                        {
                            if (c != null && c.alive)
                            {
                                if (Vector3.Distance(bettyPos, c.eyesOrigin) < Vars.activateRadius && !Checks.hasObstruction(bettyPos, c.eyesOrigin, c.gameObject))
                                {
                                    if (ownerUID == c.netUser.userID)
                                    {
                                        hasNearby = Vars.ownerActivateBetty;
                                    }
                                    else
                                    {
                                        if (ownersFaction.Count() > 0)
                                        {
                                            if (ownersFaction[0].Value.ContainsKey(c.netUser.userID.ToString()))
                                            {
                                                hasNearby = Vars.factionActivateBetty;
                                            }
                                            else
                                            {
                                                if (Vars.alliances.ContainsKey(ownersFaction[0].Key))
                                                {
                                                    KeyValuePair<string, Dictionary<string, string>>[] targetsFaction = Array.FindAll(Vars.factions.ToArray(), (KeyValuePair<string, Dictionary<string, string>> kv) => kv.Value.ContainsKey(c.netUser.userID.ToString()));
                                                    if (targetsFaction.Count() > 0 && Vars.alliances[ownersFaction[0].Key].Contains(targetsFaction[0].Key))
                                                        hasNearby = Vars.allyActivateBetty;
                                                    else
                                                        hasNearby = true;
                                                }
                                                else
                                                    hasNearby = true;
                                            }
                                        }
                                        else
                                        {
                                            hasNearby = true;
                                        }
                                    }
                                }
                            }
                        }

                        if (hasNearby)
                            break;
                    }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("PIN: " + ex.ToString());
                    }
                    yield return new WaitForSeconds(0.1f);
                }
                else
                    hasNearby = true;
            }
        }

        public static void strapExplosive(ulong playerUID, int amount)
        {
            if (!strappedUserIDs.ContainsKey(playerUID))
            {
                strappedUserIDs.Add(playerUID, amount);
            }
            else
            {
                strappedUserIDs[playerUID] += amount;
            }
        }

        public static void unstrapExplosive(ulong playerUID, int amount)
        {
            int currentAmount = getAmountStrapped(playerUID);
            if (currentAmount - amount > 0)
            {
                strappedUserIDs[playerUID] -= amount;
            }
            else if (currentAmount - amount == 0)
            {
                strappedUserIDs.Remove(playerUID);
            }
        }

        public static bool isStrapped(ulong playerUID)
        {
            return strappedUserIDs.ContainsKey(playerUID);
        }

        public static int getAmountStrapped(ulong playerUID)
        {
            int amount = 0;
            if (strappedUserIDs.ContainsKey(playerUID))
                amount = strappedUserIDs[playerUID];

            return amount;
        }
    }

    [Serializable]
    public class BouncingBetty : Facepunch.MonoBehaviour
    {
        public ulong ownerID;
        public string ownerName;
        public Vector3 bettyPos;

        public void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public class BettyList<T> : List<T> where T : BouncingBetty
    {
        public bool IsAtLimit(ulong userID)
        {
            int bettyCount = 0;
            foreach (var obj in this)
            {
                if (obj is BouncingBetty)
                {
                    BouncingBetty betty = obj as BouncingBetty;
                    if (betty.ownerID == userID)
                        bettyCount++;
                }
            }
            return bettyCount >= Vars.bettiesPerPlayer;
        }

        public bool Update(ulong userID, string newUserName)
        {
            if (Vars.vanishedList.Contains(userID.ToString()))
                return false;

            List<int> bettyIndexes = new List<int>();
            int curIndex = 0;
            foreach (var obj in this)
            {
                if (obj is BouncingBetty)
                {
                    BouncingBetty betty = obj as BouncingBetty;
                    if (betty.ownerID == userID)
                    {
                        bettyIndexes.Add(curIndex);
                    }
                }
                curIndex++;
            }

            bool hasNewName = false;
            if (bettyIndexes.Count > 0)
            {
                foreach (int index in bettyIndexes)
                {
                    if (this[index].ownerName != newUserName)
                    {
                        hasNewName = true;
                        this[index].ownerName = newUserName;
                    }
                }
            }

            return hasNewName;
        }
    }

    public class BouncingBettyKill
    {
        public ulong victimID;
        public ulong killerID;
        public Vector3 bettyPos;

        public BouncingBettyKill(ulong victimID, ulong killerID, Vector3 bettyPos)
        {
            this.victimID = victimID;
            this.killerID = killerID;
            this.bettyPos = bettyPos;
        }
    }

    public class BettyKillList<T> : List<T> where T : BouncingBettyKill
    {
        public bool KilledByBetty(ulong userID)
        {
            foreach (var obj in this)
            {
                if (obj is BouncingBettyKill)
                {
                    BouncingBettyKill bettyKill = obj as BouncingBettyKill;
                    if (bettyKill.victimID == userID)
                        return true;
                }
            }
            return false;
        }

        public void RemoveByID(ulong userID)
        {
            BouncingBettyKill bettyKill = null;
            foreach (var obj in this)
            {
                if (obj is BouncingBettyKill)
                {
                    if ((obj as BouncingBettyKill).victimID == userID)
                        bettyKill = obj as BouncingBettyKill;
                }
            }
            
            if (bettyKill != null)
                this.Remove((T)bettyKill);
        }

        public BouncingBettyKill GetByID(ulong userID)
        {
            foreach (var obj in this)
            {
                if (obj is BouncingBettyKill)
                {
                    BouncingBettyKill bettyKill = obj as BouncingBettyKill;
                    if (bettyKill.victimID == userID)
                        return bettyKill;
                }
            }
            return null;
        }
    }
}