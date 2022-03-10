using System.Reflection;
using PCC.Libs.Nulls;

namespace PCC.Datastructures.CSharp.WpfForm;

public class ReflectionSimpleProperties : SimpleProperties
{
    private readonly object _store;
    private readonly Type _storeType;
    private readonly Dictionary<string, PropertyInfo> _propertyInfos = new Dictionary<string, PropertyInfo>();

    public ReflectionSimpleProperties(object store)
    {
        _store = store;
        _storeType = store.GetType();
    }

    protected override void SetPropertyImplementation(string propertyName, object value)
    {
        RefreshPropertyInfo(propertyName);
        _propertyInfos[propertyName].SetValue(_store, value);
    }

    protected override Maybe<object> GetPropertyImplementation(string propertyName)
    {
        RefreshPropertyInfo(propertyName);
        var value = _propertyInfos[propertyName].GetValue(_store);
        return value != null ? 
            Maybe.Some<object>(value) : 
            Maybe.None<object>();
    }

    private void RefreshPropertyInfo(string propertyName)
    {
        if (!_propertyInfos.ContainsKey(propertyName))
            _propertyInfos.Add(propertyName, _storeType.GetProperty(propertyName));
    }
}