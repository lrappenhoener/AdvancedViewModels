using FluentAssertions;
using PCC.Libraries.AdvancedViewModels.UnitTests.Common;
using Xunit;

namespace PCC.Libraries.AdvancedViewModels.UnitTests.WrapperTests;

public class SampleWrapperTests
{
    [Fact]
    public void Model_Set_Successful()
    {
        var model = new SampleWrapper();
        
        var wrapper = new Wrapper<SampleWrapper>(model);

        wrapper.Model.Should().Be(model);
    }
    
    [Fact]
    public void Base_Of_Type_BaseViewModel()
    {
        var model = new SampleWrapper();
        
        var wrapper = new Wrapper<SampleWrapper>(model);

        wrapper.GetType().BaseType.Should().Be(typeof(BaseViewModel));
    }
}