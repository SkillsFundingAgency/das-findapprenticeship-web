using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.UserAddress;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.UserAddress;
public class WhenHandlingCreateUserAddressCommand
{
    [Test, MoqAutoData]
    public async Task Handle_WhenApiClientReturnsSuccess_ShouldReturnUnit(
        UpdateAddressCommand command,
        [Frozen] Mock<IApiClient> apiClientMock,
        UpdateAddressCommandHandler handler)
    {
        var request = new CreateUserAddressApiRequest(command.GovUkIdentifier, new CreateUserAddressApiRequestData
        {
            Email = command.Email,
            AddressLine1 = command.AddressLine1,
            AddressLine2 = command.AddressLine2,
            AddressLine3 = command.AddressLine3,
            AddressLine4 = command.AddressLine4,
            Postcode = command.Postcode,
            Uprn = command.Uprn
        });
        apiClientMock.Setup(x =>
                x.PostWithResponseCode(It.Is<CreateUserAddressApiRequest>(c =>
                c.PostUrl.Equals(request.PostUrl)
                    && ((CreateUserAddressApiRequestData)c.Data).Email.Equals(command.Email)
                )))
            .Returns(() => Task.CompletedTask);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
    }
}
