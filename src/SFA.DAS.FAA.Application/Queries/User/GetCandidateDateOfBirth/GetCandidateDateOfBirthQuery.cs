using MediatR;

namespace SFA.DAS.FAA.Application.Queries.User.GetCandidateDateOfBirth;
public class GetCandidateDateOfBirthQuery : IRequest<GetCandidateDateOfBirthQueryResult>
{
    public string GovUkIdentifier { get; set; }
}
