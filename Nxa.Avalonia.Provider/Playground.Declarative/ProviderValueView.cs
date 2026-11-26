using Avalonia.LogicalTree;
using Avalonia.Markup.Declarative;
using CommunityToolkit.Mvvm.ComponentModel;
using Nxa.Avalonia.Provider;

namespace Playground.Declarative;

public sealed class ProviderValueView : ViewBase<ProviderValueView.ViewState>
{
    public ProviderValueView() : this(new ViewState())
    {
    }

    protected override object Build(ViewState state) =>
        new TextBlock()
            .Text(state, x => x.ValueText);

    protected override void OnAttachedToLogicalTree(
        LogicalTreeAttachmentEventArgs e)
    {
        base.OnAttachedToLogicalTree(e);

        _source = this.FetchProvidedValue<MyStateObject>();
        _source.Changed += OnChanged;

        UpdateValue();
    }

    protected override void OnDetachedFromLogicalTree(
        LogicalTreeAttachmentEventArgs e)
    {
        if (_source is not null)
            _source.Changed -= OnChanged;

        _source = null;

        base.OnDetachedFromLogicalTree(e);
    }

    private void OnChanged()
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(UpdateValue);
    }

    private void UpdateValue()
    {
        if (_source is not null)
            _state.ValueText = $"Current value: {_source.Value}";
    }

    private ProviderValueView(ViewState state)
        : base(state)
    {
        _state = state;
    }

    public sealed class ViewState : ObservableObject
    {
        public string ValueText
        {
            get => _valueText;
            set => SetProperty(ref _valueText, value);
        }

        private string _valueText = "Waiting for provider...";
    }

    private readonly ViewState _state;
    private MyStateObject? _source;
}