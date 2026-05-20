using Microsoft.Extensions.DependencyInjection;

namespace Wajek.UI.Core.Interfaces;

public interface ILoader {
    public static abstract void Load(IServiceCollection collection);
}
