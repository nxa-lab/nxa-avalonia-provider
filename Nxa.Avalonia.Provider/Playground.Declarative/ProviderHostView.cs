using Avalonia.Markup.Declarative;
using CommunityToolkit.Mvvm.ComponentModel;
using Nxa.Avalonia.Provider;

namespace Playground.Declarative;

public sealed class ProviderHostView : ViewBase<ProviderHostView.ViewState>
{
    public ProviderHostView() : this(new ViewState())
    {
    }

    protected override object Build(ViewState state)
    {
        _source = new MyStateObject
        {
            Value = state.Value
        };
        var _valueBloc = new ValueBloc();
        var provider = new ProviderHost();
        provider.RegisterValue(_source);
        provider.RegisterValue(_valueBloc);

        return provider.Content(
            new StackPanel()
                .Children(
                    new ProviderValueView(),
                    new Button()
                        .Content("Change value")
                        .OnClick(_ => { state.Value++; _valueBloc.Add(new IncrementEvent()); })
                )
        );
    }

    private ProviderHostView(ViewState state) : base(state)
    {
        _state = state;

        _state.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(ViewState.Value))
                _source.Value = _state.Value;
        };
    }

    public sealed class ViewState : ObservableObject
    {
        public int Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        private int _value = 10;
    }

    private readonly ViewState _state;
    private MyStateObject _source = new();

    public sealed class MyStateObject
    {
        public int Value
        {
            get => _value;
            set
            {
                if (_value == value)
                    return;

                _value = value;
                Changed?.Invoke();
            }
        }

        public event Action? Changed;

        private int _value;
    }
}