using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries;
using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.UnitTests.Models;

public class WhenCreatingSearchApprenticeshipsViewModel
{
    [Test]
    [TestCase(1, "1 apprenticeship currently listed")]
    [TestCase(2, "2 apprenticeships currently listed")]
    [TestCase(2034, "2,034 apprenticeships currently listed")]
    public void Then_The_Text_Is_Shown_Correctly_For_Number_Of_Vacancies(int numberOfVacancies, string expectedText)
    {
        var source = new GetSearchApprenticeshipsIndexResult
        {
            Total = numberOfVacancies
        };
        
        var actual = (SearchApprenticeshipsViewModel)source;

        Assert.AreEqual(expectedText, actual.TotalText);

    }

}