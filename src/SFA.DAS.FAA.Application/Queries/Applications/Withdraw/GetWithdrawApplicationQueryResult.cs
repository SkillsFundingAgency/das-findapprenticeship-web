namespace SFA.DAS.FAA.Application.Queries.Applications.Withdraw;

public class GetWithdrawApplicationQueryResult
{
    public Guid ApplicationId { get; set; }

    public string? AdvertTitle { get; set; }

    public string? EmployerName { get; set; }

    public DateTime? SubmittedDate { get; set; }

    public DateTime ClosingDate { get; set; }
    
    public DateTime? ClosedDate { get; set; }
}