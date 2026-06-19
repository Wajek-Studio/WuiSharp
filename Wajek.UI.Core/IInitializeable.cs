namespace Wajek.UI.Core;

public interface IInitializableAsync {
    Task InitializeAsync();
}

public interface IInitializable {
    void Initialize();
}

public interface IInitializableWithParamAsync<TParam> {
    Task InitializeAsync(TParam param);
}

public interface IInitializableWithParam<TParam> {
    void Initialize(TParam param);
}
