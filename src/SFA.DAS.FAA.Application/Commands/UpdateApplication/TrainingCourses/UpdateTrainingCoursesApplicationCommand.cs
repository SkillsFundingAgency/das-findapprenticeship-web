using MediatR;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Application.Commands.UpdateApplication.TrainingCourses;
public record UpdateTrainingCoursesApplicationCommand : IRequest<UpdateTrainingCoursesApplicationCommandResult>
{
    public required Guid ApplicationId { get; init; }
    public required Guid CandidateId { get; init; }
    public SectionStatus TrainingCoursesSectionStatus { get; init; }
}
