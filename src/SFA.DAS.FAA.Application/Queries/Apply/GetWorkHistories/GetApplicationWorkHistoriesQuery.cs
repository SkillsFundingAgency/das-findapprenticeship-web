using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetWorkHistories;

public record GetApplicationWorkHistoriesQuery : IRequest<GetApplicationWorkHistoriesQueryResult>
{
    public Guid CandidateId { get; init; }
    public Guid ApplicationId { get; init; }
}