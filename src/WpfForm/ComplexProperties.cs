using System.ComponentModel;
using System.Reflection;

namespace PCC.Datastructures.CSharp.WpfForm;

internal class ComplexProperties : INotifyPropertyChanged
{
    private readonly Dictionary<string, PropertyInfo> _complexPropertyInfos;
    private readonly Dictionary<string, ComplexPropertyRegistration>
        _complexPropertyRegistrations =
            new Dictionary<string, ComplexPropertyRegistration>();
    
    public ComplexProperties(Type storeType)
    {
        _complexPropertyInfos = FilterComplexPropertyInfos(storeType);
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public void RegisterAllComplexPropertiesFrom(object store)
    {
        foreach (var complexPropertyInfoEntry in _complexPropertyInfos)
        {
            var complexPropertyName = complexPropertyInfoEntry.Key;
            var complexPropertyInstance = GetValueByReflection(complexPropertyName, store) as INotifyPropertyChanged;
            if (complexPropertyInstance == null) continue;
            RegisterComplexProperty(complexPropertyName, complexPropertyInstance);
        }
    }
    
    private void RegisterComplexProperty(string complexPropertyName, INotifyPropertyChanged complexPropertyInstance)
    {
        if (complexPropertyInstance == null) return;
        var handler = new PropertyChangedEventHandler((o, e) => FirePropertyChanged(complexPropertyName));
        complexPropertyInstance.PropertyChanged += handler;
        _complexPropertyRegistrations.Add(complexPropertyName, new ComplexPropertyRegistration(complexPropertyName, handler, complexPropertyInstance));
    }
    
    internal void UpdateComplexProperty(string complexPropertyName, object value)
    {
        if (RegistrationExists(complexPropertyName))
            UnregisterComplexProperty(complexPropertyName);
        var complexPropertyInstance = value as INotifyPropertyChanged;
        RegisterComplexProperty(complexPropertyName, complexPropertyInstance);
    }
    
    private void UnregisterComplexProperty(string complexPropertyName)
    {
        var registration = _complexPropertyRegistrations[complexPropertyName];
        registration.Target.PropertyChanged -= registration.Handler;
        _complexPropertyRegistrations.Remove(complexPropertyName);
    }
    
    private object GetValueByReflection(string propertyName, object instance)
    {
        return _complexPropertyInfos[propertyName].GetValue(instance);
    }

    private bool RegistrationExists(string propertyName)
    {
        return _complexPropertyRegistrations.ContainsKey(propertyName);
    }

    internal bool IsComplexProperty(string propertyName)
    {
        return _complexPropertyInfos.ContainsKey(propertyName);
    }

    private Dictionary<string, PropertyInfo> FilterComplexPropertyInfos(Type storeType)
    {
        return new Dictionary<string, PropertyInfo>(storeType.GetProperties().Where(p => p.PropertyType.GetInterface(nameof(INotifyPropertyChanged)) != null).Select(p => new KeyValuePair<string,PropertyInfo>(p.Name, p)));
    }

    private void FirePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}