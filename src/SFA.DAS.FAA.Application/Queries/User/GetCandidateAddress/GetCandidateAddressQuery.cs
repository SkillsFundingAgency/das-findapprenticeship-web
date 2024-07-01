using MediatR;

namespace CreateAccount.GetCandidatePostcode;
public class GetCandidateAddressQuery : IRequest<GetCandidateAddressQueryResult>
{
    public Guid CandidateId { get; set; }
}
