﻿using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringOrWorkExperienceItem;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class DeleteVolunteeringOrWorkExperienceViewModel
{
    [FromRoute]
    public Guid ApplicationId { get; set; }
    [FromRoute]
    public Guid VolunteeringWorkExperienceId { get; set; }
    public string Dates { get; set; }
    public string Organisation { get; set; }
    public string Description { get; set; }

    public static implicit operator DeleteVolunteeringOrWorkExperienceViewModel(GetVolunteeringOrWorkExperienceItemQueryResult source)
    {
        return new DeleteVolunteeringOrWorkExperienceViewModel
        {
            ApplicationId = source.ApplicationId,
            VolunteeringWorkExperienceId = source.Id,
            Dates = source.EndDate is null ? $"{source.StartDate:MMMM yyyy} onwards" : $"{source.StartDate:MMMM yyyy} to {source.EndDate:MMMM yyyy}",
            Organisation = source.Organisation,
            Description = source.Description
        };
    }
}