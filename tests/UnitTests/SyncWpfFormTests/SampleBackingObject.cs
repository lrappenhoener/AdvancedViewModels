namespace PCC.Datastructures.CSharp.WpfForm.UnitTests.SyncWpfFormTests;

public class SampleBackingObject
{
    private static int _depth = 0;
    public SampleBackingObject()
    {
        _depth++;
        if (_depth > 2) return;
        SomeComplex = new SampleBaseSyncWpfForm(new SampleBackingObject
        {
            SomeInteger = 1977,
            SomeString = "Birthday"
        });
    }
    
    public int SomeInteger { get; set; } = 42;
    public string SomeString { get; set; } = "nobody";
    public SampleBaseSyncWpfForm SomeComplex { get; set; } 
}