using SFA.DAS.FAA.Domain.Extensions;

namespace SFA.DAS.FAA.Domain.UnitTests.Extensions;

public class WhenGettingGdsDateString
{
    [Test]
    public void Then_The_String_Is_Formatted_Properly()
    {
        // arrange
        var date = new DateTime(2025, 1, 9);
        
        // act
        var result = date.ToGdsDateString();
        
        // assert
        result.Should().Be("9 January 2025");
    }
    
    [Test]
    public void Then_The_String_Is_Formatted_With_The_Day_Of_The_Week_Properly()
    {
        // arrange
        var date = new DateTime(2025, 1, 9);
        
        // act
        var result = date.ToGdsDateStringWithDayOfWeek();
        
        // assert
        result.Should().Be("Thursday 9 January 2025");
    }
}