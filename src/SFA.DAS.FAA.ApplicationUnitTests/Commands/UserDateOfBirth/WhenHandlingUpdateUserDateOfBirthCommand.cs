using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.UserDateOfBirth;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.UserDateOfBirth;
public class WhenHandlingUpdateUserDateOfBirthCommand
{
    [Test, MoqAutoData]
    public async Task Handle_WhenApiClientReturnsSuccess_ShouldReturnUnit(
        [Frozen] Mock<IApiClient> apiClientMock,
        UpdateDateOfBirthCommandHandler handler,
        Guid candidateId,
        string email,
        DateTime dob)
    {
        var command = new UpdateDateOfBirthCommand()
        {
            CandidateId = candidateId,
            Email = email,
            DateOfBirth = dob
        };
           
        var request = new UpdateDateOfBirthApiRequest(command.CandidateId, new UpdateDateOfBirthRequestData
        {
            Email = command.Email,
            DateOfBirth = command.DateOfBirth
        });
        apiClientMock.Setup(x => 
                x.PostWithResponseCode(It.Is<UpdateDateOfBirthApiRequest>(c =>
                c.PostUrl.Equals(request.PostUrl)
                    && ((UpdateDateOfBirthRequestData)c.Data).Email.Equals(command.Email)
                    && ((UpdateDateOfBirthRequestData)c.Data).DateOfBirth.Equals(command.DateOfBirth)
                )))
            .Returns(() => Task.CompletedTask);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
    }
}
