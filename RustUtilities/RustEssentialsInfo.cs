/**
 * @file: RustEssentialsInfo.cs
 * @author: Team Cerionn (https://github.com/Team-Cerionn)
 * @version: 1.0.0.0
 * @description: Information class for Rust Essentials
 */
using LeatherLoader.ModList;
using RustEssentials.Util;

namespace RustEssentials
{
    public class RustEssentialsInfo : IModInfo
    {
        public string GetModName()
        {
            return "Rust-Essentials";
        }

        public string GetModVersion()
        {
            return Vars.currentVersion;
        }

        public string GetPrettyModName()
        {
            return "Rust Essentials";
        }

        public string GetPrettyModVersion()
        {
            return "Version " + Vars.currentVersion;
        }

        public bool CanAcceptModlessClients()
        {
            return true;
        }

        public bool CanConnectToModlessServers()
        {
            return true;
        }

        public string GetCreditString()
        {
            return "By Team Cerionn";
        }
    }
}
