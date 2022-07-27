using System.Collections.Generic;
using PCC.Libraries.AdvancedViewModels.UnitTests.Common;

namespace PCC.Libraries.AdvancedViewModels.UnitTests.SyncComplexCollectionTests;

public class SyncComplexCollectionTests : Tests
{
    protected override SyncComplexCollection<SampleViewModel> CreateSut()
    {
        return new SyncComplexCollection<SampleViewModel>();
    }

    protected override SyncComplexCollection<SampleViewModel> CreateSut(List<SampleViewModel> wrappers)
    {
        return new SyncComplexCollection<SampleViewModel>(wrappers);
    }
}