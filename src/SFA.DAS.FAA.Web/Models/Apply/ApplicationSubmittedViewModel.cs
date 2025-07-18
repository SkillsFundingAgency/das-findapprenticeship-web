namespace SFA.DAS.FAA.Web.Models.Apply;

public record ApplicationSubmittedViewModel
{
    public Guid ApplicationId { get; init; }
    public ApplicationSubmittedVacancyInfo? VacancyInfo { get; init; }
    public bool? AnswerEqualityQuestions { get; set; }
    public string PageTitle => VacancyInfo is { HasAnsweredEqualityQuestions: false } 
        ? "Do you want to answer equality questions?"
        : "Application submitted";
    public bool IsVacancyClosed => !string.IsNullOrEmpty(ClosedDate);
    public bool IsVacancyClosedEarly { get; set; }
    public string? ClosedDate { get; set; }
    public string ClosedBannerHeaderText => "Sorry, we could not submit your application";
    public string? ClosedBannerText => IsVacancyClosedEarly
        ? "This vacancy has been closed early."
        : "This vacancy has now closed.";
}
