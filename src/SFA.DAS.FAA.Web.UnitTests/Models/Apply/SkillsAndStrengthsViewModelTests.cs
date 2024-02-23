using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetExpectedSkillsAndStrengths;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Apply;
public class SkillsAndStrengthsViewModelTests
{
    [Test, MoqAutoData]
    public void WhenMappingFromQueryResult_ThenFieldsAreMappedCorrectly(GetExpectedSkillsAndStrengthsQueryResult queryResult)
    {
        var viewModel = (SkillsAndStrengthsViewModel)queryResult;

        using (new AssertionScope())
        {
            viewModel.Employer.Should().BeEquivalentTo(queryResult.Employer);
            viewModel.ApplicationId.Should().Be(queryResult.ApplicationId);
            viewModel.ExpectedSkillsAndStrengths.Should().BeEquivalentTo(queryResult.ExpectedSkillsAndStrengths.ToList());
        }
    }
}
