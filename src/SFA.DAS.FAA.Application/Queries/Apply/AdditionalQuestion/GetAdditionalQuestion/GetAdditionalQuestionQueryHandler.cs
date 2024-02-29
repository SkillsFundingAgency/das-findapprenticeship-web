using MediatR;
using SFA.DAS.FAA.Domain.Apply.GetAdditionalQuestion;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.AdditionalQuestion.GetAdditionalQuestion;

public record GetAdditionalQuestionQueryHandler(IApiClient ApiClient)
    : IRequestHandler<GetAdditionalQuestionQuery, GetAdditionalQuestionQueryResult>
{
    public async Task<GetAdditionalQuestionQueryResult> Handle(GetAdditionalQuestionQuery query, CancellationToken cancellationToken)
    {
        var response = await ApiClient.Get<GetAdditionalQuestionApiResponse>(new GetAdditionalQuestionApiRequest(query.ApplicationId, query.CandidateId, query.AdditionQuestionId));
        return response;
    }
}