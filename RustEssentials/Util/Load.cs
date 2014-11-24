using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

namespace RustEssentials.Util
{
    public class Load
    {
        public void loadRemoverBlacklist()
        {
            try
            {
                if (File.Exists(Vars.removerBlacklistFile))
                {
                    Vars.removerObjectBlacklist.Clear();
                    using (StreamReader sr = new StreamReader(Vars.removerBlacklistFile))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (!line.StartsWith("#"))
                            {
                                if (line.IndexOf("#") > -1)
                                {
                                    line = line.Substring(0, line.IndexOf("#"));
                                }

                                if (!line.Contains("[") && !line.Contains("]"))
                                {
                                    Vars.removerObjectBlacklist.Add(line);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.conLog.Error("LRB: " + ex.ToString());
            }
        }

        public void loadPathConfig()
        {
            try
            {
                if (File.Exists(Vars.pathsFile))
                {
                    using (StreamReader sr = new StreamReader(Vars.pathsFile))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (!line.StartsWith("#"))
                            {
                                if (line.IndexOf("#") > -1)
                                {
                                    line = line.Substring(0, line.IndexOf("#"));
                                }

                                if (line.Contains("="))
                                {
                                    string variable = line.Split('=')[0];
                                    string value = line.Split('=')[1];

                                    switch (variable)
                                    {
                                        case "useDefaultPaths":
                                            bool boolValue;
                                            if (bool.TryParse(value, out boolValue))
                                            {
                                                Vars.useDefaultPaths = boolValue;
                                            }
                                            break;
                                        case "logPath":
                                            if (value.StartsWith(".\\"))
                                                value = Vars.rootDir + value.Substring(1);

                                            if (!Vars.useDefaultPaths)
                                            {
                                                if (value != "")
                                                    Vars.logsDir = Path.Combine(value, "Logs");
                                                else
                                                    Vars.logsDir = Path.Combine(Vars.essentialsDir, "Logs");
                                            }
                                            break;
                                        case "tablesPath":
                                            if (value.StartsWith(".\\"))
                                                value = Vars.rootDir + value.Substring(1);

                                            if (!Vars.useDefaultPaths)
                                            {
                                                if (value != "")
                                                    Vars.tablesDir = Path.Combine(value, "Tables");
                                                else
                                                    Vars.tablesDir = Path.Combine(Vars.essentialsDir, "Tables");
                                            }
                                            break;
                                        case "configPath":
                                            if (value.StartsWith(".\\"))
                                                value = Vars.rootDir + value.Substring(1);

                                            if (!Vars.useDefaultPaths)
                                            {
                                                if (value != "")
                                                    Vars.essentialsDir = Path.Combine(value, "RustEssentials");
                                            }
                                            break;
                                        case "bigBrotherPath":
                                            if (value.StartsWith(".\\"))
                                                value = Vars.rootDir + value.Substring(1);

                                            if (!Vars.useDefaultPaths)
                                            {
                                                if (value != "")
                                                    Vars.bigBrotherDir = Path.Combine(value, "BigBrother");
                                                else
                                                    Vars.bigBrotherDir = Path.Combine(Vars.logsDir, "BigBrother");
                                            }
                                            break;
                                        case "storageLogsPath":
                                            if (value.StartsWith(".\\"))
                                                value = Vars.rootDir + value.Substring(1);

                                            if (!Vars.useDefaultPaths)
                                            {
                                                if (value != "")
                                                    Vars.storageLogsDir = Path.Combine(value, "Storage Logs");
                                                else
                                                    Vars.storageLogsDir = Path.Combine(Vars.bigBrotherDir, "Storage Logs");
                                            }
                                            break;
                                        case "sleeperDeathLogsPath":
                                            if (value.StartsWith(".\\"))
                                                value = Vars.rootDir + value.Substring(1);

                                            if (!Vars.useDefaultPaths)
                                            {
                                                if (value != "")
                                                    Vars.sleeperDeathLogsDir = Path.Combine(value, "Sleeper Death Logs");
                                                else
                                                    Vars.sleeperDeathLogsDir = Path.Combine(Vars.bigBrotherDir, "Sleeper Death Logs");
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Vars.logToFile = false;
                Vars.conLog.Error("LPC: " + ex.ToString());
            }
        }

        private string currentRank = "";
        private string currentPrefix = "";

        public void loadRanks()
        {
            try
            {
                if (File.Exists(Vars.ranksFile))
                {
                    Vars.rankPrefixes.Clear();
                    Vars.rankList.Clear();
                    using (StreamReader sr = new StreamReader(Vars.ranksFile))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (!line.StartsWith("#"))
                            {
                                if (line.IndexOf("#") > -1)
                                {
                                    line = line.Substring(0, line.IndexOf("#"));
                                }

                                line = line.Trim();

                                if (line.StartsWith("[") && line.EndsWith("]"))
                                {
                                    if (line.Contains("."))
                                    {
                                        currentRank = line.Substring(1, line.IndexOf(".") - 1);
                                        currentPrefix = line.Substring(line.IndexOf(".") + 1, line.Length - line.IndexOf(".") - 2);
                                        if (!Vars.rankPrefixes.ContainsKey(currentRank) && !Vars.rankList.ContainsKey(currentRank))
                                        {
                                            Vars.rankPrefixes.Add(currentRank, "[" + currentPrefix + "]");
                                            Vars.rankList.Add(currentRank, new List<ulong>());
                                        }
                                    }
                                    else
                                    {
                                        currentRank = line.Substring(1, line.Length - 2);
                                        if (!Vars.rankList.ContainsKey(currentRank))
                                            Vars.rankList.Add(currentRank, new List<ulong>());
                                    }
                                    Vars.conLog.Info("Creating rank [" + currentRank + "].");
                                }
                                else if (line.Equals("isDefaultRank"))
                                {
                                    Vars.defaultRank = currentRank;
                                }
                                else
                                {
                                    if (line.Length >= 17)
                                    {
                                        if (currentRank != "Member" && currentRank != Vars.defaultRank)
                                        {
                                            ulong UID;
                                            if (Vars.rankList.ContainsKey(currentRank) && ulong.TryParse(line, out UID))
                                                Vars.rankList[currentRank].Add(UID);
                                            Vars.conLog.Info("Adding " + line + " as " + currentRank + ".");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Vars.conLog.Error("LOADR: " + ex.ToString()); }
        }

        public string currentRestriction = "";
        public bool skipRestriction = false;
        public void loadController()
        {
            try
            {
                if (File.Exists(Vars.itemControllerFile))
                {
                    Vars.restrictCrafting.Clear();
                    Vars.restrictResearch.Clear();
                    Vars.restrictBlueprints.Clear();
                    int restrictionCount = 0;
                    using (StreamReader sr = new StreamReader(Vars.itemControllerFile))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (!line.StartsWith("#"))
                            {
                                if (line.IndexOf("#") > -1)
                                {
                                    line = line.Substring(0, line.IndexOf("#"));
                                }

                                line = line.Trim();

                                if (line.StartsWith("[") && line.EndsWith("]"))
                                {
                                    currentRestriction = line;
                                }
                                else
                                {
                                    if (line.Length > 0)
                                    {
                                        if (!Vars.itemIDs.ContainsValue(line))
                                            Vars.conLog.Error("No such item named \"" + line + "\" in section " + currentRestriction + ".");
                                        else
                                        {
                                            switch (currentRestriction)
                                            {
                                                case "[Item Restrictions]":
                                                    if (!Vars.restrictItems.Contains(line))
                                                    {
                                                        Vars.restrictItems.Add(line);
                                                        restrictionCount++;
                                                    }
                                                    break;
                                                case "[Crafting Restrictions]":
                                                    if (!Vars.restrictCrafting.Contains(line))
                                                    {
                                                        Vars.restrictCrafting.Add(line);
                                                        restrictionCount++;
                                                    }
                                                    break;
                                                case "[Research Restrictions]":
                                                    if (!Vars.restrictResearch.Contains(line))
                                                    {
                                                        Vars.restrictResearch.Add(line);
                                                        restrictionCount++;
                                                    }
                                                    break;
                                                case "[Blueprint Restrictions]":
                                                    if (!Vars.restrictBlueprints.Contains(line))
                                                    {
                                                        Vars.restrictBlueprints.Add(line);
                                                        restrictionCount++;
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Vars.conLog.Info("Item Controller successfully loaded with " + restrictionCount + " restrictions!");
                }
            }
            catch (Exception ex) { Vars.conLog.Error("LOADIC: " + ex.ToString()); }
        }

        public void loadTables()
        {
            try
            {
                string currentSection = "";
                bool isSkipping = false;

                if (!Directory.Exists(Vars.tablesDir))
                    Directory.CreateDirectory(Vars.tablesDir);

                List<string> tables = Directory.GetFiles(Vars.tablesDir, "*.ini").ToList();
                int curTables = 0;
                foreach (string table in tables)
                {
                    if (Vars.originalLootTables.ContainsKey(table))
                    {
                        curTables++;
                    }
                }

                if (curTables < Vars.originalLootTables.Count)
                {
                    foreach (KeyValuePair<string, LootSpawnList> lootTable in Vars.originalLootTables)
                    {
                        string tableName = lootTable.Key;
                        string filePath = Path.Combine(Vars.tablesDir, tableName + ".ini");

                        if (!File.Exists(filePath))
                        {
                            using (StreamWriter sw = new StreamWriter(filePath))
                            {
                                sw.WriteLine("[Settings]");
                                sw.WriteLine("# Minimum number of items/tables that can be selected.");
                                sw.WriteLine("minimumSelections=" + lootTable.Value.minPackagesToSpawn);
                                sw.WriteLine("# Maximum number of items/tables that can be selected.");
                                sw.WriteLine("maximumSelections=" + lootTable.Value.maxPackagesToSpawn);
                                sw.WriteLine("# If true, the same item/table can be randomly selected multiple times for spawning.");
                                sw.WriteLine("allowDuplicates=" + (!lootTable.Value.noDuplicates).ToString().ToLower());
                                sw.WriteLine("# If true, all items and tables will be used regardless of probability - there will be no random selection.");
                                sw.WriteLine("useAll=" + lootTable.Value.spawnOneOfEach.ToString().ToLower());
                                sw.WriteLine("");
                                foreach (LootSpawnList.LootWeightedEntry entry in lootTable.Value.LootPackages)
                                {
                                    try
                                    {
                                        if (entry.obj is LootSpawnList)
                                        {
                                            LootSpawnList entryTable = (LootSpawnList)entry.obj;
                                            string entryName = Array.Find(Vars.originalLootTables.ToArray(), (KeyValuePair<string, LootSpawnList> kv) => kv.Value == entryTable).Key;
                                            if (entryName != null)
                                            {
                                                sw.WriteLine("[" + entryName + ".Table]");
                                                sw.WriteLine("probability=" + entry.weight);
                                                sw.WriteLine("minimumAmount=" + entry.amountMin);
                                                sw.WriteLine("maximumAmount=" + entry.amountMax);
                                                sw.WriteLine("");
                                            }
                                        }
                                        else if (entry.obj is ItemDataBlock)
                                        {
                                            ItemDataBlock entryItem = (ItemDataBlock)entry.obj;
                                            string entryName = entryItem.name;

                                            sw.WriteLine("[" + entryName + ".Item]");
                                            sw.WriteLine("probability=" + entry.weight);
                                            sw.WriteLine("minimumAmount=" + entry.amountMin);
                                            sw.WriteLine("maximumAmount=" + entry.amountMax);
                                            sw.WriteLine("");
                                        }
                                    }
                                    catch (Exception ex) { Vars.conLog.Error("Something went wrong when uncovering a loot package: " + ex.ToString()); }
                                }
                            }
                        }
                    }
                }

                tables.Clear();
                tables = Directory.GetFiles(Vars.tablesDir, "*.ini").ToList();
                foreach (string tablePath in tables)
                {
                    string tableName = Path.GetFileNameWithoutExtension(tablePath);
                    if (!DatablockDictionary._lootSpawnLists.ContainsKey(tableName))
                        DatablockDictionary._lootSpawnLists.Add(tableName, ScriptableObject.CreateInstance<LootSpawnList>());
                }

                tables.Clear();
                tables = Directory.GetFiles(Vars.tablesDir, "*.ini").ToList();

                List<LootSpawnList.LootWeightedEntry> newLootPackages = new List<LootSpawnList.LootWeightedEntry>();
                LootSpawnList.LootWeightedEntry lootPackage = new LootSpawnList.LootWeightedEntry();
                int overridedPackages = 0;
                foreach (string tablePath in tables)
                {
                    string fileName = Path.GetFileName(tablePath);
                    string tableName = Path.GetFileNameWithoutExtension(tablePath);
                    if (File.Exists(tablePath))
                    {
                        using (StreamReader sr = new StreamReader(tablePath))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                if (!line.StartsWith("#"))
                                {
                                    if (line.IndexOf("#") > -1)
                                    {
                                        line = line.Substring(0, line.IndexOf("#"));
                                    }

                                    line = line.Trim();

                                    if (line.StartsWith("[") && line.EndsWith("]"))
                                    {
                                        currentSection = line.Trim();
                                        isSkipping = false;
                                        lootPackage = new LootSpawnList.LootWeightedEntry();
                                        if (currentSection == "[Settings]")
                                        {
                                        }
                                        else if (currentSection.EndsWith(".Item]"))
                                        {
                                            try
                                            {
                                                string itemName = currentSection.Substring(1, currentSection.LastIndexOf(".Item]") - 1);
                                                if (Vars.itemIDs.ContainsValue(itemName))
                                                {
                                                    ItemDataBlock item = DatablockDictionary.GetByName(itemName);
                                                    lootPackage.obj = item;
                                                }
                                                else
                                                {
                                                    Vars.conLog.Error("Invalid item name [" + itemName + "] in " + fileName + "!");
                                                    isSkipping = true;
                                                }
                                            }
                                            catch { Vars.conLog.Error("Invalid item section name " + currentSection + " in " + fileName + "!"); isSkipping = true; }
                                        }
                                        else if (currentSection.EndsWith(".Table]"))
                                        {
                                            try
                                            {
                                                string tableSectionName = currentSection.Substring(1, currentSection.LastIndexOf(".Table]") - 1);
                                                if (Vars.originalLootTables.ContainsKey(tableSectionName))
                                                {
                                                    LootSpawnList table = DatablockDictionary.GetLootSpawnListByName(tableSectionName);
                                                    lootPackage.obj = table;
                                                }
                                                else
                                                {
                                                    Vars.conLog.Error("Invalid table name [" + tableSectionName + "] in " + fileName + "!");
                                                    isSkipping = true;
                                                }
                                            }
                                            catch { Vars.conLog.Error("Invalid table section name " + currentSection + " in " + fileName + "!"); isSkipping = true; }
                                        }
                                        else
                                        {
                                            Vars.conLog.Error("Invalid table/item section name " + currentSection + " in " + fileName + "!");
                                            isSkipping = true;
                                        }
                                    }
                                    else
                                    {
                                        if (!isSkipping)
                                        {
                                            if (currentSection == "[Settings]")
                                            {
                                                if (line.Length > 0 && line.Contains("="))
                                                {
                                                    string variableName = line.Split('=')[0];
                                                    string variableValue = line.Split('=')[1];
                                                    if (variableName.Length > 0 && variableValue.Length > 0)
                                                    {
                                                        switch (variableName)
                                                        {
                                                            case "minimumSelections":
                                                                int minimumLoot = Vars.originalLootTables[tableName].minPackagesToSpawn;
                                                                if (int.TryParse(variableValue, out minimumLoot))
                                                                {
                                                                    if (minimumLoot >= 0)
                                                                        DatablockDictionary._lootSpawnLists[tableName].minPackagesToSpawn = minimumLoot;
                                                                    else
                                                                        Vars.conLog.Error("Variable \"minimumLoot\" must be above 0 in " + fileName + "!");
                                                                }
                                                                else
                                                                {
                                                                    Vars.conLog.Error("Could not parse \"minimumLoot\" as an integer in " + fileName + "!");
                                                                }
                                                                break;
                                                            case "maximumSelections":
                                                                int maximumLoot = Vars.originalLootTables[tableName].maxPackagesToSpawn;
                                                                if (int.TryParse(variableValue, out maximumLoot))
                                                                {
                                                                    if (maximumLoot >= 0)
                                                                        DatablockDictionary._lootSpawnLists[tableName].maxPackagesToSpawn = maximumLoot;
                                                                    else
                                                                        Vars.conLog.Error("Variable \"maximumLoot\" must be above 0 in " + fileName + "!");
                                                                }
                                                                else
                                                                {
                                                                    Vars.conLog.Error("Could not parse \"maximumLoot\" as an integer in " + fileName + "!");
                                                                }
                                                                break;
                                                            case "allowDuplicates":
                                                                bool allowDuplicates = !Vars.originalLootTables[tableName].noDuplicates;
                                                                if (bool.TryParse(variableValue, out allowDuplicates))
                                                                {
                                                                    DatablockDictionary._lootSpawnLists[tableName].noDuplicates = !allowDuplicates;
                                                                }
                                                                else
                                                                {
                                                                    Vars.conLog.Error("Could not parse \"allowDuplicates\" as a boolean in " + fileName + "!");
                                                                }
                                                                break;
                                                            case "useAll":
                                                                bool useAll = !Vars.originalLootTables[tableName].spawnOneOfEach;
                                                                if (bool.TryParse(variableValue, out useAll))
                                                                {
                                                                    DatablockDictionary._lootSpawnLists[tableName].spawnOneOfEach = useAll;
                                                                }
                                                                else
                                                                {
                                                                    Vars.conLog.Error("Could not parse \"useAll\" as a boolean in " + fileName + "!");
                                                                }
                                                                break;
                                                            default:
                                                                Vars.conLog.Error("Unfamiliar variable name \"" + variableName + "\" in " + fileName + "!");
                                                                break;
                                                        }
                                                    }
                                                }
                                            }
                                            else if (currentSection.EndsWith(".Item]") || currentSection.EndsWith(".Table]"))
                                            {
                                                if (line.Length > 0 && line.Contains("="))
                                                {
                                                    string variableName = line.Split('=')[0];
                                                    string variableValue = line.Split('=')[1];
                                                    if (variableName.Length > 0 && variableValue.Length > 0)
                                                    {
                                                        switch (variableName)
                                                        {
                                                            case "probability":
                                                                float probability;
                                                                if (float.TryParse(variableValue, out probability))
                                                                {
                                                                    if (probability >= 0)
                                                                        lootPackage.weight = probability;
                                                                    else
                                                                        Vars.conLog.Error("Variable \"probability\" must be above 0 in " + fileName + "!");
                                                                }
                                                                else
                                                                {
                                                                    Vars.conLog.Error("Could not parse \"probability\" as a float in " + fileName + "!");
                                                                }
                                                                break;
                                                            case "minimumAmount":
                                                                int minimum;
                                                                if (int.TryParse(variableValue, out minimum))
                                                                {
                                                                    if (minimum >= 0)
                                                                        lootPackage.amountMin = minimum;
                                                                    else
                                                                        Vars.conLog.Error("Variable \"minimum\" must be above 0 in " + fileName + "!");
                                                                }
                                                                else
                                                                {
                                                                    Vars.conLog.Error("Could not parse \"minimum\" as an integer in " + fileName + "!");
                                                                }
                                                                break;
                                                            case "maximumAmount":
                                                                int maximum;
                                                                if (int.TryParse(variableValue, out maximum))
                                                                {
                                                                    if (maximum >= 0)
                                                                        lootPackage.amountMax = maximum;
                                                                    else
                                                                        Vars.conLog.Error("Variable \"maximum\" must be above 0 in " + fileName + "!");
                                                                }
                                                                else
                                                                {
                                                                    Vars.conLog.Error("Could not parse \"maximum\" as an integer in " + fileName + "!");
                                                                }
                                                                newLootPackages.Add(lootPackage);
                                                                break;
                                                            default:
                                                                Vars.conLog.Error("Unfamiliar variable name \"" + variableName + "\" in " + fileName + "!");
                                                                break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (newLootPackages.Count > 0)
                        {
                            overridedPackages++;
                            DatablockDictionary._lootSpawnLists[tableName].LootPackages = newLootPackages.ToArray();
                        }
                        newLootPackages.Clear();
                    }
                }
                Vars.conLog.Info(overridedPackages + "/" + DatablockDictionary._lootSpawnLists.Count + " loot tables successfully overrided!");
            }
            catch (Exception ex) { Vars.conLog.Error("LOADT: " + ex.ToString()); }
        }

        public void loadDefaultLoadout()
        {
            try
            {
                string currentSection = "";

                if (File.Exists(Vars.defaultLoadoutFile))
                {
                    Vars.defaultLoadout.Clear();
                    using (StreamReader sr = new StreamReader(Vars.defaultLoadoutFile))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (!line.StartsWith("#"))
                            {
                                if (line.IndexOf("#") > -1)
                                {
                                    line = line.Substring(0, line.IndexOf("#"));
                                }

                                line = line.Trim();

                                if (line.StartsWith("[") && line.EndsWith("]"))
                                {
                                    currentSection = line.Substring(1, line.Length - 2);
                                    Vars.defaultLoadout.Add(currentSection, new List<Items.Item>());
                                    Vars.conLog.Info("Loading items for loadout part [" + currentSection + "]...");
                                }
                                else
                                {
                                    line = line.Trim();
                                    if (line.Contains(":"))
                                    {
                                        string itemName = line.Split(':')[0];
                                        int modSlots = -1;
                                        if (itemName.Contains("(") && itemName.EndsWith(")"))
                                        {
                                            string modSlotsStr = itemName.Substring(itemName.IndexOf("(")).Replace("(", "").Replace(")", "");
                                            itemName = itemName.Substring(0, itemName.IndexOf("("));
                                            int.TryParse(modSlotsStr, out modSlots);
                                        }
                                        string amount = line.Split(':')[1];
                                        int uses = -1;
                                        if (amount.Contains("(") && amount.EndsWith(")"))
                                        {
                                            string usesStr = amount.Substring(amount.IndexOf("(")).Replace("(", "").Replace(")", "");
                                            amount = amount.Substring(0, amount.IndexOf("("));
                                            int.TryParse(usesStr, out uses);
                                        }
                                        List<string> currentMods = new List<string>();
                                        List<ItemModDataBlock> mods = new List<ItemModDataBlock>();
                                        if (line.Split(':').Length > 2)
                                        {
                                            string modsStr = line.Split(':')[2];
                                            string[] modNames = modsStr.Split(';');
                                            foreach (var modName in modNames)
                                            {
                                                var modDBVar = DatablockDictionary.GetByName(modName);
                                                if (modDBVar != null && modDBVar is ItemModDataBlock)
                                                {
                                                    var modDB = (ItemModDataBlock)modDBVar;
                                                    if (!currentMods.Contains(modName))
                                                    {
                                                        mods.Add(modDB);
                                                        currentMods.Add(modName);
                                                    }
                                                }
                                            }
                                        }

                                        try
                                        {
                                            ItemDataBlock itemData = DatablockDictionary.GetByName(itemName);
                                            if (itemData != null)
                                            {
                                                int intAmount;
                                                if (int.TryParse(amount, out intAmount))
                                                {
                                                    if (Items.allGuns.Contains(itemName))
                                                        Vars.defaultLoadout[currentSection].Add(new Items.Item(itemName, intAmount, uses, modSlots, mods.ToArray()));
                                                    else
                                                        Vars.defaultLoadout[currentSection].Add(new Items.Item(itemName, intAmount));
                                                }
                                                else
                                                    Vars.conLog.Error("\"" + itemName + "\" [" + currentSection + "] has an improper amount value.");
                                            }
                                            else
                                            {
                                                Vars.conLog.Error("\"" + itemName + "\" [" + currentSection + "] is not a valid item name.");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Vars.conLog.Error("Something went wrong when loading loadout part [" + currentSection + "]. Skipping...");
                                            isSkipping = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Vars.conLog.Error("LOADL: " + ex.ToString()); }
        }

        public void loadPrefixes()
        {
            try
            {
                if (File.Exists(Vars.prefixFile))
                {
                    Vars.playerPrefixes.Clear();
                    using (StreamReader sr = new StreamReader(Vars.prefixFile))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (!line.StartsWith("#"))
                            {
                                if (line.IndexOf("#") > -1)
                                {
                                    line = line.Substring(0, line.IndexOf("#"));
                                }

                                line = line.Trim();

                                if (line.Contains(":"))
                                {
                                    string UIDString = line.Split(':')[0];
                                    string prefix = line.Split(':')[1];
                                    ulong UID;
                                    if (ulong.TryParse(UIDString, out UID))
                                    {
                                        if (!Vars.playerPrefixes.ContainsKey(UID))
                                            Vars.playerPrefixes.Add(UID, prefix);
                                    }
                                }
                                else
                                {
                                    string UIDString = line.Trim();
                                    
                                    ulong UID;
                                    if (ulong.TryParse(UIDString, out UID))
                                    {
                                        if (!Vars.emptyPrefixes.Contains(UID))
                                            Vars.emptyPrefixes.Add(UID);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Vars.conLog.Error("LOADP: " + ex.ToString()); }
        }

        private string currentKitID = "";
        private bool isSkippingDonorKit = false;
        public void loadDonorKits()
        {
            try
            {
                if (File.Exists(Vars.donorKitsFile))
                {
                    Vars.donorKits.Clear();
                    using (StreamReader sr = new StreamReader(Vars.donorKitsFile))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (!line.StartsWith("#"))
                            {
                                if (line.IndexOf("#") > -1)
                                {
                                    line = line.Substring(0, line.IndexOf("#"));
                                }

                                line = line.Trim();

                                if (line.StartsWith("[") && line.EndsWith("]"))
                                {
                                    currentKitID = line.Substring(1, line.Length - 2);
                                    if (!Vars.donorKits.ContainsKey(currentKitID))
                                    {
                                        Vars.conLog.Info("Loading donor kit [" + currentKitID + "]...");
                                        Vars.donorKits.Add(currentKitID, new List<Items.Item>());
                                        isSkippingDonorKit = false;
                                    }
                                    else
                                    {
                                        Vars.conLog.Info("Donor kit [" + currentKitID + "] already exists! Skipping...");
                                        isSkippingDonorKit = true;
                                    }
                                }
                                else
                                {
                                    if (line.Contains(":") && !isSkippingDonorKit)
                                    {
                                        string itemName = line.Split(':')[0];
                                        int modSlots = -1;
                                        if (itemName.Contains("(") && itemName.EndsWith(")"))
                                        {
                                            string modSlotsStr = itemName.Substring(itemName.IndexOf("(")).Replace("(", "").Replace(")", "");
                                            itemName = itemName.Substring(0, itemName.IndexOf("("));
                                            int.TryParse(modSlotsStr, out modSlots);
                                        }
                                        string amount = line.Split(':')[1];
                                        int uses = -1;
                                        if (amount.Contains("(") && amount.EndsWith(")"))
                                        {
                                            string usesStr = amount.Substring(amount.IndexOf("(")).Replace("(", "").Replace(")", "");
                                            amount = amount.Substring(0, amount.IndexOf("("));
                                            int.TryParse(usesStr, out uses);
                                        }
                                        List<string> currentMods = new List<string>();
                                        List<ItemModDataBlock> mods = new List<ItemModDataBlock>();
                                        if (line.Split(':').Length > 2)
                                        {
                                            string modsStr = line.Split(':')[2];
                                            string[] modNames = modsStr.Split(';');
                                            foreach (var modName in modNames)
                                            {
                                                var modDBVar = DatablockDictionary.GetByName(modName);
                                                if (modDBVar != null && modDBVar is ItemModDataBlock)
                                                {
                                                    var modDB = (ItemModDataBlock)modDBVar;
                                                    if (!currentMods.Contains(modName))
                                                    {
                                                        mods.Add(modDB);
                                                        currentMods.Add(modName);
                                                    }
                                                }
                                            }
                                        }

                                        try
                                        {
                                            ItemDataBlock itemData = DatablockDictionary.GetByName(itemName);
                                            if (itemData != null)
                                            {
                                                try
                                                {
                                                    int itemAmount = Convert.ToInt16(amount);
                                                    if (Items.allGuns.Contains(itemName))
                                                        Vars.donorKits[currentKitID].Add(new Items.Item(itemName, itemAmount, uses, modSlots, mods.ToArray()));
                                                    else
                                                        Vars.donorKits[currentKitID].Add(new Items.Item(itemName, itemAmount));
                                                }
                                                catch (Exception ex)
                                                {
                                                    Vars.conLog.Error("Something went wrong when loading donor kit [" + currentKitID + "]. Skipping...");
                                                    isSkippingDonorKit = true;
                                                }
                                            }
                                            else
                                            {
                                                Vars.conLog.Error("\"" + itemName + "\" [" + currentKitID + "] is not a valid item name.");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Vars.conLog.Error("Something went wrong when loading donor kit [" + currentKitID + "]. Skipping...");
                                            isSkippingDonorKit = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Vars.conLog.Error("LOADD: " + ex.ToString()); }
        }

        private string currentDecaySection = "";
        public void loadDecay()
        {
            try
            {
                if (File.Exists(Vars.decayFile))
                {
                    Vars.decayIntervals.Clear();
                    using (StreamReader sr = new StreamReader(Vars.decayFile))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (!line.StartsWith("#"))
                            {
                                if (line.IndexOf("#") > -1)
                                {
                                    line = line.Substring(0, line.IndexOf("#"));
                                }

                                line = line.Trim();

                                if (line.StartsWith("[") && line.EndsWith("]"))
                                {
                                    currentDecaySection = line.Substring(1, line.Length - 2);
                                    Vars.conLog.Info("Loading [" + currentDecaySection + "]...");
                                }
                                else
                                {
                                    if (currentDecaySection == "Overall")
                                    {
                                        if (line.Contains("="))
                                        {
                                            string variableName = line.Split('=')[0];
                                            string value = line.Split('=')[1];
                                            long valueLong;
                                            bool valueBool;
                                            if (long.TryParse(value, out valueLong))
                                            {
                                                switch (variableName)
                                                {
                                                    case "decayObjectInterval":
                                                        Vars.decayObjectInterval = valueLong;
                                                        break;
                                                    case "decayStructureInterval":
                                                        Vars.decayStructureInterval = valueLong * 1000;
                                                        break;
                                                    case "decayWoodDelayInterval":
                                                        Vars.decayWoodDelayInterval = valueLong;
                                                        break;
                                                    case "decayMetalDelayInterval":
                                                        Vars.decayMetalDelayInterval = valueLong;
                                                        break;
                                                }
                                            }
                                            else if (bool.TryParse(value, out valueBool))
                                            {
                                                switch (variableName)
                                                {
                                                    case "enableDecay":
                                                        Vars.enableDecay = valueBool;
                                                        break;
                                                    case "enableCustomDecay":
                                                        Vars.enableCustomDecay = valueBool;
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                    else if (currentDecaySection == "Individual")
                                    {
                                        if (line.Contains(":"))
                                        {
                                            string itemName = line.Split(':')[0];
                                            string decayInterval = line.Split(':')[1];

                                            ItemDataBlock itemData = DatablockDictionary.GetByName(itemName);
                                            if (itemData != null)
                                            {
                                                long decayIntervalLong;
                                                if (long.TryParse(decayInterval, out decayIntervalLong))
                                                {
                                                    if (!Vars.decayIntervals.ContainsKey(itemName))
                                                    {
                                                        Vars.decayIntervals.Add(itemName, decayIntervalLong * (Vars.objectNames.Contains(itemName) ? 1 : 1000));
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Vars.conLog.Error("\"" + itemName + "\" is not a valid item name.");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Vars.conLog.Error("LOADD: " + ex.ToString()); }
        }

        public void loadCommands()
        {
            try
            {
                if (File.Exists(Vars.commandsFile))
                {
                    Vars.enabledCommands.Clear();
                    Vars.totalCommands.Clear();
                    using (StreamReader sr = new StreamReader(Vars.commandsFile))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (!line.StartsWith("#"))
                            {
                                if (line.IndexOf("#") > -1)
                                {
                                    line = line.Substring(0, line.IndexOf("#"));
                                }

                                line = line.Trim();

                                if (line.StartsWith("[") && line.EndsWith("]"))
                                {
                                    currentRank = line.Substring(1, line.Length - 2);
                                    if (!Vars.enabledCommands.ContainsKey(currentRank))
                                    {
                                        Vars.enabledCommands.Add(currentRank, new List<string>());
                                        Vars.conLog.Info("Adding commands for [" + currentRank + "]...");
                                    }
                                    else
                                    {
                                        Vars.conLog.Error("Rank [" + currentRank + "] already exists!");
                                    }
                                }
                                else
                                {
                                    if (line.StartsWith("/"))
                                    {
                                        if (!Vars.totalCommands.Contains(line))
                                        {
                                            if (Vars.enabledCommands.ContainsKey(currentRank) && !Vars.enabledCommands[currentRank].Contains(line))
                                                Vars.enabledCommands[currentRank].Add(line);

                                            Vars.totalCommands.Add(line);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                inheritCommands();
                Vars.callAPI("RustEssentialsAPI.Commands", "Reload", false);
            }
            catch (Exception ex) { Vars.conLog.Error("LOADC: " + ex.ToString()); }
        }

        public void inheritCommands()
        {
            if (Vars.inheritCommands)
            {
                foreach (KeyValuePair<string, List<string>> kv in Vars.enabledCommands)
                {
                    int indexOf = Vars.enabledCommands.Keys.ToList().IndexOf(kv.Key);
                    foreach (KeyValuePair<string, List<string>> nkv in Vars.enabledCommands)
                    {
                        int newIndexOf = Vars.enabledCommands.Keys.ToList().IndexOf(nkv.Key);
                        if (newIndexOf > indexOf)
                        {
                            foreach (string s in nkv.Value)
                            {
                                if (!Vars.enabledCommands[kv.Key].Contains(s))
                                    Vars.enabledCommands[kv.Key].Add(s);
                            }
                        }
                    }
                }
            }
        }

        public string currentKit = "";
        public bool isSkipping = false;
        public void loadKits()
        {
            try
            {
                if (File.Exists(Vars.kitsFile))
                {
                    Vars.kitCooldowns.Clear();
                    Vars.kits.Clear();
                    Vars.kitsForRanks.Clear();
                    Vars.kitsForUIDs.Clear();
                    Vars.unassignedKits.Clear();
                    Vars.TRSItems.Clear();
                    Vars.RSItems.Clear();
                    foreach (KeyValuePair<string, string> kv in Vars.rankPrefixes)
                    {
                        if (kv.Key != Vars.defaultRank)
                        {
                            Vars.kitsForRanks.Add(kv.Key, new List<string>());
                        }
                    }
                    using (StreamReader sr = new StreamReader(Vars.kitsFile))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (!line.StartsWith("#"))
                            {
                                if (line.IndexOf("#") > -1)
                                {
                                    line = line.Substring(0, line.IndexOf("#"));
                                }

                                if (line.StartsWith("[") && (line.EndsWith("]") || line.EndsWith("]*")))
                                {
                                    string lastChar = line[line.Length - 1].ToString();
                                    if (line.Contains("."))
                                    {
                                        currentKit = line.Substring(1, line.IndexOf(".") - 1);
                                        string prefix = line.Substring(line.IndexOf(".") + 1, line.Length - line.IndexOf(".") - 2);
                                        string rank = "";
                                        foreach(KeyValuePair<string, string> kv in Vars.rankPrefixes)
                                        {
                                            if (kv.Value == "[" + prefix + "]")
                                            {
                                                rank = kv.Key;
                                            }
                                        }
                                        if (Vars.rankPrefixes.ContainsKey(rank))
                                        {
                                            if (lastChar == "*")
                                                Vars.noninheritedKits.Add(currentKit.ToLower());
                                            Vars.kitsForRanks[rank].Add(currentKit.ToLower());
                                            Vars.kits.Add(currentKit.ToLower(), new List<Items.Item>());
                                            isSkipping = false;
                                            Vars.conLog.Info("Loading items for kit [" + currentKit + "]...");
                                        }
                                        else
                                        {
                                            ulong UID;
                                            if (prefix.Length == 17 && ulong.TryParse(prefix, out UID))
                                            {
                                                if (!Vars.kitsForUIDs.ContainsKey(UID))
                                                    Vars.kitsForUIDs.Add(UID, new List<string>() { { currentKit.ToLower() } });
                                                else
                                                    Vars.kitsForUIDs[UID].Add(currentKit.ToLower());

                                                if (!Vars.kits.ContainsKey(currentKit.ToLower()))
                                                    Vars.kits.Add(currentKit.ToLower(), new List<Items.Item>());

                                                isSkipping = false;
                                                Vars.conLog.Info("Loading items for kit [" + currentKit + "] for user [" + UID + "]...");
                                            }
                                            else
                                            {
                                                Vars.conLog.Error("No such rank prefix " + prefix + ". Skipping kit [" + currentKit + "]...");
                                                isSkipping = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        currentKit = line.Substring(1, line.Length - 2);
                                        if (currentKit.ToLower() != "trs" && currentKit.ToLower() != "rs")
                                        {
                                            Vars.kits.Add(currentKit.ToLower(), new List<Items.Item>());
                                            Vars.unassignedKits.Add(currentKit.ToLower());
                                        }
                                        isSkipping = false;
                                        Vars.conLog.Info("Loading items for kit [" + currentKit + "]...");
                                    }
                                }
                                else
                                {
                                    line = line.Trim();
                                    if (line.Contains(":") && !isSkipping)
                                    {
                                        string itemName = line.Split(':')[0];
                                        int modSlots = -1;
                                        if (itemName.Contains("(") && itemName.EndsWith(")"))
                                        {
                                            string modSlotsStr = itemName.Substring(itemName.IndexOf("(")).Replace("(", "").Replace(")", "");
                                            itemName = itemName.Substring(0, itemName.IndexOf("("));
                                            int.TryParse(modSlotsStr, out modSlots);
                                        }
                                        string amount = line.Split(':')[1];
                                        int uses = -1;
                                        if (amount.Contains("(") && amount.EndsWith(")"))
                                        {
                                            string usesStr = amount.Substring(amount.IndexOf("(")).Replace("(", "").Replace(")", "");
                                            amount = amount.Substring(0, amount.IndexOf("("));
                                            int.TryParse(usesStr, out uses);
                                        }
                                        List<string> currentMods = new List<string>();
                                        List<ItemModDataBlock> mods = new List<ItemModDataBlock>();
                                        if (line.Split(':').Length > 2)
                                        {
                                            string modsStr = line.Split(':')[2];
                                            string[] modNames = modsStr.Split(';');
                                            foreach (var modName in modNames)
                                            {
                                                var modDBVar = DatablockDictionary.GetByName(modName);
                                                if (modDBVar != null && modDBVar is ItemModDataBlock)
                                                {
                                                    var modDB = (ItemModDataBlock)modDBVar;
                                                    if (!currentMods.Contains(modName))
                                                    {
                                                        mods.Add(modDB);
                                                        currentMods.Add(modName);
                                                    }
                                                }
                                            }
                                        }

                                        try
                                        {
                                            ItemDataBlock itemData = DatablockDictionary.GetByName(itemName);
                                            if (itemData != null)
                                            {
                                                try
                                                {
                                                    int itemAmount = Convert.ToInt16(amount);
                                                    if (currentKit.ToLower() == "trs")
                                                    {
                                                        if (Items.allGuns.Contains(itemName))
                                                            Vars.TRSItems.Add(new Items.Item(itemName, itemAmount, uses, modSlots, mods.ToArray()));
                                                        else
                                                            Vars.TRSItems.Add(new Items.Item(itemName, itemAmount));
                                                    }
                                                    else if (currentKit.ToLower() == "rs")
                                                    {
                                                        if (Items.allGuns.Contains(itemName))
                                                            Vars.RSItems.Add(new Items.Item(itemName, itemAmount, uses, modSlots, mods.ToArray()));
                                                        else
                                                            Vars.RSItems.Add(new Items.Item(itemName, itemAmount));
                                                    }
                                                    else
                                                    {
                                                        if (Items.allGuns.Contains(itemName))
                                                            Vars.kits[currentKit.ToLower()].Add(new Items.Item(itemName, itemAmount, uses, modSlots, mods.ToArray()));
                                                        else
                                                            Vars.kits[currentKit.ToLower()].Add(new Items.Item(itemName, itemAmount));
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Vars.conLog.Error("Something went wrong when loading kit [" + currentKit + "]. Skipping...");
                                                    isSkipping = true;
                                                }
                                            }
                                            else
                                            {
                                                Vars.conLog.Error("\"" + itemName + "\" [" + currentKit + "] is not a valid item name.");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Vars.conLog.Error("Something went wrong when loading kit [" + currentKit + "]. Skipping...");
                                            isSkipping = true;
                                        }
                                    }
                                    else if (line.StartsWith("cooldown=") && !isSkipping)
                                    {
                                        try
                                        {
                                            string cooldown = line.Split('=')[1];
                                            if (cooldown != "-1")
                                            {
                                                int multiplier = 1000;
                                                if (cooldown.EndsWith("m"))
                                                    multiplier *= 60;
                                                if (cooldown.EndsWith("h"))
                                                    multiplier *= 3600;
                                                cooldown = cooldown.Remove(cooldown.Length - 1);

                                                Vars.kitCooldowns.Add(currentKit.ToLower(), Convert.ToInt64(cooldown) * multiplier);
                                                Vars.conLog.Info("Time: " + (Convert.ToInt64(cooldown) * multiplier));
                                            }
                                            else
                                            {
                                                Vars.kitCooldowns.Add(currentKit.ToLower(), -1);
                                                Vars.conLog.Info("Time: Infinite");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Vars.conLog.Error("Something went wrong when loading kit [" + currentKit + "]. Skipping...");
                                            isSkipping = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (Vars.inheritKits)
                    inheritKits();
            }
            catch (Exception ex) { Vars.conLog.Error("LOADK: " + ex.ToString()); }
        }

        public void inheritKits()
        {
            foreach (KeyValuePair<string, List<string>> kv in Vars.kitsForRanks)
            {
                foreach (KeyValuePair<string, List<string>> nkv in Vars.kitsForRanks)
                {
                    if (Checks.ofLowerRank(nkv.Key, kv.Key))
                    {
                        foreach (string s in nkv.Value)
                        {
                            if (!Vars.noninheritedKits.Contains(s))
                                Vars.kitsForRanks[kv.Key].Add(s);
                        }
                    }
                }
                foreach (KeyValuePair<string, List<Items.Item>> kv2 in Vars.kits)
                {
                    if (Vars.unassignedKits.Contains(kv2.Key))
                    {
                            Vars.kitsForRanks[kv.Key].Add(kv2.Key);
                    }
                }
            }
            Vars.conLog.Info("Kits inherited for each rank successfully!");
        }

        public string currentWarp = "";
        public bool isSkippingWarp = false;
        public void loadWarps()
        {
            try
            {
                if (File.Exists(Vars.warpsFile))
                {
                    Vars.warpCooldowns.Clear();
                    Vars.warps.Clear();
                    Vars.warpsForRanks.Clear();
                    Vars.warpsForUIDs.Clear();
                    Vars.unassignedWarps.Clear();
                    foreach (KeyValuePair<string, string> kv in Vars.rankPrefixes)
                    {
                        if (kv.Key != Vars.defaultRank)
                        {
                            Vars.warpsForRanks.Add(kv.Key, new List<string>());
                        }
                    }
                    using (StreamReader sr = new StreamReader(Vars.warpsFile))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (!line.StartsWith("#"))
                            {
                                if (line.IndexOf("#") > -1)
                                {
                                    line = line.Substring(0, line.IndexOf("#"));
                                }

                                if (line.StartsWith("[") && line.EndsWith("]"))
                                {
                                    string lastChar = line[line.Length - 1].ToString();
                                    if (line.Contains("."))
                                    {
                                        string noBrackets = line.Substring(1, line.Length - 2);
                                        currentWarp = line.Substring(1, line.LastIndexOf(".") - 1);
                                        string prefix = noBrackets.Substring(noBrackets.LastIndexOf(".") + 1);
                                        string rank = "";
                                        foreach (KeyValuePair<string, string> kv in Vars.rankPrefixes)
                                        {
                                            if (kv.Value == "[" + prefix + "]")
                                            {
                                                rank = kv.Key;
                                            }
                                        }
                                        if (Vars.rankPrefixes.ContainsKey(rank))
                                        {
                                            if (lastChar == "*")
                                                Vars.noninheritedWarps.Add(currentWarp.ToLower());

                                            if (!Vars.warpsForRanks[rank].Contains(currentWarp.ToLower()))
                                                Vars.warpsForRanks[rank].Add(currentWarp.ToLower());

                                            if (!Vars.warps.ContainsKey(currentWarp.ToLower()))
                                                Vars.warps.Add(currentWarp.ToLower(), new Vector3());
                                            isSkippingWarp = false;
                                            Vars.conLog.Info("Loading location for warp [" + currentWarp + "]...");
                                        }
                                        else
                                        {
                                            ulong UID;
                                            if (prefix.Length == 17 && ulong.TryParse(prefix, out UID))
                                            {
                                                if (!Vars.warpsForUIDs.ContainsKey(UID))
                                                    Vars.warpsForUIDs.Add(UID, new List<string>() { { currentWarp.ToLower() } });
                                                else
                                                    Vars.warpsForUIDs[UID].Add(currentWarp.ToLower());

                                                if (!Vars.warps.ContainsKey(currentWarp.ToLower()))
                                                    Vars.warps.Add(currentWarp.ToLower(), new Vector3());
                                                isSkippingWarp = false;
                                                Vars.conLog.Info("Loading location for warp [" + currentWarp + "] for user [" + UID + "]...");
                                            }
                                            else
                                            {
                                                Vars.conLog.Error("No such rank prefix " + prefix + ". Skipping warp [" + currentWarp + "]...");
                                                isSkippingWarp = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        currentWarp = line.Substring(1, line.Length - 2);
                                        if (!Vars.warps.ContainsKey(currentWarp.ToLower()))
                                            Vars.warps.Add(currentWarp.ToLower(), new Vector3());
                                        if (!Vars.unassignedWarps.Contains(currentWarp.ToLower()))
                                            Vars.unassignedWarps.Add(currentWarp.ToLower());
                                        isSkippingWarp = false;
                                        Vars.conLog.Info("Loading location for warp [" + currentWarp + "]...");
                                    }
                                }
                                else
                                {
                                    line = line.Trim();
                                    if (line.Contains("(") && line.Contains(",") && line.Contains(")") && !isSkipping)
                                    {
                                        line = line.Replace("(", "").Replace(")", "").Replace(" ", "");
                                        string xposStr = line.Split(',')[0];
                                        string yposStr = line.Split(',')[1];
                                        string zposStr = line.Split(',')[2];
                                        float xpos;
                                        float ypos;
                                        float zpos;

                                        if (float.TryParse(xposStr, out xpos) && float.TryParse(yposStr, out ypos) && float.TryParse(zposStr, out zpos))
                                        {
                                            Vector3 warpLocation = new Vector3(xpos, ypos, zpos);
                                            Vars.warps[currentWarp.ToLower()] = warpLocation;
                                        }
                                        else
                                        {
                                            Vars.conLog.Error("Something went wrong when loading warp [" + currentWarp + "]. Skipping...");
                                            isSkippingWarp = true;
                                        }
                                    }
                                    else if (line.StartsWith("cooldown=") && !isSkipping)
                                    {
                                        try
                                        {
                                            string cooldown = line.Split('=')[1];

                                            if (cooldown != "-1")
                                            {
                                                int multiplier = 1000;
                                                if (cooldown.EndsWith("m"))
                                                    multiplier *= 60;
                                                if (cooldown.EndsWith("h"))
                                                    multiplier *= 3600;
                                                cooldown = cooldown.Remove(cooldown.Length - 1);

                                                Vars.warpCooldowns.Add(currentWarp.ToLower(), Convert.ToInt64(cooldown) * multiplier);
                                                Vars.conLog.Info("Time: " + (Convert.ToInt64(cooldown) * multiplier));
                                            }
                                            else
                                            {
                                                Vars.warpCooldowns.Add(currentWarp.ToLower(), -1);
                                                Vars.conLog.Info("Time: Infinite");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Vars.conLog.Error("Something went wrong when loading warp [" + currentWarp + "]. Skipping...");
                                            isSkipping = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (Vars.inheritWarps)
                    inheritWarps();
            }
            catch (Exception ex) { Vars.conLog.Error("LOADW: " + ex.ToString()); }
        }

        public void inheritWarps()
        {
            foreach (KeyValuePair<string, List<string>> kv in Vars.warpsForRanks)
            {
                foreach (KeyValuePair<string, List<string>> nkv in Vars.warpsForRanks)
                {
                    if (Checks.ofLowerRank(nkv.Key, kv.Key))
                    {
                        foreach (string s in nkv.Value)
                        {
                            if (!Vars.noninheritedWarps.Contains(s))
                            {
                                if (!Vars.warpsForRanks[kv.Key].Contains(s))
                                    Vars.warpsForRanks[kv.Key].Add(s);
                            }
                        }
                    }
                }
                foreach (KeyValuePair<string, Vector3> kv2 in Vars.warps)
                {
                    if (Vars.unassignedWarps.Contains(kv2.Key))
                    {
                        if (!Vars.warpsForRanks[kv.Key].Contains(kv2.Key))
                            Vars.warpsForRanks[kv.Key].Add(kv2.Key);
                    }
                }
            }
            Vars.conLog.Info("Warps inherited for each rank successfully!");
        }

        public void loadItems()
        {
            using (StreamWriter sw = new StreamWriter(Vars.itemsFile))
            {

                int curIndex = 0;
                foreach (var item in DatablockDictionary.All)
                {
                    curIndex++;
                    Vars.itemIDs.Add(curIndex, item.name);

                    sw.WriteLine(curIndex + ": " + item.name);
                }

                Vars.conLog.Info(curIndex + " items found!");
            }
        }

        private string currentMode = "";
        private int currentInstance = 0;
        public void loadMOTD()
        {
            
            try
            {
                if (File.Exists(Vars.motdFile))
                {
                    Vars.motdList.Clear();
                    Vars.cycleMOTDList.Clear();
                    Vars.onceMOTDList.Clear();
                    Vars.listMOTDList.Clear();
                    foreach (var kv in Vars.cycleMOTDTimers)
                    {
                        TimerPlus t = kv.Value;
                        t.dispose();
                    }
                    Vars.cycleMOTDTimers.Clear();
                    foreach (var kv in Vars.onceMOTDTimers)
                    {
                        TimerPlus t = kv.Value;
                        t.dispose();
                    }
                    Vars.onceMOTDTimers.Clear();
                    foreach (var kv in Vars.listMOTDTimers)
                    {
                        TimerPlus t = kv.Value;
                        t.dispose();
                    }
                    Vars.listMOTDTimers.Clear();
                    using (StreamReader sr = new StreamReader(Vars.motdFile))
                    {
                        int lineNumber = 0;
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            lineNumber++;
                            line = line.Trim();
                            if (!line.StartsWith("#"))
                            {
                                if (line.IndexOf("#") > -1)
                                {
                                    string newString = line.Substring(0, line.IndexOf("#"));
                                    if (newString.EndsWith("[color "))
                                    {
                                        while (newString.Contains("#"))
                                        {
                                            int index = newString.IndexOf("#");
                                            if ((newString.Substring(0, index).EndsWith("[color ")))
                                            {
                                                newString = newString.Substring(0, index) + newString.Substring(index + 1);
                                            }
                                            else
                                            {
                                                line = line.Substring(0, index);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        line = newString;
                                }

                                if (line.StartsWith("[") && line.EndsWith("]"))
                                {
                                    try
                                    {
                                        if (line.Contains("."))
                                        {
                                            currentMode = line.Substring(1, line.IndexOf(".") - 1);
                                            string interval = line.Substring(line.IndexOf(".") + 1, line.Length - line.IndexOf(".") - 2);
                                            int multiplier = 1000;
                                            if (interval.EndsWith("m"))
                                                multiplier *= 60;
                                            if (interval.EndsWith("h"))
                                                multiplier *= 3600;
                                            if (currentMode == "Cycle")
                                            {
                                                int instances = Vars.cycleMOTDList.Count + 1;
                                                currentInstance = instances;
                                                Vars.conLog.Info("Adding MOTD [" + currentMode + "]...");
                                                try
                                                {
                                                    Vars.cycleMOTDList.Add(new MOTD(currentMode + instances, (Convert.ToInt16(interval.Remove(interval.Length - 1)) * multiplier).ToString(), new List<string>()));
                                                }
                                                catch (Exception ex)
                                                {
                                                    Vars.conLog.Error("Cycle Interval must be an integer! Defaulting to 15 minutes...");
                                                    Vars.cycleMOTDList.Add(new MOTD(currentMode + instances, "900000", new List<string>()));
                                                }
                                            }
                                            else if (currentMode == "Once")
                                            {
                                                Vars.conLog.Info("Adding MOTD [" + currentMode + "]...");
                                                try
                                                {
                                                    int instances = Vars.onceMOTDList.Count() + 1;
                                                    currentInstance = instances;
                                                    Vars.onceMOTDList.Add(new MOTD(currentMode + instances, (Convert.ToInt16(interval.Remove(interval.Length - 1)) * multiplier).ToString(), new List<string>()));
                                                }
                                                catch (Exception ex)
                                                {
                                                    Vars.conLog.Error("Once Interval must be an integer on line " + lineNumber + "! Skipping...");
                                                }
                                            }
                                            else if (currentMode == "List")
                                            {
                                                Vars.conLog.Info("Adding MOTD [" + currentMode + "]...");
                                                try
                                                {
                                                    int instances = Vars.listMOTDList.Count() + 1;
                                                    currentInstance = instances;
                                                    Vars.listMOTDList.Add(new MOTD(currentMode + instances, (Convert.ToInt16(interval.Remove(interval.Length - 1)) * multiplier).ToString(), new List<string>()));
                                                }
                                                catch (Exception ex)
                                                {
                                                    Vars.conLog.Error("List Interval must be an integer on line " + lineNumber + "! Skipping...");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            currentMode = line.Substring(1, line.Length - 2);

                                            if (!Vars.motdList.ContainsKey(currentMode))
                                            {
                                                Vars.motdList.Add(currentMode, new List<string>());
                                                Vars.conLog.Info("Adding MOTD [" + currentMode + "]...");
                                            }
                                        }
                                    }
                                    catch (Exception ex) { Vars.conLog.Error("LOADMOTD #2: " + ex.ToString()); }
                                }
                                else
                                {
                                    try
                                    {
                                        if (line.Length > 1)
                                        {
                                            if (Vars.cycleMOTDList.ContainsMOTD(currentMode + currentInstance))
                                            {
                                                Vars.cycleMOTDList.GetMOTD(currentMode + currentInstance).messages.Add(line);
                                            }
                                            else if (Vars.onceMOTDList.ContainsMOTD(currentMode + currentInstance))
                                            {
                                                Vars.onceMOTDList.GetMOTD(currentMode + currentInstance).messages.Add(line);
                                            }
                                            else if (Vars.listMOTDList.ContainsMOTD(currentMode + currentInstance))
                                            {
                                                Vars.listMOTDList.GetMOTD(currentMode + currentInstance).messages.Add(line);
                                            }
                                            else
                                            {
                                                if (Vars.motdList.ContainsKey(currentMode))
                                                    Vars.motdList[currentMode].Add(line);
                                            }
                                        }
                                    }
                                    catch (Exception ex) { Vars.conLog.Error("LOADMOTD #3: " + ex.ToString()); }
                                }
                            }
                        }
                    }
                    Vars.cycleMOTD();
                    Vars.onceMOTD();
                    Vars.listMOTD();
                }
            }
            catch (Exception ex) { Vars.conLog.Error("LOADMOTD #1: " + ex.ToString()); }
        }

        public bool loadConfig()
        {
            if (File.Exists(Vars.cfgFile))
            {
                string configInput = File.ReadAllText(Vars.cfgFile);
                List<string> configLines = new List<string>();
                foreach (string s in configInput.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    string line = s.Trim();
                    if (!line.StartsWith("#"))
                    {
                        configLines.Add(line);
                    }
                }

                if (Config.setVariables())
                {
                    try { Vars.enableWhitelist = Convert.ToBoolean(Config.enabledWhitelist); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableWhitelist could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    Vars.useMySQL = false;
                    //try { Vars.useMySQL = Convert.ToBoolean(Config.MySQL); }
                    //catch (Exception ex)
                    //{
                    //    Vars.conLog.Error("useMySQL could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    //}
                    try { Vars.useSteamGroup = Convert.ToBoolean(Config.useSteamGroup); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("useSteamGroup could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    Vars.steamGroup = Config.steamGroup.Replace("\r\n", "").Replace("\n", "");
                    try { Vars.autoRefresh = Convert.ToBoolean(Config.autoRefresh); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("autoRefresh could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.refreshInterval = Convert.ToInt64(Config.refreshInterval) * 1000; }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("refreshInterval could not be parsed as a number!");
                    }
                    try { Vars.useAsMembers = Convert.ToBoolean(Config.useAsMembers); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("useAsMembers could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    Vars.whitelistKickCMD = Config.whitelistKickCMD;
                    Vars.whitelistKickJoin = Config.whitelistKickJoin;
                    Vars.whitelistCheckGood = Config.whitelistCheckGood;
                    Vars.whitelistCheckBad = Config.whitelistCheckBad;

                    try { Vars.announceDrops = Convert.ToBoolean(Config.announceDrops); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("announceDrops could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.onFirstPlayer = Convert.ToBoolean(Config.onFirstPlayer); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("onFirstPlayer could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try
                    {
                        int number = Convert.ToInt32(Config.dropInterval.Substring(0, Config.dropInterval.Length - 1));
                        int multiplier = 1000;
                        if (Config.dropInterval.EndsWith("m"))
                            multiplier *= 60;
                        if (Config.dropInterval.EndsWith("h"))
                            multiplier *= 3600;
                        Vars.dropInterval = number * multiplier;
                    }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("dropInterval could not be parsed!");
                    }
                    try
                    {
                        Vars.dropTime = Convert.ToInt16(Config.dropTime);
                    }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("dropTime could not be parsed as a number!");
                    }
                    try
                    {
                        Vars.dropMode = (Convert.ToInt16(Config.dropMode) > 1 || Convert.ToInt16(Config.dropMode) < 0 ? 0 : Convert.ToInt16(Config.dropMode));
                    }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("dropMode could not be parsed as a number!");
                    }
                    try { Vars.minimumPlayers = Convert.ToInt16(Config.minimumPlayers); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("minimumPlayers could not be parsed as a number!");
                    }
                    try { Vars.planeCount = Convert.ToInt16(Config.planeCount); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("planeCount could not be parsed as a number!");
                    }
                    try { Vars.minimumCrates = Convert.ToInt16(Config.minimumCrates); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("minimumCrates could not be parsed as a number!");
                    }
                    try { Vars.maximumCrates = Convert.ToInt16(Config.maximumCrates); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("maximumCrates could not be parsed as a number!");
                    }

                    try { Vars.constantFullMoon = Convert.ToBoolean(Config.constantFullMoon); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("constantFullMoon could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.fallDamage = Convert.ToBoolean(Config.fallDamage); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("fallDamage could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { if (!Vars.fallDamage) Vars.enableFallSound = Convert.ToBoolean(Config.enableFallSound); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableFallSound could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }   
                    try { voice.distance = (float)Convert.ToInt16(Config.voiceDistance); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("voiceDistance could not be parsed as a number!");
                    }
                    try { Vars.enableRepair = Convert.ToBoolean(Config.enableRepair); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableRepair could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.doorStops = Convert.ToBoolean(Config.doorStops); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("doorStops could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.enableDurability = Convert.ToBoolean(Config.enableDurability); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableDurability could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.enableDoorHolding = Convert.ToBoolean(Config.enableDoorHolding); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableDoorHolding could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.enableKeepItems = Convert.ToBoolean(Config.enableKeepItems); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableKeepItems could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }

                    try { Vars.forceNudity = Convert.ToBoolean(Config.forceNudity); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("forceNudity could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.onlyOnJoin = Convert.ToBoolean(Config.onlyOnJoin); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("onlyOnJoin could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.nudityRefreshInterval = Convert.ToDouble(Config.nudityRefreshInterval) * 1000; }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("nudityRefreshInterval could not be parsed as a number!");
                    }

                    try { Vars.directChat = Convert.ToBoolean(Config.directChat); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("directChat could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.globalChat = Convert.ToBoolean(Config.globalChat); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("globalChat could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.removeTag = Convert.ToBoolean(Config.removeTag); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("removeTag could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    if (Vars.directChat)
                        Vars.removeTag = false;
                    Vars.defaultChat = Config.defaultChat;
                    if (!Vars.directChat && !Vars.globalChat)
                    {
                        if (Vars.defaultChat == "direct" || Vars.defaultChat == "global")
                            Vars.conLog.Error("Both chat channels were disabled! Enabling channel defined as defaultChat...");
                        else
                        {
                            Vars.conLog.Error("Both chat channels were disabled and defaultChat was not a recognized channel!");
                            Vars.conLog.Error("Defaulting to direct...");
                        }
                    }
                    Vars.allowedChars = Config.allowedChars.Replace("\n", "").Split(',').ToList();
                    try { Vars.restrictChars = Convert.ToBoolean(Config.restrictChars); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("restrictChars could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.minimumNameCount = Convert.ToInt16(Config.minimumNameCount); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("minimumNameCount could not be parsed as a number!");
                    }
                    try { Vars.maximumNameCount = Convert.ToInt16(Config.maximumNameCount); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("maximumNameCount could not be parsed as a number!");
                    }
                    try { Vars.kickDuplicate = Convert.ToBoolean(Config.kickDuplicate); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("kickDuplicate could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.lowerAuthority = Convert.ToBoolean(Config.lowerAuthority); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("lowerAuthority could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    Vars.illegalWords = Config.illegalWords.Replace("\n", "").Split(',').ToList();
                    if (Vars.illegalWords.Count == 1 && string.IsNullOrEmpty(Vars.illegalWords[0]))
                        Vars.illegalWords.Clear();
                    try { Vars.censorship = Convert.ToBoolean(Config.censorship); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("censorship could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }

                    Vars.botName = Vars.replaceQuotes(Config.botName);
                    Vars.defaultColor = Vars.replaceQuotes(Config.defaultColor);
                    Vars.serverIP = Config.serverIP.Replace("\n", "");
                    try { Vars.versionOnJoin = Convert.ToBoolean(Config.versionOnJoin); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("versionOnJoin could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    Vars.commandsToChat = Config.commandsToChat.Replace("\n", "").Split(',').ToList();
                    if (Vars.commandsToChat.Count == 1 && string.IsNullOrEmpty(Vars.commandsToChat[0]))
                        Vars.commandsToChat.Clear();
                    Vars.modMessageRanks = Config.modMessageRanks.Replace("\n", "").Split(',').ToList();
                    if (Vars.modMessageRanks.Count == 1 && string.IsNullOrEmpty(Vars.modMessageRanks[0]))
                        Vars.modMessageRanks.Clear();
                    try { Vars.enableKickBanMessages = Convert.ToBoolean(Config.enableKickBanMessages); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableKickBanMessages could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.enableMuteMessageToAll = Convert.ToBoolean(Config.enableMuteMessageToAll); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableMuteMessageToAll could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    Vars.joinMessage = Vars.replaceQuotes(Config.joinMessage).Replace("\n", "");
                    try { Vars.enableJoin = Convert.ToBoolean(Config.enableJoin); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableJoin could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }

                    Vars.leaveMessage = Vars.replaceQuotes(Config.leaveMessage).Replace("\n", "");
                    try { Vars.enableLeave = Convert.ToBoolean(Config.enableLeave); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableLeave could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }

                    Vars.suicideMessage = Vars.replaceQuotes(Config.suicideMessage).Replace("\n", "");
                    try { Vars.suicideMessages = Convert.ToBoolean(Config.enableSuicide); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableSuicide could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }

                    Vars.murderMessage = Vars.replaceQuotes(Config.murderMessage).Replace("\n", "");
                    if (Config.murderMessageUnknown.Contains("$VICTIM$") && Config.murderMessageUnknown.Contains("$KILLER$"))
                        Vars.murderMessageUnknown = Vars.replaceQuotes(Config.murderMessageUnknown).Replace("\n", "");
                    else
                        Vars.conLog.Error("Murder Message Unknown must contain both $VICTIM$ and $KILLER$! Defaulting to original...");
                    try { Vars.murderMessages = Convert.ToBoolean(Config.enableMurder); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableMurder could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }

                    Vars.accidentMessage = Vars.replaceQuotes(Config.deathMessage).Replace("\n", "");
                    try { Vars.accidentMessages = Convert.ToBoolean(Config.enableDeath); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableDeath could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    Vars.dropItemMessage = Vars.replaceQuotes(Config.dropItemMessage).Replace("\n", "");
                    try { Vars.hideKills = Convert.ToBoolean(Config.hideKills); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("hideKills could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.killsToConsole = Convert.ToBoolean(Config.killsToConsole); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("killsToConsole could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.includePositionsInLog = Convert.ToBoolean(Config.includePositionsInLog); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("includePositionsInLog could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }

                    try { Vars.enableConsoleLogging = Convert.ToBoolean(Config.enableConsoleLogging); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableConsoleLogging could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.enableChatLogging = Convert.ToBoolean(Config.enableChatLogging); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableChatLogging could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.logPluginChat = Convert.ToBoolean(Config.logPluginChat); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("logModMessages could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.chatLogCap = Convert.ToInt16(Config.chatLogCap); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("chatLogCap could not be parsed as a number!");
                    }
                    try { Vars.logCap = Convert.ToInt16(Config.logCap); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("logCap could not be parsed as a number!");
                    }
                    try { Vars.logBroadcastErrors = Convert.ToBoolean(Config.logBroadcastErrors); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("logBroadcastErrors could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.catchBroadcastErrors = Convert.ToBoolean(Config.catchBroadcastErrors); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("catchBroadcastErrors could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.disconnectEvenIfNull = Convert.ToBoolean(Config.disconnectEvenIfNull); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("disconnectEvenIfNull could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.logBedPlacements = Convert.ToBoolean(Config.logBedPlacements); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("logBedPlacements could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }

                    try { Vars.storageLogsCap = Convert.ToInt16(Config.storageLogCap); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("storageLogCap could not be parsed as a number!");
                    }
                    try { Vars.enableStorageLogs = Convert.ToBoolean(Config.enableStorageLogs); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableStorageLogs could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.logStorageTransfer = Convert.ToBoolean(Config.logStorageTransfer); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("logStorageTransfer could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.sleeperDeathLogsCap = Convert.ToInt16(Config.sleeperDeathLogsCap); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("sleeperDeathLogsCap could not be parsed as a number!");
                    }
                    try { Vars.enableSleeperDeathLogs = Convert.ToBoolean(Config.enableSleeperDeathLogs); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableSleeperDeathLogs could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }

                    try { Vars.unknownCommand = Convert.ToBoolean(Config.unknownCommand); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("unknownCommand could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.nextToName = Convert.ToBoolean(Config.nextToName); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("nextToName could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.removePrefix = Convert.ToBoolean(Config.removePrefix); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("removePrefix could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.enableWordWrap = Convert.ToBoolean(Config.enableWordWrap); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableWordWrap could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.wordWrapLimit = Convert.ToInt32(Config.wordWrapLimit); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("wordWrapLimit could not be parsed as a number!");
                    }
                    try { Vars.sunBasedCompass = Convert.ToBoolean(Config.sunBasedCompass); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("sunBasedCompass could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.sendChatToConsoles = Convert.ToBoolean(Config.sendChatToConsoles); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("sendChatToConsoles could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.enableDropdownKills = Convert.ToBoolean(Config.enableDropdownKills); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableDropdownKills could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.enableAllyName = Convert.ToBoolean(Config.enableAllyName); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableAllyName could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.enableDropdownFactionHits = Convert.ToBoolean(Config.enableDropdownFactionHits); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableDropdownFactionHits could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.enableDropdownAllyHits = Convert.ToBoolean(Config.enableDropdownAllyHits); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableDropdownAllyHits could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    Vars.dropdownKillMessage = Vars.replaceQuotes(Config.dropdownKillMessage).Replace("\n", "");
                    try { Vars.infAmmoClipSize = Convert.ToInt32(Config.infAmmoClipSize); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("infAmmoClipSize could not be parsed as a number!");
                    }

                    try { Vars.teleportRequestOn = Convert.ToBoolean(Config.teleportRequest); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("teleportRequest could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.requestDelay = Convert.ToInt16(Config.requestDelay); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("requestDelay could not be parsed as a number!");
                    }
                    try { Vars.warpDelay = Convert.ToInt16(Config.warpDelay); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("warpDelay could not be parsed as a number!");
                    }
                    try { Vars.requestCooldownType = Convert.ToInt16(Config.requestCooldownType); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("requestCooldownType could not be parsed as a number!");
                    }
                    try
                    {
                        long number = Convert.ToInt64(Config.requestCooldown.Substring(0, Config.requestCooldown.Length - 1));
                        int multiplier = 1000;
                        if (Config.requestCooldown.EndsWith("m"))
                            multiplier *= 60;
                        Vars.requestCooldown = number * multiplier;
                    }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("requestCooldown could not be parsed!");
                    }
                    try { Vars.denyRequestWarzone = Convert.ToBoolean(Config.denyRequestWarzone); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("denyRequestWarzone could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.enableInHouse = Convert.ToBoolean(Config.enableInHouse); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableInHouse could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.wandDistance = Convert.ToSingle(Config.wandDistance); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("wandDistance could not be parsed as a number!");
                    }
                    ItemDataBlock item = Array.Find(DatablockDictionary.All.ToArray(), (ItemDataBlock idb) => idb.name == Config.wandTool);
                    if (item != null || Config.wandTool.ToLower() == "any")
                    {
                        Vars.wandName = Config.wandTool;
                    }
                    else
                        Vars.conLog.Error("wandTool cannot be set to \"" + Config.wandTool + "\" because it is not a known item!");
                    item = Array.Find(DatablockDictionary.All.ToArray(), (ItemDataBlock idb) => idb.name == Config.portalTool);
                    if (item != null || Config.portalTool.ToLower() == "any")
                    {
                        Vars.portalName = Config.portalTool;
                    }
                    else
                        Vars.conLog.Error("portalTool cannot be set to \"" + Config.wandTool + "\" because it is not a known item!");
                    try { Vars.freezeRefreshDelay = Convert.ToSingle(Config.freezeRefreshDelay); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("freezeRefreshDelay could not be parsed as a number!");
                    }
                    try { Vars.freezeDistance = Convert.ToSingle(Config.freezeDistance); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("freezeDistance could not be parsed as a number!");
                    }

                    try { Vars.inheritCommands = Convert.ToBoolean(Config.inheritCommands); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("inheritCommands could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.inheritKits = Convert.ToBoolean(Config.inheritKits); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("inheritKits could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.inheritWarps = Convert.ToBoolean(Config.inheritWarps); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("inheritWarps could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }

                    try { Vars.friendlyDamage = Convert.ToSingle(Config.friendlyDamage); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("friendlyDamage could not be parsed as a number!");
                    }
                    try { Vars.allyDamage = Convert.ToSingle(Config.alliedDamage); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("alliedDamage could not be parsed as a number!");
                    }
                    try { Vars.neutralDamage = Convert.ToSingle(Config.neutralDamage); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("neutralDamage could not be parsed as a number!");
                    }
                    try { Vars.warDamage = Convert.ToSingle(Config.warDamage); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("warDamage could not be parsed as a number!");
                    }
                    try { Vars.warFriendlyDamage = Convert.ToSingle(Config.warFriendlyDamage); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("warFriendlyDamage could not be parsed as a number!");
                    }
                    try { Vars.warAllyDamage = Convert.ToSingle(Config.warAllyDamage); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("warAllyDamage could not be parsed as a number!");
                    }

                    //try { Vars.researchAtBench = Convert.ToBoolean(Config.researchAtBench); }
                    //catch (Exception ex)
                    //{
                    //    Vars.conLog.Error("researchAtBench could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    //}
                    try { Vars.infiniteResearch = Convert.ToBoolean(Config.infiniteResearch); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("infiniteResearch could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.researchPaper = Convert.ToBoolean(Config.researchPaper); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("researchPaper could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    //try { Vars.craftAtBench = Convert.ToBoolean(Config.craftAtBench); }
                    //catch (Exception ex)
                    //{
                    //    Vars.conLog.Error("craftAtBench could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    //}
                    try { Vars.removeOnDeath = Convert.ToBoolean(Config.removeOnDeath); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("removeOnDeath could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.removeOnDisconnect = Convert.ToBoolean(Config.removeOnDisconnect); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("removeOnDisconnect could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.enableDropItem = Convert.ToBoolean(Config.enableDropItem); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableDropItem could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }

                    try { Vars.enableRemover = Convert.ToBoolean(Config.enableRemover); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableRemover could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.returnItems = Convert.ToBoolean(Config.returnItems); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("returnItems could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.removerAttackDelay = Convert.ToSingle(Config.removerAttackDelay); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("removerAttackDelay could not be parsed as a number!");
                    }
                    try { Vars.disregardCeilingWeight = Convert.ToBoolean(Config.disregardCeilingWeight); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("disregardCeilingWeight could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.disregardPillarWeight = Convert.ToBoolean(Config.disregardPillarWeight); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("disregardPillarWeight could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.disregardFoundationWeight = Convert.ToBoolean(Config.disregardFoundationWeight); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("disregardFoundationWeight could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.enableWithGuns = Convert.ToBoolean(Config.enableWithGuns); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableWithGuns could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.enableOneHit = Convert.ToBoolean(Config.enableOneHit); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableOneHit could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.confirmOneHit = Convert.ToBoolean(Config.confirmOneHit); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("confirmOneHit could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.removerDeactivateInterval = Convert.ToInt64(Config.removerDeactivateInterval); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("removerDeactivateInterval could not be parsed as a number!");
                    }
                    try { Vars.enableTimedRemover = Convert.ToBoolean(Config.enableTimedRemover); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableTimedRemover could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }

                    try { Vars.enableTRSVoting = Convert.ToBoolean(Config.enableTRSVoting); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableTRSVoting could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    Vars.TRSAPIKey = Config.TRSAPIKey;
                    Vars.TRSvoteLink = Config.TRSvoteLink;
                    Vars.TRSvotingMessage = Config.TRSvotingMessage;

                    try { Vars.enableRSVoting = Convert.ToBoolean(Config.enableRSVoting); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableRSVoting could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    Vars.RSAPIKey = Config.RSAPIKey;
                    Vars.RSvoteLink = Config.RSvoteLink;
                    Vars.RSvotingMessage = Config.RSvotingMessage;

                    try { Vars.enableRank = Convert.ToBoolean(Config.enableRank); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableRank could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }

                    try { Vars.checkIfInZone = Convert.ToBoolean(Config.checkIfInZone); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("checkIfInZone could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }

                    try { Vars.enableLimitedSleepers = Convert.ToBoolean(Config.enableLimitedSleepers); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableLimitedSleepers could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.sleeperElapseInterval = Convert.ToInt64(Config.sleeperElapseInterval) * 1000; }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("sleeperElapseInterval could not be parsed as a number!");
                    }
                    Vars.excludeFromSleepers = Config.excludeFromSleepers.Replace("\n", "").Split(',').ToList();
                    if (Vars.excludeFromSleepers.Count == 1 && string.IsNullOrEmpty(Vars.excludeFromSleepers[0]))
                        Vars.excludeFromSleepers.Clear();

                    try 
                    {
                        if (!Vars.checkModeIsSet)
                        {
                            Vars.checkMode = Convert.ToInt16(Config.checkMode);
                            Vars.checkModeIsSet = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("checkMode could not be parsed as a number!");
                    }
                    try { Vars.enableAntiSpeed = Convert.ToBoolean(Config.enableAntiSpeed); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableAntiSpeed could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.enableAntiJump = Convert.ToBoolean(Config.enableAntiJump); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableAntiJump could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.enableAntiBP = Convert.ToBoolean(Config.enableAntiBP); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableAntiBP could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.enableAntiAW = Convert.ToBoolean(Config.enableAntiAW); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableAntiAW could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.enableAntiRange = Convert.ToBoolean(Config.enableAntiRange); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableAntiRange could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.rangeFlexibility = Convert.ToSingle(Config.rangeFlexibility); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("rangeFlexibility could not be parsed as a number!");
                    }
                    try { Vars.violationLimit = Convert.ToInt32(Config.violationLimit); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("violationLimit could not be parsed as a number!");
                    }
                    try { Vars.offenseLimit = Convert.ToInt32(Config.offenseLimit); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("offenseLimit could not be parsed as a number!");
                    }
                    try { Vars.maximumSpeed = Convert.ToSingle(Config.maximumSpeed); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("maximumSpeed could not be parsed as a number!");
                    }
                    try { Vars.maximumJumpSpeed = Convert.ToSingle(Config.maximumJumpSpeed); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("maximumJumpSpeed could not be parsed as a number!");
                    }
                    try { Vars.moveBackSpeed = Convert.ToBoolean(Config.moveBackSpeed); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("moveBackSpeed could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.moveBackJump = Convert.ToBoolean(Config.moveBackJump); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("moveBackJump could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.lowerViolationInterval = Convert.ToInt32(Config.lowerViolationInterval) * 2; }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("lowerViolationInterval could not be parsed as a number!");
                    }
                    try { Vars.sendAHToConsole = Convert.ToBoolean(Config.sendAHToConsole); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("sendAHToConsole could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.calculateInterval = Convert.ToSingle(Config.calculateInterval) / 1000; }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("calculateInterval could not be parsed as a number!");
                    }
                    try { Vars.bedAndBagDistance = Convert.ToSingle(Config.bedAndBagDistance); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("bedAndBagDistance could not be parsed as a number!");
                    }
                    try { Vars.gatewayDistance = Convert.ToSingle(Config.gatewayDistance); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("gatewayDistance could not be parsed as a number!");
                    }

                    try { Vars.rockMultiplier = Convert.ToSingle(Config.rockMultiplier); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("rockMultiplier could not be parsed as a number!");
                    }
                    try { Vars.sHatchetMultiplier = Convert.ToSingle(Config.sHatchetMultiplier); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("sHatchetMultiplier could not be parsed as a number!");
                    }
                    try { Vars.hatchetMultiplier = Convert.ToSingle(Config.hatchetMultiplier); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("hatchetMultiplier could not be parsed as a number!");
                    }
                    try { Vars.pickaxeMultiplier = Convert.ToSingle(Config.pickaxeMultiplier); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("pickaxeMultiplier could not be parsed as a number!");
                    }
                    try { Vars.overrideWoodResources = Convert.ToBoolean(Config.overrideWoodResources); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("overrideWoodResources could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.overrideOreResources = Convert.ToBoolean(Config.overrideOreResources); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("overrideOreResources could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.overrideAIResources = Convert.ToBoolean(Config.overrideAIResources); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("overrideAIResources could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.multiplyMaxWood = Convert.ToBoolean(Config.multiplyMaxWood); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("multiplyMaxWood could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.multiplyMaxOre = Convert.ToBoolean(Config.multiplyMaxOre); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("multiplyMaxOre could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.multiplyMaxAIResources = Convert.ToBoolean(Config.multiplyMaxAIResources); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("multiplyMaxAIResources could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }

                    //try { Vars.enableShopify = Convert.ToBoolean(Config.enableShopify); }
                    //catch (Exception ex)
                    //{
                    //    Vars.conLog.Error("enableShopify could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    //}
                    //Vars.shopifyAPIKey = Config.shopifyAPIKey;
                    //Vars.shopifyLink = Config.shopifyLink;

                    try { Vars.enableBouncingBetty = Convert.ToBoolean(Config.enableBouncingBetty); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableBouncingBetty could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try
                    {
                        bool atBettyRecipe = false;
                        bool inRecipe = false;
                        int curindex = 0;
                        Vars.bettyRecipe = new List<Items.Item>();
                        foreach (string s in configLines)
                        {
                            if (s.StartsWith("bettyRecipe="))
                                atBettyRecipe = true;
                            else if (atBettyRecipe)
                            {
                                string line = s.Trim();
                                if (line == "[")
                                {
                                    inRecipe = true;
                                }
                                else if (line == "]")
                                {
                                    inRecipe = false;
                                    break;
                                }
                                else if (inRecipe)
                                {
                                    curindex++;
                                    line = line.Replace(",", "").Trim();
                                    if (line.Contains(":"))
                                    {
                                        string[] itemInfo = line.Split(':');
                                        string itemName = itemInfo[0];
                                        string itemAmount = itemInfo[1];

                                        if (!string.IsNullOrEmpty(itemName))
                                        {
                                            if (Items.isItem(itemName))
                                            {
                                                int amount;
                                                if (int.TryParse(itemAmount, out amount))
                                                {
                                                    Vars.bettyRecipe.Add(new Items.Item(itemName, amount));
                                                }
                                                else
                                                {
                                                    Vars.conLog.Error("Item " + itemName + " has an invalid amount \"" + itemAmount + "\"!");
                                                    inRecipe = false;
                                                    atBettyRecipe = false;
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                Vars.conLog.Error("\"" + itemName + "\" is not a valid item name in bettyRecipe!");
                                                inRecipe = false;
                                                atBettyRecipe = false;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string itemName = line;
                                        if (!string.IsNullOrEmpty(itemName))
                                        {
                                            if (Items.isItem(itemName))
                                            {
                                                Vars.bettyRecipe.Add(new Items.Item(itemName, 1));
                                            }
                                            else
                                            {
                                                Vars.conLog.Error("\"" + itemName + "\" is not a valid item name in bettyRecipe!");
                                                inRecipe = false;
                                                atBettyRecipe = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("bettyRecipe could not be parsed!");
                    }
                    try { Vars.bettiesPerPlayer = Convert.ToInt16(Config.bettiesPerPlayer); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("bettiesPerPlayer could not be parsed as a number!");
                    }
                    try { Vars.bettyArmingDelay = Convert.ToInt16(Config.bettyArmingDelay); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("bettyArmingDelay could not be parsed as a number!");
                    }
                    try { Vars.bettyNearOtherHouses = Convert.ToBoolean(Config.bettyNearOtherHouses); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("bettyNearOtherHouses could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.distanceFromOtherHouses = Convert.ToSingle(Config.distanceFromOtherHouses); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("distanceFromOtherHouses could not be parsed as a number!");
                    }
                    try { Vars.ownerActivateBetty = Convert.ToBoolean(Config.ownerActivateBetty); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("ownerActivateBetty could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.factionActivateBetty = Convert.ToBoolean(Config.factionActivateBetty); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("factionActivateBetty could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.allyActivateBetty = Convert.ToBoolean(Config.allyActivateBetty); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("allyActivateBetty could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.bettyHurtOwner = Convert.ToBoolean(Config.bettyHurtOwner); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("bettyHurtOwner could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.bettyHurtFaction = Convert.ToBoolean(Config.bettyHurtFaction); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("bettyHurtFaction could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.bettyHurtAlly = Convert.ToBoolean(Config.bettyHurtAlly); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("bettyHurtAlly could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.ownerPickupBetty = Convert.ToBoolean(Config.ownerPickupBetty); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("ownerPickupBetty could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.factionPickupBetty = Convert.ToBoolean(Config.factionPickupBetty); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("factionPickupBetty could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.allyPickupBetty = Convert.ToBoolean(Config.allyPickupBetty); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("allyPickupBetty could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.neutralPickupBetty = Convert.ToBoolean(Config.neutralPickupBetty); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("neutralPickupBetty could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.returnBettyMaterials = Convert.ToBoolean(Config.returnBettyMaterials); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("returnBettyMaterials could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.bettyDeathDeleteItems = Convert.ToBoolean(Config.bettyDeathDeleteItems); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("bettyDeathDeleteItems could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    try { Vars.activateRadius = Convert.ToSingle(Config.activateRadius); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("activateRadius could not be parsed as a number!");
                    }
                    try { Vars.breakLegsRadius = Convert.ToSingle(Config.breakLegsRadius); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("breakLegsRadius could not be parsed as a number!");
                    }
                    try { Vars.bleedingRadius = Convert.ToSingle(Config.bleedingRadius); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("bleedingRadius could not be parsed as a number!");
                    }
                    try { Vars.hurtRadius = Convert.ToSingle(Config.hurtRadius); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("hurtRadius could not be parsed as a number!");
                    }
                    try { Vars.maxBettyPlayerDamage = Convert.ToSingle(Config.maxBettyPlayerDamage); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("maxBettyPlayerDamage could not be parsed as a number!");
                    }
                    try { Vars.maxBettyObjectDamage = Convert.ToSingle(Config.maxBettyObjectDamage); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("maxBettyObjectDamage could not be parsed as a number!");
                    }

                    try { Vars.enableAntiFamilyShare = Convert.ToBoolean(Config.enableAntiFamilyShare); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("enableAntiFamilyShare could not be parsed as a boolean! Make sure it is equal to ONLY true or false.");
                    }
                    Vars.steamAPIKey = Config.steamAPIKey;
                    Vars.excludeFromFamilyCheck = Config.excludeFromFamilyCheck.Replace("\n", "").Split(',').ToList();
                    if (Vars.excludeFromFamilyCheck.Count == 1 && string.IsNullOrEmpty(Vars.excludeFromFamilyCheck[0]))
                        Vars.excludeFromFamilyCheck.Clear();

                    try { Vars.defaultLightsRange = Convert.ToSingle(Config.defaultLightsRange); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("defaultLightsRange could not be parsed as a number!");
                    }
                    try { Vars.maxLightsRange = Convert.ToSingle(Config.maxLightsRange); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("maxLightsRange could not be parsed as a number!");
                    }
                    try { Vars.maxLightsPerHouse = Convert.ToInt32(Config.maxLightsPerHouse); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("maxLightsPerHouse could not be parsed as a number!");
                    }
                    try { Vars.maxLightsPerPerson = Convert.ToInt32(Config.maxLightsPerPerson); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("maxLightsPerPerson could not be parsed as a number!");
                    }

                    try { Vars.homeLimit = Convert.ToInt32(Config.homeLimit); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("homeLimit could not be parsed as a number!");
                    }
                    try { Vars.homeDelay = Convert.ToInt64(Config.homeDelay); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("homeDelay could not be parsed as a number!");
                    }
                    try
                    {
                        long number = Convert.ToInt64(Config.homeCooldown.Substring(0, Config.homeCooldown.Length - 1));
                        int multiplier = 1000;
                        if (Config.homeCooldown.EndsWith("m"))
                            multiplier *= 60;
                        Vars.homeCooldown = number * multiplier;
                    }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("homeCooldown could not be parsed as a number!");
                    }

                    try { Vars.memberLimit = Convert.ToInt32(Config.memberLimit); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("memberLimit could not be parsed as a number!");
                    }
                    try { Vars.factionHomeDelay = Convert.ToInt64(Config.factionHomeDelay); }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("factionHomeDelay could not be parsed as a number!");
                    }
                    try
                    {
                        long number = Convert.ToInt64(Config.factionHomeCooldown.Substring(0, Config.factionHomeCooldown.Length - 1));
                        int multiplier = 1000;
                        if (Config.factionHomeCooldown.EndsWith("m"))
                            multiplier *= 60;
                        Vars.factionHomeCooldown = number * multiplier;
                    }
                    catch (Exception ex)
                    {
                        Vars.conLog.Error("factionHomeCooldown could not be parsed as a number!");
                    }

                    Vars.conLog.Info("Config loaded.");
                    return true;
                }
            }
            else
            {
                Vars.conLog.Error("Config was not found! Using defaults...");
            }
            return false;
        }

        public void loadBans()
        {
            if (File.Exists(Vars.bansFile))
            {
                List<string> previousIPBans = new List<string>();
                Dictionary<string, string> previousBans = new Dictionary<string, string>();
                Dictionary<string, string> previousBanReasons = new Dictionary<string, string>();
                using (StreamReader sr = new StreamReader(Vars.bansFile))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string reason = "Unknown reason.";
                        if (line.Contains("#"))
                        {
                            reason = line.Substring(line.LastIndexOf("#") + 1).Trim();
                            line = line.Substring(0, line.LastIndexOf("#")).Trim();
                        }

                        if (line.Contains("="))
                        {
                            string playerName = line.Split('=')[0];
                            string playerUID = line.Split('=')[1];
                            if (!previousBans.ContainsKey(playerUID))
                                previousBans.Add(playerUID, playerName);
                            if (!previousBanReasons.ContainsKey(playerUID))
                                previousBanReasons.Add(playerUID, reason);
                        }
                        else if (line.Contains("."))
                        {
                            if (!previousIPBans.Contains(line))
                                previousIPBans.Add(line);
                            if (!previousBanReasons.ContainsKey(line))
                                previousBanReasons.Add(line, reason);
                        }
                    }
                }

                Vars.currentIPBans = previousIPBans;
                Vars.currentBans = previousBans;
                Vars.currentBanReasons = previousBanReasons;
                Vars.saveBans();
            }
        }
    }
}
