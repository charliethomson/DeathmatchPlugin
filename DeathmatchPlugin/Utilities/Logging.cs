using DeathmatchPlugin.Config;

namespace DeathmatchPlugin.Utilities;

public static class Logging
{
    public static void Log(string message)
    {
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"{DeathmatchConfig.ChatPrefix}: [INFO]: {message}");
        Console.ResetColor();
    }

    public static void LogTrace(string message)
    {
        if (!DeathmatchConfig.Debug) return;
        Console.BackgroundColor = ConsoleColor.DarkGray;
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine($"{DeathmatchConfig.ChatPrefix}: [TRACE]: {message}");
        Console.ResetColor();
    }

    public static void LogWarn(string message)
    {
        Console.BackgroundColor = ConsoleColor.Yellow;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine($"{DeathmatchConfig.ChatPrefix}: [WARN]: {message}");
        Console.ResetColor();
    }

    public static void LogError(string message)
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine($"{DeathmatchConfig.ChatPrefix}: [ERROR]: {message}");
        Console.ResetColor();
    }
}