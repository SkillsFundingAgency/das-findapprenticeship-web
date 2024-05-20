using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Applications.Withdraw;
using SFA.DAS.FAA.Domain.Applications.WithdrawApplication;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Applications;

public class WhenHandlingGetWithdrawApplicationQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Request_Is_Handled_And_Response_Returned(
        GetWithdrawApplicationQuery query,
        GetWithdrawApplicationApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClient,
        GetWithdrawApplicationQueryHandler handler)
    {
        apiClient.Setup(x => x.Get<GetWithdrawApplicationApiResponse>(
                It.Is<GetWithdrawApplicationApiRequest>(c =>
                c.GetUrl.Contains(query.ApplicationId.ToString()) && c.GetUrl.Contains(query.CandidateId.ToString()))))
            .ReturnsAsync(apiResponse);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Should().BeEquivalentTo(apiResponse);
    }
}