namespace SFA.DAS.FAA.Domain.User;
public class GetCandidateAddressApiResponse
{
    public bool IsAddressFromLookup { get; set; }
    public string? Postcode { get; set; }
    public string AddressLine1 { get; set; } = null!;
    public string? AddressLine2 { get; set; }
    public string Town { get; set; } = null!;
    public string? County { get; set; }
}
