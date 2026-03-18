using CreateAccount.GetCandidateDateOfBirth;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.CreateAccount;

public class WhenHandlingGetCandidateDateOfBirthQuery
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned(
        GetCandidateDateOfBirthQuery query,
        GetCandidateDateOfBirthApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        GetCandidateDateOfBirthQueryHandler handler)
    {
        var apiRequestUri = new GetCandidateDateOfBirthApiRequest(query.CandidateId);

        apiClientMock.Setup(client =>
                client.Get<GetCandidateDateOfBirthApiResponse>(
                    It.Is<GetCandidateDateOfBirthApiRequest>(c =>
                        c.GetUrl == apiRequestUri.GetUrl)))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(apiResponse);
    }
}
