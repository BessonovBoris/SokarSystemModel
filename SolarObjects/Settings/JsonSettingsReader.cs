using System.Text;
using System.Text.Json;

namespace SolarObjects.Settings;

public static class JsonSettingsReader
{
    public static ISettings LoadSettings(string path)
    {
        using var stream = new FileStream(path, FileMode.OpenOrCreate);
        byte[] streamByte = new byte[stream.Length];
        stream.Read(streamByte);
        string json = Encoding.Default.GetString(streamByte);

        Settings? settings = JsonSerializer.Deserialize<Settings>(json);

        if (settings is null)
        {
            throw new ArgumentException("Can't read this settings");
        }

        return settings;
    }

    public static void SerializeSettings(string path, ISettings settings)
    {
        using var stream = new FileStream(path, FileMode.Create);
        string json = JsonSerializer.Serialize(settings);
        Console.WriteLine(json);

        byte[] jsonByte = Encoding.Default.GetBytes(json);

        stream.Write(jsonByte);
    }
}