using AdvancedViewModels.UnitTests.Common;

namespace AdvancedViewModels.UnitTests.SyncViewModelTests;

public class SampleSyncViewModelTests : Tests
{
    protected override SampleViewModel CreateSut(SampleBackingObject wrappedObject)
    {
        return new SampleViewModel(wrappedObject, 0);
    }

    protected override SampleBackingObject CreateWrappedObject()
    {
        return new SampleBackingObject(0);
    }
}