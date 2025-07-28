using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.CreateAccount.PhoneNumber;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.CreateAccount.UserPhoneNumber;
public class WhenHandlingCreateUserPhoneNumberCommand
{
    [Test, MoqAutoData]
    public async Task Handle_WhenApiClientReturnsSuccess_ShouldReturnUnit(
        Guid candidateId,
        string email,
        string phoneNumber,
        [Frozen] Mock<IApiClient> apiClientMock,
        UpdatePhoneNumberCommandHandler handler)
    {
        var command = new UpdatePhoneNumberCommand()
        {
            CandidateId = candidateId,
            Email = email,
            PhoneNumber = phoneNumber
        };

        var request = new CreateUserPhoneNumberApiRequest(command.CandidateId, new CreateUserPhoneNumberApiRequestData
        {
            Email = command.Email,
            PhoneNumber = command.PhoneNumber
        });
        apiClientMock.Setup(x =>
                x.Post(It.Is<CreateUserPhoneNumberApiRequest>(c =>
                c.PostUrl.Equals(request.PostUrl)
                    && ((CreateUserPhoneNumberApiRequestData)c.Data).Email.Equals(command.Email)
                    && ((CreateUserPhoneNumberApiRequestData)c.Data).PhoneNumber.Equals(command.PhoneNumber)
                )))
            .Returns(() => Task.CompletedTask);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
    }
}
