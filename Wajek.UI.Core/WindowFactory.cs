using Wajek.UI.Core.Base;
using Wajek.UI.Core.Interfaces;
using Wajek.UI.Core.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Wajek.UI.Core;

public class WindowFactory(BaseFactory factory)
{

    private readonly List<WindowBase> _windows = [];

    private readonly BaseFactory _factory = factory;

    private void Add(WindowBase window, ViewModelBase viewModel, object? param = null)
    {
        window.DataContext = viewModel;
        window.Closed += (_, _) =>
        {
            _windows.Remove(window);
        };

        if (window.DataContext is IInitializableAsync vmInitializableAsync)
        {
            window.Loaded += async (s, e) =>
            {
                await vmInitializableAsync.InitializeAsync(param);
            };
        }

        if (viewModel is IInitializable vmInitializable)
        {
            window.Loaded += (s, e) =>
            {
                vmInitializable.Initialize(param);
            };
        }
        _windows.Add(window);
    }

    public WindowBase Create<TViewModel>(object? param = null) where TViewModel : ViewModelBase
    {
        var viewModel = _factory.CreateViewModel<TViewModel>();
        var vmFullName = viewModel.GetType().FullName;
        if (vmFullName == null) throw new Exception("Gagal mengambil type viewmodel");

        var windowFullName = vmFullName.ReplaceLastOccurrence("WindowViewModel", "Window");
        var windowType = Type.GetType(windowFullName);
        if (windowType == null) throw new Exception("Gagal mengambil type window");

        var window = (WindowBase)_factory.GetProvider().GetRequiredService(windowType);

        Add(window, viewModel, param);
        return window;
    }

    public WindowBase Create<TWindow, TViewModel>(object? param = null)
        where TWindow : WindowBase
        where TViewModel : ViewModelBase
    {
        var window = _factory.CreateWindow<TWindow>();
        var viewModel = _factory.CreateViewModel<TViewModel>();

        Add(window, viewModel, param);
        return window;
    }

    public WindowBase Get(WindowBase window)
    {
        var result = _windows.FirstOrDefault(w => w.GetType() == window.GetType()) ?? throw new InvalidOperationException($"Window dengan type {window.GetType()} tidak ditemukan");
        return result;
    }

    public WindowBase Get(ViewModelBase viewModel)
    {
        var result = _windows.FirstOrDefault(w => w.DataContext?.GetType() == viewModel.GetType()) ?? throw new InvalidOperationException($"Window dengan view model {viewModel.GetType()} tidak ditemukan");
        return result;
    }

}
