using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Nxa.Avalonia.Provider;

namespace Playground.Xaml;

public class MyStateObject
{
    private static int factoryValue;

    public int Value { get; set; }
    public int FactoryValue { get => factoryValue; set => factoryValue = value; }
    public MyStateObject()
    {
        factoryValue++;
    }
}
public partial class MyContentPage : ContentPage
{
    public MyContentPage()
    {
        InitializeComponent();
        var provider = this.FindControl<ProviderHost>("Provider");
        provider.RegisterValue(new MyStateObject() { Value = 2 });
        provider.RegisterFactory(()=>new MyStateObject());
    }
}