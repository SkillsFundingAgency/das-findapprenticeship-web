﻿namespace SFA.DAS.FAA.Domain.Apply.GetInterviewAdjustments;
public class GetInterviewAdjustmentsApiResponse
{
    public Guid ApplicationId { get; set; }
    public string? InterviewAdjustmentsDescription { get; set; }
    public bool? Status { get; set; }
}
