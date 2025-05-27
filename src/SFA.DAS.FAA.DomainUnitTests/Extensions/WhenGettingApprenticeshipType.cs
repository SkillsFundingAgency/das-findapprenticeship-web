using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Extensions;

namespace SFA.DAS.FAA.Domain.UnitTests.Extensions;

public class WhenGettingApprenticeshipType
{
    [TestCase(ApprenticeshipTypes.Foundation, true)]
    [TestCase(ApprenticeshipTypes.Standard, false)]
    public void Then_IsFoundation_Returns_Correct_Value(ApprenticeshipTypes apprenticeshipType, bool expectedValue)
    {
        // act
        var result = apprenticeshipType.IsFoundation();

        // assert
        result.Should().Be(expectedValue);
    }
}