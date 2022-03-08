using System.Collections.Generic;
using PCC.Datastructures.CSharp.WpfForm.UnitTests.Common;

namespace PCC.Datastructures.CSharp.WpfForm.UnitTests.SyncComplexCollectionTests;

public class SyncComplexCollectionTests : Tests
{
    protected override SyncComplexCollection<SampleBaseSyncWpfForm> CreateSut()
    {
        return new SyncComplexCollection<SampleBaseSyncWpfForm>();
    }

    protected override SyncComplexCollection<SampleBaseSyncWpfForm> CreateSut(IEnumerable<SampleBaseSyncWpfForm> elements)
    {
        return new SyncComplexCollection<SampleBaseSyncWpfForm>(elements);
    }

    protected override SampleBaseSyncWpfForm CreateElement()
    {
        return new SampleBaseSyncWpfForm(new SampleBackingObject(0), 0);
    }

    protected override IEnumerable<SampleBaseSyncWpfForm> CreateElements(int count)
    {
        var elements = new List<SampleBaseSyncWpfForm>();
        for (int i = 0; i < count; i++)
        {
            elements.Add(CreateElement());
        }

        return elements;
    }
}