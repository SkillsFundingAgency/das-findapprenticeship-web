namespace SFA.DAS.FAA.Web.Models;

public class ViewModelBase
{
    public Dictionary<string, string?> ErrorDictionary { get; set; }

    protected ViewModelBase()
    {
        ErrorDictionary = new Dictionary<string, string?>();
    }

    protected string GetErrorMessage(string propertyName)
    {
        return (ErrorDictionary.Any() && ErrorDictionary.ContainsKey(propertyName) ? ErrorDictionary[propertyName] : "") ?? string.Empty;
    }
}