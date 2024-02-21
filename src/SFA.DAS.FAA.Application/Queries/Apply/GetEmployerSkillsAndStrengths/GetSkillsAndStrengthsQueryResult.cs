using SFA.DAS.FAA.Domain.Apply.GetEmployerSkillsAndStrengths;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetEmployerSkillsAndStrengths;
public class GetSkillsAndStrengthsQueryResult
{
    public Guid ApplicationId { get; set; }
    public string Employer { get; set; }
    public IEnumerable<string> ExpectedSkillsAndStrengths { get; set; }

    public static implicit operator GetSkillsAndStrengthsQueryResult(GetSkillsAndStrengthsApiResponse source)
    {
        return new GetSkillsAndStrengthsQueryResult()
        {
            ApplicationId = source.ApplicationId,
            Employer = source.Employer,
            ExpectedSkillsAndStrengths = source.ExpectedSkillsAndStrengths
        };
    }
}
