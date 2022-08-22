namespace AdvancedViewModels.UnitTests.Common;

public class SampleWrapper : Wrapper<SampleBackingObject>
{
    public SampleWrapper(SampleBackingObject model) : base(model)
    {
    }

    public int SomeInteger
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }
}