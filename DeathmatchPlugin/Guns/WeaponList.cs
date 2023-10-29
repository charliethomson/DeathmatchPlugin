using CounterStrikeSharp.API.Core;

namespace DeathmatchPlugin.Guns;

public static class WeaponList
{
    public const string Taser = "taser";
    public const string M249 = "m249";
    public const string Mag7 = "mag7";
    public const string Negev = "negev";
    public const string Nova = "nova";
    public const string SawedOff = "sawedoff";
    public const string Xm1014 = "xm1014";
    public const string CZ75a = "cz75a";
    public const string Deagle = "deagle";
    public const string Elite = "elite";
    public const string FiveSeven = "fiveseven";
    public const string Glock = "glock";
    public const string HKP2000 = "hkp2000";
    public const string P250 = "p250";
    public const string Revolver = "revolver";
    public const string Tec9 = "tec9";
    public const string USPSilencer = "usp_silencer";
    public const string Ak47 = "ak47";
    public const string Aug = "aug";
    public const string Awp = "awp";
    public const string Famas = "famas";
    public const string G3SG1 = "g3sg1";
    public const string GalilAR = "galilar";
    public const string M4A1 = "m4a1";
    public const string M4A1Silencer = "m4a1_silencer";
    public const string Scar20 = "scar20";
    public const string SG556 = "sg556";
    public const string SSG08 = "ssg08";
    public const string Bizon = "bizon";
    public const string Mac10 = "mac10";
    public const string MP5SD = "mp5sd";
    public const string MP7 = "mp7";
    public const string MP9 = "mp9";
    public const string P90 = "p90";
    public const string UMP45 = "ump45";

    public static readonly string[] AllWeaponSlugs = new[]
    {
        Taser,
        M249,
        Mag7,
        Negev,
        Nova,
        SawedOff,
        Xm1014,
        CZ75a,
        Deagle,
        Elite,
        FiveSeven,
        Glock,
        HKP2000,
        P250,
        Revolver,
        Tec9,
        USPSilencer,
        Ak47,
        Aug,
        Awp,
        Famas,
        G3SG1,
        GalilAR,
        M4A1,
        M4A1Silencer,
        Scar20,
        SG556,
        SSG08,
        Bizon,
        Mac10,
        MP5SD,
        MP7,
        MP9,
        P90,
        UMP45,
    };

    public static bool IsValidWeapon(string weaponName)
    {
        return AllWeaponSlugs.Contains(weaponName);
    }

    public static void DebugWeaponNameConsole(CCSPlayerController player)
    {
        foreach (var weaponName in WeaponList.AllWeaponSlugs)
        {
            var alts = WeaponRelations.AliasMap.GetValueOrDefault(weaponName) ?? new HashSet<string>();
            alts.Add(weaponName);

            player.PrintToConsole($"weapon_{weaponName}:\n");
            foreach (var alt in alts)
            {
                player.PrintToConsole($"\t/{alt}\n");
            }

            player.PrintToConsole("\n");
        }
    }
}