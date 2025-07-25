using MediatR;
using SFA.DAS.FAA.Domain.Apply.EqualityQuestions;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.EqualityQuestions
{
    public class CreateEqualityQuestionsCommandHandler(IApiClient apiClient) : IRequestHandler<CreateEqualityQuestionsCommand, CreateEqualityQuestionsCommandResult>
    {
        public async Task<CreateEqualityQuestionsCommandResult> Handle(CreateEqualityQuestionsCommand request, CancellationToken cancellationToken)
        {
            var postUpdateApplicationRequest = new PostEqualityQuestionsApiRequest(
                request.CandidateId,
                new UpdateEqualityQuestionsModel
                {
                    Sex = request.Sex,
                    EthnicGroup = request.EthnicGroup,
                    EthnicSubGroup = request.EthnicSubGroup,
                    IsGenderIdentifySameSexAtBirth = request.IsGenderIdentifySameSexAtBirth,
                    OtherEthnicSubGroupAnswer = request.OtherEthnicSubGroupAnswer
                });

            var response = await apiClient.Post<PostEqualityQuestionsApiResponse>(postUpdateApplicationRequest);

            return new CreateEqualityQuestionsCommandResult
            {
                Id = response!.Id
            };
        }
    }
}
