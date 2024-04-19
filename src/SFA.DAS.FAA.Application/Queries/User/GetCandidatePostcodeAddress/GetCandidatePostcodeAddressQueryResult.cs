using SFA.DAS.FAA.Domain.User;

namespace CreateAccount.GetCandidatePostcodeAddress;
public class GetCandidatePostcodeAddressQueryResult
{
    public bool PostcodeExists { get; set; }

    public static implicit operator GetCandidatePostcodeAddressQueryResult(GetCandidatePostcodeAddressApiResponse source)
    {
        return new GetCandidatePostcodeAddressQueryResult()
        {
            PostcodeExists = source.PostcodeExists
        };
    }
}
