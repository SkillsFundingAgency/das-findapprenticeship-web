using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAA.Web.Extensions;

namespace SFA.DAS.FAA.Web.Tests.Extensions
{
    [TestFixture]
    public class AddressExtensionsTests
    {
        [Test]
        public void GetLastNonEmptyField_ShouldReturnLastNonEmptyField()
        {
            var address = new Address
            {
                AddressLine1 = "Line1",
                AddressLine2 = "Line2",
                AddressLine3 = "Line3",
                AddressLine4 = null
            };

            var result = address.GetLastNonEmptyField();

            Assert.AreEqual("Line3", result);
        }

        [Test]
        public void ToSingleLineFullAddress_ShouldReturnFormattedAddress()
        {
            var address = new Address
            {
                AddressLine1 = "Line1",
                AddressLine2 = "Line2",
                AddressLine3 = "Line3",
                AddressLine4 = "Line4",
                Postcode = "Postcode"
            };

            var result = address.ToSingleLineFullAddress();

            Assert.AreEqual("Line1, Line2, Line3, Line4, Postcode", result);
        }

        [Test]
        public void ToSingleLineAnonymousAddress_ShouldReturnFormattedAddress()
        {
            var address = new Address
            {
                AddressLine1 = "Line1",
                AddressLine2 = "Line2",
                AddressLine3 = "Line3",
                AddressLine4 = "Line4",
                Postcode = "AB12 3CD"
            };

            var result = address.ToSingleLineAnonymousAddress();

            Assert.AreEqual("Line4 (AB12)", result);
        }

        [Test]
        public void OrderByCity_ShouldReturnAddressesOrderedByCity()
        {
            var addresses = new List<Address>
            {
                new Address { AddressLine1 = "Line1", AddressLine4 = "CityB" },
                new Address { AddressLine1 = "Line1", AddressLine4 = "CityA" }
            };

            var result = addresses.OrderByCity().ToList();

            Assert.AreEqual("CityA", result[0].AddressLine4);
            Assert.AreEqual("CityB", result[1].AddressLine4);
        }

        [Test]
        public void GetPopulatedAddressLines_ShouldReturnNonEmptyAddressLines()
        {
            var address = new Address
            {
                AddressLine1 = "Line1",
                AddressLine2 = null,
                AddressLine3 = "Line3",
                AddressLine4 = "Line4",
                Postcode = "Postcode"
            };

            var result = address.GetPopulatedAddressLines().ToList();

            Assert.AreEqual(4, result.Count);
            Assert.Contains("Line1", result);
            Assert.Contains("Line3", result);
            Assert.Contains("Line4", result);
            Assert.Contains("Postcode", result);
        }        
    }
}