using System.Collections.Specialized;

namespace AdvancedViewModels;

public interface ISyncComplexCollection<T> : IList<T>, INotifyCollectionChanged, IComplexProperty
{
}