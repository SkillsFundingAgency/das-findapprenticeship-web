using SFA.DAS.FAA.Web.TagHelpers;

namespace SFA.DAS.FAA.Web.UnitTests.TagHelpers;

public class AccessibleListTagHelperTests: TagHelperTestsBase
{
    [Test]
    public async Task Output_Is_Suppressed_When_Items_Are_Null()
    {
        // arrange
        var sut = new AccessibleListTagHelper();

        // act
        await sut.ProcessAsync(TagHelperContext, TagHelperOutput);

        // assert
        TagHelperOutput.AsString().Should().BeEmpty();
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
        await sut.ProcessAsync(TagHelperContext, TagHelperOutput);

        // assert
        TagHelperOutput.AsString().Should().Contain("class=\"class1 class2\"");
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
        await sut.ProcessAsync(TagHelperContext, TagHelperOutput);

        // assert
        TagHelperOutput.AsString().Should().Contain("class=\"govuk-list govuk-list--bullet class1 class2\"");
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
        await sut.ProcessAsync(TagHelperContext, TagHelperOutput);

        // assert
        TagHelperOutput.AsString().Should().Be("<div>HtmlEncode[[item 1]]</div>");
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
        await sut.ProcessAsync(TagHelperContext, TagHelperOutput);

        // assert
        TagHelperOutput.AsString().Should().Be("""<ul class="govuk-list govuk-list--bullet"><li>HtmlEncode[[item 1]]</li><li>HtmlEncode[[item 2]]</li></ul>""");
    }
}