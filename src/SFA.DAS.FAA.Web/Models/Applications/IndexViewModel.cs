using SFA.DAS.FAA.Application.Queries.Applications.GetIndex;
using System.Globalization;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.Models.Applications
{
    public class IndexViewModel
    {
        public string TabTitle { get; set; }
        public string TabText { get; set; }
        public bool IsTabTextInset { get; set; }
        public string TabTextStyle => IsTabTextInset ? "govuk-inset-text" : "govuk-body";

        public List<Application> Applications { get; set; } = [];
        public List<Application> ExpiredApplications { get; set; } = [];

        public class Application
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string EmployerName { get; set; }
            public string StartedOn { get; set; }
            public string ClosingDate { get; set; }
            public bool IsClosingSoon { get; set; }
            public bool IsClosed { get; set; }
        }

        public static IndexViewModel Create(ApplicationsTab tab, GetIndexQueryResult source, IDateTimeService dateTimeService)
        {
            var result = new IndexViewModel();

            foreach (var application in source.Applications.OrderByDescending(x => x.CreatedDate))
            {
                var daysToExpiry = (application.ClosingDate - dateTimeService.GetDateTime()).Days;

                var closingDate = "";
                switch (daysToExpiry)
                {
                    case < 0:
                        closingDate = $"Closed {application.ClosingDate.ToString("dddd dd MMMM yyyy", CultureInfo.InvariantCulture)}";
                        break;
                    case 0:
                        closingDate = "Closes today at 11:59pm";
                        break;
                    case 1:
                        closingDate = $"Closes tomorrow ({application.ClosingDate.ToString("dddd dd MMMM yyyy", CultureInfo.InvariantCulture)} at 11:59pm)";
                        break;
                    case <= 31:
                        closingDate = $"Closes in {daysToExpiry} days ({application.ClosingDate.ToString("dddd dd MMMM yyyy", CultureInfo.InvariantCulture)} at 11:59pm)";
                        break;
                    default:
                        closingDate = $"Closes on {application.ClosingDate.ToString("dddd dd MMMM yyyy", CultureInfo.InvariantCulture)} at 11:59pm";
                        break;
                }

                var applicationViewModel = new Application
                {
                    Id = application.Id,
                    Title = application.Title,
                    EmployerName = application.EmployerName,
                    StartedOn =
                        $"Started on {application.CreatedDate.ToString("dd MMMM yyyy", CultureInfo.InvariantCulture)}",
                    ClosingDate = closingDate,
                    IsClosingSoon = daysToExpiry <= 7,
                    IsClosed = daysToExpiry < 0
                };

                if (applicationViewModel.IsClosed)
                {
                    result.ExpiredApplications.Add(applicationViewModel);
                }
                else
                {
                    result.Applications.Add(applicationViewModel);
                }
            }

            result.TabTitle = tab.GetTabTitle();
            result.TabText = source.Applications.Any() ? tab.GetTabPopulatedText() : tab.GetTabEmptyText();
            result.IsTabTextInset = tab == ApplicationsTab.Started && source.Applications.Any();

            return result;
        }
    }
}
