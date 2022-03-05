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
    public void Accepted_Changed_Value_Property_Successful_Updates_Wrapped_Object()
    {
        var wrappedObject = CreateWrappedObject();
        var sut = CreateSut(wrappedObject);
        var expected = 77;

        sut.SomeInteger = expected;
        sut.AcceptChanges();

        wrappedObject.SomeInteger.Should().Be(expected);
    }
}

public class SampleBaseSyncWpfFormTests : Tests
{
    protected override SampleBaseSyncWpfForm CreateSut(object wrappedObject)
    {
        return new SampleBaseSyncWpfForm(wrappedObject);
    }

    protected override SampleBackingObject CreateWrappedObject()
    {
        return new SampleBackingObject();
    }
}

public class SampleBaseSyncWpfForm : BaseSyncWpfForm
{
    public SampleBaseSyncWpfForm(object wrapped) : base(wrapped)
    {
    }

    public int SomeInteger
    {
        get => 1;
        set => SetProperty(value);
    }
}