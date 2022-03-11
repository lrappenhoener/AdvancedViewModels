using System.Collections.Generic;
using PCC.Datastructures.CSharp.BaseViewModel.UnitTests.Common;

namespace PCC.Datastructures.CSharp.BaseViewModel.UnitTests.ISyncComplexCollectionTests;

public class SyncComplexCollectionTests : Tests
{
    protected override SyncComplexCollection<SampleBaseSyncViewModel> CreateSut()
    {
        return new SyncComplexCollection<SampleBaseSyncViewModel>();
    }

    protected override SyncComplexCollection<SampleBaseSyncViewModel> CreateSut(List<SampleBaseSyncViewModel> elements)
    {
        return new SyncComplexCollection<SampleBaseSyncViewModel>(elements);
    }

    protected override SampleBaseSyncViewModel CreateElement()
    {
        return new SampleBaseSyncViewModel(new SampleBackingObject(0), 0);
    }

    protected override List<SampleBaseSyncViewModel> CreateElements(int count)
    {
        var elements = new List<SampleBaseSyncViewModel>();
        for (int i = 0; i < count; i++)
        {
            elements.Add(CreateElement());
        }

        return elements;
    }
}