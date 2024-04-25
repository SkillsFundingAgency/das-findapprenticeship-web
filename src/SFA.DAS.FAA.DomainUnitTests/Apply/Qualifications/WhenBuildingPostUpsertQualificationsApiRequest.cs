using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.Qualifications;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.Qualifications;

public class WhenBuildingPostUpsertQualificationsApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_And_Data_Are_Built_Correctly(Guid applicationId, Guid qualificationReferenceId, PostUpsertQualificationsApiRequest.PostUpsertQualificationsApiRequestData data)
    {
        var actual = new PostUpsertQualificationsApiRequest(applicationId, qualificationReferenceId, data);

        actual.PostUrl.Should().Be($"applications/{applicationId}/qualifications/{qualificationReferenceId}/modify");
        ((PostUpsertQualificationsApiRequest.PostUpsertQualificationsApiRequestData)actual.Data).Should()
            .BeEquivalentTo(data);
    }
}