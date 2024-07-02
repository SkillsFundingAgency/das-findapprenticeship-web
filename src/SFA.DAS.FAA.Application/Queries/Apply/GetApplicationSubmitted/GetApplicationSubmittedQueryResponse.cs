using SFA.DAS.FAA.Domain.Apply.GetApplicationSubmitted;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSubmitted;
public class GetApplicationSubmittedQueryResponse
{
    public string VacancyTitle { get; set; }
    public string EmployerName { get; set; }
    public bool HasAnsweredEqualityQuestions { get; set; }

    public static implicit operator GetApplicationSubmittedQueryResponse(GetApplicationSubmittedApiResponse source)
    {
        return new GetApplicationSubmittedQueryResponse
        {
            VacancyTitle = source.VacancyTitle,
            EmployerName = source.EmployerName,
            HasAnsweredEqualityQuestions = source.HasAnsweredEqualityQuestions
        };
    }
}
