using System.Runtime.CompilerServices;

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
    }

    protected void SetProperty(object value, [CallerMemberName] string propertyName = null)
    {
        _unsavedValues.Add(propertyName, value);
    }
}