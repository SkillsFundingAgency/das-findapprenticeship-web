using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetDisabilityConfident;
using SFA.DAS.FAA.Domain.Apply.DisabilityConfident;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply;

[TestFixture]
public class WhenMappingApiResponseWithGetDisabilityConfidentDetailsQueryResult
{
    [Test, MoqAutoData]
    public void Map_Returns_Expected_Result(
        GetDisabilityConfidentDetailsApiResponse source)
    {
        var result = (GetDisabilityConfidentDetailsQueryResult)source;

        using var scope = new AssertionScope();
        result.ApplyUnderDisabilityConfidentScheme.Should().Be(source.ApplyUnderDisabilityConfidentScheme);
        result.IsSectionCompleted.Should().Be(source.IsSectionCompleted);
    }
}