using Avalonia.LogicalTree;
using Avalonia.Markup.Declarative;
using CommunityToolkit.Mvvm.ComponentModel;
using Nxa.Avalonia.Provider;
using static Playground.Declarative.ProviderHostView;

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

        _valueBloc = this.FetchProvidedValue<ValueBloc>();

        if (_valueBloc is null)
            return;

        _valueBloc.StateChanged += ValueBlocChangedEventHandler;
    }

    protected override void OnDetachedFromLogicalTree(
        LogicalTreeAttachmentEventArgs e)
    {
        if (_valueBloc is not null)
            _valueBloc.StateChanged -= ValueBlocChangedEventHandler;

        _valueBloc = null;

        base.OnDetachedFromLogicalTree(e);
    }

    private void ValueBlocChangedEventHandler(int value)
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(
            () => UpdateValue(value));
    }

    private void UpdateValue(int value)
    {
        _state.ValueText = value.ToString();
    }

    private ProviderValueView(ViewState state) : base(state)
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
    private ValueBloc? _valueBloc;
}