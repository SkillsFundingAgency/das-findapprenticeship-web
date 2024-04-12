using MediatR;

namespace CreateAccount.GetCandidateDateOfBirth;
public class GetCandidateDateOfBirthQuery : IRequest<GetCandidateDateOfBirthQueryResult>
{
    public Guid CandidateId { get; set; }
}
