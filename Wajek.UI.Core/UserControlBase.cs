using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Wajek.UI.Core;

public class UserControlBase : UserControl
{

    public UserControlBase()
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
}