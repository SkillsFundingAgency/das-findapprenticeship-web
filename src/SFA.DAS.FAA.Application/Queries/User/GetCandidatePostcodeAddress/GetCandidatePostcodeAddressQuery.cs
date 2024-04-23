using MediatR;

namespace CreateAccount.GetCandidatePostcodeAddress;
public class GetCandidatePostcodeAddressQuery : IRequest<GetCandidatePostcodeAddressQueryResult>
{
    public string Postcode { get; set; }
}
