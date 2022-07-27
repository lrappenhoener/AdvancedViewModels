using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using PCC.Libraries.AdvancedViewModels.UnitTests.Common;
using Xunit;

namespace PCC.Libraries.AdvancedViewModels.UnitTests.SyncWrapperCollectionTests;

public class SyncWrapperCollectionTests : Tests
{
    [Fact]
    public void Add_Wrapper_And_AcceptChanges_Correctly_Synchronizes_Models()
    {
        var expectedModel = new SampleBackingObject(1);
        var expectedWrapper = new WrapperSampleBackingObject(expectedModel, 1);
        var (models, wrappers) = CreateItems();
        var sut = new SyncWrapperCollection<WrapperSampleBackingObject, SampleBackingObject>(models, wrappers);

        sut.Add(expectedWrapper);
        sut.AcceptChanges();

        models.Last().Should().Be(expectedModel);
    }

    [Fact]
    public void Add_Wrapper_And_Not_AcceptChanges_Not_Synchronizes_Models()
    {
        var expectedModel = new SampleBackingObject(1);
        var expectedWrapper = new WrapperSampleBackingObject(expectedModel, 1);
        var (models, wrappers) = CreateItems();
        var lastWrapper = wrappers.Last();
        var sut = new SyncWrapperCollection<WrapperSampleBackingObject, SampleBackingObject>(models, wrappers);

        sut.Add(expectedWrapper);

        models.Last().Should().Be(lastWrapper.Model);
    }

    [Fact]
    public void Remove_Wrapper_And_AcceptChanges_Correctly_Synchronizes_Models()
    {
        var (models, wrappers) = CreateItems();
        var removeWrapper = wrappers.First();
        var sut = new SyncWrapperCollection<WrapperSampleBackingObject, SampleBackingObject>(models, wrappers);

        sut.Remove(removeWrapper);
        sut.AcceptChanges();

        models.First().Should().NotBe(removeWrapper.Model);
    }

    [Fact]
    public void Remove_Wrapper_And_Not_AcceptChanges_Not_Synchronizes_Models()
    {
        var (models, wrappers) = CreateItems();
        var removeWrapper = wrappers.First();
        var sut = new SyncWrapperCollection<WrapperSampleBackingObject, SampleBackingObject>(models, wrappers);

        sut.Remove(removeWrapper);

        models.First().Should().Be(removeWrapper.Model);
    }

    [Fact]
    public void Move_Wrapper_And_AcceptChanges_Correctly_Synchronizes_Models()
    {
        var (models, wrappers) = CreateItems();
        var firstWrapper = wrappers.First();
        var lastWrapper = wrappers.Last();
        var sut = new SyncWrapperCollection<WrapperSampleBackingObject, SampleBackingObject>(models, wrappers);

        sut[0] = lastWrapper;
        sut[wrappers.Count - 1] = firstWrapper;
        sut.AcceptChanges();

        models.First().Should().Be(lastWrapper.Model);
        models.Last().Should().Be(firstWrapper.Model);
    }

    [Fact]
    public void Move_Wrapper_And_Not_AcceptChanges_Not_Synchronizes_Models()
    {
        var (models, wrappers) = CreateItems();
        var firstWrapper = wrappers.First();
        var lastWrapper = wrappers.Last();
        var sut = new SyncWrapperCollection<WrapperSampleBackingObject, SampleBackingObject>(models, wrappers);

        sut[0] = lastWrapper;
        sut[wrappers.Count - 1] = firstWrapper;

        models.First().Should().Be(firstWrapper.Model);
        models.Last().Should().Be(lastWrapper.Model);
    }

    protected override ISyncComplexCollection<WrapperSampleBackingObject> CreateSut()
    {
        var models = new List<SampleBackingObject>
        {
            new(1),
            new(1),
            new(1),
            new(1)
        };
        var wrappers = new List<WrapperSampleBackingObject>();
        return new SyncWrapperCollection<WrapperSampleBackingObject, SampleBackingObject>(models, wrappers);
    }

    protected override ISyncComplexCollection<WrapperSampleBackingObject> CreateSut(
        List<WrapperSampleBackingObject> wrappers)
    {
        var models = new List<SampleBackingObject>();
        return new SyncWrapperCollection<WrapperSampleBackingObject, SampleBackingObject>(models, wrappers);
    }

    private (List<SampleBackingObject>, List<WrapperSampleBackingObject>) CreateItems()
    {
        var models = new List<SampleBackingObject>
        {
            new(1),
            new(1),
            new(1),
            new(1)
        };
        var wrappers = models.Select(m => new WrapperSampleBackingObject(m, 1)).ToList();
        return (models, wrappers);
    }
}

public class WrapperSampleBackingObject : Wrapper<SampleBackingObject>
{
    public WrapperSampleBackingObject(SampleBackingObject model, int depth) : base(model)
    {
        if (depth > 2) return;
        SomeComplex = new WrapperSampleBackingObject(model.SomeComplex, depth + 1);
    }

    public int SomeInteger
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int UninitializedInteger
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public object SomeReference
    {
        get => GetProperty<object>();
        set => SetProperty(value);
    }

    public WrapperSampleBackingObject SomeComplex
    {
        get => GetComplexProperty<WrapperSampleBackingObject>();
        set => SetComplexProperty(value);
    }

    public WrapperSampleBackingObject NullComplex
    {
        get => GetComplexProperty<WrapperSampleBackingObject>();
        set => SetComplexProperty(value);
    }

    public SyncComplexCollection<WrapperSampleBackingObject> ComplexCollection
    {
        get => GetComplexProperty<SyncComplexCollection<WrapperSampleBackingObject>>();
        set => SetComplexProperty(value);
    }

    public bool ValidationCalled { get; private set; }

    protected override IEnumerable<FailedPropertyValidation> ValidateImpl()
    {
        ValidationCalled = true;
        var results = new List<FailedPropertyValidation>();

        if (SomeInteger < 0)
            results.Add(new FailedPropertyValidation(nameof(SomeInteger), new List<string>
            {
                "SomeInteger < 0"
            }));

        return results;
    }
}