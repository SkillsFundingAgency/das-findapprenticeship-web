using SFA.DAS.FAA.Domain.Apply.WorkHistory;
using static SFA.DAS.FAA.Domain.Apply.WorkHistory.PostDeleteJobApiRequest;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.WorkHistory;

public class WhenBuildingPostDeleteJobsApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        Guid applicationId,
        Guid jobId,
        PostDeleteJobApiRequestData data)
    {
        var actual = new PostDeleteJobApiRequest(applicationId, jobId, data);

        actual.PostUrl.Should().Be($"applications/{applicationId}/jobs/{jobId}/delete");
    }
}