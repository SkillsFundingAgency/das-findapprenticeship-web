namespace SFA.DAS.FAA.Domain.Apply.WorkHistory.SummaryVolunteeringAndWorkExperience
{
    public record GetVolunteeringAndWorkExperiencesApiResponse
    {
        public List<VolunteeringAndWorkExperience> VolunteeringAndWorkExperiences { get; set; } = null!;

        public record VolunteeringAndWorkExperience
        {
            public Guid Id { get; set; }
            public string? Employer { get; set; }
            public string? JobTitle { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public Guid ApplicationId { get; set; }
            public string? Description { get; set; }
        }
    }
}
