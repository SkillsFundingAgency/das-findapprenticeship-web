using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.DeleteQualifications;
using SFA.DAS.FAA.Domain.Apply.Qualifications;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;


namespace SFA.DAS.FAA.Application.UnitTests.Commands.Qualifications
{
    public class WhenHandlingDeleteQualifications
    {
        [Test, MoqAutoData]
          public async Task The_Qualification_Is_Deleted(
          DeleteQualificationsCommand command,
          [Frozen] Mock<IApiClient> apiClient,
          DeleteQualificationsCommandHandler handler)
        {
            var expectedRequest = new PostDeleteQualificationsApiRequest(command.ApplicationId, command.QualificationReferenceId, new PostDeleteQualificationsApiRequest.PostDeleteQualificationsApiRequestBody
            {
                CandidateId = command.CandidateId,
            });

            apiClient.Setup(client => client.Post(It.Is<PostDeleteQualificationsApiRequest>(r => r.PostUrl == expectedRequest.PostUrl)));

            await handler.Handle(command, CancellationToken.None);

            apiClient.Verify(x => x.Post(It.IsAny<PostDeleteQualificationsApiRequest>()), Times.Once);
        }
    }
}
