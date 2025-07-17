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
                new PostSaveVacancyApiRequest(command.CandidateId, new PostSaveVacancyApiRequestData{ VacancyId = command.VacancyId});

            apiClient.Setup(x =>
                    x.Post<PostSaveVacancyApiResponse>(
                        It.Is<PostSaveVacancyApiRequest>(r => r.PostUrl == expectedApiRequest.PostUrl
                                                              && ((PostSaveVacancyApiRequestData)r.Data).VacancyId == command.VacancyId
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
                new PostSaveVacancyApiRequest(command.CandidateId, new PostSaveVacancyApiRequestData { VacancyId = command.VacancyId });

            apiClient.Setup(x =>
                    x.Post<PostSaveVacancyApiResponse>(
                        It.Is<PostSaveVacancyApiRequest>(r => r.PostUrl == expectedApiRequest.PostUrl
                                                              && ((PostSaveVacancyApiRequestData)r.Data).VacancyId == command.VacancyId 
                        )))
                .ReturnsAsync(() => null);

            var result = await handler.Handle(command, It.IsAny<CancellationToken>());

            result.Id.Should().Be(Guid.Empty);
        }
    }
}