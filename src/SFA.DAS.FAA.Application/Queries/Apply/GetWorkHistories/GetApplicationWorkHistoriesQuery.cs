using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetWorkHistories;

public class GetApplicationWorkHistoriesQuery : IRequest<GetApplicationWorkHistoriesQueryResult>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
}