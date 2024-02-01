using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetWorkHistories;

public record GetJobsQuery : IRequest<GetJobsQueryResult>
{
    public Guid CandidateId { get; init; }
    public Guid ApplicationId { get; init; }
}