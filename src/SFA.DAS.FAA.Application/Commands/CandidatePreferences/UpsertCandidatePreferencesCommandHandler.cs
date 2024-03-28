using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.FAA.Infrastructure.Api;

namespace SFA.DAS.FAA.Application.Commands.CandidatePreferences;
public class UpsertCandidatePreferencesCommandHandler(IApiClient apiClient) : IRequestHandler<UpsertCandidatePreferencesCommand, Unit>
{
    public async Task<Unit> Handle(UpsertCandidatePreferencesCommand request, CancellationToken cancellationToken)
    {
        var requestData = request.NotificationPreferences.Select(np => new UpsertCandidatePreferencesData.CandidatePreference
        {
            PreferenceId = np.PreferenceId,
            PreferenceMeaning = np.Meaning,
            PreferenceHint = np.Hint,
            ContactMethodsAndStatus = new List<UpsertCandidatePreferencesData.ContactMethodStatus>()
            {
                new UpsertCandidatePreferencesData.ContactMethodStatus()
                {
                    ContactMethod = Constants.Constants.CandidatePreferences.ContactMethodText,
                    Status = np.TextPreference
                },
                new UpsertCandidatePreferencesData.ContactMethodStatus()
                {
                    ContactMethod = Constants.Constants.CandidatePreferences.ContactMethodEmail,
                    Status = np.EmailPreference
                }
            }
        }).ToList();

        await apiClient.PostWithResponseCode<NullResponse>
            (new UpsertCandidatePreferencesApiRequest(request.CandidateId, new UpsertCandidatePreferencesData
            {
                CandidatePreferences = requestData,
                Email = request.CandidateEmail
            }));

        return Unit.Value;
    }
}
