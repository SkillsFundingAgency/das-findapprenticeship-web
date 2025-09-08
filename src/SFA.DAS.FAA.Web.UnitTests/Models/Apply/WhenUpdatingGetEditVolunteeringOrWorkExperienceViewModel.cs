using SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringOrWorkExperienceItem;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Models.Custom;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Apply;

public class WhenUpdatingGetEditVolunteeringOrWorkExperienceViewModel
{
    [Test, MoqAutoData]
    public void Map_Returns_Expected_Result(GetVolunteeringOrWorkExperienceItemQueryResult source)
    {
        var result = (EditVolunteeringAndWorkExperienceViewModel)source;

        using (new AssertionScope())
        {
            result.VolunteeringOrWorkExperienceId.Should().Be(source.Id);
            result.CompanyName.Should().Be(source.Organisation);
            result.Description.Should().Be(source.Description);
            result.ApplicationId.Should().Be(source.ApplicationId);
            result.IsCurrentRole.Should().BeFalse();
            result.StartDate.Should().BeEquivalentTo(new MonthYearDate(source.StartDate));
            result.EndDate.Should().BeEquivalentTo(new MonthYearDate(source.EndDate));
        }
    }

    [Test, MoqAutoData]
    public void Map_Returns_Expected_Result_When_EndDate_Is_Null(GetVolunteeringOrWorkExperienceItemQueryResult source)
    {
        source.EndDate = null;
        var result = (EditVolunteeringAndWorkExperienceViewModel)source;

        using (new AssertionScope())
        {
            result.VolunteeringOrWorkExperienceId.Should().Be(source.Id);
            result.CompanyName.Should().Be(source.Organisation);
            result.Description.Should().Be(source.Description);
            result.ApplicationId.Should().Be(source.ApplicationId);
            result.IsCurrentRole.Should().BeTrue();
            result.StartDate.Should().BeEquivalentTo(new MonthYearDate(source.StartDate));
        }
    }
}