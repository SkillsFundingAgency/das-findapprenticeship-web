using MediatR;
using SFA.DAS.FAA.Domain.Apply.PostAdditionalQuestion;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.AdditionalQuestion.AddAdditionalQuestion;

public record AddAdditionalQuestionCommandHandler(IApiClient ApiClient)
    : IRequestHandler<AddAdditionalQuestionCommand, AddAdditionalQuestionCommandHandlerResult>
{
    public async Task<AddAdditionalQuestionCommandHandlerResult> Handle(AddAdditionalQuestionCommand request, CancellationToken cancellationToken)
    {
        var data = new PostAdditionalQuestionApiRequest.PostAdditionalQuestionApiRequestData
        {
            CandidateId = request.CandidateId,
            Answer = request.Answer,
            Id = request.Id
        };

        var apiRequest = new PostAdditionalQuestionApiRequest(request.ApplicationId, data);

        var apiResponse = await ApiClient.PostWithResponseCode<PostAdditionalQuestionApiResponse>(apiRequest);

        if (apiResponse != null)
            return new AddAdditionalQuestionCommandHandlerResult
            {
                Id = apiResponse.Id
            };

        return new AddAdditionalQuestionCommandHandlerResult();
    }
}