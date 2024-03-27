using MediatR;

namespace SFA.DAS.FAA.Application.Queries.User.GetAddressesByPostcode;
public class GetAddressesByPostcodeQuery : IRequest<GetAddressesByPostcodeQueryResult>
{
    public string Postcode { get; set; }
}
