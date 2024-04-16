using SFA.DAS.FAA.Domain.User;

namespace CreateAccount.GetCandidateDateOfBirth;
public class GetCandidateDateOfBirthQueryResult
{
    public DateTime? DateOfBirth { get; set; }

    public static implicit operator GetCandidateDateOfBirthQueryResult(GetCandidateDateOfBirthApiResponse source)
    {
        return new GetCandidateDateOfBirthQueryResult
        {
            DateOfBirth = source.DateOfBirth
        };
    }
}
