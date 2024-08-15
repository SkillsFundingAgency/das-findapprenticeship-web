using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Web.UI;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SFA.DAS.FAA.Web.Models.Custom;

namespace SFA.DAS.FAA.Web.TagHelpers;

public class DayMonthYearDateTagHelper : TagHelper
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
        var dayValue = GetDayValue();
        var monthValue = GetMonthValue();
        var yearValue = GetYearValue();
        var errorMessage = GetErrorMessage();

        var stringWriter = new StringWriter();
        var writer = new HtmlTextWriter(stringWriter);


        if (!string.IsNullOrWhiteSpace(Label))
        {
            //fieldset
            writer.AddAttribute("class", "govuk-fieldset");
            writer.RenderBeginTag("fieldset");

            //legend
            writer.AddAttribute("class", "govuk-fieldset__legend govuk-fieldset__legend--m");
            writer.RenderBeginTag("legend");
            writer.Write(Label);
            writer.RenderEndTag();
        }

        //hint
        writer.AddAttribute("class", "govuk-hint");
        writer.AddAttribute("id", $"{Property.Name}Hint");
        writer.RenderBeginTag("div");
        writer.Write(HintText);
        writer.RenderEndTag();

        //error message
        if (!string.IsNullOrWhiteSpace(errorMessage))
        {
            writer.AddAttribute("class", "govuk-error-message");
            writer.RenderBeginTag("span");
            writer.Write(errorMessage);
            writer.RenderEndTag();
        }

        //date inputs
        writer.AddAttribute("class", "govuk-date-input");
        writer.RenderBeginTag("div");
        WriteInput(writer, $"{Property.Name}Day", dayValue, "Day", 2);
        WriteInput(writer, $"{Property.Name}Month", monthValue, "Month", 2);
        WriteInput(writer, $"{Property.Name}Year", yearValue, "Year", 4);
        writer.RenderEndTag();
        
        if (!string.IsNullOrWhiteSpace(Label))
        {
            writer.RenderEndTag(); //fieldset
        }

        var errorHighlight = string.IsNullOrWhiteSpace(errorMessage) ? string.Empty : ErrorCssClass;
        output.TagName = "div";
        output.Attributes.Add("id", $"{Property.Name}");
        output.Attributes.Add("name", $"{Property.Name}");
        output.Attributes.Add("class", $"govuk-form-group govuk-!-margin-bottom-7 {errorHighlight}");
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
        writer.AddAttribute("autocomplete", "off");
        writer.AddAttribute("class", $"govuk-input govuk-date-input__input govuk-input--width-{width}");
        writer.AddAttribute("inputmode", "numeric");
        if (!string.IsNullOrWhiteSpace(value))
        {
            writer.AddAttribute("value", value);
        }
        writer.RenderBeginTag("input");
        writer.RenderEndTag();
        writer.RenderEndTag();
        writer.RenderEndTag();

    }

    private string GetDayValue()
    {
        var modelState = ViewContext.ModelState[$"{Property.Name}Day"];
        if (modelState != null)
        {
            return modelState.AttemptedValue ?? string.Empty;
        }

        if (Property.Model is DayMonthYearDate { DateTimeValue: not null } model)
        {
            return model.DateTimeValue.Value.Day.ToString();
        }

        return string.Empty;
    }

    private string GetMonthValue()
    {
        var modelState = ViewContext.ModelState[$"{Property.Name}Month"];
        if (modelState != null)
        {
            return modelState.AttemptedValue ?? string.Empty;
        }

        if (Property.Model is DayMonthYearDate { DateTimeValue: not null } model)
        {
            return model.DateTimeValue.Value.Month.ToString();
        }

        return string.Empty;
    }

    private string GetYearValue()
    {
        var modelState = ViewContext.ModelState[$"{Property.Name}Year"];
        if (modelState != null)
        {
            return modelState.AttemptedValue ?? string.Empty;
        }

        if (Property.Model is DayMonthYearDate { DateTimeValue: not null } model)
        {
            return model.DateTimeValue.Value.Year.ToString();
        }

        return string.Empty;
    }

    private string GetErrorMessage()
    {
        if (!ViewContext.ModelState.ContainsKey(Property.Name)) return string.Empty;
        var modelState = ViewContext.ModelState[Property.Name];
        return modelState is { Errors.Count: > 0 } ? modelState.Errors.First().ErrorMessage : string.Empty;
    }
}

