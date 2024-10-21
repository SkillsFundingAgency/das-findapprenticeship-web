using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.Queries.User.GetSignIntoYourOldAccount
{
    public class GetSignIntoYourOldAccountQuery : IRequest<GetSignIntoYourOldAccountQueryResult>
    {
        public Guid CandidateId { get; set; }
        public  required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class GetSignIntoYourOldAccountQueryResult
    {
        public bool IsValid { get; set; }
    }

    public class GetSignIntoYourOldAccountQueryHandler(IApiClient apiClient) : IRequestHandler<GetSignIntoYourOldAccountQuery, GetSignIntoYourOldAccountQueryResult>
    {
        public async Task<GetSignIntoYourOldAccountQueryResult> Handle(GetSignIntoYourOldAccountQuery request, CancellationToken cancellationToken)
        {
            var apiRequest = new PostSignIntoYourOldAccountApiRequest(new PostSignIntoYourOldAccountApiRequestData
            {
                CandidateId = request.CandidateId,
                Email = request.Email,
                Password = request.Password
            });
            var result = await apiClient.PostWithResponseCode<PostSignIntoYourOldAccountApiResponse>(apiRequest);

            if (result is null)
                return new GetSignIntoYourOldAccountQueryResult
                {
                    IsValid = false
                };

            return new GetSignIntoYourOldAccountQueryResult
            {
                IsValid = result.IsValid
            };
        }
    }
}
