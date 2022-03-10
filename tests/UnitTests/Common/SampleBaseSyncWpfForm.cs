namespace PCC.Datastructures.CSharp.WpfForm.UnitTests.Common;

public class SampleBaseSyncWpfForm : BaseWpfForm
{
    public SampleBaseSyncWpfForm(SampleBackingObject wrapped, int depth) : base(wrapped)
    {
        if (depth > 2) return;
        SomeComplex = new SampleBaseSyncWpfForm(wrapped.SomeComplex, depth + 1);
    }
    
    public SampleBaseSyncWpfForm(int depth) : base()
    {
        if (depth > 2) return;
        SomeComplex = new SampleBaseSyncWpfForm(depth + 1);
    }

    public int SomeInteger
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public object SomeReference
    {
        get => GetProperty<object>();
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