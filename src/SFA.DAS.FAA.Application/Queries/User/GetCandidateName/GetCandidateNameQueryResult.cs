using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.Queries.User.GetCandidateName;
public class GetCandidateNameQueryResult
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public static implicit operator GetCandidateNameQueryResult(GetCandidateNameApiResponse source)
    {
        return new GetCandidateNameQueryResult
        {
            FirstName = source.FirstName,
            LastName = source.LastName
        };
    }
}
