using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.WorkHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.TrainingCourses
{
    public class WhenBuildingDeleteTrainingCourseApiRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(
            Guid applicationId,
            Guid jobId,
            Guid candidateId)
        {
            var actual = new GetDeleteJobApiRequest(applicationId, candidateId, jobId);

            actual.GetUrl.Should().Be($"applications/{applicationId}/jobs/{jobId}/delete?candidateId={candidateId}");
        }
    }
}
