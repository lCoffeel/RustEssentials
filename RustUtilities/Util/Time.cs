/**
 * @file: Time.cs
 * @author: Team Cerionn (https://github.com/Team-Cerionn)

 * @description: Time class for Rust Essentials
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

namespace RustEssentials.Util
{
    public class Time
    {
        public static double timeScale = 0.01D;

        public static void freezeTime(bool b)
        {
            Vars.timeFrozen = b;
            if (b && EnvironmentControlCenter.timeScale > 0)
                timeScale = EnvironmentControlCenter.timeScale;
            EnvironmentControlCenter.ServerSetTiming(getTime(), (b ? 0D : timeScale));
        }

        public static double getScale()
        {
            return EnvironmentControlCenter.timeScale;
        }

        public static void setScale(double d)
        {
            EnvironmentControlCenter.ServerSetTiming(getTime(), d);
        }

        public static double getTime()
        {
            return EnvironmentControlCenter.time;
        }

        public static void setTime(double d)
        {
            EnvironmentControlCenter.ServerSetTiming(d, EnvironmentControlCenter.timeScale);
        }

        public static void setDay()
        {
            EnvironmentControlCenter.ServerSetTiming(12D, EnvironmentControlCenter.timeScale);
        }

        public static void setNight()
        {
            EnvironmentControlCenter.ServerSetTiming(2D, EnvironmentControlCenter.timeScale);
        }
    }
}
