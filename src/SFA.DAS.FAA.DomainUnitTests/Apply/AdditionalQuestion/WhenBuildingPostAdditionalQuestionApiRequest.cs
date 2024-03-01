using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.PostAdditionalQuestion;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.AdditionalQuestion;

[TestFixture]
public class WhenBuildingPostAdditionalQuestionApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        Guid applicationId,
        PostAdditionalQuestionApiRequest.PostAdditionalQuestionApiRequestData data)
    {
        var actual = new PostAdditionalQuestionApiRequest(applicationId, data);

        actual.PostUrl.Should().Be($"applications/{applicationId}/additionalquestions");
    }
}