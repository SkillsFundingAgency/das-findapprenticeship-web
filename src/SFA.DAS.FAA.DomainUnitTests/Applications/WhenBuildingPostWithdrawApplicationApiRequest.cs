using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Applications.WithdrawApplication;

namespace SFA.DAS.FAA.Domain.UnitTests.Applications;

public class WhenBuildingPostWithdrawApplicationApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(Guid applicationId, Guid candidateId)
    {
        var actual = new PostWithdrawApplicationApiRequest(applicationId, candidateId);

        actual.PostUrl.Should().Be($"applications/{applicationId}/withdraw?candidateId={candidateId}");
    }
}