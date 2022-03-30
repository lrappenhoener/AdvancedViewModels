using System.ComponentModel;

namespace PCC.Libraries.AdvancedViewModels;

internal class ComplexPropertyRegistration
{
    public ComplexPropertyRegistration(string propertyName, PropertyChangedEventHandler propertyChangedHandler,
        EventHandler<DataErrorsChangedEventArgs> errorsChangedHandler, IComplexProperty target)
    {
        PropertyName = propertyName;
        PropertyChangedHandler = propertyChangedHandler;
        ErrorsChangedHandler = errorsChangedHandler;
        Target = target;
    }

    internal string PropertyName { get; private set; }
    internal PropertyChangedEventHandler PropertyChangedHandler { get; private set; }
    internal EventHandler<DataErrorsChangedEventArgs> ErrorsChangedHandler { get; }
    internal IComplexProperty Target { get; private set; }
}