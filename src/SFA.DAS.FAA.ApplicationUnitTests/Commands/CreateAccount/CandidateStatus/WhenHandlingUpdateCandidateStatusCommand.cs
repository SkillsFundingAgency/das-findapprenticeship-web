using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.CreateAccount.CandidateStatus;
using SFA.DAS.FAA.Domain.Candidates;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.FAA.Domain.Candidates.UpdateCandidateStatusApiRequest;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.CreateAccount.CandidateStatus
{
    public class WhenHandlingUpdateCandidateStatusCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Put_Is_Sent_And_Data_Returned(
            string govUkId,
            UpdateCandidateStatusCommand command,
            PutCandidateApiResponse putCandidateApiResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            UpdateCandidateStatusCommandHandler handler)
        {
            putCandidateApiResponse.Status = UserStatus.InProgress;
            command.GovIdentifier = govUkId;
            command.CandidateEmail = putCandidateApiResponse.Email;

            var candidateRequest = new PutCandidateApiRequest(command.GovIdentifier, new PutCandidateApiRequestData
            {
                Email = command.CandidateEmail
            });
            apiClientMock.Setup(x =>
                    x.Put<PutCandidateApiResponse>(
                        It.Is<PutCandidateApiRequest>(c => c.PutUrl.Equals(candidateRequest.PutUrl)
                                                           && ((PutCandidateApiRequestData)c.Data).Email == command.CandidateEmail
                        )))
                .ReturnsAsync(putCandidateApiResponse);

            var request = new UpdateCandidateStatusApiRequest(command.GovIdentifier, new UpdateCandidateStatusApiRequest.UpdateCandidateStatusApiRequestData
            {
                Email = command.CandidateEmail,
                Status = UserStatus.Completed
            });
            apiClientMock.Setup(x =>
                    x.Post(It.Is<UpdateCandidateStatusApiRequest>(c =>
                        c.PostUrl.Equals(request.PostUrl)
                        && ((UpdateCandidateStatusApiRequestData)c.Data).Email.Equals(command.CandidateEmail)
                    )))
                .Returns(() => Task.CompletedTask);


            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().BeEquivalentTo(putCandidateApiResponse, options => options
                .Excluding(c => c.Status)
                .Excluding(c => c.PhoneNumber)
                .Excluding(c => c.IsEmailAddressMigrated)
            );
            apiClientMock.Verify(x =>
                x.Post(It.Is<UpdateCandidateStatusApiRequest>(c =>
                    c.PostUrl.Equals(request.PostUrl)
                    && ((UpdateCandidateStatusApiRequestData)c.Data).Email.Equals(command.CandidateEmail)
                )), Times.Once());
        }

        [Test]
        [MoqInlineAutoData(UserStatus.Incomplete)]
        [MoqInlineAutoData(UserStatus.Completed)]
        public async Task Then_The_Status_Not_InProgress_Put_Is_Never_Sent_And_Data_Returned(
            UserStatus status,
            string govUkId,
            UpdateCandidateStatusCommand command,
            PutCandidateApiResponse putCandidateApiResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            UpdateCandidateStatusCommandHandler handler)
        {
            putCandidateApiResponse.Status = status;
            command.GovIdentifier = govUkId;
            command.CandidateEmail = putCandidateApiResponse.Email;

            var candidateRequest = new PutCandidateApiRequest(command.GovIdentifier, new PutCandidateApiRequestData
            {
                Email = command.CandidateEmail
            });
            apiClientMock.Setup(x =>
                    x.Put<PutCandidateApiResponse>(
                        It.Is<PutCandidateApiRequest>(c => c.PutUrl.Equals(candidateRequest.PutUrl)
                                                           && ((PutCandidateApiRequestData)c.Data).Email == command.CandidateEmail
                        )))
                .ReturnsAsync(putCandidateApiResponse);

            var request = new UpdateCandidateStatusApiRequest(command.GovIdentifier, new UpdateCandidateStatusApiRequest.UpdateCandidateStatusApiRequestData
            {
                Email = command.CandidateEmail,
                Status = UserStatus.Completed
            });

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().BeEquivalentTo(putCandidateApiResponse, options => options
                .Excluding(c => c.Status)
                .Excluding(c => c.PhoneNumber)
                .Excluding(c => c.IsEmailAddressMigrated)
            );
            apiClientMock.Verify(x =>
                x.Post(It.Is<UpdateCandidateStatusApiRequest>(c =>
                    c.PostUrl.Equals(request.PostUrl)
                    && ((UpdateCandidateStatusApiRequestData)c.Data).Email.Equals(command.CandidateEmail)
                )), Times.Never());
        }
    }
}
