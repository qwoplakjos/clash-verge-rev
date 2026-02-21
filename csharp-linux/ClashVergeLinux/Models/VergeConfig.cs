namespace ClashVergeLinux.Models;

public sealed class VergeConfig
{
    public string ThemeMode { get; set; } = "system";
    public bool EnableSystemProxy { get; set; }
    public bool EnableTunMode { get; set; }
    public bool EnableAutoLaunch { get; set; }
    public int VergeMixedPort { get; set; } = 7897;
    public string Language { get; set; } = "en";
    public string StartPage { get; set; } = "/";
    public string[] MenuOrder { get; set; } =
    [
        "/", "/proxies", "/profile", "/connections", "/rules", "/logs", "/unlock", "/settings"
    ];
}
