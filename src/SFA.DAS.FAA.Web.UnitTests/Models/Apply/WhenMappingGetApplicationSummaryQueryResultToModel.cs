using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSummary;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Apply;

public class WhenMappingGetApplicationSummaryQueryResultToModel
{
    [Test, MoqAutoData]
    public void Map_Returns_Expected_Result(GetApplicationSummaryQueryResult source)
    {
        var result = (ApplicationSummaryViewModel)source;

        using (new AssertionScope())
        {
            result.Candidate.Should().BeEquivalentTo(source.Candidate);
            result.WorkHistory.Should().BeEquivalentTo(source.WorkHistory);
            result.EducationHistory.Should().BeEquivalentTo(source.EducationHistory);
            result.InterviewAdjustments.Should().BeEquivalentTo(source.InterviewAdjustments);
            result.ApplicationQuestions.Should().BeEquivalentTo(source.ApplicationQuestions);
            result.IsDisabilityConfident.Should().Be(source.IsDisabilityConfident);
        }
    }
}