namespace DeathmatchPlugin.Guns;

public class WeaponRelations
{
    public static readonly Dictionary<string, HashSet<string>> AliasMap = new()
    {
        { WeaponList.Ak47, new HashSet<string> { WeaponAliases.AK } },
        { WeaponList.M4A1, new HashSet<string> { WeaponAliases.M4A4, WeaponAliases.A4 } },
        { WeaponList.M4A1Silencer, new HashSet<string> { WeaponAliases.M4A1S, WeaponAliases.A1, WeaponAliases.A1S } },
        { WeaponList.CZ75a, new HashSet<string> { WeaponAliases.CZ, WeaponAliases.CZ75 } },
        { WeaponList.Deagle, new HashSet<string> { WeaponAliases.Deag } },
        { WeaponList.Elite, new HashSet<string> { WeaponAliases.Dualies } },
        { WeaponList.HKP2000, new HashSet<string> { WeaponAliases.P2K, WeaponAliases.P2000 } },
        { WeaponList.Revolver, new HashSet<string> { WeaponAliases.R8 } },
        { WeaponList.USPSilencer, new HashSet<string> { WeaponAliases.USP } },
        { WeaponList.G3SG1, new HashSet<string> { WeaponAliases.G3 } },
        { WeaponList.GalilAR, new HashSet<string> { WeaponAliases.Galil } },
        { WeaponList.Scar20, new HashSet<string> { WeaponAliases.Scar } },
        { WeaponList.SG556, new HashSet<string> { WeaponAliases.SG, WeaponAliases.Kreig } },
        { WeaponList.SSG08, new HashSet<string> { WeaponAliases.Scout } },
        { WeaponList.Bizon, new HashSet<string> { WeaponAliases.PP, WeaponAliases.PPBizon } },
        { WeaponList.MP5SD, new HashSet<string> { WeaponAliases.MP5 } },
        { WeaponList.UMP45, new HashSet<string> { WeaponAliases.UMP } },
        { WeaponList.Xm1014, new HashSet<string> { WeaponAliases.XM } },
        { WeaponList.Taser, new HashSet<string> { WeaponAliases.Zeus } },
    };

    private static string? TryGetMappedWeaponName(string weaponName)
    {
        return weaponName switch
        {
            WeaponAliases.AK => WeaponList.Ak47,
            WeaponAliases.A4 => WeaponList.M4A1,
            WeaponAliases.M4A4 => WeaponList.M4A1,
            WeaponAliases.A1 => WeaponList.M4A1Silencer,
            WeaponAliases.A1S => WeaponList.M4A1Silencer,
            WeaponAliases.M4A1S => WeaponList.M4A1Silencer,
            WeaponAliases.CZ => WeaponList.CZ75a,
            WeaponAliases.CZ75 => WeaponList.CZ75a,
            WeaponAliases.Deag => WeaponList.Deagle,
            WeaponAliases.Dualies => WeaponList.Elite,
            WeaponAliases.P2K => WeaponList.HKP2000,
            WeaponAliases.P2000 => WeaponList.HKP2000,
            WeaponAliases.R8 => WeaponList.Revolver,
            WeaponAliases.USP => WeaponList.USPSilencer,
            WeaponAliases.G3 => WeaponList.G3SG1,
            WeaponAliases.Galil => WeaponList.GalilAR,
            WeaponAliases.Scar => WeaponList.Scar20,
            WeaponAliases.SG => WeaponList.SG556,
            WeaponAliases.Kreig => WeaponList.SG556,
            WeaponAliases.Scout => WeaponList.SSG08,
            WeaponAliases.PP => WeaponList.Bizon,
            WeaponAliases.PPBizon => WeaponList.Bizon,
            WeaponAliases.MP5 => WeaponList.MP5SD,
            WeaponAliases.UMP => WeaponList.UMP45,
            WeaponAliases.XM => WeaponList.Xm1014,
            WeaponAliases.Zeus => WeaponList.Taser,
            _ => null
        };
    }

    public static string? ResolveWeaponName(string weaponName)
    {
        weaponName = weaponName.ToLower();

        if (weaponName.StartsWith("weapon_")) weaponName = weaponName.Split("weapon_")[1];
        if (WeaponList.IsValidWeapon(weaponName)) return weaponName;

        var mappedWeaponName = TryGetMappedWeaponName(weaponName);
        return !string.IsNullOrEmpty(mappedWeaponName) ? mappedWeaponName : null;
    }
}