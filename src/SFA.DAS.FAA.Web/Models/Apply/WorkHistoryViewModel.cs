using SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringAndWorkExperiences;
using SFA.DAS.FAA.Application.Queries.Apply.GetWorkHistories;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.FAA.Web.Models.Apply;

public record WorkHistoryViewModel
{
    public Guid Id { get; private init; }
    public string? Employer { get; private init; }
    public string? JobTitle { get; private init; }
    public string? JobDates { get; private init; }
    [DataType(DataType.MultilineText)]
    public string? Description { get; private init; }

    public static implicit operator WorkHistoryViewModel(GetJobsQueryResult.Job source)
    {
        return new WorkHistoryViewModel
        {
            Id = source.Id,
            JobTitle = source.JobTitle,
            Employer = source.Employer,
            Description = source.Description,
            JobDates = source.EndDate is null ? $"{source.StartDate:MMMM yyyy} onwards" : $"{source.StartDate:MMMM yyyy} to {source.EndDate:MMMM yyyy}"
        };
    }

    public static implicit operator WorkHistoryViewModel(GetVolunteeringAndWorkExperiencesQueryResult.VolunteeringAndWorkExperience source)
    {
        return new WorkHistoryViewModel
        {
            Id = source.Id,
            JobTitle = source.JobTitle,
            Employer = source.Employer,
            Description = source.Description,
            JobDates = source.EndDate is null ? $"{source.StartDate:MMMM yyyy} onwards" : $"{source.StartDate:MMMM yyyy} to {source.EndDate:MMMM yyyy}"
        };
    }
}