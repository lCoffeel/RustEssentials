using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RustEssentials
{
    public class Zone
    {
        public string zoneName;
        public bool buildable;
        public bool sleepersCanDie;
        public Vector2 firstPoint;
        public Vector2 secondPoint;
        public Vector2 thirdPoint;
        public Vector2 fourthPoint;

        public Zone(string zoneName, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, bool buildable = false, bool sleepersCanDie = true)
        {
            this.zoneName = zoneName;
            this.firstPoint = p1;
            this.secondPoint = p2;
            this.thirdPoint = p3;
            this.fourthPoint = p4;
            this.buildable = buildable;
            this.sleepersCanDie = sleepersCanDie;
        }
    }

    public class Zones : List<Zone>
    {
        public Zone GetByName(string zoneName)
        {
            foreach (var obj in this)
            {
                if (obj.zoneName == zoneName)
                    return obj;
            }

            return null;
        }

        public bool ContainsZone(string zoneName)
        {
            foreach (var obj in this)
            {
                if (obj.zoneName == zoneName)
                    return true;
            }

            return false;
        }

        public void Remove(string zoneName)
        {
            Zone zone = null;
            foreach (var obj in this)
            {
                if (obj.zoneName == zoneName)
                    zone = obj;
            }

            if (zone != null)
                this.Remove(zone);
        }
    }
}
