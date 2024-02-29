using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringAndWorkExperiences;
using SFA.DAS.FAA.Domain.Apply.WorkHistory.SummaryVolunteeringAndWorkExperience;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply;

[TestFixture]
public class WhenHandlingGetVolunteeringAndWorkExperiencesQuery
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned(
        GetVolunteeringAndWorkExperiencesQuery query,
        GetVolunteeringAndWorkExperiencesApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        GetVolunteeringAndWorkExperiencesQueryHandler handler)
    {
        // Arrange
        var apiRequestUri = new GetVolunteeringAndWorkExperiencesApiRequest(query.ApplicationId, query.CandidateId);

        apiClientMock.Setup(client =>
                client.Get<GetVolunteeringAndWorkExperiencesApiResponse>(
                    It.Is<GetVolunteeringAndWorkExperiencesApiRequest>(c =>
                        c.GetUrl == apiRequestUri.GetUrl)))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(apiResponse);
    }
}