using MediatR;

namespace CreateAccount.GetCandidateName;
public class GetCandidateNameQuery : IRequest<GetCandidateNameQueryResult>
{
    public Guid CandidateId { get; set; }
}
