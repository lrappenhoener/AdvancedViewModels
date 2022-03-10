using PCC.Libs.Nulls;

namespace PCC.Datastructures.CSharp.WpfForm;

public interface IStore
{
    void SetValue(string propertyName, object value);
    Maybe<object> GetValue(string propertyName);
}