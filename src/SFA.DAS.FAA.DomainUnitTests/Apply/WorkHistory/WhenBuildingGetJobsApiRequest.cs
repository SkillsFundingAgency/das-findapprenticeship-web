using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.WorkHistory;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.WorkHistory
{
    public class WhenBuildingGetJobsApiRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(
            Guid applicationId,
            Guid candidateId)
        {
            var actual = new GetJobsApiRequest(applicationId, candidateId);

            actual.GetUrl.Should().Be($"applications/{applicationId}/jobs?candidateId={candidateId}");
        }
    }
}
