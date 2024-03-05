using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.CreateSkillsAndStrengths;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.SkillsAndStrengths;
public class WhenBuildingPostSkillsAndStrengthsApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        Guid applicationId,
        PostSkillsAndStrengthsApiRequest.PostCreateSkillsAndStrengthsRequestData data)
    {
        var actual = new PostSkillsAndStrengthsApiRequest(applicationId, data);

        actual.PostUrl.Should().Be($"applications/{applicationId}/skillsandstrengths");
    }
}
