using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Events;
using DeathmatchPlugin.Config;
using DeathmatchPlugin.Effects;
using DeathmatchPlugin.Extensions;
using DeathmatchPlugin.Guns;
using DeathmatchPlugin.Managers;
using DeathmatchPlugin.Utilities;
using CSSUtilities = CounterStrikeSharp.API.Utilities;
using CSSTimer = CounterStrikeSharp.API.Modules.Timers.Timer;
using CSSTimerFlags = CounterStrikeSharp.API.Modules.Timers.TimerFlags;

namespace DeathmatchPlugin;

public class DeathmatchPlugin : BasePlugin
{
    public override string ModuleName => "Deathmatch Plugin";
    public override string ModuleVersion => "v1.0.0";

    private LoadoutManager _loadoutManager = new();


    public override void Load(bool hotReload)
    {
        _loadoutManager.Init();
        RegisterLoadoutChangeCommands();
    }

    public override void Unload(bool hotReload)
    {
        UnregisterLoadoutChangeCommands();
        _loadoutManager.Cleanup();
    }

    public void CommandChangeLoadout(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null || !player.IsOnATeam())
        {
            Logging.LogWarn("Player doesnt exist or is not on a team, not changing loadout");
            return;
        }

        var alias = command.GetArg(0);
        var weapon = new Weapon(alias);
        _loadoutManager.OnChooseWeapon(player, weapon);
    }

    public void RegisterLoadoutChangeCommands()
    {
        foreach (var alias in WeaponAliases.AllWeaponAliases)
        {
            var weapon = new Weapon(alias);
            var aliasSpecificDescription = $"Set your {weapon.Slot} weapon to {weapon.Slug}";

            AddCommand(alias, aliasSpecificDescription, CommandChangeLoadout);
            Logging.Log($"{ChatConfig.ChatPrefix}: Registered command \"{alias}\" => \"{weapon.Slug}\"");
        }
    }

    public void UnregisterLoadoutChangeCommands()
    {
        foreach (var alias in WeaponAliases.AllWeaponAliases)
        {
            RemoveCommand(alias, CommandChangeLoadout);
            Logging.Log($"{ChatConfig.ChatPrefix}: Registered command \"{alias}\"");
        }
    }

    [GameEventHandler]
    public HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
    {
        var attacker = @event.Attacker;
        var target = @event.Userid;
        if (!(attacker.IsOnATeam() && target.IsOnATeam())) return HookResult.Continue;
        if (attacker.SteamID == target.SteamID)
        {
            Logging.Log("Suicide");
            return HookResult.Continue;
        }

        RefillPlayerAmmo.Do(attacker);

        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (!player.IsOnATeam()) return HookResult.Continue;

        _loadoutManager.OnPlayerSpawn(@event);

        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnPlayerConnectFull(EventPlayerConnectFull @event, GameEventInfo info)
    {
        _loadoutManager.OnPlayerConnect(@event);
        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo info)
    {
        _loadoutManager.OnPlayerDisconnect(@event);
        return HookResult.Continue;
    }


    [ConsoleCommand("loadout", "Show your current loadout")]
    public void OnLoadout(CCSPlayerController? player, CommandInfo command)
    {
        if (CommandUtilities.ClientCommand(player)) return;

        var loadout = _loadoutManager.GetLoadout(player!);
        var loadoutDisplay = "";

        loadoutDisplay += $"Primary: {loadout?.PrimaryWeapon?.DisplayName ?? "None"}";
        loadoutDisplay += $"\nSecondary: {loadout?.SecondaryWeapon?.DisplayName ?? "None"}";
        if (!string.IsNullOrEmpty(loadout?.SpecialWeapon?.DisplayName))
            loadoutDisplay += $"\nSpecial: {loadout.SpecialWeapon.DisplayName}";

        player!.PrintToCenter(loadoutDisplay);
    }

    [ConsoleCommand("guns", "List weapon names and the command to choose them")]
    public void OnGuns(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null || !player.IsOnATeam()) return;
        WeaponList.DebugWeaponNameConsole(player);
    }
}