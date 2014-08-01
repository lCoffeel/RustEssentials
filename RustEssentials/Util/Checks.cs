using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RustEssentials.Util
{
    public static class Checks
    {
        public static bool inHouse(PlayerClient playerClient)
        {
            Character playerChar;
            if (playerClient != null)
            {
                Character.FindByUser(playerClient.userID, out playerChar);
                if (playerChar != null)
                {
                    Vector3 getPos = Vars.getPosition(playerClient) + new Vector3(0, 5, 0);
                    //getPos, Vector3.down, out hit, out didHit, out hitInstance
                    foreach (var hit in Physics.RaycastAll(getPos, Vector3.down, 10005f, -472317957))
                    {
                        IDMain main = IDBase.GetMain(hit.collider);

                        //Vars.conLog.Info(hit.collider.gameObject.name);
                        if ((hit.collider.gameObject.layer == 10 && hit.collider.gameObject.name.Contains("Shelter")) || (main != null && main is StructureMaster))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool onStructure(PlayerClient playerClient, out StructureMaster master)
        {
            Character playerChar;
            if (playerClient != null)
            {
                Character.FindByUser(playerClient.userID, out playerChar);
                if (playerChar != null)
                {
                    Vector3 getPos = Vars.getPosition(playerClient) + new Vector3(0, 5, 0);
                    //getPos, Vector3.down, out hit, out didHit, out hitInstance
                    foreach (var hit in Physics.RaycastAll(getPos, Vector3.down, 10f, -472317957))
                    {
                        IDMain main = IDBase.GetMain(hit.collider);

                        //Vars.conLog.Info(hit.collider.gameObject.name);
                        if (main != null && main is StructureMaster)
                        {
                            master = main as StructureMaster;
                            return true;
                        }
                    }
                }
            }
            master = null;
            return false;
        }

        public static bool onStructure(Vector3 sourcePos, out StructureMaster master)
        {
            Vector3 getPos = sourcePos + new Vector3(0, 5, 0);
            //getPos, Vector3.down, out hit, out didHit, out hitInstance
            foreach (var hit in Physics.RaycastAll(getPos, Vector3.down, 10005f, -472317957))
            {
                IDMain main = IDBase.GetMain(hit.collider);

                //Vars.conLog.Info(hit.collider.gameObject.name);
                if (main != null && main is StructureMaster)
                {
                    master = main as StructureMaster;
                    return true;
                }
            }
            master = null;
            return false;
        }

        public static bool aboveGround(Character playerChar)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerChar.eyesRay.origin, Vector3.down, out hit, 10005f, -472317957))
            {
                if (Vector3.Distance(playerChar.eyesRay.origin, hit.point) < 1)
                    return false;

                if (hit.collider is TerrainCollider || hit.collider.gameObject.layer == 10 || hit.collider.gameObject.layer == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool aboveGround(PlayerClient playerClient)
        {
            Character playerChar;
            if (playerClient != null)
            {
                Character.FindByUser(playerClient.userID, out playerChar);
                if (playerChar != null)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(playerChar.eyesRay.origin, Vector3.down, out hit, 10005f, -472317957))
                    {
                        if (Vector3.Distance(playerChar.eyesRay.origin, hit.point) < 1)
                            return false;

                        if (hit.collider is TerrainCollider || hit.collider.gameObject.layer == 10 || hit.collider.gameObject.layer == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool aboveGround(Vector3 origin)
        {
            RaycastHit hit;
            if (Physics.Raycast(origin, Vector3.down, out hit, 10005f, -472317957))
            {
                if (hit.collider is TerrainCollider || hit.collider.gameObject.layer == 10 || hit.collider.gameObject.layer == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isPointingAtDoor(Character playerChar, out GameObject doorObject)
        {
            RaycastHit hit;
            if (playerChar != null && playerChar.alive)
            {
                if (Physics.Raycast(playerChar.eyesRay, out hit, 30005f, -472317957))
                {
                    if (hit.collider.gameObject.layer == 20 && hit.collider.gameObject.GetComponent<DeployableObject>() != null && hit.collider.gameObject.GetComponent<BasicDoor>() != null)
                    {
                        doorObject = hit.collider.gameObject;
                        return true;
                    }
                }
            }
            doorObject = null;
            return false;
        }

        public static bool hasObstruction(Vector3 sourcePos, Vector3 destinationPos, GameObject target)
        {
            var direction = destinationPos - sourcePos;
            foreach (var hit in Physics.RaycastAll(new Ray(sourcePos, direction), 10))
            {
                if (hit.collider.gameObject.GetComponent<Character>() != null && hit.collider.gameObject == target)
                {
                    return false;
                }

                if ((hit.collider.gameObject.layer == 10 && !hit.collider.gameObject.name.Contains("road")) || hit.collider.gameObject.layer == 0 || (hit.collider.gameObject.layer == 20 && hit.collider.gameObject.GetComponent<DeployableObject>() != null && hit.collider.gameObject.GetComponent<BasicDoor>() != null))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isPointingAtObject(Character playerChar, out Vector3 destination)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerChar.eyesRay, out hit, 30005f, -472317957))
            {
                if (hit.collider is TerrainCollider || hit.collider.gameObject.layer == 10 || hit.collider.gameObject.layer == 0 || (hit.collider.gameObject.layer == 20 && hit.collider.gameObject.GetComponent<DeployableObject>() != null && hit.collider.gameObject.GetComponent<BasicDoor>() != null))
                {
                    destination = Vector3.MoveTowards(hit.point, playerChar.transform.position, 2);
                    return true;
                }
            }
            destination = playerChar.transform.position;
            return false;
        }

        public static bool inZone(Vector3 origin)
        {
            Vector2 origin2D = new Vector2(origin.x, origin.z);

            foreach (Zone zone in Vars.safeZones)
            {
                Vector2 point1 = zone.firstPoint;
                Vector2 point2 = zone.secondPoint;
                Vector2 point3 = zone.thirdPoint;
                Vector2 point4 = zone.fourthPoint;
                float s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, origin2D) + Vector2.Distance(origin2D, point1)) / 2;
                float s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, origin2D) + Vector2.Distance(origin2D, point2)) / 2;
                float s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, origin2D) + Vector2.Distance(origin2D, point3)) / 2;
                float s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, origin2D) + Vector2.Distance(origin2D, point4)) / 2;
                double areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, origin2D)) * (s1 - Vector2.Distance(origin2D, point1)));
                double areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, origin2D)) * (s2 - Vector2.Distance(origin2D, point2)));
                double areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, origin2D)) * (s3 - Vector2.Distance(origin2D, point3)));
                double areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, origin2D)) * (s4 - Vector2.Distance(origin2D, point4)));
                double areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                double areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                if (areaAdded <= (areaActual + 1))
                {
                    return true;
                }
            }
            foreach (Zone zone in Vars.warZones)
            {
                Vector2 point1 = zone.firstPoint;
                Vector2 point2 = zone.secondPoint;
                Vector2 point3 = zone.thirdPoint;
                Vector2 point4 = zone.fourthPoint;
                float s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, origin2D) + Vector2.Distance(origin2D, point1)) / 2;
                float s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, origin2D) + Vector2.Distance(origin2D, point2)) / 2;
                float s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, origin2D) + Vector2.Distance(origin2D, point3)) / 2;
                float s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, origin2D) + Vector2.Distance(origin2D, point4)) / 2;
                double areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, origin2D)) * (s1 - Vector2.Distance(origin2D, point1)));
                double areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, origin2D)) * (s2 - Vector2.Distance(origin2D, point2)));
                double areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, origin2D)) * (s3 - Vector2.Distance(origin2D, point3)));
                double areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, origin2D)) * (s4 - Vector2.Distance(origin2D, point4)));
                double areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                double areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                if (areaAdded <= (areaActual + 1))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool inZone(Vector3 origin, out Zone zoneInst)
        {
            Vector2 origin2D = new Vector2(origin.x, origin.z);

            zoneInst = null;
            foreach (Zone zone in Vars.safeZones)
            {
                Vector2 point1 = zone.firstPoint;
                Vector2 point2 = zone.secondPoint;
                Vector2 point3 = zone.thirdPoint;
                Vector2 point4 = zone.fourthPoint;
                float s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, origin2D) + Vector2.Distance(origin2D, point1)) / 2;
                float s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, origin2D) + Vector2.Distance(origin2D, point2)) / 2;
                float s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, origin2D) + Vector2.Distance(origin2D, point3)) / 2;
                float s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, origin2D) + Vector2.Distance(origin2D, point4)) / 2;
                double areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, origin2D)) * (s1 - Vector2.Distance(origin2D, point1)));
                double areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, origin2D)) * (s2 - Vector2.Distance(origin2D, point2)));
                double areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, origin2D)) * (s3 - Vector2.Distance(origin2D, point3)));
                double areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, origin2D)) * (s4 - Vector2.Distance(origin2D, point4)));
                double areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                double areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                if (areaAdded <= (areaActual + 1))
                {
                    zoneInst = zone;
                    return true;
                }
            }
            foreach (Zone zone in Vars.warZones)
            {
                Vector2 point1 = zone.firstPoint;
                Vector2 point2 = zone.secondPoint;
                Vector2 point3 = zone.thirdPoint;
                Vector2 point4 = zone.fourthPoint;
                float s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, origin2D) + Vector2.Distance(origin2D, point1)) / 2;
                float s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, origin2D) + Vector2.Distance(origin2D, point2)) / 2;
                float s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, origin2D) + Vector2.Distance(origin2D, point3)) / 2;
                float s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, origin2D) + Vector2.Distance(origin2D, point4)) / 2;
                double areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, origin2D)) * (s1 - Vector2.Distance(origin2D, point1)));
                double areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, origin2D)) * (s2 - Vector2.Distance(origin2D, point2)));
                double areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, origin2D)) * (s3 - Vector2.Distance(origin2D, point3)));
                double areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, origin2D)) * (s4 - Vector2.Distance(origin2D, point4)));
                double areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                double areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                if (areaAdded <= (areaActual + 1))
                {
                    zoneInst = zone;
                    return true;
                }
            }

            return false;
        }

        public static bool nearZone(Vector3 origin, out Zone zone)
        {
            for (int i = 1; i < 6; i++)
            {
                Vector2 origin2D = new Vector2(origin.x, origin.z);
                Vector2 originLeft = new Vector2(origin.x - (2 * i), origin.z);
                Vector2 originRight = new Vector2(origin.x + (2 * i), origin.z);
                Vector2 originBottom = new Vector2(origin.x, origin.z - (2 * i));
                Vector2 originTop = new Vector2(origin.x, origin.z + (2 * i));
                foreach (Zone z in Vars.safeZones)
                {
                    Vector2 point1 = z.firstPoint;
                    Vector2 point2 = z.secondPoint;
                    Vector2 point3 = z.thirdPoint;
                    Vector2 point4 = z.fourthPoint;
                    float s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, origin2D) + Vector2.Distance(origin2D, point1)) / 2;
                    float s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, origin2D) + Vector2.Distance(origin2D, point2)) / 2;
                    float s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, origin2D) + Vector2.Distance(origin2D, point3)) / 2;
                    float s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, origin2D) + Vector2.Distance(origin2D, point4)) / 2;
                    double areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, origin2D)) * (s1 - Vector2.Distance(origin2D, point1)));
                    double areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, origin2D)) * (s2 - Vector2.Distance(origin2D, point2)));
                    double areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, origin2D)) * (s3 - Vector2.Distance(origin2D, point3)));
                    double areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, origin2D)) * (s4 - Vector2.Distance(origin2D, point4)));
                    double areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    double areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        zone = z;
                        return true;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originLeft) + Vector2.Distance(originLeft, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originLeft) + Vector2.Distance(originLeft, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originLeft) + Vector2.Distance(originLeft, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originLeft) + Vector2.Distance(originLeft, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originLeft)) * (s1 - Vector2.Distance(originLeft, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originLeft)) * (s2 - Vector2.Distance(originLeft, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originLeft)) * (s3 - Vector2.Distance(originLeft, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originLeft)) * (s4 - Vector2.Distance(originLeft, point4)));
                    areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        zone = z;
                        return true;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originRight) + Vector2.Distance(originRight, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originRight) + Vector2.Distance(originRight, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originRight) + Vector2.Distance(originRight, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originRight) + Vector2.Distance(originRight, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originRight)) * (s1 - Vector2.Distance(originRight, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originRight)) * (s2 - Vector2.Distance(originRight, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originRight)) * (s3 - Vector2.Distance(originRight, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originRight)) * (s4 - Vector2.Distance(originRight, point4)));
                    areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        zone = z;
                        return true;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originTop) + Vector2.Distance(originTop, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originTop) + Vector2.Distance(originTop, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originTop) + Vector2.Distance(originTop, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originTop) + Vector2.Distance(originTop, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originTop)) * (s1 - Vector2.Distance(originTop, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originTop)) * (s2 - Vector2.Distance(originTop, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originTop)) * (s3 - Vector2.Distance(originTop, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originTop)) * (s4 - Vector2.Distance(originTop, point4)));
                    areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        zone = z;
                        return true;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originBottom) + Vector2.Distance(originBottom, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originBottom) + Vector2.Distance(originBottom, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originBottom) + Vector2.Distance(originBottom, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originBottom) + Vector2.Distance(originBottom, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originBottom)) * (s1 - Vector2.Distance(originBottom, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originBottom)) * (s2 - Vector2.Distance(originBottom, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originBottom)) * (s3 - Vector2.Distance(originBottom, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originBottom)) * (s4 - Vector2.Distance(originBottom, point4)));
                    areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        zone = z;
                        return true;
                    }
                }

                foreach (Zone z in Vars.warZones)
                {
                    Vector2 point1 = z.firstPoint;
                    Vector2 point2 = z.secondPoint;
                    Vector2 point3 = z.thirdPoint;
                    Vector2 point4 = z.fourthPoint;
                    float s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, origin2D) + Vector2.Distance(origin2D, point1)) / 2;
                    float s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, origin2D) + Vector2.Distance(origin2D, point2)) / 2;
                    float s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, origin2D) + Vector2.Distance(origin2D, point3)) / 2;
                    float s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, origin2D) + Vector2.Distance(origin2D, point4)) / 2;
                    double areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, origin2D)) * (s1 - Vector2.Distance(origin2D, point1)));
                    double areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, origin2D)) * (s2 - Vector2.Distance(origin2D, point2)));
                    double areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, origin2D)) * (s3 - Vector2.Distance(origin2D, point3)));
                    double areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, origin2D)) * (s4 - Vector2.Distance(origin2D, point4)));
                    double areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    double areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        zone = z;
                        return true;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originLeft) + Vector2.Distance(originLeft, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originLeft) + Vector2.Distance(originLeft, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originLeft) + Vector2.Distance(originLeft, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originLeft) + Vector2.Distance(originLeft, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originLeft)) * (s1 - Vector2.Distance(originLeft, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originLeft)) * (s2 - Vector2.Distance(originLeft, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originLeft)) * (s3 - Vector2.Distance(originLeft, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originLeft)) * (s4 - Vector2.Distance(originLeft, point4)));
                    areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        zone = z;
                        return true;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originRight) + Vector2.Distance(originRight, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originRight) + Vector2.Distance(originRight, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originRight) + Vector2.Distance(originRight, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originRight) + Vector2.Distance(originRight, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originRight)) * (s1 - Vector2.Distance(originRight, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originRight)) * (s2 - Vector2.Distance(originRight, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originRight)) * (s3 - Vector2.Distance(originRight, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originRight)) * (s4 - Vector2.Distance(originRight, point4)));
                    areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        zone = z;
                        return true;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originTop) + Vector2.Distance(originTop, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originTop) + Vector2.Distance(originTop, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originTop) + Vector2.Distance(originTop, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originTop) + Vector2.Distance(originTop, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originTop)) * (s1 - Vector2.Distance(originTop, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originTop)) * (s2 - Vector2.Distance(originTop, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originTop)) * (s3 - Vector2.Distance(originTop, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originTop)) * (s4 - Vector2.Distance(originTop, point4)));
                    areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        zone = z;
                        return true;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originBottom) + Vector2.Distance(originBottom, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originBottom) + Vector2.Distance(originBottom, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originBottom) + Vector2.Distance(originBottom, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originBottom) + Vector2.Distance(originBottom, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originBottom)) * (s1 - Vector2.Distance(originBottom, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originBottom)) * (s2 - Vector2.Distance(originBottom, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originBottom)) * (s3 - Vector2.Distance(originBottom, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originBottom)) * (s4 - Vector2.Distance(originBottom, point4)));
                    areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        zone = z;
                        return true;
                    }
                }
            }
            zone = null;
            return false;
        }

        public static bool nearZone(Vector3 origin)
        {
            for (int i = 1; i < 6; i++)
            {
                Vector2 origin2D = new Vector2(origin.x, origin.z);
                Vector2 originLeft = new Vector2(origin.x - (2 * i), origin.z);
                Vector2 originRight = new Vector2(origin.x + (2 * i), origin.z);
                Vector2 originBottom = new Vector2(origin.x, origin.z - (2 * i));
                Vector2 originTop = new Vector2(origin.x, origin.z + (2 * i));
                foreach (Zone zone in Vars.safeZones)
                {
                    Vector2 point1 = zone.firstPoint;
                    Vector2 point2 = zone.secondPoint;
                    Vector2 point3 = zone.thirdPoint;
                    Vector2 point4 = zone.fourthPoint;
                    float s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, origin2D) + Vector2.Distance(origin2D, point1)) / 2;
                    float s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, origin2D) + Vector2.Distance(origin2D, point2)) / 2;
                    float s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, origin2D) + Vector2.Distance(origin2D, point3)) / 2;
                    float s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, origin2D) + Vector2.Distance(origin2D, point4)) / 2;
                    double areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, origin2D)) * (s1 - Vector2.Distance(origin2D, point1)));
                    double areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, origin2D)) * (s2 - Vector2.Distance(origin2D, point2)));
                    double areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, origin2D)) * (s3 - Vector2.Distance(origin2D, point3)));
                    double areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, origin2D)) * (s4 - Vector2.Distance(origin2D, point4)));
                    double areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    double areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        return true;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originLeft) + Vector2.Distance(originLeft, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originLeft) + Vector2.Distance(originLeft, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originLeft) + Vector2.Distance(originLeft, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originLeft) + Vector2.Distance(originLeft, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originLeft)) * (s1 - Vector2.Distance(originLeft, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originLeft)) * (s2 - Vector2.Distance(originLeft, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originLeft)) * (s3 - Vector2.Distance(originLeft, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originLeft)) * (s4 - Vector2.Distance(originLeft, point4)));
                    areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        return true;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originRight) + Vector2.Distance(originRight, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originRight) + Vector2.Distance(originRight, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originRight) + Vector2.Distance(originRight, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originRight) + Vector2.Distance(originRight, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originRight)) * (s1 - Vector2.Distance(originRight, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originRight)) * (s2 - Vector2.Distance(originRight, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originRight)) * (s3 - Vector2.Distance(originRight, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originRight)) * (s4 - Vector2.Distance(originRight, point4)));
                    areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        return true;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originTop) + Vector2.Distance(originTop, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originTop) + Vector2.Distance(originTop, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originTop) + Vector2.Distance(originTop, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originTop) + Vector2.Distance(originTop, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originTop)) * (s1 - Vector2.Distance(originTop, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originTop)) * (s2 - Vector2.Distance(originTop, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originTop)) * (s3 - Vector2.Distance(originTop, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originTop)) * (s4 - Vector2.Distance(originTop, point4)));
                    areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        return true;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originBottom) + Vector2.Distance(originBottom, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originBottom) + Vector2.Distance(originBottom, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originBottom) + Vector2.Distance(originBottom, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originBottom) + Vector2.Distance(originBottom, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originBottom)) * (s1 - Vector2.Distance(originBottom, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originBottom)) * (s2 - Vector2.Distance(originBottom, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originBottom)) * (s3 - Vector2.Distance(originBottom, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originBottom)) * (s4 - Vector2.Distance(originBottom, point4)));
                    areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        return true;
                    }
                }

                foreach (Zone zone in Vars.warZones)
                {
                    Vector2 point1 = zone.firstPoint;
                    Vector2 point2 = zone.secondPoint;
                    Vector2 point3 = zone.thirdPoint;
                    Vector2 point4 = zone.fourthPoint;
                    float s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, origin2D) + Vector2.Distance(origin2D, point1)) / 2;
                    float s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, origin2D) + Vector2.Distance(origin2D, point2)) / 2;
                    float s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, origin2D) + Vector2.Distance(origin2D, point3)) / 2;
                    float s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, origin2D) + Vector2.Distance(origin2D, point4)) / 2;
                    double areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, origin2D)) * (s1 - Vector2.Distance(origin2D, point1)));
                    double areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, origin2D)) * (s2 - Vector2.Distance(origin2D, point2)));
                    double areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, origin2D)) * (s3 - Vector2.Distance(origin2D, point3)));
                    double areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, origin2D)) * (s4 - Vector2.Distance(origin2D, point4)));
                    double areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    double areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        return true;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originLeft) + Vector2.Distance(originLeft, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originLeft) + Vector2.Distance(originLeft, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originLeft) + Vector2.Distance(originLeft, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originLeft) + Vector2.Distance(originLeft, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originLeft)) * (s1 - Vector2.Distance(originLeft, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originLeft)) * (s2 - Vector2.Distance(originLeft, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originLeft)) * (s3 - Vector2.Distance(originLeft, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originLeft)) * (s4 - Vector2.Distance(originLeft, point4)));
                    areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        return true;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originRight) + Vector2.Distance(originRight, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originRight) + Vector2.Distance(originRight, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originRight) + Vector2.Distance(originRight, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originRight) + Vector2.Distance(originRight, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originRight)) * (s1 - Vector2.Distance(originRight, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originRight)) * (s2 - Vector2.Distance(originRight, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originRight)) * (s3 - Vector2.Distance(originRight, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originRight)) * (s4 - Vector2.Distance(originRight, point4)));
                    areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        return true;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originTop) + Vector2.Distance(originTop, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originTop) + Vector2.Distance(originTop, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originTop) + Vector2.Distance(originTop, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originTop) + Vector2.Distance(originTop, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originTop)) * (s1 - Vector2.Distance(originTop, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originTop)) * (s2 - Vector2.Distance(originTop, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originTop)) * (s3 - Vector2.Distance(originTop, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originTop)) * (s4 - Vector2.Distance(originTop, point4)));
                    areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        return true;
                    }

                    s1 = (Vector2.Distance(point1, point2) + Vector2.Distance(point2, originBottom) + Vector2.Distance(originBottom, point1)) / 2;
                    s2 = (Vector2.Distance(point2, point3) + Vector2.Distance(point3, originBottom) + Vector2.Distance(originBottom, point2)) / 2;
                    s3 = (Vector2.Distance(point3, point4) + Vector2.Distance(point4, originBottom) + Vector2.Distance(originBottom, point3)) / 2;
                    s4 = (Vector2.Distance(point4, point1) + Vector2.Distance(point1, originBottom) + Vector2.Distance(originBottom, point4)) / 2;
                    areaT1 = Math.Sqrt(s1 * (s1 - Vector2.Distance(point1, point2)) * (s1 - Vector2.Distance(point2, originBottom)) * (s1 - Vector2.Distance(originBottom, point1)));
                    areaT2 = Math.Sqrt(s2 * (s2 - Vector2.Distance(point2, point3)) * (s2 - Vector2.Distance(point3, originBottom)) * (s2 - Vector2.Distance(originBottom, point2)));
                    areaT3 = Math.Sqrt(s3 * (s3 - Vector2.Distance(point3, point4)) * (s3 - Vector2.Distance(point4, originBottom)) * (s3 - Vector2.Distance(originBottom, point3)));
                    areaT4 = Math.Sqrt(s4 * (s4 - Vector2.Distance(point4, point1)) * (s4 - Vector2.Distance(point1, originBottom)) * (s4 - Vector2.Distance(originBottom, point4)));
                    areaActual = Vars.PolygonArea(new Vector2[] { point1, point2, point3, point4 });
                    areaAdded = areaT1 + areaT2 + areaT3 + areaT4;

                    if (areaAdded <= (areaActual + 1))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool isPlayer(IDMain idMain)
        {
            if (idMain is Character)
            {
                Character character = idMain as Character;
                Controller player = character.playerControlledController;
                if (player != null)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public static bool ofEqualRank(string lowerRank, bool lowerIsUID, string higherRank, bool higherIsUID)
        {
            int lowerRankLevel;
            int higherRankLevel;

            if (lowerIsUID)
                lowerRankLevel = Vars.findRankPriority(Vars.findRank(lowerRank));
            else
                lowerRankLevel = Vars.findRankPriority(lowerRank);

            if (higherIsUID)
                higherRankLevel = Vars.findRankPriority(Vars.findRank(higherRank));
            else
                higherRankLevel = Vars.findRankPriority(higherRank);

            return (lowerRankLevel == higherRankLevel);
        }

        public static bool ofLowerRank(string lowerRank, bool lowerIsUID, string higherRank, bool higherIsUID)
        {
            int lowerRankLevel;
            int higherRankLevel;

            if (lowerIsUID)
                lowerRankLevel = Vars.findRankPriority(Vars.findRank(lowerRank));
            else
                lowerRankLevel = Vars.findRankPriority(lowerRank);

            if (higherIsUID)
                higherRankLevel = Vars.findRankPriority(Vars.findRank(higherRank));
            else
                higherRankLevel = Vars.findRankPriority(higherRank);

            return (lowerRankLevel < higherRankLevel);
        }

        public static bool ArrayContains(object[] objArray, object obj)
        {
            return Array.IndexOf(objArray, obj) > -1;
        }

        public static bool isAlive(PlayerClient playerClient)
        {
            Character playerChar;
            if (Vars.getPlayerChar(playerClient, out playerChar))
            {
                return playerChar.alive;
            }
            return false;
        }

        public static bool ContinueHook(Hook hook)
        {
            return hook == Hook.Continue || hook == Hook.Success;
        }
    }
}
