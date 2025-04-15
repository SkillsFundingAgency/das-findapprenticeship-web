using AutoFixture.NUnit3;
using SFA.DAS.FAA.Domain.Apply.UpdateEmploymentLocations;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.GetEmploymentLocations
{
    [TestFixture]
    public class WhenBuildingPostUpdateEmploymentLocationsApiRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(
            Guid applicationId,
            PostEmploymentLocationsApiRequest.PostEmploymentLocationsApiRequestData data)
        {
            var actual = new PostEmploymentLocationsApiRequest(applicationId, data);

            actual.PostUrl.Should().Be($"applications/{applicationId}/employmentLocations");
        }
    }
}