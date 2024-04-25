using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using CreateAccount.GetCandidateName;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.CreateAccount;
public class WhenHandlingGetCandidateNameQuery
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned(
        GetCandidateNameQuery query,
        GetCandidateNameApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        GetCandidateNameQueryHandler handler)
    {
        var apiRequestUri = new GetCandidateNameApiRequest(query.CandidateId);

        apiClientMock.Setup(client =>
                client.Get<GetCandidateNameApiResponse>(
                    It.Is<GetCandidateNameApiRequest>(c =>
                        c.GetUrl == apiRequestUri.GetUrl)))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.FirstName.Should().BeEquivalentTo(apiResponse.FirstName);
        result.LastName.Should().BeEquivalentTo(apiResponse.LastName);
    }
}
