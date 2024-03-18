namespace SFA.DAS.FAA.Web.Models.Apply;

public class ApplicationSubmittedViewModel
{
    public string VacancyTitle { get; set; }
    public string EmployerName { get; set; }
    public Guid ApplicationId { get; set; }
    public bool? AnswerEqualityQuestions { get; set; }
}
