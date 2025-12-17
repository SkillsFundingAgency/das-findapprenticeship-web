using SFA.DAS.FAA.Web.TagHelpers;

namespace SFA.DAS.FAA.Web.UnitTests.TagHelpers;

public class GovUkBannerHeadingTagHelperTests: TagHelperTestsBase
{
    [Test]
    public async Task Output_Renders_Correctly()
    {
        // arrange
        var sut = new GovUkBannerHeadingTagHelper();

        // act
        await sut.ProcessAsync(TagHelperContext, TagHelperOutput);

        // assert
        TagHelperOutput.AsString().Should().Be("<h3 class=\"HtmlEncode[[govuk-notification-banner__heading]]\">default content</h3>");
    }
}