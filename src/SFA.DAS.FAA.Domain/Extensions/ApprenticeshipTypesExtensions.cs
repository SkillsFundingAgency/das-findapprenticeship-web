using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Domain.Extensions;

public static class ApprenticeshipTypesExtensions
{
    public static bool IsFoundation(this ApprenticeshipTypes apprenticeshipType)
    {
        return apprenticeshipType == ApprenticeshipTypes.Foundation;
    }
}