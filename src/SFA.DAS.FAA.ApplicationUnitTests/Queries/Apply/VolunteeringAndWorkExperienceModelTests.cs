using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringAndWorkExperiences;
using SFA.DAS.FAA.Domain.Apply.WorkHistory.SummaryVolunteeringAndWorkExperience;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply;

[TestFixture]
public class VolunteeringAndWorkExperienceModelTests
{
    [Test, MoqAutoData]
    public void Map_Returns_Expected_Result(
        GetVolunteeringAndWorkExperiencesApiResponse.VolunteeringAndWorkExperience source)
    {
        var result = (GetVolunteeringAndWorkExperiencesQueryResult.VolunteeringAndWorkExperience)source;

        using var scope = new AssertionScope();
        result.Id.Should().Be(source.Id);
        result.JobTitle.Should().Be(source.JobTitle);
        result.Description.Should().Be(source.Description);
        result.Employer.Should().Be(source.Employer);
        result.StartDate.Should().Be(source.StartDate);
        result.EndDate.Should().Be(source.EndDate);
    }
}