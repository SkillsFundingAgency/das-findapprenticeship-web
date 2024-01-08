using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.Models.SearchResults;
using SFA.DAS.FAA.Web.Services;

namespace SFA.DAS.FAA.Web.UnitTests.Services
{
    [TestFixture]
    public class FilterBuilderTests
    {
        private const string SearchResultsUrl = "searchResults";

        [Test]
        public void BuildFilterChoicesForNoFilters()
        {
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns(SearchResultsUrl);

            var request = new GetSearchResultsRequest
            {
                SearchTerm = null,
                PageSize = null,
                PageNumber = null
            };

            var actual = FilterBuilder.BuildFullQueryString(request, mockUrlHelper.Object);
            actual.Should().NotBeNull();
        }

        [TestCase(null, "", 0)]
        [TestCase("1", "Job Category", 1)]
        [TestCase("2", "Job Category", 1)]
        [TestCase("3", "Job Category", 1)]
        public void BuildJobCategorySearchFiltersForSingleRouteId(string? routeId, string fieldName, int expectedNumberOfFilters)
        {
            const string parameterName = "routeIds";
            var request = new GetSearchResultsRequest { RouteIds = [] };
            var categoriesLookups = new List<ChecklistLookup>();

            var jobCategoryFilters = new SearchResultsViewModel
            {
                SelectedRouteIds = []
            };


            if (routeId != null)
            {
                var lookup = new ChecklistLookup(parameterName, routeId, null,true);

                categoriesLookups.Add(lookup);

                jobCategoryFilters.SelectedRouteIds.Add(routeId);
                request.RouteIds.Add(routeId);
            }

            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns(SearchResultsUrl);

            var actual = FilterBuilder.Build(request, mockUrlHelper.Object, new SearchApprenticeshipFilterChoices
                { JobCategoryChecklistDetails = new ChecklistDetails { Lookups = categoriesLookups } });

            if (expectedNumberOfFilters == 0)
            {
                actual.Count.Should().Be(0);
                return;
            }

            var firstItem = actual.First();
            firstItem.Filters.Count.Should().Be(expectedNumberOfFilters);
            firstItem.FieldName.Should().Be(fieldName);
            firstItem.FieldOrder.Should().Be(1);

            var filter = firstItem.Filters.First();
            filter.ClearFilterLink.Should().Be(SearchResultsUrl);
            filter.Order.Should().Be(1);
            filter.Value.Should().Be(parameterName);
        }

        [TestCase("1", "2", "?routeIds=2", "?routeIds=1")]
        [TestCase("1", "3", "?routeIds=3", "?routeIds=1")]
        [TestCase("2", "3", "?routeIds=3", "?routeIds=2")]
        public void BuildJobCategorySearchFiltersForTwoRouteIds(string routeIds1, string routeIds2, string expectedFirst, string expectedSecond)
        {
            const string parameterName = "routeIds";
            var request = new GetSearchResultsRequest { RouteIds = [] };
            var categoriesLookups = new List<ChecklistLookup>();

            var jobCategoryFilters = new SearchResultsViewModel
            {
                SelectedRouteIds = []
            };

            var lookup = new ChecklistLookup(parameterName, routeIds1)
            {
                Checked = "Checked"
            };

            categoriesLookups.Add(lookup);

            request.RouteIds.Add(routeIds1);
            request.RouteIds.Add(routeIds2);
            jobCategoryFilters.SelectedRouteIds.Add(routeIds1);
            jobCategoryFilters.SelectedRouteIds.Add(routeIds2);

            categoriesLookups.Add(new ChecklistLookup(parameterName, routeIds2));

            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns(SearchResultsUrl);

            var actual = FilterBuilder.Build(request, mockUrlHelper.Object, new SearchApprenticeshipFilterChoices
                { JobCategoryChecklistDetails = new ChecklistDetails { Lookups = categoriesLookups } });

            var firstItem = actual.First();
            firstItem.Filters.Count.Should().Be(2);
            firstItem.FieldName.Should().Be("Job Category");
            firstItem.FieldOrder.Should().Be(1);

            var filter = firstItem.Filters.First();
            filter.ClearFilterLink.Should().Be(SearchResultsUrl + expectedFirst);
            filter.Order.Should().Be(1);
            filter.Value.Should().Be(parameterName);

            var filterSecond = firstItem.Filters.Skip(1).First();
            filterSecond.ClearFilterLink.Should().Be(SearchResultsUrl + expectedSecond);
            filterSecond.Order.Should().Be(2);
            filterSecond.Value.Should().Be(parameterName);
        }

        [TestCase("?routeIds=2&routeIds=3", "?routeIds=1&routeIds=3", "?routeIds=1&routeIds=2", 3, "checked")]
        [TestCase("?routeIds=3", "?routeIds=2", "", 2, "")]
        public void BuildJobCategorySearchFiltersForThreeRouteIdsCheckedAndUnchecked(
           string expectedFirst, string expectedSecond, string expectedThird, int expectedNumberOfFilters, string routeId1Checked)
        {
            const string parameterName = "routeIds";
            var request = new GetSearchResultsRequest { RouteIds = [] };
            var categoriesLookups = new List<ChecklistLookup>();

            var jobCategoryFilters = new SearchResultsViewModel
            {
                SelectedRouteIds = []
            };

            var lookup = new ChecklistLookup(parameterName, 1.ToString())
            {
                Checked = routeId1Checked
            };

            categoriesLookups.Add(lookup);

            jobCategoryFilters.SelectedRouteIds.Add("1");

            if (routeId1Checked == "checked")
            {
                request.RouteIds.Add("1");
            }

            categoriesLookups.Add(new ChecklistLookup(parameterName, 2.ToString()));
            jobCategoryFilters.SelectedRouteIds.Add("2");
            request.RouteIds.Add("2");

            categoriesLookups.Add(new ChecklistLookup(parameterName, 3.ToString()));
            jobCategoryFilters.SelectedRouteIds.Add("3");
            request.RouteIds.Add("3");

            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns(SearchResultsUrl);

            var actual = FilterBuilder.Build(request, mockUrlHelper.Object,
                new SearchApprenticeshipFilterChoices
                    { JobCategoryChecklistDetails = new ChecklistDetails { Lookups = categoriesLookups } });

            var firstItem = actual.First();
            firstItem.Filters.Count.Should().Be(expectedNumberOfFilters);
            firstItem.FieldName.Should().Be("Job Category");
            firstItem.FieldOrder.Should().Be(1);

            var filter = firstItem.Filters.First();
            filter.ClearFilterLink.Should().Be(SearchResultsUrl + expectedFirst);
            filter.Order.Should().Be(1);
            filter.Value.Should().Be(parameterName);

            var filterSecond = firstItem.Filters.Skip(1).First();
            filterSecond.ClearFilterLink.Should().Be(SearchResultsUrl + expectedSecond);
            filterSecond.Order.Should().Be(2);
            filterSecond.Value.Should().Be(parameterName);

            if (firstItem.Filters.Count <= 2) return;
            var filterThird = firstItem.Filters.Skip(2).First();
            filterThird.ClearFilterLink.Should().Be(SearchResultsUrl + expectedThird);
            filterThird.Order.Should().Be(3);
            filterThird.Value.Should().Be(parameterName);
        }

        [TestCase(null, "", 0)]
        [TestCase("1", "Apprenticeship Level", 1)]
        [TestCase("2", "Apprenticeship Level", 1)]
        [TestCase("3", "Apprenticeship Level", 1)]
        public void BuildJobLevelsSearchFiltersForSingleLevelId(string? levelId, string fieldName, int expectedNumberOfFilters)
        {
            const string parameterName = "levelIds";
            var request = new GetSearchResultsRequest { LevelIds = [] };
            var levelsLookups = new List<ChecklistLookup>();
            if (levelId != null)
            {
                var lookup = new ChecklistLookup(parameterName, levelId, null, true);
                levelsLookups.Add(lookup);
                request.LevelIds.Add(levelId);
            }
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns(SearchResultsUrl);

            var actual = FilterBuilder.Build(request, mockUrlHelper.Object, new SearchApprenticeshipFilterChoices
                { CourseLevelsChecklistDetails = new ChecklistDetails { Lookups = levelsLookups } });

            if (expectedNumberOfFilters == 0)
            {
                actual.Count.Should().Be(0);
                return;
            }
            var firstItem = actual.First();
            firstItem.Filters.Count.Should().Be(expectedNumberOfFilters);
            firstItem.FieldName.Should().Be(fieldName);
            firstItem.FieldOrder.Should().Be(1);

            var filter = firstItem.Filters.First();
            filter.ClearFilterLink.Should().Be(SearchResultsUrl);
            filter.Order.Should().Be(1);
            filter.Value.Should().Be(parameterName);
        }

        [TestCase("1", "2", "?levelIds=2", "?levelIds=1")]
        [TestCase("1", "3", "?levelIds=3", "?levelIds=1")]
        [TestCase("2", "3", "?levelIds=3", "?levelIds=2")]
        public void BuildJobLevelSearchFiltersForTwoLevelIds(string levelIds1, string levelIds2, string expectedFirst, string expectedSecond)
        {
            const string parameterName = "levelIds";
            var request = new GetSearchResultsRequest { LevelIds = [] };
            var categoriesLookups = new List<ChecklistLookup>
            {
                new(parameterName, levelIds1)
                {
                    Checked = "Checked"
                },
                new(parameterName, levelIds2)
                {
                    Checked = "Checked"
                }
            };

            request.LevelIds.Add(levelIds1);
            request.LevelIds.Add(levelIds2);


            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns(SearchResultsUrl);

            var actual = FilterBuilder.Build(request, mockUrlHelper.Object, new SearchApprenticeshipFilterChoices
                { CourseLevelsChecklistDetails = new ChecklistDetails { Lookups = categoriesLookups } });

            var firstItem = actual.First();
            firstItem.Filters.Count.Should().Be(2);
            firstItem.FieldName.Should().Be("Apprenticeship Level");
            firstItem.FieldOrder.Should().Be(1);

            var filter = firstItem.Filters.First();
            filter.ClearFilterLink.Should().Be(SearchResultsUrl + expectedFirst);
            filter.Order.Should().Be(1);
            filter.Value.Should().Be(parameterName);

            var filterSecond = firstItem.Filters.Skip(1).First();
            filterSecond.ClearFilterLink.Should().Be(SearchResultsUrl + expectedSecond);
            filterSecond.Order.Should().Be(2);
            filterSecond.Value.Should().Be(parameterName);
        }

        [TestCase("?levelIds=2&levelIds=3", "?levelIds=1&levelIds=3", "?levelIds=1&levelIds=2", 3, "checked")]
        [TestCase("?levelIds=3", "?levelIds=2", "", 2, "")]
        public void BuildJobLevelSearchFiltersForThreeLevelIdsCheckedAndUnchecked(
           string expectedFirst, string expectedSecond, string expectedThird, int expectedNumberOfFilters, string levelId1Checked)
        {
            const string parameterName = "levelIds";
            var request = new GetSearchResultsRequest { LevelIds = [] };
            var categoriesLookups = new List<ChecklistLookup>
            {
                new (parameterName, 1.ToString())
                {
                    Checked = levelId1Checked
                }
            };

            if (levelId1Checked == "checked")
            {
                request.LevelIds.Add("1");
            }

            categoriesLookups.Add(new ChecklistLookup(parameterName, 2.ToString()));
            request.LevelIds.Add("2");

            categoriesLookups.Add(new ChecklistLookup(parameterName, 3.ToString()));
            request.LevelIds.Add("3");

            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns(SearchResultsUrl);

            var actual = FilterBuilder.Build(request, mockUrlHelper.Object,
                new SearchApprenticeshipFilterChoices
                { CourseLevelsChecklistDetails = new ChecklistDetails { Lookups = categoriesLookups } });

            var firstItem = actual.First();
            firstItem.Filters.Count.Should().Be(expectedNumberOfFilters);
            firstItem.FieldName.Should().Be("Apprenticeship Level");
            firstItem.FieldOrder.Should().Be(1);

            var filter = firstItem.Filters.First();
            filter.ClearFilterLink.Should().Be(SearchResultsUrl + expectedFirst);
            filter.Order.Should().Be(1);
            filter.Value.Should().Be(parameterName);

            var filterSecond = firstItem.Filters.Skip(1).First();
            filterSecond.ClearFilterLink.Should().Be(SearchResultsUrl + expectedSecond);
            filterSecond.Order.Should().Be(2);
            filterSecond.Value.Should().Be(parameterName);

            if (firstItem.Filters.Count <= 2) return;
            var filterThird = firstItem.Filters.Skip(2).First();
            filterThird.ClearFilterLink.Should().Be(SearchResultsUrl + expectedThird);
            filterThird.Order.Should().Be(3);
            filterThird.Value.Should().Be(parameterName);
        }

        [Test]
        public void Then_What_Search_Term_Is_Added_To_Filter_List()
        {
            var request = new GetSearchResultsRequest { RouteIds = [], SearchTerm = "Software Developer"};
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns(SearchResultsUrl);
            
            var actual = FilterBuilder.Build(request, mockUrlHelper.Object,
                new SearchApprenticeshipFilterChoices
                    { JobCategoryChecklistDetails = new ChecklistDetails { Lookups = new List<ChecklistLookup>() } });

            actual.First().FieldName.Should().Be("What");
            actual.First().Filters.First().Value.Should().Be("Software Developer");
            actual.First().Filters.First().ClearFilterLink.Should().Be("searchResults");

        }
        
        [Test]
        public void Then_What_Search_Term_Is_Added_To_Filter_List_With_Sort()
        {
            var request = new GetSearchResultsRequest { RouteIds = [], SearchTerm = "Software Developer", Sort = VacancySort.AgeAsc.ToString()};
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns(SearchResultsUrl);
            
            var actual = FilterBuilder.Build(request, mockUrlHelper.Object,
                new SearchApprenticeshipFilterChoices
                    { JobCategoryChecklistDetails = new ChecklistDetails { Lookups = new List<ChecklistLookup>() } });

            actual.First().FieldName.Should().Be("What");
            actual.First().Filters.First().Value.Should().Be("Software Developer");
            actual.First().Filters.First().ClearFilterLink.Should().Be("searchResults?sort=AgeAsc");

        }

        [TestCase(null,"Coventry (Across England)")]
        [TestCase(10,"Coventry (within 10 miles)")]
        public void Then_Where_With_Distance_Search_Term_Is_Added_To_Filter_List(int? distance, string expected)
        {
            var request = new GetSearchResultsRequest { RouteIds = [], Location = "Coventry", Distance = distance};
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns(SearchResultsUrl);
            
            var actual = FilterBuilder.Build(request, mockUrlHelper.Object,
                new SearchApprenticeshipFilterChoices
                    { JobCategoryChecklistDetails = new ChecklistDetails { Lookups = new List<ChecklistLookup>() } });

            actual.First().FieldName.Should().Be("Where");
            actual.First().Filters.First().Value.Should().Be(expected);
            actual.First().Filters.First().ClearFilterLink.Should().Be("searchResults");
        }

        [TestCase(true, "DisabilityConfident", "True")]
        [TestCase(false, "DisabilityConfident", null)]
        public void Then_DisabilityConfident_Filter_Is_Added_To_Filter_List(bool isDisabilityConfident, string expectedFieldName, string expectedFilterValue)
        {
            // Arrange
            var request = new GetSearchResultsRequest { DisabilityConfident = isDisabilityConfident };
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns(SearchResultsUrl);

            // Act
            var actual = FilterBuilder.Build(request, mockUrlHelper.Object,
                new SearchApprenticeshipFilterChoices
                {
                    JobCategoryChecklistDetails = new ChecklistDetails { Lookups = new List<ChecklistLookup>() }
                });

            // Assert
            if (isDisabilityConfident)
            {
                actual.Should().ContainSingle();
                var disabilityConfidentFilter = actual.Single();

                disabilityConfidentFilter.FieldName.Should().Be(expectedFieldName);
                actual.Should().ContainSingle();

                disabilityConfidentFilter.FieldName.Should().Be(expectedFieldName);
                disabilityConfidentFilter.Filters.Should().HaveCount(1);
                var filter = disabilityConfidentFilter.Filters.Single();
                filter.Value.Should().Be(expectedFilterValue);
                filter.ClearFilterLink.Should().Be("searchResults");
            }
            else
            {
                actual.Should().BeEmpty();
            }
        }

    }
}
