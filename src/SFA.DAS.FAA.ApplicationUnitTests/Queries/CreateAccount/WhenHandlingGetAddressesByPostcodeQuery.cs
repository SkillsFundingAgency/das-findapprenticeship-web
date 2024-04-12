using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.User.GetAddressesByPostcode;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.CreateAccount;
public class WhenHandlingGetAddressesByPostcodeQuery
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned(
        GetAddressesByPostcodeQuery query,
        GetAddressesByPostcodeApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        GetAddressesByPostcodeQueryHandler handler)
    {
        var apiRequestUri = new GetAddressesByPostcodeApiRequest(query.CandidateId, query.Postcode);

        apiClientMock.Setup(client =>
                client.Get<GetAddressesByPostcodeApiResponse>(
                    It.Is<GetAddressesByPostcodeApiRequest>(c =>
                        c.GetUrl == apiRequestUri.GetUrl)))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Addresses.Count().Should().Be(apiResponse.Addresses.Count());
    }
}
