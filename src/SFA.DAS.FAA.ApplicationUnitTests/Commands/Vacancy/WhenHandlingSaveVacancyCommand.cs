using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.Vacancy.SaveVacancy;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SavedVacancies;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.Vacancy
{
    [TestFixture]
    public class WhenHandlingSaveVacancyCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_CommandResult_Is_Returned_As_Expected(
            SaveVacancyCommand command,
            PostSaveVacancyApiResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClient,
            [Greedy] SaveVacancyCommandHandler handler)
        {
            var expectedApiRequest =
                new PostSaveVacancyApiRequest(command.CandidateId, new PostSaveVacancyApiRequestData{ VacancyReference = command.VacancyReference });

            apiClient.Setup(x =>
                    x.PostWithResponseCode<PostSaveVacancyApiResponse>(
                        It.Is<PostSaveVacancyApiRequest>(r => r.PostUrl == expectedApiRequest.PostUrl
                                                              && ((PostSaveVacancyApiRequestData)r.Data).VacancyReference == command.VacancyReference
                        )))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(command, It.IsAny<CancellationToken>());

            result.Id.Should().Be(apiResponse.Id);
        }

        [Test, MoqAutoData]
        public async Task Then_The_CommandResult_Null_Returned_As_Expected(
            SaveVacancyCommand command,
            [Frozen] Mock<IApiClient> apiClient,
            [Greedy] SaveVacancyCommandHandler handler)
        {
            var expectedApiRequest =
                new PostSaveVacancyApiRequest(command.CandidateId, new PostSaveVacancyApiRequestData { VacancyReference = command.VacancyReference });

            apiClient.Setup(x =>
                    x.PostWithResponseCode<PostSaveVacancyApiResponse>(
                        It.Is<PostSaveVacancyApiRequest>(r => r.PostUrl == expectedApiRequest.PostUrl
                                                              && ((PostSaveVacancyApiRequestData)r.Data).VacancyReference == command.VacancyReference
                        )))
                .ReturnsAsync(() => null);

            var result = await handler.Handle(command, It.IsAny<CancellationToken>());

            result.Id.Should().Be(Guid.Empty);
        }
    }
}