using MediatR;
using SFA.DAS.FAA.Domain.Apply.EqualityQuestions;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.EqualityQuestions
{
    public class GetEqualityQuestionsQuery : IRequest<GetEqualityQuestionsQueryResult>
    {
        public Guid CandidateId { get; set; }
    }

    public class GetEqualityQuestionsQueryResult
    {
        public EqualityQuestionsItem? EqualityQuestions { get; set; }

        public class EqualityQuestionsItem
        {
            public GenderIdentity? Sex { get; set; }
            public EthnicGroup? EthnicGroup { get; set; }
            public EthnicSubGroup? EthnicSubGroup { get; set; }
            public string? IsGenderIdentifySameSexAtBirth { get; set; }
            public string? OtherEthnicSubGroupAnswer { get; set; }
        }
    }

    public class GetEqualityQuestionsQueryHandler(IApiClient apiClient) : IRequestHandler<GetEqualityQuestionsQuery, GetEqualityQuestionsQueryResult>
    {
        public async Task<GetEqualityQuestionsQueryResult> Handle(GetEqualityQuestionsQuery request, CancellationToken cancellationToken)
        {
            var result =
                await apiClient.Get<GetEqualityQuestionsApiResponse>(
                    new GetEqualityQuestionsApiRequest(request.CandidateId));

            if (result.EqualityQuestions == null)
            {
                return new GetEqualityQuestionsQueryResult
                {
                    EqualityQuestions = null
                };
            }

            return new GetEqualityQuestionsQueryResult
            {
                EqualityQuestions = new GetEqualityQuestionsQueryResult.EqualityQuestionsItem
                {
                    EthnicGroup = result.EqualityQuestions.EthnicGroup,
                    EthnicSubGroup = result.EqualityQuestions.EthnicSubGroup,
                    IsGenderIdentifySameSexAtBirth = result.EqualityQuestions.IsGenderIdentifySameSexAtBirth,
                    OtherEthnicSubGroupAnswer = result.EqualityQuestions.OtherEthnicSubGroupAnswer,
                    Sex = result.EqualityQuestions.Sex
                }
            };
        }
    }
}
