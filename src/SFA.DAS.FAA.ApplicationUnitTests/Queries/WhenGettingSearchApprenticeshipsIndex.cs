﻿using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries
{
    public class WhenGettingSearchApprenticeshipsIndex
    {
        [Test, MoqAutoData]
        public async Task Then_Result_Is_Returned()
        {
            // Arrange
            var apiClientMock = new Mock<IApiClient>();

            var handler = new GetSearchApprenticeshipsIndexQueryHandler(apiClientMock.Object);

            // Mock the response from the API client
            var expectedResponse = new SearchApprenticeshipsApiResponse();
            apiClientMock.Setup(client => client.Get<SearchApprenticeshipsApiResponse>(It.IsAny<GetSearchApprenticeshipsIndexApiRequest>()))
                .ReturnsAsync(expectedResponse);

            var query = new GetSearchApprenticeshipsIndexQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponse.Total, result.Total);
        }
    }
}
