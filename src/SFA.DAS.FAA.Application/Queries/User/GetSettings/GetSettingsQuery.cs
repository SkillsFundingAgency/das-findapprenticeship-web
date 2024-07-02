using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.Queries.User.GetSettings
{
    public class GetSettingsQuery : IRequest<GetSettingsQueryResult>
    {
        public Guid CandidateId { get; set; }
        public string? Email { get; set; }
    }

    public class GetSettingsQueryResult
    {
        public string FirstName { get; set; }
        public string MiddleNames { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Uprn { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool HasAnsweredEqualityQuestions { get; set; }
        public List<CandidatePreference> CandidatePreferences { get; set; }

        public class CandidatePreference
        {
            public Guid PreferenceId { get; set; }
            public string Meaning { get; set; }
            public string Hint { get; set; }
            public bool EmailPreference { get; set; }
        }
    }

    public class GetSettingsQueryHandler(IApiClient apiClient) : IRequestHandler<GetSettingsQuery, GetSettingsQueryResult>
    {
        public async Task<GetSettingsQueryResult> Handle(GetSettingsQuery request, CancellationToken cancellationToken)
        {
            var getSettingsApiResponse = await
                apiClient.Get<GetSettingsApiResponse>(
                    new GetSettingsApiRequest(request.CandidateId, request.Email));

            return new GetSettingsQueryResult
            {
                FirstName = getSettingsApiResponse.FirstName,
                MiddleNames = getSettingsApiResponse.MiddleNames,
                LastName = getSettingsApiResponse.LastName,
                DateOfBirth = getSettingsApiResponse.DateOfBirth ?? DateTime.MinValue,
                Uprn = getSettingsApiResponse.Uprn,
                AddressLine1 = getSettingsApiResponse.AddressLine1,
                AddressLine2 = getSettingsApiResponse.AddressLine2,
                Town = getSettingsApiResponse.Town,
                County = getSettingsApiResponse.County,
                Postcode = getSettingsApiResponse.Postcode,
                PhoneNumber = getSettingsApiResponse.PhoneNumber,
                Email = getSettingsApiResponse.Email,
                HasAnsweredEqualityQuestions = getSettingsApiResponse.HasAnsweredEqualityQuestions,
                CandidatePreferences = getSettingsApiResponse.CandidatePreferences.Select(x => new GetSettingsQueryResult.CandidatePreference
                {
                    PreferenceId = x.PreferenceId,
                    Meaning = x.PreferenceMeaning,
                    Hint = x.PreferenceHint,
                    EmailPreference = x.ContactMethodsAndStatus?.Where(x => x.ContactMethod == Constants.Constants.CandidatePreferences.ContactMethodEmail).FirstOrDefault()?.Status ?? false,
                }).ToList()
            };
        }

    }
}
