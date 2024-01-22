using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.SearchResults;

namespace SFA.DAS.FAA.Web.UnitTests.Models
{
    public class PaginationViewModelTests

    {
        public const string BaseUrl = @"http://baseUrl";

        [TestCase("https://baseUrl.com", "https://baseUrl.com?pageNumber=2")]
        [TestCase("https://baseUrl.com?x=1", "https://baseUrl.com?x=1&pageNumber=2")]
        public void CorrectlyAppendsToTheBaseUrl(string baseUrl, string expectedUrl)
        {
            PaginationViewModel sut = new(1, 2, baseUrl);
            sut.LinkItems.Last().Url.Should().Be(expectedUrl);
        }

        [Test]
        public void ReturnsNoLinksWhenNoTotalPages()
        {
            PaginationViewModel sut = new(1, 0, BaseUrl);
            sut.LinkItems.Count.Should().Be(0);
        }

        [TestCase(1, 2, 2, 1, 2, false, false)]
        [TestCase(1, 6, 6, 1, 6, false, false)]
        [TestCase(2, 6, 6, 1, 6, false, false)]
        [TestCase(3, 6, 4, 1, 4, false, false)]
        public void PopulatesLinkItem(int currentPage, int totalPages, int totalLinkItems, int firstPageExpected, int lastPageExpected, bool isPreviousExpected, bool isNextExpected)
        {
            var pageSize = 10;
            PaginationViewModel sut = new(currentPage, totalPages, BaseUrl);

            sut.LinkItems.Count.Should().Be(totalLinkItems);

            sut.LinkItems.First(s => s.Text == currentPage.ToString()).HasLink.Should().BeFalse();
            sut.LinkItems.Where(s => s.Text != currentPage.ToString() && s.IsEllipsesLink == false).All(s => s.HasLink).Should().BeTrue();
            if (!isPreviousExpected)
            {
                sut.LinkItems.First().Text.Should().Be(firstPageExpected.ToString());
            }
            else
            {
                sut.LinkItems.Skip(1).First().Text.Should().Be(firstPageExpected.ToString());
            }

            if (!isNextExpected)
            {
                sut.LinkItems.Last().Text.Should().Be(lastPageExpected.ToString());
            }
            else
            {
                sut.LinkItems[^2].Text.Should().Be(lastPageExpected.ToString());
            }

            if (isPreviousExpected)
            {
                sut.LinkItems.First(s => s.Text == "Previous").Url.Should().Be(BaseUrl + "?pageNumber=" + (currentPage - 1));
            }
            else
            {
                sut.LinkItems.Exists(s => s.Text == "Previous").Should().BeFalse();
            }

            if (isNextExpected)
            {
                sut.LinkItems.First(s => s.Text == "Next").Url.Should().Be(BaseUrl + "?pageNumber=" + (currentPage + 1) );
            }
            else
            {
                sut.LinkItems.Exists(s => s.Text == "Next").Should().BeFalse();
            }
        }
    }
}
