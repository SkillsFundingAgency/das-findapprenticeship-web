using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SFA.DAS.FAA.Web.TagHelpers;

[HtmlTargetElement("accessible-list", Attributes = "items")]
public class AccessibleListTagHelper : TagHelper
{
    public List<string>? Items { get; set; }
    public string? Class { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (Items == null || Items.Count == 0)
        {
            output.SuppressOutput();
            return;
        }

        output.TagMode = TagMode.StartTagAndEndTag;
        if (Items is { Count: 1 })
        {
            RenderSpan(output);
            return;
        }

        RenderList(output);
    }

    private void RenderSpan(TagHelperOutput output)
    {
        output.TagName = "div";
        output.Content.Append(Items![0]);
        AddCssClasses(output);
    }
    
    private void RenderList(TagHelperOutput output)
    {
        output.TagName = "ul";
        output.AddClass("govuk-list", HtmlEncoder.Default);
        output.AddClass("govuk-list--bullet", HtmlEncoder.Default);
        AddCssClasses(output);

        foreach (var item in Items!)
        {
            var li = new TagBuilder("li");
            li.InnerHtml.Append(item);
            output.Content.AppendHtml(li);
        }
    }

    private void AddCssClasses(TagHelperOutput output)
    {
        if (string.IsNullOrWhiteSpace(Class))
        {
            return;
        }
        
        var classNames = Class
            .Trim()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Distinct()
            .ToList();
        
        foreach (var className in classNames)
        {
            output.AddClass(className, HtmlEncoder.Default);
        }
    }
}