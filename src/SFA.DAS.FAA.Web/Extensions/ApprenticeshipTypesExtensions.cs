using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Web.Extensions;

public static class ApprenticeshipTypesExtensions
{
    public static List<string>? GetDisplayTexts(this List<ApprenticeshipTypes>? apprenticeshipTypes)
    {
        return apprenticeshipTypes?.Select(GetDisplayText).ToList();
    }
    
    public static string GetDisplayText(this ApprenticeshipTypes apprenticeshipType)
    {
        return apprenticeshipType switch
        {
            ApprenticeshipTypes.Standard => "Apprenticeship",
            ApprenticeshipTypes.Foundation => "Foundation apprenticeship",
            _ => throw new ArgumentOutOfRangeException(nameof(apprenticeshipType), apprenticeshipType, null)
        };
    }
}