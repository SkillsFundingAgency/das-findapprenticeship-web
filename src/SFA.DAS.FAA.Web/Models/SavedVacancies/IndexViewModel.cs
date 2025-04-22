using SFA.DAS.FAA.Application.Queries.GetSavedVacancies;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Extensions;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Web.Models.SavedVacancies
{
    public class IndexViewModel
    {
        public List<SavedVacancy> SavedVacancies { get; set; } = [];
        public List<SavedVacancy> ExpiredSavedVacancies { get; set; } = [];
        public bool HasExpiredVacancies => ExpiredSavedVacancies.Any();
        public bool HasSavedVacancies => SavedVacancies.Any();
        public bool ShowSortComponent => SavedVacancies.Count > 1;
        public SortOrder SortOrder { get; set; }
        public DeletedSavedVacancy? DeletedVacancy { get; set; }
        public bool ShowDeletedVacancyConfirmationMessage => DeletedVacancy != null;

        public List<SelectListItem> SortOrderOptions =>
        [
            new()
            {
                Value = SortOrder.DateSaved.ToString(),
                Text = "Date saved",
                Selected = SortOrder == SortOrder.DateSaved
            },
            new()
            {
                Value = SortOrder.ClosingSoon.ToString(),
                Text = "Closing soon",
                Selected = SortOrder == SortOrder.ClosingSoon
            }
        ];

        public class SavedVacancy
        {
            public Guid Id { get; set; }
            public string? VacancyReference { get; set; }
            public string? Title { get; set; }
            public string? EmployerName { get; set; }
            public string? CreatedOn { get; set; }
            public string ClosingDateLabel { get; set; }
            public DateTime ClosingDate { get; set; }
            public bool IsClosingSoon { get; set; }
            public string? EmploymentWorkLocation { get; set; }
            public bool IsExternalVacancy { get; set; }
            public string ExternalVacancyUrl { get; set; }
            public bool ShowApplyButton { get; set; }
        }

        public class DeletedSavedVacancy
        {
            public string? VacancyReference { get; set; }
            public string? VacancyTitle { get; set; }
            public string? EmployerName { get; set; }

            public static implicit operator DeletedSavedVacancy?(GetSavedVacanciesQueryResult.DeletedSavedVacancy? source)
            {
                if (source is null) return null;

                return new DeletedSavedVacancy
                {
                    EmployerName = source.EmployerName,
                    VacancyReference = source.VacancyReference,
                    VacancyTitle = source.VacancyTitle
                };
            }
        }

        public static IndexViewModel Map(GetSavedVacanciesQueryResult source, IDateTimeService dateTimeService, SortOrder sortOrder)
        {
            var result = new IndexViewModel
            {
                SortOrder = sortOrder,
                DeletedVacancy = source.DeletedVacancy != null ? new DeletedSavedVacancy
                {
                    VacancyReference = source.DeletedVacancy?.VacancyReference,
                    VacancyTitle = source.DeletedVacancy?.VacancyTitle,
                    EmployerName = source.DeletedVacancy?.EmployerName,
                } : null
            };

            var expired = new List<SavedVacancy>();
            var savedVacancies = new List<SavedVacancy>();

            foreach (var vacancy in source.SavedVacancies)
            {
                var timeUntilClosing = vacancy.ClosingDate.Date - dateTimeService.GetDateTime();
                var daysToExpiry = (int)Math.Ceiling(timeUntilClosing.TotalDays);

                var savedVacancy = new SavedVacancy
                {
                    Id = vacancy.Id,
                    VacancyReference = vacancy.VacancyReference,
                    Title = vacancy.Title,
                    EmployerName = vacancy.EmployerName,
                    EmploymentWorkLocation = vacancy.EmployerLocationOption switch
                    {
                        AvailableWhere.MultipleLocations => VacancyDetailsHelperService.GetEmploymentLocationCityNames(vacancy.Addresses),
                        AvailableWhere.AcrossEngland => "Recruiting nationally",
                        _ => VacancyDetailsHelperService.GetOneLocationCityName(vacancy.WorkLocation)
                    },
                    IsExternalVacancy = vacancy.IsExternalVacancy,
                    ExternalVacancyUrl = !string.IsNullOrEmpty(vacancy.ExternalVacancyUrl) && !vacancy.ExternalVacancyUrl.StartsWith("http") ? $"https://{vacancy.ExternalVacancyUrl}" : vacancy.ExternalVacancyUrl,
                    CreatedOn = 
                        $"Saved on {vacancy.CreatedDate.ToGdsDateString()}",
                    ClosingDate = vacancy.ClosingDate,
                    ClosingDateLabel = VacancyDetailsHelperService.GetClosingDate(dateTimeService, vacancy.ClosingDate, vacancy.IsExternalVacancy),
                    IsClosingSoon = daysToExpiry is >= 0 and <= 7,
                    ShowApplyButton = vacancy.Status == null || (vacancy.Status != ApplicationStatus.Submitted && vacancy.Status != ApplicationStatus.Successful && vacancy.Status != ApplicationStatus.Unsuccessful),
                };

                if (daysToExpiry < 0)
                {
                    expired.Add(savedVacancy);
                }
                else
                {
                    savedVacancies.Add(savedVacancy);
                }

                result.SavedVacancies = sortOrder == SortOrder.ClosingSoon
                    ? savedVacancies.OrderBy(x => x.ClosingDate).ToList()
                    : savedVacancies.OrderByDescending(x => x.CreatedOn).ToList();
                
                result.ExpiredSavedVacancies = expired.OrderByDescending(x => x.CreatedOn).ToList();
            }

            return result;
        }
    }
}
