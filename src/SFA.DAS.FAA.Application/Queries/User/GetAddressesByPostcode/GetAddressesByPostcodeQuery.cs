using MediatR;

namespace SFA.DAS.FAA.Application.Queries.User.GetAddressesByPostcode;
public class GetAddressesByPostcodeQuery : IRequest<GetAddressesByPostcodeQueryResult>
{
    public Guid CandidateId { get; set; }
    public string Postcode { get; set; }
}
