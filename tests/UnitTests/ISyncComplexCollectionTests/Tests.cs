using PCC.Datastructures.CSharp.WpfForm.UnitTests.Common;
using PCC.Datastructures.CSharp.WpfForm.UnitTests.SyncWpfFormTests;

namespace PCC.Datastructures.CSharp.WpfForm.UnitTests.SyncComplexCollectionTests;

public abstract class Tests
{
    public abstract SyncComplexCollection<SampleBaseSyncWpfForm> CreateSut();
    public abstract SampleBaseSyncWpfForm CreateElement();
}