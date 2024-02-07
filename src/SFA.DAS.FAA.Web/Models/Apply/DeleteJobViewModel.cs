using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetDeleteJob;
using SFA.DAS.FAA.Web.Models.Custom;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class DeleteJobViewModel : JobViewModelBase
    {
        [FromRoute]
        public Guid ApplicationId { get; set; }

        [FromRoute]
        public Guid JobId { get; set; }

        public static implicit operator DeleteJobViewModel(GetDeleteJobQueryResult source)
        {
            return new DeleteJobViewModel
            {
                ApplicationId = source.ApplicationId,
                EmployerName = source.Employer,
                StartDate = new MonthYearDate(source.StartDate),
                EndDate = new MonthYearDate(source.EndDate),
                IsCurrentRole = !source.EndDate.HasValue, //not sure if need this, asked chris L
                JobDescription = source.Description,
                JobTitle = source.JobTitle
            };
        }
    }
}
