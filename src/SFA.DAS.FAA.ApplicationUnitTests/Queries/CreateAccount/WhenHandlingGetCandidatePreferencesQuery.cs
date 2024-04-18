using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using CreateAccount.GetCandidatePreferences;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.CreateAccount;
public class WhenHandlingGetCandidatePreferencesQuery
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned(
        GetCandidatePreferencesQuery query,
        GetCandidatePreferencesApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        GetCandidatePreferencesQueryHandler handler)
    {
        var apiRequestUri = new GetCandidatePreferencesApiRequest(query.CandidateId);

        apiClientMock.Setup(client =>
                client.Get<GetCandidatePreferencesApiResponse>(
                    It.Is<GetCandidatePreferencesApiRequest>(c =>
                        c.GetUrl == apiRequestUri.GetUrl)))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.CandidatePreferences.Count.Should().Be(apiResponse.CandidatePreferences.Count);
    }
}
