using CounterStrikeSharp.API;
using DeathmatchPlugin.Config;
using DeathmatchPlugin.Extensions;
using DeathmatchPlugin.Utilities;

namespace DeathmatchPlugin.Killstreaks;

public class MultiKillSubscriber : KillstreakSubscriber
{
    private class KillChainLink
    {
        public KillChainLink(KillChainLink? chain)
        {
            Depth = chain == null ? 0 : chain.Depth + 1;
            Last = chain;
            At = DateTime.Now;
        }

        public DateTime At { get; set; }
        public KillChainLink? Last { get; set; }
        public int Depth { get; set; }
    }

    private static readonly string[] MultiKillMessages =
    {
        "Kill",
        "Double kill",
        "Mega Kill",
        "Ultra Kill",
        "Monster kill",
    };

    private static readonly string[] MultiKillColors =
    {
        TextColor.Default,
        TextColor.Default,
        TextColor.LightOlive,
        TextColor.LightRed,
        TextColor.Red,
    };

    private Dictionary<ulong, KillChainLink> _chains = new();

    private KillChainLink? TryGetKillChainLink(ulong steamId)
    {
        if (!_chains.TryGetValue(steamId, out var chain)) return null;
        var delta = DateTime.Now - chain.At;

        if (delta > KillstreakConfig.MultiKillDelta)
        {
            _chains.Remove(steamId);
            return null;
        }

        return chain;
    }


    public override void OnKillstreak(ulong steamId, ulong kills)
    {
        if (kills == 0)
        {
            _chains.Remove(steamId);
            return;
        }

        if (!PlayerUtilities.TryFindPlayerBySteamId(steamId, out var player))
        {
            Logging.LogWarn("Got bad killstreak message: player not found");
            return;
        }

        var chain = TryGetKillChainLink(steamId);

        var nextChain = new KillChainLink(chain);

        _chains[steamId] = nextChain;
        var chainDepth = Math.Min(nextChain.Depth, MultiKillMessages.Length - 1);
        var message = MultiKillMessages[chainDepth];
        var extraMessage = chainDepth < 4 ? null : $"({nextChain.Depth + 1} Kills)";
        var color = MultiKillColors[chainDepth];

        if (nextChain.Depth > 0)
            Server.PrintToChatAll(
                $"{DeathmatchConfig.ChatPrefix} {player.TeamColoredPlayerName()} {color}{message} {extraMessage}{TextColor.Reset}");
    }

    public override void Init()
    {
        _chains = new Dictionary<ulong, KillChainLink>();
    }

    public override void Cleanup()
    {
        _chains = new Dictionary<ulong, KillChainLink>();
    }
}