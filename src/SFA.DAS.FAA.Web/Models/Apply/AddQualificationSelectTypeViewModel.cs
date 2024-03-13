using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Domain.Apply.Qualifications;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class AddQualificationSelectTypeViewModel
{
    [FromRoute]
    public required Guid ApplicationId { get; set; }

    public List<QualificationTypeApiResponse> Qualifications { get; set; } = [];
        
    public Guid QualificationReferenceId { get; set; }
}