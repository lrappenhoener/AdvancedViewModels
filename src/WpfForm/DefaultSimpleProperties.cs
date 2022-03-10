using System.Dynamic;
using PCC.Libs.Nulls;

namespace PCC.Datastructures.CSharp.WpfForm;

public class DefaultSimpleProperties : SimpleProperties
{
    private readonly Dictionary<string, object> _store = new Dictionary<string, object>();
    
    protected override void SetPropertyImplementation(string propertyName, object value)
    {
        _store[propertyName] = value;
    }

    protected override T GetPropertyImplementation<T>(string propertyName)
    {
        return _store.ContainsKey(propertyName) ? 
            (T)_store[propertyName] : 
            default(T);
    }
}