using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using PCC.Datastructures.CSharp.WpfForm.UnitTests.Common;
using PCC.Datastructures.CSharp.WpfForm.UnitTests.SyncWpfFormTests;
using Xunit;

namespace PCC.Datastructures.CSharp.WpfForm.UnitTests.SyncComplexCollectionTests;

public abstract class Tests
{
    protected abstract SyncComplexCollection<SampleBaseSyncWpfForm> CreateSut();
    protected abstract SyncComplexCollection<SampleBaseSyncWpfForm> CreateSut(IEnumerable<SampleBaseSyncWpfForm> elements);
    protected abstract SampleBaseSyncWpfForm CreateElement();
    protected abstract IEnumerable<SampleBaseSyncWpfForm> CreateElements(int count);

    [Fact]
    public void NotDirty_Initially()
    {
        var sut = CreateSut();

        sut.IsDirty.Should().BeFalse();
    }
    
    [Fact]
    public void IsDirty_When_Adding_Element()
    {
        var sut = CreateSut();
        var element = CreateElement();
        
        sut.Add(element);

        sut.IsDirty.Should().BeTrue();
    }
    
    [Fact]
    public void NotDirty_When_Adding_Element_And_AcceptChanges()
    {
        var sut = CreateSut();
        var element = CreateElement();
        sut.Add(element);
        
        sut.AcceptChanges();

        sut.IsDirty.Should().BeFalse();
    }
}