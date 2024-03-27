using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Domain.UnitTests.Users;
public class WhenBuildingUpdatePhoneNumberApiRequest
{
    [Test, AutoData]
    public void Then_Then_Request_Is_Built(string govIdentifier, string email, string phoneNumber)
    {
        var data = new CreateUserPhoneNumberApiRequestData()
        {
            Email = email,
            PhoneNumber = phoneNumber
        };
        var actual = new CreateUserPhoneNumberApiRequest(govIdentifier, data);

        actual.PostUrl.Should().Be($"users/{govIdentifier}/phone-number");
    }
}
