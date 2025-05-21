using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SFA.DAS.FAA.Web.TagHelpers
{
    [HtmlTargetElement(TagName, TagStructure = TagStructure.NormalOrSelfClosing)]
    public class NotificationBadgeTagHelper : TagHelper
    {
        private const string TagName = "notification-badge";
        [HtmlAttributeName("count")]
        public required string Count { get; set; }
        [HtmlAttributeName("class")]
        public string? Class { get; set; } = "faa-badge";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!string.IsNullOrEmpty(Count))
            {
                output.TagName = "span";
                output.Attributes.SetAttribute("class", Class);
                output.Content.SetContent(Count);
            }
            else
            {
                output.SuppressOutput();
            }
        }
    }
}
