using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Web.Extensions
{
    public static class SectionStatusExtensions
    {
        public static string GetLabel(this SectionStatus sectionStatus)
        {
            return sectionStatus switch
            {
                SectionStatus.NotStarted => "Not yet started",
                SectionStatus.InProgress => "Incomplete",
                SectionStatus.Incomplete => "Incomplete",
                SectionStatus.Completed => "Complete",
                SectionStatus.NotRequired => "Not Required",
                SectionStatus.PreviousAnswer => "Previous answer",
                _ => string.Empty
            };
        }

        public static string GetCssClass(this SectionStatus sectionStatus)
        {
            return sectionStatus switch
            {
                SectionStatus.NotStarted => "govuk-tag das-no-wrap govuk-tag--blue",
                SectionStatus.InProgress => "govuk-tag das-no-wrap govuk-tag--light-blue",
                SectionStatus.Incomplete => "govuk-tag das-no-wrap govuk-tag--light-blue",
                SectionStatus.Completed => "govuk-body das-no-wrap",
                SectionStatus.NotRequired => "govuk-tag das-no-wrap govuk-tag--grey",
                SectionStatus.PreviousAnswer => "govuk-body das-no-wrap",
                _ => string.Empty
            };
        }
    }
}
