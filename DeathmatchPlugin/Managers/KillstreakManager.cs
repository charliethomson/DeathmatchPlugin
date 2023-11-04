using CounterStrikeSharp.API.Core;
using DeathmatchPlugin.Killstreaks;
using DeathmatchPlugin.Utilities;

namespace DeathmatchPlugin.Managers;

public class KillstreakManager : LifeCycle
{
    private Dictionary<ulong, ulong> _killstreaks = new();
    private Dictionary<string, KillstreakSubscriber> _subscribers = new();

    public void Subscribe(string subscriberName, KillstreakSubscriber subscriber)
    {
        if (_subscribers.ContainsKey(subscriberName))
            throw new ArgumentOutOfRangeException(nameof(subscriberName), subscriberName,
                $"A subscriber named {subscriberName} is already registered");

        _subscribers.Add(subscriberName, subscriber);
    }

    public void Unsubscribe(string subscriberName)
    {
        if (!_subscribers.ContainsKey(subscriberName)) // TODO: Probably just a Logging.LogWarn
            throw new ArgumentOutOfRangeException(nameof(subscriberName), subscriberName,
                $"No subscriber named {subscriberName} is registered");

        _subscribers.Remove(subscriberName);
    }

    private void PublishMessage(ulong steamId, ulong kills)
    {
        foreach (var subscriber in _subscribers.Values)
            subscriber.OnKillstreak(steamId, kills);
    }


    public void OnKill(CCSPlayerController attacker)
    {
        var attackerKillStreak = _killstreaks.GetValueOrDefault(attacker.SteamID) + 1;

        _killstreaks[attacker.SteamID] = attackerKillStreak;

        PublishMessage(attacker.SteamID, attackerKillStreak);
    }

    public void OnPlayerDeath(CCSPlayerController target)
    {
        _killstreaks[target.SteamID] = 0;
        PublishMessage(target.SteamID, 0);
    }

    public override void Init()
    {
        _killstreaks = new();
    }

    public override void Cleanup()
    {
        _killstreaks = new();
    }
}