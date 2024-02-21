using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetEmployerSkillsAndStrengths;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class SkillsAndStrengthsViewModel
{
    [FromRoute]
    public required Guid ApplicationId { get; init; }
    public bool? IsSectionComplete { get; set; }
    public string? SkillsAndStrengths { get; set; }

    public string Employer { get; set; }
    public List<string> ExpectedSkillsAndStrengths { get; set; }

    public static implicit operator SkillsAndStrengthsViewModel(GetSkillsAndStrengthsQueryResult source)
    {
        return new SkillsAndStrengthsViewModel
        {
            ApplicationId = source.ApplicationId,
            Employer = source.Employer,
            ExpectedSkillsAndStrengths = source.ExpectedSkillsAndStrengths.ToList()
        };
    }
}
