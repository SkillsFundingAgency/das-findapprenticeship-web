using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.WorkHistory;
using SFA.DAS.FAA.Domain.Apply.WorkHistory.SummaryVolunteeringAndWorkExperience;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.VolunteeringAndWorkExperience;

public class WhenBuildingGetVolunteeringAndWorkExperiencesApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        Guid applicationId,
        Guid candidateId)
    {
        var actual = new GetVolunteeringAndWorkExperiencesApiRequest(applicationId, candidateId);

        actual.GetUrl.Should().Be($"applications/{applicationId}/workExperiences?candidateId={candidateId}");
    }
}