using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.TrainingCourses.DeleteTrainingCourse;
using SFA.DAS.FAA.Domain.Apply.DeleteTrainingCourse;
using SFA.DAS.FAA.Domain.Apply.WorkHistory;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteJob;
using SFA.DAS.Testing.AutoFixture;
using SFA.FAA.FindAnApprenticeship.Application.TrainingCourses.DeleteTrainingCourse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            apiClient.Setup(client => client.PostWithResponseCode(It.Is<DeleteTrainingCourseApiRequest>(r => r.PostUrl == expectedRequest.PostUrl)));

            await handler.Handle(command, CancellationToken.None);

            apiClient.Verify(x => x.PostWithResponseCode(It.IsAny<DeleteTrainingCourseApiRequest>()), Times.Once);
        }
    }
}
