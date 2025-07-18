using SFA.DAS.FAA.Application.Queries.Applications.GetIndex;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Extensions;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.Models.Applications
{
    public class IndexViewModel
    {
        public ApplicationsTab SelectedTab { get; set; }
        public string? PageTitle { get; set; }
        public string? TabTitle { get; set; }
        public string? TabText { get; set; }
        public bool IsTabTextInset { get; set; }
        public string TabTextStyle => IsTabTextInset ? "govuk-inset-text" : "govuk-body";

        public List<Application> Applications { get; set; } = [];
        public List<Application> ExpiredApplications { get; set; } = [];
        public List<Application> WithdrawnApplications { get; set; } = [];
        public string? WithdrawnBannerMessage { get; set; }
        public string? ApplicationSubmittedBannerMessage { get; set; }
        public bool ShowEqualityQuestionsBannerMessage { get; set; }

        public string? NewSuccessfulApplicationsCount { get; set; }

        public string? NewUnsuccessfulApplicationsCount { get; set; }

        public class Application
        {
            public Guid Id { get; set; }
            public string? VacancyReference { get; set; }
            public string? Title { get; set; }
            public string? EmployerName { get; set; }
            public string? StartedOn { get; set; }
            public string? ClosingDate { get; set; }
            public string? WithdrawnDate { get; set; }
            public string? SubmittedDate { get; set; }
            public string? ResponseDate { get; set; }
            public bool IsClosingSoon { get; set; }
            public bool IsClosed { get; set; }
            public ApplicationStatus Status { get; set; }
            public string ResponseNotes { get; set; } = null!;
            public DateTime CloseDateTime { get; set; }
            public string? EmploymentWorkLocation { get; set; }
            public ApprenticeshipTypes ApprenticeshipType { get; init; }
            public bool ShowFoundationTag => ApprenticeshipType == ApprenticeshipTypes.Foundation;

            public static Application From(GetIndexQueryResult.Application source, IDateTimeService dateTimeService)
            {
                var timeUntilClosing = (source.ClosedDate ?? source.ClosingDate).Date - dateTimeService.GetDateTime();
                var daysToExpiry = (int)Math.Ceiling(timeUntilClosing.TotalDays);
                
                return new Application
                {
                    Id = source.Id,
                    VacancyReference = source.VacancyReference,
                    Title = source.Title,
                    EmployerName = source.EmployerName,
                    CloseDateTime = source.ClosingDate,
                    StartedOn = $"Started on {source.CreatedDate.ToGdsDateString()}",
                    ClosingDate = VacancyDetailsHelperService.GetClosingDate(dateTimeService, source.ClosingDate, source.ClosedDate),
                    IsClosingSoon = !source.ClosedDate.HasValue && (daysToExpiry is >= 0 and <= 7),
                    IsClosed = source.ClosedDate.HasValue,
                    Status = source.Status,
                    SubmittedDate = source.Status is (ApplicationStatus.Submitted)
                        ? $"Submitted on {source.SubmittedDate?.ToGdsDateString()}" 
                        : string.Empty,
                    WithdrawnDate = $"Withdrawn application on {source.WithdrawnDate?.ToGdsDateString()}",
                    ResponseDate = source.Status switch
                    {
                        (ApplicationStatus.Successful) => $"Offered on {source.SubmittedDate?.ToGdsDateString()}",
                        ApplicationStatus.Unsuccessful => $"Feedback received on {source.SubmittedDate?.ToGdsDateString()}",
                        _ => string.Empty
                    },
                    ResponseNotes = source.Status is ApplicationStatus.Unsuccessful
                        ? source.ResponseNotes
                        : string.Empty,
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

        public static IndexViewModel Map(ApplicationsTab tab,
            GetIndexQueryResult source,
            IDateTimeService dateTimeService)
        {
            var expiredApplications = new List<Application>();
            var result = new IndexViewModel
            {
                SelectedTab = tab,
                PageTitle = $"{tab.GetTabTitle()} applications",
            };

            var applications = source.Applications.OrderByDescending(
                x => tab == ApplicationsTab.Unsuccessful
                    ? x.ResponseDate
                    : x.CreatedDate);

            foreach (var application in applications)
            {
                var applicationViewModel = Application.From(application, dateTimeService);
                
                switch (applicationViewModel.Status)
                {
                    case ApplicationStatus.Expired:
                        expiredApplications.Add(applicationViewModel);
                        break;
                    case ApplicationStatus.Withdrawn:
                        result.WithdrawnApplications.Add(applicationViewModel);
                        break;
                    default:
                        result.Applications.Add(applicationViewModel);
                        break;
                }
            }
            result.TabTitle = tab.GetTabTitle();
            result.TabText = result.Applications.Count > 0 ? tab.GetTabPopulatedText() : tab.GetTabEmptyText();
            result.IsTabTextInset = tab == ApplicationsTab.Started && result.Applications.Count > 0;
            result.ExpiredApplications = [.. expiredApplications
                .OrderByDescending(fil => fil.CloseDateTime)
                .ThenBy(fil => fil.Title)];
            return result;
        }
    }
}