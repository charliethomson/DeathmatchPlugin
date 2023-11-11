using CounterStrikeSharp.API;
using DeathmatchPlugin.Config;
using DeathmatchPlugin.Utilities;

namespace DeathmatchPlugin.Effects;

public static class ChatSpam
{
    private static int _spamIndex;

    private static readonly int MaxSpamIndex = DeathmatchConfig.SpamMessages.Count - 1;

    public static void Do()
    {
        if (_spamIndex == MaxSpamIndex) _spamIndex = 0;
        else _spamIndex++;

        var message = DeathmatchConfig.SpamMessages[_spamIndex];
        Server.PrintToChatAll($"{DeathmatchConfig.ChatPrefix} {message}");
    }
}