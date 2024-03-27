using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.DisabilityConfident
{
    public record GetDisabilityConfidentDetailsApiRequest(Guid ApplicationId, Guid CandidateId) : IGetApiRequest
    {
        public string GetUrl => $"applications/{ApplicationId}/disability-confident/details?candidateId={CandidateId}";
    }

    public class GetDisabilityConfidentDetailsApiResponse
    {
        public bool? ApplyUnderDisabilityConfidentScheme { get; set; }
        public bool? IsSectionCompleted { get; set; }
    }
}
