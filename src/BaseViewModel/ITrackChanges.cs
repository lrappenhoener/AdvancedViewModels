using System.ComponentModel;

namespace PCC.Datastructures.CSharp.BaseViewModel;

public interface ITrackChanges : INotifyPropertyChanged, INotifyDataErrorInfo
{
    bool IsDirty { get; }
    void AcceptChanges();
    void RejectChanges();
}