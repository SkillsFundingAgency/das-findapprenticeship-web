using MediatR;

namespace SFA.DAS.FAA.Application.Queries.User.GetCandidateAccountDetails;
public class GetCandidateAccountDetailsQuery : IRequest<GetCandidateAccountDetailsQueryResult>
{
    public Guid CandidateId { get; set; }
}
