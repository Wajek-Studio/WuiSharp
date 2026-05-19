using Avalonia;
using Avalonia.Controls;

namespace Wajek.UI.Core.Controls;

public partial class WuiMainMenuTabContent : UserControl
{
    public static readonly StyledProperty<object?> InnerContentProperty =
        AvaloniaProperty.Register<WuiMainMenuTabContent, object?>(nameof(InnerContent));

    public object? InnerContent
    {
        get => GetValue(InnerContentProperty);
        set => SetValue(InnerContentProperty, value);
    }

    public WuiMainMenuTabContent()
    {
        InitializeComponent();
    }
}
