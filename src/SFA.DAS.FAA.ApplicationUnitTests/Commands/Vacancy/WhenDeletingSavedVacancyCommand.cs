﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.Vacancy.DeleteSavedVacancy;
using SFA.DAS.FAA.Domain;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SavedVacancies;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.Vacancy
{
    [TestFixture]
    public class WhenDeletingSavedVacancyCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_CommandResult_Is_Returned_As_Expected(
            DeleteSavedVacancyCommand command,
            [Frozen] Mock<IApiClient> apiClient,
            [Greedy] DeleteSavedVacancyCommandHandler handler)
        {
            var expectedApiRequest =
                new PostDeleteSavedVacancyApiRequest(command.CandidateId, new PostDeleteSavedVacancyApiRequestData { VacancyId = command.VacancyId, DeleteAllByVacancyReference = command.DeleteAllByVacancyReference });

            apiClient.Setup(x =>
                    x.PostWithResponseCode(
                        It.Is<PostDeleteSavedVacancyApiRequest>(r => r.PostUrl == expectedApiRequest.PostUrl
                                                                     && ((PostDeleteSavedVacancyApiRequestData)r.Data).VacancyId == command.VacancyId
                                                                     && ((PostDeleteSavedVacancyApiRequestData)r.Data).DeleteAllByVacancyReference == command.DeleteAllByVacancyReference
                        ))).Returns(Task.CompletedTask);

            var result = await handler.Handle(command, It.IsAny<CancellationToken>());

            result.Should().Be(new Unit());
            apiClient.Verify(x =>
                    x.PostWithResponseCode(It.Is<PostDeleteSavedVacancyApiRequest>(r => r.PostUrl == expectedApiRequest.PostUrl
                                                                    && ((PostDeleteSavedVacancyApiRequestData)r.Data).VacancyId == command.VacancyId
                                                                    && ((PostDeleteSavedVacancyApiRequestData)r.Data).DeleteAllByVacancyReference == command.DeleteAllByVacancyReference
                                                                    )), Times.Once);
        }
    }
}