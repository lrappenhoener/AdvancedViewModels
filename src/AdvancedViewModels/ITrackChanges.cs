using System.ComponentModel;

namespace AdvancedViewModels;

public interface ITrackChanges : INotifyPropertyChanged
{
    bool IsDirty { get; }
    void AcceptChanges();
    void RejectChanges();
}