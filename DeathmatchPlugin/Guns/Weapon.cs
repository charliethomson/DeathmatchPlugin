using DeathmatchPlugin.Utilities;

namespace DeathmatchPlugin.Guns;

public class Weapon
{
    public string Slug { get; set; } // weapon_ak47
    public string Name { get; set; } // ak47
    public string DisplayName { get; set; } // AK-47
    public WeaponSlot Slot { get; set; } // primary
    public HashSet<string> AltNames => WeaponRelations.AliasMap[Name];

    public Weapon(string alias /* ak47, ak */)
    {
        var weaponName = WeaponRelations.ResolveWeaponName(alias);
        if (string.IsNullOrEmpty(weaponName))
            throw new ArgumentOutOfRangeException(nameof(weaponName), $"{weaponName} is not a valid weapon name");

        Slot = new WeaponSlot(weaponName);
        DisplayName = WeaponDisplayName.GetWeaponDisplayName(weaponName);
        Name = weaponName;
        Slug = $"weapon_{weaponName}";
    }
}