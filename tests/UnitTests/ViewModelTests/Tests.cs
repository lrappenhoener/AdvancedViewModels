using AdvancedViewModels.UnitTests.Common;
using FluentAssertions;
using Xunit;

namespace AdvancedViewModels.UnitTests.ViewModelTests;

#nullable disable

public abstract class Tests
{
    protected abstract SampleViewModel CreateSut();

    protected abstract SampleViewModel CreateComplex();

    #region CanSave

    [Fact]
    public void CanSave_Is_False_When_Valid_Instance_Not_Dirty()
    {
        var sut = CreateSut();

        sut.CanSave.Should().BeFalse();
    }

    [Fact]
    public void CanSave_Is_False_When_Invalid_Dirty_Instance()
    {
        var sut = CreateSut();

        sut.SomeInteger = -1;

        sut.CanSave.Should().BeFalse();
    }

    [Fact]
    public void CanSave_PropertyChanged_Fires_When_Invalid_Instance()
    {
        var sut = CreateSut();
        var invoked = false;
        sut.PropertyChanged += (o, e) =>
        {
            if (e.PropertyName != nameof(sut.CanSave))
                return;
            invoked = true;
        };

        sut.SomeInteger = -1;

        invoked.Should().BeTrue();
    }

    [Fact]
    public void CanSave_PropertyChanged_Fires_When_Changing_Invalid_To_Valid_Instance()
    {
        var sut = CreateSut();
        sut.SomeInteger = -1;
        var invoked = false;
        sut.PropertyChanged += (o, e) =>
        {
            if (e.PropertyName != nameof(sut.CanSave))
                return;
            invoked = true;
        };

        sut.SomeInteger = 42;

        invoked.Should().BeTrue();
    }

    [Fact]
    public void CanSave_Is_True_When_Valid_Dirty_Instance()
    {
        var sut = CreateSut();

        sut.SomeInteger = 88;

        sut.CanSave.Should().BeTrue();
    }

    #endregion

    #region HasErrors

    [Fact]
    public void HasErrors_PropertyChanged_Fires_When_Property_Becomes_Invalid()
    {
        var sut = CreateSut();
        var invoked = false;
        sut.PropertyChanged += (o, e) =>
        {
            if (e.PropertyName != nameof(sut.HasErrors)) return;
            invoked = true;
        };

        sut.SomeInteger = -1;

        invoked.Should().BeTrue();
    }

    [Fact]
    public void HasErrors_PropertyChanged_Fires_When_Property_Becomes_Valid()
    {
        var sut = CreateSut();
        var invoked = false;
        sut.SomeInteger = -1;
        sut.PropertyChanged += (o, e) =>
        {
            if (e.PropertyName != nameof(sut.HasErrors)) return;
            invoked = true;
        };

        sut.SomeInteger = 42;

        invoked.Should().BeTrue();
    }

    [Fact]
    public void HasErrors_Is_False_When_Valid_Instance()
    {
        var sut = CreateSut();

        sut.HasErrors.Should().BeFalse();
    }

    [Fact]
    public void HasErrors_Is_True_When_Invalid_Instance()
    {
        var sut = CreateSut();

        sut.SomeInteger = -1;

        sut.HasErrors.Should().BeTrue();
    }

    [Fact]
    public void HasErrors_Is_True_When_Invalid_Complex_Property()
    {
        var sut = CreateSut();

        sut.SomeComplex.SomeInteger = -1;

        sut.HasErrors.Should().BeTrue();
    }

    [Fact]
    public void HasErrors_Is_False_When_Changing_Invalid_Instance_To_Valid()
    {
        var sut = CreateSut();
        sut.SomeInteger = -1;

        sut.SomeInteger = 42;

        sut.HasErrors.Should().BeFalse();
    }

    #endregion

    #region GetErrors

    [Fact]
    public void GetErrors_Successful_Returns_Invalid_Property_Errors()
    {
        var sut = CreateSut();
        sut.SomeInteger = -1;

        var errors = sut.GetErrors(nameof(sut.SomeInteger));

        var count = 0;
        foreach (var error in errors)
        {
            Assert.True((string) error == "SomeInteger < 0");
            count++;
        }

        count.Should().Be(1);
    }

    [Fact]
    public void GetErrors_Successful_Returns_Empty_List()
    {
        var sut = CreateSut();

        var errors = sut.GetErrors(nameof(sut.SomeInteger));

        var count = 0;
        foreach (var error in errors)
            count++;

        count.Should().Be(0);
    }

    #endregion

    #region ErrorsChanged

    [Fact]
    public void ErrorsChanged_Fires_When_Property_Becomes_Invalid()
    {
        var sut = CreateSut();
        var invoked = false;
        sut.ErrorsChanged += (o, e) =>
        {
            if (e.PropertyName != nameof(sut.SomeInteger)) return;
            invoked = true;
        };

        sut.SomeInteger = -1;

        invoked.Should().BeTrue();
    }

    [Fact]
    public void ErrorsChanged_Not_Fires_For_Already_Existing_Property_Error()
    {
        var sut = CreateSut();
        var invoked = false;
        sut.SomeInteger = -1;
        sut.ErrorsChanged += (o, e) =>
        {
            if (e.PropertyName != nameof(sut.SomeInteger)) return;
            invoked = true;
        };

        sut.SomeInteger = -2;

        invoked.Should().BeFalse();
    }

    [Fact]
    public void ErrorsChanged_Fires_When_Invalid_Property_Becomes_Valid()
    {
        var sut = CreateSut();
        sut.SomeInteger = -1;
        var invoked = false;
        sut.ErrorsChanged += (o, e) =>
        {
            if (e.PropertyName != nameof(sut.SomeInteger)) return;
            invoked = true;
        };

        sut.SomeInteger = 42;

        invoked.Should().BeTrue();
    }

    #endregion

    #region IsDirty

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

    #endregion

    #region PropertyChanged

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

    #endregion

    #region Get

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

    #endregion

    #region RejectChanges

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

    #endregion

    #region Set

    [Fact]
    public void Setting_Property_Multiple_Times_Does_Not_Throw()
    {
        var sut = CreateSut();
        sut.SomeReference = new object();

        var exception = Record.Exception(() => sut.SomeReference = new object());

        Assert.Null(exception);
    }

    #endregion

    #region Post(Reject) Accept(Reject)Changes

    [Fact]
    public void AcceptChanges_Successful_Invokes_PreAcceptChanges_Before_PostAcceptChanges()
    {
        var sut = CreateSut();
        sut.SomeInteger++;

        sut.AcceptChanges();

        sut.PreAcceptChangesInvoked.Should().BeTrue();
        sut.PostAcceptChangesInvoked.Should().BeTrue();
        sut.PostAcceptChangesInvokedAt.Should().BeAfter(sut.PreAcceptChangesInvokedAt);
    }

    [Fact]
    public void RejectChanges_Successful_Invokes_PreRejectChanges_Before_PostRejectChanges()
    {
        var sut = CreateSut();
        sut.SomeInteger++;

        sut.RejectChanges();

        sut.PreRejectChangesInvoked.Should().BeTrue();
        sut.PostRejectChangesInvoked.Should().BeTrue();
        sut.PostRejectChangesInvokedAt.Should().BeAfter(sut.PreRejectChangesInvokedAt);
    }

    #endregion
}