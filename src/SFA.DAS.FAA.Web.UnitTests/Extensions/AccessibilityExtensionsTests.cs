using SFA.DAS.FAA.Web.Extensions;

namespace SFA.DAS.FAA.Web.UnitTests.Extensions;

public class AccessibilityExtensionsTests
{
    [Test]
    public void MakeSingleItemListsAccessible_Handles_Null()
    {
        // act
        var result = AccessibilityExtensions.MakeSingleItemListsAccessible(null);
        
        // assert
        result.Should().BeNull();
    }
    
    [Test]
    public void MakeSingleItemListsAccessible_Handles_Empty_String()
    {
        // act
        var result = "".MakeSingleItemListsAccessible();
        
        // assert
        result.Should().BeEmpty();
    }
    
    [Test]
    public void MakeSingleItemListsAccessible_Does_Not_Change_String_With_No_Matches()
    {
        // arrange
        var value = """
                    This is some text.
                    Which has no matches.
                    <ul>
                        <li>item 1</li>
                        <li>item 2</li>
                    </ul>
                    """;
        
        // act
        var result = value.MakeSingleItemListsAccessible();
        
        // assert
        result.Should().Be(value);
    }
    
    [Test]
    public void MakeSingleItemListsAccessible_Replaces_Single_Match()
    {
        // arrange
        const string value = """
                                 <ul>
                                     <li>item 1</li>
                                 </ul>
                             """;
        const string expectedResult = 
                            """
                                <p>item 1</p>
                            """;
        
        // act
        var result = value.MakeSingleItemListsAccessible();
        
        // assert
        result.Should().Be(expectedResult);
    }
    
    [Test]
    public void MakeSingleItemListsAccessible_Replaces_Multiple_Matches()
    {
        // arrange
        const string value =
            """
                 <p>This is some paragraph text</p>
                 <ul>
                     <li>item 1</li>
                 </ul>
                 <p>This is some paragraph text</p>
                 <ul>
                     <li>item 1</li>
                     <li>item 2</li>
                 </ul>
                 <p>This is some paragraph text</p>
                 <ul>
                     <li>item 1</li>
                 </ul>
             """;
        const string expectedResult =
            """
                <p>This is some paragraph text</p>
                <p>item 1</p>
                <p>This is some paragraph text</p>
                <ul>
                    <li>item 1</li>
                    <li>item 2</li>
                </ul>
                <p>This is some paragraph text</p>
                <p>item 1</p>
            """;
        
        // act
        var result = value.MakeSingleItemListsAccessible();
        
        // assert
        result.Should().Be(expectedResult);
    }
}