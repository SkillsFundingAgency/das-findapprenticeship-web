namespace SFA.DAS.FAA.Domain.Apply.GetApplicationSubmitted;
public class GetApplicationSubmittedApiResponse
{
    public string VacancyTitle { get; set; }
    public string EmployerName { get; set; }
    public bool HasAnsweredEqualityQuestions { get; set; }
}
