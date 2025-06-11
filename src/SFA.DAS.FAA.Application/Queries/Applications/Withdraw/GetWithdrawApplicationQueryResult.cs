using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.Queries.Applications.Withdraw;

public class GetWithdrawApplicationQueryResult
{
    public Guid ApplicationId { get; set; }

    public string? AdvertTitle { get; set; }

    public string? EmployerName { get; set; }

    public DateTime? SubmittedDate { get; set; }

    public DateTime ClosingDate { get; set; }
    
    public DateTime? ClosedDate { get; set; }
    public Address WorkLocation { get; set; } = null!;
    public List<Address> Addresses { get; set; } = [];
    public string? EmploymentLocationInformation { get; set; }
    public AvailableWhere? EmployerLocationOption { get; set; }
    public ApprenticeshipTypes ApprenticeshipType { get; set; } = ApprenticeshipTypes.Standard;
}