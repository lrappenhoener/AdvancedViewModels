using PCC.Libraries.AdvancedViewModels.UnitTests.Common;

namespace PCC.Libraries.AdvancedViewModels.UnitTests.ViewModelTests;

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