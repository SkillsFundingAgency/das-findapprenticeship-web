using MediatR;

namespace SFA.DAS.FAA.Application.Queries.User.GetCandidateDateOfBirth;
public class GetCandidateDateOfBirthQuery : IRequest<GetCandidateDateOfBirthQueryResult>
{
    public Guid CandidateId { get; set; }
}
