namespace SFA.DAS.FAA.Domain;

public interface IVacancyAdvert
{
    string WageText { get; set; }
    int WageType { get; set; }
    decimal? Over25NationalMinimumWage { get; set; }
    decimal? Between21AndUnder25NationalMinimumWage { get; set; }
    decimal? Between18AndUnder21NationalMinimumWage { get; set; }
    decimal? Under18NationalMinimumWage { get; set; }
    decimal? ApprenticeMinimumWage { get; set; }
    DateTime StartDate { get; set; }
    
}