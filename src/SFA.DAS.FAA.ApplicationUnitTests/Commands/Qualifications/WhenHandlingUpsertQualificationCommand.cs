using SFA.DAS.FAA.Application.Commands.UpsertQualification;
using SFA.DAS.FAA.Domain.Apply.Qualifications;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.Qualifications;

public class WhenHandlingUpsertQualificationCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_With_Qualification_Data(
        UpsertQualificationCommand command,
        [Frozen] Mock<IApiClient> apiClient,
        UpsertQualificationCommandHandler handler
        )
    {
        await handler.Handle(command, CancellationToken.None);

        apiClient.Verify(x =>
            x.Post(
                It.Is<PostUpsertQualificationsApiRequest>(c => 
                    c.PostUrl.Contains(command.ApplicationId.ToString())
                    && c.PostUrl.Contains(command.ApplicationId.ToString())
                    && c.PostUrl.Contains(command.ApplicationId.ToString())
                    && ((PostUpsertQualificationsApiRequest.PostUpsertQualificationsApiRequestData)c.Data).CandidateId == command.CandidateId
                    && ((PostUpsertQualificationsApiRequest.PostUpsertQualificationsApiRequestData)c.Data).Subjects == command.Subjects
                    )));

    }
}