using System.Configuration;
using FluentAssertions;
using Xunit;

namespace PCC.Datastructures.CSharp.WpfForm.UnitTests.SyncWpfFormTests;

public abstract class Tests
{
    protected abstract SampleBaseSyncWpfForm CreateSut(object wrappedObject);
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

        sut.SomeString = "zen";
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

        sut.SomeString = "zen";

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
        var expected = "zen";

        sut.SomeString = expected;
        
        sut.SomeString.Should().Be(expected);
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
        var expected = "zen";

        sut.SomeString = expected;
        sut.AcceptChanges();

        wrappedObject.SomeString.Should().Be(expected);
    }
}