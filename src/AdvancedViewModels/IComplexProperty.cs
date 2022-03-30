using System.ComponentModel;

namespace PCC.Libraries.AdvancedViewModels;

public interface IComplexProperty : ITrackChanges, INotifyDataErrorInfo
{
    bool CanSave { get; }
}