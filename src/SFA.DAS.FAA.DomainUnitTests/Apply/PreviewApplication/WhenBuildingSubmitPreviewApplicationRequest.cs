using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.SubmitPreviewApplication;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.PreviewApplication;

public class WhenBuildingSubmitPreviewApplicationRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Built(Guid candidateId, Guid applicationId)
    {
        var actual = new SubmitPreviewApplicationRequest(candidateId, applicationId);

        actual.PostUrl.Should().Be($"applications/{applicationId}/preview?candidateId={candidateId}");
    }
}