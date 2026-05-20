using System.Collections.Generic;

namespace Wajek.UI.Core.Services;

public class ValidationResult {
    public Dictionary<string, string> Errors { get; set; } = [];

    public string? this[string key] => Errors.TryGetValue(key, out var value) ? value : null;

    public void SetError(string name, string message) {
        Errors[name] = message;
    }

    public void ClearError(string name) {
        Errors.Remove(name);
    }

    public void Clear() {
        Errors.Clear();
    }

    public bool IsValid() => Errors.Count == 0;

    public bool HasError(string name) => Errors.ContainsKey(name);
}
