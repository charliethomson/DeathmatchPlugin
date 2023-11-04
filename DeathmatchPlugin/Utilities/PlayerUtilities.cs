using CounterStrikeSharp.API.Core;

namespace DeathmatchPlugin.Utilities;

public static class PlayerUtilities
{
    public static IEnumerable<CCSPlayerController> GetAllPlayers()
    {
        return CounterStrikeSharp.API.Utilities.FindAllEntitiesByDesignerName<CCSPlayerController>(
            "cs_player_controller");
    }

    public static IEnumerable<CCSPlayerController> FindPlayers(Func<CCSPlayerController, bool> predicate)
    {
        return GetAllPlayers().Where(predicate);
    }

    public static bool TryFindPlayerByName(string playerName, Action<string> feedback, out CCSPlayerController player)
    {
        player = null!;
        var matchingPlayers =
            FindPlayers(p => p.PlayerName.ToLower().Contains(playerName.ToLower())).ToList();


        switch (matchingPlayers.Count)
        {
            case > 1:
                feedback($"[MG] Found multiple players matching \"{playerName}\", please try again");
                return false;
            case 0:
                feedback($"[MG] No players matching \"{playerName}\" found, please try again");
                return false;
            default:
                player = matchingPlayers[0];
                return true;
        }
    }

    public static bool TryFindPlayerBySteamId(ulong steamId, out CCSPlayerController player)
    {
        player = null!;
        var foundPlayer = GetAllPlayers().FirstOrDefault(player => player.SteamID == steamId);

        if (foundPlayer == null) return false;

        player = foundPlayer;
        return true;
    }
}