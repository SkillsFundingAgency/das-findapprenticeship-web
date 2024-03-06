using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.VolunteeringOrWorkExperience;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.VolunteeringOrWorkExperience;
public class WhenBuildingPostDeleteVolunteeringOrWorkExperienceApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        Guid applicationId,
        Guid id,
        PostDeleteVolunteeringOrWorkExperienceApiRequest.PostDeleteVolunteeringOrWorkExperienceApiRequestData data)
    {
        var actual = new PostDeleteVolunteeringOrWorkExperienceApiRequest(applicationId, id, data);

        actual.PostUrl.Should().Be($"applications/{applicationId}/volunteeringorworkexperience/{id}/delete");
    }
}
