using System.ComponentModel;

namespace PCC.Libraries.AdvancedViewModels;

internal class ComplexProperties : ITrackChanges
{
    private readonly Dictionary<string, ComplexPropertyRegistration>
        _complexPropertyRegistrations = new();

    private readonly Dictionary<string, Dictionary<string, IEnumerable<string>>> _errors = new();

    public event PropertyChangedEventHandler? PropertyChanged;
    public event Action<string, object, Dictionary<string, IEnumerable<string>>>? ErrorsChanged;

    public bool IsDirty => _complexPropertyRegistrations.Any(cpr => cpr.Value.Target.IsDirty);
    public bool HasErrors => _errors.Any();

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
            return default;
        var registration = _complexPropertyRegistrations[propertyName];
        return (T) registration.Target;
    }

    public void AcceptChanges()
    {
        var dirtyComplexProperties = _complexPropertyRegistrations.Where(cpr => cpr.Value.Target.IsDirty)
            .Select(cpr => cpr.Value.Target);
        foreach (var dirtyComplexProperty in dirtyComplexProperties) dirtyComplexProperty.AcceptChanges();
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
        var propertyChangedHandler =
            new PropertyChangedEventHandler((_, _) => FirePropertyChanged(complexPropertyName));
        complexProperty.PropertyChanged += propertyChangedHandler;

        var errorsChangedHandler =
            new EventHandler<DataErrorsChangedEventArgs>((o, e) =>
            {
                if (o is not IComplexProperty sender) return;
                var propertyName = e.PropertyName;
                OnErrorsChanged(complexPropertyName, sender, propertyName);
            });
        complexProperty.ErrorsChanged += errorsChangedHandler;

        _complexPropertyRegistrations.Add(complexPropertyName,
            new ComplexPropertyRegistration(complexPropertyName, propertyChangedHandler, errorsChangedHandler,
                complexProperty));
    }

    private void OnErrorsChanged(string complexPropertyName, IComplexProperty complexProperty, string propertyName)
    {
        var errors = (IEnumerable<string>) complexProperty.GetErrors(propertyName);

        if (!_errors.ContainsKey(complexPropertyName))
            _errors.Add(complexPropertyName, new Dictionary<string, IEnumerable<string>>());

        var complexPropertyErrors = _errors[complexPropertyName];
        if (complexPropertyErrors.ContainsKey(propertyName))
            complexPropertyErrors.Remove(propertyName);

        complexPropertyErrors.Add(propertyName, errors);

        ErrorsChanged?.Invoke(complexPropertyName, complexProperty, complexPropertyErrors);
    }

    private void UnregisterComplexProperty(string complexPropertyName)
    {
        var registration = _complexPropertyRegistrations[complexPropertyName];
        registration.Target.PropertyChanged -= registration.PropertyChangedHandler;
        registration.Target.ErrorsChanged -= registration.ErrorsChangedHandler;
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