using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.UserName;
using SFA.DAS.FAA.Domain;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.FAA.Infrastructure.Api;
using SFA.DAS.Testing.AutoFixture;
using System.Net;


namespace SFA.DAS.FAA.Application.UnitTests.Commands.UserName
{
    public class WhenHandlingUpdateNameCommand
    {
        [Test, MoqAutoData]
        public async Task Handle_WhenApiClientReturnsSuccess_ShouldReturnUnit(
        [Frozen] Mock<IApiClient> apiClientMock,
        UpdateNameCommandHandler handler,
        UpdateNameCommand command)
        {

            // Arrange
            var request = new UpdateNameApiRequest(command.CandidateId, new UpdateNameApiRequestData
            {
                Email = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName
            });
            apiClientMock.Setup(x => x.PutWithResponseCode<NullResponse>(It.Is<UpdateNameApiRequest>(c =>
                    c.PutUrl.Equals(request.PutUrl)
                        && ((UpdateNameApiRequestData)c.Data).FirstName.Equals(command.FirstName)
                        && ((UpdateNameApiRequestData)c.Data).LastName.Equals(command.LastName)
                        && ((UpdateNameApiRequestData)c.Data).Email.Equals(command.Email)
                    )))
                .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.Accepted, ""));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
        }



        [Test, MoqAutoData]
        public void Handle_WhenApiClientReturnsError_ShouldThrowInvalidOperationException(
            [Frozen] Mock<IApiClient> apiClientMock,
            UpdateNameCommandHandler handler,
            UpdateNameCommand command)
        {
            // Arrange
            apiClientMock.Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<UpdateNameApiRequest>()))
                .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.BadRequest, "error"));

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
