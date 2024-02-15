using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.TrainingCourses.AddTrainingCourse;
using SFA.DAS.FAA.Domain.Apply.AddTrainingCourse;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.TrainingCourses;
public class WhenHandlingAddTrainingCourse
{
    [Test, MoqAutoData]
    public async Task Then_The_CommandResult_Is_Returned_As_Expected(
            AddTrainingCourseCommand command,
            Guid apiResponse,
            [Frozen] Mock<IApiClient> apiClient,
            [Greedy] AddTrainingCourseCommandHandler handler)
    {
        var expectedApiRequest =
            new PostTrainingCourseApiRequest(command.ApplicationId, new PostTrainingCourseApiRequest.PostTrainingCourseApiRequestData());

        apiClient.Setup(x =>
                x.PostWithResponseCode<Guid>(
                    It.Is<PostTrainingCourseApiRequest>(r => r.PostUrl == expectedApiRequest.PostUrl
                                    && ((PostTrainingCourseApiRequest.PostTrainingCourseApiRequestData)r.Data).CandidateId == command.CandidateId
                                    && ((PostTrainingCourseApiRequest.PostTrainingCourseApiRequestData)r.Data).CourseName == command.CourseName
                                    && ((PostTrainingCourseApiRequest.PostTrainingCourseApiRequestData)r.Data).YearAchieved == command.YearAchieved
                    )))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        result.Id.Should().Be(apiResponse);
    }
}
