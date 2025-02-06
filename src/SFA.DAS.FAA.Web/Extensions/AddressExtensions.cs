using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Web.Extensions
{
    public static class AddressExtensions
    {
        private const int PostcodeMinLength = 5;
        private const int InCodeLength = 3;

        public static string? GetLastNonEmptyField(this Address address)
        {
            return new[]
            {
                address.AddressLine4,
                address.AddressLine3,
                address.AddressLine2,
                address.AddressLine1,
            }.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
        }

        public static string ToSingleLineFullAddress(this Address address)
        {
            string[] addressArray = [address.AddressLine1, address.AddressLine2, address.AddressLine3, address.AddressLine4, address.Postcode];
            return string.Join(", ", addressArray.Where(a => !string.IsNullOrWhiteSpace(a)).Select(a => a.Trim()));
        }

        public static string ToSingleLineAbridgedAddress(this Address address)
        {
            return $"{address.GetLastNonEmptyField()} ({address.Postcode})";
        }

        public static string ToSingleLineAnonymousAddress(this Address address)
        {
            return $"{address.GetLastNonEmptyField()} ({address.PostcodeAsOutCode()})";
        }

        public static IEnumerable<Address> OrderByCity(this IEnumerable<Address> addresses)
        {
            return addresses.OrderBy(x => x.GetLastNonEmptyField());
        }

        public static IEnumerable<string?> GetPopulatedAddressLines(this Address address)
        {
            return new[]
            {
                address.AddressLine1,
                address.AddressLine2,
                address.AddressLine3,
                address.AddressLine4,
                address.Postcode
            }.Where(x => !string.IsNullOrEmpty(x?.Trim()));
        }

        private static string? PostcodeAsOutCode(this Address address)
        {
            var postcode = address.Postcode?.Replace(" ", "");

            return postcode is {Length: < PostcodeMinLength} ? null : postcode?[..^InCodeLength];
        }
    }
}
