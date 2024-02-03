using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Collections;

namespace Lantern.Blazor;

public sealed class BlazorWindowRootComponents : IJSComponentConfiguration
{
    private readonly LanternWebViewManager _manager;

    internal BlazorWindowRootComponents(LanternWebViewManager manager, JSComponentConfigurationStore jsComponents)
    {
        _manager = manager;
        JSComponents = jsComponents;
    }

    public JSComponentConfigurationStore JSComponents { get; }

    /// <summary>
    /// Adds a root component to the window.
    /// </summary>
    /// <typeparam name="TComponent">The component type.</typeparam>
    /// <param name="selector">A CSS selector describing where the component should be added in the host page.</param>
    /// <param name="parameters">An optional dictionary of parameters to pass to the component.</param>
    public void Add(Type typeComponent, string selector, IDictionary<string, object?>? parameters = null)
    {
        var parameterView = parameters == null
            ? ParameterView.Empty
            : ParameterView.FromDictionary(parameters);

        // Dispatch because this is going to be async, and we want to catch any errors
        _ = _manager.Dispatcher.InvokeAsync(async () =>
        {
            await _manager.AddRootComponentAsync(typeComponent, selector, parameterView);
        });
    }
}

public class RootComponentList : IEnumerable<(Type, string)>
{
    private readonly List<(Type componentType, string domElementSelector)> components = new List<(Type componentType, string domElementSelector)>();

    public void Add<TComponent>(string selector) where TComponent : IComponent
    {
        components.Add((typeof(TComponent), selector));
    }

    public void Add(Type componentType, string selector)
    {
        if (!typeof(IComponent).IsAssignableFrom(componentType))
        {
            throw new ArgumentException("The component type must implement IComponent interface.");
        }

        components.Add((componentType, selector));
    }

    public IEnumerator<(Type, string)> GetEnumerator()
    {
        return components.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return components.GetEnumerator();
    }
}
