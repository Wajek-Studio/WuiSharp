using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Wajek.UI.Core.Utilities;

namespace Wajek.UI.Core;

public class Factory(IServiceProvider provider) {

    private readonly IServiceProvider _provider = provider;

    private readonly List<WindowBase> _windows = [];

    public WindowBase GetWindow(object vm) {
        return _windows.FirstOrDefault(w => object.ReferenceEquals(w.DataContext, vm))
            ?? throw new InvalidOperationException($"Unable to find window for view model of type {vm.GetType().FullName}");
    }
    
    public T Create<T>() {
        return (T)_provider.GetRequiredService(typeof(T));
    }

    public WindowBase FindWindow<TViewModel>(TViewModel vm) where TViewModel : notnull {
        var vmClassName = vm.GetType().FullName;
        if(vmClassName == null) throw new Exception("Unable to get view model class name");

        var windowClassName = vmClassName.ReplaceLastOccurrence("WindowViewModel", "Window");
        if(windowClassName == vmClassName) windowClassName = vmClassName.ReplaceLastOccurrence("ViewModel", "View");
        if(windowClassName == vmClassName) throw new Exception("Unable to auto detect window class name");

        var windowType = vm.GetType().Assembly.GetType(windowClassName);
        if(windowType == null) throw new Exception($"Unable to get window type {windowClassName}");

        var window = (WindowBase)_provider.GetRequiredService(windowType);
        if(window == null) throw new Exception("Unable to create window");
        return window;
    }

    private void AddWindow<TViewModel>(WindowBase window, TViewModel viewModel) {
        window.DataContext = viewModel;

        if (window.DataContext is IInitializableAsync vmInitializableAsync) {
            window.Loaded += async (s, e) => {
                await vmInitializableAsync.InitializeAsync();
            };
        }

        if (viewModel is IInitializable vmInitializable) {
            window.Loaded += (s, e) => {
                vmInitializable.Initialize();
            };
        }
        
        window.Closed += (_, _) => {
            _windows.Remove(window);
        };

        _windows.Add(window);
    }

    private void AddWindow<TViewModel, TParam>(WindowBase window, TViewModel viewModel, TParam param) {
        if (window.DataContext is IInitializableWithParamAsync<TParam> vmInitializableAsync) {
            window.Loaded += async (s, e) => {
                await vmInitializableAsync.InitializeAsync(param);
            };
        }

        if (viewModel is IInitializableWithParam<TParam> vmInitializable) {
            window.Loaded += (s, e) => {
                vmInitializable.Initialize(param);
            };
        }

        AddWindow(window, viewModel);
    }

    public WindowBase CreateWindow<TViewModel, TParam>(TParam param) where TViewModel : notnull
    {
        var vm = Create<TViewModel>();
        if(vm == null) throw new Exception("Unable to create vm instance");

        var window = FindWindow(vm);
        AddWindow(window, vm, param);
        return window;
    }

    public WindowBase CreateWindow<TViewModel>() where TViewModel : notnull
    {
        var vm = Create<TViewModel>();
        if(vm == null) throw new Exception("Unable to create vm instance");

        var window = FindWindow(vm);
        AddWindow(window, vm);
        return window;
    }

    public TViewModel CreateView<TViewModel>() where TViewModel : notnull
    {
        var vm = Create<TViewModel>();
        if(vm == null) throw new Exception("Unable to create vm instance");

        if(vm is IInitializableAsync vmInitAsync)
        {
            _ = vmInitAsync.InitializeAsync();
        }

        if(vm is IInitializable vmInit)
        {
            vmInit.Initialize();
        }

        return vm;
    }

    public TViewModel CreateView<TViewModel, TParam>(TParam param) where TViewModel : notnull
    {
        var vm = Create<TViewModel>();
        if(vm == null) throw new Exception("Unable to create vm instance");

        if(vm is IInitializableWithParamAsync<TParam> vmInitParamAsync)
        {
            _ = vmInitParamAsync.InitializeAsync(param);
        }

        if(vm is IInitializableWithParam<TParam> vmInitParam)
        {
            vmInitParam.Initialize(param);
        }

        return vm;
    }

}