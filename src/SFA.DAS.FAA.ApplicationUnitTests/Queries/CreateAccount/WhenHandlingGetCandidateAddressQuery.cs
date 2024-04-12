using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using CreateAccount.GetCandidatePostcode;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.CreateAccount;
public class WhenHandlingGetCandidateAddressQuery
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned(
        GetCandidateAddressQuery query,
        GetCandidateAddressApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        GetCandidateAddressQueryHandler handler)
    {
        var apiRequestUri = new GetCandidateAddressApiRequest(query.CandidateId);

        apiClientMock.Setup(client =>
                client.Get<GetCandidateAddressApiResponse>(
                    It.Is<GetCandidateAddressApiRequest>(c =>
                        c.GetUrl == apiRequestUri.GetUrl)))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.AddressLine1.Should().BeEquivalentTo(apiResponse.AddressLine1);
    }
}
