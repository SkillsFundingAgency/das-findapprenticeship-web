using SFA.DAS.FAA.Application.Queries.User.GetCreateAccountInform;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.CreateAccount;
public class WhenHandlingGetInformQuery
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned(
        GetInformQuery query,
        GetInformApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        GetInformQueryHandler handler)
    {
        var apiRequestUri = new GetInformApiRequest(query.CandidateId);

        apiClientMock.Setup(client =>
                client.Get<GetInformApiResponse>(
                    It.Is<GetInformApiRequest>(c =>
                        c.GetUrl == apiRequestUri.GetUrl)))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
    }
}
