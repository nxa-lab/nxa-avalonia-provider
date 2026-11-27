using Nxa.Avalonia.Provider.Bloc;

public interface IValueEvent
{
    
}
public class IncrementEvent: IValueEvent
{
    
}
public class ValueBloc : BlocBase<int, IValueEvent>
{
    public ValueBloc():base(0)
    {
        this.On<IncrementEvent>(async (IncrementEvent evt) =>
        {
            this.Emit(this.State+2);
        });
    }
}