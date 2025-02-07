using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAA.Web.Extensions;

namespace SFA.DAS.FAA.Web.TagHelpers
{
    [HtmlTargetElement(TagName, TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AddressTagHelper : TagHelper
    {
        private const string TagName = "address";
        public bool Anonymised { get; set; } = false;
        public required Address Value { get; set; }
        public bool Flat { get; set; } = false;
        public required string Class { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Value is null)
            {
                output.SuppressOutput();
                return;
            }

            output.TagName = Flat ? "li" : "p";
            output.TagMode = TagMode.StartTagAndEndTag;

            AddCssClasses(output, Class);
            Render(output, Value, Anonymised, Flat);
        }

        private static void Render(TagHelperOutput output, Address address, bool anonymised, bool flat)
        {
            // city (outcode)
            if (anonymised)
            {
                output.Content.AppendHtml(address.ToSingleLineAnonymousAddress());
                return;
            }

            // single line
            if (flat)
            {
                output.Content.AppendHtml(address.ToSingleLineFullAddress());
                return;
            }

            // multi line
            var lines = address.GetPopulatedAddressLines().ToArray();
            var index = 0;
            foreach (var line in lines)
            {
                var br = ++index < lines.Length ? "<br>" : string.Empty;
                output.Content.AppendHtml($"{line}{br}");
            }
        }

        private static void AddCssClasses(TagHelperOutput output, string cssClasses)
        {
            if(string.IsNullOrEmpty(cssClasses)) return;
            var classes = cssClasses.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            foreach (var cssClass in classes)
            {
                output.AddClass(cssClass, HtmlEncoder.Default);
            }
        }
    }
}
