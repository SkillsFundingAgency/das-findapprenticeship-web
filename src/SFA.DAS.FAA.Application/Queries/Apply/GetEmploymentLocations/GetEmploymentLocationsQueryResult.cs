using SFA.DAS.FAA.Domain.Apply.GetEmploymentLocations;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetEmploymentLocations;

public record GetEmploymentLocationsQueryResult
{
    public LocationDto EmploymentLocation { get; init; } = new();
    public bool? IsSectionCompleted { get; set; }


    public static implicit operator GetEmploymentLocationsQueryResult(GetEmploymentLocationsApiResponse source)
    {
        return new GetEmploymentLocationsQueryResult
        {
            IsSectionCompleted = source.IsSectionCompleted,
            EmploymentLocation = source.EmploymentLocation,
        };
    }
}