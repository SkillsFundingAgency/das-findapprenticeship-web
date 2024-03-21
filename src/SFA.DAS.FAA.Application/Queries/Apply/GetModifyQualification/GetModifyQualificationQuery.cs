using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetModifyQualification;

public class GetModifyQualificationQuery : IRequest<GetModifyQualificationQueryResult>
{
    public Guid QualificationReferenceId { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
    public Guid? QualificationId { get; set; }
}