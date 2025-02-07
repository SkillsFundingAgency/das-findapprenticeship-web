using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAA.Web.Extensions;

namespace SFA.DAS.FAA.Web.UnitTests.Extensions
{
    [TestFixture]
    public class AddressExtensionsTests
    {
        [Test]
        public void GetLastNonEmptyField_ShouldReturnLastNonEmptyField()
        {
            var address = new Address("Line1", "Line2", "Line3", null, null);

            var result = address.GetLastNonEmptyField();

            Assert.AreEqual("Line3", result);
        }

        [Test]
        public void ToSingleLineFullAddress_ShouldReturnFormattedAddress()
        {
            var address = new Address("Line1", "Line2", "Line3", "Line4", "Postcode");

            var result = address.ToSingleLineFullAddress();

            Assert.AreEqual("Line1, Line2, Line3, Line4, Postcode", result);
        }

        [Test]
        public void ToSingleLineAnonymousAddress_ShouldReturnFormattedAddress()
        {
            var address = new Address("Line1", "Line2", "Line3", "Line4", "AB12 3CD");

            var result = address.ToSingleLineAnonymousAddress();

            Assert.AreEqual("Line4 (AB12)", result);
        }

        [Test]
        public void OrderByCity_ShouldReturnAddressesOrderedByCity()
        {
            var addresses = new List<Address>
            {
                new Address("Line1", null, null, "CityB", null),
                new Address("Line1", null, null, "CityA", null),
            };

            var result = addresses.OrderByCity().ToList();

            Assert.AreEqual("CityA", result[0].AddressLine4);
            Assert.AreEqual("CityB", result[1].AddressLine4);
        }

        [Test]
        public void GetPopulatedAddressLines_ShouldReturnNonEmptyAddressLines()
        {
            var address = new Address("Line1", null, "Line3", "Line4", "Postcode");

            var result = address.GetPopulatedAddressLines().ToList();

            Assert.AreEqual(4, result.Count);
            Assert.Contains("Line1", result);
            Assert.Contains("Line3", result);
            Assert.Contains("Line4", result);
            Assert.Contains("Postcode", result);
        }        
    }
}