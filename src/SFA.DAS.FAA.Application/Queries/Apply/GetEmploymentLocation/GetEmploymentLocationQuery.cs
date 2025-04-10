using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetEmploymentLocation;

public record GetEmploymentLocationQuery(Guid ApplicationId, Guid CandidateId)
    : IRequest<GetEmploymentLocationQueryResult>;