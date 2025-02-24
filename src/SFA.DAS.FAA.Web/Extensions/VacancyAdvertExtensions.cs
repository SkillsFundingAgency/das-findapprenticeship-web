using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Web.Extensions;

public static class VacancyAdvertExtensions
{
    // TODO: this should use the AvaibleWhere property once it has been surfaced
    public static bool IsRecruitingNationally(this VacancyAdvert vacancyAdvert) => 
        string.IsNullOrWhiteSpace(vacancyAdvert.Postcode) && !string.IsNullOrWhiteSpace(vacancyAdvert.EmploymentLocationInformation);
    
    public static string GetLocationDescription(this VacancyAdvert vacancyAdvert)
    {
        if (vacancyAdvert.IsRecruitingNationally())
        {
            return "Recruiting nationally";
        }
        
        List<string?> lines = [
            vacancyAdvert.AddressLine4,
            vacancyAdvert.AddressLine3,
            vacancyAdvert.AddressLine2,
            vacancyAdvert.AddressLine1,
        ];

        var city = lines.FirstOrDefault(x => !string.IsNullOrEmpty(x));
        var location = string.IsNullOrEmpty(city)
            ? $"{vacancyAdvert.Postcode}"
            : $"{city} ({vacancyAdvert.Postcode})";
    
        return vacancyAdvert.OtherAddresses switch
        {
            { Count: 1 } => $"{location} and 1 other location",
            { Count: >1 } => $"{location} and {vacancyAdvert.OtherAddresses.Count} other locations",
            _ => location,
        };
    }
}