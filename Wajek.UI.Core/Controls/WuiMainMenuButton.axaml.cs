using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using System.Collections.Specialized;
using System.Windows.Input;

namespace Wajek.UI.Core.Controls;

public partial class WuiMainMenuButton : UserControl
{
    public static readonly StyledProperty<string?> IconProperty =
        AvaloniaProperty.Register<WuiMainMenuButton, string?>(nameof(Icon),
            defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<WuiMainMenuButton, string?>(nameof(Text),
            defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<WuiMainMenuButton, ICommand?>(nameof(Command),
            defaultBindingMode: BindingMode.OneWay);

    public static readonly StyledProperty<object?> CommandParameterProperty =
        AvaloniaProperty.Register<WuiMainMenuButton, object?>(nameof(CommandParameter),
            defaultBindingMode: BindingMode.OneWay);

    public string? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public WuiMainMenuButton()
    {
        InitializeComponent();
        Classes.CollectionChanged += OnClassesChanged;
        SyncButtonClasses();
    }

    private void OnClassesChanged(object? sender, NotifyCollectionChangedEventArgs e) =>
        SyncButtonClasses();

    private void SyncButtonClasses()
    {
        if (PART_Button is null)
            return;

        SetClass(PART_Button, "Active", Classes.Contains("Active"));
    }

    private static void SetClass(Button button, string className, bool enabled)
    {
        if (enabled)
            button.Classes.Add(className);
        else
            button.Classes.Remove(className);
    }
}
