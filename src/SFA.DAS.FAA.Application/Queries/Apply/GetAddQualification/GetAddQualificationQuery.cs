using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetAddQualification;

public class GetAddQualificationQuery : IRequest<GetAddQualificationQueryResult>
{
    public Guid QualificationReferenceId { get; set; }
    public Guid ApplicationId { get; set; }
}