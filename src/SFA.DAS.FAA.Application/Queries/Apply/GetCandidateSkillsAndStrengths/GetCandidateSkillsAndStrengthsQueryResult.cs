﻿namespace SFA.DAS.FAA.Application.Queries.Apply.GetCandidateSkillsAndStrengths;
public class GetCandidateSkillsAndStrengthsQueryResult
{
    public string? SkillsAndStrengths { get; set; }
    public string? Improvements { get; set; }
    public string? HobbiesAndInterests { get; set; }
    public string? Support { get; set; }
    public Guid ApplicationId { get; set; }
}
