using System.ComponentModel;

namespace PCC.Datastructures.CSharp.BaseViewModel;

public interface IComplexProperty : ITrackChanges, INotifyDataErrorInfo
{
    bool CanSave { get; }
}