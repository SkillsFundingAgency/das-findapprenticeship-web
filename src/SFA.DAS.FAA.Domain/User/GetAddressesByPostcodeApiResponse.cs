using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.User;
public class GetAddressesByPostcodeApiResponse
{
    public string? Postcode { get; set; }
    public string? Uprn { get; set; }

    [JsonProperty("addresses")]
    public List<AddressListItem>? Addresses { get; set; }

    public class AddressListItem
    {
        public string Uprn { get; set; }
        public string Organisation { get; set; }
        public string Premises { get; set; }
        public string Thoroughfare { get; set; }
        public string Locality { get; set; }
        public string PostTown { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public double? Match { get; set; }
    }
}
