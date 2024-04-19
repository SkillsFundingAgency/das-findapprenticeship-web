using System.Web;
using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;

namespace CreateAccount.GetAddressesByPostcode;
public class GetAddressesByPostcodeQueryHandler : IRequestHandler<GetAddressesByPostcodeQuery, GetAddressesByPostcodeQueryResult>
{
    private readonly IApiClient _apiClient;

    public GetAddressesByPostcodeQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetAddressesByPostcodeQueryResult> Handle(GetAddressesByPostcodeQuery query, CancellationToken cancellationToken)
    {
        var request = new GetAddressesByPostcodeApiRequest(query.CandidateId, HttpUtility.UrlEncode(query.Postcode));
        var results = await _apiClient.Get<GetAddressesByPostcodeApiResponse>(request);

        return results;
    }
}
