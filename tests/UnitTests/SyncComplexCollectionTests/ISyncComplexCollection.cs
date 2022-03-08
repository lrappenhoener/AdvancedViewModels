using System.Collections.Generic;
using System.Collections.Specialized;

namespace PCC.Datastructures.CSharp.WpfForm.UnitTests.SyncComplexCollectionTests;

public interface ISyncComplexCollection<T> : IList<T>, INotifyCollectionChanged, ITrackChanges
{
}