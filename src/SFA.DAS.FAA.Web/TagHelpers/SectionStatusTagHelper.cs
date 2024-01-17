using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json.Linq;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Models.Apply;
using System.Reflection;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Web.TagHelpers
{
    public class GovUkTagTagHelper : TagHelper
    {
        private const string ValueAttributeName = "status";

        [HtmlAttributeName(ValueAttributeName)]
        public ModelExpression Property { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var status = (SectionStatus) Property.Model;

            var cssClass = status.GetCssClass();
            var label = status.GetLabel();

            output.TagName = "strong";
            output.Attributes.Add("class", $"govuk-tag das-no-wrap {cssClass}");
            output.Content.SetHtmlContent(label);
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}
