using SFA.DAS.FAA.Domain.Applications.GetApplications;

namespace SFA.DAS.FAA.Application.Queries.Applications.GetIndex;

public class GetIndexQueryResult
{
    public List<Application> Applications { get; set; }

    public class Application
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string EmployerName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ClosingDate { get; set; }
    }

    public static implicit operator GetIndexQueryResult(GetApplicationsApiResponse source)
    {
        return new GetIndexQueryResult
        {
            Applications = source.Applications.Select(x => new Application
            {
                Id = x.Id,
                Title = x.Title,
                EmployerName = x.EmployerName,
                CreatedDate = x.CreatedDate,
                ClosingDate = x.ClosingDate,
            }).ToList()
        };
    }
}