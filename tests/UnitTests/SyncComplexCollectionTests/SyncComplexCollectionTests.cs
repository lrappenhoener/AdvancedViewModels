using System.Collections.Generic;
using PCC.Libraries.AdvancedViewModels.UnitTests.Common;

namespace PCC.Libraries.AdvancedViewModels.UnitTests.SyncComplexCollectionTests;

public class SyncComplexCollectionTests : Tests
{
    protected override SyncComplexCollection<SampleViewModel> CreateSut()
    {
        return new SyncComplexCollection<SampleViewModel>();
    }

    protected override SyncComplexCollection<SampleViewModel> CreateSut(List<SampleViewModel> elements)
    {
        return new SyncComplexCollection<SampleViewModel>(elements);
    }

    protected override SampleViewModel CreateElement()
    {
        return new SampleViewModel(new SampleBackingObject(0), 0);
    }

    protected override List<SampleViewModel> CreateElements(int count)
    {
        var elements = new List<SampleViewModel>();
        for (int i = 0; i < count; i++)
        {
            elements.Add(CreateElement());
        }

        return elements;
    }
}