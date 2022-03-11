using PCC.Datastructures.CSharp.BaseViewModel.UnitTests.Common;

namespace PCC.Datastructures.CSharp.BaseViewModel.UnitTests.WpfFormTests;

public class SampleBaseWpfFormTests : Tests
{
    protected override SampleBaseSyncViewModel CreateSut()
    {
        return new SampleBaseSyncViewModel(0);
    }

    protected override SampleBaseSyncViewModel CreateComplex()
    {
        return new SampleBaseSyncViewModel(0);
    }
}