namespace DeathmatchPlugin.Guns;

public class Weapon
{
    public string Slug { get; set; } // weapon_ak47
    public string Name { get; set; } // ak47
    public string DisplayName { get; set; } // AK-47
    public WeaponSlot Slot { get; set; } // primary
    public HashSet<string> AltNames => WeaponRelations.AliasMap[Name];

    public Weapon(string alias /* ak47, ak */, ushort? itemDefinitionIndex = null)
    {
        if (itemDefinitionIndex.HasValue) alias = DisambiguateWeapon(alias, itemDefinitionIndex.Value);
        var weaponName = WeaponRelations.ResolveWeaponName(alias);
        if (string.IsNullOrEmpty(weaponName))
            throw new ArgumentOutOfRangeException(nameof(alias), $"{alias} is not a valid weapon name");

        Slot = new WeaponSlot(weaponName);
        DisplayName = WeaponDisplayName.GetWeaponDisplayName(weaponName);
        Name = weaponName;
        Slug = $"weapon_{weaponName}";
    }

    private string DisambiguateWeapon(string alias, ushort itemDefinitionIndex)
    {
        return itemDefinitionIndex switch
        {
            16 => WeaponList.M4A1,
            60 => WeaponList.M4A1Silencer,
            32 => WeaponList.HKP2000,
            61 => WeaponList.USPSilencer,
            23 => WeaponList.MP5SD,
            33 => WeaponList.MP7,
            _ => alias
        };
    }
}