using FluentAssertions;
using PCC.Datastructures.CSharp.WpfForm.UnitTests.Common;
using PCC.Datastructures.CSharp.WpfForm.UnitTests.SyncWpfFormTests;
using Xunit;

namespace PCC.Datastructures.CSharp.WpfForm.UnitTests.SyncComplexCollectionTests;

public abstract class Tests
{
    protected abstract SyncComplexCollection<SampleBaseSyncWpfForm> CreateSut();
    protected abstract SampleBaseSyncWpfForm CreateElement();

    [Fact]
    public void NotDirty_Initially()
    {
        var sut = CreateSut();

        sut.IsDirty.Should().BeFalse();
    }
}