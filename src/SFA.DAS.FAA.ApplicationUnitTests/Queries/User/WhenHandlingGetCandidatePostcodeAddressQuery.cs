using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.User.GetCandidatePostcodeAddress;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.User;
public class WhenHandlingGetCandidatePostcodeAddressQuery
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned(
        GetCandidatePostcodeAddressQuery query,
        GetCandidatePostcodeAddressApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        GetCandidatePostcodeAddressQueryHandler handler)
    {
        var apiRequestUri = new GetCandidatePostcodeAddressApiRequest(query.Postcode);

        apiClientMock.Setup(client =>
                client.Get<GetCandidatePostcodeAddressApiResponse>(
                    It.Is<GetCandidatePostcodeAddressApiRequest>(c =>
                        c.GetUrl == apiRequestUri.GetUrl)))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(apiResponse);
    }
}
