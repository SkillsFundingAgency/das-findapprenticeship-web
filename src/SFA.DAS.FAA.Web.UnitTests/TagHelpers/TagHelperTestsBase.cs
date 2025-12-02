using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SFA.DAS.FAA.Web.TagHelpers;

namespace SFA.DAS.FAA.Web.UnitTests.TagHelpers;

public abstract class TagHelperTestsBase
{
    protected TagHelperContext TagHelperContext;
    protected TagHelperOutput TagHelperOutput;
    protected virtual string DefaultContent => "default content";
    
    [SetUp]
    public void SetUp()
    {
        TagHelperContext = new TagHelperContext([], new Dictionary<object, object>(), "id");
        TagHelperOutput = new TagHelperOutput(GovUkBannerTagHelper.TagName, [], Func);
        return;

        Task<TagHelperContent> Func(bool result, HtmlEncoder encoder)
        {
            var tagHelperContent = new DefaultTagHelperContent();
            tagHelperContent.SetHtmlContent(DefaultContent);
            return Task.FromResult<TagHelperContent>(tagHelperContent);
        }
    }
}