namespace PCC.Datastructures.CSharp.WpfForm.UnitTests.Common;
#nullable disable

public class SampleBackingObject
{
    public SampleBackingObject(int depth)
    {
        if (depth > 2) return;
        SomeComplex = new SampleBackingObject(depth + 1)
        {
            SomeInteger = 1977,
            SomeReference = new object()
        };
    }
    
    public int SomeInteger { get; set; } = 42;
    public object SomeReference { get; set; } = new object();
    public SampleBackingObject SomeComplex { get; set; } 
}