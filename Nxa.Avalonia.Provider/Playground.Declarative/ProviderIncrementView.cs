using Avalonia.LogicalTree;
using Avalonia.Markup.Declarative;
using CommunityToolkit.Mvvm.ComponentModel;
using Nxa.Avalonia.Provider;

namespace Playground.Declarative;

public sealed class ProviderIncrementView : ViewBase<ProviderIncrementView.ViewState>
{
    public ProviderIncrementView() : base(new ViewState())
    {
    }

    protected override object Build(ViewState state) =>
        new Button()
            .Content("Increment shared value")
            .OnClick(_ =>
            {
                if (_source is not null)
                    _source.Value++;
            });

    protected override void OnAttachedToLogicalTree(
        LogicalTreeAttachmentEventArgs e)
    {
        base.OnAttachedToLogicalTree(e);

        _source = this.FetchProvidedValue<MyStateObject>();
    }

    protected override void OnDetachedFromLogicalTree(
        LogicalTreeAttachmentEventArgs e)
    {
        _source = null;

        base.OnDetachedFromLogicalTree(e);
    }

    public sealed class ViewState : ObservableObject;

    private MyStateObject? _source;
}