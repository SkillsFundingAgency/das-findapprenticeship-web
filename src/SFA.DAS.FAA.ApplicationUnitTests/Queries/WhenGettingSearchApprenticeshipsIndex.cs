using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
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
        public async Task Then_Result_Is_Returned(
            GetSearchApprenticeshipsIndexQuery query,
            SearchApprenticeshipsApiResponse expectedResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            GetSearchApprenticeshipsIndexQueryHandler handler)
        {
            // Arrange
            apiClientMock.Setup(client =>
                    client.Get<SearchApprenticeshipsApiResponse>(
                        It.Is<GetSearchApprenticeshipsIndexApiRequest>(c =>
                            c.GetUrl.Contains(query.LocationSearchTerm!) && c.GetUrl.Contains(query.CandidateId.ToString()!))))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }
        [Test, MoqAutoData]
        public async Task Then_Result_Is_Returned_For_No_Searches_Or_Location(
            GetSearchApprenticeshipsIndexQuery query,
            SearchApprenticeshipsApiResponse expectedResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            GetSearchApprenticeshipsIndexQueryHandler handler)
        {
            // Arrange
            expectedResponse.Location = null;
            expectedResponse.SavedSearches = [];
            apiClientMock.Setup(client =>
                    client.Get<SearchApprenticeshipsApiResponse>(
                        It.Is<GetSearchApprenticeshipsIndexApiRequest>(c =>
                            c.GetUrl.Contains(query.LocationSearchTerm!) && c.GetUrl.Contains(query.CandidateId.ToString()!))))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }
    }
}

