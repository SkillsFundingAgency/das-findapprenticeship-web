using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.TrainingCourses.DeleteTrainingCourse;
using SFA.DAS.FAA.Domain.Apply.DeleteTrainingCourse;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;


namespace SFA.DAS.FAA.Application.UnitTests.Commands.TrainingCourses
{
    public class WhenHandlingDeleteTrainingCourse
    {
        [Test, MoqAutoData]
          public async Task The_Course_Is_Deleted(
          DeleteTrainingCourseCommand command,
          [Frozen] Mock<IApiClient> apiClient,
          DeleteTrainingCourseCommandHandler handler)
        {
            var expectedRequest = new DeleteTrainingCourseApiRequest(command.ApplicationId, command.TrainingCourseId, new DeleteTrainingCourseApiRequest.DeleteTrainingCourseApiRequestData
            {
                CandidateId = command.CandidateId,
            });

            apiClient.Setup(client => client.Post(It.Is<DeleteTrainingCourseApiRequest>(r => r.PostUrl == expectedRequest.PostUrl)));

            await handler.Handle(command, CancellationToken.None);

            apiClient.Verify(x => x.Post(It.IsAny<DeleteTrainingCourseApiRequest>()), Times.Once);
        }
    }
}
