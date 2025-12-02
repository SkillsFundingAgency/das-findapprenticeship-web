using SFA.DAS.FAA.Web.TagHelpers;

namespace SFA.DAS.FAA.Web.UnitTests.TagHelpers;

public class FaaTagsTagHelperTests: TagHelperTestsBase
{
    [Test]
    public async Task RaaTags_TagHelper_Renders_Default_Output()
    {
        // arrange
        var sut = new FaaTagsTagHelper();

        // act
        await sut.ProcessAsync(TagHelperContext, TagHelperOutput);

        // assert
        TagHelperOutput.AsString().Should().Be("""<strong class="HtmlEncode[[govuk-tag]]">default content</strong>""");
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
        await sut.ProcessAsync(TagHelperContext, TagHelperOutput);

        // assert
        TagHelperOutput.AsString().Should().Be("""<strong class="govuk-tag class1 class2">default content</strong>""");
    }
    
    [Test]
    public async Task FoundationTag_TagHelper_Renders_Output()
    {
        // arrange
        var sut = new FoundationTagTagHelper();

        // act
        await sut.ProcessAsync(TagHelperContext, TagHelperOutput);

        // assert
        TagHelperOutput.AsString().Should().Be("""<strong class="govuk-tag--pink govuk-tag">Foundation</strong>""");
    }
}