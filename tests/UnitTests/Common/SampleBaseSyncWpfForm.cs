using PCC.Datastructures.CSharp.WpfForm.UnitTests.SyncWpfFormTests;

namespace PCC.Datastructures.CSharp.WpfForm.UnitTests.Common;

public class SampleBaseSyncWpfForm : BaseSyncWpfForm
{
    public SampleBaseSyncWpfForm(SampleBackingObject wrapped, int depth) : base(wrapped)
    {
        if (depth > 2) return;
        SomeComplex = new SampleBaseSyncWpfForm(wrapped.SomeComplex, depth + 1);
    }

    public int SomeInteger
    {
        get => (int)GetProperty().Value;
        set => SetProperty(value);
    }

    public object SomeReference
    {
        get => GetProperty().Value;
        set => SetProperty(value);
    }

    public SampleBaseSyncWpfForm SomeComplex
    {
        get => GetComplexProperty<SampleBaseSyncWpfForm>();
        set => SetComplexProperty(value);
    }

    public SyncComplexCollection<SampleBaseSyncWpfForm> ComplexCollection 
    {
        get => GetComplexProperty<SyncComplexCollection<SampleBaseSyncWpfForm>>();
        set => SetComplexProperty(value);
    }
}