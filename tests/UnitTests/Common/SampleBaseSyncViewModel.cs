using System.Collections.Generic;

namespace PCC.Datastructures.CSharp.BaseViewModel.UnitTests.Common;

public class SampleBaseSyncViewModel : BaseViewModel
{
    public SampleBaseSyncViewModel(SampleBackingObject wrapped, int depth) : base(wrapped)
    {
        if (depth > 2) return;
        SomeComplex = new SampleBaseSyncViewModel(wrapped.SomeComplex, depth + 1);
    }
    
    public SampleBaseSyncViewModel(int depth)
    {
        if (depth > 2) return;
        SomeComplex = new SampleBaseSyncViewModel(depth + 1);
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
    public SampleBaseSyncViewModel SomeComplex
    {
        get => GetComplexProperty<SampleBaseSyncViewModel>();
        set => SetComplexProperty(value);
    }
    
    public SampleBaseSyncViewModel NullComplex
    {
        get => GetComplexProperty<SampleBaseSyncViewModel>();
        set => SetComplexProperty(value);
    }

    public SyncComplexCollection<SampleBaseSyncViewModel> ComplexCollection 
    {
        get => GetComplexProperty<SyncComplexCollection<SampleBaseSyncViewModel>>();
        set => SetComplexProperty(value);
    }

    public bool ValidationCalled { get; private set; }

    protected override IEnumerable<FailedPropertyValidation> ValidateImpl()
    {
        ValidationCalled = true;
        var results = new List<FailedPropertyValidation>();
        
        if (SomeInteger < 0)
            results.Add(new FailedPropertyValidation(nameof(SomeInteger), new List<string>
            {
                "SomeInteger < 0"
            }));
        
        return results;
    }
}