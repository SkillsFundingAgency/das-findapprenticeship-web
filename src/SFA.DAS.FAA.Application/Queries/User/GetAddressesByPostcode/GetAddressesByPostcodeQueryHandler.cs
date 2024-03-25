using System.Web;
using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using static SFA.DAS.FAA.Domain.User.GetAddressesByPostcodeApiResponse;

namespace SFA.DAS.FAA.Application.Queries.User.GetAddressesByPostcode;
public class GetAddressesByPostcodeQueryHandler : IRequestHandler<GetAddressesByPostcodeQuery, GetAddressesByPostcodeQueryResult>
{
    private readonly IApiClient _apiClient;

    public GetAddressesByPostcodeQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetAddressesByPostcodeQueryResult> Handle(GetAddressesByPostcodeQuery query, CancellationToken cancellationToken)
    {
        var request = new GetAddressesByPostcodeApiRequest(HttpUtility.UrlEncode(query.Postcode));
        var results = await _apiClient.Get<GetAddressesByPostcodeApiResponse>(request);

        if (results is null)
        {
            return new GetAddressesByPostcodeQueryResult
            {
                Addresses = Enumerable.Empty<AddressListItem>()
            };
        }

        return new GetAddressesByPostcodeQueryResult
        {
            Addresses = results.Addresses
        };
    }
}
