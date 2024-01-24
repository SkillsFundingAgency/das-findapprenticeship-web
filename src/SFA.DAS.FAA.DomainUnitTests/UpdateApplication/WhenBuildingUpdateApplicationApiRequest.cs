using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication;

namespace SFA.DAS.FAA.Domain.UnitTests.UpdateApplication;

public class WhenBuildingUpdateApplicationApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        string vacancyReference,
        Guid applicationId,
        Guid candidateId)
    {
        var actual = new UpdateApplicationApiRequest(vacancyReference, applicationId, candidateId, null!);

        actual.PostUrl.Should().Be($"/vacancies/{vacancyReference}/apply/{applicationId}/{candidateId}");
    }
}