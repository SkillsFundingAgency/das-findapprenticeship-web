using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.VolunteeringOrWorkExperience;
using static SFA.DAS.FAA.Domain.Apply.VolunteeringOrWorkExperience.PostUpdateVolunteeringOrWorkExperienceApiRequest;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.VolunteeringAndWorkExperience;

public class WhenBuildingPostUpdateVolunteeringAndWorkExperienceRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        Guid applicationId,
        Guid id,
        PostUpdateVolunteeringOrWorkExperienceApiRequestData data)
    {
        var actual = new PostUpdateVolunteeringOrWorkExperienceApiRequest(applicationId, id, data);

        actual.PostUrl.Should().Be($"applications/{applicationId}/volunteeringorworkexperience/{id}");
    }
}