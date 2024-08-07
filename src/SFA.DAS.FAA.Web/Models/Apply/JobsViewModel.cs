﻿using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetWorkHistories;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class JobsViewModel
    {
        public static readonly int MaximumItems = 100;

        [FromRoute] 
        public required Guid ApplicationId { get; init; }

        public List<Job>? JobHistory { get; set; }

        public bool ShowJobHistory { get; set; }

        public bool? IsSectionCompleted { get; set; }
        public bool? DoYouWantToAddAnyJobs { get; set; }
        public bool MaximumItemsReached => JobHistory?.Count >= MaximumItems;

        public class Job
        {
            public Guid Id { get; private init; }
            public string JobHeader => $"{JobTitle}, {Employer}";
            public string? Employer { get; private init; }
            public string? JobTitle { get; private init; }
            public string? JobDates { get; private init; }
            public string? Description { get; private init; }

            public static implicit operator Job(GetJobsQueryResult.Job source)
            {
                return new Job
                {
                    Id = source.Id,
                    JobTitle = source.JobTitle,
                    Employer = source.Employer,
                    Description = source.Description,
                    JobDates = source.EndDate is null ? $"{source.StartDate:MMMM yyyy} onwards" : $"{source.StartDate:MMMM yyyy} to {source.EndDate:MMMM yyyy}"
                };
            }
        }
    }
}
