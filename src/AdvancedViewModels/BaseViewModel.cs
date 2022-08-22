using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AdvancedViewModels;

public abstract class BaseViewModel : IComplexProperty
{
    private readonly SimpleProperties _simpleProperties;
    private readonly ComplexProperties _complexProperties;
    private readonly Dictionary<string, IEnumerable<string>> _errors = new();

    protected BaseViewModel(object store) : this(new ReflectionSimpleProperties(store))
    {
    }

    protected BaseViewModel() : this(new DefaultSimpleProperties())
    {
    }

    private BaseViewModel(SimpleProperties simpleProperties)
    {
        _simpleProperties = simpleProperties;
        _simpleProperties.PropertyChanged += (_, e) => FirePropertyChanged(e.PropertyName);
        _complexProperties = new ComplexProperties();
        _complexProperties.PropertyChanged += (_, e) => ComplexPropertyChanged(e.PropertyName);
        _complexProperties.ErrorsChanged += OnComplexErrorsChanged;
        Validate();
    }

    private void OnComplexErrorsChanged(string propertyName, object property,
        Dictionary<string, IEnumerable<string>> errors)
    {
        FireErrorChanged(propertyName);
    }

    private void ComplexPropertyChanged(string propertyName)
    {
        FirePropertyChanged(propertyName);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public bool IsDirty => _simpleProperties.IsDirty || _complexProperties.IsDirty;
    public bool CanSave => IsDirty && !HasErrors;

    public void AcceptChanges()
    {
        PreAcceptChanges();
        _complexProperties.AcceptChanges();
        _simpleProperties.AcceptChanges();
        PostAcceptChanges();
    }

    protected virtual void PreAcceptChanges()
    {
    }

    protected virtual void PostAcceptChanges()
    {
    }

    public void RejectChanges()
    {
        PreRejectChanges();
        _complexProperties.RejectChanges();
        _simpleProperties.RejectChanges();
        PostRejectChanges();
    }

    protected virtual void PreRejectChanges()
    {
    }

    protected virtual void PostRejectChanges()
    {
    }

    protected void SetProperty(object value, [CallerMemberName] string propertyName = "")
    {
        _simpleProperties.SetProperty(propertyName, value);
        Validate();
    }

    private void Validate()
    {
        var results = ValidateImpl();
        var fixedErrors = _errors.Where(e => results.All(ee => ee.PropertyName != e.Key)).ToList();
        var newErrors = results.Where(e => _errors.All(ee => ee.Key != e.PropertyName)).ToList();
        var fireHasErrorsPropertyChanged = fixedErrors.Any() || newErrors.Any();

        _errors.Clear();

        foreach (var newError in newErrors)
        {
            _errors.Add(newError.PropertyName, newError.Errors);
            FireErrorChanged(newError.PropertyName);
        }

        foreach (var fixedError in fixedErrors) FireErrorChanged(fixedError.Key);

        if (fireHasErrorsPropertyChanged)
        {
            FirePropertyChanged(nameof(HasErrors));
            FirePropertyChanged(nameof(CanSave));
        }
    }

    protected T GetProperty<T>([CallerMemberName] string propertyName = "")
    {
        return _simpleProperties.GetProperty<T>(propertyName);
    }

    protected void SetComplexProperty(IComplexProperty? complexProperty, [CallerMemberName] string propertyName = "")
    {
        _complexProperties.SetProperty(propertyName, complexProperty);
    }

    protected T? GetComplexProperty<T>([CallerMemberName] string propertyName = "")
    {
        return _complexProperties.GetProperty<T>(propertyName);
    }

    public void FirePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDirty)));
    }

    private void FireErrorChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    public IEnumerable GetErrors(string propertyName)
    {
        if (!_errors.ContainsKey(propertyName))
            return new List<string>();

        return _errors[propertyName];
    }

    public bool HasErrors => _errors.Any() || _complexProperties.HasErrors;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    protected virtual IEnumerable<FailedPropertyValidation> ValidateImpl()
    {
        return new List<FailedPropertyValidation>();
    }
}