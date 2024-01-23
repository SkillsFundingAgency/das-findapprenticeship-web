namespace SFA.DAS.FAA.Web.Models;

public class ViewModelBase
{
    public bool Valid => !ErrorDictionary.Any();
    public Dictionary<string, string?> ErrorDictionary { get; set; }

    protected ViewModelBase()
    {
        ErrorDictionary = new Dictionary<string, string?>();
    }

    protected string GetErrorMessage(string propertyName)
    {
        return (ErrorDictionary.Any() && ErrorDictionary.ContainsKey(propertyName) ? ErrorDictionary[propertyName] : "") ?? string.Empty;
    }
    public void AddErrorsFromDictionary(Dictionary<string, string> errorDictionary)
    {
        foreach (var property in errorDictionary.Keys)
        {
            if (ErrorDictionary.ContainsKey(property)) continue;
            if (!errorDictionary[property].Any()) continue;

            var error = errorDictionary[property];
            ErrorDictionary.Add(property, error);
        }
    }
}