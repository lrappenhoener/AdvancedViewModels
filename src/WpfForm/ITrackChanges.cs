using System.ComponentModel;

namespace PCC.Datastructures.CSharp.WpfForm;

public interface ITrackChanges : INotifyPropertyChanged
{
    bool IsDirty { get; }
    void AcceptChanges();
    void RejectChanges();
}