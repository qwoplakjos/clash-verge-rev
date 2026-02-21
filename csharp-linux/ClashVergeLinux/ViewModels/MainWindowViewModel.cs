using ClashVergeLinux.Models;
using ClashVergeLinux.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ClashVergeLinux.ViewModels;

public sealed class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly ConfigService _configService;
    private readonly MihomoApiClient _mihomoApiClient;

    private VergeConfig _config = new();
    private ClashOverview _overview = new();
    private string _status = "Initializing";

    public MainWindowViewModel(ConfigService configService, MihomoApiClient mihomoApiClient)
    {
        _configService = configService;
        _mihomoApiClient = mihomoApiClient;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public VergeConfig Config
    {
        get => _config;
        private set => SetField(ref _config, value);
    }

    public ClashOverview Overview
    {
        get => _overview;
        private set => SetField(ref _overview, value);
    }

    public string Status
    {
        get => _status;
        private set => SetField(ref _status, value);
    }

    public ObservableCollection<string> ProxyGroups { get; } = [];

    public async Task InitializeAsync()
    {
        Config = await _configService.LoadVergeConfigAsync();
        await RefreshAsync();
    }

    public async Task RefreshAsync()
    {
        Status = "Syncing with mihomo...";
        Overview = await _mihomoApiClient.GetOverviewAsync();

        ProxyGroups.Clear();
        foreach (var item in await _mihomoApiClient.GetProxyGroupNamesAsync())
        {
            ProxyGroups.Add(item);
        }

        Status = "Ready";
    }

    public async Task SaveConfigAsync()
    {
        await _configService.SaveVergeConfigAsync(Config);
        Status = "Config saved";
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
