using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.TrainingCourses.UpdateTrainingCourse;
using SFA.DAS.FAA.Domain.Apply.UpdateTrainingCourse;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.TrainingCourses;
public class WhenHandlingUpdateTrainingCourseCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_CommandResult_Is_Returned_As_Expected(
        UpdateTrainingCourseCommand command,
        [Frozen] Mock<IApiClient> apiClient,
        [Greedy] UpdateTrainingCourseCommandHandler handler)
    {
        var expectedApiRequest =
            new PostUpdateTrainingCourseApiRequest(command.ApplicationId, command.TrainingCourseId, new PostUpdateTrainingCourseApiRequest.PostUpdateTrainingCourseRequestData());

        apiClient.Setup(x =>
                x.Post(
                    It.Is<PostUpdateTrainingCourseApiRequest>(r => r.PostUrl == expectedApiRequest.PostUrl
                                    && ((PostUpdateTrainingCourseApiRequest.PostUpdateTrainingCourseRequestData)r.Data).CandidateId == command.CandidateId
                                    && ((PostUpdateTrainingCourseApiRequest.PostUpdateTrainingCourseRequestData)r.Data).CourseName == command.CourseName
                                    && ((PostUpdateTrainingCourseApiRequest.PostUpdateTrainingCourseRequestData)r.Data).YearAchieved == command.YearAchieved
                    )))
            .Returns(() => Task.CompletedTask);

        await handler.Handle(command, It.IsAny<CancellationToken>());

        apiClient.Verify(x => x.Post(It.IsAny<IPostApiRequest>()), Times.Once);
    }
}
