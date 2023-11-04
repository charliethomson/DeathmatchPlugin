using System.Drawing;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;
using DeathmatchPlugin.Utilities;

namespace DeathmatchPlugin.Extensions
{
    public static class PlayerExtensions
    {
        public static void SetColor(this CCSPlayerController player, Color color)
        {
            // TODO: Fix bug with "set color" -> "doesnt update til you move, shoot, right click, middle click, switch weps, rlly do anyhting other thant look around"
            if (!player.PlayerPawn.IsValid)
            {
                Logging.LogWarn("SetColor:ERR - Attempted to set invalid player's color!");
                return;
            }

            Logging.LogTrace($"SetColor - Setting {player.PlayerName} to {color.ToString()}");
            var col = 255;
            col <<= 8;
            col |= color.B;
            col <<= 8;
            col |= color.G;
            col <<= 8;
            col |= color.R;
            Schema.SetSchemaValue(player.PlayerPawn.Value.Handle, nameof(CBaseModelEntity), "m_clrRender", col);
        }


        public static bool IsAlive(this CCSPlayerController? player)
        {
            if (player == null || !player.IsValid || !player.PlayerPawn.IsValid)
            {
                Logging.LogWarn("IsAlive:ER - InvalidPlayer");
                return false;
            }

            var isAlive = player.PlayerPawn.Value.LifeState == 0;
            Logging.LogTrace($"IsAlive - {player.PlayerName} isAlive? {isAlive}");
            return isAlive;
        }

        public static bool IsDying(this CCSPlayerController? player)
        {
            if (player == null || !player.IsValid || !player.PlayerPawn.IsValid)
            {
                Logging.LogWarn("IsDying:ER - InvalidPlayer");
                return false;
            }

            var isDying = player.PlayerPawn.Value.LifeState == 1;
            Logging.LogTrace($"IsDying - {player.PlayerName} isDying? {isDying}");
            return isDying;
        }

        public static bool IsDead(this CCSPlayerController? player)
        {
            if (player == null || !player.IsValid || !player.PlayerPawn.IsValid)
            {
                Logging.LogWarn("IsDead:ER - InvalidPlayer");
                return false;
            }

            var isDead = player.PlayerPawn.Value.LifeState == 2;
            Logging.LogTrace($"IsDead - {player.PlayerName} isDead? {isDead}");
            return isDead;
        }


        public static bool IsTerrorist(this CCSPlayerController? player)
        {
            if (player == null || !player.IsValid || !player.PlayerPawn.IsValid)
            {
                Logging.LogWarn("IsTerrorist:ER - InvalidPlayer");
                return false;
            }

            var isTerrorist = player.PlayerPawn.Value.TeamNum == 2;
            Logging.LogTrace($"IsTerrorist - {player.PlayerName} isTerrorist? {isTerrorist}");
            return isTerrorist;
        }

        public static bool IsCounterTerrorist(this CCSPlayerController? player)
        {
            if (player == null || !player.IsValid || !player.PlayerPawn.IsValid)
            {
                Logging.LogWarn("IsCounterTerrorist:ER - InvalidPlayer");
                return false;
            }

            var isCounterTerrorist = player.PlayerPawn.Value.TeamNum == 3;
            Logging.LogTrace($"IsCounterTerrorist - {player.PlayerName} isCounterTerrorist? {isCounterTerrorist}");
            return isCounterTerrorist;
        }

        public static bool IsOnATeam(this CCSPlayerController player)
        {
            return player.IsTerrorist() || player.IsCounterTerrorist();
        }

        public static CBasePlayerWeapon? ActiveWeapon(this CCSPlayerController player)
        {
            if (!player.IsOnATeam()) return null;
            var weaponServices = player.PlayerPawn.Value.WeaponServices;
            return weaponServices?.ActiveWeapon.GetValueOrNull();
        }

        public static string TeamColoredPlayerName(this CCSPlayerController player)
        {
            
            return player.IsTerrorist()
                ? Colored.LightOlive(player.PlayerName)
                : Colored.LightBlue(player.PlayerName);

        }
    }
}