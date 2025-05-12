using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Domain.Apply.UpdateEmploymentLocations
{
    public record PostEmploymentLocationsApiRequest(Guid ApplicationId, PostEmploymentLocationsApiRequest.PostEmploymentLocationsApiRequestData Body) : IPostApiRequest
    {
        public string PostUrl => $"applications/{ApplicationId}/employmentLocations";
        public object Data { get; set; } = Body;

        public class PostEmploymentLocationsApiRequestData
        {
            public Guid CandidateId { get; set; }
            public LocationDto EmployerLocation { get; set; }
            public SectionStatus EmploymentLocationSectionStatus { get; set; } = SectionStatus.NotStarted;
        }
    }
}