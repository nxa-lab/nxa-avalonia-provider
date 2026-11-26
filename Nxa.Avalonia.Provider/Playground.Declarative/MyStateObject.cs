namespace Playground.Declarative;

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
        }
    }

    private int _value;
}