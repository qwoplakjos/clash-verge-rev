using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ClashVergeLinux.Services;
using ClashVergeLinux.ViewModels;
using ClashVergeLinux.Views;

namespace ClashVergeLinux;

public partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var configService = new ConfigService();
            var mihomoClient = new MihomoApiClient(configService);
            var viewModel = new MainWindowViewModel(configService, mihomoClient);
            await viewModel.InitializeAsync();

            desktop.MainWindow = new MainWindow
            {
                DataContext = viewModel,
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
