using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace RustEssentials.Util
{
    public static class Elevators
    {
        public static bool simulateTeleport(PlayerClient playerClient, Vector3 destination)
        {
            Character playerChar;
            if (Vars.getPlayerChar(playerClient, out playerChar))
            {
                if (Vars.lastPositions.ContainsKey(playerChar))
                    Vars.lastPositions[playerChar] = destination;
            }

            RustServerManagement RSM = RustServerManagement.Get();
            if (playerClient.netUser != null)
            {
                playerClient.netUser.truthDetector.NoteTeleported(destination, 2.0);
            }
            if (playerClient.controllable != null)
            {
                RSM.networkView.RPC<Vector3>("UnstickMove", playerClient.netPlayer, destination);
                return true;
            }
            return false;
        }

        public static Dictionary<ulong, List<StructureComponent>> elevatorObjects = new Dictionary<ulong, List<StructureComponent>>();

        public static bool isElevator(StructureComponent SC)
        {
            foreach (var list in elevatorObjects.Values)
            {
                foreach (var comp in list)
                {
                    if (comp == SC)
                        return true;
                }
            }
            return false;
        }

        public static void addElevator(StructureComponent SC, Character playerChar, bool silent = false)
        {
            try
            {
                if (SC._master.ownerID == playerChar.netUser.userID)
                {
                    if (!elevatorObjects.ContainsKey(playerChar.netUser.userID))
                    {
                        elevatorObjects.Add(playerChar.netUser.userID, new List<StructureComponent>() { { SC } });
                        if (!silent)
                            Broadcast.broadcastTo(playerChar.netUser.networkPlayer, "You have established an elevator!");
                    }
                    else
                    {
                        if (!elevatorObjects[playerChar.netUser.userID].Contains(SC))
                        {
                            elevatorObjects[playerChar.netUser.userID].Add(SC);
                            if (!silent)
                                Broadcast.broadcastTo(playerChar.netUser.networkPlayer, "You have established an elevator!");
                        }
                        else if (!silent)
                            Broadcast.broadcastTo(playerChar.netUser.networkPlayer, "You already own this elevator!");
                    }
                }
                else if (!silent)
                    Broadcast.broadcastTo(playerChar.netUser.networkPlayer, "You can only make elevators out of ceilings owned by you.");
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("ADDELE: " + ex.ToString());
            }
        }

        public static void removeElevator(StructureComponent SC, Character playerChar, bool silent = false)
        {
            try
            {
                if (SC._master.ownerID == playerChar.netUser.userID)
                {
                    if (elevatorObjects.ContainsKey(playerChar.netUser.userID))
                    {
                        elevatorObjects[playerChar.netUser.userID].Remove(SC);
                        if (!silent)
                            Broadcast.broadcastTo(playerChar.netUser.networkPlayer, "You have removed an elevator!");
                    }
                    else
                    {
                        if (!silent)
                            Broadcast.broadcastTo(playerChar.netUser.networkPlayer, "This is not an elevator!");
                    }
                }
                else if (!silent)
                    Broadcast.broadcastTo(playerChar.netUser.networkPlayer, "You can only remove elevators owned by you.");
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("REMELE: " + ex.ToString());
            }
        }

        public static List<object> objectsOnElevator(StructureComponent SC)
        {
            List<object> objects = new List<object>();
            try
            {
                Vector3 elevatorOrigin = SC.transform.position + new Vector3(0, 5, 0);
                RaycastHit[] hits = Physics.SphereCastAll(elevatorOrigin, 2.6f, Vector3.up, 5f);
                foreach (var hit in hits)
                {
                    if (Mathf.Abs(hit.collider.gameObject.transform.position.y - elevatorOrigin.y) < 2)
                    {
                        if (hit.collider.gameObject.GetComponent<DeployableObject>() != null)
                        {
                            objects.Add(hit.collider.gameObject.GetComponent<DeployableObject>());
                        }
                        if (hit.collider.gameObject.GetComponent<Character>() != null)
                        {
                            objects.Add(hit.collider.gameObject.GetComponent<Character>());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("OOE: " + ex.ToString());
            }
            return objects;
        }

        public static List<StructureComponent> playersElevators(Character playerChar)
        {
            try
            {
                ulong UID = playerChar.netUser.userID;
                if (elevatorObjects.ContainsKey(UID))
                {
                    List<StructureComponent> elevators = new List<StructureComponent>();
                    foreach (var obj in elevatorObjects[UID])
                    {
                        if (obj != null)
                            elevators.Add(obj);
                    }
                    elevatorObjects[UID] = elevators;
                    return elevatorObjects[UID];
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("PELEV: " + ex.ToString());
            }
            return new List<StructureComponent>();
        }

        public static List<StructureComponent> childCeilings(StructureMaster SM)
        {
            List<StructureComponent> ceilings = new List<StructureComponent>();

            if (SM != null)
            {
                ceilings = Array.FindAll(UnityEngine.Object.FindObjectsOfType<StructureComponent>().ToArray(), (StructureComponent SC) => SC._master == SM && SC.type == StructureComponent.StructureComponentType.Ceiling).ToList();
            }

            return ceilings;
        }

        public static bool hasCeilingAbove(StructureComponent elevator, out StructureComponent ceiling)
        {
            if (elevator != null)
            {
                Vector2 vector = new Vector2(elevator.transform.position.x, elevator.transform.position.z);
                StructureComponent[] ceilings = Array.FindAll(childCeilings(elevator._master).ToArray(), (StructureComponent SC) => SC != elevator && new Vector2(SC.transform.position.x, SC.transform.position.z) == vector && SC.transform.position.y > elevator.transform.position.y);
                if (ceilings != null && ceilings.Count() > 0)
                {
                    foreach (var comp in ceilings)
                    {
                        if (comp != null)
                        {
                            if ((comp.transform.position.y - elevator.transform.position.y) == 4)
                            {
                                ceiling = comp;
                                return true;
                            }
                        }
                    }
                }
            }
            ceiling = null;
            return false;
        }

        public static bool hasCeilingBelow(StructureComponent elevator, out StructureComponent ceiling)
        {
            if (elevator != null)
            {
                Vector2 vector = new Vector2(elevator.transform.position.x, elevator.transform.position.z);
                StructureComponent[] ceilings = Array.FindAll(childCeilings(elevator._master).ToArray(), (StructureComponent SC) => SC != elevator && new Vector2(SC.transform.position.x, SC.transform.position.z) == vector && SC.transform.position.y == elevator.transform.position.y);
                if (ceilings != null && ceilings.Count() > 0)
                {
                    foreach (var comp in ceilings)
                    {
                        if (elevator.transform.position.y == comp.transform.position.y)
                        {
                            ceiling = comp;
                            return true;
                        }
                    }
                }
            }
            ceiling = null;
            return false;
        }

        public static bool isStandingOnElevator(Character playerChar, out StructureComponent SC)
        {
            Vector3 getPos = playerChar.eyesOrigin;

            StructureComponent[] components = Array.FindAll(playersElevators(playerChar).ToArray(), (StructureComponent comp) => Vector3.Distance(comp.transform.position + new Vector3(0, 5, 0), getPos) <= 2.6);
            if (components != null && components.Count() > 0)
            {
                foreach (var obj in components)
                {
                    if (obj != null)
                    {
                        SC = obj;
                        return true;
                    }
                    else
                    {
                        removeElevator(obj, playerChar, true);
                    }
                }
            }

            SC = null;
            return false;
        }

        public static bool hasOwnership(ulong UID, StructureComponent SC)
        {
            if (elevatorObjects.ContainsKey(UID))
                return elevatorObjects[UID].Contains(SC);
            return false;
        }

        public static void moveElevatorUp(PlayerClient senderClient, Character senderChar, string[] args)
        {
            try
            {
                ulong UID = senderClient.userID;
                int goalDistance;
                int distance = 0;
                if (args.Count() > 1)
                {
                    int multiplier = 1;
                    if (int.TryParse(args[1], out multiplier))
                    {
                        goalDistance = 4 * multiplier;
                        StructureComponent elevatorObj;
                        if (isStandingOnElevator(senderChar, out elevatorObj))
                        {
                            bool isWood = elevatorObj._materialType == StructureMaster.StructureMaterialType.Wood;
                            StructureMaster master = elevatorObj._master;
                            if (hasOwnership(UID, elevatorObj))
                            {
                                if (!Vars.currentlyTeleporting.Contains(senderClient))
                                    Vars.currentlyTeleporting.Add(senderClient);
                                Thread t = new Thread(() =>
                                {
                                    StructureComponent finalInstance = null;
                                    removeElevator(elevatorObj, senderChar, true);
                                    bool hadCeilingAbove = false;
                                    bool ceilingAboveWasElevator = false;
                                    Vector3 ceilingAbovePosition = new Vector3();
                                    Quaternion ceilingAboveRotation = new Quaternion();
                                    StructureComponent ceilingAbove;
                                    if (hasCeilingAbove(elevatorObj, out ceilingAbove)) // Find the ceiling above the current elevator
                                    {
                                        hadCeilingAbove = true;
                                        ceilingAbovePosition = ceilingAbove.transform.position;
                                        ceilingAboveRotation = ceilingAbove.transform.rotation;
                                        if (isElevator(ceilingAbove))
                                        {
                                            removeElevator(ceilingAbove, senderChar, true);
                                            ceilingAboveWasElevator = true; // If the ceiling above us was an elevator, save it
                                        }
                                        if (ceilingAbove.gameObject != null)
                                        {
                                            NetCull.Destroy(ceilingAbove.gameObject); // Destroy the ceiling above us
                                            Thread.Sleep(50);
                                        }
                                    }
                                    bool didSleep = false;
                                    while (distance < goalDistance)
                                    {
                                        if (distance % 4 == 0 && distance > 3)
                                        {
                                            ceilingAbove = NetCull.InstantiateStatic(!isWood ? ";struct_wood_ceiling" : ";struct_metal_ceiling", ceilingAbovePosition, ceilingAboveRotation).GetComponent<StructureComponent>();
                                            if (ceilingAbove != null) // If successfully recreated, restore its StructureMaster instance and make it an elevator if it was one
                                            {
                                                master.AddStructureComponent(ceilingAbove);
                                                if (ceilingAboveWasElevator)
                                                    addElevator(ceilingAbove, senderChar, true);
                                            }
                                            if (hasCeilingAbove(elevatorObj, out ceilingAbove)) // Find the ceiling above the current elevator
                                            {
                                                ceilingAbovePosition = ceilingAbove.transform.position;
                                                ceilingAboveRotation = ceilingAbove.transform.rotation;
                                                if (isElevator(ceilingAbove))
                                                {
                                                    removeElevator(ceilingAbove, senderChar, true);
                                                    ceilingAboveWasElevator = true; // If the ceiling above us was an elevator, save it
                                                }
                                                if (ceilingAbove.gameObject != null)
                                                {
                                                    NetCull.Destroy(ceilingAbove.gameObject); // Destroy the ceiling above us
                                                    Thread.Sleep(50);
                                                    didSleep = true;
                                                }
                                            }
                                        }
                                        List<object> objects = objectsOnElevator(elevatorObj);
                                        Vector3 oldPosition = elevatorObj.transform.position;
                                        Quaternion rotation = elevatorObj.transform.rotation;
                                        if (elevatorObj.gameObject != null)
                                        {
                                            NetCull.Destroy(elevatorObj.gameObject);
                                            Thread.Sleep(50);
                                        }

                                        elevatorObj = NetCull.InstantiateStatic(isWood ? ";struct_wood_ceiling" : ";struct_metal_ceiling", oldPosition + new Vector3(0, 1, 0), rotation).GetComponent<StructureComponent>();
                                        if (elevatorObj != null)
                                        {
                                            finalInstance = elevatorObj;
                                        }

                                        foreach (var obj in objects)
                                        {
                                            if (obj is Character)
                                            {
                                                Character character = (Character)obj;
                                                if (character != null && character.alive && character.netUser != null && character.netUser.networkPlayer != null)
                                                {
                                                    Vector3 destination = new Vector3(character.eyesOrigin.x, elevatorObj.transform.position.y, character.eyesOrigin.z);
                                                    destination.y += 6f;
                                                    simulateTeleport(character.playerClient, destination);
                                                }
                                            }
                                        }
                                        distance++;
                                        Thread.Sleep(didSleep ? 400 : 450);
                                    }
                                    if (finalInstance != null)
                                    {
                                        master.AddStructureComponent(finalInstance);
                                        addElevator(finalInstance, senderChar, true);
                                    }

                                    if (hadCeilingAbove)
                                    {
                                        // Recreate the ceiling we destroyed earlier
                                        ceilingAbove = NetCull.InstantiateStatic(!isWood ? ";struct_wood_ceiling" : ";struct_metal_ceiling", ceilingAbovePosition, ceilingAboveRotation).GetComponent<StructureComponent>();
                                        if (ceilingAbove != null) // If successfully recreated, restore its StructureMaster instance and make it an elevator if it was one
                                        {
                                            master.AddStructureComponent(ceilingAbove);
                                            if (ceilingAboveWasElevator)
                                                addElevator(ceilingAbove, senderChar, true);
                                        }
                                    }
                                });
                                t.IsBackground = true;
                                t.Start();
                                Vars.currentlyTeleporting.Remove(senderClient);
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "You do not own this elevator!");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not standing on an elevator. Make sure there are no obstructions.");
                    }
                }
                else
                {
                    try
                    {
                        goalDistance = 4;
                        StructureComponent elevatorObj;
                        if (isStandingOnElevator(senderChar, out elevatorObj))
                        {
                            bool isWood = elevatorObj._materialType == StructureMaster.StructureMaterialType.Wood;
                            StructureMaster master = elevatorObj._master;
                            if (hasOwnership(UID, elevatorObj))
                            {
                                if (!Vars.currentlyTeleporting.Contains(senderClient))
                                    Vars.currentlyTeleporting.Add(senderClient);
                                Thread t = new Thread(() =>
                                {
                                    try
                                    {
                                        StructureComponent finalInstance = null;
                                        removeElevator(elevatorObj, senderChar, true);
                                        bool hadCeilingAbove = false;
                                        bool ceilingAboveWasElevator = false;
                                        Vector3 ceilingAbovePosition = new Vector3();
                                        Quaternion ceilingAboveRotation = new Quaternion();
                                        StructureComponent ceilingAbove;
                                        if (hasCeilingAbove(elevatorObj, out ceilingAbove)) // Find the ceiling above the current elevator
                                        {
                                            try
                                            {
                                                hadCeilingAbove = true;
                                                ceilingAbovePosition = ceilingAbove.transform.position;
                                                ceilingAboveRotation = ceilingAbove.transform.rotation;
                                                if (isElevator(ceilingAbove))
                                                {
                                                    removeElevator(ceilingAbove, senderChar, true);
                                                    ceilingAboveWasElevator = true; // If the ceiling above us was an elevator, save it
                                                }
                                                if (ceilingAbove.gameObject != null)
                                                {
                                                    NetCull.Destroy(ceilingAbove.gameObject); // Destroy the ceiling above us
                                                    Thread.Sleep(50);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Vars.conLog.Error("MEU #3: " + ex.ToString());
                                            }
                                        }
                                        while (distance < goalDistance)
                                        {
                                            List<object> objects = objectsOnElevator(elevatorObj);
                                            Vector3 oldPosition = elevatorObj.transform.position;
                                            Quaternion rotation = elevatorObj.transform.rotation;

                                            try
                                            {
                                                if (elevatorObj.gameObject != null)
                                                {
                                                    NetCull.Destroy(elevatorObj.gameObject);
                                                    Thread.Sleep(50);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Vars.conLog.Error("MEU #4: " + ex.ToString());
                                            }

                                            elevatorObj = NetCull.InstantiateStatic(isWood ? ";struct_wood_ceiling" : ";struct_metal_ceiling", oldPosition + new Vector3(0, 1, 0), rotation).GetComponent<StructureComponent>();
                                            if (elevatorObj != null)
                                            {
                                                finalInstance = elevatorObj;
                                            }

                                            foreach (var obj in objects)
                                            {
                                                if (obj is Character)
                                                {
                                                    Character character = (Character)obj;
                                                    if (character != null && character.alive && character.netUser != null && character.netUser.networkPlayer != null)
                                                    {
                                                        Vector3 destination = new Vector3(character.eyesOrigin.x, elevatorObj.transform.position.y, character.eyesOrigin.z);
                                                        destination.y += 6f;
                                                        simulateTeleport(character.playerClient, destination);
                                                    }
                                                }
                                            }
                                            distance++;
                                            Thread.Sleep(450);
                                        }
                                        if (finalInstance != null)
                                        {
                                            master.AddStructureComponent(finalInstance);
                                            addElevator(finalInstance, senderChar, true);
                                        }

                                        if (hadCeilingAbove)
                                        {
                                            // Recreate the ceiling we destroyed earlier
                                            ceilingAbove = NetCull.InstantiateStatic(!isWood ? ";struct_wood_ceiling" : ";struct_metal_ceiling", ceilingAbovePosition, ceilingAboveRotation).GetComponent<StructureComponent>();
                                            if (ceilingAbove != null) // If successfully recreated, restore its StructureMaster instance and make it an elevator if it was one
                                            {
                                                master.AddStructureComponent(ceilingAbove);
                                                if (ceilingAboveWasElevator)
                                                    addElevator(ceilingAbove, senderChar, true);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Vars.conLog.Error("MEU #2: " + ex.ToString());
                                    }
                                });
                                t.IsBackground = true;
                                t.Start();
                                Vars.currentlyTeleporting.Remove(senderClient);
                            }
                            else
                                Broadcast.broadcastTo(senderClient.netPlayer, "You do not own this elevator!");
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You are not standing on an elevator. Make sure there are no obstructions.");
                    }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("MEU #1: " + ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("MEU: " + ex.ToString());
            }
        }

        public static void moveElevatorDown(PlayerClient senderClient, Character senderChar, string[] args)
        {
            RustServerManagement RSM = RustServerManagement.Get();
            ulong UID = senderClient.userID;
            int goalDistance;
            int distance = 0;
            if (args.Count() > 1)
            {
                int multiplier = 1;
                if (int.TryParse(args[1], out multiplier))
                {
                    goalDistance = 4 * multiplier;
                    StructureComponent elevatorObj;
                    if (isStandingOnElevator(senderChar, out elevatorObj))
                    {
                        bool isWood = elevatorObj._materialType == StructureMaster.StructureMaterialType.Wood;
                        StructureMaster master = elevatorObj._master;
                        if (hasOwnership(UID, elevatorObj))
                        {
                            if (!Vars.currentlyTeleporting.Contains(senderClient))
                                Vars.currentlyTeleporting.Add(senderClient);
                            Thread t = new Thread(() =>
                            {
                                StructureComponent finalInstance = null;
                                removeElevator(elevatorObj, senderChar, true);
                                bool hadCeilingBelow = false;
                                bool ceilingBelowWasElevator = false;
                                Vector3 ceilingBelowPosition = new Vector3();
                                Quaternion ceilingBelowRotation = new Quaternion();
                                StructureComponent ceilingBelow;
                                if (hasCeilingBelow(elevatorObj, out ceilingBelow)) // Find the ceiling below the current elevator
                                {
                                    hadCeilingBelow = true;
                                    ceilingBelowPosition = ceilingBelow.transform.position;
                                    ceilingBelowRotation = ceilingBelow.transform.rotation;
                                    if (isElevator(ceilingBelow))
                                    {
                                        removeElevator(ceilingBelow, senderChar, true);
                                        ceilingBelowWasElevator = true; // If the ceiling below us was an elevator, save it
                                    }
                                    if (ceilingBelow.gameObject != null)
                                    {
                                        NetCull.Destroy(ceilingBelow.gameObject); // Destroy the ceiling below us
                                        Thread.Sleep(50);
                                    }
                                }
                                bool didSleep = false;
                                while (distance < goalDistance)
                                {
                                    if (distance % 4 == 0 && distance > 3)
                                    {
                                        ceilingBelow = NetCull.InstantiateStatic(!isWood ? ";struct_wood_ceiling" : ";struct_metal_ceiling", ceilingBelowPosition, ceilingBelowRotation).GetComponent<StructureComponent>();
                                        if (ceilingBelow != null) // If successfully recreated, restore its StructureMaster instance and make it an elevator if it was one
                                        {
                                            master.AddStructureComponent(ceilingBelow);
                                            if (ceilingBelowWasElevator)
                                                addElevator(ceilingBelow, senderChar, true);
                                        }
                                        if (hasCeilingBelow(elevatorObj, out ceilingBelow)) // Find the ceiling above the current elevator
                                        {
                                            ceilingBelowPosition = ceilingBelow.transform.position;
                                            ceilingBelowRotation = ceilingBelow.transform.rotation;
                                            if (isElevator(ceilingBelow))
                                            {
                                                removeElevator(ceilingBelow, senderChar, true);
                                                ceilingBelowWasElevator = true; // If the ceiling above us was an elevator, save it
                                            }
                                            if (ceilingBelow.gameObject != null)
                                            {
                                                NetCull.Destroy(ceilingBelow.gameObject);
                                                Thread.Sleep(50);
                                                didSleep = true;
                                            }
                                        }
                                    }
                                    List<object> objects = objectsOnElevator(elevatorObj);
                                    Vector3 oldPosition = elevatorObj.transform.position;
                                    Quaternion rotation = elevatorObj.transform.rotation;
                                    if (elevatorObj.gameObject != null)
                                    {
                                        NetCull.Destroy(elevatorObj.gameObject);
                                        Thread.Sleep(50);
                                    }

                                    elevatorObj = NetCull.InstantiateStatic(isWood ? ";struct_wood_ceiling" : ";struct_metal_ceiling", oldPosition - new Vector3(0, 1, 0), rotation).GetComponent<StructureComponent>();
                                    if (elevatorObj != null)
                                    {
                                        finalInstance = elevatorObj;
                                    }

                                    distance++;
                                    Thread.Sleep(didSleep ? 400 : 450);
                                }
                                if (finalInstance != null)
                                {
                                    master.AddStructureComponent(finalInstance);
                                    addElevator(finalInstance, senderChar, true);
                                }

                                if (hadCeilingBelow)
                                {
                                    // Recreate the ceiling we destroyed earlier
                                    ceilingBelow = NetCull.InstantiateStatic(!isWood ? ";struct_wood_ceiling" : ";struct_metal_ceiling", ceilingBelowPosition, ceilingBelowRotation).GetComponent<StructureComponent>();
                                    if (ceilingBelow != null) // If successfully recreated, restore its StructureMaster instance and make it an elevator if it was one
                                    {
                                        master.AddStructureComponent(ceilingBelow);
                                        if (ceilingBelowWasElevator)
                                            addElevator(ceilingBelow, senderChar, true);
                                    }
                                }
                            });
                            t.IsBackground = true;
                            t.Start();
                            Vars.currentlyTeleporting.Remove(senderClient);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You do not own this elevator!");
                    }
                    else
                        Broadcast.broadcastTo(senderClient.netPlayer, "You are not standing on an elevator. Make sure there are no obstructions.");
                }
            }
            else
            {
                try
                {
                    goalDistance = 4;
                    StructureComponent elevatorObj;
                    if (isStandingOnElevator(senderChar, out elevatorObj))
                    {
                        bool isWood = elevatorObj._materialType == StructureMaster.StructureMaterialType.Wood;
                        StructureMaster master = elevatorObj._master;
                        if (hasOwnership(UID, elevatorObj))
                        {
                            if (!Vars.currentlyTeleporting.Contains(senderClient))
                                Vars.currentlyTeleporting.Add(senderClient);
                            Thread t = new Thread(() =>
                            {
                                try
                                {
                                    StructureComponent finalInstance = null;
                                    removeElevator(elevatorObj, senderChar, true);
                                    bool hadCeilingBelow = false;
                                    bool ceilingBelowWasElevator = false;
                                    Vector3 ceilingBelowPosition = new Vector3();
                                    Quaternion ceilingBelowRotation = new Quaternion();
                                    StructureComponent ceilingBelow;
                                    if (hasCeilingBelow(elevatorObj, out ceilingBelow)) // Find the ceiling below the current elevator
                                    {
                                        try
                                        {
                                            hadCeilingBelow = true;
                                            ceilingBelowPosition = ceilingBelow.transform.position;
                                            ceilingBelowRotation = ceilingBelow.transform.rotation;
                                            if (isElevator(ceilingBelow))
                                            {
                                                removeElevator(ceilingBelow, senderChar, true);
                                                ceilingBelowWasElevator = true; // If the ceiling below us was an elevator, save it
                                            }
                                            if (ceilingBelow.gameObject != null)
                                            {
                                                NetCull.Destroy(ceilingBelow.gameObject); // Destroy the ceiling below us
                                                Thread.Sleep(50);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Vars.conLog.Info("MED #3: " + ex.ToString());
                                        }
                                    }
                                    while (distance < goalDistance)
                                    {
                                        List<object> objects = objectsOnElevator(elevatorObj);
                                        Vector3 oldPosition = elevatorObj.transform.position;
                                        Quaternion rotation = elevatorObj.transform.rotation;
                                        try
                                        {
                                            if (elevatorObj.gameObject != null)
                                            {
                                                NetCull.Destroy(elevatorObj.gameObject);
                                                Thread.Sleep(50);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Vars.conLog.Info("MED #4: " + ex.ToString());
                                        }

                                        elevatorObj = NetCull.InstantiateStatic(isWood ? ";struct_wood_ceiling" : ";struct_metal_ceiling", oldPosition - new Vector3(0, 1, 0), rotation).GetComponent<StructureComponent>();
                                        if (elevatorObj != null)
                                        {
                                            finalInstance = elevatorObj;
                                        }

                                        distance++;
                                        Thread.Sleep(450);
                                    }
                                    if (finalInstance != null)
                                    {
                                        master.AddStructureComponent(finalInstance);
                                        addElevator(finalInstance, senderChar, true);
                                    }

                                    if (hadCeilingBelow)
                                    {
                                        // Recreate the ceiling we destroyed earlier
                                        ceilingBelow = NetCull.InstantiateStatic(!isWood ? ";struct_wood_ceiling" : ";struct_metal_ceiling", ceilingBelowPosition, ceilingBelowRotation).GetComponent<StructureComponent>();
                                        if (ceilingBelow != null) // If successfully recreated, restore its StructureMaster instance and make it an elevator if it was one
                                        {
                                            master.AddStructureComponent(ceilingBelow);
                                            if (ceilingBelowWasElevator)
                                                addElevator(ceilingBelow, senderChar, true);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Vars.conLog.Info("MED #2: " + ex.ToString());
                                }
                            });
                            t.IsBackground = true;
                            t.Start();
                            Vars.currentlyTeleporting.Remove(senderClient);
                        }
                        else
                            Broadcast.broadcastTo(senderClient.netPlayer, "You do not own this elevator!");
                    }
                    else
                        Broadcast.broadcastTo(senderClient.netPlayer, "You are not standing on an elevator. Make sure there are no obstructions.");
                }
                catch (Exception ex)
                {
                    Vars.conLog.Error("MED #1: " + ex.ToString());
                }
            }
        }
    }
}
