namespace AdvancedViewModels;

public abstract class Wrapper<TModel> : BaseViewModel, IComplexProperty<TModel> where TModel : class
{
    protected Wrapper(TModel model) : base(model)
    {
        Model = model;
    }

    public TModel Model { get; }
}