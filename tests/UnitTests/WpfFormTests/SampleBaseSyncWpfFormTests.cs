using PCC.Datastructures.CSharp.WpfForm.UnitTests.Common;

namespace PCC.Datastructures.CSharp.WpfForm.UnitTests.WpfFormTests;

public class SampleBaseSyncWpfFormTests : Tests
{
    protected override SampleBaseSyncWpfForm CreateSut()
    {
        return new SampleBaseSyncWpfForm(new SampleBackingObject(0), 0);
    }

    protected override SampleBaseSyncWpfForm CreateComplex()
    {
        return new SampleBaseSyncWpfForm(new SampleBackingObject(0), 0);
    }
}