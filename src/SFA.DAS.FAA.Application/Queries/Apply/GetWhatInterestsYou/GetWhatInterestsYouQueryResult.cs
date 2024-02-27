using SFA.DAS.FAA.Domain.Apply.WhatInterestsYou;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetWhatInterestsYou;

public class GetWhatInterestsYouQueryResult
{
    public string StandardName { get; set; }
    public string EmployerName { get; set; }

    public static implicit operator GetWhatInterestsYouQueryResult(GetWhatInterestsYouApiResponse source)
    {
        return new GetWhatInterestsYouQueryResult
        {
            EmployerName = source.EmployerName,
            StandardName = source.StandardName
        };
    }
}