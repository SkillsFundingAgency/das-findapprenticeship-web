using SFA.DAS.FAA.Application.Queries.Applications.Withdraw;
using SFA.DAS.FAA.Web.Models.Applications;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Applications;

[TestFixture]
public class WhenCreatingWithdrawApplicationViewModel
{
    [Test, MoqAutoData]
    public void If_The_Vacancy_Is_Closed_Then_The_View_Model_Uses_The_Vacancy_Closed_Date(
        IDateTimeService dateTimeService,
        GetWithdrawApplicationQueryResult queryResult)
    {
        // arrange
        var expected = VacancyDetailsHelperService.GetClosingDate(dateTimeService, queryResult.ClosedDate!.Value);
        
        // act
        var actual = new WithdrawApplicationViewModel(dateTimeService, queryResult);

        // assert
        actual.ClosesOnDate.Should().Be(expected);
    }
    
    [Test, MoqAutoData]
    public void If_The_Vacancy_Is_Open_Then_The_View_Model_Uses_The_Vacancy_Closing_Date(
        IDateTimeService dateTimeService,
        GetWithdrawApplicationQueryResult queryResult)
    {
        // arrange
        queryResult.ClosedDate = null;
        var expected = VacancyDetailsHelperService.GetClosingDate(dateTimeService, queryResult.ClosingDate);
        
        // act
        var actual = new WithdrawApplicationViewModel(dateTimeService, queryResult);

        // assert
        actual.ClosesOnDate.Should().Be(expected);
    }

}