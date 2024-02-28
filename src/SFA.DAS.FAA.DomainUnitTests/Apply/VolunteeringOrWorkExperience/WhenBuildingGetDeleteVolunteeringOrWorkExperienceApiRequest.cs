using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.VolunteeringOrWorkExperience;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.VolunteeringOrWorkExperience;
public class WhenBuildingGetDeleteVolunteeringOrWorkExperienceApiRequest
{
    [Test, MoqAutoData]
    public void Then_The_Request_Url_Is_Built_Correctly(Guid applicationId, Guid id, Guid candidateId)
    {
        var request = new GetDeleteVolunteeringOrWorkExperienceApiRequest(applicationId, id, candidateId);

        request.GetUrl.Should().Be($"applications/{applicationId}/volunteeringorworkexperience/{id}/delete?candidateId={candidateId}");
    }
}
