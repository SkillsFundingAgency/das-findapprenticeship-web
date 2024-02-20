using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.VolunteeringOrWorkExperience.DeleteVolunteeringOrWorkExperience;
using SFA.DAS.FAA.Domain.Apply.VolunteeringOrWorkExperience;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.VolunteeringOrWorkExperience;
public class WhenHandlingDeleteVolunteeringOrWorkExperience
{
    [Test, MoqAutoData]
    public async Task The_VolunteeringOrWorkExperienceItem_Is_Deleted(
        DeleteVolunteeringOrWorkExperienceCommand command,
        [Frozen] Mock<IApiClient> apiClient,
        DeleteVolunteeringOrWorkExperienceCommandHandler handler)
    {
        var expectedRequest = new PostDeleteVolunteeringOrWorkExperienceApiRequest(command.ApplicationId, command.Id, new PostDeleteVolunteeringOrWorkExperienceApiRequest.PostDeleteVolunteeringOrWorkExperienceApiRequestData
        {
            CandidateId = command.CandidateId,
        });

        apiClient.Setup(client => client.PostWithResponseCode(It.Is<PostDeleteVolunteeringOrWorkExperienceApiRequest>(r => r.PostUrl == expectedRequest.PostUrl)));

        await handler.Handle(command, CancellationToken.None);

        apiClient.Verify(x => x.PostWithResponseCode(It.IsAny<PostDeleteVolunteeringOrWorkExperienceApiRequest>()), Times.Once);
    }
}
