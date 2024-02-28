using SFA.DAS.FAA.Domain.Apply.WhatInterestsYou;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetWhatInterestsYou;

public class GetWhatInterestsYouQueryResult
{
    public string StandardName { get; set; }
    public string EmployerName { get; set; }
    public string AnswerText { get; set; }
    public bool? IsSectionCompleted { get; set; }

    public static implicit operator GetWhatInterestsYouQueryResult(GetWhatInterestsYouApiResponse source)
    {
        return new GetWhatInterestsYouQueryResult
        {
            EmployerName = source.EmployerName,
            StandardName = source.StandardName,
            AnswerText = source.AnswerText,
            IsSectionCompleted = source.IsSectionCompleted
        };
    }
}