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
                Meaning = x.PreferenceMeaning,
                Hint = x.PreferenceHint,
                Preference = x.ContactMethodsAndStatus?.Where(x => x.ContactMethod == Constants.Constants.CandidatePreferences.ContactMethodEmail).FirstOrDefault()?.Status ?? null,
            }).ToList()
        };
    }
}
