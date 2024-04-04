using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.Queries.User.GetCandidateDateOfBirth;
public class GetCandidateDateOfBirthQueryHandler : IRequestHandler<GetCandidateDateOfBirthQuery, GetCandidateDateOfBirthQueryResult>
{
    private readonly IApiClient _apiClient;

    public GetCandidateDateOfBirthQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetCandidateDateOfBirthQueryResult> Handle(GetCandidateDateOfBirthQuery request, CancellationToken cancellationToken)
    {
        return await _apiClient.Get<GetCandidateDateOfBirthApiResponse>(new GetCandidateDateOfBirthApiRequest(request.GovUkIdentifier));
    }
}
