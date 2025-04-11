using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.FAA.Application.Queries.GetSavedVacancies;
using SFA.DAS.FAA.Domain.SavedVacancies;
using SFA.DAS.FAA.Domain.GetApprenticeshipVacancy;

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
            query.DeleteVacancyId = null;
            var apiRequestUri = new GetSavedVacanciesApiRequest(query.CandidateId);

            apiClientMock.Setup(client =>
                    client.Get<GetSavedVacanciesApiResponse>(
                        It.Is<GetSavedVacanciesApiRequest>(c =>
                            c.GetUrl == apiRequestUri.GetUrl)))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.SavedVacancies.Should().BeEquivalentTo(apiResponse.SavedVacancies, options => options.Excluding(x => x.Address).Excluding(x => x.OtherAddresses));
            result.DeletedVacancy.Should().BeNull();

            apiClientMock.Verify(client =>
                client.Get<GetApprenticeshipVacancyApiResponse>(
                    It.Is<GetApprenticeshipVacancyApiRequest>(c =>
                        c.GetUrl == apiRequestUri.GetUrl)), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_VacancyReference_Is_Null_Result_Is_Returned(
            GetSavedVacanciesQuery query,
            GetSavedVacanciesApiResponse apiResponse,
            GetApprenticeshipVacancyApiResponse getApprenticeshipVacancyApiResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            GetSavedVacanciesQueryHandler handler)
        {
            // Arrange
            query.DeleteVacancyId = getApprenticeshipVacancyApiResponse.VacancyReference;
            var apiRequestUri = new GetSavedVacanciesApiRequest(query.CandidateId);

            apiClientMock.Setup(client =>
                    client.Get<GetSavedVacanciesApiResponse>(
                        It.Is<GetSavedVacanciesApiRequest>(c =>
                            c.GetUrl == apiRequestUri.GetUrl)))
                .ReturnsAsync(apiResponse);


            var getApprenticeshipVacancyApiRequest = new GetApprenticeshipVacancyApiRequest(query.DeleteVacancyId, null);

            apiClientMock.Setup(client =>
                    client.Get<GetApprenticeshipVacancyApiResponse>(
                        It.Is<GetApprenticeshipVacancyApiRequest>(c =>
                            c.GetUrl == getApprenticeshipVacancyApiRequest.GetUrl)))
                .ReturnsAsync(getApprenticeshipVacancyApiResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.SavedVacancies.Should().BeEquivalentTo(apiResponse.SavedVacancies, options => options
                .Excluding(x => x.Address)
                .Excluding(x => x.OtherAddresses));
            result.DeletedVacancy.EmployerName.Should().Be(getApprenticeshipVacancyApiResponse.EmployerName);
            result.DeletedVacancy.VacancyTitle.Should().Be(getApprenticeshipVacancyApiResponse.Title);
            result.DeletedVacancy.VacancyId.Should().Be(getApprenticeshipVacancyApiResponse.VacancyReference);
        }
    }
}
