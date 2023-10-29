namespace DeathmatchPlugin.Guns;

public static class WeaponDisplayName
{
    public static string GetWeaponDisplayName(string weaponName)
    {
        return weaponName switch
        {
            WeaponList.Taser => "Zeus",
            WeaponList.M249 => "M249",
            WeaponList.Mag7 => "MAG-7",
            WeaponList.Negev => "Negev",
            WeaponList.Nova => "Nova",
            WeaponList.SawedOff => "Sawed-Off",
            WeaponList.Xm1014 => "XM1014",
            WeaponList.CZ75a => "CZ75",
            WeaponList.Deagle => "Desert Eagle",
            WeaponList.Elite => "Dual Berettas",
            WeaponList.FiveSeven => "Five-SeveN",
            WeaponList.Glock => "Glock-18",
            WeaponList.HKP2000 => "P2000",
            WeaponList.P250 => "P250",
            WeaponList.Revolver => "R8 Revolver",
            WeaponList.Tec9 => "Tec-9",
            WeaponList.USPSilencer => "USP-S",
            WeaponList.Ak47 => "AK-47",
            WeaponList.Aug => "AUG",
            WeaponList.Awp => "AWP",
            WeaponList.Famas => "Famas",
            WeaponList.G3SG1 => "G3SG1",
            WeaponList.GalilAR => "Galil",
            WeaponList.M4A1 => "M4A4",
            WeaponList.M4A1Silencer => "M4A1-S",
            WeaponList.Scar20 => "SCAR-20",
            WeaponList.SG556 => "SG 556",
            WeaponList.SSG08 => "SSG-08",
            WeaponList.Bizon => "PP Bizon",
            WeaponList.Mac10 => "MAC-10",
            WeaponList.MP5SD => "MP5-SD",
            WeaponList.MP7 => "MP7",
            WeaponList.MP9 => "MP9",
            WeaponList.P90 => "P90",
            WeaponList.UMP45 => "UMP-45",
            _ => throw new ArgumentOutOfRangeException(nameof(weaponName), weaponName, null)
        };
    }
}