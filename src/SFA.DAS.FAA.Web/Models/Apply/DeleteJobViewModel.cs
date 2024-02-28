using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetDeleteJob;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class DeleteJobViewModel : JobViewModelBase
    {
        [FromRoute]
        public Guid ApplicationId { get; set; }

        [FromRoute]
        public Guid JobId { get; set; }
        public string? JobDates { get; private init; }

        public static implicit operator DeleteJobViewModel(GetDeleteJobQueryResult source)
        {
            return new DeleteJobViewModel
            {
                ApplicationId = source.ApplicationId,
                EmployerName = source.Employer,
                JobDates = source.EndDate is null ? $"{source.StartDate:MMMM yyyy} onwards" : $"{source.StartDate:MMMM yyyy} to {source.EndDate:MMMM yyyy}",
                IsCurrentRole = !source.EndDate.HasValue, //not sure if need this, asked chris L
                JobDescription = source.Description,
                JobTitle = source.JobTitle
            };
        }
    }
}
