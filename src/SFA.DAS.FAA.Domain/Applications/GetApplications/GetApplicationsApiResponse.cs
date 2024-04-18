namespace SFA.DAS.FAA.Domain.Applications.GetApplications;

public class GetApplicationsApiResponse
{
    public List<Application> Applications { get; set; } = [];

    public class Application
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string EmployerName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ClosingDate { get; set; }
    }
}