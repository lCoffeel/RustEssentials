/**
 * @file: Whitelist.cs
 * @author: Team Cerionn (https://github.com/Team-Cerionn)

 * @description: Whitelist class for Rust Essentials
 */
using System.IO;
using Facepunch;
using LeatherLoader;
using UnityEngine;
using Rust;
using RustProto;
using System.Collections.Generic;
using System.Timers;
using Rust.Steam;
using System.Runtime.InteropServices;
using System;
using System.Threading;

namespace RustEssentials.Util
{
    public class Whitelist
    {
        public static void Start()
        {
            if (File.Exists(Vars.whiteListFile))
            {
                Vars.startTimer();
                readWhitelist();
                TimerPlus t = new TimerPlus();
                t.AutoReset = true;
                t.Interval = Vars.refreshInterval;
                t.Elapsed += readWhitelistElapsed;
                t.Start();
            }
            else
            {
                Vars.conLog.Error("Whitelist file was not found!");
            }
        }

        private static int timesElapsed = 0;
        public static void readWhitelistElapsed(object sender, ElapsedEventArgs e)
        {
            if (Vars.enableWhitelist && Vars.autoRefresh)
            {
                if (timesElapsed > 0)
                {
                    int lineIndex = 0;
                    if (!Vars.useMySQL)
                    {
                        using (StreamReader sr = new StreamReader(Vars.whiteListFile))
                        {
                            Vars.whitelist.Clear();
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                if (line.Contains("#") && line.IndexOf("#") > 16)
                                {
                                    line = line.Substring(0, line.IndexOf("#")).Trim();
                                }
                                else
                                    line = line.Trim();
                                if (line.Length >= 17)
                                {
                                    lineIndex++;
                                    Vars.whitelist.Add(line);
                                    //Vars.conLog.Info("Adding to whitelist... (" + line + ")");
                                }
                            }
                        }
                    }
                    else
                    {
                        Vars.conLog.Info("Attempting connection to whitelist database...");
                        string connectionInfo = "SERVER=" + Config.host + ";PORT=" + Config.port + ";DATABASE=" + Config.database +
                            ";UID=" + Config.user + ";PASSWORD=" + Config.pass;

                        //Vars.mysqlConnection = new MySqlConnection(connectionInfo);
                        //try
                        //{
                        //    Vars.mysqlConnection.Open();
                        //    Vars.conLog.Info("Successfully connected to the database!");
                        //    Vars.whitelist.Clear();

                        //    MySqlCommand select = new MySqlCommand("SELECT * FROM whitelist", Vars.mysqlConnection);
                        //    MySqlDataReader mdr = select.ExecuteReader();

                        //    while (mdr.Read())
                        //    {
                        //        string UID = mdr["UID"] + "";
                        //        if (UID != "")
                        //        {
                        //            lineIndex++;
                        //            Vars.whitelist.Add(UID);
                        //        }
                        //    }
                        //    mdr.Close();

                        //    Vars.mysqlConnection.Close();
                        //}
                        //catch (MySqlException ex)
                        //{
                        //    switch (ex.Number)
                        //    {
                        //        case 0:
                        //            Vars.conLog.Error("Unable to connect to MySQL server!");
                        //            break;
                        //        case 1045:
                        //            Vars.conLog.Error("Invalid credentials when connecting to the MySQL server!");
                        //            break;
                        //    }
                        //}
                    }
                    if (Vars.whitelist.Count > 0 && Vars.whitelistToMembers && Vars.rankList.ContainsKey("Member"))
                        Vars.rankList["Member"] = Vars.whitelist;
                }
                timesElapsed++;
            }
        }

        public static void readWhitelist()
        {
            if (Vars.useSteamGroup)
            {
                Thread t = new Thread(Vars.grabGroupMembers);
                t.Start();
            }
            else
            {
                int lineIndex = 0;
                if (!Vars.useMySQL)
                {
                    using (StreamReader sr = new StreamReader(Vars.whiteListFile))
                    {
                        Vars.whitelist.Clear();
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.Contains("#") && line.IndexOf("#") > 16)
                            {
                                line = line.Substring(0, line.IndexOf("#")).Trim();
                            }
                            else
                                line = line.Trim();
                            if (line.Length >= 17)
                            {
                                lineIndex++;
                                Vars.whitelist.Add(line);
                                //Vars.conLog.Info("Adding to whitelist... (" + line + ")");
                            }
                        }
                    }
                }
                else
                {
                    Vars.conLog.Info("Attempting connection to whitelist database...");
                    string connectionInfo = "SERVER=" + Config.host + ";PORT=" + Config.port + ";DATABASE=" + Config.database +
                        ";UID=" + Config.user + ";PASSWORD=" + Config.pass;

                    //Vars.mysqlConnection = new MySqlConnection(connectionInfo);
                    //try
                    //{
                    //    Vars.mysqlConnection.Open();
                    //    Vars.conLog.Info("Successfully connected to the database!");
                    //    Vars.whitelist.Clear();

                    //    MySqlCommand select = new MySqlCommand("SELECT * FROM whitelist", Vars.mysqlConnection);
                    //    MySqlDataReader mdr = select.ExecuteReader();

                    //    while (mdr.Read())
                    //    {
                    //        string UID = mdr["UID"] + "";
                    //        if (UID != "")
                    //        {
                    //            lineIndex++;
                    //            Vars.whitelist.Add(UID);
                    //        }
                    //    }
                    //    mdr.Close();

                    //    Vars.mysqlConnection.Close();
                    //}
                    //catch (MySqlException ex)
                    //{
                    //    switch (ex.Number)
                    //    {
                    //        case 0:
                    //            Vars.conLog.Error("Unable to connect to MySQL server!");
                    //            break;
                    //        case 1045:
                    //            Vars.conLog.Error("Invalid credentials when connecting to the MySQL server!");
                    //            break;
                    //    }
                    //}
                }
                if (Vars.whitelist.Count > 0)
                {
                    if (Vars.enableWhitelist)
                        Vars.conLog.Info("Whitelist loaded with " + lineIndex + " entries.");
                    if (Vars.whitelistToMembers && Vars.rankList.ContainsKey("Member"))
                    {
                        Vars.conLog.Info("Member list loaded with " + lineIndex + " entries.");
                        Vars.rankList["Member"] = Vars.whitelist;
                    }
                }
                else
                {
                    if (Vars.enableWhitelist)
                        Vars.conLog.Error("Whitelist is enabled but no entries were found!");
                }
            }
        }
    }
}
