using System.Text.Encodings.Web;
using System.Web.UI;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SFA.DAS.FAA.Web.Models.Custom;

namespace SFA.DAS.FAA.Web.TagHelpers
{
    public class MonthYearDateTagHelper : TagHelper
    {
        [HtmlAttributeName("property")]
        public ModelExpression Property { get; set; }

        [HtmlAttributeName("label")]
        public string Label { get; set; }

        [HtmlAttributeName("hint-text")]
        public string HintText { get; set; }

        [HtmlAttributeName("error-class")]
        public string ErrorCssClass { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var model = Property.Model as MonthYearDate;

            var monthId = $"{Property.Name}Month";
            var yearId = $"{Property.Name}Year";

            var monthValue = model == null ? string.Empty : model.Value.Value.Month.ToString();
            var yearValue = model == null ? string.Empty : model.Value.Value.Year.ToString();

            var isError = false;
            var errorOutput = string.Empty;
            var errorMessage = string.Empty;
            if (ViewContext.ModelState.ContainsKey(Property.Name))
            {
                var modelState = ViewContext.ModelState[Property.Name];
                if (modelState.Errors.Count > 0)
                {
                    isError = true;
                    errorOutput = ErrorCssClass;
                    errorMessage = modelState.Errors.First().ErrorMessage;
                }
            }

            var stringWriter = new StringWriter();
            var writer = new HtmlTextWriter(stringWriter);

            writer.AddAttribute("class", "govuk-fieldset");
            writer.RenderBeginTag("fieldset");
            
            writer.AddAttribute("class", "govuk-fieldset__legend govuk-fieldset__legend--m");
            writer.RenderBeginTag("legend");
            writer.Write(Label);
            writer.RenderEndTag(); //legend

            writer.AddAttribute("class","govuk-hint");
            writer.AddAttribute("id", $"{Property.Name}Hint");
            
            writer.RenderBeginTag("div"); //hint
            writer.Write(HintText);
            writer.RenderEndTag(); //hint

            if (isError)
            {
                writer.AddAttribute("class", "govuk-error-message");
                writer.RenderBeginTag("span"); //error message
                writer.Write(errorMessage);
                writer.RenderEndTag();
            }

            writer.AddAttribute("class", "govuk-date-input");
            writer.RenderBeginTag("div");

            WriteInput(writer, monthId, monthValue, "Month", 2);
            WriteInput(writer, yearId, yearValue, "Year", 4);

            writer.RenderEndTag(); //div
            writer.RenderEndTag(); //fieldset
            

            output.TagName = "div";
            output.Attributes.Add("id", $"{Property.Name}");
            output.Attributes.Add("name", $"{Property.Name}");
            output.Attributes.Add("class", $"govuk-form-group govuk-!-margin-bottom-7 {errorOutput}");
            output.Content.SetHtmlContent(stringWriter.ToString());
            output.TagMode = TagMode.StartTagAndEndTag;

            base.Process(context, output);
       }

        private void WriteInput(HtmlTextWriter writer, string id, string value, string label, int width)
        {
            writer.AddAttribute("class", "govuk-date-input__item");
            writer.RenderBeginTag("div");
            writer.AddAttribute("class", "govuk-form-group");
            writer.RenderBeginTag("div");
            writer.AddAttribute("class", "govuk-label govuk-date-input__label");
            writer.AddAttribute("for", id);
            writer.RenderBeginTag("label");
            writer.Write(label);
            writer.RenderEndTag();
            writer.AddAttribute("id", id);
            writer.AddAttribute("name", id);
            writer.AddAttribute("class", $"govuk-input govuk-date-input__input govuk-input--width-{width}");
            writer.WriteAttribute("inputmode", "numeric");
            if (!string.IsNullOrWhiteSpace(value))
            {
                writer.AddAttribute("value", value);
            }
            writer.RenderBeginTag("input");
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();

        }
    }
}
