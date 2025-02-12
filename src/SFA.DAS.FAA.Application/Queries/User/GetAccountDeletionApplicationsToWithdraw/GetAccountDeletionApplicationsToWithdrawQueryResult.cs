using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.Queries.User.GetAccountDeletionApplicationsToWithdraw
{
    public record GetAccountDeletionApplicationsToWithdrawQueryResult
    {
        public List<Application> SubmittedApplications { get; set; } = [];

        public static implicit operator GetAccountDeletionApplicationsToWithdrawQueryResult(GetAccountDeletionApplicationsToWithdrawApiResponse source)
        {
            return new GetAccountDeletionApplicationsToWithdrawQueryResult
            {
                SubmittedApplications = source.SubmittedApplications.Select(app => (Application)app).ToList()
            };
        }

        public class Application
        {
            public Guid Id { get; set; }
            public string? Title { get; set; }
            public string? VacancyReference { get; set; }
            public string? EmployerName { get; set; }
            public Address WorkLocation { get; set; } = null!;
            public List<Address> Addresses { get; set; } = [];
            public string? EmploymentLocationInformation { get; set; }
            public AvailableWhere? EmploymentLocationOption { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? SubmittedDate { get; set; }
            public DateTime ClosingDate { get; set; }
            public ApplicationStatus Status { get; set; }

            public static implicit operator Application(GetAccountDeletionApplicationsToWithdrawApiResponse.SubmittedApplication source)
            {
                var addresses = new List<Address> { source.Address };
                if (source.OtherAddresses is { Count: > 0 }) addresses.AddRange(source.OtherAddresses);

                return new Application
                {
                    Id = source.Id,
                    Title = source.Title,
                    VacancyReference = source.VacancyReference,
                    EmployerName = source.EmployerName,
                    CreatedDate = source.CreatedDate,
                    ClosingDate = source.ClosingDate,
                    SubmittedDate = source.SubmittedDate,
                    Status = (ApplicationStatus)source.Status,
                    WorkLocation = source.Address,
                    Addresses = addresses,
                    EmploymentLocationOption = source.EmploymentLocationOption,
                    EmploymentLocationInformation = source.EmploymentLocationInformation,
                };
            }
        }
    }
}
