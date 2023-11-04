using DeathmatchPlugin.Config;
using DeathmatchPlugin.Utilities;

namespace DeathmatchPlugin.Killstreaks;

public class HealthShotSubscriber : KillstreakSubscriber
{
    private const string HealthShotEntityName = "weapon_healthshot";

    public override void OnKillstreak(ulong steamId, ulong kills)
    {
        if (kills == 0 || kills % KillstreakConfig.HealthShotKillCount != 0) return;

        if (!PlayerUtilities.TryFindPlayerBySteamId(steamId, out var player))
        {
            Logging.LogError("[UNEXPECTED]: Player got killstreak, but is not in the server");
            return;
        }

        player.GiveNamedItem(HealthShotEntityName);
    }
}