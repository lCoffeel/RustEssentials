using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;

namespace RustEssentials.Util
{
    public class Faction
    {
        public string name;
        public string owner;
        public ulong ownerID;
        public FactionMembers members;
        public List<string> allies;
        public FactionHome home;

        public Faction(string name, string owner, ulong ownerID)
        {
            this.name = name;
            this.owner = owner;
            this.ownerID = ownerID;
            members = new FactionMembers();
            allies = new List<string>();
            home = new FactionHome();
            members.Add(new FactionMember(owner, "owner", ownerID));
        }

        public Faction(string name, string owner, ulong ownerID, FactionMembers members, List<string> allies)
        {
            this.name = name;
            this.owner = owner;
            this.ownerID = ownerID;
            this.members = members;
            this.allies = allies;
            home = new FactionHome();
        }

        [JsonConstructor]
        public Faction(string name, string owner, ulong ownerID, FactionMembers members, List<string> allies, FactionHome home)
        {
            this.name = name;
            this.owner = owner;
            this.ownerID = ownerID;
            this.members = members;
            this.allies = allies;
            this.home = home;
        }

        public FactionMember GetMember(ulong userID)
        {
            return members.Get(userID);
        }

        public void AddAlly(string allyName)
        {
            Faction faction = this;
            Faction ally = Vars.factions.GetByName(allyName);
            Vars.factions.Remove(ally.name, false);
            ally.allies.Add(faction.name);
            Vars.factions.Add(ally);
            Vars.factions.Remove(this.name, false);
            faction.allies.Add(ally.name);
            Vars.factions.Add(faction);
        }

        public void RemoveAlly(string allyName)
        {
            Faction faction = this;
            Faction ally = Vars.factions.GetByName(allyName);
            Vars.factions.Remove(ally.name, false);
            ally.allies.Remove(faction.name);
            Vars.factions.Add(ally);
            Vars.factions.Remove(this.name, false);
            faction.allies.Remove(ally.name);
            Vars.factions.Add(faction);
        }

        public void RemoveMember(ulong userID)
        {
            Faction faction = this;
            Vars.factions.Remove(this.name, false);
            faction.members.Remove(GetMember(userID));
            Vars.factions.Add(faction);
        }

        public void AddMember(string userName, string rank, ulong userID)
        {
            Faction faction = this;
            Vars.factions.Remove(this.name, false);
            faction.members.Add(new FactionMember(userName, rank, userID));
            Vars.factions.Add(faction);
        }

        public void SetRank(ulong userID, string rank)
        {
            FactionMember member = GetMember(userID);
            members.Remove(member);
            member.rank = rank;
            if (rank == "owner")
            {
                Faction faction = new Faction(this.name, member.name, userID, this.members, this.allies);
                Vars.factions.Remove(this.name, false);
                Vars.factions.Add(faction);
            }
            members.Add(member);
        }
    }

    public class FactionHome
    {
        public float x;
        public float y;
        public float z;
        [JsonIgnore]
        public Vector3 origin;

        public FactionHome()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
            origin = new Vector3();
        }

        public FactionHome(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            origin = new Vector3(x, y, z);
        }

        public FactionHome(Vector3 vector)
        {
            this.x = vector.x;
            this.y = vector.y;
            this.z = vector.z;
            origin = vector;
        }
    }

    public class FactionMember
    {
        public string name;
        public string rank;
        public ulong userID;

        public FactionMember(string name, string rank, ulong userID)
        {
            this.name = name;
            this.rank = rank;
            this.userID = userID;
        }
    }

    public class FactionList : List<Faction>
    {
        public Faction GetByOwner(ulong ownerID)
        {
            foreach (var obj in this)
            {
                if (obj.ownerID == ownerID)
                    return obj;
            }

            return null;
        }

        public FactionList GetListByName(string factionName)
        {
            FactionList list = new FactionList();
            foreach (var obj in this)
            {
                if (obj.name == factionName)
                    list.Add(obj);
            }

            return list;
        }

        public Faction GetByName(string factionName)
        {
            foreach (var obj in this)
            {
                if (obj.name == factionName)
                    return obj;
            }

            return null;
        }

        public Faction GetByMember(ulong memberID)
        {
            foreach (var obj in this)
            {
                if (obj.members.Count > 0)
                {
                    foreach (var member in obj.members)
                    {
                        if (member.userID == memberID)
                            return obj;
                    }
                }
            }

            return null;
        }

        public void Remove(string factionName, bool deleteAllies = true)
        {
            List<string> allies = null;
            foreach (var obj in this)
            {
                if (obj.name == factionName)
                {
                    allies = obj.allies;
                    if (deleteAllies)
                    {
                        foreach (var ally in obj.allies)
                        {
                            Faction allyFaction = Vars.factions.GetByName(ally);
                            allyFaction.allies.Remove(factionName);
                        }
                    }
                    this.Remove(obj);
                    return;
                }
            }
        }
    }

    public class FactionMembers : List<FactionMember>
    {
        public FactionMember Get(ulong userID)
        {
            foreach (var obj in this)
            {
                if (obj.userID == userID)
                    return obj;
            }

            return null;
        }
    }
}
