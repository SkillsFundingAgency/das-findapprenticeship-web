using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetCandidateSkillsAndStrengths;
using SFA.DAS.FAA.Application.Queries.Apply.GetExpectedSkillsAndStrengths;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class SkillsAndStrengthsViewModel
{
    [FromRoute]
    public Guid ApplicationId { get; init; }
    public bool? IsSectionComplete { get; set; }
    public string? SkillsAndStrengths { get; set; }

    public string? Employer { get; set; }
    public List<string>? ExpectedSkillsAndStrengths { get; set; }

    public SkillsAndStrengthsViewModel(
        GetExpectedSkillsAndStrengthsQueryResult expectedSkillsAndStrengths, 
        GetCandidateSkillsAndStrengthsQueryResult? candidateSkillsAndStrengths,
        Guid applicationId)
    {
        ApplicationId = applicationId;
        Employer = expectedSkillsAndStrengths.Employer;
        ExpectedSkillsAndStrengths = expectedSkillsAndStrengths.ExpectedSkillsAndStrengths.ToList();
        SkillsAndStrengths = candidateSkillsAndStrengths.SkillsAndStrengths;
    }

}
