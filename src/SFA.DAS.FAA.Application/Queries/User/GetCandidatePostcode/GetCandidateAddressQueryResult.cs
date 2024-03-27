namespace SFA.DAS.FAA.Application.Queries.User.GetCandidatePostcode;
public class GetCandidateAddressQueryResult
{
    public string? Postcode { get; set; }
    public string AddressLine1 { get; set; } = null!;
    public string? AddressLine2 { get; set; }
    public string Town { get; set; } = null!;
    public string? County { get; set; }
}
