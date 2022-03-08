using PCC.Datastructures.CSharp.WpfForm.UnitTests.Common;

namespace PCC.Datastructures.CSharp.WpfForm.UnitTests.SyncWpfFormTests;

public class SampleBaseSyncWpfFormTests : Tests
{
    protected override SampleBaseSyncWpfForm CreateSut(SampleBackingObject wrappedObject)
    {
        return new SampleBaseSyncWpfForm(wrappedObject, 0);
    }

    protected override SampleBackingObject CreateWrappedObject()
    {
        return new SampleBackingObject(0);
    }
}