using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Domain.UnitTests.Users;
public class WhenBuildingUpdateAddressApiRequest
{
    [Test, AutoData]
    public void Then_Then_Request_Is_Built(
        string govIdentifier, string email,
        string addressLine1, string addressLine2,
        string addressLine3, string addressLine4,
        string postcode, string uprn)
    {
        var data = new CreateUserAddressApiRequestData()
        {
            Email = email,
            AddressLine1 = addressLine1,
            AddressLine2 = addressLine2,
            AddressLine3 = addressLine3,
            AddressLine4 = addressLine4,
            Postcode = postcode,
            Uprn = uprn
        };
        var actual = new CreateUserAddressApiRequest(govIdentifier, data);

        actual.PostUrl.Should().Be($"users/{govIdentifier}/select-address");
    }
}
