using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.DisabilityConfident
{
    public class GetDisabilityConfidentApiRequest(Guid ApplicationId, Guid CandidateId) : IGetApiRequest
    {
        public string GetUrl => $"applications/{ApplicationId}/disability-confident?candidateId={CandidateId}";
    }

    public class GetDisabilityConfidentApiResponse
    {
        public bool? ApplyUnderDisabilityConfidentScheme { get; set; }
        public string EmployerName { get; set; } = null!;
    }
}
