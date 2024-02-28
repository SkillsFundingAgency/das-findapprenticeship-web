using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.DeleteTrainingCourse;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.TrainingCourses
{
    public class WhenBuildingDeleteTrainingCourseApiRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(
            Guid applicationId,
            Guid trainingCourseId,
            DeleteTrainingCourseApiRequest.DeleteTrainingCourseApiRequestData data)
        {
            var actual = new DeleteTrainingCourseApiRequest(applicationId, trainingCourseId, data);

            actual.PostUrl.Should().Be($"applications/{applicationId}/trainingcourses/{trainingCourseId}/delete");
        }

    }
}
