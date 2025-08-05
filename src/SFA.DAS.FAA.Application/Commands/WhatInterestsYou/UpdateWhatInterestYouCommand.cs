using MediatR;
using SFA.DAS.FAA.Domain.Apply.WhatInterestsYou;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.WhatInterestsYou
{
    public class UpdateWhatInterestsYouCommand : IRequest
    {
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
        public string AnswerText { get; set; }
        public bool IsComplete { get; set; }
    }

    public class UpdateWhatInterestsYouCommandHandler(IApiClient apiClient) : IRequestHandler<UpdateWhatInterestsYouCommand>
    {
        public async Task Handle(UpdateWhatInterestsYouCommand request, CancellationToken cancellationToken)
        {
            var data = new PostWhatInterestsYouApiRequest.PostWhatInterestsYouRequestData
            {
                CandidateId = request.CandidateId,
                AnswerText = request.AnswerText,
                IsComplete = request.IsComplete
            };
            var apiRequest = new PostWhatInterestsYouApiRequest(request.ApplicationId, data);

            await apiClient.Post(apiRequest);
        }
    }
}
