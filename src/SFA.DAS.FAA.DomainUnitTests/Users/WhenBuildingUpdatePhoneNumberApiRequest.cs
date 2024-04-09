using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Domain.UnitTests.Users;
public class WhenBuildingUpdatePhoneNumberApiRequest
{
    [Test, AutoData]
    public void Then_Then_Request_Is_Built(Guid candidateId, string email, string phoneNumber)
    {
        var data = new CreateUserPhoneNumberApiRequestData()
        {
            Email = email,
            PhoneNumber = phoneNumber
        };
        var actual = new CreateUserPhoneNumberApiRequest(candidateId, data);

        actual.PostUrl.Should().Be($"users/{candidateId}/phone-number");
    }
}
