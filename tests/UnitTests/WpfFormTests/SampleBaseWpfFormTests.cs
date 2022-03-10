using PCC.Datastructures.CSharp.WpfForm.UnitTests.Common;

namespace PCC.Datastructures.CSharp.WpfForm.UnitTests.WpfFormTests;

public class SampleBaseWpfFormTests : Tests
{
    protected override SampleBaseSyncWpfForm CreateSut()
    {
        return new SampleBaseSyncWpfForm(0);
    }

    protected override SampleBaseSyncWpfForm CreateComplex()
    {
        return new SampleBaseSyncWpfForm(0);
    }
}