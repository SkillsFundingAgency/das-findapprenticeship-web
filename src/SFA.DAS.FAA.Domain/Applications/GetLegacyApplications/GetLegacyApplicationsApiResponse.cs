using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Domain.Applications.GetLegacyApplications
{
    public class GetLegacyApplicationsApiResponse
    {
        public List<Application> Applications { get; set; } = [];

        public class Application
        {
            public LegacyApplicationStatus Status { get; set; }
        }
    }
}