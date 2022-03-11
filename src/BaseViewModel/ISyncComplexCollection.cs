using System.Collections.Specialized;

namespace PCC.Datastructures.CSharp.BaseViewModel;

public interface ISyncComplexCollection<T> : IList<T>, INotifyCollectionChanged, IComplexProperty
{
}