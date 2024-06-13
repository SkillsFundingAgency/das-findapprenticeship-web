using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Models.User
{
    public class EmailViewModel
    {
        public string PageCaption => JourneyPath == UserJourneyPath.Settings ? string.Empty : "Create an account";
        public string BackLink => GetBackLink();
        public UserJourneyPath JourneyPath { get; init; } = UserJourneyPath.ConfirmAccountDetails;

        private string GetBackLink()
        {
            return JourneyPath switch
            {
                UserJourneyPath.ConfirmAccountDetails => RouteNames.ConfirmAccountDetails,
                UserJourneyPath.Settings => RouteNames.Settings,
                _ => RouteNames.ConfirmAccountDetails
            };
        }
    }
}
