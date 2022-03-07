namespace PCC.Datastructures.CSharp.WpfForm;

public interface ITrackChanges
{
    bool IsDirty { get; }
    void AcceptChanges();
    void RejectChanges();
}