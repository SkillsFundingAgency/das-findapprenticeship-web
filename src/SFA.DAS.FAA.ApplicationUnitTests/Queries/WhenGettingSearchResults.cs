using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.GetSearchResults;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries;

public class WhenGettingSearchResults
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned(
        GetSearchResultsQuery query,
        GetSearchResultsApiResponse expectedResponse,
        [Frozen] Mock<IApiClient> apiClient,
        GetSearchResultsQueryHandler handler)
    {
        query.Sort = string.Empty;
        var sort = string.IsNullOrEmpty(query.Sort)
            ? VacancySort.DistanceAsc
            : (VacancySort)Enum.Parse(typeof(VacancySort), query.Sort, true);

        // Mock the response from the API client
        var expectedGetUrl = new GetSearchResultsApiRequest(query.Location, query.SelectedRouteIds, query.Distance, query.SearchTerm, query.PageNumber, query.PageSize, sort);
        apiClient.Setup(client => client.Get<GetSearchResultsApiResponse>(It.Is<GetSearchResultsApiRequest>(c=>c.GetUrl.Equals(expectedGetUrl.GetUrl))))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponse.TotalFound, result.Total);
            Assert.AreEqual(expectedResponse.Vacancies, result.Vacancies);
            result.Routes.Should().BeEquivalentTo(expectedResponse.Routes);
            result.Location.Should().BeEquivalentTo(expectedResponse.Location);
            result.PageNumber.Should().Be(expectedResponse.PageNumber);
            result.PageSize.Should().Be(expectedResponse.PageSize);
            result.Sort.Should().Be(sort.ToString());
            result.VacancyReference.Should().Be(expectedResponse.VacancyReference);
        }
    }
}