using SFA.DAS.FAA.Application.Queries.Apply.GetDeleteVolunteeringOrWorkExperience;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class DeleteVolunteeringOrWorkExperienceViewModel
{
    public Guid ApplicationId { get; set; }
    public Guid Id { get; set; }
    public string Dates { get; set; }
    public string Organisation { get; set; }
    public string Description { get; set; }

    public static implicit operator DeleteVolunteeringOrWorkExperienceViewModel(GetDeleteVolunteeringOrWorkExperienceQueryResult source)
    {
        return new DeleteVolunteeringOrWorkExperienceViewModel
        {
            ApplicationId = source.ApplicationId,
            Id = source.Id,
            Dates = source.ToDate is null ? $"{source.FromDate:MMMM yyyy} onwards" : $"{source.FromDate:MMMM yyyy} to {source.ToDate:MMMM yyyy}",
            Organisation = source.Organisation,
            Description = source.Description
        };
    }
}