using MediatR;
using SFA.DAS.FAA.Domain.Apply.Qualifications;

namespace SFA.DAS.FAA.Application.Commands.UpsertQualification;

public class UpsertQualificationCommand : IRequest<Unit>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
    public Guid QualificationReferenceId { get; set; }
    
    public List<PostUpsertQualificationsApiRequest.Subject> Subjects { get; set; }
}