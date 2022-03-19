using System.ComponentModel;

namespace PCC.Datastructures.CSharp.BaseViewModel;

public interface ITrackChanges : INotifyPropertyChanged, INotifyDataErrorInfo
{
    bool IsDirty { get; }
    bool IsDirtyAndValid { get; }
    bool IsValid { get; }
    void AcceptChanges();
    void RejectChanges();
}