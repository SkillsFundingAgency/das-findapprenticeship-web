using System.Globalization;
using SFA.DAS.FAA.Application.Queries.User.GetAccountDeletionApplicationsToWithdraw;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Services;

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
            public string? SubmittedDate { get; set; }
            public string? EmploymentWorkLocation { get; set; }
            private ApprenticeshipTypes ApprenticeshipType { get; init; } = ApprenticeshipTypes.Standard;
            public bool ShowFoundationTag => ApprenticeshipType == ApprenticeshipTypes.Foundation;

            public static implicit operator Application(GetAccountDeletionApplicationsToWithdrawQueryResult.Application source)
            {
                return new Application
                {
                    Id = source.Id,
                    EmployerName = source.EmployerName,
                    SubmittedDate = $"Submitted on {source.SubmittedDate?.ToString("d MMMM yyyy", CultureInfo.InvariantCulture)}",
                    Title = source.Title,
                    EmploymentWorkLocation = source.EmployerLocationOption switch
                    {
                        AvailableWhere.MultipleLocations => VacancyDetailsHelperService.GetEmploymentLocationCityNames(source.Addresses),
                        AvailableWhere.AcrossEngland => "Recruiting nationally",
                        _ => VacancyDetailsHelperService.GetOneLocationCityName(source.WorkLocation)
                    },
                    ApprenticeshipType = source.ApprenticeshipType,
                };
            }
        }
    }
}