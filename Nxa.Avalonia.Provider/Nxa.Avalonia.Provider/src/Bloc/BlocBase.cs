namespace Nxa.Avalonia.Provider.Bloc;

public abstract class BlocBase<Tstate, Tevent>
    where Tstate : notnull
    where Tevent : notnull
{
    public event Action<Tstate>? StateChanged;
    public Tstate State { get; protected set; }
    public BlocBase(Tstate initialState)
    {
        this.State = initialState;
    }
    public Task Add<T>(T evt) where T : Tevent
    {
        if (_handlers.TryGetValue(evt.GetType(), out var handler))
            return handler(evt);
        throw new InvalidOperationException($"No handler registered for event {typeof(T).Name}");
    }
    protected void On<T>(Func<T, Task> handler) where T : Tevent
    {
        _handlers.Add(typeof(T), (e) => handler((T)e));
    }
    protected void Emit(Tstate newState)
    {
        State = newState;
        StateChanged?.Invoke(State);
    }
    private Dictionary<Type, Func<Tevent, Task>> _handlers = new();
}