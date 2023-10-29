using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using DeathmatchPlugin.Extensions;

namespace DeathmatchPlugin.Utilities;

public class CommandUtilities
{
    public static bool ClientCommand(CCSPlayerController? player, bool mustBeOnATeam = true)
    {
        if (player == null)
        {
            Server.PrintToConsole("Command not executed - Client only command");
            return true;
        }

        if (mustBeOnATeam && !player.IsOnATeam())
        {
            player.PrintToConsole("Command not executed - Player must be on a team");
            return true;
        }

        return false;
    }
}