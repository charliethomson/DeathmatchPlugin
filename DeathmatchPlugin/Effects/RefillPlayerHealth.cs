using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;
using DeathmatchPlugin.Config;
using DeathmatchPlugin.Utilities;
namespace DeathmatchPlugin.Effects;

public static class RefillPlayerHealth {

    public static void onKill(CCSPlayerController player)
    {
        if (player.Health == 100) return;
        string giveBack = "weapon_healthshot";
        string playerName = player.PlayerName;
        player.PlayerPawn.Value.Health = 100; // give health
        player.PlayerPawn.Value.MaxHealth = 100;
        player.PlayerPawn.Value.ArmorValue = 100; // give armor
        //  PrintChat giveHealth
        player.PrintToChat($"{DeathmatchConfig.ChatPrefix} {Colored.Blue(playerName)} : {Colored.Green("+100")} hp");
        /*
            Due to the nature of the HUD, the HUD is not updated until an action is taken by an external factor, 
            so the HUD is updated using healthShot.
        */
        foreach (var weapon in player.PlayerPawn.Value.WeaponServices!.MyWeapons)
        {
            if (weapon is { IsValid:true, Value.IsValid: true } && weapon.Value.DesignerName.Contains("healthshot")) {
                giveBack = weapon.Value.DesignerName;
                weapon.Value.Remove();
            }
        }
        // give HealthShot
        VirtualFunctions.GiveNamedItem(player.PlayerPawn.Value.ItemServices!.Handle, giveBack, 0, 0, 0, 0);
    }
}
