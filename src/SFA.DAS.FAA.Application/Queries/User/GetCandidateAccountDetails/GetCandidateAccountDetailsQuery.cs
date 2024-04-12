using MediatR;

namespace CreateAccount.GetCandidateAccountDetails;
public class GetCandidateAccountDetailsQuery : IRequest<GetCandidateAccountDetailsQueryResult>
{
    public Guid CandidateId { get; set; }
}
