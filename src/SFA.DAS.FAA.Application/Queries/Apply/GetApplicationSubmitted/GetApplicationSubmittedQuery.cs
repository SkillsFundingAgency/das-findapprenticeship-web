using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSubmitted;
public class GetApplicationSubmittedQuery : IRequest<GetApplicationSubmittedQueryResponse>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
}
