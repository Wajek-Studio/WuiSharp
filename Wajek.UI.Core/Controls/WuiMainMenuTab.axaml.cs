using Avalonia;
using Avalonia.Controls;

namespace Wajek.UI.Core.Controls;

public partial class WuiMainMenuTab : UserControl
{
    public static readonly StyledProperty<object?> LeftContentProperty =
        AvaloniaProperty.Register<WuiMainMenuTab, object?>(nameof(LeftContent));

    public static readonly StyledProperty<object?> RightContentProperty =
        AvaloniaProperty.Register<WuiMainMenuTab, object?>(nameof(RightContent));

    public object? LeftContent
    {
        get => GetValue(LeftContentProperty);
        set => SetValue(LeftContentProperty, value);
    }

    public object? RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    public WuiMainMenuTab()
    {
        InitializeComponent();
    }
}
