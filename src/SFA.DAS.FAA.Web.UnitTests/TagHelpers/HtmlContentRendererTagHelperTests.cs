using Microsoft.AspNetCore.Razor.TagHelpers;
using SFA.DAS.FAA.Web.TagHelpers;

namespace SFA.DAS.FAA.Web.UnitTests.TagHelpers;

[TestFixture]
internal class HtmlContentRendererTagHelperTests : TagHelperTestsBase
{
    [Test]
    [MoqInlineAutoData("<p>Hello, world!</p>")]
    [MoqInlineAutoData("<p>Hello, world!</p><u>Underline</u><p>Another paragraph</p><b>Bold</b><i>Italic</i></br>")]
    [MoqInlineAutoData("<p>Hello, world!</p><ul><li>item</li><li>Another item</li></ul>")]
    public void ProcessAsync_WhenContentIsHtml_ShouldRenderHtml(string content)
    {
        // Arrange
        var helper = new HtmlContentRendererTagHelper
        {
            Content = content
        };

        var output = new TagHelperOutput("html-content-renderer", [], (s, d) => Task.FromResult<TagHelperContent>(null!));

        // Act
        helper.ProcessAsync(TagHelperContext, output).Wait();

        // Assert
        output.Content.GetContent().Should().Contain($"{content}");
    }

    [Test]
    [MoqInlineAutoData("")]
    [MoqInlineAutoData(null)]
    public void ProcessAsync_WhenContentIsHtml_When_Null_Or_Empty_ShouldRenderHtml(string content)
    {
        // Arrange
        var helper = new HtmlContentRendererTagHelper
        {
            Content = content
        };

        var output = new TagHelperOutput("html-content-renderer", [], (s, d) => Task.FromResult<TagHelperContent>(null!));

        // Act
        helper.ProcessAsync(TagHelperContext, output).Wait();

        // Assert
        output.Content.GetContent().Should().BeNullOrEmpty();
    }

    [Test, MoqAutoData]
    public void ProcessAsync_WhenContentIsPlainText_ShouldRenderWrappedHtml(string content)
    {
        // Arrange
        var helper = new HtmlContentRendererTagHelper
        {
            Content = content
        };

        var output = new TagHelperOutput("html-content-renderer", [], (s, d) => Task.FromResult<TagHelperContent>(null!));
        
        // Act
        helper.ProcessAsync(TagHelperContext, output).Wait();

        // Assert
        output.Content.GetContent().Should().Contain($"<p class='govuk-body govuk-!-margin-bottom-0'>{content}</p>");
    }
}