using SFA.DAS.FAA.Application.Queries.Applications.GetIndex;
using System.Globalization;
using SFA.DAS.FAA.Domain.Enums;
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
        public string? BannerMessage { get; set; }

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
        }

        public static IndexViewModel Map(ApplicationsTab tab, GetIndexQueryResult source, IDateTimeService dateTimeService)
        {
            var result = new IndexViewModel
            {
                SelectedTab = tab,
                PageTitle = $"{tab.GetTabTitle()} applications"
            };

            foreach (var application in source.Applications.OrderByDescending(x => x.CreatedDate))
            {
                var timeUntilClosing = application.ClosingDate.Date - dateTimeService.GetDateTime();
                var daysToExpiry = (int)Math.Ceiling(timeUntilClosing.TotalDays);

                var applicationViewModel = new Application
                {
                    Id = application.Id,
                    VacancyReference = application.VacancyReference,
                    Title = application.Title,
                    EmployerName = application.EmployerName,
                    StartedOn =
                        $"Started on {application.CreatedDate.ToString("d MMMM yyyy", CultureInfo.InvariantCulture)}",
                    ClosingDate = VacancyDetailsHelperService.GetClosingDate(dateTimeService, application.ClosingDate),
                    IsClosingSoon = daysToExpiry is >= 0 and <= 7,
                    IsClosed = daysToExpiry < 0,
                    Status = application.Status,
                    SubmittedDate = application.Status is (ApplicationStatus.Submitted)
                        ? $"Submitted on {application.SubmittedDate?.ToString("d MMMM yyyy", CultureInfo.InvariantCulture)}" 
                        : string.Empty,
                    WithdrawnDate = $"Withdrawn application on {application.WithdrawnDate?.ToString("d MMMM yyyy", CultureInfo.InvariantCulture)}",
                    ResponseDate = application.Status is (ApplicationStatus.Successful)
                        ? $"Offered on {application.SubmittedDate?.ToString("d MMMM yyyy", CultureInfo.InvariantCulture)}"
                        : string.Empty
                };

                if (applicationViewModel.IsClosed)
                {
                    result.ExpiredApplications.Add(applicationViewModel);
                }
                else if (applicationViewModel.Status == ApplicationStatus.Withdrawn)
                {
                    result.WithdrawnApplications.Add(applicationViewModel);
                }
                else
                {
                    result.Applications.Add(applicationViewModel);
                }
            }

            result.TabTitle = tab.GetTabTitle();
            result.TabText = result.Applications.Count > 0 ? tab.GetTabPopulatedText() : tab.GetTabEmptyText();
            result.IsTabTextInset = tab == ApplicationsTab.Started && result.Applications.Count > 0;

            return result;
        }
    }
}
