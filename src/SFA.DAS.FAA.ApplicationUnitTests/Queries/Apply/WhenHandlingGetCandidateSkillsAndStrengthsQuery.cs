using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetCandidateSkillsAndStrengths;
using SFA.DAS.FAA.Domain.Apply.GetCandidateSkillsAndStrengths;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply;
public class WhenHandlingGetCandidateSkillsAndStrengthsQuery
{
    [Test, MoqAutoData]
    public async Task Then_Api_Is_Called_And_Query_Response_Returned(
        GetCandidateSkillsAndStrengthsQuery query,
        GetCandidateSkillsAndStrengthsApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClient,
        [Greedy] GetCandidateSkillsAndStrengthsQueryHandler handler)
    {
        apiClient.Setup(x => x.Get<GetCandidateSkillsAndStrengthsApiResponse>(It.Is<GetCandidateSkillsAndStrengthsApiRequest>
            (x => x.GetUrl.Equals($"applications/{query.ApplicationId}/skillsandstrengths/candidate?candidateId={query.CandidateId}"))))
            .ReturnsAsync(apiResponse);

        var actual = await handler.Handle(query, CancellationToken.None);

        using (new AssertionScope())
        {
            apiClient.Invocations.Count.Should().Be(1);
            actual.Should().NotBeNull();
            actual.Should().BeOfType<GetCandidateSkillsAndStrengthsQueryResult>();
            actual.Should().BeEquivalentTo(apiResponse.AboutYou);
            actual.SkillsAndStrengths.Should().BeEquivalentTo(apiResponse.AboutYou.SkillsAndStrengths);
        }
    }
}
