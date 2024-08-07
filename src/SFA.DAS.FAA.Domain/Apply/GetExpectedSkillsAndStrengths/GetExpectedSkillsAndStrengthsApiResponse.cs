﻿namespace SFA.DAS.FAA.Domain.Apply.GetExpectedSkillsAndStrengths;

public class GetExpectedSkillsAndStrengthsApiResponse
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
    public string Employer { get; set; }
    public IEnumerable<string> ExpectedSkillsAndStrengths { get; set; }
    public bool? IsSectionCompleted { get; set; }
    public string? Strengths { get; set; }
}
