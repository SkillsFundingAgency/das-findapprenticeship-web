using SFA.DAS.FAA.Application.Queries.Applications.Withdraw;
using SFA.DAS.FAA.Domain.Applications.WithdrawApplication;
using SFA.DAS.FAA.Domain.Exceptions;
using SFA.DAS.FAA.Domain.Interfaces;

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
                    c.GetUrl.Contains(query.ApplicationId.ToString()) &&
                    c.GetUrl.Contains(query.CandidateId.ToString()))))
            .ReturnsAsync(apiResponse);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Should().BeEquivalentTo(apiResponse, options => options
            .Excluding(x => x.Address)
            .Excluding(x => x.OtherAddresses)
        );
    }

    [Test, MoqAutoData]
    public async Task Then_An_Exception_Is_Thrown_When_The_Request_Is_Not_Handled(
        GetWithdrawApplicationQuery query,
        GetWithdrawApplicationApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClient,
        GetWithdrawApplicationQueryHandler handler)
    {
        // arrange
        apiClient.Setup(x => x.Get<GetWithdrawApplicationApiResponse>(
                It.Is<GetWithdrawApplicationApiRequest>(c =>
                    c.GetUrl.Contains(query.ApplicationId.ToString()) &&
                    c.GetUrl.Contains(query.CandidateId.ToString()))))!
            .ReturnsAsync((GetWithdrawApplicationApiResponse)null!);

        Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

        // act/assert
        await act.Should().ThrowAsync<ResourceNotFoundException>();
    }
}