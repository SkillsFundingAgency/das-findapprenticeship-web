using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

[HtmlTargetElement("accessible-list", Attributes = "items")]
public class AccessibleListTagHelper : TagHelper
{
    public List<string>? Items { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (Items == null || Items.Count == 0)
        {
            output.SuppressOutput();
            return;
        }

        if (Items.Count == 1)
        {
            output.TagName = "span";
            output.Content.SetContent(Items[0]);
        }
        else
        {
            output.TagName = "ul";
            output.Attributes.SetAttribute("class", "govuk-list govuk-list--bullet");

            var sb = new StringBuilder();
            foreach (var item in Items)
            {
                sb.AppendFormat("<li>{0}</li>", item);
            }

            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}