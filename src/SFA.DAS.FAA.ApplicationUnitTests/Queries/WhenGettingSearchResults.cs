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
        VacancySort vacancySort,
        GetSearchResultsQuery query,
        GetSearchResultsApiResponse expectedResponse,
        [Frozen] Mock<IApiClient> apiClient,
        GetSearchResultsQueryHandler handler)
    {
        query.Sort = vacancySort.ToString();
        
        // Mock the response from the API client
        var expectedGetUrl = new GetSearchResultsApiRequest(query.Location, query.SelectedRouteIds, query.SelectedLevelIds, query.Distance, query.SearchTerm, query.PageNumber, query.PageSize, vacancySort, query.DisabilityConfident, query.CandidateId);
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
            result.Levels.Should().BeEquivalentTo(expectedResponse.Levels);
            result.Location.Should().BeEquivalentTo(expectedResponse.Location);
            result.PageNumber.Should().Be(expectedResponse.PageNumber);
            result.Sort.Should().Be(vacancySort.ToString());
            result.VacancyReference.Should().Be(expectedResponse.VacancyReference);
            result.DisabilityConfident.Should().Be(expectedResponse.DisabilityConfident);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_If_The_Sort_Is_Not_Valid_Defaults_To_Distance(
        GetSearchResultsQuery query,
        GetSearchResultsApiResponse expectedResponse,
        [Frozen] Mock<IApiClient> apiClient,
        GetSearchResultsQueryHandler handler)
    {
        // Mock the response from the API client
        var expectedGetUrl = new GetSearchResultsApiRequest(query.Location, query.SelectedRouteIds, query.SelectedLevelIds, query.Distance, query.SearchTerm, query.PageNumber, query.PageSize, VacancySort.DistanceAsc, query.DisabilityConfident, query.CandidateId);
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
            result.Levels.Should().BeEquivalentTo(expectedResponse.Levels);
            result.Location.Should().BeEquivalentTo(expectedResponse.Location);
            result.PageNumber.Should().Be(expectedResponse.PageNumber);
            result.Sort.Should().Be(VacancySort.DistanceAsc.ToString());
            result.VacancyReference.Should().Be(expectedResponse.VacancyReference);
            result.DisabilityConfident.Should().Be(expectedResponse.DisabilityConfident);
        }
    }
    
    
    [Test, MoqAutoData]
    public async Task Then_If_The_Sort_Is_Empty_Defaults_To_Distance(
        GetSearchResultsQuery query,
        GetSearchResultsApiResponse expectedResponse,
        [Frozen] Mock<IApiClient> apiClient,
        GetSearchResultsQueryHandler handler)
    {
        // Mock the response from the API client
        query.Sort = string.Empty;
        var expectedGetUrl = new GetSearchResultsApiRequest(query.Location, query.SelectedRouteIds, query.SelectedLevelIds, query.Distance, query.SearchTerm, query.PageNumber, query.PageSize, VacancySort.DistanceAsc, query.DisabilityConfident, query.CandidateId);
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
            result.Levels.Should().BeEquivalentTo(expectedResponse.Levels);
            result.Location.Should().BeEquivalentTo(expectedResponse.Location);
            result.PageNumber.Should().Be(expectedResponse.PageNumber);
            result.Sort.Should().Be(VacancySort.DistanceAsc.ToString());
            result.VacancyReference.Should().Be(expectedResponse.VacancyReference);
            result.DisabilityConfident.Should().Be(expectedResponse.DisabilityConfident);
        }
    }
}