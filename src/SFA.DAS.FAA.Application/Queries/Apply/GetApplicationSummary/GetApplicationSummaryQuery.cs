using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSummary;

public record GetApplicationSummaryQuery : IRequest<GetApplicationSummaryQueryResult>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
}