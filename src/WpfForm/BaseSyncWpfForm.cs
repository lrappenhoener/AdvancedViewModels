using System.Runtime.CompilerServices;
using PCC.Libs.Nulls;

namespace PCC.Datastructures.CSharp.WpfForm;

public abstract class BaseSyncWpfForm
{
    private readonly Dictionary<string, object> _unsavedValues = new Dictionary<string, object>();

    protected BaseSyncWpfForm(object store)
    {
        Store = store;
    }

    public bool IsDirty => _unsavedValues.Any();

    protected object Store { get; }

    public void AcceptChanges()
    {
        foreach (var unsavedValue in _unsavedValues)
        {
            var propertyInfo = Store.GetType().GetProperty(unsavedValue.Key);
            propertyInfo.SetValue(Store, unsavedValue.Value);
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
        var propertyInfo = Store.GetType().GetProperty(propertyName);
        var value = propertyInfo.GetValue(Store);
        return Maybe.Some<object>(value);
    }

    private Maybe<object> GetPropertyFromUnsavedValues(string propertyName)
    {
        return _unsavedValues.ContainsKey(propertyName)
            ? Maybe.Some<object>(_unsavedValues[propertyName])
            : Maybe.None<object>();
    }
}