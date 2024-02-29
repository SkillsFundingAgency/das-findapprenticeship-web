using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply.Base;

public record AdditionalQuestionViewModelBase
{
    [BindProperty]
    public bool? IsSectionCompleted { get; init; }
    public string? AdditionalQuestionLabel { get; init; }
    public string? AdditionalQuestionAnswer { get; init; }
}