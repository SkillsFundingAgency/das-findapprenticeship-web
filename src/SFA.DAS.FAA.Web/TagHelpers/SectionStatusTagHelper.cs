using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Web.TagHelpers
{
    public class GovUkTagTagHelper : TagHelper
    {
        [HtmlAttributeName("status")]
        public ModelExpression Property { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var status = (SectionStatus) Property.Model;

            var cssClass = status.GetCssClass();
            var label = status.GetLabel();

            output.TagName = "strong";
            output.Attributes.Add("class", $"{cssClass}");
            output.Content.SetHtmlContent(label);
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}
