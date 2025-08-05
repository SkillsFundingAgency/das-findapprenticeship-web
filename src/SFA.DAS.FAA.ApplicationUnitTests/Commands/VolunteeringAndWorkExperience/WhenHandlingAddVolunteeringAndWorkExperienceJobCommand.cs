using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.VolunteeringAndWorkExperience.AddVolunteeringAndWorkExperience;
using SFA.DAS.FAA.Domain.Apply.WorkHistory.AddVolunteeringAndWorkExperience;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.VolunteeringAndWorkExperience
{
    [TestFixture]
    public class WhenHandlingAddVolunteeringAndWorkExperienceJobCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_CommandResult_Is_Returned_As_Expected(
            AddVolunteeringAndWorkExperienceCommand command,
            PostVolunteeringAndWorkExperienceResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClient,
            [Greedy] AddVolunteeringAndWorkExperienceCommandHandler handler)
        {
            var expectedApiRequest =
                new PostVolunteeringAndWorkExperienceRequest(command.ApplicationId, new PostVolunteeringAndWorkExperienceRequest.PostVolunteeringAndWorkExperienceApiRequestData());

            apiClient.Setup(x =>
                    x.Post<PostVolunteeringAndWorkExperienceResponse>(
                        It.Is<PostVolunteeringAndWorkExperienceRequest>(r => r.PostUrl == expectedApiRequest.PostUrl
                                                                             && ((PostVolunteeringAndWorkExperienceRequest.PostVolunteeringAndWorkExperienceApiRequestData)r.Data).CandidateId == command.CandidateId
                                                                             && ((PostVolunteeringAndWorkExperienceRequest.PostVolunteeringAndWorkExperienceApiRequestData)r.Data).Description == command.Description
                                                                             && ((PostVolunteeringAndWorkExperienceRequest.PostVolunteeringAndWorkExperienceApiRequestData)r.Data).CompanyName == command.CompanyName
                                                                             && ((PostVolunteeringAndWorkExperienceRequest.PostVolunteeringAndWorkExperienceApiRequestData)r.Data).StartDate == command.StartDate
                                                                             && ((PostVolunteeringAndWorkExperienceRequest.PostVolunteeringAndWorkExperienceApiRequestData)r.Data).EndDate == command.EndDate
                        )))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(command, It.IsAny<CancellationToken>());

            result.Id.Should().Be(apiResponse.Id);
        }
    }
}
