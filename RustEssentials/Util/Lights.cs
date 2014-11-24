using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RustEssentials.Util
{
    public class Lights
    {
        private static LightList<Light> list = new LightList<Light>();
        private static Dictionary<ulong, float> lightRanges = new Dictionary<ulong, float>();

        public static void loadList()
        {
            foreach (var obj in UnityEngine.GameObject.FindObjectsOfType<DeployableObject>())
            {
                string fancyName = Vars.getFullObjectName(obj.name);

                if (fancyName == "Camp Fire" || fancyName == "Furnace")
                {
                    StructureMaster master;
                    if (Checks.onStructure(obj.transform.position, out master))
                        Lights.add(obj.ownerID, obj, master);
                    else
                        Lights.add(obj.ownerID, obj);
                }
            }
        }

        public static void add(ulong ownerID, DeployableObject lightObj)
        {
            list.Add(new Light(ownerID, lightObj));
        }

        public static void add(ulong ownerID, DeployableObject lightObj, StructureMaster master)
        {
            list.Add(new Light(ownerID, lightObj, master));
        }

        public static void remove(DeployableObject lightObj)
        {
            list.removeLight(lightObj);
        }

        public static void handle(ulong playerUID, Character playerChar, string[] args)
        {
            if (args.Count() > 1)
            {
                string mode = args[1];
                switch (mode)
                {
                    case "on":
                        turnOn(playerUID, playerChar);
                        break;
                    case "off":
                        turnOff(playerUID, playerChar);
                        break;
                    case "serveroff":
                        string rankToUse = Vars.findRank(playerChar.netUser.userID);
                        if (!Vars.enabledCommands.ContainsKey(rankToUse))
                            rankToUse = Vars.defaultRank;

                        if (Vars.enabledCommands[rankToUse].Contains("/lights serveroff"))
                            turnAllOnServerOff(playerChar, args);
                        break;
                    case "alloff":
                        turnAllOff(playerUID, playerChar);
                        break;
                    default:
                        float range;
                        if (float.TryParse(mode, out range))
                        {
                            if (range <= Vars.maxLightsRange)
                            {
                                if (range < 0)
                                    range = 0;

                                if (lightRanges.ContainsKey(playerUID))
                                {
                                    lightRanges[playerUID] = range;
                                }
                                else
                                {
                                    lightRanges.Add(playerUID, range);
                                }
                                Broadcast.broadcastTo(playerChar.netUser.networkPlayer, "Lights range set to " + range + ".");
                            }
                            else
                            {
                                range = Vars.maxLightsRange;
                                if (lightRanges.ContainsKey(playerUID))
                                {
                                    lightRanges[playerUID] = range;
                                }
                                else
                                {
                                    lightRanges.Add(playerUID, range);
                                }
                                Broadcast.broadcastTo(playerChar.netUser.networkPlayer, "You cannot set your range over " + Vars.maxLightsRange + ". Lights range set to " + Vars.maxLightsRange + ".");
                            }
                        }
                        break;
                }
            }
        }

        public static void turnOn(ulong playerUID, Character playerChar)
        {
            if (playerChar != null && playerChar.playerClient != null)
            {
                StructureMaster master = null;
                bool onStructure = Checks.onStructure(playerChar.playerClient, out master);
                float range = getRange(playerUID);
                int amountToLight = 0;
                foreach (var light in list.getLights(playerUID, onStructure, master))
                {
                    GameObject lightObj = light.lightObj.gameObject;
                    if (!light.isLit)
                    {
                        if (onStructure)
                        {
                            amountToLight++;
                        }
                        else
                        {
                            if (Vector3.Distance(playerChar.eyesOrigin, light.lightObj.transform.position) < range)
                            {
                                amountToLight++;
                            }
                        }
                    }
                }

                if (onStructure)
                {
                    int currentStructureAmount = Lights.getLitCount(master);
                    int currentAmount = Lights.getLitCount(playerUID);
                    if (currentStructureAmount + amountToLight > Vars.maxLightsPerHouse) // we will reach the max for the house
                    {
                        int amountTried = amountToLight;
                        amountToLight = Vars.maxLightsPerHouse - currentStructureAmount;
                        if (amountToLight <= 0) // already maxed for house
                        {
                            Broadcast.broadcastTo(playerChar.netUser.networkPlayer, "This house already has a maximum of " + Vars.maxLightsPerHouse + " light(s) lit!");
                        }
                        else // light some, them max out for house
                        {
                            if (currentAmount + amountToLight > Vars.maxLightsPerPerson) // we will reach the max for ourselves
                            {
                                amountToLight = Vars.maxLightsPerPerson - currentAmount;
                                if (amountToLight <= 0) // already maxed for ourselves
                                {
                                    Broadcast.broadcastTo(playerChar.netUser.networkPlayer, "You already have a maximum of " + Vars.maxLightsPerPerson + " light(s) lit!");
                                }
                                else // light some, then max out for ourselves
                                    Broadcast.broadcastTo(playerChar.netUser.networkPlayer, "You lit " + amountToLight + " light(s), and " + (amountTried - amountToLight) + " light(s) couldn't be lit.");
                            }
                            else // we won't reach the max for ourselves
                            {
                                Broadcast.broadcastTo(playerChar.netUser.networkPlayer, "You lit " + amountToLight + " light(s), and " + (amountTried - amountToLight) + " light(s) couldn't be lit.");
                            }
                        }
                    }
                    else // we won't reach the max for the house
                    {
                        if (currentAmount + amountToLight > Vars.maxLightsPerPerson) // we will reach the max for ourselves but not the house
                        {
                            int amountTried = amountToLight;
                            amountToLight = Vars.maxLightsPerPerson - currentAmount;
                            if (amountToLight <= 0) // already maxed for ourselves
                            {
                                Broadcast.broadcastTo(playerChar.netUser.networkPlayer, "You already have a maximum of " + Vars.maxLightsPerPerson + " light(s) lit!");
                            }
                            else // light some, then max out for ourselves
                                Broadcast.broadcastTo(playerChar.netUser.networkPlayer, "You lit " + amountToLight + " light(s), and " + (amountTried - amountToLight) + " light(s) couldn't be lit.");
                        }
                        else // we won't reach the max for ourselves or for the house
                        {
                            Broadcast.broadcastTo(playerChar.netUser.networkPlayer, "You lit " + amountToLight + " light(s).");
                        }
                    }
                }
                else
                {
                    int currentAmount = Lights.getLitCount(playerUID);
                    if (currentAmount + amountToLight > Vars.maxLightsPerPerson)
                    {
                        int amountTried = amountToLight;
                        amountToLight = Vars.maxLightsPerPerson - currentAmount;
                        if (amountToLight <= 0)
                        {
                            Broadcast.broadcastTo(playerChar.netUser.networkPlayer, "You already have a maximum of " + Vars.maxLightsPerPerson + " light(s) lit!");
                        }
                        else
                            Broadcast.broadcastTo(playerChar.netUser.networkPlayer, "You lit " + amountToLight + " light(s), and " + (amountTried - amountToLight) + " light(s) couldn't be lit.");
                    }
                    else
                    {
                        Broadcast.broadcastTo(playerChar.netUser.networkPlayer, "You lit " + amountToLight + " light(s).");
                    }
                }

                if (amountToLight > 0)
                {
                    int lightIndex = 0;
                    PlayerClient playerClient = Vars.getPlayerClient(playerUID);
                    foreach (var light in list.getLights(playerUID, onStructure, master))
                    {
                        lightIndex++;
                        if (lightIndex > amountToLight)
                            break;

                        GameObject lightObj = light.lightObj.gameObject;
                        if (!light.isLit)
                        {
                            if (onStructure)
                            {
                                light.turnOn(playerClient);
                            }
                            else
                            {
                                if (Vector3.Distance(playerChar.eyesOrigin, light.lightObj.transform.position) < range)
                                {
                                    light.turnOn(playerClient);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void turnOff(ulong playerUID, Character playerChar)
        {
            StructureMaster master = null;
            bool onStructure = Checks.onStructure(playerChar.playerClient, out master);
            float range = getRange(playerUID);
            int amountTurnedOff = 0;
            foreach (var light in list.getLights(playerUID, onStructure, master))
            {
                GameObject lightObj = light.lightObj.gameObject;
                if (light.isLit)
                {
                    if (onStructure)
                    {
                        light.turnOff();
                        amountTurnedOff++;
                    }
                    else
                    {
                        if (Vector3.Distance(playerChar.eyesOrigin, light.lightObj.transform.position) < range)
                        {
                            light.turnOff();
                            amountTurnedOff++;
                        }
                    }
                }
            }

            if (amountTurnedOff > 0)
            {
                Broadcast.broadcastTo(playerChar.netUser.networkPlayer, "You unlit " + amountTurnedOff + " light(s).");
            }
        }

        public static void turnAllOnServerOff(Character playerChar, string[] args)
        {
            int amountTurnedOff = 0;

            if (args.Count() > 2)
            {
                float distance;
                if (float.TryParse(args[2], out distance))
                {
                    foreach (var light in list)
                    {
                        GameObject lightObj = light.lightObj.gameObject;
                        if (light.isLit)
                        {
                            if (Vector3.Distance(playerChar.eyesOrigin, light.lightObj.transform.position) < distance)
                            {
                                light.turnOff();
                                amountTurnedOff++;
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var light in list)
                {
                    GameObject lightObj = light.lightObj.gameObject;
                    if (light.isLit)
                    {
                        light.turnOff();
                        amountTurnedOff++;
                    }
                }
            }

            if (amountTurnedOff > 0)
            {
                Broadcast.broadcastTo(playerChar.netUser.networkPlayer, "You unlit " + amountTurnedOff + " light(s).");
            }
        }

        public static void turnAllOff(ulong playerUID, Character playerChar)
        {
            int amountTurnedOff = 0;
            foreach (var light in list.getAllLights(playerUID))
            {
                GameObject lightObj = light.lightObj.gameObject;
                if (light.isLit)
                {
                    light.turnOff();
                    amountTurnedOff++;
                }
            }

            if (amountTurnedOff > 0)
            {
                Broadcast.broadcastTo(playerChar.netUser.networkPlayer, "You unlit " + amountTurnedOff + " light(s).");
            }
        }

        public static bool isLight(DeployableObject deployable)
        {
            return list.isLight(deployable);
        }

        public static Light getLight(DeployableObject deployable)
        {
            return list.getLight(deployable);
        }

        public static float getRange(ulong ownerID)
        {
            if (lightRanges.ContainsKey(ownerID))
            {
                float range = lightRanges[ownerID];
                if (range > 0 && range <= Vars.maxLightsRange)
                    return range;
            }

            return Vars.defaultLightsRange;
        }

        public static int getLitCount(ulong ownerID)
        {
            return list.getLitCount(ownerID);
        }

        public static int getLitCount(StructureMaster master)
        {
            return list.getLitCount(master);
        }
    }

    public class Light
    {
        public ulong ownerID;
        public DeployableObject lightObj;
        public StructureMaster master;
        public bool isLit;

        public Light(ulong ownerID, DeployableObject lightObj)
        {
            this.ownerID = ownerID;
            this.lightObj = lightObj;
            this.master = null;
            this.isLit = false;
        }

        public Light(ulong ownerID, DeployableObject lightObj, StructureMaster master)
        {
            this.ownerID = ownerID;
            this.lightObj = lightObj;
            this.master = master;
            this.isLit = false;
        }

        public void turnOn(PlayerClient playerClient)
        {
            isLit = true;
            GameObject GO = lightObj.gameObject;
            if (GO.GetComponent<FireBarrel>() != null)
            {
                if (GO.GetComponent<Furnace>() != null)
                    GO.GetComponent<Furnace>().SetOn(true);
                else
                    GO.GetComponent<FireBarrel>().SetOn(true);
            }
            Vars.REB.StartCoroutine(distanceCheck(playerClient));
        }

        public void turnOff()
        {
            isLit = false;
            GameObject GO = lightObj.gameObject;
            if (GO.GetComponent<FireBarrel>() != null)
            {
                if (GO.GetComponent<Furnace>() != null)
                    GO.GetComponent<Furnace>().SetOn(false);
                else
                    GO.GetComponent<FireBarrel>().SetOn(false);
            }
        }

        public IEnumerator distanceCheck(PlayerClient playerClient)
        {
            GameObject GO = lightObj.gameObject;
            while (isLit && GO != null && playerClient != null)
            {
                yield return new WaitForSeconds(15f);
                GO = lightObj.gameObject;
                if (GO != null && playerClient != null)
                {
                    if (Vars.AllCharacters.ContainsKey(playerClient))
                    {
                        Character playerChar = Vars.AllCharacters[playerClient];
                        if (playerChar != null)
                        {
                            if (Vector3.Distance(GO.transform.position, playerChar.eyesOrigin) > 300)
                                turnOff();
                        }
                        else
                        {
                            playerChar = Vars.getPlayerChar(playerClient);
                            if (playerChar != Vars.AllCharacters[playerClient])
                                Vars.AllCharacters[playerClient] = playerChar;
                        }
                    }
                    else
                    {
                        Character playerChar = Vars.getPlayerChar(playerClient);
                        if (playerChar != null)
                        {
                            Vars.AllCharacters.Add(playerClient, playerChar);
                            if (Vector3.Distance(GO.transform.position, playerChar.eyesOrigin) > 300)
                                turnOff();
                        }
                    }
                }
            }
        }
    }

    public class LightList<T> : List<T> where T : Light
    {
        public int getLitCount(ulong ownerID)
        {
            int curIndex = 0;
            foreach (var obj in this)
            {
                if (obj is Light)
                {
                    Light light = obj as Light;
                    if (light.ownerID == ownerID && light.isLit)
                    {
                        curIndex++;
                    }
                }
            }
            return curIndex;
        }

        public int getLitCount(StructureMaster master)
        {
            int curIndex = 0;
            foreach (var obj in this)
            {
                if (obj is Light)
                {
                    Light light = obj as Light;
                    if (light.master == master && light.isLit)
                    {
                        curIndex++;
                    }
                }
            }
            return curIndex;
        }

        public bool isLight(DeployableObject lightObj)
        {
            foreach (var obj in this)
            {
                if (obj is Light)
                {
                    if ((obj as Light).lightObj == lightObj)
                        return true;
                }
            }
            return false;
        }

        public Light getLight(DeployableObject lightObj)
        {
            foreach (var obj in this)
            {
                if (obj is Light)
                {
                    if ((obj as Light).lightObj == lightObj)
                        return (obj as Light);
                }
            }
            return null;
        }

        public List<Light> getLights(ulong ownerID, bool onStructure, StructureMaster master)
        {
            List<Light> ownerLights = new List<Light>();
            foreach (var obj in this)
            {
                if (obj is Light)
                {
                    Light light = obj as Light;
                    if (light.ownerID == ownerID && !ownerLights.Contains(light))
                    {
                        if (onStructure && light.master != null && master != null && light.master == master)
                            ownerLights.Add(light);
                        else if (!onStructure && light.master == null)
                            ownerLights.Add(light);
                    }
                }
            }
            return ownerLights;
        }

        public List<Light> getAllLights(ulong ownerID)
        {
            List<Light> ownerLights = new List<Light>();
            foreach (var obj in this)
            {
                if (obj is Light)
                {
                    Light light = obj as Light;
                    if (light.ownerID == ownerID && !ownerLights.Contains(light))
                    {
                        ownerLights.Add(light);
                    }
                }
            }
            return ownerLights;
        }

        public void removeLight(DeployableObject lightObj)
        {
            Light light = null;
            foreach (var obj in this)
            {
                if (obj is Light)
                {
                    if ((obj as Light).lightObj == lightObj)
                        light = obj as Light;
                }
            }

            this.Remove((T)light);
        }
    }
}
