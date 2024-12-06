using SFA.DAS.FAA.Application.Queries.GetNhsApprenticeshipVacancy;

namespace SFA.DAS.FAA.Web.Models.Vacancy
{
    public class NhsVacancyDetailsViewModel
    {
        public string? Title { get; init; }
        public string? Address { get; init; }
        public string? EmployerName { get; init; }
        public string? ApplicationUrl { get; init; }
        public string? BackLinkUrl { get; set; }

        public static implicit operator NhsVacancyDetailsViewModel(GetNhsApprenticeshipVacancyQueryResult source)
        {
            return new NhsVacancyDetailsViewModel
            {
                Title = source.Vacancy?.Title,
                EmployerName = source.Vacancy?.EmployerName,
                ApplicationUrl = source.Vacancy?.ApplicationUrl,
                Address = !string.IsNullOrEmpty(source.Vacancy?.Address?.AddressLine4) ? $"{source.Vacancy?.Address?.AddressLine4}, {source.Vacancy?.Address?.Postcode}" :
                    !string.IsNullOrEmpty(source.Vacancy?.Address?.AddressLine3) ? $"{source.Vacancy?.Address?.AddressLine3}, {source.Vacancy?.Address?.Postcode}" :
                    !string.IsNullOrEmpty(source.Vacancy?.Address?.AddressLine2) ? $"{source.Vacancy?.Address?.AddressLine2}, {source.Vacancy?.Address?.Postcode}" :
                    !string.IsNullOrEmpty(source.Vacancy?.Address?.AddressLine1) ? $"{source.Vacancy?.Address?.AddressLine1}, {source.Vacancy?.Address?.Postcode}" :
                    source.Vacancy?.Address?.Postcode,
            };
        }
    }
}
