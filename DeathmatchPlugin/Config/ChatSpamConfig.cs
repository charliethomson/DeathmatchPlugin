using DeathmatchPlugin.Utilities;

namespace DeathmatchPlugin.Config;

public static class ChatSpamConfig
{
    public static readonly List<string> SpamMessages = new()
    {
        Colored.Lime("Type /guns in chat to get a printout (in console) of all weapon commands!"),
        $"{Colored.Lime("Join Mercury Gaming!")} {Colored.Blue("discord.gg/mercurygaming")}"
    };

    public static string GetMessage(int spamIndex)
    {
        return SpamMessages[spamIndex];
    }
}