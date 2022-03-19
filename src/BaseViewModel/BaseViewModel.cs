using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PCC.Datastructures.CSharp.BaseViewModel;

public abstract class BaseViewModel : IComplexProperty
{
    private readonly SimpleProperties _simpleProperties;
    private readonly ComplexProperties _complexProperties;

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
        _complexProperties.PropertyChanged += (_, e) => FirePropertyChanged(e.PropertyName);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public bool IsDirty => _simpleProperties.IsDirty || _complexProperties.IsDirty;
    public bool IsDirtyAndValid { get; }
    public bool IsValid { get; private set; }

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