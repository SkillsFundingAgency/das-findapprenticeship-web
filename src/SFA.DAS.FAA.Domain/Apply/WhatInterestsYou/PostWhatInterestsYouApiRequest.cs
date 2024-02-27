using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.WhatInterestsYou
{
    public class PostWhatInterestsYouApiRequest(Guid applicationId, PostWhatInterestsYouApiRequest.PostWhatInterestsYouRequestData body) : IPostApiRequest
    {
        public string PostUrl => $"applications/{applicationId}/what-interests-you";
        public object Data { get; set; } = body;

        public class PostWhatInterestsYouRequestData
        {
            public Guid CandidateId { get; set; }
            public string? YourInterest { get; set; }
        }
    }
}
