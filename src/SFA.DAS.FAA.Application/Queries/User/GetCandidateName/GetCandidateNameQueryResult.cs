using SFA.DAS.FAA.Domain.User;

namespace CreateAccount.GetCandidateName;
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
