using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RustEssentials
{
    public class Zone
    {
        public Vector2 firstPoint;
        public Vector2 secondPoint;
        public Vector2 thirdPoint;
        public Vector2 forthPoint;

        public Zone(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            firstPoint = p1;
            secondPoint = p2;
            thirdPoint = p3;
            forthPoint = p4;
        }
    }
}
