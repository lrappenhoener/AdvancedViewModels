using System.Collections.Specialized;

namespace PCC.Datastructures.CSharp.WpfForm;

public interface ISyncComplexCollection<T> : IList<T>, INotifyCollectionChanged, ITrackChanges
{
}