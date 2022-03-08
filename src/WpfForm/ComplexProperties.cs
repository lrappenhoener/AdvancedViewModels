using System.ComponentModel;
using System.Reflection;

namespace PCC.Datastructures.CSharp.WpfForm;

internal class ComplexProperties : ITrackChanges
{
    private readonly Dictionary<string, ComplexPropertyRegistration>
        _complexPropertyRegistrations =
            new Dictionary<string, ComplexPropertyRegistration>();
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public bool IsDirty => _complexPropertyRegistrations.Any(cpr => cpr.Value.Target.IsDirty);
    
    public void SetProperty(string complexPropertyName, IComplexProperty complexProperty)
    {
        if (RegistrationExists(complexPropertyName))
            UnregisterComplexProperty(complexPropertyName);
        RegisterComplexProperty(complexPropertyName, complexProperty);
    }

    public T GetProperty<T>(string propertyName)
    {
        var registration = _complexPropertyRegistrations[propertyName];
        return (T)registration.Target;
    }
    
    public void AcceptChanges()
    {
        var dirtyComplexProperties = _complexPropertyRegistrations.Where(cpr => cpr.Value.Target.IsDirty)
            .Select(cpr => cpr.Value.Target);
        foreach (var dirtyComplexProperty in dirtyComplexProperties)
        {
            dirtyComplexProperty.AcceptChanges();
        }
    }

    public void RejectChanges()
    {
        var dirtyComplexProperties = _complexPropertyRegistrations.Where(cpr => cpr.Value.Target.IsDirty)
            .Select(cpr => cpr.Value.Target);
        foreach (var dirtyComplexProperty in dirtyComplexProperties)
        {
            dirtyComplexProperty.RejectChanges();
        }
    }
    
    private void RegisterComplexProperty(string complexPropertyName, IComplexProperty complexProperty)
    {
        if (complexProperty == null) return;
        var handler = new PropertyChangedEventHandler((o, e) => FirePropertyChanged(complexPropertyName));
        complexProperty.PropertyChanged += handler;
        _complexPropertyRegistrations.Add(complexPropertyName,
            new ComplexPropertyRegistration(complexPropertyName, handler, complexProperty));
    }
    
    private void UnregisterComplexProperty(string complexPropertyName)
    {
        var registration = _complexPropertyRegistrations[complexPropertyName];
        registration.Target.PropertyChanged -= registration.Handler;
        _complexPropertyRegistrations.Remove(complexPropertyName);
    }
    
    private bool RegistrationExists(string propertyName)
    {
        return _complexPropertyRegistrations.ContainsKey(propertyName);
    }

    private void FirePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}