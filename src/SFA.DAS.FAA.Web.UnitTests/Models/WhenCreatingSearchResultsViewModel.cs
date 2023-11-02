using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.GetSearchResults;
using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.UnitTests.Models;

public class WhenCreatingSearchResultsViewModel
{

    [Test]
    [TestCase(0, "0 apprenticeships found")]
    [TestCase(1, "1 apprenticeship found")]
    [TestCase(2, "2 apprenticeships found")]
    [TestCase(2034, "2,034 apprenticeships found")]
    public void Then_The_Text_Is_Shown_Correctly_For_Number_Of_Vacancies(int numberOfVacancies, string expectedText)
    {
        var source = new GetSearchResultsResult()
        {
            Total = numberOfVacancies
        };

        var actual = (SearchResultsViewModel)source;

        Assert.AreEqual(expectedText, actual.TotalMessage);

    }
}