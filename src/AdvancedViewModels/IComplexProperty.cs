using System.ComponentModel;

namespace AdvancedViewModels;

public interface IComplexProperty : ITrackChanges, INotifyDataErrorInfo
{
    bool CanSave { get; }
}

public interface IComplexProperty<out TModel> : IComplexProperty
{
    TModel Model { get; }
}