using MediatR;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.UpdateApplication.AdditionalQuestion;

public class UpdateAdditionalQuestionApplicationCommandHandler(IApiClient apiClient)
    : IRequestHandler<UpdateAdditionalQuestionApplicationCommand, UpdateAdditionalQuestionApplicationCommandResult>
{
    public async Task<UpdateAdditionalQuestionApplicationCommandResult> Handle(UpdateAdditionalQuestionApplicationCommand request, CancellationToken cancellationToken)
    {
        var postRequest = new UpdateAdditionalQuestionApplicationApiRequest(
            request.ApplicationId,
            request.CandidateId,
            new UpdateAdditionalQuestionApplicationModel
            {
                AdditionalQuestionOne = request.AdditionQuestionOne,
                AdditionalQuestionTwo = request.AdditionQuestionTwo,
            });

        var response = await apiClient.PostWithResponseCode<Domain.Apply.UpdateApplication.Application>(postRequest);

        return new UpdateAdditionalQuestionApplicationCommandResult
        {
            Application = response
        };
    }
}