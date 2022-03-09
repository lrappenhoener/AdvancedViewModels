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
    public void IsDirty_When_Replacing_Element()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var element = CreateElement();
        
        sut[6] = element;

        sut.IsDirty.Should().BeTrue();
    }
    
    [Fact]
    public void PropertyChanged_Event_Fires_IsDirty_When_Adding_Element()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var element = CreateElement();
        var invoked = false;
        sut.PropertyChanged += (o, e) =>
        {
            if (e.PropertyName == nameof(sut.IsDirty))
                invoked = true;
        };
        
        sut.Add(element);

        invoked.Should().BeTrue();
    }
    
    [Fact]
    public void PropertyChanged_Event_Fires_IsDirty_When_Removing_Element()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var element = elements.ElementAt(5);
        var invoked = false;
        sut.PropertyChanged += (o, e) =>
        {
            if (e.PropertyName == nameof(sut.IsDirty))
                invoked = true;
        };
        
        sut.Remove(element);

        invoked.Should().BeTrue();
    }
    
    [Fact]
    public void PropertyChanged_Event_Fires_IsDirty_When_RemoveAt_Element()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var invoked = false;
        sut.PropertyChanged += (o, e) =>
        {
            if (e.PropertyName == nameof(sut.IsDirty))
                invoked = true;
        };
        
        sut.RemoveAt(5);

        invoked.Should().BeTrue();
    }
    
    [Fact]
    public void PropertyChanged_Event_Fires_IsDirty_When_Inserting_Element()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var element = CreateElement();
        var invoked = false;
        sut.PropertyChanged += (o, e) =>
        {
            if (e.PropertyName == nameof(sut.IsDirty))
                invoked = true;
        };
        
        sut.Insert(5, element);

        invoked.Should().BeTrue();
    }
    
    [Fact]
    public void PropertyChanged_Event_Fires_IsDirty_When_Replacing_Element()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var element = CreateElement();
        var invoked = false;
        sut.PropertyChanged += (o, e) =>
        {
            if (e.PropertyName == nameof(sut.IsDirty))
                invoked = true;
        };

        sut[5] = element;

        invoked.Should().BeTrue();
    }
    
    [Fact]
    public void PropertyChanged_Event_Fires_IsDirty_When_Element_Mutates()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var element = elements.ElementAt(5);
        var invoked = false;
        sut.PropertyChanged += (o, e) =>
        {
            if (e.PropertyName == nameof(sut.IsDirty))
                invoked = true;
        };

        element.SomeInteger++;

        invoked.Should().BeTrue();
    }
    
    [Fact]
    public void PropertyChanged_Event_Fires_IsDirty_When_New_Added_Element_Mutates()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var element = CreateElement();
        sut.Add(element);
        var invoked = false;
        sut.PropertyChanged += (o, e) =>
        {
            if (e.PropertyName == nameof(sut.IsDirty))
                invoked = true;
        };

        element.SomeInteger++;

        invoked.Should().BeTrue();
    }
    
    [Fact]
    public void PropertyChanged_Event_Fires_IsDirty_When_Inserted_Element_Mutates()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var element = CreateElement();
        sut.Insert(5, element);
        var invoked = false;
        sut.PropertyChanged += (o, e) =>
        {
            if (e.PropertyName == nameof(sut.IsDirty))
                invoked = true;
        };

        element.SomeInteger++;

        invoked.Should().BeTrue();
    }
    
    [Fact]
    public void PropertyChanged_Event_Fires_IsDirty_Once_When_Replaced_NewElement_And_OldElement_Both_Mutate()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var index = 5;
        var newElement = CreateElement();
        var replacedElement = elements.ElementAt(index);
        sut[index] = newElement;
        var timesInvoked = 0;
        sut.PropertyChanged += (o, e) =>
        {
            if (e.PropertyName == nameof(sut.IsDirty))
                timesInvoked++;
        };

        newElement.SomeInteger++;
        replacedElement.SomeInteger++;

        timesInvoked.Should().Be(1);
    }
    
    [Fact]
    public void PropertyChanged_Event_Does_Not_Fire_IsDirty_When_Removed_Element_Mutates()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var oldElement = elements.ElementAt(5);
        sut.Remove(oldElement);
        var invoked = false;
        sut.PropertyChanged += (o, e) =>
        {
            if (e.PropertyName == nameof(sut.IsDirty))
                invoked = true;
        };

        oldElement.SomeInteger++;

        invoked.Should().BeFalse();
    }
    
    [Fact]
    public void PropertyChanged_Event_Does_Not_Fire_IsDirty_When_RemovedAt_Index_Element_Mutates()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var index = 5;
        var oldElement = elements.ElementAt(index);
        sut.RemoveAt(index);
        var invoked = false;
        sut.PropertyChanged += (o, e) =>
        {
            if (e.PropertyName == nameof(sut.IsDirty))
                invoked = true;
        };

        oldElement.SomeInteger++;

        invoked.Should().BeFalse();
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

    [Fact]
    public void NotifyCollectionChanged_Event_Fires_When_Adding_Element()
    {
        var sut = CreateSut();
        var element = CreateElement();
        var invoked = false;
        sut.CollectionChanged += (o, e) =>
        {
            if (e.NewItems.Contains(element))
                invoked = true;
        };
        
        sut.Add(element);

        invoked.Should().BeTrue();
    }
    
    [Fact]
    public void NotifyCollectionChanged_Event_Fires_When_Removing_Element()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var element = elements.ElementAt(5);
        var invoked = false;
        sut.CollectionChanged += (o, e) =>
        {
            if (e.OldItems.Contains(element))
                invoked = true;
        };
        
        sut.Remove(element);

        invoked.Should().BeTrue();
    }
    
    [Fact]
    public void NotifyCollectionChanged_Event_Fires_When_Replacing_Element()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var oldElement = elements.ElementAt(5);
        var newElement = CreateElement();
        var invoked = false;
        sut.CollectionChanged += (o, e) =>
        {
            if (e.OldItems.Contains(oldElement) && e.NewItems.Contains(newElement))
                invoked = true;
        };
        
        sut[5] = newElement;

        invoked.Should().BeTrue();
    }

    [Fact]
    public void NotifyCollectionChanged_Event_Fires_When_Reject_Changes()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        Add_Insert_Replace_Remove_Elements(sut);
        var invoked = false;
        sut.CollectionChanged += (o, e) =>
        {
            invoked = true;
        };
        
        sut.RejectChanges();

        invoked.Should().BeTrue();
    }
    
    private void Add_Insert_Replace_Remove_Elements(SyncComplexCollection<SampleBaseSyncWpfForm> sut)
    {
        sut[3] = CreateElement();
        sut.Add(CreateElement());
        sut.RemoveAt(9);
        sut.Insert(2, CreateElement());
    }
}