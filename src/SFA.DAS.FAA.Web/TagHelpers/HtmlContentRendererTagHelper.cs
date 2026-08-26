using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace SFA.DAS.FAA.Web.TagHelpers;

/// <summary>
/// Renders the supplied content either as raw HTML (when it already contains markup)
/// or wrapped in a GOV.UK-style paragraph (when it is plain text).
/// </summary>
[HtmlTargetElement(TagName)]
public class HtmlContentRendererTagHelper : TagHelper
{
    private const string TagName = "html-content-renderer";

    // Matches an opening or closing HTML tag, e.g. <p>, </p>, <br/>, <a href="...">
    private static readonly Regex HtmlTagRegex = new(@"<\s*/?\s*[a-zA-Z][a-zA-Z0-9]*(\s[^<>]*)?\s*/?\s*>", RegexOptions.None, TimeSpan.FromMilliseconds(100));
    private static readonly Regex BlockLevelTagRegex = new(@"<\s*/?\s*(p|div|ul|ol|li|h[1-6])\b", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(100));

    [HtmlAttributeName("content")]
    public string? Content { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (string.IsNullOrWhiteSpace(Content))
        {
            output.SuppressOutput();
            return;
        }

        // We don't want the <html-content-renderer> element itself to appear in the
        // output - just whichever element we decide to emit below.
        output.TagName = null;

        var text = Content.Trim();

        if (BlockLevelTagRegex.IsMatch(text))
        {
            // Has <p> tags — restyle them, no wrapping needed
            var styled = Regex.Replace(
                text,
                @"<p(?![\w])",
                "<p class='govuk-body'",
                RegexOptions.IgnoreCase);

            output.Content.SetHtmlContent(styled);
        }
        else if (HtmlTagRegex.IsMatch(text))
        {
            // Has inline markup only e.g. <br> — convert to paragraphs
            output.Content.SetHtmlContent($"<p class='govuk-body govuk-!-margin-bottom-0'>{text}</p>");
        }
        else
        {
            // Plain text - encode defensively (handles stray & < > etc.), then wrap.
            var encoded = HtmlEncoder.Default.Encode(text);
            output.Content.SetHtmlContent($"<p class='govuk-body govuk-!-margin-bottom-0'>{encoded}</p>");
        }
    }
}