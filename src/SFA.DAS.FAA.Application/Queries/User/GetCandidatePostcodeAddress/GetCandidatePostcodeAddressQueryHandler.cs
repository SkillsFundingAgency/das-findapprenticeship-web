using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.Queries.User.GetCandidatePostcodeAddress;
public class GetCandidatePostcodeAddressQueryHandler : IRequestHandler<GetCandidatePostcodeAddressQuery, GetCandidatePostcodeAddressQueryResult>
{
    private readonly IApiClient _apiClient;

    public GetCandidatePostcodeAddressQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetCandidatePostcodeAddressQueryResult> Handle(GetCandidatePostcodeAddressQuery request, CancellationToken cancellationToken)
    {
        return await _apiClient.Get<GetCandidatePostcodeAddressApiResponse>(new GetCandidatePostcodeAddressApiRequest(request.Postcode));
    }
}
