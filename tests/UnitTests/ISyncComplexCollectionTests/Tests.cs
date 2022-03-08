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

    [Fact]
    public void Returns_Correct_Initial_Element()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var expected = elements.ElementAt(5);

        var returned = sut.ElementAt(5);

        returned.Should().Be(expected);
    }
    
    [Fact]
    public void Returns_Correct_Element_After_Replacing()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var expected = CreateElement();
        sut[3] = expected;

        var returned = sut.ElementAt(3);

        returned.Should().Be(expected);
    }
    
    [Fact]
    public void Returns_Correct_Element_After_Replacing_And_RejectChanges()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var expected = sut[3];
        var newElement = CreateElement();
        sut[3] = newElement;

        sut.RejectChanges();
        
        var returned = sut.ElementAt(3);
        returned.Should().Be(expected);
    }
}