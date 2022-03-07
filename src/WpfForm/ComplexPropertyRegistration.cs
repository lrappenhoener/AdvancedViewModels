using System.ComponentModel;

namespace PCC.Datastructures.CSharp.WpfForm;

internal class ComplexPropertyRegistration
{
    public ComplexPropertyRegistration(string propertyName, PropertyChangedEventHandler handler, IComplexProperty target)
    {
        PropertyName = propertyName;
        Handler = handler;
        Target = target;
    }

    internal string PropertyName { get; private set; }
    internal PropertyChangedEventHandler Handler { get; private set; }
    internal IComplexProperty Target { get; private set; }
}