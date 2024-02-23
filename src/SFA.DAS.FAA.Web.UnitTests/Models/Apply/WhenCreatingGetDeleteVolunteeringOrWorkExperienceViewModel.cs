using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringOrWorkExperienceItem;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Apply;
public class WhenCreatingGetDeleteVolunteeringOrWorkExperienceViewModel
{
    [Test, MoqAutoData]
    public void Map_Returns_Expected_Result(GetVolunteeringOrWorkExperienceItemQueryResult source)
    {
        var result = (DeleteVolunteeringOrWorkExperienceViewModel)source;

        using (new AssertionScope())
        {
            result.VolunteeringWorkExperienceId.Should().Be(source.Id);
            result.Organisation.Should().Be(source.Organisation);
            result.Description.Should().Be(source.Description);
            result.ApplicationId.Should().Be(source.ApplicationId);
            result.Dates.Should().Be($"{source.StartDate:MMMM yyyy} to {source.EndDate:MMMM yyyy}");
        }
    }

    [Test, MoqAutoData]
    public void When_EndDate_Is_Null_Map_Returns_Expected_Result(GetVolunteeringOrWorkExperienceItemQueryResult source)
    {
        source.EndDate = null;
        var result = (DeleteVolunteeringOrWorkExperienceViewModel)source;

        using (new AssertionScope())
        {
            result.VolunteeringWorkExperienceId.Should().Be(source.Id);
            result.Organisation.Should().Be(source.Organisation);
            result.Description.Should().Be(source.Description);
            result.ApplicationId.Should().Be(source.ApplicationId);
            result.Dates.Should().Be($"{source.StartDate:MMMM yyyy} onwards");
        }
    }
}