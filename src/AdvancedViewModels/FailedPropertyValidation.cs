namespace PCC.Libraries.AdvancedViewModels;

public class FailedPropertyValidation
{
    public FailedPropertyValidation(string propertyName, IEnumerable<string> errors)
    {
        PropertyName = propertyName;
        Errors = errors;
    }

    public string PropertyName { get; }
    public IEnumerable<string> Errors { get; }
}