using System.ComponentModel;

namespace PCC.Libraries.AdvancedViewModels;

public interface ITrackChanges : INotifyPropertyChanged
{
    bool IsDirty { get; }
    void AcceptChanges();
    void RejectChanges();
}