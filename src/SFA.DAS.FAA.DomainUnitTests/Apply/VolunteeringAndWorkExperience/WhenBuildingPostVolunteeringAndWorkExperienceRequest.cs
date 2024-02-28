using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.WorkHistory.AddVolunteeringAndWorkExperience;
using static SFA.DAS.FAA.Domain.Apply.WorkHistory.AddVolunteeringAndWorkExperience.PostVolunteeringAndWorkExperienceRequest;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.VolunteeringAndWorkExperience;

public class WhenBuildingPostVolunteeringAndWorkExperienceRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        Guid applicationId,
        PostVolunteeringAndWorkExperienceApiRequestData data)
    {
        var actual = new PostVolunteeringAndWorkExperienceRequest(applicationId, data);

        actual.PostUrl.Should().Be($"applications/{applicationId}/volunteeringorworkexperience");
    }
}