using ClashVergeLinux.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ClashVergeLinux.Services;

public sealed class ConfigService
{
    private readonly IDeserializer _deserializer = new DeserializerBuilder()
        .WithNamingConvention(UnderscoredNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .Build();

    private readonly ISerializer _serializer = new SerializerBuilder()
        .WithNamingConvention(UnderscoredNamingConvention.Instance)
        .Build();

    public string AppHome { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "clash-verge-csharp");

    public string VergeConfigPath => Path.Combine(AppHome, "verge.yaml");

    public async Task<VergeConfig> LoadVergeConfigAsync()
    {
        Directory.CreateDirectory(AppHome);

        if (!File.Exists(VergeConfigPath))
        {
            var defaults = new VergeConfig();
            await SaveVergeConfigAsync(defaults);
            return defaults;
        }

        var raw = await File.ReadAllTextAsync(VergeConfigPath);
        return _deserializer.Deserialize<VergeConfig>(raw) ?? new VergeConfig();
    }

    public async Task SaveVergeConfigAsync(VergeConfig config)
    {
        Directory.CreateDirectory(AppHome);
        var yaml = _serializer.Serialize(config);
        await File.WriteAllTextAsync(VergeConfigPath, yaml);
    }
}
