namespace PCC.Datastructures.CSharp.WpfForm.UnitTests.SyncWpfFormTests;

public class SampleBaseSyncWpfForm : BaseSyncWpfForm
{
    public SampleBaseSyncWpfForm(object wrapped) : base(wrapped)
    {
    }

    public int SomeInteger
    {
        get => 1;
        set => SetProperty(value);
    }
}