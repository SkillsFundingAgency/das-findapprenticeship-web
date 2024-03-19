using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetQualificationTypes;

public class GetQualificationTypesQuery : IRequest<GetQualificationTypesQueryResponse>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
}