using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetInterviewAdjustments;
using SFA.DAS.FAA.Domain.Apply.GetInterviewAdjustments;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply;

[TestFixture]
public class WhenMappingApiResponseWithGetInterviewAdjustmentsQueryResult
{
    [Test, MoqAutoData]
    public void Map_Returns_Expected_Result(
        GetInterviewAdjustmentsApiResponse source)
    {
        var result = (GetInterviewAdjustmentsQueryResult)source;

        using var scope = new AssertionScope();
        result.InterviewAdjustmentsDescription.Should().Be(source.InterviewAdjustmentsDescription);
        result.Status.Should().Be(source.Status);
    }
}