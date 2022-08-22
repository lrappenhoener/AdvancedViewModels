using AdvancedViewModels.UnitTests.Common;
using FluentAssertions;
using Xunit;

namespace AdvancedViewModels.UnitTests.WrapperTests;

public abstract class Tests
{
    protected abstract SampleWrapper CreateSut(SampleBackingObject model);
    protected abstract SampleBackingObject CreateModel();

    [Fact]
    public void Model_Set_Successful()
    {
        var model = CreateModel();

        var sut = CreateSut(model);

        sut.Model.Should().Be(model);
    }

    [Fact]
    public void Inherits_correct_type()
    {
        var model = CreateModel();

        var sut = CreateSut(model);

        (sut is Wrapper<SampleBackingObject>).Should().BeTrue();
        (sut is BaseViewModel).Should().BeTrue();
    }

    [Fact]
    public void Initializes_BaseViewModel_Successful()
    {
        var model = CreateModel();

        var sut = CreateSut(model);

        sut.SomeInteger.Should().Be(model.SomeInteger);
    }
}