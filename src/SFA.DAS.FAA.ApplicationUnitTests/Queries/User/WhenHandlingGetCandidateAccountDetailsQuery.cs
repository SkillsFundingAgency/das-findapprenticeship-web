using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.User.GetCandidateAccountDetails;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.User;
public class WhenHandlingGetCandidateAccountDetailsQuery
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned(
        GetCandidateAccountDetailsQuery query,
        GetCandidateAddressApiResponse address,
        GetCandidateNameApiResponse name,
        GetCandidateDateOfBirthApiResponse dateOfBirth,
        GetCandidatePreferencesApiResponse preferences,
        [Frozen] Mock<IApiClient> apiClientMock,
        GetCandidateAccountDetailsQueryHandler handler)
    {
        apiClientMock.Setup(client =>
                client.Get<GetCandidateAddressApiResponse>(
                    It.Is<GetCandidateAddressApiRequest>(c =>
                        c.GetUrl.Contains(query.CandidateId.ToString()))))
            .ReturnsAsync(address);

        apiClientMock.Setup(client =>
                client.Get<GetCandidateNameApiResponse>(
                    It.Is<GetCandidateNameApiRequest>(x => x.GetUrl.Contains(query.CandidateId.ToString()))))
            .ReturnsAsync(name);

        apiClientMock.Setup(client =>
                client.Get<GetCandidateDateOfBirthApiResponse>(
                    It.Is<GetCandidateDateOfBirthApiRequest>(x => x.GetUrl.Contains(query.CandidateId.ToString()))))
            .ReturnsAsync(dateOfBirth);

        apiClientMock.Setup(client =>
        client.Get<GetCandidatePreferencesApiResponse>(
            It.Is<GetCandidatePreferencesApiRequest>(x => x.GetUrl.Contains(query.CandidateId.ToString()))))
            .ReturnsAsync(preferences);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        apiClientMock.Verify(x => x.Get<GetCandidateAddressApiResponse>(It.IsAny<GetCandidateAddressApiRequest>()), Times.Once);
        apiClientMock.Verify(x => x.Get<GetCandidateDateOfBirthApiResponse>(It.IsAny<GetCandidateDateOfBirthApiRequest>()), Times.Once);
        apiClientMock.Verify(x => x.Get<GetCandidateNameApiResponse>(It.IsAny<GetCandidateNameApiRequest>()), Times.Once);
        apiClientMock.Verify(x => x.Get<GetCandidatePreferencesApiResponse>(It.IsAny<GetCandidatePreferencesApiRequest>()), Times.Once);

    }
}
