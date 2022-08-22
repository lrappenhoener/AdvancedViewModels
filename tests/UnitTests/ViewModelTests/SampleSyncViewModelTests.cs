using AdvancedViewModels.UnitTests.Common;

namespace AdvancedViewModels.UnitTests.ViewModelTests;

public class SampleSyncViewModelTests : Tests
{
    protected override SampleViewModel CreateSut()
    {
        return new SampleViewModel(new SampleBackingObject(0), 0);
    }

    protected override SampleViewModel CreateComplex()
    {
        return new SampleViewModel(new SampleBackingObject(0), 0);
    }
}