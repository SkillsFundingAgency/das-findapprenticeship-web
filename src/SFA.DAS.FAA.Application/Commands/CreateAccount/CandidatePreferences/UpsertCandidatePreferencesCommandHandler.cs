using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.FAA.Infrastructure.Api;

namespace SFA.DAS.FAA.Application.Commands.CreateAccount.CandidatePreferences;
public class UpsertCandidatePreferencesCommandHandler(IApiClient apiClient) : IRequestHandler<UpsertCandidatePreferencesCommand, Unit>
{
    public async Task<Unit> Handle(UpsertCandidatePreferencesCommand request, CancellationToken cancellationToken)
    {
        var candidatePreferencesApiResponse = await apiClient.Get<GetCandidatePreferencesApiResponse>(new GetCandidatePreferencesApiRequest(request.CandidateId));

        var requestData = candidatePreferencesApiResponse.CandidatePreferences.Select(np => new UpsertCandidatePreferencesData.CandidatePreference
        {
            PreferenceId = np.PreferenceId,
            PreferenceMeaning = np.PreferenceMeaning,
            PreferenceHint = np.PreferenceHint,
            ContactMethodsAndStatus =
            [
                new UpsertCandidatePreferencesData.ContactMethodStatus
                {
                    ContactMethod = Constants.Constants.CandidatePreferences.ContactMethodEmail,
                    Status = request.UnfinishedApplicationReminders
                }
            ]
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
