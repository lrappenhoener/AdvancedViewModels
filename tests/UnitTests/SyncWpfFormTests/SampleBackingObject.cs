namespace PCC.Datastructures.CSharp.WpfForm.UnitTests.SyncWpfFormTests;

public class SampleBackingObject
{
    public SampleBackingObject(int depth)
    {
        if (depth > 2) return;
        SomeComplex = new SampleBaseSyncWpfForm(new SampleBackingObject(depth + 1)
        {
            SomeInteger = 1977,
            SomeString = "Birthday"
        });
    }
    
    public int SomeInteger { get; set; } = 42;
    public string SomeString { get; set; } = "nobody";
    public SampleBaseSyncWpfForm SomeComplex { get; set; } 
}