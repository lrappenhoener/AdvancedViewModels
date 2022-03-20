using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PCC.Datastructures.CSharp.BaseViewModel;

public abstract class BaseViewModel : IComplexProperty
{
    private readonly SimpleProperties _simpleProperties;
    private readonly ComplexProperties _complexProperties;
    private readonly Dictionary<string, IEnumerable<string>> _errors =
        new Dictionary<string, IEnumerable<string>>();

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
        Validate();
    }

    private void ComplexPropertyChanged(string propertyName)
    {
        FirePropertyChanged(propertyName);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public bool IsDirty => _simpleProperties.IsDirty || _complexProperties.IsDirty;
    public bool IsDirtyAndValid { get; }
    public bool IsValid => !_errors.Any();

    public void AcceptChanges()
    {
        _complexProperties.AcceptChanges();
        _simpleProperties.AcceptChanges();
    }

    public void RejectChanges()
    {
        _complexProperties.RejectChanges();
        _simpleProperties.RejectChanges();
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

        _errors.Clear();
        foreach (var failedPropertyValidation in results)
        {
            _errors[failedPropertyValidation.PropertyName] = failedPropertyValidation.Errors;
            FireErrorChanged(failedPropertyValidation.PropertyName);
        }

        foreach (var fixedError in fixedErrors)
        {
            FireErrorChanged(fixedError.Key);
        }
    }

    protected T GetProperty<T>([CallerMemberName] string propertyName = "")
    {
        return _simpleProperties.GetProperty<T>(propertyName);
    }

    protected void SetComplexProperty(IComplexProperty complexProperty, [CallerMemberName] string propertyName = "")
    {
        _complexProperties.SetProperty(propertyName, complexProperty);        
    }

    protected T? GetComplexProperty<T>([CallerMemberName] string propertyName = "")
    {
        return _complexProperties.GetProperty<T>(propertyName);        
    }
    
    private void FirePropertyChanged(string propertyName)
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
        throw new NotImplementedException();
    }

    public bool HasErrors { get; }
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    protected virtual IEnumerable<FailedPropertyValidation> ValidateImpl()
    {
        return new List<FailedPropertyValidation>();
    }
}