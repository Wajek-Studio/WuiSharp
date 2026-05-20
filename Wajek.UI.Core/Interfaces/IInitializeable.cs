using System.Threading.Tasks;

namespace Wajek.UI.Core.Interfaces;

public interface IInitializableAsync {
    Task InitializeAsync(object? parameter);
}

public interface IInitializable {
    void Initialize(object? parameter);
}
