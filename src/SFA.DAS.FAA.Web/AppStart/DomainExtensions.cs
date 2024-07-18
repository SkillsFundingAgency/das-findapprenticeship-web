namespace SFA.DAS.FAA.Web.AppStart;

public static class DomainExtensions
{
    public static string GetDomain(string environment)
    {
        if (environment.ToLower() == "local")
        {
            return "";
        }

        if (environment.ToLower() == "prd")
        {
            return "findapprenticeship.service.gov.uk";
        }

        var environmentPart = environment.ToLower() == "prd" ? "" : $"{environment.ToLower()}-";
        return $"{environmentPart}findapprenticeship.apprenticeships.education.gov.uk";
    }
}   