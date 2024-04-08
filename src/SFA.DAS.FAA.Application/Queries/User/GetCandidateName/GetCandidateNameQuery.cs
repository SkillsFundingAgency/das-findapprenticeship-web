using MediatR;

namespace SFA.DAS.FAA.Application.Queries.User.GetCandidateName;
public class GetCandidateNameQuery : IRequest<GetCandidateNameQueryResult>
{
    public Guid CandidateId { get; set; }
}
