using Avalonia.Controls;
using Avalonia.LogicalTree;

namespace Nxa.Avalonia.Provider;

public static class ControlExtensions
{
    
    public static T? FetchProvidedValue<T>(this Control control)
    {
        var parent = control.GetLogicalParent();
        while (parent != null)
        {
            if (parent is ProviderHost providerHost)
                return providerHost.GetValue<T>();
            parent = parent.GetLogicalParent();
        }
        throw new Exception("Missing a Provider host");
    }
        public static T? FetchProvidedFactoryValue<T>(this Control control)
    {
        var parent = control.GetLogicalParent();
        while (parent != null)
        {
            if (parent is ProviderHost providerHost)
                return providerHost.GetFactoryValue<T>();
            parent = parent.GetLogicalParent();
        }
        throw new Exception("Missing a Provider host");
    }
}