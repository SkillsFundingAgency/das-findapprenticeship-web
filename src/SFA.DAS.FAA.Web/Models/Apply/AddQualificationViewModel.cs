using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Domain.Apply.Qualifications;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class AddQualificationViewModel
{
    [FromRoute]
    public required Guid ApplicationId { get; set; }

    public QualificationTypeApiResponse QualificationType { get; set; }
    
    [FromRoute]
    public Guid QualificationReferenceId { get; set; }
}