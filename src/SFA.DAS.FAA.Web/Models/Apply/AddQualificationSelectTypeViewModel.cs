using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Domain.Apply.Qualifications;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class AddQualificationSelectTypeViewModel
{
    [FromRoute]
    public required Guid ApplicationId { get; set; }

    public List<QualificationTypeApiResponse> Qualifications { get; set; } = [];
        
    public Guid QualificationReferenceId { get; set; }
    public bool HasAddedQualifications { get; set; }
    public string PageTitle => HasAddedQualifications ? "Add another qualification" : "What is your most recent qualification?";
}