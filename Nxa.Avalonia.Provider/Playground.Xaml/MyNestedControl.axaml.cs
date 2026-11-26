using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Nxa.Avalonia.Provider;
namespace Playground.Xaml;

public partial class MyNestedControl : UserControl
{
    public MyNestedControl()
    {
        InitializeComponent();
    }
    protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnAttachedToLogicalTree(e);
        var state = this.FetchProvidedValue<MyStateObject>();

        Console.WriteLine(state.Value);
    }
}