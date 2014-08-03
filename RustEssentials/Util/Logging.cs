using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facepunch;
using UnityEngine;
using uLink;
using Rust;
using RustProto;
using System.IO;

namespace RustEssentials.Util
{
    public class Logging
    {
        public void Info(string s)
        {
            ConsoleSystem.Log("[RustEssentials] " + s);
            if (Vars.enableConsoleLogging)
                logToFile(s, "info");
        }

        public void Error(string s)
        {
            ConsoleSystem.LogError("[RustEssentials] " + s);
            if (Vars.enableConsoleLogging)
                logToFile(s, "error");
        }

        public void Warning(string s)
        {
            ConsoleSystem.LogWarning("[RustEssentials] " + s);
            if (Vars.enableConsoleLogging)
                logToFile(s, "warning");
        }

        public void Chat(string s)
        {
            if (Vars.enableChatLogging)
                logToFile(s, "chat");
        }

        public void StorageAddItem(string s)
        {
            if (Vars.enableStorageLogs && Vars.logStorageTransfer)
                logToFile("[+] " + s, "storage");
        }

        public void StorageRemoveItem(string s)
        {
            if (Vars.enableStorageLogs && Vars.logStorageTransfer)
                logToFile("[-] " + s, "storage");
        }

        public void Storage(string s)
        {
            if (Vars.enableStorageLogs)
                logToFile("[O] " + s, "storage");
        }

        public void StorageEmpty(string s)
        {
            if (Vars.enableStorageLogs)
                logToFile("[=] " + s, "storage");
        }

        public void SleeperDeath(string s)
        {
            if (Vars.enableSleeperDeathLogs)
                logToFile("[S] " + s, "sleeper");
        }

        public void deleteSleeperDeathLogs()
        {
            string[] fileArr = Directory.GetFiles(Vars.sleeperDeathLogsDir, "*.log");
            int totalFiles = Array.FindAll(fileArr.ToArray(), (string s) => s.Contains("essentialsSleeperDeaths_")).Count();
            DateTime[] creationTimes = new DateTime[fileArr.Length];
            for (int i2 = 0; i2 < fileArr.Length; i2++)
            {
                creationTimes[i2] = new FileInfo(fileArr[i2]).CreationTime;
            }
            Array.Sort(creationTimes, fileArr);
            int curIndex = 0;
            for (int i3 = 0; i3 < fileArr.Length; i3++)
            {
                FileInfo fi = new FileInfo(fileArr[i3]);
                if (fi.Name.Contains("essentialsSleeperDeaths_"))
                {
                    if (totalFiles - curIndex > Vars.sleeperDeathLogsCap)
                    {
                        fi.Delete();
                    }
                    curIndex++;
                }
            }
        }

        public void deleteStorageLogs()
        {
            string[] fileArr = Directory.GetFiles(Vars.storageLogsDir, "*.log");
            int totalFiles = Array.FindAll(fileArr.ToArray(), (string s) => s.Contains("essentialsStorage_")).Count();
            DateTime[] creationTimes = new DateTime[fileArr.Length];
            for (int i2 = 0; i2 < fileArr.Length; i2++)
            {
                creationTimes[i2] = new FileInfo(fileArr[i2]).CreationTime;
            }
            Array.Sort(creationTimes, fileArr);
            int curIndex = 0;
            for (int i3 = 0; i3 < fileArr.Length; i3++)
            {
                FileInfo fi = new FileInfo(fileArr[i3]);
                if (fi.Name.Contains("essentialsStorage_"))
                {
                    if (totalFiles - curIndex > Vars.storageLogsCap)
                    {
                        fi.Delete();
                    }
                    curIndex++;
                }
            }
        }

        public void deleteChatLogs()
        {
            string[] fileArr = Directory.GetFiles(Vars.logsDir, "*.log");
            int totalFiles = Array.FindAll(fileArr.ToArray(), (string s) => s.Contains("essentialsChat_")).Count();
            DateTime[] creationTimes = new DateTime[fileArr.Length];
            for (int i2 = 0; i2 < fileArr.Length; i2++)
            {
                creationTimes[i2] = new FileInfo(fileArr[i2]).CreationTime;
            }
            Array.Sort(creationTimes, fileArr);
            int curIndex = 0;
            for (int i3 = 0; i3 < fileArr.Length; i3++)
            {
                FileInfo fi = new FileInfo(fileArr[i3]);
                if (fi.Name.Contains("essentialsChat_"))
                {
                    if (totalFiles - curIndex > Vars.chatLogCap)
                    {
                        fi.Delete();
                    }
                    curIndex++;
                }
            }
        }

        public void deleteLogs()
        {
            string[] fileArr = Directory.GetFiles(Vars.logsDir, "*.log");
            int totalFiles = Array.FindAll(fileArr.ToArray(), (string s) => s.Contains("essentials_")).Count();
            DateTime[] creationTimes = new DateTime[fileArr.Length];
            for (int i2 = 0; i2 < fileArr.Length; i2++)
            {
                creationTimes[i2] = new FileInfo(fileArr[i2]).CreationTime;
            }
            Array.Sort(creationTimes, fileArr);
            int curIndex = 0;
            for (int i3 = 0; i3 < fileArr.Length; i3++)
            {
                FileInfo fi = new FileInfo(fileArr[i3]);
                if (fi.Name.Contains("essentials_"))
                {
                    if (totalFiles - curIndex > Vars.logCap)
                    {
                        fi.Delete();
                    }
                    curIndex++;
                }
            }
        }

        public void startLoggingSleeperDeaths()
        {
            DateTime dt = DateTime.Now;
            string dateStamp = dt.ToString("yyyy-MM-dd_HH-mm-ss");
            string fileName = "essentialsSleeperDeaths_" + dateStamp + ".log";

            if (!File.Exists(Path.Combine(Vars.sleeperDeathLogsDir, fileName)))
            {
                Vars.currentSleeperDeathsLog = Path.Combine(Vars.sleeperDeathLogsDir, fileName);
                File.Create(Vars.currentSleeperDeathsLog).Close();
                logToFile("====================================================================================", "sleeper");
                logToFile("RustEssentials Sleeper Deaths Logs :: v" + Vars.currentVersion, "sleeper");
                logToFile(DateTime.Now.ToString(), "sleeper");
                logToFile("====================================================================================", "sleeper");
                if (Vars.remoteVersion == "?")
                    logToFile("VERSION STATUS UNKNOWN\r\n", "sleeper");
                else if (Vars.currentVersion != Vars.remoteVersion && Vars.currentBuild == "")
                    logToFile("VERSION OUT OF DATE\r\n", "sleeper");
                else
                    logToFile("\r\n", "sleeper");
            }
        }

        public void startLoggingStorage()
        {
            DateTime dt = DateTime.Now;
            string dateStamp = dt.ToString("yyyy-MM-dd_HH-mm-ss");
            string fileName = "essentialsStorage_" + dateStamp + ".log";

            if (!File.Exists(Path.Combine(Vars.storageLogsDir, fileName)))
            {
                Vars.currentStorageLog = Path.Combine(Vars.storageLogsDir, fileName);
                File.Create(Vars.currentStorageLog).Close();
                logToFile("====================================================================================", "storage");
                logToFile("RustEssentials Storage Logs :: v" + Vars.currentVersion, "storage");
                logToFile(DateTime.Now.ToString(), "storage");
                logToFile("====================================================================================", "storage");
                if (Vars.remoteVersion == "?")
                    logToFile("VERSION STATUS UNKNOWN\r\n", "storage");
                else if (Vars.currentVersion != Vars.remoteVersion && Vars.currentBuild == "")
                    logToFile("VERSION OUT OF DATE\r\n", "storage");
                else
                    logToFile("\r\n", "storage");
            }
        }

        public void startLoggingChat()
        {
            DateTime dt = DateTime.Now;
            string dateStamp = dt.ToString("yyyy-MM-dd_HH-mm-ss");
            string fileName = "essentialsChat_" + dateStamp + ".log";

            if (!File.Exists(Path.Combine(Vars.logsDir, fileName)))
            {
                Vars.currentChatLog = Path.Combine(Vars.logsDir, fileName);
                File.Create(Vars.currentChatLog).Close();
                logToFile("====================================================================================", "nonechat");
                logToFile("RustEssentials Chat Logs :: v" + Vars.currentVersion, "nonechat");
                logToFile(DateTime.Now.ToString(), "nonechat");
                logToFile("====================================================================================", "nonechat");
                if (Vars.remoteVersion == "?")
                    logToFile("VERSION STATUS UNKNOWN\r\n", "nonechat");
                else if (Vars.currentVersion != Vars.remoteVersion && Vars.currentBuild == "")
                    logToFile("VERSION OUT OF DATE\r\n", "nonechat");
                else
                    logToFile("\r\n", "nonechat");
            }
        }

        public void startLogging()
        {
            DateTime dt = DateTime.Now;
            string dateStamp = dt.ToString("yyyy-MM-dd_HH-mm-ss");
            string fileName = "essentials_" + dateStamp + ".log";

            if (!File.Exists(Path.Combine(Vars.logsDir, fileName)))
            {
                Vars.currentLog = Path.Combine(Vars.logsDir, fileName);
                File.Create(Vars.currentLog).Close();
                logToFile("====================================================================================", "none");
                logToFile("RustEssentials :: v" + Vars.currentVersion, "none");
                logToFile(DateTime.Now.ToString(), "none");
                logToFile("====================================================================================", "none");
                if (Vars.remoteVersion == "?")
                    logToFile("VERSION STATUS UNKNOWN\r\n", "none");
                else if (Vars.currentVersion != Vars.remoteVersion && Vars.currentBuild == "")
                    logToFile("VERSION OUT OF DATE\r\n", "none");
                else
                    logToFile("\r\n", "none");
            }
        }

        /// <summary>
        /// Log all events to specified log file.        
        /// </summary>
        /// <param name="s"></param>
        /// Message to log
        public void logToFile(string s, string type)
        {
            if (!Vars.logToFile)
                return;
            s = s.Replace("\r\n", "").Replace("\n", "");
            switch (type)
            {
                case "none":
                    if (File.Exists(Vars.currentLog))
                    {
                        using (StreamWriter sw = new StreamWriter(Vars.currentLog, true))
                        {
                            DateTime dt = DateTime.Now;
                            string dateStamp = dt.ToString("MM/dd HH:mm:ss");
                            sw.WriteLine(dateStamp + "  " + s);
                        }
                    }
                    break;
                case "error":
                    if (File.Exists(Vars.currentLog))
                    {
                        using (StreamWriter sw = new StreamWriter(Vars.currentLog, true))
                        {
                            DateTime dt = DateTime.Now;
                            string dateStamp = dt.ToString("MM/dd HH:mm:ss");
                            sw.WriteLine(dateStamp + "  " + "[ERROR] " + s);
                        }
                    }
                    break;
                case "info":
                    if (File.Exists(Vars.currentLog))
                    {
                        using (StreamWriter sw = new StreamWriter(Vars.currentLog, true))
                        {
                            DateTime dt = DateTime.Now;
                            string dateStamp = dt.ToString("MM/dd HH:mm:ss");
                            sw.WriteLine(dateStamp + "  " + "[INFO] " + s);
                        }
                    }
                    break;
                case "warning":
                    if (File.Exists(Vars.currentLog))
                    {
                        using (StreamWriter sw = new StreamWriter(Vars.currentLog, true))
                        {
                            DateTime dt = DateTime.Now;
                            string dateStamp = dt.ToString("MM/dd HH:mm:ss");
                            sw.WriteLine(dateStamp + "  " + "[WARN] " + s);
                        }
                    }
                    break;
                case "chat":
                    if (File.Exists(Vars.currentChatLog))
                    {
                        using (StreamWriter sw = new StreamWriter(Vars.currentChatLog, true))
                        {
                            DateTime dt = DateTime.Now;
                            string dateStamp = dt.ToString("MM/dd HH:mm:ss");
                            sw.WriteLine(dateStamp + "  " + s);
                        }
                    }
                    break;
                case "nonechat":
                    if (File.Exists(Vars.currentChatLog))
                    {
                        using (StreamWriter sw = new StreamWriter(Vars.currentChatLog, true))
                        {
                            DateTime dt = DateTime.Now;
                            string dateStamp = dt.ToString("MM/dd HH:mm:ss");
                            sw.WriteLine(dateStamp + "  " + s);
                        }
                    }
                    break;
                case "storage":
                    if (File.Exists(Vars.currentStorageLog))
                    {
                        using (StreamWriter sw = new StreamWriter(Vars.currentStorageLog, true))
                        {
                            DateTime dt = DateTime.Now;
                            string dateStamp = dt.ToString("MM/dd HH:mm:ss");
                            sw.WriteLine(dateStamp + "  " + s);
                        }
                    }
                    break;
                case "sleeper":
                    if (File.Exists(Vars.currentSleeperDeathsLog))
                    {
                        using (StreamWriter sw = new StreamWriter(Vars.currentSleeperDeathsLog, true))
                        {
                            DateTime dt = DateTime.Now;
                            string dateStamp = dt.ToString("MM/dd HH:mm:ss");
                            sw.WriteLine(dateStamp + "  " + s);
                        }
                    }
                    break;
            }
        }
    }
}
