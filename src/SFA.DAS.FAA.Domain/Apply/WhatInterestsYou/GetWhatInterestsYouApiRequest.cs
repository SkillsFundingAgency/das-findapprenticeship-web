using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.WhatInterestsYou
{
    public class GetWhatInterestsYouApiRequest(Guid ApplicationId, Guid CandidateId) : IGetApiRequest
    {
        public string GetUrl => $"applications/{ApplicationId}/what-interests-you?candidateId={CandidateId}";
    }

    public class GetWhatInterestsYouApiResponse
    {
        public string EmployerName { get; set; }
        public string StandardName { get; set; }
        public bool? IsSectionCompleted { get; set; }
    }
}
