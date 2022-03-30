using System.Collections.Specialized;

namespace PCC.Libraries.AdvancedViewModels;

public interface ISyncComplexCollection<T> : IList<T>, INotifyCollectionChanged, IComplexProperty
{
}