using System.ComponentModel;

namespace PCC.Datastructures.CSharp.BaseViewModel;

public interface IValidate : INotifyDataErrorInfo
{
    bool IsDirtyAndValid { get; }
    bool IsValid { get; }    
}