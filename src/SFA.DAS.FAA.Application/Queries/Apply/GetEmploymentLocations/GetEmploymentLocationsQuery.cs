using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetEmploymentLocations;

public record GetEmploymentLocationsQuery(Guid ApplicationId, Guid CandidateId)
    : IRequest<GetEmploymentLocationsQueryResult>;