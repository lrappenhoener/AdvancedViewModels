namespace PCC.Datastructures.CSharp.WpfForm.UnitTests.SyncWpfFormTests;

public class SampleBaseSyncWpfForm : BaseSyncWpfForm
{
    public SampleBaseSyncWpfForm(object wrapped) : base(wrapped)
    {
    }

    public int SomeInteger
    {
        get => (int)GetProperty().Value;
        set => SetProperty(value);
    }

    public string SomeString
    {
        get => (string)GetProperty().Value;
        set => SetProperty(value);
    }

    public SampleBaseSyncWpfForm SomeComplex
    {
        get => (SampleBaseSyncWpfForm)GetProperty().Value;
        set => SetProperty(value);
    }
}