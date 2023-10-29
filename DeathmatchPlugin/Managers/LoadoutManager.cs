using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using DeathmatchPlugin.Guns;
using DeathmatchPlugin.Utilities;

namespace DeathmatchPlugin.Managers;

public class LoadoutManager
{
    private Dictionary<ulong, WeaponLoadout> _loadouts { get; set; }

    public void Init()
    {
        _loadouts = new();
    }

    public void Cleanup()
    {
        _loadouts.Clear();
    }

    public void OnPlayerConnect(EventPlayerConnectFull @event)
    {
        _loadouts[@event.Userid.SteamID] = new();
    }

    public void OnPlayerDisconnect(EventPlayerDisconnect @event)
    {
        _loadouts.Remove(@event.Userid.SteamID);
    }

    public void OnPlayerSpawn(EventPlayerSpawn @event)
    {
        var player = @event.Userid;
        var loadout = GetLoadout(player);
        if (loadout == null)
        {
            Logging.LogWarn($"Loadout doesnt exist?");
            return;
        }

        Logging.LogWarn($"giving player weapon_knife");
        player.GiveNamedItem("weapon_knife");
        if (loadout.PrimaryWeapon != null)
        {
            Logging.LogWarn($"giving player {loadout.PrimaryWeapon.Slug}");
            player.GiveNamedItem(loadout.PrimaryWeapon.Slug);
        }

        Logging.LogWarn($"giving player {loadout.SecondaryWeapon.Slug}");
        player.GiveNamedItem(loadout.SecondaryWeapon.Slug);

        if (loadout.SpecialWeapon != null)
        {
            Logging.LogWarn($"giving player {loadout.SpecialWeapon.Slug}");
            player.GiveNamedItem(loadout.SpecialWeapon.Slug);
        }
    }

    public void OnChooseWeapon(CCSPlayerController player, Weapon weapon)
    {
        var loadout = GetLoadout(player);
        if (loadout == null)
        {
            Logging.LogWarn($"Loadout doesnt exist?");
            return;
        }

        if (weapon.Slot.IsPrimary) loadout.PrimaryWeapon = weapon;
        else if (weapon.Slot.IsSecondary) loadout.SecondaryWeapon = weapon;
        else if (weapon.Slot.IsSpecial) loadout.SpecialWeapon = weapon;
    }

    public WeaponLoadout? GetLoadout(CCSPlayerController player)
    {
        return _loadouts.GetValueOrDefault(player.SteamID);
    }
}