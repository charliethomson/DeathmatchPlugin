using CounterStrikeSharp.API.Core;
using DeathmatchPlugin.Extensions;

namespace DeathmatchPlugin.Effects;

public static class RefillPlayerAmmo
{
    private static readonly HashSet<string> RefillDenyList = new()
    {
        "weapon_negev",
    };

    private static bool ShouldRefillPlayerAmmo(CEntityInstance weapon)
    {
        return !RefillDenyList.Contains(weapon.DesignerName);
    }

    public static void Do(CCSPlayerController player)
    {
        var activeWeapon = player.ActiveWeapon();
        if (activeWeapon == null) return;
        if (!ShouldRefillPlayerAmmo(activeWeapon)) return;
        activeWeapon.Clip1 = 250;
        activeWeapon.ReserveAmmo[0] = 250;
    }
}