using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Wajek.UI.Core.Utilities;

namespace Wajek.UI.Core;

public class Factory(IServiceProvider provider)
{

    private readonly IServiceProvider _provider = provider;

    private readonly List<WindowBase> _windows = [];

    public WindowBase GetWindow(object vm)
    {
        return _windows.FirstOrDefault(w => object.ReferenceEquals(w.DataContext, vm))
            ?? throw new InvalidOperationException($"Unable to find window for view model of type {vm.GetType().FullName}");
    }

    public WindowBase FindWindow<TViewModel>(TViewModel vm) where TViewModel : notnull
    {
        var vmClassName = vm.GetType().FullName;
        if (vmClassName == null) throw new Exception("Unable to get view model class name");

        var windowClassName = vmClassName.ReplaceLastOccurrence("WindowViewModel", "Window");
        if (windowClassName == vmClassName) windowClassName = vmClassName.ReplaceLastOccurrence("ViewModel", "View");
        if (windowClassName == vmClassName) throw new Exception("Unable to auto detect window class name");

        var windowType = vm.GetType().Assembly.GetType(windowClassName);
        if (windowType == null) throw new Exception($"Unable to get window type {windowClassName}");

        var window = (WindowBase)_provider.GetRequiredService(windowType);
        if (window == null) throw new Exception("Unable to create window");
        return window;
    }

    private void AddWindow<TViewModel>(WindowBase window, TViewModel viewModel)
    {
        window.DataContext = viewModel;
        window.Closed += (_, _) =>
        {
            _windows.Remove(window);
        };
        _windows.Add(window);
    }

    public WindowBase CreateWindow<TViewModel>()
        where TViewModel : notnull
    {
        var vm = (TViewModel)_provider.GetRequiredService(typeof(TViewModel));
        if (vm == null) throw new Exception("Unable to create vm instance");

        var window = FindWindow(vm);
        AddWindow(window, vm);
        return window;
    }

    public WindowBase CreateWindow<TViewModel, TParam>(TParam param)
        where TViewModel : notnull
        where TParam : notnull
    {
        var vm = ActivatorUtilities.CreateInstance<TViewModel>(_provider, param);
        if (vm == null) throw new Exception("Unable to create vm instance");

        var window = FindWindow(vm);
        AddWindow(window, vm);
        return window;
    }

    public TViewModel CreateView<TViewModel>() where TViewModel : notnull
    {
        var vm = (TViewModel)_provider.GetRequiredService(typeof(TViewModel));
        if (vm == null) throw new Exception("Unable to create vm instance");
        return vm;
    }

    public TViewModel CreateView<TViewModel, TParam>(TParam param)
        where TViewModel : notnull
        where TParam : notnull
    {
        var vm = ActivatorUtilities.CreateInstance<TViewModel>(_provider, param);
        if (vm == null) throw new Exception("Unable to create vm instance");
        return vm;
    }

}