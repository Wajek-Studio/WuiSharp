using System.Collections.Concurrent;

namespace Wajek.UI.Core.Services;

/// <summary>
/// Simple named pub/sub service.
/// <c>Set</c> broadcasts a value; <c>Get</c> subscribes and only receives values emitted after subscription.
/// </summary>
public class MessageService
{
    private readonly ConcurrentDictionary<string, object> _subjects = new();

    /// <summary>
    /// Broadcast <paramref name="data"/> to all subscribers of <paramref name="name"/>.
    /// </summary>
    public void Set<T>(string name, T data)
    {
        GetOrCreate<T>(name).Set(data);
    }

    /// <summary>
    /// Subscribe to <paramref name="name"/>.
    /// <paramref name="handler"/> is only called for values <c>Set</c> after this subscription.
    /// Returns <see cref="IDisposable"/> — dispose to unsubscribe.
    /// </summary>
    public IDisposable Get<T>(string name, Action<T> handler)
    {
        return GetOrCreate<T>(name).Subscribe(handler);
    }

    private NamedSubject<T> GetOrCreate<T>(string name)
    {
        return (NamedSubject<T>)_subjects.GetOrAdd(name, _ => new NamedSubject<T>());
    }

    // ── internal subject ────────────────────────────────────────────────

    private sealed class NamedSubject<T>
    {
        private readonly List<Action<T>> _handlers = [];
        private readonly Lock _lock = new();

        public void Set(T value)
        {
            Action<T>[] snapshot;
            lock (_lock)
            {
                snapshot = _handlers.ToArray();
            }

            foreach (var handler in snapshot)
                handler(value);
        }

        public IDisposable Subscribe(Action<T> handler)
        {
            lock (_lock)
            {
                _handlers.Add(handler);
            }

            return new Unsubscriber(() =>
            {
                lock (_lock) { _handlers.Remove(handler); }
            });
        }
    }

    // ── lightweight IDisposable ─────────────────────────────────────────

    private sealed class Unsubscriber(Action dispose) : IDisposable
    {
        public void Dispose() => dispose();
    }
}
