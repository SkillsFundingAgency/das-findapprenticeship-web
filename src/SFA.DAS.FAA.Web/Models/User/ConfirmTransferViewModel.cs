using SFA.DAS.FAA.Application.Queries.User.GetTransferUserData;

namespace SFA.DAS.FAA.Web.Models.User
{
    public class ConfirmTransferViewModel
    {
        public string? Name { get; init; }
        public string EmailAddress { get; set; }

        public bool ShowStartedApplicationsCount => StartedApplicationsCount > 0;
        public long StartedApplicationsCount { get; init; } = 0;

        public bool ShowSubmittedApplicationsCount => SubmittedApplicationsCount > 0;
        public long SubmittedApplicationsCount { get; init; } = 0;

        public bool ShowSavedApplicationsCount => SavedApplicationsCount > 0;
        public long SavedApplicationsCount { get; init; } = 0;

        public static implicit operator ConfirmTransferViewModel(GetTransferUserDataQueryResult source)
        {
            return new ConfirmTransferViewModel
            {
                Name = source.CandidateFirstName,
                SavedApplicationsCount = source.SavedApplications,
                StartedApplicationsCount = source.StartedApplications,
                SubmittedApplicationsCount = source.SubmittedApplications,
            };
        }
    }
}