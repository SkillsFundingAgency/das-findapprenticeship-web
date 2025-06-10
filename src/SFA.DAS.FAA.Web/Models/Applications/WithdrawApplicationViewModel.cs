using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Applications.Withdraw;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Extensions;
using SFA.DAS.FAA.Domain.Models;
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

public class WithdrawApplicationViewModel(IDateTimeService dateTimeService, GetWithdrawApplicationQueryResult source)
{
    public Guid ApplicationId { get; set; } = source.ApplicationId;
    public bool? WithdrawApplication { get; set; }
    public string? AdvertTitle { get; set; } = source.AdvertTitle;
    public string? EmployerName { get; set; } = source.EmployerName;
    public string? SubmittedDate { get; set; } = source.SubmittedDate?.ToGdsDateString();
    public string? ClosesOnDate { get; set; } = VacancyDetailsHelperService.GetClosingDate(dateTimeService, source.ClosedDate ?? source.ClosingDate);
    public bool ClosesToday { get; set; } = VacancyDetailsHelperService.GetDaysUntilExpiry(dateTimeService, source.ClosingDate) == 0;
    public bool ClosesTomorrow { get; set; } = VacancyDetailsHelperService.GetDaysUntilExpiry(dateTimeService, source.ClosingDate) == 1;
    public bool ShowFoundationTag => source.ApprenticeshipType == ApprenticeshipTypes.Foundation;

    private List<Address> Addresses { get; } = source.Addresses;
    private AvailableWhere? EmployerLocationOption { get; } = source.EmployerLocationOption;

    public string? EmploymentWorkLocation => EmployerLocationOption switch
    {
        AvailableWhere.MultipleLocations => VacancyDetailsHelperService.GetEmploymentLocationCityNames(Addresses),
        AvailableWhere.AcrossEngland => "Recruiting nationally",
        _ => VacancyDetailsHelperService.GetOneLocationCityName(source.WorkLocation)
    };
}
