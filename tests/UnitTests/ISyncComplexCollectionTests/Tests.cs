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
    public void IsDirty_When_Removing_Element()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var element = elements.ElementAt(7);
        
        sut.Remove(element);

        sut.IsDirty.Should().BeTrue();
    }
    
    [Fact]
    public void IsDirty_When_Removing_At_Index()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        
        sut.RemoveAt(7);

        sut.IsDirty.Should().BeTrue();
    }
    
    [Fact]
    public void IsDirty_When_Insert_Element()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var element = CreateElement();
        
        sut.Insert(6, element);

        sut.IsDirty.Should().BeTrue();
    }
    
    [Fact]
    public void Successful_Insert_Element()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var element = CreateElement();
        
        sut.Insert(5, element);

        for (int i = 0; i < 5; i++)
        {
            var expected = elements.ElementAt(i);
            var returned = sut.ElementAt(i);
            returned.Should().Be(expected);
        }

        sut.ElementAt(5).Should().Be(element);
        
        for (int i = 6; i < 10; i++)
        {
            var expected = elements.ElementAt(i - 1);
            var returned = sut.ElementAt(i);
            returned.Should().Be(expected);
        }
    }
    
    [Fact]
    public void NotDirty_When_AcceptChanges()
    {
        var sut = CreateSut();
        var element = CreateElement();
        sut.Add(element);
        
        sut.AcceptChanges();

        sut.IsDirty.Should().BeFalse();
    }
    
    [Fact]
    public void NotDirty_When_RejectChanges()
    {
        var sut = CreateSut();
        var element = CreateElement();
        sut.Add(element);
        
        sut.RejectChanges();

        sut.IsDirty.Should().BeFalse();
    }

    [Fact]
    public void Returns_Correct_Initial_Elements()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);

        for (int i = 0; i < 10; i++)
        {
            var expected = elements.ElementAt(i);
            var returned = sut.ElementAt(i);
            returned.Should().Be(expected);    
        }
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
    public void Returns_Correct_Elements_After_RejectChanges()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        Add_Insert_Replace_Remove_Elements(sut);

        sut.RejectChanges();

        for (int i = 0; i < 10; i++)
        {
            var expected = sut[i];
            var returned = sut.ElementAt(i);
            returned.Should().Be(expected);
        }
    }

    [Fact]
    public void Returns_Correct_Elements_After_Changing_Elements_And_AcceptChanges_And_RejectChanges()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        Add_Insert_Replace_Remove_Elements(sut);
        var expectedElements = new List<SampleBaseSyncWpfForm>(sut); 
        
        sut.AcceptChanges();
        sut.RejectChanges();

        for (int i = 0; i < sut.Count; i++)
        {
            var expected = expectedElements[i];
            var returned = sut.ElementAt(i);
            returned.Should().Be(expected);    
        }
    }

    [Fact]
    public void Count_Returns_Correct_Value()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);

        sut.Count.Should().Be(10);
    }
    
    [Fact]
    public void Clear_Successful_Removes_All_Elements()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        
        sut.Clear();

        sut.Any().Should().BeFalse();
    }

    [Fact]
    public void IndexOf_Returns_Correct_Index()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var expectedIndex = 6;
        var element = elements.ElementAt(expectedIndex);

        var index = sut.IndexOf(element);

        index.Should().Be(expectedIndex);
    }

    [Fact]
    public void Contains_Recognizes_Existing_Element()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var element = elements.ElementAt(6);

        var result = sut.Contains(element);

        result.Should().BeTrue();
    }
    
    [Fact]
    public void Contains_Recognizes_Not_Existing_Element()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var element = CreateElement();

        var result = sut.Contains(element);

        result.Should().BeFalse();
    }

    private void Add_Insert_Replace_Remove_Elements(SyncComplexCollection<SampleBaseSyncWpfForm> sut)
    {
        sut[3] = CreateElement();
        sut.Add(CreateElement());
        sut.RemoveAt(9);
        sut.Insert(2, CreateElement());
    }
}