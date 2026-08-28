using Microsoft.AspNetCore.Razor.TagHelpers;
using SFA.DAS.FAA.Web.TagHelpers;

namespace SFA.DAS.FAA.Web.UnitTests.TagHelpers;

[TestFixture]
internal class HtmlContentRendererTagHelperTests : TagHelperTestsBase
{
    private static TagHelperOutput CreateOutput() =>
        new("html-content-renderer", [], (s, d) => Task.FromResult<TagHelperContent>(null!));

    // -------------------------------------------------------------------------
    // Null / empty content
    // -------------------------------------------------------------------------

    [Test]
    [MoqInlineAutoData("")]
    [MoqInlineAutoData("   ")]
    [MoqInlineAutoData(null)]
    public void Process_WhenContentIsNullOrWhiteSpace_ShouldSuppressOutput(string? content)
    {
        // Arrange
        var helper = new HtmlContentRendererTagHelper { Content = content };
        var output = CreateOutput();

        // Act
        helper.Process(TagHelperContext, output);

        // Assert
        output.Content.GetContent().Should().BeNullOrEmpty();
    }

    // -------------------------------------------------------------------------
    // Block-level tags — restyle existing <p> tags
    // -------------------------------------------------------------------------

    [Test]
    [MoqInlineAutoData("<div>Hello world</div>")]
    [MoqInlineAutoData("<ul><li>Item one</li></ul>")]
    public void Process_WhenContentHasBlockLevelTagsWithNoPTags_ShouldPassThroughAsIs(string content)
    {
        // Arrange
        var helper = new HtmlContentRendererTagHelper { Content = content };
        var output = CreateOutput();

        // Act
        helper.Process(TagHelperContext, output);

        // Assert
        var result = output.Content.GetContent();
        result.Should().Be(content); // no modification since no <p> tags to restyle
    }

    [Test]
    public void Process_WhenContentHasBlockLevelTags_ShouldNotWrapInExtraParagraph()
    {
        // Arrange
        var helper = new HtmlContentRendererTagHelper { Content = "<p>Hello world</p>" };
        var output = CreateOutput();

        // Act
        helper.Process(TagHelperContext, output);

        // Assert
        var result = output.Content.GetContent();
        result.Should().Be("<p class='govuk-body'>Hello world</p>");
    }

    [Test]
    public void Process_WhenContentHasExistingClassOnPTag_ShouldNotDuplicateClass()
    {
        // Arrange
        var helper = new HtmlContentRendererTagHelper { Content = "<p class='existing'>Hello</p>" };
        var output = CreateOutput();

        // Act
        helper.Process(TagHelperContext, output);

        // Assert
        var result = output.Content.GetContent();
        result.Should().Contain("<p class='govuk-body'");
        result.Should().NotContain("<p class='existing'"); // existing class replaced
    }

    [Test]
    public void Process_WhenContentHasMultiplePTags_ShouldRestyleAllOfThem()
    {
        // Arrange
        var helper = new HtmlContentRendererTagHelper { Content = "<p>First</p><p>Second</p><p>Third</p>" };
        var output = CreateOutput();

        // Act
        helper.Process(TagHelperContext, output);

        // Assert
        var result = output.Content.GetContent();
        result.Should().Contain("<p class='govuk-body'>First</p>");
        result.Should().Contain("<p class='govuk-body'>Second</p>");
        result.Should().Contain("<p class='govuk-body'>Third</p>");
    }

    // -------------------------------------------------------------------------
    // Inline tags only (<br>) — wrap in paragraph
    // -------------------------------------------------------------------------

    [Test]
    [MoqInlineAutoData("Line one<br />Line two")]
    [MoqInlineAutoData("Line one<br>Line two")]
    [MoqInlineAutoData("Line one<br/>Line two")]
    public void Process_WhenContentHasOnlyInlineTags_ShouldWrapInGovukParagraph(string content)
    {
        // Arrange
        var helper = new HtmlContentRendererTagHelper { Content = content };
        var output = CreateOutput();

        // Act
        helper.Process(TagHelperContext, output);

        // Assert
        var result = output.Content.GetContent();
        result.Should().StartWith("<p class='govuk-body govuk-!-margin-bottom-0'>");
        result.Should().EndWith("</p>");
        result.Should().Contain(content);
    }

    [Test]
    public void Process_WhenContentHasBrTags_ShouldPreserveBrTagsInsideParagraph()
    {
        // Arrange
        const string content = "Line one<br />Line two<br />Line three";
        var helper = new HtmlContentRendererTagHelper { Content = content };
        var output = CreateOutput();

        // Act
        helper.Process(TagHelperContext, output);

        // Assert
        var result = output.Content.GetContent();
        result.Should().Be($"<p class='govuk-body govuk-!-margin-bottom-0'>{content}</p>");
    }

    // -------------------------------------------------------------------------
    // Plain text — encode and wrap
    // -------------------------------------------------------------------------

    [Test]
    [MoqAutoData]
    public void Process_WhenContentIsPlainText_ShouldEncodeAndWrapInParagraph(string content)
    {
        // Arrange
        // MoqAutoData generates random strings unlikely to contain HTML
        var helper = new HtmlContentRendererTagHelper { Content = content };
        var output = CreateOutput();

        // Act
        helper.Process(TagHelperContext, output);

        // Assert
        var result = output.Content.GetContent();
        result.Should().StartWith("<p class='govuk-body govuk-!-margin-bottom-0'>");
        result.Should().EndWith("</p>");
    }

    [Test]
    [MoqInlineAutoData("Hello & world", "&amp;")]
    [MoqInlineAutoData("Price > 100", "&gt;")]
    [MoqInlineAutoData("a < b", "&lt;")]
    public void Process_WhenPlainTextContainsSpecialCharacters_ShouldHtmlEncode(string content, string expectedEncoded)
    {
        // Arrange
        var helper = new HtmlContentRendererTagHelper { Content = content };
        var output = CreateOutput();

        // Act
        helper.Process(TagHelperContext, output);

        // Assert
        var result = output.Content.GetContent();
        result.Should().Contain(expectedEncoded);
        result.Should().StartWith("<p class='govuk-body govuk-!-margin-bottom-0'>");
        result.Should().EndWith("</p>");
    }

    [Test]
    public void Process_WhenPlainTextContainsAmpersand_ShouldEncodeIt()
    {
        // Arrange
        var helper = new HtmlContentRendererTagHelper { Content = "Apprenticeship & qualification" };
        var output = CreateOutput();

        // Act
        helper.Process(TagHelperContext, output);

        // Assert
        var result = output.Content.GetContent();
        result.Should().Be("<p class='govuk-body govuk-!-margin-bottom-0'>Apprenticeship &amp; qualification</p>");
    }

    // -------------------------------------------------------------------------
    // Output tag name suppressed
    // -------------------------------------------------------------------------

    [Test]
    public void Process_Always_ShouldNotRenderHtmlContentRendererElement()
    {
        // Arrange
        var helper = new HtmlContentRendererTagHelper { Content = "<p>Hello</p>" };
        var output = CreateOutput();

        // Act
        helper.Process(TagHelperContext, output);

        // Assert
        output.TagName.Should().BeNull();
    }
}