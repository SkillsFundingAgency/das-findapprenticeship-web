namespace SFA.DAS.FAA.Domain.Apply.GetApplicationSubmitted;
public record GetApplicationSubmittedApiResponse
{
    public string? VacancyTitle { get; set; }
    public string? EmployerName { get; set; }
    public bool HasAnsweredEqualityQuestions { get; set; }
    public DateTime ClosingDate { get; set; }
    public DateTime? ClosedDate { get; set; }
}
