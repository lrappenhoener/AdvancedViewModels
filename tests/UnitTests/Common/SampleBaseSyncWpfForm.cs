namespace PCC.Datastructures.CSharp.WpfForm.UnitTests.Common;

public class SampleBaseSyncWpfForm : BaseWpfForm
{
    public SampleBaseSyncWpfForm(SampleBackingObject wrapped, int depth) : base(wrapped)
    {
        if (depth > 2) return;
        SomeComplex = new SampleBaseSyncWpfForm(wrapped.SomeComplex, depth + 1);
    }
    
    public SampleBaseSyncWpfForm(int depth)
    {
        if (depth > 2) return;
        SomeComplex = new SampleBaseSyncWpfForm(depth + 1);
    }

    public int SomeInteger
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int UninitializedInteger 
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public object SomeReference
    {
        get => GetProperty<object>();
        set => SetProperty(value);
    }
    
    #nullable disable
    public SampleBaseSyncWpfForm SomeComplex
    {
        get => GetComplexProperty<SampleBaseSyncWpfForm>();
        set => SetComplexProperty(value);
    }
    
    public SampleBaseSyncWpfForm NullComplex
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