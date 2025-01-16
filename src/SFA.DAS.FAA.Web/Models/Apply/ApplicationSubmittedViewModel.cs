namespace SFA.DAS.FAA.Web.Models.Apply;

public record ApplicationSubmittedViewModel
{
    public Guid ApplicationId { get; init; }
    public ApplicationSubmittedVacancyInfo? VacancyInfo { get; init; }
    public bool? AnswerEqualityQuestions { get; set; }
    public string PageTitle => VacancyInfo is { HasAnsweredEqualityQuestions: false } 
        ? "Do you want to answer equality questions?"
        : "Application submitted";
}
