using System.ComponentModel;
using System.Runtime.CompilerServices;
using PCC.Libs.Nulls;

namespace PCC.Datastructures.CSharp.WpfForm;

public abstract class BaseSyncWpfForm : IComplexProperty
{
    private readonly Dictionary<string, object> _unsavedValues = new Dictionary<string, object>();
    private readonly ComplexProperties _complexProperties;

    protected BaseSyncWpfForm(object store)
    {
        Store = store;
        _complexProperties = new ComplexProperties();
        _complexProperties.PropertyChanged += (o, e) => FirePropertyChanged(e.PropertyName);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    
    public bool IsDirty => _unsavedValues.Any() || _complexProperties.IsDirty;

    protected object Store { get; }

    public void AcceptChanges()
    {
        foreach (var unsavedValue in _unsavedValues)
        {
            var propertyInfo = Store.GetType().GetProperty(unsavedValue.Key);
            propertyInfo.SetValue(Store, unsavedValue.Value);
        }
        _complexProperties.AcceptChanges();
        _unsavedValues.Clear();
    }

    protected void SetProperty(object value, [CallerMemberName] string propertyName = null)
    {
        _unsavedValues.Add(propertyName, value);
        FirePropertyChanged(propertyName);
    }
    
    private void FirePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void RejectChanges()
    {
        _complexProperties.RejectChanges();
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

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void SetComplexProperty(IComplexProperty complexProperty, [CallerMemberName] string propertyName = null)
    {
        _complexProperties.SetProperty(propertyName, complexProperty);        
    }
    
    protected T GetComplexProperty<T>([CallerMemberName] string propertyName = null)
    {
        return _complexProperties.GetProperty<T>(propertyName);        
    }
}