using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SFA.DAS.FAA.Web.TagHelpers
{
    [HtmlTargetElement("span", Attributes = ValidationForAttributeName)]
    public class DasValidationMessageTagHelper : TagHelper
    {
        private const string ValidationForAttributeName = "das-validation-for";

        [HtmlAttributeName(ValidationForAttributeName)]
        public ModelExpression Property { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator Generator { get; }

        public DasValidationMessageTagHelper(IHtmlGenerator generator)
        {
            Generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("id", $"{Property.Name}-error");

            var tagBuilder = Generator.GenerateValidationMessage(
                ViewContext,
                Property.ModelExplorer,
                Property.Name,
                message: string.Empty,
                tag: null,
                htmlAttributes: null);

            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
