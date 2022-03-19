using System.Collections;
using System.ComponentModel;

namespace PCC.Datastructures.CSharp.BaseViewModel;

internal class ComplexProperties : ITrackChanges
{
    private readonly Dictionary<string, ComplexPropertyRegistration>
        _complexPropertyRegistrations =
            new Dictionary<string, ComplexPropertyRegistration>();
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public bool IsDirty => _complexPropertyRegistrations.Any(cpr => cpr.Value.Target.IsDirty);
    public bool IsDirtyAndValid { get; }
    public bool IsValid { get; }

    public void SetProperty(string complexPropertyName, IComplexProperty? complexProperty)
    {
        if (RegistrationExists(complexPropertyName))
            UnregisterComplexProperty(complexPropertyName);
        if (complexProperty == null) return;
        RegisterComplexProperty(complexPropertyName, complexProperty);
    }

    public T? GetProperty<T>(string propertyName)
    {
        if (!_complexPropertyRegistrations.ContainsKey(propertyName))
            return default(T);
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
            .Select(cpr => Tuple.Create(cpr.Key, cpr.Value.Target));
        foreach (var dirtyComplexProperty in dirtyComplexProperties)
        {
            dirtyComplexProperty.Item2.RejectChanges();
            FirePropertyChanged(dirtyComplexProperty.Item1);
        }
    }
    
    private void RegisterComplexProperty(string complexPropertyName, IComplexProperty complexProperty)
    {
        var handler = new PropertyChangedEventHandler((_, _) => FirePropertyChanged(complexPropertyName));
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

    public IEnumerable GetErrors(string propertyName)
    {
        throw new NotImplementedException();
    }

    public bool HasErrors { get; }
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
}