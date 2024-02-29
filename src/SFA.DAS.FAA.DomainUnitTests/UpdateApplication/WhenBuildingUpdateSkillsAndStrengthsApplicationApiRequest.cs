using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication;

namespace SFA.DAS.FAA.Domain.UnitTests.UpdateApplication;
public class WhenBuildingUpdateSkillsAndStrengthsApplicationApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(Guid applicationId)
    {
        var actual = new PostSkillsAndStrengthsApiRequest(applicationId, new PostSkillsAndStrengthsModel());

        actual.PostUrl.Should().Be($"applications/{applicationId}/skillsandstrengths");
    }
}
