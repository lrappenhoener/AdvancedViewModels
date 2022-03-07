using System.ComponentModel;

namespace PCC.Datastructures.CSharp.WpfForm;

internal class ComplexPropertyRegistration
{
    public ComplexPropertyRegistration(string propertyName, PropertyChangedEventHandler handler, ITrackChanges target)
    {
        PropertyName = propertyName;
        Handler = handler;
        Target = target;
    }

    internal string PropertyName { get; private set; }
    internal PropertyChangedEventHandler Handler { get; private set; }
    internal ITrackChanges Target { get; private set; }
}