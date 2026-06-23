using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;

namespace Wajek.UI.Core.Controls;

public partial class Header : ContentControl, INotifyPropertyChanged
{
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<Header, string>(nameof(Title));

    public static readonly StyledProperty<string> DescriptionProperty =
        AvaloniaProperty.Register<Header, string>(nameof(Description),
        defaultValue: string.Empty);

    public static readonly StyledProperty<string> IconProperty =
        AvaloniaProperty.Register<Header, string>(nameof(Icon));

    public static readonly StyledProperty<bool> HasDescriptionProperty =
        AvaloniaProperty.Register<Header, bool>(nameof(HasDescription));

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public string Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public bool HasDescription
    {
        get => GetValue(HasDescriptionProperty);
        private set => SetValue(HasDescriptionProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == DescriptionProperty)
        {
            HasDescription = !string.IsNullOrWhiteSpace(change.NewValue as string);
        }
    }
}