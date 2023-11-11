using CounterStrikeSharp.API.Core;
using DeathmatchPlugin.Extensions;

namespace DeathmatchPlugin.Guns;

public class WeaponLoadout
{
    public Weapon? PrimaryWeapon;
    public Weapon SecondaryWeapon = new Weapon("glock");
    public Weapon? SpecialWeapon;

    public static WeaponLoadout Default(CCSPlayerController player)
    {
        if (player.IsTerrorist())
            return new WeaponLoadout
            {
                PrimaryWeapon = new Weapon(WeaponList.Ak47),
                SecondaryWeapon = new Weapon(WeaponList.Deagle)
            };

        if (player.IsCounterTerrorist())
            return new WeaponLoadout
            {
                PrimaryWeapon = new Weapon(WeaponList.M4A1Silencer),
                SecondaryWeapon = new Weapon(WeaponList.Deagle)
            };

        return new WeaponLoadout();
    }
}