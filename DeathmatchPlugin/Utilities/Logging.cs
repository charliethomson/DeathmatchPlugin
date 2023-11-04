using DeathmatchPlugin.Config;

namespace DeathmatchPlugin.Utilities;

public static class Logging
{
    public static bool Debug = false;

    public static void LogTrace(string message)
    {
        if (!Debug) return;
        Console.BackgroundColor = ConsoleColor.DarkGray;
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine($"{ChatConfig.ChatPrefix}: [TRACE]: {message}");
        Console.ResetColor();
    }

    public static void LogWarn(string message)
    {
        Console.BackgroundColor = ConsoleColor.Yellow;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine($"{ChatConfig.ChatPrefix}: [WARN]: {message}");
        Console.ResetColor();
    }

    public static void LogError(string message)
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine($"{ChatConfig.ChatPrefix}: [ERROR]: {message}");
        Console.ResetColor();
    }
}