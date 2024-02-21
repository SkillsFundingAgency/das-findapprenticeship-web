using SFA.DAS.FAA.Domain.Apply.WorkHistory.SummaryVolunteeringAndWorkExperience;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringAndWorkExperiences;

public record GetVolunteeringAndWorkExperiencesQueryResult
{
    public List<VolunteeringAndWorkExperience> VolunteeringAndWorkExperiences { get; private init; } = [];

    public record VolunteeringAndWorkExperience
    {
        public Guid Id { get; private init; }
        public string? Employer { get; private init; }
        public string? JobTitle { get; private init; }
        public DateTime StartDate { get; private init; }
        public DateTime? EndDate { get; set; }
        public Guid ApplicationId { get; set; }
        public string? Description { get; private init; }

        public static implicit operator VolunteeringAndWorkExperience(GetVolunteeringAndWorkExperiencesApiResponse.VolunteeringAndWorkExperience source)
        {
            return new VolunteeringAndWorkExperience
            {
                Id = source.Id,
                Employer = source.Employer,
                JobTitle = source.JobTitle,
                StartDate = source.StartDate,
                EndDate = source.EndDate,
                ApplicationId = source.ApplicationId,
                Description = source.Description
            };
        }
    }

    public static implicit operator GetVolunteeringAndWorkExperiencesQueryResult(GetVolunteeringAndWorkExperiencesApiResponse source)
    {
        if (source.VolunteeringAndWorkExperiences is {Count: > 0})
            return new GetVolunteeringAndWorkExperiencesQueryResult
            {
                VolunteeringAndWorkExperiences = source.VolunteeringAndWorkExperiences
                    .Select(x => (VolunteeringAndWorkExperience) x).ToList()
            };

        return new GetVolunteeringAndWorkExperiencesQueryResult();
    }
}