using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SFA.DAS.FAA.Web.TagHelpers;

namespace SFA.DAS.FAA.Web.UnitTests.TagHelpers;

public class AccessibleListTagHelperTests
{
    private TagHelperContext _tagHelperContext;
    private TagHelperOutput _tagHelperOutput;
    
    [SetUp]
    public void SetUp()
    {
        _tagHelperContext = new TagHelperContext([], new Dictionary<object, object>(), "id");
        _tagHelperOutput = new TagHelperOutput(string.Empty, [], Func);
        return;

        static Task<TagHelperContent> Func(bool result, HtmlEncoder encoder)
        {
            var tagHelperContent = new DefaultTagHelperContent();
            tagHelperContent.SetHtmlContent(string.Empty);
            return Task.FromResult<TagHelperContent>(tagHelperContent);
        }
    }
    
    [Test]
    public async Task Output_Is_Suppressed_When_Items_Are_Null()
    {
        // arrange
        var sut = new AccessibleListTagHelper();

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.AsString().Should().BeEmpty();
    }
    
    [Test]
    public async Task Renders_Css_For_Single_Item()
    {
        // arrange
        var sut = new AccessibleListTagHelper
        {
            Class = " class1 class2",
            Items = ["item 1"]
        };

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.AsString().Should().Contain("class=\"class1 class2\"");
    }
    
    [Test]
    public async Task Renders_Css_For_Multiple_Items()
    {
        // arrange
        var sut = new AccessibleListTagHelper
        {
            Class = " class1 class2",
            Items = ["item 1", "item 2"]
        };

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.AsString().Should().Contain("class=\"govuk-list govuk-list--bullet class1 class2\"");
    }
    
    [Test]
    public async Task Renders_Div_For_Single_Item()
    {
        // arrange
        var sut = new AccessibleListTagHelper
        {
            Items = ["item 1"]
        };

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.AsString().Should().Be("<div>HtmlEncode[[item 1]]</div>");
    }
    
    [Test]
    public async Task Renders_List_For_Multiple_Items()
    {
        // arrange
        var sut = new AccessibleListTagHelper
        {
            Items = ["item 1", "item 2"]
        };

        // act
        await sut.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // assert
        _tagHelperOutput.AsString().Should().Be("""<ul class="govuk-list govuk-list--bullet"><li>HtmlEncode[[item 1]]</li><li>HtmlEncode[[item 2]]</li></ul>""");
    }
}