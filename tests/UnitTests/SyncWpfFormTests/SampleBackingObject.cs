namespace PCC.Datastructures.CSharp.WpfForm.UnitTests.SyncWpfFormTests;

public class SampleBackingObject
{
    public SampleBackingObject(int depth)
    {
        if (depth > 2) return;
        SomeComplex = new SampleBaseSyncWpfForm(new SampleBackingObject(depth + 1)
        {
            SomeInteger = 1977,
            SomeReference = new object()
        });
    }
    
    public int SomeInteger { get; set; } = 42;
    public object SomeReference { get; set; } = new object();
    public SampleBaseSyncWpfForm SomeComplex { get; set; } 
}