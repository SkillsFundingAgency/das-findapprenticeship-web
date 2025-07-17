using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.VolunteeringAndWorkExperience.UpdateVolunteeringAndWorkExperience;
using SFA.DAS.FAA.Domain.Apply.VolunteeringOrWorkExperience;
using SFA.DAS.FAA.Domain.Apply.WorkHistory.AddVolunteeringAndWorkExperience;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.VolunteeringAndWorkExperience
{
    [TestFixture]
    public class WhenHandlingEditVolunteeringAndWorkExperienceJobCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_CommandResult_Is_Returned_As_Expected(
            UpdateVolunteeringAndWorkExperienceCommand command,
            PostVolunteeringAndWorkExperienceResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClient,
            [Greedy] UpdateVolunteeringAndWorkExperienceCommandHandler handler)
        {
            var expectedApiRequest =
                new PostUpdateVolunteeringOrWorkExperienceApiRequest(command.ApplicationId, command.VolunteeringOrWorkExperienceId, new PostUpdateVolunteeringOrWorkExperienceApiRequest.PostUpdateVolunteeringOrWorkExperienceApiRequestData());

            apiClient.Setup(x =>
                    x.Post<PostVolunteeringAndWorkExperienceResponse>(
                        It.Is<PostUpdateVolunteeringOrWorkExperienceApiRequest>(r => r.PostUrl == expectedApiRequest.PostUrl
                                                                             && ((PostVolunteeringAndWorkExperienceRequest.PostVolunteeringAndWorkExperienceApiRequestData)r.Data).CandidateId == command.CandidateId
                                                                             && ((PostVolunteeringAndWorkExperienceRequest.PostVolunteeringAndWorkExperienceApiRequestData)r.Data).Description == command.Description
                                                                             && ((PostVolunteeringAndWorkExperienceRequest.PostVolunteeringAndWorkExperienceApiRequestData)r.Data).CompanyName == command.CompanyName
                                                                             && ((PostVolunteeringAndWorkExperienceRequest.PostVolunteeringAndWorkExperienceApiRequestData)r.Data).StartDate == command.StartDate
                                                                             && ((PostVolunteeringAndWorkExperienceRequest.PostVolunteeringAndWorkExperienceApiRequestData)r.Data).EndDate == command.EndDate
                        )))
                .ReturnsAsync(apiResponse);

            await handler.Handle(command, It.IsAny<CancellationToken>());

            apiClient.Verify(x => x.Post(It.IsAny<IPostApiRequest>()), Times.Once);
        }
    }
}
