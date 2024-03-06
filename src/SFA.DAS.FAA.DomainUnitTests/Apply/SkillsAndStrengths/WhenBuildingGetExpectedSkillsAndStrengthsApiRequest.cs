using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.GetEmployerSkillsAndStrengths;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.SkillsAndStrengths;
public class WhenBuildingGetExpectedSkillsAndStrengthsApiRequest
{
    [Test, MoqAutoData]
    public void Then_GetUrl_Is_Built_Corectly(Guid applicationId, Guid candidateId)
    {
        var request = new GetExpectedSkillsAndStrengthsApiRequest(applicationId, candidateId);

        request.GetUrl.Should().BeEquivalentTo($"applications/{applicationId}/skillsandstrengths/expected?candidateId={candidateId}");
    }
}
