using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using CreateAccount.GetCandidatePhoneNumber;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.CreateAccount;
public class WhenHandlingGetCandidatePhoneNumberQuery
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned(
        GetCandidatePhoneNumberQuery query,
        GetCandidatePhoneNumberApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        GetCandidatePhoneNumberQueryHandler handler)
    {
        var apiRequestUri = new GetCandidatePhoneNumberApiRequest(query.CandidateId);

        apiClientMock.Setup(client =>
                client.Get<GetCandidatePhoneNumberApiResponse>(
                    It.Is<GetCandidatePhoneNumberApiRequest>(c =>
                        c.GetUrl == apiRequestUri.GetUrl)))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.PhoneNumber.Should().BeEquivalentTo(apiResponse.PhoneNumber);
    }
}
