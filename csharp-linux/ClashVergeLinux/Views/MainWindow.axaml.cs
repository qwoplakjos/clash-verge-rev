using Avalonia.Controls;
using ClashVergeLinux.ViewModels;

namespace ClashVergeLinux.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void OnRefreshClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            await vm.RefreshAsync();
        }
    }

    private async void OnSaveConfigClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            await vm.SaveConfigAsync();
        }
    }
}
