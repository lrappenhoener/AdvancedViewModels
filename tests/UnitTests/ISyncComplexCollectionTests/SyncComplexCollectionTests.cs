using PCC.Datastructures.CSharp.WpfForm.UnitTests.Common;

namespace PCC.Datastructures.CSharp.WpfForm.UnitTests.SyncComplexCollectionTests;

public class SyncComplexCollectionTests : Tests
{
    protected override SyncComplexCollection<SampleBaseSyncWpfForm> CreateSut()
    {
        return new SyncComplexCollection<SampleBaseSyncWpfForm>();
    }

    protected override SampleBaseSyncWpfForm CreateElement()
    {
        return new SampleBaseSyncWpfForm(new SampleBackingObject(0), 0);
    }
}