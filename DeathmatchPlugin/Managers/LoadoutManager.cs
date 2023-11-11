using CounterStrikeSharp.API.Core;
using DeathmatchPlugin.Guns;
using DeathmatchPlugin.Utilities;

namespace DeathmatchPlugin.Managers;

public class LoadoutManager : LifeCycle
{
    private Dictionary<ulong, WeaponLoadout> Loadouts { get; set; } = null!;
    private HashSet<ulong> LoadoutsToUpdate { get; set; } = null!;

    private bool _lockUpdates;

    public override void Init()
    {
        Loadouts = new Dictionary<ulong, WeaponLoadout>();
        LoadoutsToUpdate = new HashSet<ulong>();
    }

    public override void Cleanup()
    {
        LoadoutsToUpdate.Clear();
        Loadouts.Clear();
    }

    public void OnPlayerConnect(EventPlayerConnectFull @event)
    {
        Loadouts[@event.Userid.SteamID] = WeaponLoadout.Default(@event.Userid);
    }

    public void OnPlayerDisconnect(EventPlayerDisconnect @event)
    {
        Loadouts.Remove(@event.Userid.SteamID);
    }

    public void OnPlayerSpawn(EventPlayerSpawn @event)
    {
        var player = @event.Userid;
        var loadout = GetLoadout(player);

        player.GiveNamedItem("weapon_knife");
        player.GiveNamedItem("item_assaultsuit");
        player.GiveNamedItem(loadout.SecondaryWeapon.Slug);
        if (loadout.PrimaryWeapon != null) player.GiveNamedItem(loadout.PrimaryWeapon.Slug);
        if (loadout.SpecialWeapon != null) player.GiveNamedItem(loadout.SpecialWeapon.Slug);
    }

    public void OnChooseWeapon(CCSPlayerController player, Weapon weapon)
    {
        var loadout = GetLoadout(player);

        player.PrintToCenter(
            $"You have selected {weapon.DisplayName} as your {weapon.Slot} weapon\nThis will take effect on your next spawn");

        if (weapon.Slot.IsPrimary) loadout.PrimaryWeapon = weapon;
        else if (weapon.Slot.IsSecondary) loadout.SecondaryWeapon = weapon;
        else if (weapon.Slot.IsSpecial) loadout.SpecialWeapon = weapon;
    }

    public void OnPurchaseWeapon(CCSPlayerController player)
    {
        LoadoutsToUpdate.Add(player.SteamID);
    }

    public void FlushOutdatedLoadouts()
    {
        if (_lockUpdates) return;
        _lockUpdates = true;

        Logging.LogTrace($"Flushing outdated loadouts ({LoadoutsToUpdate.Count})");

        foreach (var steamId in LoadoutsToUpdate)
        {
            if (!PlayerUtilities.TryFindPlayerBySteamId(steamId, out var player))
            {
                Logging.LogWarn($"[UpdateLoadouts] steamId={steamId}");
                LoadoutsToUpdate.Remove(steamId);

                continue; // TODO: Remove from list
            }

            if (!player.PlayerPawn.IsValid)
            {
                Logging.LogWarn("[UpdateLoadouts] InvalidPlayerPawn");
                LoadoutsToUpdate.Remove(steamId);

                continue;
            }

            var weaponServices = player.PlayerPawn.Value.WeaponServices;
            if (weaponServices == null)
            {
                Logging.LogWarn("[UpdateLoadouts] AcquireWeaponServices");
                LoadoutsToUpdate.Remove(steamId);

                continue;
            }

            foreach (var playerWeapon in weaponServices.MyWeapons)
            {
                if (!playerWeapon.IsValid)
                {
                    Logging.LogWarn("[UpdateLoadouts][WeaponsIter] InvalidWeaponHandle");
                    LoadoutsToUpdate.Remove(steamId);

                    continue;
                }

                try
                {
                    var weaponName = playerWeapon.Value.DesignerName;
                    var itemDefinitionIndex = playerWeapon.Value.AttributeManager.Item.ItemDefinitionIndex;
                    Logging.LogTrace($"weaponName={weaponName};idi={itemDefinitionIndex}");
                    var weapon = new Weapon(weaponName, itemDefinitionIndex);
                    Logging.LogTrace($"Setting {player.PlayerName}'s {weapon.Slot} weapon to {weapon.Slug}");
                    OnChooseWeapon(player, weapon);
                }
                catch (Exception e)
                {
                    Logging.LogError(e.Message);
                }
            }

            LoadoutsToUpdate.Remove(steamId);
        }

        _lockUpdates = false;
    }

    public WeaponLoadout GetLoadout(CCSPlayerController player)
    {
        FlushOutdatedLoadouts();

        if (Loadouts.TryGetValue(player.SteamID, out var loadout))
            return loadout;

        var newLoadout = new WeaponLoadout();
        Loadouts.Add(player.SteamID, newLoadout);
        return newLoadout;
    }
}