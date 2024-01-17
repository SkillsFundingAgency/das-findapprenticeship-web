namespace SFA.DAS.FAA.Web.Models;

public class SignedOutViewModel
{
    private readonly string _environmentPart;
    private readonly string _domainPart;

    //todo: get correct urls
    public SignedOutViewModel(string environment)
    {
        _environmentPart = environment.ToLower() == "prd" ? "findanapprenticeship" : $"{environment.ToLower()}-eas.apprenticeships";
        _domainPart = environment.ToLower() == "prd" ? "service" : "education";
    }
    public string ServiceLink => $"https://accounts.{_environmentPart}.{_domainPart}.gov.uk";
}