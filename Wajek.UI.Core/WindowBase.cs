using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Wajek.UI.Core;

public enum WindowCloseReason
{

    ApplicationShutdown,
    OSShutdown,
    OwnerWindowClosing,
    WindowClosing,
    ForceClose,
    Undefined

}

public class WindowBase : Window
{

    public WindowBase()
    {
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (DataContext is IInitializableAsync initAsync)
        {
            await initAsync.InitializeAsync();
        }

        if (DataContext is IInitializable init)
        {
            init.Initialize();
        }
    }

    public WindowCloseReason CloseReason { get; set; } = WindowCloseReason.Undefined;

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        if (CloseReason == WindowCloseReason.Undefined)
        {
            CloseReason = e.CloseReason switch
            {
                Avalonia.Controls.WindowCloseReason.ApplicationShutdown => WindowCloseReason.ApplicationShutdown,
                Avalonia.Controls.WindowCloseReason.OSShutdown => WindowCloseReason.OSShutdown,
                Avalonia.Controls.WindowCloseReason.OwnerWindowClosing => WindowCloseReason.OwnerWindowClosing,
                Avalonia.Controls.WindowCloseReason.WindowClosing => WindowCloseReason.WindowClosing,
                _ => WindowCloseReason.Undefined
            };
        }

        base.OnClosing(e);
    }

}
