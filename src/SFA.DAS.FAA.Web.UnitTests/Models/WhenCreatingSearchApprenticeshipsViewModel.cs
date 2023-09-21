using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Vacancies.Queries;
using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.UnitTests.Models;

public class WhenCreatingSearchApprenticeshipsViewModel
{   
    [Test, AutoData]
    public void Then_The_Model_Is_Mapped_Correctly()
    {
        //Act
        var source = new GetSearchApprenticeshipsIndexResult();

        var actual = (SearchApprenticeshipsViewModel)source;

        //Assert
        Assert.AreEqual(source.Total, actual.Total);

    }

    [Test]
    [TestCase(1, "1 apprenticeship")]
    [TestCase(2, "2 apprenticeships")]
    [TestCase(2034, "2,034 apprenticeships")]
    public void Then_The_Text_Is_Shown_Correctly_For_Number_Of_Vacancies(int numberOfVacanices, string expectedText)
    {
        var source = new GetSearchApprenticeshipsIndexResult();
        source.Total = numberOfVacanices;
        var actual = (SearchApprenticeshipsViewModel)source;

        
        Assert.AreEqual(expectedText, actual.TotalText);

    }

}