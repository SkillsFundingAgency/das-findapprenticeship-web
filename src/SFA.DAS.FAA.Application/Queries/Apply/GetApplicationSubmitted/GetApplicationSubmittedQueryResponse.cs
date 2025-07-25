﻿using SFA.DAS.FAA.Domain.Apply.GetApplicationSubmitted;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSubmitted;
public class GetApplicationSubmittedQueryResponse
{
    public string VacancyTitle { get; set; }
    public string EmployerName { get; set; }
    public bool HasAnsweredEqualityQuestions { get; set; }
    public DateTime ClosingDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    public bool IsVacancyClosedEarly => ClosedDate < ClosingDate;

    public static implicit operator GetApplicationSubmittedQueryResponse(GetApplicationSubmittedApiResponse source)
    {
        return new GetApplicationSubmittedQueryResponse
        {
            VacancyTitle = source.VacancyTitle,
            EmployerName = source.EmployerName,
            HasAnsweredEqualityQuestions = source.HasAnsweredEqualityQuestions,
            ClosingDate = source.ClosingDate,
            ClosedDate = source.ClosedDate,
        };
    }
}
