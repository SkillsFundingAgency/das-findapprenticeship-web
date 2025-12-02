using SFA.DAS.FAA.Web.TagHelpers;

namespace SFA.DAS.FAA.Web.UnitTests.TagHelpers;

public class GovUkBannerTagHelperTests: TagHelperTestsBase
{
    [TestCase(BannerStyle.Success, "Success")]
    [TestCase(BannerStyle.Important, "Important")]
    public async Task Output_Contains_Default_Header_For_Type(BannerStyle bannerStyle, string title)
    {
        // arrange
        var sut = new GovUkBannerTagHelper
        {
            Style = bannerStyle
        };

        // act
        await sut.ProcessAsync(TagHelperContext, TagHelperOutput);

        // assert
        TagHelperOutput.AsString().Should().Contain($">{title}<");
    }
    
    [TestCase(BannerStyle.Success, "New Title")]
    public async Task Output_Contains_Overridden_Header_For_Type(BannerStyle bannerStyle, string title)
    {
        // arrange
        var sut = new GovUkBannerTagHelper
        {
            Title = title,
            Style = bannerStyle
        };

        // act
        await sut.ProcessAsync(TagHelperContext, TagHelperOutput);

        // assert
        TagHelperOutput.AsString().Should().Contain($">{title}<");
    }
    
    [Test]
    public async Task Output_Contains_Default_Content()
    {
        // arrange
        var sut = new GovUkBannerTagHelper { Style = BannerStyle.Success };

        // act
        await sut.ProcessAsync(TagHelperContext, TagHelperOutput);

        // assert
        TagHelperOutput.AsString().Should().Contain(">default content<");
    }
    
    [Test]
    public async Task Banners_Role_Is_Alert()
    {
        // arrange
        var sut = new GovUkBannerTagHelper { Style = BannerStyle.Success };

        // act
        await sut.ProcessAsync(TagHelperContext, TagHelperOutput);

        // assert
        TagHelperOutput.AsString().Should().Contain("role=\"HtmlEncode[[alert]]\"");
    }
}