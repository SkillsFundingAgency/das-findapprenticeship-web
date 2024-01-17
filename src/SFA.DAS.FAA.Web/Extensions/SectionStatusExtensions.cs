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
                SectionStatus.Completed => "Complete",
                SectionStatus.NotRequired => "Not Required",
                _ => string.Empty
            };
        }

        public static string GetCssClass(this SectionStatus sectionStatus)
        {
            return sectionStatus switch
            {
                SectionStatus.NotStarted => "govuk-tag--blue",
                SectionStatus.InProgress => "govuk-tag--grey",
                SectionStatus.Completed => "govuk-tag--green",
                SectionStatus.NotRequired => "govuk-tag--grey",
                _ => string.Empty
            };
        }
    }
}
