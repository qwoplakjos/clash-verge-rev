using ClashVergeLinux.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClashVergeLinux.Services;

public sealed class MihomoApiClient
{
    private readonly HttpClient _httpClient = new();
    private readonly ConfigService _configService;

    public MihomoApiClient(ConfigService configService)
    {
        _configService = configService;
    }

    private async Task<string> BuildBaseUrlAsync()
    {
        var cfg = await _configService.LoadVergeConfigAsync();
        return $"http://127.0.0.1:{cfg.VergeMixedPort}";
    }

    public async Task<ClashOverview> GetOverviewAsync()
    {
        var baseUrl = await BuildBaseUrlAsync();

        // Mihomo API path assumptions for a Linux-only standalone rewrite.
        var versionTask = _httpClient.GetStringAsync($"{baseUrl}/version");
        var trafficTask = _httpClient.GetStringAsync($"{baseUrl}/traffic");

        await Task.WhenAll(versionTask, trafficTask);

        var overview = new ClashOverview();

        try
        {
            var versionJson = JsonDocument.Parse(versionTask.Result);
            overview.Version = versionJson.RootElement.GetProperty("version").GetString() ?? "unknown";
            overview.ExternalController = baseUrl;
        }
        catch
        {
            overview.Version = "offline";
        }

        try
        {
            var traffic = JsonDocument.Parse(trafficTask.Result).RootElement;
            overview.UploadTotal = traffic.GetProperty("up").GetInt64();
            overview.DownloadTotal = traffic.GetProperty("down").GetInt64();
        }
        catch
        {
            overview.UploadTotal = 0;
            overview.DownloadTotal = 0;
        }

        return overview;
    }

    public async Task<string[]> GetProxyGroupNamesAsync()
    {
        var baseUrl = await BuildBaseUrlAsync();
        try
        {
            var json = await _httpClient.GetStringAsync($"{baseUrl}/proxies");
            using var doc = JsonDocument.Parse(json);
            var proxies = doc.RootElement.GetProperty("proxies");
            var names = new List<string>();
            foreach (var item in proxies.EnumerateObject())
            {
                if (item.Value.TryGetProperty("all", out _))
                {
                    names.Add(item.Name);
                }
            }
            return names.ToArray();
        }
        catch
        {
            return ["GLOBAL", "DIRECT", "REJECT"];
        }
    }
}
