using SFA.DAS.FAA.Domain.Apply.GetEmploymentLocations;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.GetEmploymentLocations;

[TestFixture]
public class WhenBuildingGetEmploymentLocationsApiRequest
{
    [Test, MoqAutoData]
    public void Then_GetUrl_Is_Built_Correctly(Guid applicationId, Guid candidateId)
    {
        var request = new GetEmploymentLocationsApiRequest(applicationId, candidateId);

        request.GetUrl.Should().BeEquivalentTo($"applications/{applicationId}/employmentLocations?candidateId={candidateId}");
    }
}