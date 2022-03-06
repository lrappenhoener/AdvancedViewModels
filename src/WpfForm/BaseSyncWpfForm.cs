using System.Runtime.CompilerServices;
using PCC.Libs.Nulls;

namespace PCC.Datastructures.CSharp.WpfForm;

public abstract class BaseSyncWpfForm
{
    protected object _store;
    private readonly Dictionary<string, object> _unsavedValues = new Dictionary<string, object>();

    protected BaseSyncWpfForm(object store)
    {
        _store = store;
    }

    public bool IsDirty => _unsavedValues.Any();
    
    public void AcceptChanges()
    {
        foreach (var unsavedValue in _unsavedValues)
        {
            var propertyInfo = _store.GetType().GetProperty(unsavedValue.Key);
            propertyInfo.SetValue(_store, unsavedValue.Value);
        }
        _unsavedValues.Clear();
    }

    protected void SetProperty(object value, [CallerMemberName] string propertyName = null)
    {
        _unsavedValues.Add(propertyName, value);
    }

    public void RejectChanges()
    {
        _unsavedValues.Clear();
    }

    protected Maybe<object> GetProperty([CallerMemberName] string propertyName = null)
    {
        var maybeValue = GetPropertyFromUnsavedValues(propertyName);
        return maybeValue.HasValue ? maybeValue : GetPropertyFromStore(propertyName);
    }

    private Maybe<object> GetPropertyFromStore(string propertyName)
    {
        var propertyInfo = _store.GetType().GetProperty(propertyName);
        var value = propertyInfo.GetValue(_store);
        return Maybe.Some<object>(value);
    }

    private Maybe<object> GetPropertyFromUnsavedValues(string propertyName)
    {
        return _unsavedValues.ContainsKey(propertyName)
            ? Maybe.Some<object>(_unsavedValues[propertyName])
            : Maybe.None<object>();
    }
}