using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.GetCandidateSkillsAndStrengths;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.SkillsAndStrengths;
public class WhenBuildingGetCandidateSkillsAndStrengthsApiRequest
{
    [Test, MoqAutoData]
    public void Then_GetUrl_Is_Built_Correctly(Guid applicationId, Guid candidateId)
    {
        var request = new GetCandidateSkillsAndStrengthsApiRequest(candidateId, applicationId);

        request.GetUrl.Should().BeEquivalentTo($"applications/{applicationId}/skillsandstrengths/candidate?candidateId={candidateId}");
    }
}
