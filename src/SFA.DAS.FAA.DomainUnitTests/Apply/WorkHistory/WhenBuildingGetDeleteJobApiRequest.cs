using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.WorkHistory;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var actual = new GetDeleteJobApiRequest(applicationId, jobId, candidateId);

            actual.GetUrl.Should().Be($"applications/{applicationId}/jobs/{jobId}/delete?candidateId={candidateId}");
        }
    }
}
