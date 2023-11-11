using CounterStrikeSharp.API;
using DeathmatchPlugin.Config;
using DeathmatchPlugin.Extensions;
using DeathmatchPlugin.Utilities;

namespace DeathmatchPlugin.Killstreaks;

public class KillstreakLogger : KillstreakSubscriber
{
    private static readonly Dictionary<ulong, string> KillstreakMessages = new Dictionary<ulong, string>
    {
        { 5, $"is on a {Colored.Red("Killing Spree")}" },
        { 10, $"is on a {Colored.Red("Rampage")}" },
        { 15, $"is {Colored.Red("Dominating")}" },
        { 20, $"is {Colored.Red("Unstoppable")}" },
        { 25, $"is {Colored.Red("Godlike")}" },
    };

    public override void OnKillstreak(ulong steamId, ulong kills)
    {
        if (!PlayerUtilities.TryFindPlayerBySteamId(steamId, out var player))
        {
            Logging.LogWarn("Got bad killstreak message: player not found");
            return;
        }

        if (!KillstreakMessages.TryGetValue(kills, out var message)) return;

        Server.PrintToChatAll($"{DeathmatchConfig.ChatPrefix} {player.TeamColoredPlayerName()} {message}");
    }
}