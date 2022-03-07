using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using PCC.Libs.Nulls;

namespace PCC.Datastructures.CSharp.WpfForm;

public abstract class BaseSyncWpfForm : INotifyPropertyChanged
{
    private Dictionary<string, PropertyInfo> _complexPropertyInfos;
    private readonly Dictionary<string, PropertyChangedEventHandler>
        _complexPropertyHandlers =
            new Dictionary<string, PropertyChangedEventHandler>();
    private readonly Dictionary<string, object> _unsavedValues = new Dictionary<string, object>();

    protected BaseSyncWpfForm(object store)
    {
        Store = store;
        RegisterComplexProperties(store);
    }

    private void RegisterComplexProperties(object store)
    {
        _complexPropertyInfos = FilterComplexPropertyInfos(store);
        foreach (var complexPropertyInfoEntry in _complexPropertyInfos)
        {
            var complexPropertyName = complexPropertyInfoEntry.Key;
            var complexPropertyInstance = GetValueByReflection(complexPropertyName, store) as INotifyPropertyChanged;
            if (complexPropertyInstance == null) continue;
            RegisterComplexProperty(complexPropertyName, complexPropertyInstance);
        }
    }

    private void RegisterComplexProperty(string complexPropertyName, INotifyPropertyChanged propertyInstance)
    {
        var handler = new PropertyChangedEventHandler((o, e) => FirePropertyChanged(complexPropertyName));
        if (propertyInstance == null) return;
        propertyInstance.PropertyChanged += handler;
        _complexPropertyHandlers.Add(complexPropertyName, handler);
    }

    private Dictionary<string, PropertyInfo> FilterComplexPropertyInfos(object store)
    {
        return new Dictionary<string, PropertyInfo>(store.GetType().GetProperties().Where(p => p.PropertyType.GetInterface(nameof(INotifyPropertyChanged)) != null).Select(p => new KeyValuePair<string,PropertyInfo>(p.Name, p)));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    
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
        if (IsComplexProperty(propertyName))
            RefreshComplexProperty(propertyName, value);
        _unsavedValues.Add(propertyName, value);
        FirePropertyChanged(propertyName);
    }

    private void RefreshComplexProperty(string complexPropertyName, object value)
    {
        if (OldHandlerExists(complexPropertyName))
            UnregisterComplexProperty(complexPropertyName);
        var complexPropertyInstance = value as INotifyPropertyChanged;
        RegisterComplexProperty(complexPropertyName, complexPropertyInstance);
    }

    private void UnregisterComplexProperty(string complexPropertyName)
    {
        var oldPropertyHandler = _complexPropertyHandlers[complexPropertyName];
        var oldPropertyInstance = GetOldPropertyInstance(complexPropertyName);
        if (oldPropertyInstance != null)
            oldPropertyInstance.PropertyChanged -= oldPropertyHandler;
        _complexPropertyHandlers.Remove(complexPropertyName);
    }

    private INotifyPropertyChanged GetOldPropertyInstance(string propertyName)
    {
        return _unsavedValues.ContainsKey(propertyName)
            ? _unsavedValues[propertyName] as INotifyPropertyChanged
            : GetValueByReflection(propertyName, Store) as INotifyPropertyChanged;
    }

    private object GetValueByReflection(string propertyName, object instance)
    {
        return _complexPropertyInfos[propertyName].GetValue(instance);
    }

    private bool OldHandlerExists(string propertyName)
    {
        return _complexPropertyHandlers.ContainsKey(propertyName);
    }

    private bool IsComplexProperty(string propertyName)
    {
        return _complexPropertyInfos.ContainsKey(propertyName);
    }

    private void FirePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}