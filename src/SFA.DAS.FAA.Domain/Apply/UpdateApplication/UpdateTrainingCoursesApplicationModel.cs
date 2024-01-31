using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Domain.Apply.UpdateApplication;
public record UpdateTrainingCoursesApplicationModel
{
    public SectionStatus TrainingCoursesSectionStatus { get; set; }
}
