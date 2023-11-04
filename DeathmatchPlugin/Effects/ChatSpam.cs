using CounterStrikeSharp.API;
using DeathmatchPlugin.Config;

namespace DeathmatchPlugin.Effects;

public static class ChatSpam
{
    private static int _spamIndex;
    private static readonly int MaxSpamIndex = ChatSpamConfig.SpamMessages.Count - 1;

    public static void Do()
    {
        if (_spamIndex == MaxSpamIndex) _spamIndex = 0;
        else _spamIndex++;

        var message = ChatSpamConfig.GetMessage(_spamIndex);
        Server.PrintToChatAll($"{ChatConfig.ChatPrefix} {message}");
    }
}