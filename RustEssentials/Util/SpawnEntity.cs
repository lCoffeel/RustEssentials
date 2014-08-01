using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RustEssentials.Util
{
    public static class SpawnEntity
    {
        public static void spawnEntity(PlayerClient senderClient, string[] args)
        {
            Character senderChar;
            if (Vars.getPlayerChar(senderClient, out senderChar))
            {
                if (args.Count() > 1)
                {
                    int count = 1;
                    if (args.Count() > 2)
                    {
                        if (!int.TryParse(args[2], out count))
                        {
                            Broadcast.broadcastTo(senderClient.netPlayer, "Entity spawn count must be a number.");
                            return;
                        }
                    }
                    string entityName = getEntity(args[1]);
                    if (entityName != null)
                    {
                        Vector3 position = senderChar.transform.position;
                        Quaternion rotation = new Quaternion(0f, 0f, 0f, 1.0f);
                        if (isLoot(entityName) || isResource(entityName))
                        {
                            RustServerManagement RSM = RustServerManagement.Get();
                            RSM.TeleportPlayerToWorld(senderClient.netPlayer, senderChar.eyesOrigin + new Vector3(0, (isResource(entityName) ? 1 : 0), 0));
                        }
                        if (entityName == "SupplyCrate")
                        {
                            position = senderChar.transform.position + (senderChar.transform.forward * 3);
                            position.y += 3;
                        }
                        for (int i = 0; i < count; i++)
                        {
                            if (isAnimal(entityName))
                            {
                                float randX = UnityEngine.Random.Range(-0.5f, 0.5f);
                                float randZ = UnityEngine.Random.Range(-0.5f, 0.5f);
                                position.x += randX;
                                position.z += randZ;
                            }
                            var obj = NetCull.InstantiateStatic(entityName, position, rotation);
                        }
                    }
                    else
                        Broadcast.broadcastTo(senderClient.netPlayer, "No such spawnable entity named \"" + args[1] + "\"!");
                }
                else
                    Broadcast.broadcastTo(senderClient.netPlayer, "You must specify an entity name!");
            }
        }

        public static bool isAnimal(string entityName)
        {
            List<string> animals = new List<string>()
            {
                "Wolf", "Bear", "MutantWolf", "MutantBear", "Stag_A", "Rabbit_A", "Chicken_A", "Boar_A"
            };
            if (animals.Contains(entityName))
                return true;
            return false;
        }

        public static bool isLoot(string entityName)
        {
            List<string> lootboxes = new List<string>()
            {
                "AmmoLootBox", "MedicalLootBox", "WeaponLootBox", "BoxLoot"
            };
            if (lootboxes.Contains(entityName))
                return true;
            return false;
        }

        public static bool isSack(string entityName)
        {
            List<string> lootboxes = new List<string>()
            {
                "AILootSack", "LootSack"
            };
            if (lootboxes.Contains(entityName))
                return true;
            return false;
        }

        public static bool isResource(string entityName)
        {
            List<string> resources = new List<string>()
            {
                ";res_woodpile", ";res_ore_1", ";res_ore_2", ";res_ore_3"
            };
            if (resources.Contains(entityName))
                return true;
            return false;
        }

        public static string getEntity(string shortName)
        {
            switch (shortName)
            {
                case "wolf":
                    return "Wolf";
                case "bear":
                    return "Bear";
                case "mutantwolf":
                    return "MutantWolf";
                case "mutantbear":
                    return "MutantBear";
                case "deer":
                    return "Stag_A";
                case "rabbit":
                    return "Rabbit_A";
                case "chicken":
                    return "Chicken_A";
                case "boar":
                    return "Boar_A";
                case "pig":
                    return "Boar_A";
                case "ammobox":
                    return "AmmoLootBox";
                case "medbox":
                    return "MedicalLootBox";
                case "weaponbox":
                    return "WeaponLootBox";
                case "box":
                    return "BoxLoot";
                case "crate":
                    return "SupplyCrate";
                case "wood":
                    return ";res_woodpile";
                case "ore1":
                    return ";res_ore_1";
                case "ore2":
                    return ";res_ore_2";
                case "ore3":
                    return ";res_ore_3";
            }
            return null;
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      
    }
}
