using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.Queries.User.GetCandidatePostcode;
public class GetCandidateAddressQueryHandler : IRequestHandler<GetCandidateAddressQuery, GetCandidateAddressQueryResult>
{
    private readonly IApiClient _apiClient;

    public GetCandidateAddressQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetCandidateAddressQueryResult> Handle(GetCandidateAddressQuery request, CancellationToken cancellationToken)
    {
        var result = await _apiClient.Get<GetCandidateAddressApiResponse>(new GetCandidateAddressApiRequest(request.CandidateId));

        return new GetCandidateAddressQueryResult
        {
            IsAddressFromLookup = result.IsAddressFromLookup,
            Postcode = result.Postcode,
            AddressLine1 = result.AddressLine1,
            AddressLine2 = result.AddressLine2,
            Town = result.Town,
            County = result.County
        };
    }
}
