using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Applications.WithdrawApplication;

public class GetWithdrawApplicationApiRequest(Guid applicationId, Guid candidateId) : IGetApiRequest
{
    public string GetUrl => $"applications/{applicationId}/withdraw?candidateId={candidateId}";
}

public class GetWithdrawApplicationApiResponse
{
    public Guid ApplicationId { get; set; }

    public string? AdvertTitle { get; set; }

    public string? EmployerName { get; set; }

    public DateTime? SubmittedDate { get; set; }

    public DateTime ClosingDate { get; set; }
    
    public DateTime? ClosedDate { get; set; }
}