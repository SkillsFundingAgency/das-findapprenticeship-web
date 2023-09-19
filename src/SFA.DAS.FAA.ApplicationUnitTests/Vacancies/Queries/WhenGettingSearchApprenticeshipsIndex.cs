using Moq;
using SFA.DAS.FAA.Application.Vacancies.Queries;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace SFA.DAS.FAA.Application.UnitTests.Vacancies.Queries
{
    public class WhenGettingSearchApprenticeshipsIndex
    {
        [Test]
        public async Task Then_Result_Is_Returned()
        {
            // Arrange
            var apiClientMock = new Mock<IApiClient>();
            var configMock = new Mock<FindAnApprenticeshipApi>();

            var handler = new GetSearchApprenticeshipsIndexQueryHandler(apiClientMock.Object, configMock.Object);

            // Mock the response from the API client
            var expectedResponse = new SearchApprenticeshipsIndex { Total = 42 };
            apiClientMock.Setup(client => client.Get<SearchApprenticeshipsIndex>(It.IsAny<GetSearchApprenticeshipsIndexApiRequest>()))
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

