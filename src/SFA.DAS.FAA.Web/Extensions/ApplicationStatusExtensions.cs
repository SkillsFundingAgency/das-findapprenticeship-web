using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Web.Extensions
{
    public static class ApplicationStatusExtensions
    {
        public static string GetLabel(this ApplicationStatus status)
        {
            return status switch
            {
                ApplicationStatus.Draft => "Application started",
                ApplicationStatus.Submitted => "Applied",
                ApplicationStatus.Withdrawn => "Applied",
                ApplicationStatus.Successful => "Applied",
                ApplicationStatus.Unsuccessful => "Applied",
                _ => string.Empty
            };
        }

        public static string GetCssClass(this ApplicationStatus status)
        {
            return status switch
            {
                ApplicationStatus.Draft => "govuk-tag govuk-tag--yellow govuk-!-margin-bottom-2",
                ApplicationStatus.Submitted => "govuk-tag govuk-tag--green govuk-!-margin-bottom-2",
                ApplicationStatus.Withdrawn => "govuk-tag govuk-tag--green govuk-!-margin-bottom-2",
                ApplicationStatus.Successful => "govuk-tag govuk-tag--green govuk-!-margin-bottom-2",
                ApplicationStatus.Unsuccessful => "govuk-tag govuk-tag--green govuk-!-margin-bottom-2",
                _ => string.Empty
            };
        }

        public static string GetCountLabel(this int count)
        {
            return count switch
            {
                > 0 and <= 99 => count.ToString(),
                > 99 => "99+",
                _ => string.Empty
            };
        }
    }
}
