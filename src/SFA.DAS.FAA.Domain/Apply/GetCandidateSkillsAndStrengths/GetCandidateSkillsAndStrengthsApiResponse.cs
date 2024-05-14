namespace SFA.DAS.FAA.Domain.Apply.GetCandidateSkillsAndStrengths;
public class GetCandidateSkillsAndStrengthsApiResponse
{
    public AboutYouItem? AboutYou { get; set; }
}

public class AboutYouItem
{
    public string? SkillsAndStrengths { get; set; }
    public string? Support { get; set; }
    public Guid ApplicationId { get; set; }
}
