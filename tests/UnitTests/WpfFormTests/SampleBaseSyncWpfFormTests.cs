using PCC.Datastructures.CSharp.BaseViewModel.UnitTests.Common;

namespace PCC.Datastructures.CSharp.BaseViewModel.UnitTests.WpfFormTests;

public class SampleBaseSyncWpfFormTests : Tests
{
    protected override SampleBaseSyncViewModel CreateSut()
    {
        return new SampleBaseSyncViewModel(new SampleBackingObject(0), 0);
    }

    protected override SampleBaseSyncViewModel CreateComplex()
    {
        return new SampleBaseSyncViewModel(new SampleBackingObject(0), 0);
    }
}