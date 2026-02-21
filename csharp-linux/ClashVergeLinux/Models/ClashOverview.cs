namespace ClashVergeLinux.Models;

public sealed class ClashOverview
{
    public string Version { get; set; } = "unknown";
    public string ExternalController { get; set; } = "127.0.0.1:9090";
    public string CurrentMode { get; set; } = "rule";
    public long UploadTotal { get; set; }
    public long DownloadTotal { get; set; }
}
