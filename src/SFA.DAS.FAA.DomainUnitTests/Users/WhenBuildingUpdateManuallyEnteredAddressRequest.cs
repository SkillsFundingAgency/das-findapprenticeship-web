using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Domain.UnitTests.Users;
public class WhenBuildingUpdateManuallyEnteredAddressRequest
{
    [Test, AutoData]
    public void Then_Then_Request_Is_Built(
        Guid candidateId, string email,
        string addressLine1, string addressLine2,
        string townOrCity, string county,
        string postcode)
    {
        var data = new CreateUserManuallyEnteredAddressApiRequestData()
        {
            Email = email,
            AddressLine1 = addressLine1,
            AddressLine2 = addressLine2,
            TownOrCity = townOrCity,
            County = county,
            Postcode = postcode
        };
        var actual = new CreateUserManuallyEnteredAddressApiRequest(candidateId, data);

        actual.PostUrl.Should().Be($"users/{candidateId}/enter-address");
    }
}
