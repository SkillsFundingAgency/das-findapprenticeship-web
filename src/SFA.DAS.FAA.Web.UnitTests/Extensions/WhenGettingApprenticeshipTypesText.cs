using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Extensions;

namespace SFA.DAS.FAA.Web.UnitTests.Extensions;

public class WhenGettingApprenticeshipTypesText
{
    [TestCase(ApprenticeshipTypes.Standard, "Apprenticeship")]
    [TestCase(ApprenticeshipTypes.Foundation, "Foundation apprenticeship")]
    public void GetDisplayText_Returns_The_Correct_Texts(ApprenticeshipTypes apprenticeshipType, string expectedText)
    {
        // act
        var result = apprenticeshipType.GetDisplayText();

        // assert
        result.Should().Be(expectedText);
    }
    
    [Test]
    public void GetDisplayTexts_Returns_The_Correct_Texts()
    {
        // arrange
        List<ApprenticeshipTypes> list = [
            ApprenticeshipTypes.Standard,
            ApprenticeshipTypes.Foundation,
        ]; 
        
        // act
        var result = list.GetDisplayTexts();

        // assert
        result.Should().BeEquivalentTo(new List<string> { "Apprenticeship", "Foundation apprenticeship" });
    }
    
    [Test]
    public void GetDisplayTexts_Handles_Null_List()
    {
        // act
        var result = ApprenticeshipTypesExtensions.GetDisplayTexts(null);

        // assert
        result.Should().BeNull();
    }
}