using System.ComponentModel;
using PCC.Libs.Nulls;

namespace PCC.Datastructures.CSharp.WpfForm;

public abstract class SimpleProperties
{
    private readonly Dictionary<string, object> _unsavedValues = new Dictionary<string, object>();
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public bool IsDirty => _unsavedValues.Any();

    public void SetProperty(string propertyName, object value)
    {
        _unsavedValues[propertyName] = value;
    }

    public Maybe<object> GetProperty(string propertyName)
    {
        return _unsavedValues.ContainsKey(propertyName)
            ? Maybe.Some<object>(_unsavedValues[propertyName])
            : GetPropertyImplementation(propertyName);
    }
    
    public void AcceptChanges()
    {
        foreach (var unsavedValue in _unsavedValues)
        {
            var propertyName = unsavedValue.Key;
            SetPropertyImplementation(propertyName, unsavedValue.Value);
        }
        _unsavedValues.Clear();
    }

    public void RejectChanges()
    {
        _unsavedValues.Clear();
    }    
    
    protected abstract void SetPropertyImplementation(string propertyName, object value);

    protected abstract Maybe<object> GetPropertyImplementation(string propertyName);
}