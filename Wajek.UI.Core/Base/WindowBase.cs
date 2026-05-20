using Avalonia.Controls;
using Wajek.UI.Core.Enums;

namespace Wajek.UI.Core.Base;

public class WindowBase : Window {

    public AppWindowCloseReason CloseReason { get; set; } = AppWindowCloseReason.Undefined;

    protected override void OnClosing(WindowClosingEventArgs e) {
        if (CloseReason == AppWindowCloseReason.Undefined) {
            CloseReason = e.CloseReason switch {
                WindowCloseReason.ApplicationShutdown => AppWindowCloseReason.ApplicationShutdown,
                WindowCloseReason.OSShutdown => AppWindowCloseReason.OSShutdown,
                WindowCloseReason.OwnerWindowClosing => AppWindowCloseReason.OwnerWindowClosing,
                WindowCloseReason.WindowClosing => AppWindowCloseReason.WindowClosing,
                _ => AppWindowCloseReason.Undefined
            };
        }

        base.OnClosing(e);
    }

}
