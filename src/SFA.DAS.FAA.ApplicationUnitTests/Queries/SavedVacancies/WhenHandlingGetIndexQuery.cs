using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.FAA.Application.Queries.GetSavedVacancies;
using SFA.DAS.FAA.Domain.SavedVacancies;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.SavedVacancies
{
    public class WhenHandlingGetIndexQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Result_Is_Returned(
            GetSavedVacanciesQuery query,
            GetSavedVacanciesApiResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            GetSavedVacanciesQueryHandler handler)
        {
            // Arrange
            var apiRequestUri = new GetSavedVacanciesApiRequest(query.CandidateId);

            apiClientMock.Setup(client =>
                    client.Get<GetSavedVacanciesApiResponse>(
                        It.Is<GetSavedVacanciesApiRequest>(c =>
                            c.GetUrl == apiRequestUri.GetUrl)))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.SavedVacancies.Should().BeEquivalentTo(apiResponse.SavedVacancies);
        }
    }
}
