using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Models.User
{
    public class EmailViewModel(bool isProd)
    {
        public string PageCaption => GetPageCaption();
        public string BackLink => GetBackLink();
        public UserJourneyPath JourneyPath { get; init; } = UserJourneyPath.ConfirmAccountDetails;

        public string ChangeEmailLink => isProd 
            ? "https://home.account.gov.uk/settings" 
            : "https://home.integration.account.gov.uk/settings";
        
        private string GetBackLink()
        {
            return JourneyPath switch
            {
                UserJourneyPath.ConfirmAccountDetails => RouteNames.ConfirmAccountDetails,
                UserJourneyPath.Settings => RouteNames.Settings,
                _ => RouteNames.ConfirmAccountDetails
            };
        }

        private string GetPageCaption()
        {
            return JourneyPath switch
            {
                UserJourneyPath.Settings => string.Empty,
                _ => "Create an account"
            };
        }
    }
}
