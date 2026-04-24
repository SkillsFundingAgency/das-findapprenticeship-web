using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Web.Models.SearchResults;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.UnitTests.Models;

public class WhenCreatingSearchApprenticeshipsViewModel
{
    [Test]
    [TestCase(1, "1 apprenticeship listed")]
    [TestCase(2, "2 apprenticeships listed")]
    [TestCase(2034, "2,034 apprenticeships listed")]
    public void Then_The_Text_Is_Shown_Correctly_For_Number_Of_Vacancies(int numberOfVacancies, string expectedText)
    {
        var source = new GetSearchApprenticeshipsIndexResult
        {
            Total = numberOfVacancies
        };
        
        var actual = (SearchApprenticeshipsViewModel)source;

        
        Assert.That(actual.TotalText, Is.EqualTo(expectedText));

    }
}