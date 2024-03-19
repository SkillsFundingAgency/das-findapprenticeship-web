using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetAddQualification;

public class GetModifyQualificationQuery : IRequest<GetModifyQualificationQueryResult>
{
    public Guid QualificationReferenceId { get; set; }
    public Guid ApplicationId { get; set; }
}