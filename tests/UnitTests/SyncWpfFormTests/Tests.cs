using System.Configuration;
using FluentAssertions;
using PCC.Datastructures.CSharp.WpfForm.UnitTests.Common;
using Xunit;

namespace PCC.Datastructures.CSharp.WpfForm.UnitTests.SyncWpfFormTests;

public abstract class Tests
{
    protected abstract SampleBaseSyncWpfForm CreateSut(SampleBackingObject wrappedObject);
    protected abstract SampleBackingObject CreateWrappedObject();

    [Fact]
    public void Changed_Value_Property_Without_Accepting_Does_Not_Update_Wrapped_Object()
    {
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);
        var original = wrappedObject.SomeInteger;

        sut.SomeInteger++;

        wrappedObject.SomeInteger.Should().Be(original);
    }
    
    [Fact]
    public void IsDirty_When_Value_Property_Changed()
    {
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);

        sut.SomeInteger++;

        sut.IsDirty.Should().BeTrue();
    }
    
    [Fact]
    public void IsDirty_When_Complex_Property_Mutated()
    {
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);

        sut.SomeComplex.SomeInteger++;

        sut.IsDirty.Should().BeTrue();
    }
    
    [Fact]
    public void PropertyChanged_Fires_When_Value_Property_Changed()
    {
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);
        var invoked = false;
        sut.PropertyChanged += (o, e) =>
        {
            if (e.PropertyName != nameof(sut.SomeInteger))
                return;
            invoked = true;
        };

        sut.SomeInteger++;

        invoked.Should().BeTrue();
    }
    
    [Fact]
    public void PropertyChanged_Fires_When_Complex_Property_Mutates()
    {
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);
        var invoked = false;
        sut.PropertyChanged += (o, e) =>
        {
            if (e.PropertyName != nameof(sut.SomeComplex))
                return;
            invoked = true;
        };

        sut.SomeComplex.SomeInteger++;

        invoked.Should().BeTrue();
    }
    
    [Fact]
    public void PropertyChanged_Does_Not_Fire_When_Current_Complex_Property_Null_And_Previous_Complex_Property_Mutates()
    {
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);
        var invoked = false;
        var old = sut.SomeComplex;
        sut.SomeComplex = null;
        sut.PropertyChanged += (o, e) =>
        {
            if (e.PropertyName != nameof(sut.SomeComplex))
                return;
            invoked = true;
        };

        old.SomeInteger++;

        invoked.Should().BeFalse();
    }
    
    [Fact]
    public void PropertyChanged_Fires_When_Updated_Complex_Property_Mutates()
    {
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);
        sut.SomeComplex = new SampleBaseSyncWpfForm(CreateWrappedObject(), 0);
        var invoked = false;
        sut.PropertyChanged += (o, e) =>
        {
            if (e.PropertyName != nameof(sut.SomeComplex))
                return;
            invoked = true;
        };

        sut.SomeComplex.SomeInteger++;

        invoked.Should().BeTrue();
    }
    
    [Fact]
    public void PropertyChanged_Fires_Only_One_Time_When_Updated_And_Old_Complex_Property_Both_Mutate()
    {
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);
        var old = sut.SomeComplex;
        sut.SomeComplex = new SampleBaseSyncWpfForm(CreateWrappedObject(), 0);
        var invoked = false;
        sut.PropertyChanged += (o, e) =>
        {
            if (e.PropertyName != nameof(sut.SomeComplex))
                return;
            invoked = true;
        };

        old.SomeInteger++;

        invoked.Should().BeFalse();
    }
    
    [Fact]
    public void PropertyChanged_Fires_When_Reference_Property_Changed()
    {
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);
        var invoked = false;
        sut.PropertyChanged += (o, e) =>
        {
            if (e.PropertyName != nameof(sut.SomeReference))
                return;
            invoked = true;
        };

        sut.SomeReference = new object();

        invoked.Should().BeTrue();
    }
    
    [Fact]
    public void IsDirty_Is_False_When_Value_Property_Changed_And_Accepted()
    {
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);

        sut.SomeInteger++;
        sut.AcceptChanges();

        sut.IsDirty.Should().BeFalse();
    }
    
    [Fact]
    public void IsDirty_Is_False_When_Value_Property_Changed_And_Rejected()
    {
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);

        sut.SomeInteger++;
        sut.RejectChanges();

        sut.IsDirty.Should().BeFalse();
    }
    
    [Fact]
    public void IsDirty_Is_False_When_Reference_Property_Changed_And_Rejected()
    {
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);

        sut.SomeReference = new object();
        sut.RejectChanges();

        sut.IsDirty.Should().BeFalse();
    }
    
    [Fact]  
    public void IsDirty_Is_False_Initially()
    {
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);

        sut.IsDirty.Should().BeFalse();
    }
    
    [Fact]
    public void IsDirty_When_Reference_Property_Changed()
    {
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);

        sut.SomeReference = new object();

        sut.IsDirty.Should().BeTrue();
    }

    [Fact]
    public void Get_Correct_Value_Property()
    {
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);
        var expected = 77;

        sut.SomeInteger = expected;
        
        sut.SomeInteger.Should().Be(expected);
    }
    
    [Fact]
    public void Get_Correct_Reference_Property()
    {
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);
        var expected = new object();

        sut.SomeReference = expected;
        
        sut.SomeReference.Should().Be(expected);
    }
    
    [Fact]
    public void Accepted_Changed_Value_Property_Successful_Updates_Wrapped_Object()
    {
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);
        var expected = 77;

        sut.SomeInteger = expected;
        sut.AcceptChanges();

        wrappedObject.SomeInteger.Should().Be(expected);
    }
    
    [Fact]
    public void Accepted_Changed_Reference_Property_Successful_Updates_Wrapped_Object()
    {
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);
        var expected = new object();

        sut.SomeReference = expected;
        sut.AcceptChanges();

        wrappedObject.SomeReference.Should().Be(expected);
    }
    
    [Fact]
    public void Accepted_Changed_Complex_Property_Successful_Updates_Wrapped_Object()
    {
        var childWrappedObject = CreateWrappedObject();
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);
        sut.SomeComplex = new SampleBaseSyncWpfForm(childWrappedObject, 0);
        var expected = sut.SomeComplex.SomeInteger + 1;

        sut.SomeComplex.SomeInteger++;
        sut.AcceptChanges();

        childWrappedObject.SomeInteger.Should().Be(expected);
    }
    
    [Fact]
    public void Rejected_Changed_Complex_Property_Successful_Reset_Changes()
    {
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);
        var expectedValue = sut.SomeComplex.SomeInteger;
        var expectedReference = sut.SomeComplex.SomeReference;

        sut.SomeComplex.SomeInteger++;
        sut.SomeComplex.SomeReference = new object();
        sut.RejectChanges();

        sut.SomeComplex.SomeInteger.Should().Be(expectedValue);
        sut.SomeComplex.SomeReference.Should().Be(expectedReference);
    }
    
    [Fact]
    public void Rejected_Changed_Reference_Property_Successful_Reset_Changes()
    {
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);
        var expected = sut.SomeReference;

        sut.SomeReference = new object();
        sut.RejectChanges();

        sut.SomeReference.Should().Be(expected);
    }
    
    [Fact]
    public void Rejected_Changed_Value_Property_Successful_Reset_Changes()
    {
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);
        var expected = sut.SomeInteger;

        sut.SomeInteger++;
        sut.RejectChanges();

        sut.SomeInteger.Should().Be(expected);
    }
}