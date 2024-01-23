namespace SFA.DAS.FAA.Web.Models;

public class SignedOutViewModel
{
    private readonly string _environmentPart;
    private readonly string _domainPart;

    public SignedOutViewModel(string environment)
    {
        _environmentPart = environment.ToLower() == "prd" ? "findapprenticeship" : $"{environment.ToLower()}-findapprenticeship";
        _domainPart = environment.ToLower() == "prd" ? "education" : "apprenticeships.education";
    }
    public string ServiceLink => $"https://{_environmentPart}.{_domainPart}.gov.uk";
}