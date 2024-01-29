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
            apiClientMock.Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<UpdateNameApiRequest>()))
                .ReturnsAsync(new ApiResponse<NullResponse>
                {
                    StatusCode = HttpStatusCode.OK
                });

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
                .ReturnsAsync(new ApiResponse<NullResponse>
                {
                    StatusCode = HttpStatusCode.BadRequest
                });

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }
    }
}
