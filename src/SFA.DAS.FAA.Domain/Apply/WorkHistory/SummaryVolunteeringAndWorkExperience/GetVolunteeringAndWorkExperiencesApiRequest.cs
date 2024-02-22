using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.WorkHistory.SummaryVolunteeringAndWorkExperience
{
    public record GetVolunteeringAndWorkExperiencesApiRequest(Guid ApplicationId, Guid CandidateId) : IGetApiRequest
    {
        public string GetUrl => $"applications/{ApplicationId}/volunteeringorworkexperience?candidateId={CandidateId}";
    }
}
