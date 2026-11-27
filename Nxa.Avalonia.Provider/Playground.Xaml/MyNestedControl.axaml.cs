using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Nxa.Avalonia.Provider;
using CommunityToolkit.Mvvm.ComponentModel;
namespace Playground.Xaml;



public partial class MyNestedControl : UserControl
{
    public partial class State : ObservableObject
    {
        [ObservableProperty]
        private int _factoryValue = 0;
    }
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
    public void OnClick(object? sender, RoutedEventArgs args)
    {
        var provided = this.FetchProvidedFactoryValue<MyStateObject>();
        ViewModel.FactoryValue = provided.FactoryValue;
    }

    public State ViewModel { get; } = new();
}