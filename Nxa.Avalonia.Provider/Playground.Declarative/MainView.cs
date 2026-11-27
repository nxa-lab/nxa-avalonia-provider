using Nxa.Avalonia.Provider;

namespace Playground.Declarative;

public class MainView() : ViewBase
{
    protected override object Build()
        => ViewFactory.Create<ProviderHostView>()
            .Name("MainView", Scope);
}