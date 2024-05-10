using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Models.Applications;

namespace SFA.DAS.FAA.Web.Extensions
{
    public static class ApplicationsTabExtensions
    {
        public static ApplicationStatus ToApplicationStatus(this ApplicationsTab tab)
        {
            return tab switch
            {
                ApplicationsTab.Started => ApplicationStatus.Draft,
                ApplicationsTab.Submitted => ApplicationStatus.Submitted,
                ApplicationsTab.Successful => ApplicationStatus.Successful,
                ApplicationsTab.Unsuccessful => ApplicationStatus.Unsuccessful,
                _ => throw new ArgumentException($"Unable to map tab {tab} to status value")
            };
        }

        public static string GetTabTitle(this ApplicationsTab tab)
        {
            return tab switch
            {
                ApplicationsTab.Started => "Started",
                ApplicationsTab.Submitted => "Submitted",
                ApplicationsTab.Successful => "Successful",
                ApplicationsTab.Unsuccessful => "Unsuccessful",
                _ => ""
            };
        }

        public static string GetTabPopulatedText(this ApplicationsTab tab)
        {
            return tab switch
            {
                ApplicationsTab.Started => "Apply as soon as you can. A company may close their vacancy early if they receive a lot of applications.",
                ApplicationsTab.Submitted => "Companies will contact you using the telephone number or email address on your application.",
                ApplicationsTab.Successful => "The company will contact you to discuss your next steps.",
                ApplicationsTab.Unsuccessful => "You’ve been given feedback to help you improve your future applications.",
                _ => ""
            };
        }

        public static string GetTabEmptyText(this ApplicationsTab tab)
        {
            return tab switch
            {
                ApplicationsTab.Started => "You have no applications to finish.",
                ApplicationsTab.Submitted => "You have no submitted applications that are waiting for a response.",
                ApplicationsTab.Successful => "If an application you make is successful, it’ll appear here.",
                ApplicationsTab.Unsuccessful => "If an application you make is unsuccessful, you’ll get feedback here to help you improve your future applications.",
                _ => ""
            };
        }
    }
}
