using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RustEssentials.Util
{
    public static class Antihack
    {
        public static void collectiveMovementCheck()
        {
            RustServerManagement RSM = RustServerManagement.Get();
            List<PlayerClient> playerClients = new List<PlayerClient>();
            foreach (PlayerClient pc in Vars.AllPlayerClients) { playerClients.Add(pc); }

            foreach (PlayerClient pc in playerClients)
            {
                if (pc != null)
                {
                    if (Vars.AllCharacters.ContainsKey(pc))
                    {
                        Character playerChar = Vars.AllCharacters[pc];
                        if (playerChar != null && playerChar.alive)
                        {
                            if (!Vars.lastPositions.ContainsKey(playerChar))
                            {
                                Vars.lastPositions.Add(playerChar, playerChar.transform.position);
                            }
                            else
                            {
                                Vector3 lastPosition = Vars.lastPositions[playerChar];
                                Vector3 currentPosition = playerChar.transform.position;
                                Vector2 lastPosition2D = new Vector2(lastPosition.x, lastPosition.z);
                                Vector2 currentPosition2D = new Vector2(playerChar.eyesOrigin.x, playerChar.eyesOrigin.z);
                                float yDifference = currentPosition.y - lastPosition.y; // If the player is falling, the yDifference should be negative and positive when ascending

                                double speed = 0;
                                double distance = 0;
                                double jumpspeed = 0;
                                double time = (Vars.currentTime - Vars.lastSpeedTime);

                                if (time > (Vars.calculateInterval * 0.5) && !Vars.currentlyTeleporting.Contains(pc) && !Vars.wandList.ContainsKey(pc.userID.ToString()) && !Vars.portalList.Contains(pc.userID.ToString()) && !Vars.bypassList.Contains(pc.userID.ToString()) && !Vars.ghostList.ContainsKey(pc.userID.ToString()))
                                {
                                    if (lastPosition2D != currentPosition2D && Vars.enableAntiSpeed)
                                    {
                                        distance = Vector2.Distance(lastPosition2D, currentPosition2D);
                                        speed = Math.Round(distance / time, 5);
                                        //Vars.conLog.logToFile("Player " + pc.userName + " traveled at a speed of " + speed + " in " + time + " ms.", "info");
                                    }

                                    if (speed >= Vars.maximumSpeed && Vars.enableAntiSpeed)
                                    {
                                        if (pc.netPlayer != null && pc.netUser != null)
                                        {
                                            if (!Vars.antihackTeleport.Contains(pc))
                                            {
                                                if (Vars.moveBackSpeed)
                                                {
                                                    Vars.currentlyTeleporting.Add(pc);
                                                    RSM.TeleportPlayerToWorld(pc.netPlayer, lastPosition);
                                                    Vars.currentlyTeleporting.Remove(pc);
                                                }

                                                Broadcast.broadcastTo(Vars.notifyList.Values.ToList(), "Player " + pc.userName + " traveled at a speed of " + speed + " m/ms (" + Math.Round(distance, 2) + "m) in " + time + " ms.");
                                                if (Vars.sendAHToConsole)
                                                    Vars.conLog.Info("Player " + pc.userName + " traveled at a speed of " + speed + " m/ms (" + Math.Round(distance, 2) + "m) in " + time + " ms.");
                                                Vars.lastPositions[playerChar] = playerChar.eyesOrigin;
                                                // Add notifications/alerts here
                                                if (!Vars.violationCount.ContainsKey(pc))
                                                    Vars.violationCount.Add(pc, 1);
                                                else
                                                {
                                                    if (Vars.violationCount[pc] >= Vars.violationLimit && pc.netUser != null)
                                                        Vars.kickPlayer(pc.netUser, "[AH] Moving too fast! (Speed)", false);
                                                    else
                                                        Vars.violationCount[pc]++;
                                                }
                                            }
                                            else
                                                Vars.antihackTeleport.Remove(pc);
                                        }
                                    }
                                    else
                                    {
                                        if (yDifference > 0 && Vars.enableAntiJump)
                                        {
                                            jumpspeed = Math.Round(yDifference / time, 5);
                                            //Vars.conLog.logToFile("Player " + pc.userName + " traveled at a jumpspeed of " + jumpspeed + " in " + time + " ms.", "info");
                                        }

                                        if (jumpspeed >= Vars.maximumJumpSpeed && Vars.enableAntiJump)
                                        {
                                            if (pc.netPlayer != null && pc.netUser != null)
                                            {
                                                if (!Vars.antihackTeleport.Contains(pc))
                                                {
                                                    if (Vars.moveBackJump)
                                                    {
                                                        Vars.currentlyTeleporting.Add(pc);
                                                        RSM.TeleportPlayerToWorld(pc.netPlayer, lastPosition);
                                                        Vars.currentlyTeleporting.Remove(pc);
                                                    }

                                                    Broadcast.broadcastTo(Vars.notifyList.Values.ToList(), "Player " + pc.userName + " traveled at a jumpspeed of " + jumpspeed + " m/ms (" + Math.Round(yDifference, 2) + "m) in " + time + " ms.");
                                                    if (Vars.sendAHToConsole)
                                                        Vars.conLog.Info("Player " + pc.userName + " traveled at a jumpspeed of " + jumpspeed + " m/ms (" + Math.Round(yDifference, 2) + "m) in " + time + " ms.");
                                                    Vars.lastPositions[playerChar] = playerChar.eyesOrigin;
                                                    // Add notifications/alerts here
                                                    if (!Vars.violationCount.ContainsKey(pc))
                                                        Vars.violationCount.Add(pc, 1);
                                                    else
                                                    {
                                                        if (Vars.violationCount[pc] >= Vars.violationLimit && pc.netUser != null)
                                                        {
                                                            Vars.kickPlayer(pc.netUser, "[AH] Moving too fast! (Jump)", false);
                                                            if (!Vars.playerOffenses.ContainsKey(pc.userID))
                                                                Vars.playerOffenses.Add(pc.userID, 1);
                                                            else
                                                                Vars.playerOffenses[pc.userID]++;
                                                        }
                                                        else
                                                            Vars.violationCount[pc]++;
                                                    }
                                                }
                                                else
                                                    Vars.antihackTeleport.Remove(pc);
                                            }
                                        }
                                        else
                                            Vars.lastPositions[playerChar] = playerChar.eyesOrigin;
                                    }
                                }
                                else
                                    Vars.lastPositions[playerChar] = playerChar.eyesOrigin;
                            }
                        }
                        else
                        {
                            if (Vars.lastPositions.ContainsKey(playerChar))
                                Vars.lastPositions.Remove(playerChar);
                        }
                    }
                    if (Vars.loopIndex > Vars.lowerViolationInterval)
                    {
                        if (Vars.violationCount.ContainsKey(pc))
                        {
                            if (Vars.violationCount[pc] > 0)
                                Vars.violationCount[pc]--;
                            else
                                Vars.violationCount.Remove(pc);
                        }
                    }
                }
            }
            if (Vars.loopIndex > Vars.lowerViolationInterval)
                Vars.loopIndex = 0;
            Vars.loopIndex++;
        }

        public static IEnumerator individualMovementCheck(Character playerChar)
        {
            if (playerChar != null)
            {
                RustServerManagement RSM = RustServerManagement.Get();
                PlayerClient playerClient = playerChar.playerClient;
                MovementChecker mc = new MovementChecker();
                mc.previousTime = Vars.currentTime - 500;
                while (true)
                {
                    if (Vars.enableAntiJump || Vars.enableAntiSpeed)
                    {
                        if (playerChar != null && playerChar.alive)
                        {
                            if (!Vars.lastPositions.ContainsKey(playerChar))
                            {
                                Vars.lastPositions.Add(playerChar, playerChar.transform.position);
                            }
                            else
                            {
                                Vector3 lastPosition = Vars.lastPositions[playerChar];
                                Vector3 currentPosition = playerChar.eyesOrigin;
                                Vector2 lastPosition2D = new Vector2(lastPosition.x, lastPosition.z);
                                Vector2 currentPosition2D = new Vector2(playerChar.eyesOrigin.x, playerChar.eyesOrigin.z);
                                float yDifference = currentPosition.y - lastPosition.y; // If the player is falling, the yDifference should be negative and positive when ascending

                                double speed = 0;
                                double distance = 0;
                                double jumpspeed = 0;
                                double time = (Vars.currentTime - mc.previousTime);

                                if (time > (Vars.calculateInterval * 0.5) && !Vars.currentlyTeleporting.Contains(playerClient) && !Vars.wandList.ContainsKey(playerClient.userID.ToString()) && !Vars.portalList.Contains(playerClient.userID.ToString()) && !Vars.bypassList.Contains(playerClient.userID.ToString()) && !Vars.ghostList.ContainsKey(playerClient.userID.ToString()))
                                {
                                    if (lastPosition2D != currentPosition2D && Vars.enableAntiSpeed)
                                    {
                                        distance = Vector2.Distance(lastPosition2D, currentPosition2D);
                                        speed = Math.Round(distance / time, 5);
                                        //Vars.conLog.logToFile("Player " + playerClient.userName + " traveled at a speed of " + speed + " in " + time + " ms.", "info");
                                    }

                                    if (speed >= Vars.maximumSpeed && Vars.enableAntiSpeed)
                                    {
                                        if (playerClient.netPlayer != null && playerClient.netUser != null)
                                        {
                                            if (!Vars.antihackTeleport.Contains(playerClient))
                                            {
                                                if (Vars.moveBackSpeed)
                                                {
                                                    Vars.currentlyTeleporting.Add(playerClient);
                                                    RSM.TeleportPlayerToWorld(playerClient.netPlayer, lastPosition);
                                                    Vars.currentlyTeleporting.Remove(playerClient);
                                                }

                                                Broadcast.broadcastTo(Vars.notifyList.Values.ToList(), "Player " + playerClient.userName + " traveled at a speed of " + speed + " m/ms (" + Math.Round(distance, 2) + "m) in " + time + " ms.");
                                                if (Vars.sendAHToConsole)
                                                    Vars.conLog.Info("Player " + playerClient.userName + " traveled at a speed of " + speed + " m/ms (" + Math.Round(distance, 2) + "m) in " + time + " ms.");
                                                Vars.lastPositions[playerChar] = playerChar.eyesOrigin;
                                                // Add notifications/alerts here
                                                if (!Vars.violationCount.ContainsKey(playerClient))
                                                    Vars.violationCount.Add(playerClient, 1);
                                                else
                                                {
                                                    if (Vars.violationCount[playerClient] >= Vars.violationLimit && playerClient.netUser != null)
                                                        Vars.kickPlayer(playerClient.netUser, "[AH] Moving too fast! (Speed)", false);
                                                    else
                                                        Vars.violationCount[playerClient]++;
                                                }
                                            }
                                            else
                                                Vars.antihackTeleport.Remove(playerClient);
                                        }
                                    }
                                    else
                                    {
                                        if (yDifference > 0 && Vars.enableAntiJump)
                                        {
                                            jumpspeed = Math.Round(yDifference / time, 5);
                                            //Vars.conLog.logToFile("Player " + playerClient.userName + " traveled at a jumpspeed of " + jumpspeed + " in " + time + " ms.", "info");
                                        }

                                        if (jumpspeed >= Vars.maximumJumpSpeed && Vars.enableAntiJump)
                                        {
                                            if (playerClient.netPlayer != null && playerClient.netUser != null)
                                            {
                                                if (!Vars.antihackTeleport.Contains(playerClient))
                                                {
                                                    if (Vars.moveBackJump)
                                                    {
                                                        Vars.currentlyTeleporting.Add(playerClient);
                                                        RSM.TeleportPlayerToWorld(playerClient.netPlayer, lastPosition);
                                                        Vars.currentlyTeleporting.Remove(playerClient);
                                                    }

                                                    Broadcast.broadcastTo(Vars.notifyList.Values.ToList(), "Player " + playerClient.userName + " traveled at a jumpspeed of " + jumpspeed + " m/ms (" + Math.Round(yDifference, 2) + "m) in " + time + " ms.");
                                                    if (Vars.sendAHToConsole)
                                                        Vars.conLog.Info("Player " + playerClient.userName + " traveled at a jumpspeed of " + jumpspeed + " m/ms (" + Math.Round(yDifference, 2) + "m) in " + time + " ms.");
                                                    Vars.lastPositions[playerChar] = playerChar.eyesOrigin;
                                                    // Add notifications/alerts here
                                                    if (!Vars.violationCount.ContainsKey(playerClient))
                                                        Vars.violationCount.Add(playerClient, 1);
                                                    else
                                                    {
                                                        if (Vars.violationCount[playerClient] >= Vars.violationLimit && playerClient.netUser != null)
                                                        {
                                                            Vars.kickPlayer(playerClient.netUser, "[AH] Moving too fast! (Jump)", false);
                                                            if (!Vars.playerOffenses.ContainsKey(playerClient.userID))
                                                                Vars.playerOffenses.Add(playerClient.userID, 1);
                                                            else
                                                                Vars.playerOffenses[playerClient.userID]++;
                                                        }
                                                        else
                                                            Vars.violationCount[playerClient]++;
                                                    }
                                                }
                                                else
                                                    Vars.antihackTeleport.Remove(playerClient);
                                            }
                                        }
                                        else
                                            Vars.lastPositions[playerChar] = playerChar.eyesOrigin;
                                    }
                                }
                                else
                                    Vars.lastPositions[playerChar] = playerChar.eyesOrigin;
                            }
                        }
                        else
                        {
                            if (Vars.lastPositions.ContainsKey(playerChar))
                                Vars.lastPositions.Remove(playerChar);

                            break;
                        }
                    }

                    mc.previousTime = Vars.currentTime;
                    yield return new WaitForSeconds(Vars.calculateInterval);

                    if (mc.loopIndex >= Vars.lowerViolationInterval)
                    {
                        if (Vars.violationCount.ContainsKey(playerClient))
                        {
                            if (Vars.violationCount[playerClient] > 0)
                                Vars.violationCount[playerClient]--;
                            else
                                Vars.violationCount.Remove(playerClient);
                        }
                        mc.loopIndex = 0;
                    }
                    mc.loopIndex++;
                }
            }
        }
    }

    public class MovementChecker
    {
        public int loopIndex = 0;
        public double previousTime = 0;
    }
}
