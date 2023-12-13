using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.SearchResults;
using SFA.DAS.FAA.Web.Services;

namespace SFA.DAS.FAA.Web.UnitTests.Services
{
    [TestFixture]
    public class FilterBuilderTests
    {
        private const string searchResultsUrl = "searchResults";

        [Test]
        public void BuildFilterChoicesForNoFilters()
        {
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns(searchResultsUrl);

            var request = new GetSearchResultsRequest
            {
                SearchTerm = null,
                PageSize = null,
                PageNumber = null
            };

            var actual = FilterBuilder.BuildFullQueryString(request, mockUrlHelper.Object);
            actual.Should().NotBeNull();
        }

    }
}
