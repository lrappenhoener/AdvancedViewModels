using System.ComponentModel;

namespace PCC.Datastructures.CSharp.BaseViewModel;

public interface ITrackChanges : INotifyPropertyChanged
{
    bool IsDirty { get; }
    void AcceptChanges();
    void RejectChanges();
}