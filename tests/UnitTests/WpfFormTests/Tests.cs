using FluentAssertions;
using PCC.Datastructures.CSharp.BaseViewModel.UnitTests.Common;
using Xunit;

namespace PCC.Datastructures.CSharp.BaseViewModel.UnitTests.WpfFormTests;

#nullable disable

public abstract class Tests
{
    protected abstract SampleBaseSyncViewModel CreateSut();
    
    protected abstract SampleBaseSyncViewModel CreateComplex();
    
    [Fact]
    public void IsDirty_When_Value_Property_Changed()
    {
        var sut = CreateSut();

        sut.SomeInteger = 2022;

        sut.IsDirty.Should().BeTrue();
    }
    
    [Fact]
    public void IsDirty_When_Complex_Property_Mutated()
    {
        var sut = CreateSut();

        sut.SomeComplex.SomeInteger = 2022;

        sut.IsDirty.Should().BeTrue();
    }
    [Fact]
    public void PropertyChanged_Fires_When_Value_Property_Changed()
    {
        var sut = CreateSut();
        var invoked = false;
        sut.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName != nameof(sut.SomeInteger))
                return;
            invoked = true;
        };

        sut.SomeInteger = 2022;

        invoked.Should().BeTrue();
    }
    
    [Fact]
    public void PropertyChanged_Fires_When_Complex_Property_Mutates()
    {
        var sut = CreateSut();
        var invoked = false;
        sut.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName != nameof(sut.SomeComplex))
                return;
            invoked = true;
        };

        sut.SomeComplex.SomeInteger = 2022;

        invoked.Should().BeTrue();
    }
    
    [Fact]
    public void PropertyChanged_Does_Not_Fire_When_Current_Complex_Property_Null_And_Previous_Complex_Property_Mutates()
    {
        var sut = CreateSut();
        var invoked = false;
        var old = sut.SomeComplex;
        sut.SomeComplex = null;
        sut.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName != nameof(sut.SomeComplex))
                return;
            invoked = true;
        };

        old.SomeInteger = 2022;

        invoked.Should().BeFalse();
    }
    
    [Fact]
    public void PropertyChanged_Fires_When_Updated_Complex_Property_Mutates()
    {
        var sut = CreateSut();
        sut.SomeComplex = CreateComplex();
        var invoked = false;
        sut.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName != nameof(sut.SomeComplex))
                return;
            invoked = true;
        };

        sut.SomeComplex.SomeInteger = 2022;

        invoked.Should().BeTrue();
    }
    
    [Fact]
    public void PropertyChanged_Fires_When_Updated_Complex_Property_Mutates_And_RejectChanges()
    {
        var sut = CreateSut();
        sut.SomeComplex = CreateComplex();
        var invoked = false;
        sut.SomeComplex.SomeInteger = 2022;
        sut.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName != nameof(sut.SomeComplex))
                return;
            invoked = true;
        };

        sut.RejectChanges();

        invoked.Should().BeTrue();
    }
    
    [Fact]
    public void PropertyChanged_Fires_When_Changed_Value_Property_And_RejectChanges()
    {
        var sut = CreateSut();
        var invoked = false;
        sut.SomeInteger = 2022;
        sut.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName != nameof(sut.SomeInteger))
                return;
            invoked = true;
        };

        sut.RejectChanges();

        invoked.Should().BeTrue();
    }
    
    [Fact]
    public void PropertyChanged_Fires_IsDirty_When_Changed_Value_Property_And_RejectChanges()
    {
        var sut = CreateSut();
        var invoked = false;
        sut.SomeInteger = 2022;
        sut.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName != nameof(sut.IsDirty))
                return;
            invoked = true;
        };

        sut.RejectChanges();

        invoked.Should().BeTrue();
    }
    
    [Fact]
    public void PropertyChanged_Fires_Only_One_Time_When_Updated_And_Old_Complex_Property_Both_Mutate()
    {
        var sut = CreateSut();
        var old = sut.SomeComplex;
        sut.SomeComplex = CreateComplex();
        var invoked = false;
        sut.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName != nameof(sut.SomeComplex))
                return;
            invoked = true;
        };

        old.SomeInteger = 2022;

        invoked.Should().BeFalse();
    }
    
    [Fact]
    public void PropertyChanged_Fires_When_Reference_Property_Changed()
    {
        var sut = CreateSut();
        var invoked = false;
        sut.PropertyChanged += (_, e) =>
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
        var sut = CreateSut();

        sut.SomeInteger = 2022;
        sut.AcceptChanges();

        sut.IsDirty.Should().BeFalse();
    }
    
    [Fact]
    public void IsDirty_Is_False_When_Value_Property_Changed_And_Rejected()
    {
        var sut = CreateSut();

        sut.SomeInteger = 2022;
        sut.RejectChanges();

        sut.IsDirty.Should().BeFalse();
    }
    
    [Fact]
    public void IsDirty_Is_False_When_Reference_Property_Changed_And_Rejected()
    {
        var sut = CreateSut();

        sut.SomeReference = new object();
        sut.RejectChanges();

        sut.IsDirty.Should().BeFalse();
    }
    
    [Fact]  
    public void IsDirty_Is_False_Initially()
    {
        var sut = CreateSut();

        sut.IsDirty.Should().BeFalse();
    }
    
    [Fact]
    public void IsDirty_When_Reference_Property_Changed()
    {
        var sut = CreateSut();

        sut.SomeReference = new object();

        sut.IsDirty.Should().BeTrue();
    }
    
    [Fact]
    public void Get_Correct_Value_Property()
    {
        var sut = CreateSut();
        var expected = 77;

        sut.SomeInteger = expected;
        
        sut.SomeInteger.Should().Be(expected);
    }
    
    [Fact]
    public void Get_Correct_Reference_Property()
    {
        var sut = CreateSut();
        var expected = new object();

        sut.SomeReference = expected;
        
        sut.SomeReference.Should().Be(expected);
    }
    
    [Fact]
    public void Rejected_Changed_Complex_Property_Successful_Reset_Changes()
    {
        var sut = CreateSut();
        var expectedValue = sut.SomeComplex.SomeInteger;
        var expectedReference = sut.SomeComplex.SomeReference;

        sut.SomeComplex.SomeInteger = 2022;
        sut.SomeComplex.SomeReference = new object();
        sut.RejectChanges();

        sut.SomeComplex.SomeInteger.Should().Be(expectedValue);
        sut.SomeComplex.SomeReference.Should().Be(expectedReference);
    }
    
    [Fact]
    public void Rejected_Changed_Reference_Property_Successful_Reset_Changes()
    {
        var sut = CreateSut();
        var expected = sut.SomeReference;

        sut.SomeReference = new object();
        sut.RejectChanges();

        sut.SomeReference.Should().Be(expected);
    }
    
    [Fact]
    public void Rejected_Changed_Value_Property_Successful_Reset_Changes()
    {
        var sut = CreateSut();
        var expected = sut.SomeInteger;

        sut.SomeInteger = 2022;
        sut.RejectChanges();

        sut.SomeInteger.Should().Be(expected);
    }

    [Fact]
    public void Get_On_Uninitialized_Complex_Property_Does_Not_Throw()
    {
        var sut = CreateSut();

        var result = Record.Exception(() => sut.NullComplex);
        
        Assert.Null(result);
    }
    
    [Fact]
    public void Get_On_Uninitialized_Value_Property_Does_Not_Throw()
    {
        var sut = CreateSut();

        var result = Record.Exception(() => sut.UninitializedInteger);
        
        Assert.Null(result);
    }
    
    [Fact]
    public void Setting_Property_Multiple_Times_Does_Not_Throw()
    {
        var sut = CreateSut();
        sut.SomeReference = new object();
        
        var exception = Record.Exception(() => sut.SomeReference = new object());
        
        Assert.Null(exception);
    }
}