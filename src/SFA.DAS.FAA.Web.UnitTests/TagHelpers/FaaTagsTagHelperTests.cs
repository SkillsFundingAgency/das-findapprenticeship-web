using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SFA.DAS.FAA.Web.TagHelpers;

namespace SFA.DAS.FAA.Web.UnitTests.TagHelpers;

public class FaaTagsTagHelperTests
{
    private TagHelperContext _tagHelperContext;
    private TagHelperOutput _tagHelperOutput;

    [SetUp]
    public void SetUp()
    {
        _tagHelperContext = new TagHelperContext([], new Dictionary<object, object>(), "id");
        _tagHelperOutput = new TagHelperOutput(FaaTagsTagHelper.TagName, [], Func);
        return;

        static Task<TagHelperContent> Func(bool result, HtmlEncoder encoder)
        {
            var tagHelperContent = new DefaultTagHelperContent();
            tagHelperContent.SetHtmlContent("default content");
            return Task.FromResult<TagHelperContent>(tagHelperContent);
        }
    }
    
    [Test]
    public async Task RaaTags_TagHelper_Renders_Default_Output()
    {
        // arrange
        var sut = new FaaTagsTagHelper();

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.AsString().Should().Be("""<strong class="HtmlEncode[[govuk-tag]]">default content</strong>""");
    }
    
    [Test]
    public async Task RaaTags_TagHelper_Renders_Css()
    {
        // arrange
        var sut = new FaaTagsTagHelper
        {
            Class = " class1 class2"
        };

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.AsString().Should().Be("""<strong class="govuk-tag class1 class2">default content</strong>""");
    }
    
    [Test]
    public async Task FoundationTag_TagHelper_Renders_Output()
    {
        // arrange
        var sut = new FoundationTagTagHelper();

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.AsString().Should().Be("""<strong class="govuk-tag--pink govuk-tag">Foundation</strong>""");
    }
}