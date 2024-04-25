using System.Web;
using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;

namespace CreateAccount.GetCandidatePostcodeAddress;
public class GetCandidatePostcodeAddressQueryHandler : IRequestHandler<GetCandidatePostcodeAddressQuery, GetCandidatePostcodeAddressQueryResult>
{
    private readonly IApiClient _apiClient;

    public GetCandidatePostcodeAddressQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetCandidatePostcodeAddressQueryResult> Handle(GetCandidatePostcodeAddressQuery request, CancellationToken cancellationToken)
    {
        var postcodeEncoded = HttpUtility.UrlEncode(request.Postcode);
        return await _apiClient.Get<GetCandidatePostcodeAddressApiResponse>(new GetCandidatePostcodeAddressApiRequest(postcodeEncoded));
    }
}
