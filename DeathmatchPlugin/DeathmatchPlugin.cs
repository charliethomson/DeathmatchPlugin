using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using DeathmatchPlugin.Config;
using DeathmatchPlugin.Effects;
using DeathmatchPlugin.Extensions;
using DeathmatchPlugin.Guns;
using DeathmatchPlugin.Killstreaks;
using DeathmatchPlugin.Managers;
using DeathmatchPlugin.Utilities;
using CSSUtilities = CounterStrikeSharp.API.Utilities;
using CSSTimer = CounterStrikeSharp.API.Modules.Timers.Timer;
using CSSTimerFlags = CounterStrikeSharp.API.Modules.Timers.TimerFlags;

namespace DeathmatchPlugin;

public class DeathmatchPlugin : BasePlugin
{
    public override string ModuleName => "Deathmatch Plugin";
    public override string ModuleVersion => "v1.0.2";
    public override string ModuleAuthor => "Charlie Thomson <charlie@thmsn.dev>";

    public override string ModuleDescription => "The Mercury Gaming (gg/mercurygaming) Deathmatch plugin";

    private LoadoutManager _loadoutManager = new();

    private HealthShotSubscriber _healthShotSubscriber = new();
    private KillstreakLogger _killstreakLogger = new();
    private MultiKillSubscriber _multiKillSubscriber = new();

    private KillstreakManager _killstreakManager = new();


    private CSSTimer? _chatSpamTimer;
    private CSSTimer? _loopbackLoadoutsTimer;


    public override void Load(bool hotReload)
    {
        DeathmatchConfig.LoadConfig(ModulePath);

        _healthShotSubscriber.Init();
        _killstreakLogger.Init();
        _multiKillSubscriber.Init();

        _loadoutManager.Init();

        _killstreakManager.Init();
        _killstreakManager.Subscribe("healthShotSubscriber", _healthShotSubscriber);
        _killstreakManager.Subscribe("killstreakLogger", _killstreakLogger);
        _killstreakManager.Subscribe("multiKillSubscriber", _multiKillSubscriber);

        _chatSpamTimer?.Kill();
        _chatSpamTimer = new CSSTimer(30, ChatSpam.Do, CSSTimerFlags.REPEAT);

        _loopbackLoadoutsTimer?.Kill();
        _loopbackLoadoutsTimer = new CSSTimer(3, _loadoutManager.FlushOutdatedLoadouts, CSSTimerFlags.REPEAT);

        AddCommandListener("buy", OnPlayerPurchaseWeapon);

        RegisterLoadoutChangeCommands();
    }

    public override void Unload(bool hotReload)
    {
        UnregisterLoadoutChangeCommands();

        _loadoutManager.Cleanup();
        _killstreakManager.Unsubscribe("healthShotSubscriber");
        _killstreakManager.Unsubscribe("killstreakLogger");
        _killstreakManager.Unsubscribe("multiKillSubscriber");
        _chatSpamTimer?.Kill();
        _loopbackLoadoutsTimer?.Kill();
        _multiKillSubscriber.Cleanup();
        _killstreakLogger.Cleanup();
        _healthShotSubscriber.Cleanup();
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
            Logging.LogTrace($"{DeathmatchConfig.ChatPrefix}: Registered command \"{alias}\" => \"{weapon.Slug}\"");
        }
    }

    public void UnregisterLoadoutChangeCommands()
    {
        foreach (var alias in WeaponAliases.AllWeaponAliases)
        {
            RemoveCommand(alias, CommandChangeLoadout);
            Logging.LogTrace($"{DeathmatchConfig.ChatPrefix}: Registered command \"{alias}\"");
        }
    }

    private HookResult OnPlayerPurchaseWeapon(CCSPlayerController? player, CommandInfo commandinfo)
    {
        if (player == null) return HookResult.Continue;
        _loadoutManager.OnPurchaseWeapon(player);

        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
    {
        var attacker = @event.Attacker;
        var target = @event.Userid;
        if (!(attacker.IsOnATeam() && target.IsOnATeam())) return HookResult.Continue;

        _killstreakManager.OnPlayerDeath(target);
        if (attacker.SteamID == target.SteamID)
        {
            Logging.LogTrace("Suicide");
            return HookResult.Continue;
        }

        _killstreakManager.OnKill(attacker);

        RefillPlayerAmmo.Do(attacker);
        RefillPlayerHealth.onKill(attacker);

        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (!player.IsOnATeam()) return HookResult.Continue;

        _loadoutManager.OnPlayerSpawn(@event);
        PrintLoadoutToCenter(@event.Userid);

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

    public HookResult OnItemPickup(EventItemPurchase @event, GameEventInfo info)
    {
        Logging.LogWarn($"Item purchased?");
        // Logging.LogWarn($"{@event.Userid.PlayerName} equipped {@event.Item}");
        // Server.PrintToChatAll($"{@event.Userid.PlayerName} equipped {@event.Item}");
        return HookResult.Continue;
    }


    [ConsoleCommand("css_loadout", "Show your current loadout")]
    public void OnLoadout(CCSPlayerController? player, CommandInfo command)
    {
        if (CommandUtilities.ClientCommand(player)) return;
        PrintLoadoutToCenter(player!);
    }


    public void PrintLoadoutToCenter(CCSPlayerController player)
    {
        var loadout = _loadoutManager.GetLoadout(player);
        var loadoutDisplay = "";

        loadoutDisplay += $"Primary: {loadout.PrimaryWeapon?.DisplayName ?? "None"}";
        loadoutDisplay += $"\nSecondary: {loadout.SecondaryWeapon.DisplayName}";
        if (!string.IsNullOrEmpty(loadout.SpecialWeapon?.DisplayName))
            loadoutDisplay += $"\nSpecial: {loadout.SpecialWeapon.DisplayName}";

        player.PrintToCenter(loadoutDisplay);
    }

    [ConsoleCommand("css_guns", "List weapon names and the command to choose them")]
    public void OnGuns(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null || !player.IsOnATeam()) return;
        WeaponList.DebugWeaponNameConsole(player);
    }

    [ConsoleCommand("css_dm_version", "Print the version of the DeathmatchPlugin")]
    public void OnVersion(CCSPlayerController? player, CommandInfo command)
    {
        Logging.Log($"{DeathmatchConfig.ChatPrefix} {ModuleName} - {ModuleVersion}");
        player?.PrintToChat($"{DeathmatchConfig.ChatPrefix} {ModuleName} - {ModuleVersion}");
    }

    [ConsoleCommand("css_reload_config")]
    public void OnReloadConfig(CCSPlayerController? player, CommandInfo info)
    {
        if (player != null) return;
        DeathmatchConfig.LoadConfig(ModulePath);
    }
}