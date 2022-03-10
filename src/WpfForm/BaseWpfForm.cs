using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PCC.Datastructures.CSharp.WpfForm;

public abstract class BaseWpfForm : IComplexProperty
{
    private readonly SimpleProperties _simpleProperties;
    private readonly ComplexProperties _complexProperties;

    protected BaseWpfForm(object store) : this(new ReflectionSimpleProperties(store))
    {
    }

    protected BaseWpfForm() : this(new DefaultSimpleProperties())
    {
    }

    private BaseWpfForm(SimpleProperties simpleProperties)
    {
        _simpleProperties = simpleProperties;
        _complexProperties = new ComplexProperties();
        _complexProperties.PropertyChanged += (o, e) => FirePropertyChanged(e.PropertyName);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public bool IsDirty => _simpleProperties.IsDirty || _complexProperties.IsDirty;

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
    
    protected void SetProperty(object value, [CallerMemberName] string? propertyName = null)
    {
        _simpleProperties.SetProperty(propertyName, value);
        FirePropertyChanged(propertyName);
    }
    
    protected T GetProperty<T>([CallerMemberName] string? propertyName = null)
    {
        return _simpleProperties.GetProperty<T>(propertyName);
    }

    protected void SetComplexProperty(IComplexProperty complexProperty, [CallerMemberName] string? propertyName = null)
    {
        _complexProperties.SetProperty(propertyName, complexProperty);        
    }

    protected T GetComplexProperty<T>([CallerMemberName] string? propertyName = null)
    {
        return _complexProperties.GetProperty<T>(propertyName);        
    }
    
    private void FirePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}