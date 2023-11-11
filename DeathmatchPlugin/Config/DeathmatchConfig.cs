using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using DeathmatchPlugin.Utilities;

namespace DeathmatchPlugin.Config;

public static class DeathmatchConfig
{
    private const string ConfigFileName = "deathmatch.config.json";

    public static List<string> SpamMessages = new();
    public static bool Debug = false;
    public static string ChatPrefix = "";

    private class DeathmatchConfigurationFile
    {
        [JsonPropertyName("chatSpamMessages")] [JsonInclude]
        public List<string>? ChatSpamMessages = new()
        {
            "<lime>Type /guns in chat to get a printout (in console) of all weapon commands!</lime>"
        };

        [JsonPropertyName("chatPrefix")] [JsonInclude]
        public string ChatPrefix = "[<green>DM</green>]";

        [JsonPropertyName("debug")] [JsonInclude]
        public bool? EnableTracing = false;
    }

    public static void LoadConfig(string modulePath)
    {
        var configFileAbsolutePath = Path.Join(new DirectoryInfo(modulePath).Parent!.Parent!.FullName, ConfigFileName);
        Console.BackgroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"Loading DeathmatchPlugin configuration from {configFileAbsolutePath}");
        if (!Path.Exists(configFileAbsolutePath))
        {
            Console.WriteLine("No config file present, creating default configuration");
            WriteDefaultConfig(configFileAbsolutePath);
        }

        var configurationFileContents = File.ReadAllText(configFileAbsolutePath, Encoding.UTF8);
        var configurationFile = JsonSerializer.Deserialize<DeathmatchConfigurationFile>(configurationFileContents);
        SpamMessages = (configurationFile?.ChatSpamMessages ?? new List<string>()).Select(ParseColoredMessage).ToList();
        Debug = configurationFile?.EnableTracing ?? false;
        ChatPrefix = ParseColoredMessage(configurationFile?.ChatPrefix ?? "[BADCFG]");

        Console.WriteLine("Successfully loaded config.");
        Console.WriteLine("\tSpamMessages=");
        if (SpamMessages.Count == 0)
            Console.WriteLine("\t\tNone configured.");
        else
            foreach (var customSpamMessage in SpamMessages)
                Console.WriteLine($"\t\t\"{customSpamMessage}\"");
        Console.WriteLine($"\tDebug={Debug}");
        Console.WriteLine($"\tChatPrefix={ChatPrefix}");

        Console.ResetColor();
    }

    private static string ParseColoredMessage(string message)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(message);
        writer.Flush();
        stream.Position = 0;
        using var reader =
            XmlReader.Create(stream,
                new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment, IgnoreWhitespace = false });

        var output = "";
        var colorStack = new Stack<string>();
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    colorStack.Push(reader.Name);
                    break;
                case XmlNodeType.Text:
                    colorStack.TryPeek(out var top);
                    Console.WriteLine($"\"{reader.Value}\"");
                    if (Colored.TryDispatch(reader.Value, top, out var coloredString))
                        output += coloredString;
                    else
                        output += reader.Value;
                    break;
                case XmlNodeType.EndElement:
                    colorStack.Pop();
                    break;
                case XmlNodeType.Whitespace:
                    output += ' ';
                    break;
                default:
                    Console.WriteLine(reader.NodeType);
                    Console.WriteLine(reader.Name);
                    Console.WriteLine(reader.Value);
                    Console.WriteLine(reader.ValueType);
                    break;
            }
        }

        return output;
    }

    private static void WriteDefaultConfig(string configFilePath)
    {
        var model = new DeathmatchConfigurationFile();
        var configFileContents = JsonSerializer.Serialize(model, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        Console.WriteLine($"Dumping default Json: \'\'\'{configFileContents}\'\'\'");
        File.WriteAllText(configFilePath, configFileContents, Encoding.UTF8);
    }
}