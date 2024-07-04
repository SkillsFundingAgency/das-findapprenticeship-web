using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Domain.User
{
    public class GetMigrateDataTransferApiResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Application> Applications { get; set; } = [];

        public class Application
        {
            public LegacyApplicationStatus Status { get; set; }
        }
    }
}