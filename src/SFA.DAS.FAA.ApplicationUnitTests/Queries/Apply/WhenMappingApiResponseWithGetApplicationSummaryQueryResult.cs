using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSummary;
using SFA.DAS.FAA.Domain.Apply.GetApplicationSummary;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply;

[TestFixture]
public class WhenMappingApiResponseWithGetApplicationSummaryQueryResult
{
    [Test, MoqAutoData]
    public void Map_Returns_Expected_Result(
        GetApplicationSummaryApiResponse source)
    {
        var result = (GetApplicationSummaryQueryResult)source;

        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(source);
    }
}