using Microsoft.AspNetCore.Razor.TagHelpers;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Extensions;

namespace SFA.DAS.FAA.Web.TagHelpers
{
    public class ApplicationStatusBadgeTagHelper : TagHelper
    {
        public string? Status { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var status = (ApplicationStatus)Enum.Parse(typeof(ApplicationStatus), Status!, true);

            var cssClass = status.GetCssClass();
            var label = status.GetLabel();

            output.TagName = "strong";
            output.Attributes.Add("class", $"{cssClass}");
            output.Content.SetHtmlContent(label);
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}
