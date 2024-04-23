using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Domain.Apply.Qualifications;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class AddQualificationSelectTypeViewModel
{
    [FromRoute]
    public required Guid ApplicationId { get; set; }

    public List<QualificationType> Qualifications { get; set; } = [];
        
    public Guid QualificationReferenceId { get; set; }
    public bool HasAddedQualifications { get; set; }
    public string PageTitle => HasAddedQualifications ? "Add another qualification" : "What is your most recent qualification?";

    public class QualificationType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public QualificationDisplayTypeViewModel QualificationDisplayTypeViewModel { get; set; }
    }
}