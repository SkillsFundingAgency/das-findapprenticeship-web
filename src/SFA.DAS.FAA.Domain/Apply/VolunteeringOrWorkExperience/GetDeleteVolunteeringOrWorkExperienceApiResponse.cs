﻿namespace SFA.DAS.FAA.Domain.Apply.VolunteeringOrWorkExperience;
public class GetDeleteVolunteeringOrWorkExperienceApiResponse
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public string Organisation { get; set; }
    public string Description { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}