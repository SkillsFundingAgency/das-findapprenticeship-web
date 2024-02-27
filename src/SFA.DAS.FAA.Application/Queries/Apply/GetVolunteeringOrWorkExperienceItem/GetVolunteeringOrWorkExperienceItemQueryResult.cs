namespace SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringOrWorkExperienceItem;
public class GetVolunteeringOrWorkExperienceItemQueryResult
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public string Organisation { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
