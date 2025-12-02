using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SFA.DAS.FAA.Web.TagHelpers;

public enum BannerStyle
{
    Important,
    Success,
}

[HtmlTargetElement(TagName)]
[OutputElementHint("div")]
public class GovUkBannerTagHelper: TagHelper
{
    public const string TagName = "govuk-banner";
    public string Id { get; set; } = $"banner_{Guid.NewGuid()}";
    public string? Title { get; set; }
    public BannerStyle Style { get; set; } = BannerStyle.Important;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("govuk-notification-banner", HtmlEncoder.Default);
        output.Attributes.Add("aria-labelledby", Id);
        output.Attributes.Add("data-module", "govuk-notification-banner");
        
        switch (Style)
        {
            case BannerStyle.Success:
                {
                    Title ??= "Success";
                    output.AddClass("govuk-notification-banner--success", HtmlEncoder.Default);
                    output.Attributes.Add("role", "alert");
                    break;
                }
            case BannerStyle.Important:
            default:
                {
                    Title ??= "Important";
                    output.Attributes.Add("role", "region");
                    break;
                }
        }
        
        output.Content.AppendHtml(RenderHeader(Id, Title));
        output.Content.AppendHtml(RenderContent(await output.GetChildContentAsync()));
    }

    private static TagBuilder RenderHeader(string id, string title)
    {
        var header = new TagBuilder("h2");
        header.AddCssClass("govuk-notification-banner__title");
        header.Attributes.Add("id", id);
        header.InnerHtml.AppendHtml(title);
        
        var div = new TagBuilder("div");
        div.AddCssClass("govuk-notification-banner__header");
        div.InnerHtml.AppendHtml(header);
        return div;
    }
    
    private static TagBuilder RenderContent(TagHelperContent content)
    {
        var div = new TagBuilder("div");
        div.AddCssClass("govuk-notification-banner__content");
        div.InnerHtml.AppendHtml(content);
        return div;
    }
}

[HtmlTargetElement(TagName, ParentTag = GovUkBannerTagHelper.TagName)]
[OutputElementHint("h3")]
public class GovUkBannerHeadingTagHelper: TagHelper
{
    public const string TagName = "govuk-banner-heading";

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "h3";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("govuk-notification-banner__heading", HtmlEncoder.Default);
        output.Content.AppendHtml(await output.GetChildContentAsync());
    }
}