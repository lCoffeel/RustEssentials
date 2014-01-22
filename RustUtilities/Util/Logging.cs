/**
 * @file: Logging.cs
 * @author: Team Cerionn (https://github.com/Team-Cerionn)
 * @version: 1.0.0.0
 * @description: Logging class for Rust Essentials
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facepunch;
using LeatherLoader;
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
            ConsoleSystem.Log("[Rust Essentials] " + s);
            logToFile(s, "info");
        }

        public void Error(string s)
        {
            ConsoleSystem.LogError("[Rust Essentials] " + s);
            logToFile(s, "error");
        }

        public void Warning(string s)
        {
            ConsoleSystem.LogWarning("[Rust Essentials] " + s);
            logToFile(s, "warning");
        }

        public void Chat(string s)
        {
            logToFile(s, "chat");
        }

        public void deleteChatLogs()
        {
            string[] fileArr = Directory.GetFiles(Vars.logsDir, "*.log");
            int totalFiles = fileArr.Count();
            DateTime[] creationTimes = new DateTime[fileArr.Length];
            for (int i2 = 0; i2 < fileArr.Length; i2++)
            {
                creationTimes[i2] = new FileInfo(fileArr[i2]).CreationTime;
            }
            Array.Sort(creationTimes, fileArr);
            for (int i3 = 0; i3 < fileArr.Length; i3++)
            {
                FileInfo fi = new FileInfo(fileArr[i3]);
                if (fi.Name.Contains("essentialsChat_"))
                {
                    if (totalFiles - i3 > Vars.chatLogCap)
                    {
                        fi.Delete();
                    }
                }
            }
        }

        public void deleteLogs()
        {
            string[] fileArr = Directory.GetFiles(Vars.logsDir, "*.log");
            int totalFiles = fileArr.Count();
            DateTime[] creationTimes = new DateTime[fileArr.Length];
            for (int i2 = 0; i2 < fileArr.Length; i2++)
            {
                creationTimes[i2] = new FileInfo(fileArr[i2]).CreationTime;
            }
            Array.Sort(creationTimes, fileArr);
            for (int i3 = 0; i3 < fileArr.Length; i3++)
            {
                FileInfo fi = new FileInfo(fileArr[i3]);
                if (fi.Name.Contains("essentials_"))
                {
                    if (totalFiles - i3 > Vars.logCap)
                    {
                        fi.Delete();
                    }
                }
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
                else if (Vars.currentVersion != Vars.remoteVersion)
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
                else if (Vars.currentVersion != Vars.remoteVersion)
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
            switch (type)
            {
                case "none":
                    if (File.Exists(Vars.currentLog))
                    {
                        using (StreamWriter sw = new StreamWriter(Vars.currentLog, true))
                        {
                            sw.WriteLine(s);
                        }
                    }
                    break;
                case "error":
                    if (File.Exists(Vars.currentLog))
                    {
                        using (StreamWriter sw = new StreamWriter(Vars.currentLog, true))
                        {
                            sw.WriteLine("[ERROR] " + s);
                        }
                    }
                    break;
                case "info":
                    if (File.Exists(Vars.currentLog))
                    {
                        using (StreamWriter sw = new StreamWriter(Vars.currentLog, true))
                        {
                            sw.WriteLine("[INFO] " + s);
                        }
                    }
                    break;
                case "warning":
                    if (File.Exists(Vars.currentLog))
                    {
                        using (StreamWriter sw = new StreamWriter(Vars.currentLog, true))
                        {
                            sw.WriteLine("[WARN] " + s);
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
            }
        }
    }
}
