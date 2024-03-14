using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.DisabilityConfident
{
    public class GetDisabilityConfidentApiRequest(Guid applicationId, Guid candidateId) : IGetApiRequest
    {
        public string GetUrl => $"applications/{applicationId}/disability-confident?candidateId={candidateId}";
    }

    public class GetDisabilityConfidentApiResponse
    {
        public bool? ApplyUnderDisabilityConfidentScheme { get; set; }
        public string EmployerName { get; set; } = null!;
        public bool? IsSectionCompleted { get; set; }
    }
}
