using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.WorkHistory;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.WorkHistory
{
    public class WhenBuildingGetDeleteJobApiRequest
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
