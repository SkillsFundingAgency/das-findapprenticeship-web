namespace SFA.DAS.FAA.Domain.User;

public class GetCandidatePhoneNumberApiResponse
{
    public bool IsAddressFromLookup { get; set; }
    public string? PhoneNumber { get; set; }
}