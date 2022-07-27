using System.Collections.Generic;

namespace PCC.Libraries.AdvancedViewModels.UnitTests.Common;

public class SampleViewModel : BaseViewModel
{
    public SampleViewModel(SampleBackingObject wrapped, int depth) : base(wrapped)
    {
        if (depth > 2) return;
        SomeComplex = new SampleViewModel(wrapped.SomeComplex, depth + 1);
    }

    public SampleViewModel(int depth)
    {
        if (depth > 2) return;
        SomeComplex = new SampleViewModel(depth + 1);
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
    public SampleViewModel SomeComplex
    {
        get => GetComplexProperty<SampleViewModel>();
        set => SetComplexProperty(value);
    }

    public SampleViewModel NullComplex
    {
        get => GetComplexProperty<SampleViewModel>();
        set => SetComplexProperty(value);
    }

    public SyncComplexCollection<SampleViewModel> ComplexCollection
    {
        get => GetComplexProperty<SyncComplexCollection<SampleViewModel>>();
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