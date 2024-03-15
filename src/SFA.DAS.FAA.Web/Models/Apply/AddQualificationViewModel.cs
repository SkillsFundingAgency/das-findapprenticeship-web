using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Domain.Apply.Qualifications;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class AddQualificationViewModel
{
    [FromRoute]
    public required Guid ApplicationId { get; set; }
    public QualificationDisplayTypeViewModel QualificationDisplayTypeViewModel { get; set; }
    [FromRoute]
    public Guid QualificationReferenceId { get; set; }
    public List<SubjectViewModel> Subject { get; set; }
}

public class SubjectViewModel
{
    public Guid Id { get; set; }
    public string Grade { get; set; }
    public string Name { get; set; }
    public string Level { get; set; }
    public string IsPredicted { get; set; }
}