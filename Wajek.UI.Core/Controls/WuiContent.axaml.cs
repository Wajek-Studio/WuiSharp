using Avalonia;
using Avalonia.Controls;

namespace Wajek.UI.Core.Controls;

public partial class WuiContent : UserControl
{
    public static readonly StyledProperty<object?> CurrentViewProperty =
        AvaloniaProperty.Register<WuiContent, object?>(nameof(CurrentView));

    public object? CurrentView
    {
        get => GetValue(CurrentViewProperty);
        set => SetValue(CurrentViewProperty, value);
    }

    public WuiContent()
    {
        InitializeComponent();
    }
}
