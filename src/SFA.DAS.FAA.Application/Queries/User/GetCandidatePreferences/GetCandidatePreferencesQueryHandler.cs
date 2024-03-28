using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.Queries.User.GetCandidatePreferences;
public class GetCandidatePreferencesQueryHandler(IApiClient apiClient) : IRequestHandler<GetCandidatePreferencesQuery, GetCandidatePreferencesQueryResult>
{
    public async Task<GetCandidatePreferencesQueryResult> Handle(GetCandidatePreferencesQuery request, CancellationToken cancellationToken)
    {
        var result = await apiClient.Get<GetCandidatePreferencesApiResponse>(new GetCandidatePreferencesApiRequest(request.CandidateId));

        return new GetCandidatePreferencesQueryResult
        {
            CandidatePreferences = result.CandidatePreferences.Select(x => new GetCandidatePreferencesQueryResult.CandidatePreference
            {
                PreferenceId = x.PreferenceId,
                PreferenceMeaning = x.PreferenceMeaning,
                PreferenceHint = x.PreferenceHint,
                ContactMethodsAndStatus = x.ContactMethodsAndStatus?.Select(x => new GetCandidatePreferencesQueryResult.ContactMethodStatus
                {
                    ContactMethod = x.ContactMethod,
                    Status = x.Status
                }).ToList()
            }).ToList()
        };
    }
}
