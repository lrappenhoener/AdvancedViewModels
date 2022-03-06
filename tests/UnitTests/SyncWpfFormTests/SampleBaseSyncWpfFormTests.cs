namespace PCC.Datastructures.CSharp.WpfForm.UnitTests.SyncWpfFormTests;

public class SampleBaseSyncWpfFormTests : Tests
{
    protected override SampleBaseSyncWpfForm CreateSut(object wrappedObject)
    {
        return new SampleBaseSyncWpfForm(wrappedObject);
    }

    protected override SampleBackingObject CreateWrappedObject()
    {
        return new SampleBackingObject(0);
    }
}