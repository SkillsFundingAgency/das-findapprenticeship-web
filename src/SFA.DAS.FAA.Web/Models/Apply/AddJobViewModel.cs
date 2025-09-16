using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetJob;
using SFA.DAS.FAA.Web.ModelBinding;
using SFA.DAS.FAA.Web.Models.Custom;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class AddJobViewModel : JobViewModelBase
{
    [FromRoute]
    public Guid ApplicationId { get; set; }
}

public class EditJobViewModel : JobViewModelBase
{
    [FromRoute]
    public Guid ApplicationId { get; set; }

    [FromRoute]
    public Guid JobId { get; set; }

    public static implicit operator EditJobViewModel(GetJobQueryResult source)
    {
        return new EditJobViewModel
        {
            ApplicationId = source.ApplicationId,
            EmployerName = source.Employer,
            StartDate = new MonthYearDate(source.StartDate),
            EndDate = new MonthYearDate(source.EndDate),
            IsCurrentRole = !source.EndDate.HasValue,
            JobDescription = source.Description,
            JobTitle = source.JobTitle
        };
    }
}

public class JobViewModelBase
{
    public string? JobTitle { get; set; }
    public string? EmployerName { get; set; }
    public string? JobDescription { get; set; }
    [ModelBindingError("Enter a real date for the start date")]
    public MonthYearDate? StartDate { get; set; }
    public bool? IsCurrentRole { get; set; }
    [ModelBindingError("Enter a real date for the end date")]
    public MonthYearDate? EndDate { get; set; }
}