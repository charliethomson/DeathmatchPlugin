using DeathmatchPlugin.Utilities;

namespace DeathmatchPlugin.Killstreaks;

public abstract class KillstreakSubscriber : LifeCycle
{
    public virtual void OnKillstreak(ulong steamId, ulong kills)
    {
    }
}