using System.ComponentModel;

namespace PCC.Libraries.AdvancedViewModels;

public abstract class SimpleProperties
{
    private readonly Dictionary<string, object> _unsavedValues = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    public bool IsDirty => _unsavedValues.Any();

    public void SetProperty(string propertyName, object value)
    {
        _unsavedValues[propertyName] = value;
        FirePropertyChanged(propertyName);
    }

    public T GetProperty<T>(string propertyName)
    {
        return _unsavedValues.ContainsKey(propertyName)
            ? (T) _unsavedValues[propertyName]
            : GetPropertyImplementation<T>(propertyName);
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
        foreach (var unsavedValue in _unsavedValues)
            FirePropertyChanged(unsavedValue.Key);
        _unsavedValues.Clear();
    }

    protected abstract void SetPropertyImplementation(string propertyName, object value);

    protected abstract T GetPropertyImplementation<T>(string propertyName);

    private void FirePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}