namespace SFA.DAS.FAA.Web.Models.Apply;

public record ApplicationSubmittedViewModel
{
    public Guid ApplicationId { get; init; }
    public ApplicationSubmittedVacancyInfo? VacancyInfo { get; init; }
    public bool? AnswerEqualityQuestions { get; set; }
}
