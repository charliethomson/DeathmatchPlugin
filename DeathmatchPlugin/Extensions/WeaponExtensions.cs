using CounterStrikeSharp.API.Core;

namespace DeathmatchPlugin.Extensions
{
    public static class WeaponExtensions
    {
        private static readonly HashSet<string> WeaponIgnoreList = new()
        {
            "weapon_knife",
            "weapon_c4",
            "weapon_smokegrenade",
            "weapon_flashbang",
            "weapon_hegrenade",
            "weapon_molotov",
            "weapon_decoy"
        };

        public static bool IsContrabandWeapon(this CBasePlayerWeapon? weapon, out CBasePlayerWeapon contrabandWeapon)
        {
            contrabandWeapon = null!;
            if (weapon == null) return false;
            contrabandWeapon = weapon;
            return !WeaponIgnoreList.Contains(weapon.DesignerName);
        }
    }
}