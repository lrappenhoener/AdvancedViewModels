namespace PCC.Libraries.AdvancedViewModels;

public class Wrapper<TModel>
{
    public Wrapper(TModel model)
    {
        Model = model;
    }

    public TModel Model { get; }
}