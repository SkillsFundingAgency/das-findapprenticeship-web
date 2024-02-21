using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetEmployerSkillsAndStrengths;
using SFA.DAS.FAA.Domain.Apply.GetEmployerSkillsAndStrengths;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply;
public class WhenHandlingGetSkillsAndStrengthQuery
{
    [Test, MoqAutoData]
    public async Task Then_Api_Is_Called_And_Query_Response_Returned(
        GetSkillsAndStrengthsQuery query,
        GetSkillsAndStrengthsApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClient,
        [Greedy] GetSkillsAndStrengthsQueryHandler handler)
    {
        apiClient.Setup(x => x.Get<GetSkillsAndStrengthsApiResponse>(It.Is<GetSkillsAndStrengthsApiRequest>
            (x => x.GetUrl.Equals($"applications/{query.ApplicationId}/volunteeringorworkexperience?candidateId={query.CandidateId}"))))
            .ReturnsAsync(apiResponse);

        var actual = await handler.Handle(query, CancellationToken.None);

        using (new AssertionScope())
        {
            apiClient.Invocations.Count.Should().Be(1);
            actual.Should().NotBeNull();
            actual.Should().BeOfType<GetSkillsAndStrengthsQueryResult>();
            actual.Employer.Should().BeEquivalentTo(apiResponse.Employer);
            actual.ExpectedSkillsAndStrengths.Should().BeEquivalentTo(apiResponse.ExpectedSkillsAndStrengths.ToList());
        }
    }
}
