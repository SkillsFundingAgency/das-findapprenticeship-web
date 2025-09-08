using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Models.Apply.Base;

namespace SFA.DAS.FAA.Web.Models.Apply;

public record AddAdditionalQuestionViewModel : AdditionalQuestionViewModelBase
{
    [FromRoute]
    public Guid ApplicationId { get; init; }
}