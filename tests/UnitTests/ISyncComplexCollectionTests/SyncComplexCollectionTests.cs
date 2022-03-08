using PCC.Datastructures.CSharp.WpfForm.UnitTests.Common;

namespace PCC.Datastructures.CSharp.WpfForm.UnitTests.SyncComplexCollectionTests;

public class SyncComplexCollectionTests : Tests
{
    public override SyncComplexCollection<SampleBaseSyncWpfForm> CreateSut()
    {
        return new SyncComplexCollection<SampleBaseSyncWpfForm>();
    }

    public override SampleBaseSyncWpfForm CreateElement()
    {
        return new SampleBaseSyncWpfForm(new SampleBackingObject(0), 0);
    }
}