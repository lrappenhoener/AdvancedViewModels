using FluentAssertions;
using PCC.Libraries.AdvancedViewModels.UnitTests.Common;
using Xunit;

namespace PCC.Libraries.AdvancedViewModels.UnitTests.SyncViewModelTests;

public abstract class Tests
{
    protected abstract SampleViewModel CreateSut(SampleBackingObject wrappedObject);
    protected abstract SampleBackingObject CreateWrappedObject();

    #region Set

    [Fact]
    public void Changed_Value_Property_Without_Accepting_Does_Not_Update_Wrapped_Object()
    {
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);
        var original = wrappedObject.SomeInteger;

        sut.SomeInteger++;

        wrappedObject.SomeInteger.Should().Be(original);
    }

    #endregion

    #region AcceptChanges

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
        sut.SomeComplex = new SampleViewModel(childWrappedObject, 0);
        var expected = sut.SomeComplex.SomeInteger + 1;

        sut.SomeComplex.SomeInteger++;
        sut.AcceptChanges();

        childWrappedObject.SomeInteger.Should().Be(expected);
    }

    #endregion
}