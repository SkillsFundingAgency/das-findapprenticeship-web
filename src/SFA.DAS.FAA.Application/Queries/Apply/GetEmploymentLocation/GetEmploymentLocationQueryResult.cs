using SFA.DAS.FAA.Domain.Apply.GetEmploymentLocation;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetEmploymentLocation;

public record GetEmploymentLocationQueryResult
{
    public LocationDto EmploymentLocation { get; private init; } = new();
    
    public static implicit operator GetEmploymentLocationQueryResult(GetEmploymentLocationApiResponse source)
    {
        return new GetEmploymentLocationQueryResult
        {
            EmploymentLocation = source.EmploymentLocation,
        };
    }
}