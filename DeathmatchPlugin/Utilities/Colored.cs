using System.Diagnostics.CodeAnalysis;
using DeathmatchPlugin.Config;

namespace DeathmatchPlugin.Utilities;

public static class Colored
{
    public static string Default(string inner)
    {
        return $"{TextColor.Default}{inner}{TextColor.Reset}";
    }

    public static string Red(string inner)
    {
        return $"{TextColor.Red}{inner}{TextColor.Reset}";
    }

    public static string LightPurple(string inner)
    {
        return $"{TextColor.LightPurple}{inner}{TextColor.Reset}";
    }

    public static string Green(string inner)
    {
        return $"{TextColor.Green}{inner}{TextColor.Reset}";
    }

    public static string Lime(string inner)
    {
        return $"{TextColor.Lime}{inner}{TextColor.Reset}";
    }

    public static string LightGreen(string inner)
    {
        return $"{TextColor.LightGreen}{inner}{TextColor.Reset}";
    }

    public static string LightRed(string inner)
    {
        return $"{TextColor.LightRed}{inner}{TextColor.Reset}";
    }

    public static string Gray(string inner)
    {
        return $"{TextColor.Gray}{inner}{TextColor.Reset}";
    }

    public static string LightOlive(string inner)
    {
        return $"{TextColor.LightOlive}{inner}{TextColor.Reset}";
    }

    public static string Olive(string inner)
    {
        return $"{TextColor.Olive}{inner}{TextColor.Reset}";
    }

    public static string LightBlue(string inner)
    {
        return $"{TextColor.LightBlue}{inner}{TextColor.Reset}";
    }

    public static string Blue(string inner)
    {
        return $"{TextColor.Blue}{inner}{TextColor.Reset}";
    }

    public static string Purple(string inner)
    {
        return $"{TextColor.Purple}{inner}{TextColor.Reset}";
    }

    public static string GrayBlue(string inner)
    {
        return $"{TextColor.GrayBlue}{inner}{TextColor.Reset}";
    }

    public static bool TryDispatch(string inner, string? color, [MaybeNullWhen(false)] out string coloredString)
    {
        coloredString = color?.ToLower() switch
        {
            "red" => Red(inner),
            "light-purple" => LightPurple(inner),
            "green" => Green(inner),
            "lime" => Lime(inner),
            "light-green" => LightGreen(inner),
            "light-red" => LightRed(inner),
            "gray" => Gray(inner),
            "light-olive" => LightOlive(inner),
            "olive" => Olive(inner),
            "light-blue" => LightBlue(inner),
            "blue" => Blue(inner),
            "purple" => Purple(inner),
            "gray-blue" => GrayBlue(inner),
            "default" => Default(inner),
            null => Default(inner),
            _ => null
        };

        return coloredString != null;
    }
}