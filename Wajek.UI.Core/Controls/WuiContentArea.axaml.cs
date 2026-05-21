using Avalonia;
using Avalonia.Controls;

namespace Wajek.UI.Core.Controls;

public partial class WuiContentArea : UserControl
{
    public static readonly StyledProperty<object?> CurrentViewProperty =
        AvaloniaProperty.Register<WuiContentArea, object?>(nameof(CurrentView));

    public object? CurrentView
    {
        get => GetValue(CurrentViewProperty);
        set => SetValue(CurrentViewProperty, value);
    }

    public WuiContentArea()
    {
        InitializeComponent();
    }
}
