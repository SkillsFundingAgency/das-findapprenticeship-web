using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.Queries.User.GetCandidateAccountDetails;
public class GetCandidateAccountDetailsQueryHandler : IRequestHandler<GetCandidateAccountDetailsQuery, GetCandidateAccountDetailsQueryResult>
{
    private readonly IApiClient _apiClient;

    public GetCandidateAccountDetailsQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetCandidateAccountDetailsQueryResult> Handle(GetCandidateAccountDetailsQuery request, CancellationToken cancellationToken)
    {
        var name = _apiClient.Get<GetCandidateNameApiResponse>(new GetCandidateNameApiRequest(request.GovUkIdentifier));
        var dateOfBirth = _apiClient.Get<GetCandidateDateOfBirthApiResponse>(new GetCandidateDateOfBirthApiRequest(request.GovUkIdentifier));
        var address = _apiClient.Get<GetCandidateAddressApiResponse>(new GetCandidateAddressApiRequest(request.CandidateId));
        var candidatePreferences = _apiClient.Get<GetCandidatePreferencesApiResponse>(new GetCandidatePreferencesApiRequest(request.CandidateId));

        await Task.WhenAll(dateOfBirth, address, candidatePreferences);

        return new GetCandidateAccountDetailsQueryResult
        {
            FirstName = name.Result.FirstName,
            MiddleNames = name.Result.MiddleNames,
            LastName = name.Result.LastName,
            DateOfBirth = dateOfBirth.Result.DateOfBirth.Value,
            AddressLine1 = address.Result.AddressLine1,
            AddressLine2 = address.Result.AddressLine2,
            Town = address.Result.Town,
            County = address.Result.County,
            Postcode = address.Result.Postcode,
            CandidatePreferences = candidatePreferences.Result.CandidatePreferences.Select(x => new GetCandidateAccountDetailsQueryResult.CandidatePreference
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
