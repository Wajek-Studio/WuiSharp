namespace Wajek.UI.Demos.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Avalonia!";

    // Demo: CurrentView untuk WuiContent
    public object? CurrentView { get; set; } = null;
}
