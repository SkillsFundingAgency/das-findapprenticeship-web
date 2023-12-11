using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.GetApprenticeshipVacancy;
using SFA.DAS.FAA.Domain.GetApprenticeshipVacancy;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries
{
    public class WhenGettingVacancyDetails
    {
        [Test, MoqAutoData]
        public async Task 
            Then_Result_Is_Returned(
            GetApprenticeshipVacancyQuery query,
            GetApprenticeshipVacancyApiResponse expectedResponse,
            [Frozen] Mock<IApiClient> apiClient,
            GetApprenticeshipVacancyQueryHandler handler)
        {
            // Mock the response from the API client
            var expectedGetUrl = new GetApprenticeshipVacancyApiRequest(query.VacancyReference);
            apiClient.Setup(client => client.Get<GetApprenticeshipVacancyApiResponse>(It.Is<GetApprenticeshipVacancyApiRequest>(c => c.GetUrl.Equals(expectedGetUrl.GetUrl))))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                Assert.NotNull(result);
                result.Vacancy.LongDescription.Should().Be(expectedResponse.LongDescription);
                result.Vacancy.OutcomeDescription.Should().Be(expectedResponse.OutcomeDescription);

                result.Vacancy.TrainingDescription.Should().Be(expectedResponse.TrainingDescription);
                result.Vacancy.ThingsToConsider.Should().Be(expectedResponse.ThingsToConsider);
                result.Vacancy.Category.Should().Be(expectedResponse.Category);
                result.Vacancy.CategoryCode.Should().Be(expectedResponse.CategoryCode);
                result.Vacancy.Description.Should().Be(expectedResponse.Description);
                result.Vacancy.FrameworkLarsCode.Should().Be(expectedResponse.FrameworkLarsCode);
                result.Vacancy.HoursPerWeek.Should().Be(expectedResponse.HoursPerWeek);
                result.Vacancy.IsDisabilityConfident.Should().Be(expectedResponse.IsDisabilityConfident);
                result.Vacancy.IsPositiveAboutDisability.Should().Be(expectedResponse.IsPositiveAboutDisability);
                result.Vacancy.IsRecruitVacancy.Should().Be(expectedResponse.IsRecruitVacancy);
                result.Vacancy.Location.Should().Be(expectedResponse.Location);
                result.Vacancy.NumberOfPositions.Should().Be(expectedResponse.NumberOfPositions);
                result.Vacancy.ProviderName.Should().Be(expectedResponse.ProviderName);
                result.Vacancy.StandardLarsCode.Should().Be(expectedResponse.StandardLarsCode);
                result.Vacancy.StartDate.Should().Be(expectedResponse.StartDate);
                result.Vacancy.SubCategory.Should().Be(expectedResponse.SubCategory);
                result.Vacancy.SubCategoryCode.Should().Be(expectedResponse.SubCategoryCode);
                result.Vacancy.Ukprn.Should().Be(expectedResponse.Ukprn);

                result.Vacancy.WageAmountLowerBound.Should().Be(expectedResponse.WageAmountLowerBound);
                result.Vacancy.WageAmountUpperBound.Should().Be(expectedResponse.WageAmountUpperBound);
                result.Vacancy.WageText.Should().Be(expectedResponse.WageText);
                result.Vacancy.WageUnit.Should().Be(expectedResponse.WageUnit);
                result.Vacancy.WorkingWeek.Should().Be(expectedResponse.WorkingWeek);
                result.Vacancy.ExpectedDuration.Should().Be(expectedResponse.ExpectedDuration);
                result.Vacancy.Score.Should().Be(expectedResponse.Score);

                result.Vacancy.EmployerDescription.Should().Be(expectedResponse.EmployerDescription);
                result.Vacancy.EmployerContactName.Should().Be(expectedResponse.EmployerContactName);
                result.Vacancy.EmployerContactEmail.Should().Be(expectedResponse.EmployerContactEmail);
                result.Vacancy.EmployerContactPhone.Should().Be(expectedResponse.EmployerContactPhone);
                result.Vacancy.EmployerWebsiteUrl.Should().Be(expectedResponse.EmployerWebsiteUrl);

                result.Vacancy.VacancyLocationType.Should().Be(expectedResponse.VacancyLocationType);
                result.Vacancy.Skills.Should().BeEquivalentTo(expectedResponse.Skills);
                result.Vacancy.Qualifications.Should().BeEquivalentTo(expectedResponse.Qualifications);

                result.Vacancy.Id.Should().Be(expectedResponse.Id);
                result.Vacancy.AnonymousEmployerName.Should().Be(expectedResponse.AnonymousEmployerName);
                result.Vacancy.ApprenticeshipLevel.Should().Be(expectedResponse.ApprenticeshipLevel);
                result.Vacancy.ClosingDate.Should().Be(expectedResponse.ClosingDate);
                result.Vacancy.EmployerName.Should().Be(expectedResponse.EmployerName);
                result.Vacancy.IsEmployerAnonymous.Should().Be(expectedResponse.IsEmployerAnonymous);
                result.Vacancy.PostedDate.Should().Be(expectedResponse.PostedDate);
                result.Vacancy.Title.Should().Be(expectedResponse.Title);
                result.Vacancy.VacancyReference.Should().Be(expectedResponse.VacancyReference);
                result.Vacancy.CourseTitle.Should().Be(expectedResponse.CourseTitle);
                result.Vacancy.CourseId.Should().Be(expectedResponse.CourseId);
                result.Vacancy.WageAmount.Should().Be(expectedResponse.WageAmount);
                result.Vacancy.WageType.Should().Be(expectedResponse.WageType);
                result.Vacancy.Address.Should().Be(expectedResponse.Address);
                result.Vacancy.Distance.Should().Be(expectedResponse.Distance);
                result.Vacancy.CourseRoute.Should().Be(expectedResponse.CourseRoute);
                result.Vacancy.CourseLevel.Should().Be(expectedResponse.CourseLevel);
            }
        }
    }
}
