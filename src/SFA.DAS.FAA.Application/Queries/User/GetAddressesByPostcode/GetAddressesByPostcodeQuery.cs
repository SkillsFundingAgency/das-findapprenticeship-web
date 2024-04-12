using MediatR;

namespace CreateAccount.GetAddressesByPostcode;
public class GetAddressesByPostcodeQuery : IRequest<GetAddressesByPostcodeQueryResult>
{
    public Guid CandidateId { get; set; }
    public string Postcode { get; set; }
}
