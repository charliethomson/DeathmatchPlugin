namespace DeathmatchPlugin.Guns;

public class WeaponLoadout
{
    public Weapon? PrimaryWeapon;
    public Weapon SecondaryWeapon = new Weapon("glock");
    public Weapon? SpecialWeapon;
}