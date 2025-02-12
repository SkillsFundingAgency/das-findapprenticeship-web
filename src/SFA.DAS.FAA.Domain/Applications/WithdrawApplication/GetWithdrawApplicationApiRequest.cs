using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;

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
    public Address Address { get; set; } = null!;
    public List<Address>? OtherAddresses { get; set; } = [];
    public string? EmploymentLocationInformation { get; set; }
    public AvailableWhere? EmploymentLocationOption { get; set; }
}