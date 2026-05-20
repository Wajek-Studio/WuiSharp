using System;
using Avalonia.Controls;
using Wajek.UI.Core.Interfaces;
using Wajek.UI.Core.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Wajek.UI.Core.Services;

public class Factory(IServiceProvider provider) : IFactory {

    private readonly IServiceProvider _provider = provider;

    public IServiceProvider GetProvider() {
        return _provider;
    }

    public T CreateWindow<T>() where T : Window {
        return (T)_provider.GetRequiredService(typeof(T));
    }

    public T CreateViewModel<T>() where T : ViewModelBase {
        return (T)_provider.GetRequiredService(typeof(T));
    }

    public T CreatePage<T>(object? parameter = null) where T : ViewModelBase {
        var page = (T)_provider.GetRequiredService(typeof(T));

        if (page is IInitializableAsync vmAsync) {
            _ = vmAsync.InitializeAsync(parameter);
        }

        if (page is IInitializable vm) {
            vm.Initialize(parameter);
        }

        return page;
    }

    public T CreateDbContext<T>() where T : DbContext {
        var factory = _provider.GetRequiredService<IDbContextFactory<T>>();
        return factory.CreateDbContext();
    }
}
