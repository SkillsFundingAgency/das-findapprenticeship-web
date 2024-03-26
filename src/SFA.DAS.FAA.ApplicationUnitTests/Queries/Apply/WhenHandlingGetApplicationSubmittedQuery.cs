using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSubmitted;
using SFA.DAS.FAA.Domain.Apply.GetApplicationSubmitted;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply;
public class WhenHandlingGetApplicationSubmittedQuery
{
    [Test, MoqAutoData]
    public async Task Then_Api_Is_Called_And_Query_Response_Returned(
        GetApplicationSubmittedQuery query,
        GetApplicationSubmittedApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClient,
        [Greedy] GetApplicationSubmittedQueryHandler handler)
    {
        apiClient.Setup(x => x.Get<GetApplicationSubmittedApiResponse>(It.Is<GetApplicationSubmittedApiRequest>
            (x => x.GetUrl.Equals($"applications/{query.ApplicationId}/submitted?candidateId={query.CandidateId}"))))
            .ReturnsAsync(apiResponse);

        var actual = await handler.Handle(query, CancellationToken.None);

        using (new AssertionScope())
        {
            apiClient.Invocations.Count.Should().Be(1);
            actual.Should().NotBeNull();
            actual.Should().BeOfType<GetApplicationSubmittedQueryResponse>();
            actual.EmployerName.Should().BeEquivalentTo(apiResponse.EmployerName);
        }
    }
}
