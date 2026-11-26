using Avalonia.Controls;

namespace Nxa.Avalonia.Provider;

public sealed class ProviderHost : ContentControl
{
    /// <summary>
    /// Register an object in the provider to be accesible in the scope
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    public void RegisterValue<T>(T value)
        where T : notnull
    {
        _providedObjects.Add(typeof(T), value);
    }
    /// <summary>
    /// Register a factory method to generate a new value when type is requested
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="factoryMethod"></param>
    public void RegisterFactory<T>(Func<T> factoryMethod)
        where T : notnull
    {
        _providedFactories.Add(typeof(T), () => factoryMethod()!);
    }
    /// <summary>
    /// Try Retriving Value object from the provider scope 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public T GetValue<T>()
    {
        if (_providedObjects.TryGetValue(typeof(T), out var value))
            return (T)value;

        throw new InvalidOperationException(
            $"Requested object of type {typeof(T)} isn't registered.");
    }
    /// <summary>
    /// Get a value from a factory
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public T GetFactoryValue<T>()
    {
        if (_providedFactories.TryGetValue(typeof(T), out var factory))
            return (T)factory();

        throw new InvalidOperationException(
            $"Requested factory of type {typeof(T)} isn't registered.");
    }

    private readonly Dictionary<Type, object> _providedObjects = new();
    private readonly Dictionary<Type, Func<object>> _providedFactories = new();
}