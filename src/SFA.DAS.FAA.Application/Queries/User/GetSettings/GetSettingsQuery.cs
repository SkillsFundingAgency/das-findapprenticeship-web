using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.Queries.User.GetSettings
{
    public class GetSettingsQuery : IRequest<GetSettingsQueryResult>
    {
        public Guid CandidateId { get; set; }
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
            var checkAnswersResponse = await
                apiClient.Get<GetSettingsApiResponse>(
                    new GetSettingsApiRequest(request.CandidateId));

            return new GetSettingsQueryResult
            {
                FirstName = checkAnswersResponse.FirstName,
                MiddleNames = checkAnswersResponse.MiddleNames,
                LastName = checkAnswersResponse.LastName,
                DateOfBirth = checkAnswersResponse.DateOfBirth ?? DateTime.MinValue,
                Uprn = checkAnswersResponse.Uprn,
                AddressLine1 = checkAnswersResponse.AddressLine1,
                AddressLine2 = checkAnswersResponse.AddressLine2,
                Town = checkAnswersResponse.Town,
                County = checkAnswersResponse.County,
                Postcode = checkAnswersResponse.Postcode,
                PhoneNumber = checkAnswersResponse.PhoneNumber,
                Email = checkAnswersResponse.Email,
                HasAnsweredEqualityQuestions = checkAnswersResponse.HasAnsweredEqualityQuestions,
                CandidatePreferences = checkAnswersResponse.CandidatePreferences.Select(x => new GetSettingsQueryResult.CandidatePreference
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
