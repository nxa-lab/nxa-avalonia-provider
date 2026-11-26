using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Nxa.Avalonia.Provider;

namespace Playground.Xaml;

public class MyStateObject
{
    public int Value { get; set; }
}
public partial class MyContentPage : ContentPage
{
    public MyContentPage()
    {
        InitializeComponent();
        var provider = this.FindControl<ProviderHost>("Provider");
        provider.RegisterValue(new MyStateObject() { Value = 2 });
    }
}