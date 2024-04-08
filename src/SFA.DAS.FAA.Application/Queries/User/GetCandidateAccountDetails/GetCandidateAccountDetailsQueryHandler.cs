using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.Queries.User.GetCandidateAccountDetails;
public class GetCandidateAccountDetailsQueryHandler(IApiClient apiClient)
    : IRequestHandler<GetCandidateAccountDetailsQuery, GetCandidateAccountDetailsQueryResult>
{
    public async Task<GetCandidateAccountDetailsQueryResult> Handle(GetCandidateAccountDetailsQuery request, CancellationToken cancellationToken)
    {
        var checkAnswersResponse = await
            apiClient.Get<GetCandidateCheckAnswersApiResponse>(
                new GetCandidateCheckAnswersApiRequest(request.GovUkIdentifier));

        return new GetCandidateAccountDetailsQueryResult
        {
            FirstName = checkAnswersResponse.FirstName,
            MiddleNames = checkAnswersResponse.MiddleNames,
            LastName = checkAnswersResponse.LastName,
            DateOfBirth = checkAnswersResponse.DateOfBirth ?? DateTime.MinValue,
            AddressLine1 = checkAnswersResponse.AddressLine1,
            AddressLine2 = checkAnswersResponse.AddressLine2,
            Town = checkAnswersResponse.Town,
            County = checkAnswersResponse.County,
            Postcode = checkAnswersResponse.Postcode,
            CandidatePreferences = checkAnswersResponse.CandidatePreferences.Select(x => new GetCandidateAccountDetailsQueryResult.CandidatePreference
            {
                PreferenceId = x.PreferenceId,
                Meaning = x.PreferenceMeaning,
                Hint = x.PreferenceHint,
                EmailPreference = x.ContactMethodsAndStatus?.Where(x => x.ContactMethod == Constants.Constants.CandidatePreferences.ContactMethodEmail).FirstOrDefault()?.Status ?? false,
                TextPreference = x.ContactMethodsAndStatus?.Where(x => x.ContactMethod == Constants.Constants.CandidatePreferences.ContactMethodText).FirstOrDefault()?.Status ?? false
            }).ToList()
        };
    }
}
