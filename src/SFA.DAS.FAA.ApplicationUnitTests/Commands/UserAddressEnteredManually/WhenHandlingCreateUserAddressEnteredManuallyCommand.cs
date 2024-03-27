using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.ManuallyEnteredAddress;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.UserAddressEnteredManually;
public class WhenHandlingCreateUserAddressEnteredManuallyCommand
{
    [Test, MoqAutoData]
    public async Task Handle_WhenApiClientReturnsSuccess_ShouldReturnUnit(
        UpdateManuallyEnteredAddressCommand command,
        [Frozen] Mock<IApiClient> apiClientMock,
        UpdateManuallyEnteredAddressCommandHandler handler)
    {
        var request = new CreateUserManuallyEnteredAddressApiRequest(command.CandidateId, new CreateUserManuallyEnteredAddressApiRequestData
        {
            Email = command.Email,
            AddressLine1 = command.AddressLine1,
            AddressLine2 = command.AddressLine2,
            TownOrCity = command.TownOrCity,
            County = command.County,
            Postcode = command.Postcode
        });
        apiClientMock.Setup(x =>
                x.PostWithResponseCode(It.Is<CreateUserManuallyEnteredAddressApiRequest>(c =>
                c.PostUrl.Equals(request.PostUrl)
                    && ((CreateUserAddressApiRequestData)c.Data).Email.Equals(command.Email)
                )))
            .Returns(() => Task.CompletedTask);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
    }
}
