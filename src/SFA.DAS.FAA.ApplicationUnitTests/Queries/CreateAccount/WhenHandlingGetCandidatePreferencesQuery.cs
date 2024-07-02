using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.User.GetCandidatePreferences;
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
        apiResponse.CandidatePreferences =
        [
            new GetCandidatePreferencesApiResponse.CandidatePreference
            {
                PreferenceMeaning = Constants.Constants.CandidatePreferences.ContactVacancyClosingMeaning,
                ContactMethodsAndStatus = new List<GetCandidatePreferencesApiResponse.ContactMethodStatus>
                {
                    new()
                    {
                        Status = true,
                        ContactMethod = Constants.Constants.CandidatePreferences.ContactMethodEmail
                    }
                }
            }
        ];
        var apiRequestUri = new GetCandidatePreferencesApiRequest(query.CandidateId);

        apiClientMock.Setup(client =>
                client.Get<GetCandidatePreferencesApiResponse>(
                    It.Is<GetCandidatePreferencesApiRequest>(c =>
                        c.GetUrl == apiRequestUri.GetUrl)))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.CandidatePreferences.Count.Should().Be(apiResponse.CandidatePreferences.Count);
        result.UnfinishedApplicationReminders.Should().BeTrue();
    }

    [Test, MoqAutoData]
    public async Task Then_Result_Is_Preference_Return_False(
        GetCandidatePreferencesQuery query,
        GetCandidatePreferencesApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        GetCandidatePreferencesQueryHandler handler)
    {
        apiResponse.CandidatePreferences =
        [
            new GetCandidatePreferencesApiResponse.CandidatePreference
            {
                PreferenceMeaning = Constants.Constants.CandidatePreferences.ContactVacancyClosingMeaning,
                ContactMethodsAndStatus = new List<GetCandidatePreferencesApiResponse.ContactMethodStatus>
                {
                    new()
                    {
                        Status = false,
                        ContactMethod = Constants.Constants.CandidatePreferences.ContactMethodEmail
                    }
                }
            }
        ];
        var apiRequestUri = new GetCandidatePreferencesApiRequest(query.CandidateId);

        apiClientMock.Setup(client =>
                client.Get<GetCandidatePreferencesApiResponse>(
                    It.Is<GetCandidatePreferencesApiRequest>(c =>
                        c.GetUrl == apiRequestUri.GetUrl)))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.CandidatePreferences.Count.Should().Be(apiResponse.CandidatePreferences.Count);
        result.UnfinishedApplicationReminders.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public async Task Then_Result_Is_Preference_Return_Null(
        GetCandidatePreferencesQuery query,
        GetCandidatePreferencesApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        GetCandidatePreferencesQueryHandler handler)
    {
        apiResponse.CandidatePreferences = [];
        var apiRequestUri = new GetCandidatePreferencesApiRequest(query.CandidateId);

        apiClientMock.Setup(client =>
                client.Get<GetCandidatePreferencesApiResponse>(
                    It.Is<GetCandidatePreferencesApiRequest>(c =>
                        c.GetUrl == apiRequestUri.GetUrl)))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.CandidatePreferences.Count.Should().Be(0);
        result.UnfinishedApplicationReminders.Should().BeFalse();
    }
}
