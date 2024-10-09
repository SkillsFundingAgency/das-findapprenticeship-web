using System.Globalization;
using SFA.DAS.FAA.Application.Queries.User.GetAccountDeletionApplicationsToWithdraw;

namespace SFA.DAS.FAA.Web.Models.User
{
    public class AccountDeletionWithDrawApplicationsViewModel
    {
        public List<Application> SubmittedApplications { get; set; } = [];

        public static implicit operator AccountDeletionWithDrawApplicationsViewModel(GetAccountDeletionApplicationsToWithdrawQueryResult source)
        {
            return new AccountDeletionWithDrawApplicationsViewModel
            {
                SubmittedApplications = source.SubmittedApplications.Select(app => (Application)app).ToList()
            };
        }


        public class Application
        {
            public Guid Id { get; set; }
            public string? Title { get; set; }
            public string? EmployerName { get; set; }
            public string? Address { get; set; }
            public string? SubmittedDate { get; set; }

            public static implicit operator Application(GetAccountDeletionApplicationsToWithdrawQueryResult.Application source)
            {
                return new Application
                {
                    Id = source.Id,
                    EmployerName = source.EmployerName,
                    SubmittedDate = $"Submitted on {source.SubmittedDate?.ToString("d MMMM yyyy", CultureInfo.InvariantCulture)}",
                    Title = source.Title,
                    Address = $"{source.City}, {source.Postcode}"
                };
            }
        }
    }
}
