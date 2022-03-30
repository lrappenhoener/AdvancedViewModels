using PCC.Libraries.AdvancedViewModels.UnitTests.Common;

namespace PCC.Libraries.AdvancedViewModels.UnitTests.ViewModelTests;

public class SampleViewModelTests : Tests
{
    protected override SampleViewModel CreateSut()
    {
        return new SampleViewModel(0);
    }

    protected override SampleViewModel CreateComplex()
    {
        return new SampleViewModel(0);
    }
}