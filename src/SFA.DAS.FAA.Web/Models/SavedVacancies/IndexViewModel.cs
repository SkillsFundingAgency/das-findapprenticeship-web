using SFA.DAS.FAA.Application.Queries.GetSavedVacancies;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;
using System.Globalization;

namespace SFA.DAS.FAA.Web.Models.SavedVacancies
{
    public class IndexViewModel
    {
        public List<SavedVacancy> SavedVacancies { get; set; } = [];

        public class SavedVacancy
        {
            public Guid Id { get; set; }
            public string? VacancyReference { get; set; }
            public string? Title { get; set; }
            public string? EmployerName { get; set; }
            public string? CreatedOn { get; set; }
            public string ClosingDate { get; set; }
            public bool IsClosingSoon { get; set; }
        }

        public static IndexViewModel Map(GetSavedVacanciesQueryResult source, IDateTimeService dateTimeService)
        {
            var result = new IndexViewModel();

            foreach (var vacancy in source.SavedVacancies)
            {
                var timeUntilClosing = vacancy.ClosingDate.Date - dateTimeService.GetDateTime();
                var daysToExpiry = (int)Math.Ceiling(timeUntilClosing.TotalDays);

                if (daysToExpiry < 0) continue;

                result.SavedVacancies.Add(new SavedVacancy
                {
                    Id = vacancy.Id,
                    VacancyReference = vacancy.VacancyReference,
                    Title = vacancy.Title,
                    EmployerName = vacancy.EmployerName,
                    CreatedOn = 
                        $"Saved on {vacancy.CreatedDate.ToString("d MMMM yyyy", CultureInfo.InvariantCulture)}",
                    ClosingDate = VacancyDetailsHelperService.GetClosingDate(dateTimeService, vacancy.ClosingDate),
                    IsClosingSoon = daysToExpiry is >= 0 and <= 7
                });
            }

            return result;
        }
    }
}
