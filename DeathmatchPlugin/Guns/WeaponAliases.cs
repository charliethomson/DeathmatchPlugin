namespace DeathmatchPlugin.Guns;

public class WeaponAliases
{
    public const string AK = "ak";
    public const string AK47 = "ak47";
    public const string A4 = "a4";
    public const string M4A4 = "m4a4";
    public const string A1 = "a1";
    public const string A1S = "a1s";
    public const string M4A1S = "m4a1s";
    public const string CZ = "cz";
    public const string CZ75 = "cz75";
    public const string Deag = "deag";
    public const string Dualies = "dualies";
    public const string P2K = "p2k";
    public const string P2000 = "p2000";
    public const string R8 = "r8";
    public const string USP = "usp";
    public const string G3 = "g3";
    public const string Galil = "galil";
    public const string Scar = "scar";
    public const string SG = "sg";
    public const string Kreig = "kreig";
    public const string Scout = "scout";
    public const string PP = "pp";
    public const string PPBizon = "ppbizon";
    public const string MP5 = "mp5";
    public const string UMP = "ump";
    public const string XM = "xm";
    public const string Zeus = "zeus";


    public static readonly string[] AllWeaponAliases = WeaponList.AllWeaponSlugs.Concat(new[]
    {
        AK,
        AK47,
        A4,
        M4A4,
        A1,
        A1S,
        M4A1S,
        CZ,
        CZ75,
        Deag,
        Dualies,
        P2K,
        P2000,
        R8,
        USP,
        G3,
        Galil,
        Scar,
        SG,
        Kreig,
        Scout,
        PP,
        PPBizon,
        MP5,
        UMP,
        XM,
        Zeus,
    }).ToArray();

    private static string? TryResolveAlias(string weaponName)
    {
        return weaponName switch
        {
            AK => WeaponList.Ak47,
            A4 => WeaponList.M4A1,
            M4A4 => WeaponList.M4A1,
            A1 => WeaponList.M4A1Silencer,
            A1S => WeaponList.M4A1Silencer,
            M4A1S => WeaponList.M4A1Silencer,
            CZ => WeaponList.CZ75a,
            CZ75 => WeaponList.CZ75a,
            Deag => WeaponList.Deagle,
            Dualies => WeaponList.Elite,
            P2K => WeaponList.HKP2000,
            P2000 => WeaponList.HKP2000,
            R8 => WeaponList.Revolver,
            USP => WeaponList.USPSilencer,
            G3 => WeaponList.G3SG1,
            Galil => WeaponList.GalilAR,
            Scar => WeaponList.Scar20,
            SG => WeaponList.SG556,
            Kreig => WeaponList.SG556,
            Scout => WeaponList.SSG08,
            PP => WeaponList.Bizon,
            PPBizon => WeaponList.Bizon,
            MP5 => WeaponList.MP5SD,
            UMP => WeaponList.UMP45,
            XM => WeaponList.Xm1014,
            Zeus => WeaponList.Taser,
            _ => null
        };
    }
}