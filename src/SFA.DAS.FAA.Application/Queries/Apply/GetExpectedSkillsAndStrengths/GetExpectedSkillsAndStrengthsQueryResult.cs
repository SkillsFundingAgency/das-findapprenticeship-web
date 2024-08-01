using SFA.DAS.FAA.Domain.Apply.GetExpectedSkillsAndStrengths;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetExpectedSkillsAndStrengths;
public class GetExpectedSkillsAndStrengthsQueryResult
{
    public Guid ApplicationId { get; set; }
    public string Employer { get; set; }
    public IEnumerable<string> ExpectedSkillsAndStrengths { get; set; }
    public bool? IsSectionCompleted { get; set; }
    public string? Strengths { get; set; }

    public static implicit operator GetExpectedSkillsAndStrengthsQueryResult(GetExpectedSkillsAndStrengthsApiResponse source)
    {
        return new GetExpectedSkillsAndStrengthsQueryResult()
        {
            ApplicationId = source.ApplicationId,
            Employer = source.Employer,
            ExpectedSkillsAndStrengths = source.ExpectedSkillsAndStrengths,
            IsSectionCompleted = source.IsSectionCompleted,
            Strengths = source.Strengths
        };
    }
}
