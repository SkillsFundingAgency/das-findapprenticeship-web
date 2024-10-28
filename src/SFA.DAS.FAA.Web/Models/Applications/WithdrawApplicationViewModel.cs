using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Applications.Withdraw;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.Models.Applications;

public class PostWithdrawApplicationViewModel
{
    [FromRoute]
    public Guid ApplicationId { get; set; }
    [Required(ErrorMessage = "Select if you want to withdraw this application")]
    public bool? WithdrawApplication { get; set; }
    public string? AdvertTitle { get; set; }
    public string? EmployerName { get; set; }
}

public class WithdrawApplicationViewModel
{
    public WithdrawApplicationViewModel(IDateTimeService dateTimeService, GetWithdrawApplicationQueryResult source)
    {
        ApplicationId = source.ApplicationId;
        AdvertTitle = source.AdvertTitle;
        EmployerName = source.EmployerName;
        ClosesOnDate = VacancyDetailsHelperService.GetClosingDate(dateTimeService, source.ClosedDate ?? source.ClosingDate);
        SubmittedDate = source.SubmittedDate!.Value.GetStartDate();
        ClosesToday = VacancyDetailsHelperService.GetDaysUntilExpiry(dateTimeService, source.ClosingDate) == 0;
        ClosesTomorrow = VacancyDetailsHelperService.GetDaysUntilExpiry(dateTimeService, source.ClosingDate) == 1;
    }
    public Guid ApplicationId { get; set; }
    
    public bool? WithdrawApplication { get; set; }

    public string? AdvertTitle { get; set; }
    public string? EmployerName { get; set; }
    public string? SubmittedDate { get; set; }
    public string? ClosesOnDate { get; set; }
    public bool ClosesToday { get; set; }
    public bool ClosesTomorrow { get; set; }
}