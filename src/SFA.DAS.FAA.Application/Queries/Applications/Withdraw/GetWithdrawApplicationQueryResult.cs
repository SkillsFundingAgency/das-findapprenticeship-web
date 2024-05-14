namespace SFA.DAS.FAA.Application.Queries.Applications.Withdraw;

public class GetWithdrawApplicationQueryResult
{
    public Guid ApplicationId { get; set; }

    public string? AdvertTitle { get; set; }

    public string? EmployerName { get; set; }

    public DateTimeOffset SubmittedDate { get; set; }

    public DateTimeOffset ClosingDate { get; set; }
}