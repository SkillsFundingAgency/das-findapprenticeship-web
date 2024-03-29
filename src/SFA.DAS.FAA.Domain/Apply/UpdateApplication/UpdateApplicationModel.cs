﻿using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Domain.Apply.UpdateApplication;

public record UpdateApplicationModel
{
    public SectionStatus WorkHistorySectionStatus { get; set; }
    public SectionStatus TrainingCoursesSectionStatus { get; set; }
}