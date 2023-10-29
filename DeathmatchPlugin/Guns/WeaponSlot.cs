namespace DeathmatchPlugin.Guns;

public enum WeaponSlotInner
{
    Primary,
    Secondary,
    Special
}

public class WeaponSlot
{
    public WeaponSlotInner Slot;

    public override string ToString()
    {
        return Slot switch
        {
            WeaponSlotInner.Primary => "Primary",
            WeaponSlotInner.Secondary => "Secondary",
            WeaponSlotInner.Special => "Special",
            _ => throw new ArgumentOutOfRangeException(nameof(Slot), Slot, null)
        };
    }

    public bool IsPrimary => Slot == WeaponSlotInner.Primary;
    public bool IsSecondary => Slot == WeaponSlotInner.Secondary;
    public bool IsSpecial => Slot == WeaponSlotInner.Special;

    public WeaponSlot(string weaponName)
    {
        if (!WeaponList.IsValidWeapon(weaponName)) throw new ArgumentOutOfRangeException(nameof(weaponName));

        Slot = GetSlotForWeapon(weaponName);
    }

    private static WeaponSlotInner GetSlotForWeapon(string weaponName)
    {
        switch (weaponName)
        {
            case WeaponList.Taser:
                return WeaponSlotInner.Special;
            case WeaponList.M249:
            case WeaponList.Mag7:
            case WeaponList.Negev:
            case WeaponList.Nova:
            case WeaponList.SawedOff:
            case WeaponList.Xm1014:
            case WeaponList.Ak47:
            case WeaponList.Aug:
            case WeaponList.Awp:
            case WeaponList.Famas:
            case WeaponList.G3SG1:
            case WeaponList.GalilAR:
            case WeaponList.M4A1:
            case WeaponList.M4A1Silencer:
            case WeaponList.Scar20:
            case WeaponList.SG556:
            case WeaponList.SSG08:
            case WeaponList.Bizon:
            case WeaponList.Mac10:
            case WeaponList.MP5SD:
            case WeaponList.MP7:
            case WeaponList.MP9:
            case WeaponList.P90:
            case WeaponList.UMP45:
                return WeaponSlotInner.Primary;
            case WeaponList.CZ75a:
            case WeaponList.Deagle:
            case WeaponList.Elite:
            case WeaponList.FiveSeven:
            case WeaponList.Glock:
            case WeaponList.HKP2000:
            case WeaponList.P250:
            case WeaponList.Revolver:
            case WeaponList.Tec9:
            case WeaponList.USPSilencer:
                return WeaponSlotInner.Secondary;
            default:
                throw new ArgumentOutOfRangeException(nameof(weaponName));
        }
    }
}