using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.WorkHistory;
using FluentAssertions;



namespace SFA.DAS.FAA.Domain.UnitTests.Apply.WorkHistory
{
    public class WhenBuildingDeleteJobsApiRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(
        Guid applicationId,
        Guid candidateId,
        Guid jobId)
        {
            var actual = new DeleteJobApiRequest(applicationId, candidateId, jobId);

            actual.DeleteUrl.Should().Be($"candidates/{candidateId}/applications/{applicationId}/work-history/{jobId}");
        }
    }
}
