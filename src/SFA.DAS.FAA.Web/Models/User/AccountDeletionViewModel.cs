using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Models.User
{
    public class AccountDeletionViewModel : ViewModelBase
    {
        public string PageBackLink => GetPageBackLink();
        public string? Email { get; set; }
        public bool HasAnyOutstandingApplications => JourneyPath == RouthPath.WithdrawApplications;
        public RouthPath JourneyPath { get; set; }

        public string GetPageBackLink()
        {
            switch (JourneyPath)
            {
                case RouthPath.WithdrawApplications:
                    return RouteNames.AccountDeleteWithDrawApplication;
                case RouthPath.ConfirmAccountDeletion:
                default:
                    return RouteNames.ConfirmAccountDelete;
            }
        }

        public enum RouthPath
        {
            ConfirmAccountDeletion,
            WithdrawApplications
        }
    }
}
