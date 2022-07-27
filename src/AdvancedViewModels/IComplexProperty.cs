using System.ComponentModel;

namespace PCC.Libraries.AdvancedViewModels;

public interface IComplexProperty : ITrackChanges, INotifyDataErrorInfo
{
    bool CanSave { get; }
}

public interface IComplexProperty<out TModel> : IComplexProperty
{
    TModel Model { get; }
}