using SFA.DAS.FAA.Domain.Applications.GetApplications;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.Queries.Applications.GetIndex;

public class GetIndexQueryResult
{
    public List<Application> Applications { get; set; } = [];

    public class Application
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string VacancyReference { get; set; }
        public string EmployerName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime? WithdrawnDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public ApplicationStatus Status { get; set; }
        public string ResponseNotes { get; set; }
        public Address WorkLocation { get; set; } = null!;
        public List<Address> Addresses { get; set; } = [];
        public string? EmploymentLocationInformation { get; set; }
        public AvailableWhere? EmployerLocationOption { get; set; }
    }

    public static implicit operator GetIndexQueryResult(GetApplicationsApiResponse source)
    {
        return new GetIndexQueryResult
        {
            Applications = source.Applications.Select(x => new Application
            {
                Id = x.Id,
                Title = x.Title,
                VacancyReference = x.VacancyReference,
                EmployerName = x.EmployerName,
                CreatedDate = x.CreatedDate,
                ClosingDate = x.ClosingDate,
                ClosedDate = x.ClosedDate,
                SubmittedDate = x.SubmittedDate,
                ResponseDate = x.ResponseDate,
                Status = x.Status,
                ResponseNotes = x.ResponseNotes,
                WithdrawnDate = x.WithdrawnDate,
                WorkLocation = x.Address,
                Addresses = new List<Address> { x.Address }.Concat(x.OtherAddresses).ToList(),
                EmployerLocationOption = x.EmployerLocationOption,
                EmploymentLocationInformation = x.EmploymentLocationInformation,
            }).ToList()
        };
    }
}