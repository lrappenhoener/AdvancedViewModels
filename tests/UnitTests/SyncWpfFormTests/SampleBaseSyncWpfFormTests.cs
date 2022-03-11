using PCC.Datastructures.CSharp.BaseViewModel.UnitTests.Common;

namespace PCC.Datastructures.CSharp.BaseViewModel.UnitTests.SyncWpfFormTests;

public class SampleBaseSyncWpfFormTests : Tests
{
    protected override SampleBaseSyncViewModel CreateSut(SampleBackingObject wrappedObject)
    {
        return new SampleBaseSyncViewModel(wrappedObject, 0);
    }

    protected override SampleBackingObject CreateWrappedObject()
    {
        return new SampleBackingObject(0);
    }
}