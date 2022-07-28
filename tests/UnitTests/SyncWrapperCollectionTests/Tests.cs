using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using PCC.Libraries.AdvancedViewModels.UnitTests.Common;
using Xunit;

namespace PCC.Libraries.AdvancedViewModels.UnitTests.SyncWrapperCollectionTests;

public abstract class Tests
{
    protected abstract ISyncComplexCollection<WrapperSampleBackingObject> CreateSut();

    protected abstract ISyncComplexCollection<WrapperSampleBackingObject> CreateSut(
        List<WrapperSampleBackingObject> wrappers);

    protected virtual WrapperSampleBackingObject CreateElement()
    {
        return new WrapperSampleBackingObject(new SampleBackingObject(0), 1);
    }

    protected virtual List<WrapperSampleBackingObject> CreateElements(int count)
    {
        var elements = new List<WrapperSampleBackingObject>();
        for (var i = 0; i < count; i++) elements.Add(CreateElement());

        return elements;
    }

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
    public void IsDirty_When_Element_Mutated()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var element = elements.ElementAt(5);

        element.SomeInteger++;

        sut.IsDirty.Should().BeTrue();
    }

    [Fact]
    public void PropertyChanged_Event_Fires_IsDirty_When_Adding_Element()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var element = CreateElement();
        var invoked = false;
        sut.PropertyChanged += (_, e) =>
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
        sut.PropertyChanged += (_, e) =>
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
        sut.PropertyChanged += (_, e) =>
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
        sut.PropertyChanged += (_, e) =>
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
        sut.PropertyChanged += (_, e) =>
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
        sut.PropertyChanged += (_, e) =>
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
        sut.PropertyChanged += (_, e) =>
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
        sut.PropertyChanged += (_, e) =>
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
        sut.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(sut.IsDirty))
                timesInvoked++;
        };

        newElement.SomeInteger++;
        replacedElement.SomeInteger++;

        timesInvoked.Should().Be(1);
    }

    [Fact]
    public void PropertyChanged_Does_Not_Fire_When_Added_Element_Mutates_After_Being_Removed_By_RejectChanges()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var element = CreateElement();
        sut.Add(element);
        sut.RejectChanges();
        var invoked = false;
        sut.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(sut.IsDirty))
                invoked = true;
        };

        element.SomeInteger++;

        invoked.Should().BeFalse();
    }

    [Fact]
    public void PropertyChanged_Event_Does_Not_Fire_IsDirty_When_Removed_Element_Mutates()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var oldElement = elements.ElementAt(5);
        sut.Remove(oldElement);
        var invoked = false;
        sut.PropertyChanged += (_, e) =>
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
        sut.PropertyChanged += (_, e) =>
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

        for (var i = 0; i < 5; i++)
        {
            var expected = elements.ElementAt(i);
            var returned = sut.ElementAt(i);
            returned.Should().Be(expected);
        }

        sut.ElementAt(5).Should().Be(element);

        for (var i = 6; i < 10; i++)
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

        for (var i = 0; i < 10; i++)
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

        for (var i = 0; i < 10; i++)
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
        var expectedElements = new List<WrapperSampleBackingObject>(sut);

        sut.AcceptChanges();
        sut.RejectChanges();

        for (var i = 0; i < sut.Count; i++)
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
        sut.CollectionChanged += (_, e) =>
        {
            if (e.NewItems != null && e.NewItems.Contains(element))
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
        sut.CollectionChanged += (_, e) =>
        {
            if (e.OldItems != null && e.OldItems.Contains(element))
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
        sut.CollectionChanged += (_, e) =>
        {
            if (e.OldItems != null && e.OldItems.Contains(oldElement) && e.NewItems != null &&
                e.NewItems.Contains(newElement))
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
        sut.CollectionChanged += (_, _) => { invoked = true; };

        sut.RejectChanges();

        invoked.Should().BeTrue();
    }

    [Fact]
    public void ForEach_Iteration_Successful()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var index = 0;

        foreach (var element in sut)
        {
            element.Should().Be(elements.ElementAt(index));
            index++;
        }

        index.Should().Be(10);
    }

    [Fact]
    public void All_Elements_AcceptChanges_Will_Be_Invoked_When_AcceptChanges()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var mutated = new List<WrapperSampleBackingObject>
            {elements.ElementAt(0), elements.ElementAt(4), elements.ElementAt(9)};
        foreach (var element in mutated)
            element.SomeInteger++;

        sut.AcceptChanges();

        mutated.All(e => !e.IsDirty).Should().BeTrue();
    }

    [Fact]
    public void All_Elements_RejectChanges_Will_Be_Invoked_When_RejectChanges()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var mutated = new List<WrapperSampleBackingObject>
            {elements.ElementAt(0), elements.ElementAt(4), elements.ElementAt(9)};
        foreach (var element in mutated)
            element.SomeInteger++;

        sut.RejectChanges();

        mutated.All(e => !e.IsDirty).Should().BeTrue();
    }

    [Fact]
    public void HasErrors_False_When_Elements_Have_No_Errors()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);

        sut.HasErrors.Should().BeFalse();
    }

    [Fact]
    public void HasErrors_True_When_Elements_Have_Errors()
    {
        var elements = CreateElements(10);
        elements.First().SomeInteger = -42;
        var sut = CreateSut(elements);

        sut.HasErrors.Should().BeTrue();
    }

    [Fact]
    public void ErrorsChanged_Fires_When_Element_Gets_Errors()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var invoked = false;
        sut.ErrorsChanged += (o, e) => invoked = true;

        elements.First().SomeInteger = -42;

        invoked.Should().BeTrue();
    }

    [Fact]
    public void ErrorsChanged_Fires_When_New_Added_Element_Gets_Errors()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var element = CreateElement();
        var invoked = false;
        sut.ErrorsChanged += (o, e) => invoked = true;
        sut.Add(element);

        element.SomeInteger = -42;

        invoked.Should().BeTrue();
    }

    [Fact]
    public void ErrorsChanged_Not_Fires_When_Removed_Element_Gets_Errors()
    {
        var elements = CreateElements(10);
        var element = elements.First();
        var sut = CreateSut(elements);
        sut.Remove(element);
        var invoked = false;
        sut.ErrorsChanged += (o, e) => invoked = true;

        element.SomeInteger = -42;

        invoked.Should().BeFalse();
    }

    [Fact]
    public void CanSave_Initially_False()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);

        sut.CanSave.Should().BeFalse();
    }

    [Fact]
    public void CanSave_When_Added_Element_True()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var element = CreateElement();

        sut.Add(element);

        sut.CanSave.Should().BeTrue();
    }

    [Fact]
    public void CanSave_PropertyChanged_Fires_When_Adding_Element()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var element = CreateElement();
        var invoked = false;
        sut.PropertyChanged += (o, e) =>
        {
            if (e.PropertyName != nameof(sut.CanSave)) return;
            invoked = true;
        };

        sut.Add(element);

        invoked.Should().BeTrue();
    }

    [Fact]
    public void CanSave_After_AcceptChanges_False()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var element = CreateElement();

        sut.Add(element);
        sut.AcceptChanges();

        sut.CanSave.Should().BeFalse();
    }

    [Fact]
    public void CanSave_After_RejectChanges_False()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var element = CreateElement();

        sut.Add(element);
        sut.RejectChanges();

        sut.CanSave.Should().BeFalse();
    }

    [Fact]
    public void CanSave_When_Added_Element_HasErrors_False()
    {
        var elements = CreateElements(10);
        var sut = CreateSut(elements);
        var element = CreateElement();
        element.SomeInteger = -42;

        sut.Add(element);

        sut.CanSave.Should().BeFalse();
    }

    private void Add_Insert_Replace_Remove_Elements(ISyncComplexCollection<WrapperSampleBackingObject> sut)
    {
        sut[3] = CreateElement();
        sut.Add(CreateElement());
        sut.RemoveAt(9);
        sut.Insert(2, CreateElement());
    }
}