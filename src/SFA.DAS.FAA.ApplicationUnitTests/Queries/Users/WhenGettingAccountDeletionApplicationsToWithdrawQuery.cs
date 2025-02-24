using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.User.GetAccountDeletionApplicationsToWithdraw;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Users
{
    public class WhenGettingAccountDeletionApplicationsToWithdrawQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Result_Is_Returned(
            GetAccountDeletionApplicationsToWithdrawQuery query,
            GetAccountDeletionApplicationsToWithdrawApiResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            GetAccountDeletionApplicationsToWithdrawQueryHandler handler)
        {
            // Arrange
            var apiRequestUri = new GetAccountDeletionApplicationsToWithdrawApiRequest(query.CandidateId);

            apiClientMock.Setup(client =>
                    client.Get<GetAccountDeletionApplicationsToWithdrawApiResponse>(
                        It.Is<GetAccountDeletionApplicationsToWithdrawApiRequest>(c =>
                            c.GetUrl == apiRequestUri.GetUrl)))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.SubmittedApplications.Should().BeEquivalentTo(apiResponse.SubmittedApplications, options => options
                .Excluding(op => op.Status)
                .Excluding(x => x.Address)
                .Excluding(x => x.OtherAddresses)
            );
        }
    }
}