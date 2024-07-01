using SFA.DAS.FAA.Application.Queries.User.GetTransferUserData;

namespace SFA.DAS.FAA.Web.Models.User
{
    public class ConfirmTransferViewModel
    {
        public string? Name { get; init; }
        public string EmailAddress { get; set; }

        public bool ShowStartedApplicationsCount => !string.IsNullOrEmpty(StartedApplicationsText);
        
        public bool ShowSubmittedApplicationsCount => !string.IsNullOrEmpty(SubmittedApplicationsText);
        
        public bool ShowSavedApplicationsCount => !string.IsNullOrEmpty(SavedApplicationsText);
        public long SavedApplicationsCount { get; set; }
        public long SubmittedApplicationsCount { get; set; }
        public long StartedApplicationsCount { get; set; }
        public string SavedApplicationsText { get; set; }
        public string SubmittedApplicationsText { get; set; }
        public string StartedApplicationsText { get; set; }

        public static implicit operator ConfirmTransferViewModel(GetTransferUserDataQueryResult source)
        {
            return new ConfirmTransferViewModel
            {
                Name = source.CandidateFirstName,
                
                SavedApplicationsText = source.SavedApplications switch
                {
                    0 => "",
                    1 => " saved vacancy",
                    _ => " saved vacancies"
                },
                SubmittedApplicationsText = source.SubmittedApplications switch
                {
                    0 => "",
                    1 => " submitted application",
                    _ => " submitted applications"
                },
                StartedApplicationsText = source.StartedApplications switch
                {
                    0 => "",
                    1 => " started application",
                    _ => " started applications"
                },
                StartedApplicationsCount = source.StartedApplications,
                SubmittedApplicationsCount = source.SubmittedApplications,
                SavedApplicationsCount = source.StartedApplications,
            };
        }
    }
}