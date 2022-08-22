using AdvancedViewModels.UnitTests.Common;

namespace AdvancedViewModels.UnitTests.WrapperTests;

public class SampleWrapperTests : Tests
{
    protected override SampleWrapper CreateSut(SampleBackingObject model)
    {
        return new SampleWrapper(model);
    }

    protected override SampleBackingObject CreateModel()
    {
        return new SampleBackingObject(0);
    }
}