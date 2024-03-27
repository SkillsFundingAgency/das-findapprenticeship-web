using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.GetAdditionalQuestion;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.AdditionalQuestion;

public class WhenBuildingGetAdditionalQuestionApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        Guid applicationId,
        Guid candidateId,
        Guid id,
        int additionalQuestion)
    {
        var actual = new GetAdditionalQuestionApiRequest(applicationId, candidateId, id, additionalQuestion);

        actual.GetUrl.Should().Be($"applications/{applicationId}/additionalquestions?candidateId={candidateId}&id={id}&additionalQuestion={additionalQuestion}");
    }
}