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
        public static void freezeTime(bool b)
        {
            Vars.timeFrozen = b;
            if (b && getDayLength() < 999999999f && getNightLength() < 999999999f)
            {
                setDayLength(999999999f);
                setNightLength(999999999f);
            }
            if (!b && getDayLength() >= 999999999f && getNightLength() >= 999999999f)
            {
                setDayLength(45f);
                setNightLength(15f);
            }
        }

        public static float getNightLength()
        {
            return env.nightlength;
        }

        public static float getDayLength()
        {
            return env.daylength; 
        }

        public static void setNightLength(float f)
        {
            env.nightlength = f;
        }

        public static void setDayLength(float f)
        {
            env.daylength = f;
        }

        public static double getTime()
        {
            return EnvironmentControlCenter.Singleton.GetTime();
        }

        public static void setTime(float d)
        {
            EnvironmentControlCenter.Singleton.SetTime(d);
        }

        public static void setDay()
        {
            EnvironmentControlCenter.Singleton.SetTime(12F);
        }

        public static void setNight()
        {
            EnvironmentControlCenter.Singleton.SetTime(2F);
        }
    }
}
