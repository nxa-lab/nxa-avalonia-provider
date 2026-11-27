# Nxa.Avalonia.Provider

A lightweight dependency injection and state management library for [Avalonia UI](https://avaloniaui), inspired by Flutter's Provider and BLoC patterns.

It provides two main features:

- **Provider:** Share objects and services with controls through the Avalonia logical tree.
- **BLoC:** Manage application state through events and state change notifications.

## Installation

```bash
dotnet add package Nxa.Avalonia.Provider
```

## Provider

`ProviderHost` makes registered objects available to its descendant controls.

### Registering values

Create a `ProviderHost` and register your objects before adding the host to the visual hierarchy:

```csharp
var provider = new ProviderHost();

provider.RegisterValue(new MyService());
provider.RegisterFactory(() => new AnotherService());
```

- `RegisterValue` provides the same instance whenever requested.
- `RegisterFactory` invokes the factory whenever a descendant requests the object.

You can use `ProviderHost` in XAML or construct it in code:

```xml
<provider:ProviderHost
    xmlns:provider="using:Nxa.Avalonia.Provider">

    <local:MyView />

</provider:ProviderHost>
```

### Retrieving values

Retrieve provided objects from a descendant control using:

```csharp
var service = this.FetchProvidedValue<MyService>();
```

For factory-registered objects:

```csharp
var service = this.FetchProvidedFactoryValue<AnotherService>();
```

**Important:** Retrieve provided objects when the control enters the logical tree, using `OnAttachedToLogicalTree`. At this point, the control can locate its parent `ProviderHost`.

```csharp
protected override void OnAttachedToLogicalTree(
    LogicalTreeAttachmentEventArgs e)
{
    base.OnAttachedToLogicalTree(e);

    var service = this.FetchProvidedValue<MyService>();
}
```

## BLoC

BLoC (Business Logic Component) separates application logic from the UI.

A BLoC receives events, processes them and emits new states. Controls can subscribe to state changes.

### Creating a BLoC

Define an event:

```csharp
public interface ICounterEvent { }

public sealed class IncrementEvent : ICounterEvent { }
```

Implement the BLoC:

```csharp
using Nxa.Avalonia.Provider.Bloc;

public sealed class CounterBloc : BlocBase<int, ICounterEvent>
{
    public CounterBloc() : base(0)
    {
        On<IncrementEvent>(evt =>
        {
            Emit(State + 1);
            return Task.CompletedTask;
        });
    }
}
```

### Dispatching events

```csharp
await bloc.Add(new IncrementEvent());

Console.WriteLine(bloc.State); // 1
```

### Using BLoC with Provider

Register the BLoC in a `ProviderHost`:

```csharp
var provider = new ProviderHost();
provider.RegisterValue(new CounterBloc());
```

Descendant controls can retrieve it and subscribe to state changes when they enter the logical tree.

```csharp
using Avalonia.LogicalTree;

private CounterBloc? _bloc;

protected override void OnAttachedToLogicalTree(
    LogicalTreeAttachmentEventArgs e)
{
    base.OnAttachedToLogicalTree(e);

    _bloc = this.FetchProvidedValue<CounterBloc>();

    if (_bloc is null)
        return;

    _bloc.StateChanged += OnStateChanged;

    // Read the current state because changes may have happened
    // before this control was attached.
    OnStateChanged(_bloc.State);
}

protected override void OnDetachedFromLogicalTree(
    LogicalTreeAttachmentEventArgs e)
{
    if (_bloc is not null)
        _bloc.StateChanged -= OnStateChanged;

    _bloc = null;

    base.OnDetachedFromLogicalTree(e);
}

private void OnStateChanged(int state)
{
    // Update your UI with the new state.
}
```

**Important lifecycle considerations:**

- Retrieve the BLoC in `OnAttachedToLogicalTree`, not in the constructor.
- Subscribe to `StateChanged` after retrieving it.
- Read its current `State` after subscribing, so the UI starts synchronized.
- Always unsubscribe in `OnDetachedFromLogicalTree` to avoid retaining detached controls and receiving unwanted notifications.
- A control may be attached and detached multiple times, so handle subscriptions accordingly.

## License

This project is licensed under the [MIT License](LICENSE).