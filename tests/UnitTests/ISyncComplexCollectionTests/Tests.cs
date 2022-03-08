using PCC.Datastructures.CSharp.WpfForm.UnitTests.Common;
using PCC.Datastructures.CSharp.WpfForm.UnitTests.SyncWpfFormTests;

namespace PCC.Datastructures.CSharp.WpfForm.UnitTests.SyncComplexCollectionTests;

public abstract class Tests
{
    protected abstract SyncComplexCollection<SampleBaseSyncWpfForm> CreateSut();
    protected abstract SampleBaseSyncWpfForm CreateElement();
}