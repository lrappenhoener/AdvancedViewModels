namespace PCC.Libraries.AdvancedViewModels;

public abstract class Wrapper<TModel> : BaseViewModel
{
    protected Wrapper(TModel model) : base(model)
    {
        Model = model;
    }

    public TModel Model { get; }
}