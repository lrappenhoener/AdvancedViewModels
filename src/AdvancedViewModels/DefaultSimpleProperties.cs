namespace AdvancedViewModels;

public class DefaultSimpleProperties : SimpleProperties
{
    private readonly Dictionary<string, object> _store = new();

    protected override void SetPropertyImplementation(string propertyName, object value)
    {
        _store[propertyName] = value;
    }

#nullable disable
    protected override T GetPropertyImplementation<T>(string propertyName)
    {
        return _store.ContainsKey(propertyName) ? (T) _store[propertyName] : default;
    }
}