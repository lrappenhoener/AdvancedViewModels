using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using PCC.Libs.Nulls;

namespace PCC.Datastructures.CSharp.WpfForm;

public abstract class BaseSyncWpfForm : INotifyPropertyChanged
{
    private readonly Dictionary<PropertyInfo, Tuple<INotifyPropertyChanged, PropertyChangedEventHandler>>
        _registeredChangeListeners =
            new Dictionary<PropertyInfo, Tuple<INotifyPropertyChanged, PropertyChangedEventHandler>>();
    private readonly Dictionary<string, object> _unsavedValues = new Dictionary<string, object>();

    protected BaseSyncWpfForm(object store)
    {
        Store = store;
        HookPropertyChangeProperties();
    }

    private void HookPropertyChangeProperties()
    {
        var propertiesThatInformChanges = Store.GetType().GetProperties().Where(p => p.PropertyType.GetInterface(nameof(INotifyPropertyChanged)) != null);
        foreach (var propertyThatInformChanges in propertiesThatInformChanges)
        {
            if (!(propertyThatInformChanges.GetValue(Store) is INotifyPropertyChanged property)) continue;
            var handler = new PropertyChangedEventHandler((o, e) =>
            {
                FirePropertyChanged(propertyThatInformChanges.Name);
            });
            property.PropertyChanged += handler;
            _registeredChangeListeners.Add(propertyThatInformChanges, Tuple.Create(property, handler));
        }
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
        FirePropertyChanged(propertyName);
        if (value is INotifyPropertyChanged complexValue)
        {
            var propertyInfo = Store.GetType().GetProperty(propertyName);
            if (_registeredChangeListeners.ContainsKey(propertyInfo))
            {
                var oldRegistration = _registeredChangeListeners[propertyInfo];
                oldRegistration.Item1.PropertyChanged -= oldRegistration.Item2;
            }
            var handler = new PropertyChangedEventHandler((o, e) => FirePropertyChanged(propertyName));
            _registeredChangeListeners[propertyInfo] = Tuple.Create<INotifyPropertyChanged, PropertyChangedEventHandler>(complexValue, handler);
            complexValue.PropertyChanged += handler;
        }
        _unsavedValues.Add(propertyName, value);
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