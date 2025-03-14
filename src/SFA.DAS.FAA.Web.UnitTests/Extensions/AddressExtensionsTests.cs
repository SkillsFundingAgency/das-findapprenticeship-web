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

            result.Should().Be("Line3");
        }

        [Test]
        public void ToSingleLineFullAddress_ShouldReturnFormattedAddress()
        {
            var address = new Address("Line1", "Line2", "Line3", "Line4", "Postcode");

            var result = address.ToSingleLineFullAddress();

            result.Should().Be("Line1, Line2, Line3, Line4, Postcode");
        }

        [Test]
        public void ToSingleLineAnonymousAddress_ShouldReturnFormattedAddress()
        {
            var address = new Address("Line1", "Line2", "Line3", "Line4", "AB12 3CD");

            var result = address.ToSingleLineAnonymousAddress();

            result.Should().Be("Line4 (AB12)");
        }
        
        [Test]
        public void ToSingleLineAnonymousAddress_With_No_City_Should_Return_Postcode_Only()
        {
            var address = new Address(null, null, null, null, "AB12 3CD");

            var result = address.ToSingleLineAnonymousAddress();

            result.Should().Be("AB12");
        }

        [Test]
        public void ToSingleLineAnonymousAddress_Should_Work_With_Outcode_Only()
        {
            var address = new Address(null, null, null, null, "B1");

            var result = address.ToSingleLineAnonymousAddress();

            result.Should().Be("B1");
        }

        [Test]
        public void OrderByCity_ShouldReturnAddressesOrderedByCity()
        {
            var addresses = new List<Address>
            {
                new ("Line1", null, null, "CityB", null),
                new ("Line1", null, null, "CityA", null),
            };

            var result = addresses.OrderByCity().ToList();

            result[0].AddressLine4.Should().Be("CityA");
            result[1].AddressLine4.Should().Be("CityB");
        }
        
        [Test]
        public void ToSingleLineAnonymousFullAddress_ShouldReturnFormattedAddress()
        {
            var address = new Address("Line1", "Line2", "Line3", "Line4", "AB12 3CD");

            var result = address.ToSingleLineAnonymousFullAddress();

            result.Should().Be("Line4, AB12", result);
        }
        
        [Test]
        public void ToSingleLineAnonymousFullAddress_With_No_City_Should_Return_Postcode_Only()
        {
            var address = new Address(null, null, null, null, "AB12 3CD");

            var result = address.ToSingleLineAnonymousFullAddress();

            result.Should().Be("AB12", result);
        }
        
        [Test]
        public void ToSingleLineAnonymousFullAddress_Should_Work_With_Outcode_Only()
        {
            var address = new Address(null, null, null, null, "B1");

            var result = address.ToSingleLineAnonymousFullAddress();

            result.Should().Be("B1", result);
        }

        [Test]
        public void GetPopulatedAddressLines_ShouldReturnNonEmptyAddressLines()
        {
            var address = new Address("Line1", null, "Line3", "Line4", "Postcode");

            var result = address.GetPopulatedAddressLines().ToList();

            result.Should().HaveCount(4);
            result.Should().Contain("Line1");
            result.Should().Contain("Line3");
            result.Should().Contain("Line4");
            result.Should().Contain("Postcode");
        }        
    }
}