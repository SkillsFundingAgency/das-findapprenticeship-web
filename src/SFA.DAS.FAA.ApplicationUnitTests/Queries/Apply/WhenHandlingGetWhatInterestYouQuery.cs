using SFA.DAS.FAA.Application.Queries.Apply.GetWhatInterestsYou;
using SFA.DAS.FAA.Domain.Apply.WhatInterestsYou;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply
{
    public class WhenHandlingWhatInterestsYouQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Result_Is_Returned(
            GetWhatInterestsYouQuery query,
            GetWhatInterestsYouApiResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            GetWhatInterestsYouQueryHandler handler)
        {
            // Arrange
            var apiRequestUri = new GetWhatInterestsYouApiRequest(query.ApplicationId, query.CandidateId);

            apiClientMock.Setup(client =>
                    client.Get<GetWhatInterestsYouApiResponse>(
                        It.Is<GetWhatInterestsYouApiRequest>(c =>
                            c.GetUrl == apiRequestUri.GetUrl)))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(apiResponse);
        }
    }
}
