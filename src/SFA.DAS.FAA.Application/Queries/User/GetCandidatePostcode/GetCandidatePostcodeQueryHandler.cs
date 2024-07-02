using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.Queries.User.GetCandidatePostcode;

public class GetCandidatePostcodeQueryHandler(IApiClient apiClient) : IRequestHandler<GetCandidatePostcodeQuery, GetCandidatePostcodeQueryResult>
{
    public async Task<GetCandidatePostcodeQueryResult> Handle(GetCandidatePostcodeQuery request, CancellationToken cancellationToken)
    {
        var result =
            await apiClient.Get<GetCandidatePostcodeApiResponse?>(
                new GetCandidatePostcodeApiRequest(request.CandidateId));
        
        return new GetCandidatePostcodeQueryResult
        {
            Postcode = result?.Postcode
        };
    }
}