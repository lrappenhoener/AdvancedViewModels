using AdvancedViewModels.UnitTests.Common;

namespace AdvancedViewModels.UnitTests.ViewModelTests;

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